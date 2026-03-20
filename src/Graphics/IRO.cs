/* IRO - A simple cross-platform image loader library
 *
 * Pure .NET port no native dependencies.
 * Uses SixLabors.ImageSharp for PNG/QOI decode/encode and
 * System.IO.Compression.ZLibStream for compress/decompress.
 *
 * Copyright (c) 2026 ryancheung
 *
 */

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;

namespace StarWorks;

/// <summary>
/// IRO — a simple cross-platform image loader library (pure .NET port).
/// Supports PNG and QOI image formats.
///
/// Memory management notes: In this .NET port, IRO_FreeImage and IRO_FreeBuffer
/// are not needed. All returned arrays are ordinary managed objects that the
/// garbage collector reclaims automatically.
/// </summary>
public static class IRO
{
    // -------------------------------------------------------------------------
    // Image Read API
    // -------------------------------------------------------------------------

    /// <summary>
    /// Decodes PNG or QOI image data into raw RGBA8 pixel data.
    /// Fully transparent pixels (alpha == 0) have their RGB channels zeroed to
    /// match XNA/FNA <c>Texture2D.FromStream</c> behaviour.
    /// </summary>
    /// <param name="buffer">Encoded image bytes (PNG or QOI).</param>
    /// <param name="width">Filled with the width of the image in pixels.</param>
    /// <param name="height">Filled with the height of the image in pixels.</param>
    /// <param name="len">Filled with the total byte length of the pixel array (w * h * 4).</param>
    /// <returns>
    /// A <c>byte[]</c> of raw RGBA8 pixel data laid out row-by-row, or
    /// <c>null</c> on failure.
    /// </returns>
    public static unsafe nint LoadImage(
        ReadOnlySpan<byte> buffer,
        out int width,
        out int height,
        out int len)
    {
        width = 0;
        height = 0;
        len = 0;

        if (buffer.Length == 0)
            return default;

        try
        {
            using var image = Image.Load<Rgba32>(buffer);

            width  = image.Width;
            height = image.Height;
            len = width * height * 4;

            var pixelArray = NativeMemory.Alloc((nuint)len);

            image.ProcessPixelRows(accessor =>
            {
                Rgba32 transparent = Color.Transparent;

                for (int y = 0; y < accessor.Height; y++)
                {
                    Span<Rgba32> pixelRow = accessor.GetRowSpan(y);

                    // pixelRow.Length has the same value as accessor.Width,
                    // but using pixelRow.Length allows the JIT to optimize away bounds checks:
                    for (int x = 0; x < pixelRow.Length; x++)
                    {
                        // Get a reference to the pixel at position x
                        ref Rgba32 pixel = ref pixelRow[x];

                        /* Ensure that the alpha pixels are... well, actual alpha.
                        * You think this looks stupid, but be assured: Your paint program is
                        * almost certainly even stupider.
                        * -flibit
                        */
                        if (pixel.A == 0)
                        {
                            // Overwrite the pixel referenced by 'ref Rgba32 pixel':
                            pixel = transparent;
                        }
                    }
                }
            });

            image.CopyPixelDataTo(new Span<byte>(pixelArray, len));
            image.Dispose();

            return (nint)pixelArray;
        }
        catch
        {
            return default;
        }
    }

    /// <summary>
    /// Gets image dimensions without fully decoding the image.
    /// </summary>
    /// <param name="buffer">Encoded image bytes.</param>
    /// <param name="width">Filled with the width of the image in pixels.</param>
    /// <param name="height">Filled with the height of the image in pixels.</param>
    /// <param name="len">Filled with the total byte length of the raw RGBA8 pixel data (w * h * 4).</param>
    /// <returns><c>true</c> on success, <c>false</c> on failure.</returns>
    public static bool GetImageInfo(
        ReadOnlySpan<byte> buffer,
        out int width,
        out int height,
        out int len)
    {
        width  = 0;
        height = 0;
        len    = 0;

        if (buffer.Length == 0)
            return false;

        try
        {
            var info = Image.Identify(buffer);
            if (info is null)
                return false;

            width  = info.Width;
            height = info.Height;
            len    = width * height * 4;
            return true;
        }
        catch
        {
            return false;
        }
    }

    // -------------------------------------------------------------------------
    // Image Write API
    // -------------------------------------------------------------------------

    /// <summary>
    /// Encodes raw RGBA8 pixel data as PNG.
    /// </summary>
    /// <param name="data">Raw RGBA8 pixel data (row-major, 4 bytes per pixel).</param>
    /// <param name="width">Width of the image in pixels.</param>
    /// <param name="height">Height of the image in pixels.</param>
    /// <param name="size">Filled with the byte length of the returned PNG buffer, or 0 on failure.</param>
    /// <returns>PNG-encoded bytes, or <c>null</c> on failure.</returns>
    public static unsafe nint EncodePNG(ReadOnlySpan<byte> data, int width, int height, out int size)
    {
        size = 0;
        if (data.Length == 0 || width == 0 || height == 0)
            return default;

        try
        {
            using var image = Image.LoadPixelData<Rgba32>(data, width, height);
            using var ms = new MemoryStream();
            image.SaveAsPng(ms);
            size = (int)ms.Position;

            var buffer = NativeMemory.Alloc((nuint)size);
            ms.CopyTo(new UnmanagedMemoryStream((byte*)buffer, size));
            return (nint)buffer;
        }
        catch
        {
            return default;
        }
    }

    // -------------------------------------------------------------------------
    // Compression API
    // -------------------------------------------------------------------------

    /// <summary>
    /// Compresses data using zlib (RFC 1950) encoding.
    /// </summary>
    /// <param name="data">Data to compress.</param>
    /// <param name="compressionLevel">
    /// Compression level: 1 = fastest/largest, 9 = slowest/smallest.
    /// Values 2–8 map to <see cref="CompressionLevel.Optimal"/>.
    /// </param>
    /// <param name="outLength">Filled with the byte length of the compressed buffer.</param>
    /// <returns>Compressed bytes, or <c>null</c> on failure.</returns>
    public static unsafe nint Compress(ReadOnlySpan<byte> data, int compressionLevel, out int outLength)
    {
        outLength = 0;

        if (data.Length == 0)
            return default;

        try
        {
            var level = compressionLevel <= 1 ? CompressionLevel.Fastest
                      : compressionLevel >= 9 ? CompressionLevel.SmallestSize
                      : CompressionLevel.Optimal;

            using var output = new MemoryStream();
            using var zlib = new ZLibStream(output, level, leaveOpen: true);
            zlib.Write(data);

            outLength = (int)output.Position;
            var buffer = NativeMemory.Alloc((nuint)outLength);
            output.CopyTo(new UnmanagedMemoryStream((byte*)buffer, outLength));
            return (nint)buffer;
        }
        catch
        {
            return default;
        }
    }

    /// <summary>
    /// Decompresses zlib-encoded (RFC 1950) data into a caller-supplied buffer.
    /// The buffer must be pre-allocated to the expected decompressed size.
    /// </summary>
    /// <param name="encodedBuffer">zlib-compressed input bytes.</param>
    /// <param name="decodedBuffer">A buffer to write the decoded data into.</param>
    /// Pre-allocated buffer to receive the decompressed data.
    /// Must be sized to the exact expected output length.
    /// <returns>True if decompression succeeded, false otherwise.</returns>
    public static unsafe bool Decompress(ReadOnlySpan<byte> encodedBuffer, Span<byte> decodedBuffer)
    {
        if (encodedBuffer.Length == 0)
            return false;

        try
        {
            fixed (byte* ptr = encodedBuffer)
            fixed (byte* ptrDecoded = decodedBuffer)
            {
                using var input = new UnmanagedMemoryStream(ptr, encodedBuffer.Length);
                using var zlib = new ZLibStream(input, CompressionMode.Decompress);
                zlib.CopyTo(new UnmanagedMemoryStream(ptrDecoded, decodedBuffer.Length));

                return true;
            }
        }
        catch
        {
            return false;
        }
    }

    public static unsafe void FreeBuffer(nint buffer)
    {
        NativeMemory.Free((void*)buffer);
    }
}

#region License
/* FNA - XNA4 Reimplementation for Desktop Platforms
 *
 * Copyright 2009-2024 Ethan Lee and the MonoGame Team
 *
 * Released under the Microsoft Public License.
 * See LICENSE for details.
 */
#endregion

using System;

namespace MoonWorks.Input
{
	public static class TextInputEXT
	{
		#region Events

		/// <summary>
		/// Use this event to retrieve text for objects like textboxes.
		/// This event is not raised by noncharacter keys.
		/// This event also supports key repeat.
		/// For more information this event is based off:
		/// http://msdn.microsoft.com/en-AU/library/system.windows.forms.control.keypress.aspx
		/// </summary>
		public static event Action<char> TextInput;

		public static event Action<string> ImeTextInput;

		/// <summary>
		/// This event notifies you of in-progress text composition
		/// happening in an IME or other tool and allows you to display
		/// the draft text appropriately before it has become input.
		/// For more information, see SDL's tutorial:
		/// https://wiki.libsdl.org/Tutorials-TextInput
		/// </summary>
		public static event Action<string, int, int> TextEditing;

		public static event Action<string[], int, bool> TextEditingCandidates;

		#endregion

		#region Internal Event Access Methods

		internal static void OnTextInput(char c)
		{
			if (TextInput != null)
			{
				TextInput(c);
			}
		}

		internal static void OnImeTextInput(string text)
		{
			if (ImeTextInput != null)
			{
				ImeTextInput(text);
			}
		}

		internal static void OnTextEditing(string text, int start, int length)
		{
			if (TextEditing != null)
			{
				TextEditing(text, start, length);
			}
		}

		internal static void OnTextEditingCandidates(string[] candidates, int selectedCandidate, bool horizontal)
		{
			if (TextEditingCandidates != null)
			{
				TextEditingCandidates(candidates, selectedCandidate, horizontal);
			}
		}

		#endregion
	}
}

using System;
using System.Collections.Generic;

namespace MoonWorks.Input
{
	internal static class KeysHelper
	{
		static HashSet<uint> _map;

		static KeysHelper()
		{
			_map = new HashSet<uint>();
			var allKeys = (KeyCode[]) Enum.GetValues(typeof(KeyCode));
			foreach (var key in allKeys)
			{
				_map.Add((uint) key);
			}
		}

		/// <summary>
		/// Checks if specified value is valid Key.
		/// </summary>
		/// <param name="value">KeyCode base value</param>
		/// <returns>Returns true if value is valid Key, false otherwise</returns>
		public static bool IsKey(uint value)
		{
			return _map.Contains(value);
		}

		public static bool GetKeyChar(KeyCode key, bool isShiftDown, out char keyCode)
		{
			if (key == KeyCode.A) { keyCode = isShiftDown ? 'A' : 'a'; return true; }
			if (key == KeyCode.B) { keyCode = isShiftDown ? 'B' : 'b'; return true; }
			if (key == KeyCode.C) { keyCode = isShiftDown ? 'C' : 'c'; return true; }
			if (key == KeyCode.D) { keyCode = isShiftDown ? 'D' : 'd'; return true; }
			if (key == KeyCode.E) { keyCode = isShiftDown ? 'E' : 'e'; return true; }
			if (key == KeyCode.F) { keyCode = isShiftDown ? 'F' : 'f'; return true; }
			if (key == KeyCode.G) { keyCode = isShiftDown ? 'G' : 'g'; return true; }
			if (key == KeyCode.H) { keyCode = isShiftDown ? 'H' : 'h'; return true; }
			if (key == KeyCode.I) { keyCode = isShiftDown ? 'I' : 'i'; return true; }
			if (key == KeyCode.J) { keyCode = isShiftDown ? 'J' : 'j'; return true; }
			if (key == KeyCode.K) { keyCode = isShiftDown ? 'K' : 'k'; return true; }
			if (key == KeyCode.L) { keyCode = isShiftDown ? 'L' : 'l'; return true; }
			if (key == KeyCode.M) { keyCode = isShiftDown ? 'M' : 'm'; return true; }
			if (key == KeyCode.N) { keyCode = isShiftDown ? 'N' : 'n'; return true; }
			if (key == KeyCode.O) { keyCode = isShiftDown ? 'O' : 'o'; return true; }
			if (key == KeyCode.P) { keyCode = isShiftDown ? 'P' : 'p'; return true; }
			if (key == KeyCode.Q) { keyCode = isShiftDown ? 'Q' : 'q'; return true; }
			if (key == KeyCode.R) { keyCode = isShiftDown ? 'R' : 'r'; return true; }
			if (key == KeyCode.S) { keyCode = isShiftDown ? 'S' : 's'; return true; }
			if (key == KeyCode.T) { keyCode = isShiftDown ? 'T' : 't'; return true; }
			if (key == KeyCode.U) { keyCode = isShiftDown ? 'U' : 'u'; return true; }
			if (key == KeyCode.V) { keyCode = isShiftDown ? 'V' : 'v'; return true; }
			if (key == KeyCode.W) { keyCode = isShiftDown ? 'W' : 'w'; return true; }
			if (key == KeyCode.X) { keyCode = isShiftDown ? 'X' : 'x'; return true; }
			if (key == KeyCode.Y) { keyCode = isShiftDown ? 'Y' : 'y'; return true; }
			if (key == KeyCode.Z) { keyCode = isShiftDown ? 'Z' : 'z'; return true; }

			if (((key == KeyCode.D0) && !isShiftDown) || (key == KeyCode.Keypad0)) { keyCode = '0'; return true; }
			if (((key == KeyCode.D1) && !isShiftDown) || (key == KeyCode.Keypad1)) { keyCode = '1'; return true; }
			if (((key == KeyCode.D2) && !isShiftDown) || (key == KeyCode.Keypad2)) { keyCode = '2'; return true; }
			if (((key == KeyCode.D3) && !isShiftDown) || (key == KeyCode.Keypad3)) { keyCode = '3'; return true; }
			if (((key == KeyCode.D4) && !isShiftDown) || (key == KeyCode.Keypad4)) { keyCode = '4'; return true; }
			if (((key == KeyCode.D5) && !isShiftDown) || (key == KeyCode.Keypad5)) { keyCode = '5'; return true; }
			if (((key == KeyCode.D6) && !isShiftDown) || (key == KeyCode.Keypad6)) { keyCode = '6'; return true; }
			if (((key == KeyCode.D7) && !isShiftDown) || (key == KeyCode.Keypad7)) { keyCode = '7'; return true; }
			if (((key == KeyCode.D8) && !isShiftDown) || (key == KeyCode.Keypad8)) { keyCode = '8'; return true; }
			if (((key == KeyCode.D9) && !isShiftDown) || (key == KeyCode.Keypad9)) { keyCode = '9'; return true; }

			if ((key == KeyCode.D0) && isShiftDown) { keyCode = ')'; return true; }
			if ((key == KeyCode.D1) && isShiftDown) { keyCode = '!'; return true; }
			if ((key == KeyCode.D2) && isShiftDown) { keyCode = '@'; return true; }
			if ((key == KeyCode.D3) && isShiftDown) { keyCode = '#'; return true; }
			if ((key == KeyCode.D4) && isShiftDown) { keyCode = '$'; return true; }
			if ((key == KeyCode.D5) && isShiftDown) { keyCode = '%'; return true; }
			if ((key == KeyCode.D6) && isShiftDown) { keyCode = '^'; return true; }
			if ((key == KeyCode.D7) && isShiftDown) { keyCode = '&'; return true; }
			if ((key == KeyCode.D8) && isShiftDown) { keyCode = '*'; return true; }
			if ((key == KeyCode.D9) && isShiftDown) { keyCode = '('; return true; }

			if (key == KeyCode.Space) { keyCode = ' '; return true; }
			if (key == KeyCode.Tab) { keyCode = '\t'; return true; }
			if (key == KeyCode.Return) { keyCode = (char) 13; return true; }
			if (key == KeyCode.Backspace) { keyCode = (char) 8; return true; }

			if (key == KeyCode.KeypadPlus) { keyCode = '+'; return true; }
			if (key == KeyCode.KeypadPeriod) { keyCode = '.'; return true; }
			if (key == KeyCode.KeypadDivide) { keyCode = '/'; return true; }
			if (key == KeyCode.KeypadMultiply) { keyCode = '*'; return true; }
			if (key == KeyCode.Backslash) { keyCode = '\\'; return true; }
			if ((key == KeyCode.Comma) && !isShiftDown) { keyCode = ','; return true; }
			if ((key == KeyCode.Comma) && isShiftDown) { keyCode = '<'; return true; }
			if ((key == KeyCode.LeftBracket) && !isShiftDown) { keyCode = '['; return true; }
			if ((key == KeyCode.LeftBracket) && isShiftDown) { keyCode = '{'; return true; }
			if ((key == KeyCode.RightBracket) && !isShiftDown) { keyCode = ']'; return true; }
			if ((key == KeyCode.RightBracket) && isShiftDown) { keyCode = '}'; return true; }
			if ((key == KeyCode.Period) && !isShiftDown) { keyCode = '.'; return true; }
			if ((key == KeyCode.Period) && isShiftDown) { keyCode = '>'; return true; }
			if ((key == KeyCode.Slash) && !isShiftDown) { keyCode = '/'; return true; }
			if ((key == KeyCode.Slash) && isShiftDown) { keyCode = '?'; return true; }
			if ((key == KeyCode.Equals) && !isShiftDown) { keyCode = '='; return true; }
			if ((key == KeyCode.Equals) && isShiftDown) { keyCode = '+'; return true; }
			if ((key == KeyCode.Minus) && !isShiftDown) { keyCode = '-'; return true; }
			if ((key == KeyCode.Minus) && isShiftDown) { keyCode = '_'; return true; }
			if ((key == KeyCode.Apostrophe) && !isShiftDown) { keyCode = '\''; return true; }
			if ((key == KeyCode.Apostrophe) && isShiftDown) { keyCode = '"'; return true; }
			if ((key == KeyCode.Semicolon) && !isShiftDown) { keyCode = ';'; return true; }
			if ((key == KeyCode.Semicolon) && isShiftDown) { keyCode = ':'; return true; }
			if ((key == KeyCode.Grave) && !isShiftDown) { keyCode = '`'; return true; }
			if ((key == KeyCode.Grave) && isShiftDown) { keyCode = '~'; return true; }
			if (key == KeyCode.KeypadMinus) { keyCode = '-'; return true; }

			keyCode = (char) 0;
			return false;
		}
	}
}

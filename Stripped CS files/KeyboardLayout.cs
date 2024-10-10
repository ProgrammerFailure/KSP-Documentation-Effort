using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;

public class KeyboardLayout
{
	public CultureInfo Locale;

	public KeyboardLayoutType Type;

	[DllImport("user32.dll", SetLastError = true)]
	public static extern uint GetKeyboardLayout(uint idThread);

	public static KeyboardLayout GetKeyboardLayout()
	{
		long num = GetKeyboardLayout(0u);
		Debug.Log($"Layout {num:X8}");
		return new KeyboardLayout
		{
			Locale = new CultureInfo((int)(num & 0xFFFFL)),
			Type = GetKeyboardLayoutType(num)
		};
	}

	public static KeyboardLayoutType GetKeyboardLayoutType(long layoutCode)
	{
		switch ((int)(layoutCode >> 16))
		{
		case 1062:
			return KeyboardLayoutType.Latvian;
		default:
			return KeyboardLayoutType.QWERTY;
		case 61460:
		case 66591:
			return KeyboardLayoutType.TurkishF;
		case 61442:
		case 66569:
			return KeyboardLayoutType.Dvorak;
		case 1036:
		case 2060:
			return KeyboardLayoutType.AZERTY;
		case 1031:
		case 2055:
			return KeyboardLayoutType.QWERTZ;
		}
	}
}

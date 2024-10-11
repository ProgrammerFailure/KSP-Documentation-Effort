using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public class KeyboardLayout
{
	public CultureInfo Locale;

	public KeyboardLayoutType Type;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KeyboardLayout()
	{
		throw null;
	}

	[DllImport("user32.dll", SetLastError = true)]
	private static extern uint GetKeyboardLayout(uint idThread);

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static KeyboardLayout GetKeyboardLayout()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static KeyboardLayoutType GetKeyboardLayoutType(long layoutCode)
	{
		throw null;
	}
}

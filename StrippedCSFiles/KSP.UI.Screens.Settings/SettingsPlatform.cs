using System;
using System.Runtime.CompilerServices;

namespace KSP.UI.Screens.Settings;

public static class SettingsPlatform
{
	[Flags]
	public enum Platform
	{
		Everything = -1,
		None = 0,
		PC = 1,
		Linux = 2,
		OSX = 4,
		PS4 = 0x10,
		XBoxOne = 0x20,
		XBox360 = 0x40,
		WiiU = 0x80
	}

	public static Platform platformEmulator;

	[MethodImpl(MethodImplOptions.NoInlining)]
	static SettingsPlatform()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool IsPlatform(Platform platform)
	{
		throw null;
	}
}

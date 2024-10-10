using UnityEngine;

namespace Smooth.Platform;

public static class PlatformExtensions
{
	public static BasePlatform ToBasePlatform(this RuntimePlatform runtimePlatform)
	{
		switch (runtimePlatform)
		{
		case RuntimePlatform.OSXEditor:
		case RuntimePlatform.OSXPlayer:
		case RuntimePlatform.WindowsPlayer:
		case RuntimePlatform.WindowsEditor:
			return BasePlatform.Osx;
		case RuntimePlatform.IPhonePlayer:
			return BasePlatform.Ios;
		case RuntimePlatform.Android:
			return BasePlatform.Android;
		case RuntimePlatform.LinuxPlayer:
			return BasePlatform.Linux;
		default:
			return BasePlatform.None;
		case RuntimePlatform.WebGLPlayer:
			return BasePlatform.WebGl;
		case RuntimePlatform.MetroPlayerX86:
		case RuntimePlatform.MetroPlayerX64:
		case RuntimePlatform.MetroPlayerARM:
			return BasePlatform.Metro;
		}
	}

	public static bool HasJit(this RuntimePlatform runtimePlatform)
	{
		if (runtimePlatform != RuntimePlatform.IPhonePlayer)
		{
			return runtimePlatform != RuntimePlatform.PS4;
		}
		return false;
	}

	public static bool HasJit(this BasePlatform basePlatform)
	{
		if (basePlatform != BasePlatform.Ios && basePlatform != BasePlatform.Ps3 && basePlatform != BasePlatform.Xbox360)
		{
			return basePlatform != BasePlatform.Ps4;
		}
		return false;
	}

	public static bool NoJit(this RuntimePlatform runtimePlatform)
	{
		return !runtimePlatform.HasJit();
	}

	public static bool NoJit(this BasePlatform basePlatform)
	{
		return !basePlatform.HasJit();
	}
}

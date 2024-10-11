using System.Runtime.CompilerServices;
using UnityEngine;

namespace Smooth.Platform;

public static class PlatformExtensions
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static BasePlatform ToBasePlatform(this RuntimePlatform runtimePlatform)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool HasJit(this RuntimePlatform runtimePlatform)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool HasJit(this BasePlatform basePlatform)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool NoJit(this RuntimePlatform runtimePlatform)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool NoJit(this BasePlatform basePlatform)
	{
		throw null;
	}
}

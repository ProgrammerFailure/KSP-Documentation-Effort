using System.Runtime.CompilerServices;
using UnityEngine;

namespace Smooth.Platform;

public static class Runtime
{
	public static readonly RuntimePlatform Platform;

	public static readonly BasePlatform BasePlatform;

	public static readonly bool HasJit;

	public static readonly bool NoJit;

	[MethodImpl(MethodImplOptions.NoInlining)]
	static Runtime()
	{
		throw null;
	}
}

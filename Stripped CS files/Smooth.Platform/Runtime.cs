using UnityEngine;

namespace Smooth.Platform;

public static class Runtime
{
	public static readonly RuntimePlatform Platform = Application.platform;

	public static readonly BasePlatform BasePlatform = Platform.ToBasePlatform();

	public static readonly bool HasJit = BasePlatform.HasJit();

	public static readonly bool NoJit = !HasJit;
}

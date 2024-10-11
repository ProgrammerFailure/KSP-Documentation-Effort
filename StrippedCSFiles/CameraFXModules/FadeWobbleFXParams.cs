using System;
using System.Runtime.CompilerServices;

namespace CameraFXModules;

[Serializable]
public class FadeWobbleFXParams
{
	public float duration;

	public float falloff;

	public WobbleFXParams wobblePars;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FadeWobbleFXParams()
	{
		throw null;
	}
}

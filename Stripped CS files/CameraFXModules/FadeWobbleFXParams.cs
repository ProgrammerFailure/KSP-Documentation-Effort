using System;

namespace CameraFXModules;

[Serializable]
public class FadeWobbleFXParams
{
	public float duration;

	public float falloff;

	public WobbleFXParams wobblePars;
}

using System;

namespace CameraFXModules;

[Serializable]
public class WobbleFXParams
{
	public MinMaxFloat amplitude;

	public MinMaxFloat frequency;

	public float slope;

	public FXModuleParams fxPars;
}

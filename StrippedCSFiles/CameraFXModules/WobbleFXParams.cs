using System;
using System.Runtime.CompilerServices;

namespace CameraFXModules;

[Serializable]
public class WobbleFXParams
{
	public MinMaxFloat amplitude;

	public MinMaxFloat frequency;

	public float slope;

	public FXModuleParams fxPars;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public WobbleFXParams()
	{
		throw null;
	}
}

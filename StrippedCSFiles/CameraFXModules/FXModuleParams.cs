using System;
using System.Runtime.CompilerServices;

namespace CameraFXModules;

[Serializable]
public class FXModuleParams
{
	public float rotFactor;

	public float linFactor;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FXModuleParams()
	{
		throw null;
	}
}

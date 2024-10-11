using System.Runtime.CompilerServices;

namespace ModuleWheels;

public class ModuleWheelLock : ModuleWheelSubmodule
{
	[KSPField]
	public float maxTorque;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleWheelLock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnWheelSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string OnGatherInfo()
	{
		throw null;
	}
}

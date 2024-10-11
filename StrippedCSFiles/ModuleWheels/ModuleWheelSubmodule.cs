using System.Runtime.CompilerServices;

namespace ModuleWheels;

public abstract class ModuleWheelSubmodule : PartModule
{
	[KSPField]
	public int baseModuleIndex;

	protected ModuleWheelBase wheelBase;

	internal KSPWheelController wheel;

	protected bool baseSetup;

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected ModuleWheelSubmodule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetWheelBase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnWheelInit(KSPWheelController w)
	{
		throw null;
	}

	protected abstract void OnWheelSetup();

	public abstract string OnGatherInfo();

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnSubsystemsModified(WheelSubsystems s)
	{
		throw null;
	}
}

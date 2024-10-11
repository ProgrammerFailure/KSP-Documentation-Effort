using System.Runtime.CompilerServices;

public class ModuleEffectTest : PartModule
{
	[KSPField(isPersistant = true)]
	public string fireEffectName;

	[KSPField(isPersistant = true)]
	public string powerEffectName;

	[KSPField(isPersistant = true, guiActive = true)]
	public float effectStrength;

	[KSPField(isPersistant = true)]
	public float effectStrengthSpeed;

	[KSPField(isPersistant = true)]
	public float effectTarget;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleEffectTest()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiName = "#autoLOC_502068")]
	public void ToggleAction()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001871")]
	public void FireAction()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}
}

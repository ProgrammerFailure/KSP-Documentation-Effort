using System.Runtime.CompilerServices;

public class AxisTestModule : PartModule
{
	[KSPAxisField(guiActive = true, incrementalSpeed = 2f, axisGroup = KSPAxisGroup.REPLACEWITHDEFAULT, axisMode = KSPAxisMode.Incremental, maxValue = 5f, minValue = 1f, guiName = "Test Field")]
	public float TestField;

	[KSPField]
	public KSPAxisGroup defaultAxisGroup;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AxisTestModule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}
}

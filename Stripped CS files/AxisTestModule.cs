public class AxisTestModule : PartModule
{
	[KSPAxisField(guiActive = true, incrementalSpeed = 2f, axisGroup = KSPAxisGroup.REPLACEWITHDEFAULT, axisMode = KSPAxisMode.Incremental, maxValue = 5f, minValue = 1f, guiName = "Test Field")]
	public float TestField = 3f;

	[KSPField]
	public KSPAxisGroup defaultAxisGroup;

	public override void OnStart(StartState state)
	{
		BaseAxisField baseAxisField = base.Fields["TestField"] as BaseAxisField;
		baseAxisField.defaultAxisGroup = defaultAxisGroup;
		if (baseAxisField.axisGroup == KSPAxisGroup.REPLACEWITHDEFAULT)
		{
			baseAxisField.axisGroup = defaultAxisGroup;
		}
	}
}

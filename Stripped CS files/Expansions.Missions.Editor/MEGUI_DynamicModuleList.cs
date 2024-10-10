namespace Expansions.Missions.Editor;

public sealed class MEGUI_DynamicModuleList : MEGUI_Control
{
	public bool allowMultipleModuleInstances;

	public bool displayMessageWhenEmpty;

	public string displayEmptyMessage = "";

	public MEGUI_DynamicModuleList()
	{
		allowMultipleModuleInstances = false;
	}
}

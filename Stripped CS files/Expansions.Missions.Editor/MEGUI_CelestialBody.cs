namespace Expansions.Missions.Editor;

public class MEGUI_CelestialBody : MEGUI_Control
{
	public bool usePQS;

	public bool showAnySOIoption;

	public MEGUI_CelestialBody()
	{
		checkpointValidation = CheckpointValidationType.Controls;
	}
}

namespace Expansions.Missions.Editor;

public class MEGUI_VesselGroundLocation : MEGUI_Control
{
	public bool DisableRotationX;

	public bool DisableRotationY;

	public bool DisableRotationZ;

	public bool DisplayVesselGizmoOptions;

	public bool AllowWaterSurfacePlacement;

	public MEGUI_VesselGroundLocation()
	{
		checkpointValidation = CheckpointValidationType.Controls;
	}
}

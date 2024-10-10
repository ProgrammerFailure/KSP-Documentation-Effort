using FinePrint;
using ns9;

namespace ns1;

public class DeleteCustomWaypoint : MapContextMenuOption
{
	public Waypoint waypoint;

	public DeleteCustomWaypoint(Waypoint waypoint)
		: base("Delete Waypoint")
	{
		this.waypoint = waypoint;
	}

	public override void OnSelect()
	{
		if (waypoint != null && !(ScenarioCustomWaypoints.Instance == null))
		{
			if (HighLogic.LoadedSceneIsFlight && waypoint.isNavigatable)
			{
				NavWaypoint.DeactivateIfWaypoint(waypoint);
			}
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_7003245", waypoint.FullName), 2.5f, ScreenMessageStyle.UPPER_CENTER);
			ScenarioCustomWaypoints.RemoveWaypoint(waypoint);
		}
	}

	public override bool OnCheckEnabled(out string fbText)
	{
		fbText = Localizer.Format("#autoLOC_465577");
		if (waypoint != null)
		{
			return waypoint.isCustom;
		}
		return false;
	}

	public override bool CheckAvailable()
	{
		if (waypoint != null)
		{
			return waypoint.isCustom;
		}
		return false;
	}
}

using FinePrint;
using ns9;

namespace ns1;

public class WaypointNavigation : MapContextMenuOption
{
	public Waypoint waypoint;

	public bool IsReady
	{
		get
		{
			if (HighLogic.LoadedSceneIsFlight && waypoint != null && waypoint.isNavigatable)
			{
				return NavWaypoint.fetch != null;
			}
			return false;
		}
	}

	public bool CurrentlyNavigating
	{
		get
		{
			NavWaypoint fetch = NavWaypoint.fetch;
			if (waypoint != null && fetch != null && fetch.IsActive)
			{
				return fetch.IsUsing(waypoint);
			}
			return false;
		}
	}

	public WaypointNavigation(Waypoint waypoint)
		: base(Localizer.Format("#autoLOC_465847"))
	{
		this.waypoint = waypoint;
	}

	public override void OnSelect()
	{
		if (!IsReady)
		{
			return;
		}
		NavWaypoint fetch = NavWaypoint.fetch;
		if (!(fetch == null))
		{
			if (CurrentlyNavigating)
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_465864"), 2.5f, ScreenMessageStyle.UPPER_CENTER);
				fetch.Clear();
				fetch.Deactivate();
			}
			else
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_7003247", waypoint.FullName), 2.5f, ScreenMessageStyle.UPPER_CENTER);
				fetch.Setup(waypoint);
				fetch.Activate();
			}
		}
	}

	public override bool OnCheckEnabled(out string fbText)
	{
		fbText = Localizer.Format("#autoLOC_465882");
		if (!IsReady)
		{
			return false;
		}
		fbText = ((!CurrentlyNavigating) ? Localizer.Format("#autoLOC_7003269") : Localizer.Format("#autoLOC_7003270"));
		return true;
	}

	public override bool CheckAvailable()
	{
		return IsReady;
	}
}

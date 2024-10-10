using FinePrint;

namespace Expansions.Missions;

public interface INodeWaypoint
{
	bool HasNodeWaypoint();

	Waypoint GetNodeWaypoint();
}

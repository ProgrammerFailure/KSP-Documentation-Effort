using FinePrint;

namespace Expansions.Missions;

internal interface INodeWaypoint
{
	bool HasNodeWaypoint();

	Waypoint GetNodeWaypoint();
}

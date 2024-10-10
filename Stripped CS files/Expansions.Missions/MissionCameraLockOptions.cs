using System.ComponentModel;

namespace Expansions.Missions;

public enum MissionCameraLockOptions
{
	[Description("#autoLOC_8004183")]
	NoChange,
	[Description("#autoLOC_8004187")]
	LockAllowMap,
	[Description("#autoLOC_8004189")]
	LockDisableMap,
	[Description("#autoLOC_8004188")]
	Unlock
}

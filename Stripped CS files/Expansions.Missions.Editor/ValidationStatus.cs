using System.ComponentModel;

namespace Expansions.Missions.Editor;

public enum ValidationStatus
{
	[Description("#autoLOC_8006047")]
	Pass,
	[Description("#autoLOC_8006048")]
	Warn,
	[Description("#autoLOC_8003072")]
	Fail,
	[Description("#autoLOC_6003083")]
	None
}

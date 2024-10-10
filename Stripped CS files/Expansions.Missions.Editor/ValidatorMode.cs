using System.ComponentModel;

namespace Expansions.Missions.Editor;

public enum ValidatorMode
{
	[Description("#autoLOC_8200118")]
	Manual,
	[Description("#autoLOC_8200119")]
	Save,
	[Description("#autoLOC_8200120")]
	AutoAfterRun,
	[Description("#autoLOC_8200121")]
	FullAuto
}

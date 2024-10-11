using System.ComponentModel;

namespace Expansions.Missions;

public enum ScoreModifierType
{
	[Description("#autoLOC_8100127")]
	Multiply,
	[Description("#autoLOC_8100128")]
	Divide,
	[Description("#autoLOC_8100129")]
	Substract,
	[Description("#autoLOC_8100130")]
	Set
}

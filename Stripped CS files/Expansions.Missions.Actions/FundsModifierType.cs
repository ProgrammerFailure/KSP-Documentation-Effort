using System.ComponentModel;

namespace Expansions.Missions.Actions;

public enum FundsModifierType
{
	[Description("#autoLOC_8003126")]
	Add,
	[Description("#autoLOC_8100127")]
	Multiply,
	[Description("#autoLOC_8100128")]
	Divide,
	[Description("#autoLOC_8003127")]
	Substract,
	[Description("#autoLOC_8003128")]
	Set
}

using System;
using System.ComponentModel;

[Flags]
public enum KSPAxisGroup
{
	REPLACEWITHDEFAULT = -1,
	[Description("#autoLOC_6003000")]
	None = 0,
	[Description("#autoLOC_6013004")]
	Pitch = 1,
	[Description("#autoLOC_6013005")]
	Yaw = 2,
	[Description("#autoLOC_6013006")]
	Roll = 4,
	[Description("#autoLOC_6013007")]
	TranslateX = 8,
	[Description("#autoLOC_6013008")]
	TranslateY = 0x10,
	[Description("#autoLOC_6013009")]
	TranslateZ = 0x20,
	[Description("#autoLOC_6013010")]
	MainThrottle = 0x40,
	[Description("#autoLOC_6013011")]
	WheelSteer = 0x80,
	[Description("#autoLOC_6013012")]
	WheelThrottle = 0x100,
	[Description("#autoLOC_6013013")]
	Custom01 = 0x200,
	[Description("#autoLOC_6013014")]
	Custom02 = 0x400,
	[Description("#autoLOC_6013015")]
	Custom03 = 0x800,
	[Description("#autoLOC_6013016")]
	Custom04 = 0x1000
}

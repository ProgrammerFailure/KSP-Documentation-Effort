using System;
using System.ComponentModel;

[Flags]
public enum KSPActionGroup
{
	REPLACEWITHDEFAULT = -1,
	[Description("#autoLOC_6003000")]
	None = 0,
	[Description("#autoLOC_6003001")]
	Stage = 1,
	[Description("#autoLOC_6003002")]
	Gear = 2,
	[Description("#autoLOC_6003003")]
	Light = 4,
	[Description("#autoLOC_6003004")]
	RCS = 8,
	[Description("#autoLOC_6003005")]
	SAS = 0x10,
	[Description("#autoLOC_6003006")]
	Brakes = 0x20,
	[Description("#autoLOC_6003007")]
	Abort = 0x40,
	[Description("#autoLOC_6003008")]
	Custom01 = 0x80,
	[Description("#autoLOC_6003009")]
	Custom02 = 0x100,
	[Description("#autoLOC_6003010")]
	Custom03 = 0x200,
	[Description("#autoLOC_6003011")]
	Custom04 = 0x400,
	[Description("#autoLOC_6003012")]
	Custom05 = 0x800,
	[Description("#autoLOC_6003013")]
	Custom06 = 0x1000,
	[Description("#autoLOC_6003014")]
	Custom07 = 0x2000,
	[Description("#autoLOC_6003015")]
	Custom08 = 0x4000,
	[Description("#autoLOC_6003016")]
	Custom09 = 0x8000,
	[Description("#autoLOC_6003017")]
	Custom10 = 0x10000
}

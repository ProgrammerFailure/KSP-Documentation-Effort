using System.ComponentModel;

public enum ExperimentSituations
{
	[Description("#autoLOC_6002000")]
	SrfLanded = 1,
	[Description("#autoLOC_6002001")]
	SrfSplashed = 2,
	[Description("#autoLOC_6002002")]
	FlyingLow = 4,
	[Description("#autoLOC_6002003")]
	FlyingHigh = 8,
	[Description("#autoLOC_6002004")]
	InSpaceLow = 0x10,
	[Description("#autoLOC_6002005")]
	InSpaceHigh = 0x20
}

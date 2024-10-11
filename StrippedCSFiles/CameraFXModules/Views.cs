using System;

namespace CameraFXModules;

[Flags]
public enum Views
{
	None = 0,
	FlightExternal = 1,
	FlightInternal = 2,
	FlightMap = 4,
	KSC = 8,
	TrackingStation = 0x10,
	Editors = 0x20,
	All = -1
}

using System;

namespace CameraFXModules;

[Flags]
public enum WobbleModes
{
	None = 0,
	X = 1,
	Y = 2,
	Z = 4,
	Linear = 7,
	Pitch = 8,
	Yaw = 0x10,
	Roll = 0x20,
	Rot = 0x38,
	All = -1
}

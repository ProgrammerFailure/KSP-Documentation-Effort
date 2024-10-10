using System;

namespace CameraFXModules;

[Flags]
public enum WobbleModes
{
	None = 0,
	flag_1 = 1,
	flag_2 = 2,
	flag_3 = 4,
	Linear = 7,
	Pitch = 8,
	Yaw = 0x10,
	Roll = 0x20,
	Rot = 0x38,
	All = -1
}

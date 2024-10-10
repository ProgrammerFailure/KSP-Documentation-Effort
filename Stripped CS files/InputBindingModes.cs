using System;

[Flags]
public enum InputBindingModes
{
	None = 0,
	Staging = 1,
	Docking_Translation = 2,
	Docking_Rotation = 4,
	RotationModes = 5,
	DockingModes = 6,
	Any = -1
}

using System;

[Flags]
public enum DiscoveryLevels
{
	None = 0,
	Presence = 1,
	Name = 4,
	StateVectors = 8,
	Appearance = 0x10,
	Unowned = 0x1D,
	Owned = -1
}

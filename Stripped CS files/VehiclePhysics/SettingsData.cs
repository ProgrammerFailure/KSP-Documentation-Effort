using System.Runtime.InteropServices;

namespace VehiclePhysics;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct SettingsData
{
	public const int DifferentialLock = 0;

	public const int DrivelineLock = 1;

	public const int AutoShiftOverride = 2;

	public const int AbsOverride = 3;

	public const int EscOverride = 4;

	public const int TcsOverride = 5;

	public const int AsrOverride = 6;

	public const int SteeringAidsOverride = 7;

	public const int _SETTINGS_SIZE = 10;
}

using System.Runtime.InteropServices;

namespace VehiclePhysics;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct InputData
{
	public const int Steer = 0;

	public const int Throttle = 1;

	public const int Brake = 2;

	public const int Handbrake = 3;

	public const int Clutch = 4;

	public const int ManualGear = 5;

	public const int AutomaticGear = 6;

	public const int GearShift = 7;

	public const int Retarder = 8;

	public const int Key = 9;

	public const int _STDINPUT_SIZE = 10;
}

using System.Runtime.InteropServices;

namespace VehiclePhysics;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct VehicleData
{
	public const int Speed = 0;

	public const int EngineRpm = 1;

	public const int EngineStalled = 2;

	public const int EngineWorking = 3;

	public const int EngineStarting = 4;

	public const int EngineLimiter = 5;

	public const int EngineLoad = 6;

	public const int EngineTorque = 7;

	public const int EnginePower = 8;

	public const int EngineFuelRate = 9;

	public const int ClutchTorque = 10;

	public const int ClutchLock = 11;

	public const int GearboxGear = 12;

	public const int GearboxMode = 13;

	public const int GearboxShifting = 14;

	public const int RetarderTorque = 15;

	public const int TransmissionRpm = 16;

	public const int AbsEngaged = 17;

	public const int TcsEngaged = 18;

	public const int EscEngaged = 19;

	public const int AsrEngaged = 20;

	public const int AidedSteer = 21;

	public const int _VEHICLE_SIZE = 22;
}

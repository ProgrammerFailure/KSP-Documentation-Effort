using System.Runtime.CompilerServices;

namespace ModuleWheels;

public class WheelSubsystem
{
	public enum SystemTypes
	{
		None = 0,
		Tire = 1,
		Suspension = 2,
		Steering = 4,
		Motor = 8,
		Brakes = 16,
		WheelCollider = 32,
		Bogey = 64,
		All = -1,
		Any = -1
	}

	public string reason;

	public SystemTypes type;

	public PartModule owner;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public WheelSubsystem(string reason, SystemTypes type, PartModule owner)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsType(SystemTypes type)
	{
		throw null;
	}
}

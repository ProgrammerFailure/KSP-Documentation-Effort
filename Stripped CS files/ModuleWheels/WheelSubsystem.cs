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

	public WheelSubsystem(string reason, SystemTypes type, PartModule owner)
	{
		this.reason = reason;
		this.type = type;
		this.owner = owner;
	}

	public bool IsType(SystemTypes type)
	{
		return (type & this.type) != 0;
	}
}

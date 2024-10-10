using System;

namespace ModuleWheels;

public class WheelUtil
{
	public static float RpmToDegsSec(float rpm)
	{
		return rpm * 360f * 60f;
	}

	public static float RpmToSpeed(float rpm, float wheelRadius)
	{
		return (float)Math.PI * 2f * wheelRadius * (rpm / 60f);
	}

	public static float SpeedToRpm(float speed, float wheelRadius)
	{
		return speed * 30f / (wheelRadius * (float)Math.PI);
	}
}

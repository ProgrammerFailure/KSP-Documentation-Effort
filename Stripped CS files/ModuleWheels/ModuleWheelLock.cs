namespace ModuleWheels;

public class ModuleWheelLock : ModuleWheelSubmodule
{
	[KSPField]
	public float maxTorque = 1000f;

	public override void OnWheelSetup()
	{
		wheel.maxBrakeTorque = maxTorque;
		wheel.brakeResponse = 1000f;
		wheel.brakeInput = 1f;
	}

	public override string OnGatherInfo()
	{
		return null;
	}
}

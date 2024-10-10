public class PartValues : EventValueWrapper
{
	public EventValueComparison<float> MaxThrottle = new EventValueComparison<float>("MaxThrottle", 1f, (float a, float b) => a > b);

	public EventValueComparison<float> HeatProduction = new EventValueComparison<float>("HeatProduction", 1f, (float a, float b) => a < b);

	public EventValueComparison<float> FuelUsage = new EventValueComparison<float>("FuelUsage", 1f, (float a, float b) => a < b);

	public EventValueComparison<float> EnginePower = new EventValueComparison<float>("EnginePower", 1f, (float a, float b) => a > b);

	public EventValueComparison<float> SteeringRadius = new EventValueComparison<float>("SteeringRadius", 1f, (float a, float b) => a > b);

	public EventValueComparison<int> AutopilotSkill = new EventValueComparison<int>("PilotSkill", -1, (int a, int b) => a > b);

	public EventValueComparison<int> AutopilotKerbalSkill = new EventValueComparison<int>("PilotKerbalSkill", -1, (int a, int b) => a > b);

	public EventValueComparison<int> AutopilotSASSkill = new EventValueComparison<int>("PilotSASSkill", -1, (int a, int b) => a > b);

	public EventValueComparison<int> RepairSkill = new EventValueComparison<int>("EngineerSkill", -1, (int a, int b) => a > b);

	public EventValueComparison<int> FailureRepairSkill = new EventValueComparison<int>("EngineerSkill", -1, (int a, int b) => a > b);

	public EventValueComparison<int> ScienceSkill = new EventValueComparison<int>("ScientistSkill", -1, (int a, int b) => a > b);

	public EventValueComparison<float> CommsRange = new EventValueComparison<float>("CommsRange", 1f, (float a, float b) => a > b);

	public EventValueOperation<float> ScienceReturnSum = new EventValueOperation<float>("ScienceReturn", 1f, (float a, float b) => a + b);

	public EventValueComparison<float> ScienceReturnMax = new EventValueComparison<float>("ScienceReturn_Max", 1f, (float a, float b) => a > b);

	public EventValueComparison<int> EVAChuteSkill = new EventValueComparison<int>("EVAChuteSkill", 0, (int a, int b) => a > b);
}

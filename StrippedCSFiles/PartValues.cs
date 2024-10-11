using System.Runtime.CompilerServices;

public class PartValues : EventValueWrapper
{
	public EventValueComparison<float> MaxThrottle;

	public EventValueComparison<float> HeatProduction;

	public EventValueComparison<float> FuelUsage;

	public EventValueComparison<float> EnginePower;

	public EventValueComparison<float> SteeringRadius;

	public EventValueComparison<int> AutopilotSkill;

	public EventValueComparison<int> AutopilotKerbalSkill;

	public EventValueComparison<int> AutopilotSASSkill;

	public EventValueComparison<int> RepairSkill;

	public EventValueComparison<int> FailureRepairSkill;

	public EventValueComparison<int> ScienceSkill;

	public EventValueComparison<float> CommsRange;

	public EventValueOperation<float> ScienceReturnSum;

	public EventValueComparison<float> ScienceReturnMax;

	public EventValueComparison<int> EVAChuteSkill;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartValues()
	{
		throw null;
	}
}

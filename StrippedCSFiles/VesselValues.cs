using System;
using System.Runtime.CompilerServices;

public class VesselValues
{
	public class PartValuesComparison<T>
	{
		private Vessel host;

		private Func<T, T, bool> comparer;

		private Func<Part, T> valueAccessor;

		private int currentFrame;

		private T lastValue;

		public T defaultValue;

		public T value
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public T valueUncached
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public PartValuesComparison(Vessel host, T defaultValue, Func<T, T, bool> comparison, Func<Part, T> valueAccessor)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void ResetValueCache()
		{
			throw null;
		}
	}

	public class PartValuesOperation<T>
	{
		private Vessel host;

		private Func<T, T, T> operation;

		private Func<Part, T> valueAccessor;

		public T defaultValue;

		public T value
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public PartValuesOperation(Vessel host, T defaultValue, Func<T, T, T> operation, Func<Part, T> valueAccessor)
		{
			throw null;
		}
	}

	public PartValuesComparison<float> MaxThrottle;

	public PartValuesComparison<float> HeatProduction;

	public PartValuesComparison<float> FuelUsage;

	public PartValuesComparison<float> EnginePower;

	public PartValuesComparison<float> SteeringRadius;

	public PartValuesComparison<int> AutopilotSkill;

	public PartValuesComparison<int> AutopilotKerbalSkill;

	public PartValuesComparison<int> AutopilotSASSkill;

	public PartValuesComparison<int> RepairSkill;

	public PartValuesComparison<int> FailureRepairSkill;

	public PartValuesComparison<int> ScienceSkill;

	public PartValuesComparison<int> EVAChuteSkill;

	public PartValuesComparison<float> CommsRange;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselValues(Vessel host)
	{
		throw null;
	}
}

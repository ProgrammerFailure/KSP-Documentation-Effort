using System;
using UnityEngine;

public class VesselValues
{
	public class PartValuesComparison<T>
	{
		public Vessel host;

		public Func<T, T, bool> comparer;

		public Func<Part, T> valueAccessor;

		public int currentFrame = -1;

		public T lastValue;

		public T defaultValue;

		public T value
		{
			get
			{
				if (Time.frameCount == currentFrame)
				{
					return lastValue;
				}
				T val = defaultValue;
				int count = host.Parts.Count;
				while (count-- > 0)
				{
					Part arg = host.Parts[count];
					T val2 = valueAccessor(arg);
					if (comparer(val2, val))
					{
						val = val2;
					}
				}
				currentFrame = Time.frameCount;
				lastValue = val;
				return val;
			}
		}

		public T valueUncached
		{
			get
			{
				currentFrame = -1;
				return value;
			}
		}

		public PartValuesComparison(Vessel host, T defaultValue, Func<T, T, bool> comparison, Func<Part, T> valueAccessor)
		{
			this.host = host;
			this.defaultValue = defaultValue;
			comparer = comparison;
			this.valueAccessor = valueAccessor;
		}

		public void ResetValueCache()
		{
			currentFrame = -1;
		}
	}

	public class PartValuesOperation<T>
	{
		public Vessel host;

		public Func<T, T, T> operation;

		public Func<Part, T> valueAccessor;

		public T defaultValue;

		public T value
		{
			get
			{
				T val = defaultValue;
				int count = host.Parts.Count;
				while (count-- > 0)
				{
					Part arg = host.Parts[count];
					val = operation(valueAccessor(arg), val);
				}
				return val;
			}
		}

		public PartValuesOperation(Vessel host, T defaultValue, Func<T, T, T> operation, Func<Part, T> valueAccessor)
		{
			this.host = host;
			this.defaultValue = defaultValue;
			this.operation = operation;
			this.valueAccessor = valueAccessor;
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

	public VesselValues(Vessel host)
	{
		MaxThrottle = new PartValuesComparison<float>(host, 1f, (float a, float b) => a > b, (Part p) => p.PartValues.MaxThrottle.value);
		HeatProduction = new PartValuesComparison<float>(host, 1f, (float a, float b) => a < b, (Part p) => p.PartValues.HeatProduction.value);
		FuelUsage = new PartValuesComparison<float>(host, 1f, (float a, float b) => a < b, (Part p) => p.PartValues.FuelUsage.value);
		EnginePower = new PartValuesComparison<float>(host, 1f, (float a, float b) => a > b, (Part p) => p.PartValues.EnginePower.value);
		SteeringRadius = new PartValuesComparison<float>(host, 1f, (float a, float b) => a > b, (Part p) => p.PartValues.SteeringRadius.value);
		AutopilotSkill = new PartValuesComparison<int>(host, -1, (int a, int b) => a > b, (Part p) => p.PartValues.AutopilotSkill.value);
		AutopilotKerbalSkill = new PartValuesComparison<int>(host, -1, (int a, int b) => a > b, (Part p) => p.PartValues.AutopilotKerbalSkill.value);
		AutopilotSASSkill = new PartValuesComparison<int>(host, -1, (int a, int b) => a > b, (Part p) => p.PartValues.AutopilotSASSkill.value);
		RepairSkill = new PartValuesComparison<int>(host, -1, (int a, int b) => a > b, (Part p) => p.PartValues.RepairSkill.value);
		FailureRepairSkill = new PartValuesComparison<int>(host, -1, (int a, int b) => a > b, (Part p) => p.PartValues.FailureRepairSkill.value);
		ScienceSkill = new PartValuesComparison<int>(host, -1, (int a, int b) => a > b, (Part p) => p.PartValues.ScienceSkill.value);
		EVAChuteSkill = new PartValuesComparison<int>(host, 0, (int a, int b) => a > b, (Part p) => p.PartValues.EVAChuteSkill.value);
		CommsRange = new PartValuesComparison<float>(host, 1f, (float a, float b) => a > b, (Part p) => p.PartValues.CommsRange.value);
	}
}

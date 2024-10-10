using System.Collections.Generic;

public class TemperatureModel
{
	public enum TemperatureModels
	{
		LINEAR,
		QUADRATIC,
		CONSTANT,
		STEPPED
	}

	public class TemperatureModelTemplate
	{
		public virtual float getTemperatureAtAltitude(float altitude)
		{
			return 0f;
		}
	}

	public class LinearTemperatureModel : TemperatureModelTemplate
	{
		public float m;

		public float b;

		public LinearTemperatureModel(float m, float b)
		{
			this.m = m;
			this.b = b;
		}

		public LinearTemperatureModel(float temp1, float alt1, float temp2, float alt2)
		{
			m = (temp1 - temp2) / (alt1 - alt2);
			b = temp1 - m * alt1;
		}

		public override float getTemperatureAtAltitude(float altitude)
		{
			return m * altitude + b;
		}
	}

	public class QuadraticTemperatureModel : TemperatureModelTemplate
	{
		public float float_0;

		public float float_1;

		public float float_2;

		public QuadraticTemperatureModel(float float_3, float float_4, float float_5)
		{
			float_0 = float_3;
			float_1 = float_4;
			float_2 = float_5;
		}

		public override float getTemperatureAtAltitude(float altitude)
		{
			return float_0 * altitude * altitude + float_1 * altitude + float_2;
		}
	}

	public class ConstantTemperatureModel : TemperatureModelTemplate
	{
		public float k;

		public ConstantTemperatureModel(float k)
		{
			this.k = k;
		}

		public override float getTemperatureAtAltitude(float altitude)
		{
			return k;
		}
	}

	public class SteppedTemperatureModel : TemperatureModelTemplate
	{
		public struct TempRange
		{
			public float minAlt;

			public float maxAlt;

			public TemperatureModels modelName;

			public TemperatureModelTemplate modelData;
		}

		public List<TempRange> Steps;

		public SteppedTemperatureModel()
		{
			Steps = new List<TempRange>();
		}

		public bool AddInitialLinearStepEquation(float min, float max, float m, float b)
		{
			if (Steps.Count > 0)
			{
				return false;
			}
			TempRange item = default(TempRange);
			item.minAlt = min;
			item.maxAlt = max;
			item.modelName = TemperatureModels.LINEAR;
			item.modelData = new LinearTemperatureModel(m, b);
			Steps.Add(item);
			return true;
		}

		public bool AddInitialLinearStep(float temp1, float alt1, float temp2, float alt2)
		{
			if (Steps.Count > 0)
			{
				return false;
			}
			TempRange item = default(TempRange);
			item.minAlt = ((temp1 < temp2) ? temp1 : temp2);
			item.maxAlt = ((temp1 > temp2) ? temp1 : temp2);
			item.modelName = TemperatureModels.LINEAR;
			item.modelData = new LinearTemperatureModel(temp1, alt1, temp2, alt2);
			Steps.Add(item);
			return true;
		}

		public bool AddInitialQuadraticStep(float min, float max, float float_0, float float_1, float float_2)
		{
			if (Steps.Count > 0)
			{
				return false;
			}
			TempRange item = default(TempRange);
			item.minAlt = min;
			item.maxAlt = max;
			item.modelName = TemperatureModels.QUADRATIC;
			item.modelData = new QuadraticTemperatureModel(float_0, float_1, float_2);
			Steps.Add(item);
			return true;
		}

		public bool AddInitialConstantStep(float min, float max, float float_0)
		{
			if (Steps.Count > 0)
			{
				return false;
			}
			TempRange item = default(TempRange);
			item.minAlt = min;
			item.maxAlt = max;
			item.modelName = TemperatureModels.LINEAR;
			item.modelData = new ConstantTemperatureModel(float_0);
			Steps.Add(item);
			return true;
		}

		public bool AddConstantStep(float limit, float float_0)
		{
			if (Steps.Count == 0)
			{
				return AddInitialConstantStep((limit < 0f) ? limit : 0f, (limit < 0f) ? 0f : limit, float_0);
			}
			float num = ((limit < 0f) ? 0f : limit);
			int num2 = 0;
			int count = Steps.Count;
			while (true)
			{
				if (num2 < count)
				{
					if (!(num <= Steps[num2].maxAlt))
					{
						break;
					}
					num2++;
					continue;
				}
				return false;
			}
			TempRange item = default(TempRange);
			item.minAlt = Steps[num2].maxAlt;
			item.maxAlt = num;
			item.modelName = TemperatureModels.CONSTANT;
			item.modelData = new ConstantTemperatureModel(float_0);
			Steps.Add(item);
			return true;
		}

		public bool AddLinearStep(float limit, float temp2)
		{
			if (Steps.Count == 0)
			{
				return AddInitialLinearStep((limit < 0f) ? temp2 : 0f, (limit < 0f) ? limit : 0f, (limit < 0f) ? 0f : temp2, (limit < 0f) ? 0f : limit);
			}
			float num = ((limit < 0f) ? 0f : limit);
			int num2 = 0;
			int count = Steps.Count;
			while (true)
			{
				if (num2 < count)
				{
					if (!(num <= Steps[num2].maxAlt))
					{
						break;
					}
					num2++;
					continue;
				}
				return false;
			}
			TempRange item = default(TempRange);
			item.minAlt = Steps[num2].maxAlt;
			item.maxAlt = num;
			item.modelName = TemperatureModels.LINEAR;
			item.modelData = new LinearTemperatureModel(Steps[num2].modelData.getTemperatureAtAltitude(Steps[num2].maxAlt), Steps[num2].maxAlt, temp2, limit);
			Steps.Add(item);
			return true;
		}

		public bool AddQuadraticStep(float limit, float float_0, float float_1, float float_2)
		{
			if (Steps.Count == 0)
			{
				return AddInitialQuadraticStep((limit < 0f) ? limit : 0f, (limit < 0f) ? 0f : limit, float_0, float_1, float_2);
			}
			float num = ((limit < 0f) ? 0f : limit);
			int num2 = 0;
			int count = Steps.Count;
			while (true)
			{
				if (num2 < count)
				{
					if (!(num <= Steps[num2].maxAlt))
					{
						break;
					}
					num2++;
					continue;
				}
				return false;
			}
			TempRange item = default(TempRange);
			item.minAlt = Steps[num2].maxAlt;
			item.maxAlt = num;
			item.modelName = TemperatureModels.QUADRATIC;
			item.modelData = new QuadraticTemperatureModel(float_0, float_1, float_2);
			Steps.Add(item);
			return true;
		}

		public override float getTemperatureAtAltitude(float altitude)
		{
			int num = 0;
			int count = Steps.Count;
			while (true)
			{
				if (num < count)
				{
					if ((double)altitude < (double)Steps[num].maxAlt + 0.0001 && !((double)altitude <= (double)Steps[num].minAlt - 0.0001))
					{
						break;
					}
					num++;
					continue;
				}
				return 0f;
			}
			return Steps[num].modelData.getTemperatureAtAltitude(altitude);
		}

		public void clearIntervals()
		{
			Steps.Clear();
		}
	}

	public SteppedTemperatureModel Intervals;

	public TemperatureModel()
	{
		Intervals = new SteppedTemperatureModel();
	}

	public bool AddInitialConstantStep(float min, float max, float float_0)
	{
		return Intervals.AddInitialConstantStep(min, max, float_0);
	}

	public bool AddInitialLinearStep(float temp1, float alt1, float temp2, float alt2)
	{
		return Intervals.AddInitialLinearStep(temp1, alt1, temp2, alt2);
	}

	public bool AddInitialLinearStepEquation(float min, float max, float m, float b)
	{
		return Intervals.AddInitialLinearStepEquation(min, max, m, b);
	}

	public bool AddInitialQuadraticStep(float min, float max, float float_0, float float_1, float float_2)
	{
		return Intervals.AddInitialQuadraticStep(min, max, float_0, float_1, float_2);
	}

	public bool AddConstantStep(float limit, float float_0)
	{
		return Intervals.AddConstantStep(limit, float_0);
	}

	public bool AddLinearStep(float limit, float temp2)
	{
		return Intervals.AddLinearStep(limit, temp2);
	}

	public bool AddQuadraticStep(float limit, float float_0, float float_1, float float_2)
	{
		return Intervals.AddQuadraticStep(limit, float_0, float_1, float_2);
	}

	public float getTemperatureAtAltitude(double altitude)
	{
		return Intervals.getTemperatureAtAltitude((float)altitude);
	}
}

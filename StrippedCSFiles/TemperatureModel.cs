using System.Collections.Generic;
using System.Runtime.CompilerServices;

internal class TemperatureModel
{
	private enum TemperatureModels
	{
		LINEAR,
		QUADRATIC,
		CONSTANT,
		STEPPED
	}

	private class TemperatureModelTemplate
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public TemperatureModelTemplate()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual float getTemperatureAtAltitude(float altitude)
		{
			throw null;
		}
	}

	private class LinearTemperatureModel : TemperatureModelTemplate
	{
		private float m;

		private float b;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LinearTemperatureModel(float m, float b)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LinearTemperatureModel(float temp1, float alt1, float temp2, float alt2)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override float getTemperatureAtAltitude(float altitude)
		{
			throw null;
		}
	}

	private class QuadraticTemperatureModel : TemperatureModelTemplate
	{
		private float A;

		private float B;

		private float C;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public QuadraticTemperatureModel(float A, float B, float C)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override float getTemperatureAtAltitude(float altitude)
		{
			throw null;
		}
	}

	private class ConstantTemperatureModel : TemperatureModelTemplate
	{
		private float K;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ConstantTemperatureModel(float k)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override float getTemperatureAtAltitude(float altitude)
		{
			throw null;
		}
	}

	private class SteppedTemperatureModel : TemperatureModelTemplate
	{
		private struct TempRange
		{
			public float minAlt;

			public float maxAlt;

			public TemperatureModels modelName;

			public TemperatureModelTemplate modelData;
		}

		private List<TempRange> Steps;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public SteppedTemperatureModel()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool AddInitialLinearStepEquation(float min, float max, float m, float b)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool AddInitialLinearStep(float temp1, float alt1, float temp2, float alt2)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool AddInitialQuadraticStep(float min, float max, float A, float B, float C)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool AddInitialConstantStep(float min, float max, float K)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool AddConstantStep(float limit, float K)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool AddLinearStep(float limit, float temp2)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool AddQuadraticStep(float limit, float A, float B, float C)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override float getTemperatureAtAltitude(float altitude)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void clearIntervals()
		{
			throw null;
		}
	}

	private SteppedTemperatureModel Intervals;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TemperatureModel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AddInitialConstantStep(float min, float max, float K)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AddInitialLinearStep(float temp1, float alt1, float temp2, float alt2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AddInitialLinearStepEquation(float min, float max, float m, float b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AddInitialQuadraticStep(float min, float max, float A, float B, float C)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AddConstantStep(float limit, float K)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AddLinearStep(float limit, float temp2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AddQuadraticStep(float limit, float A, float B, float C)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float getTemperatureAtAltitude(double altitude)
	{
		throw null;
	}
}

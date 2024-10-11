using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Terrain/Land Class Controller")]
public class PQSMod_LandClassController : PQSMod
{
	[Serializable]
	public class LerpRange
	{
		public double startStart;

		public double startEnd;

		public double endStart;

		public double endEnd;

		[HideInInspector]
		public double startDelta;

		[HideInInspector]
		public double endDelta;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LerpRange()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LerpRange(float startStart, float startEnd, float endStart, float endEnd)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Setup()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public double Lerp(double point)
		{
			throw null;
		}
	}

	[Serializable]
	public class LandClassScatterAmount
	{
		public string scatterName;

		public double density;

		[HideInInspector]
		public int scatterIndex;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LandClassScatterAmount()
		{
			throw null;
		}
	}

	[Serializable]
	public class LandClass
	{
		public string name;

		public float strength;

		public Color color;

		public bool useCoverageMap;

		public Texture2D coverageMap;

		public bool useAltitudeRange;

		public LerpRange altitudeRange;

		public bool useLatitudeRange;

		public bool latitudeRangeDouble;

		public LerpRange latitudeRange;

		public bool useLongitudeRange;

		public LerpRange longitudeRange;

		public bool useCoverageNoise;

		public float coverageNoiseBlend;

		public int coverageNoiseSeed;

		public int coverageNoiseOctaves;

		public float coverageNoisePersistance;

		public float coverageNoiseFrequency;

		public bool useColorNoise;

		public Color colorNoiseColor;

		public float colorNoiseBlend;

		public int colorNoiseSeed;

		public int colorNoiseOctaves;

		public float colorNoisePersistance;

		public float colorNoiseFrequency;

		[HideInInspector]
		public Simplex colorNoiseSimplex;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LandClass()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LandClass(string name)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void Reset()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Setup()
		{
			throw null;
		}
	}

	public int landClassSelectedIndex;

	public int guiMainToolbar;

	public string guiNewLCName;

	public bool drawDefaultInspector;

	public List<LandClass> landClasses;

	public double terrainMaxAltitude;

	public LandClass landClassSelected
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_LandClassController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}
}

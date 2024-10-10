using System;
using System.Collections.Generic;
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

		public LerpRange()
		{
			startStart = 0.0;
			startEnd = 0.0;
			endStart = 1.0;
			endEnd = 1.0;
		}

		public LerpRange(float startStart, float startEnd, float endStart, float endEnd)
		{
			this.startStart = startStart;
			this.startEnd = startEnd;
			this.endStart = endStart;
			this.endEnd = endEnd;
		}

		public void Setup()
		{
			startDelta = 1.0 / (startEnd - startStart);
			endDelta = 1.0 / (endEnd - endStart);
		}

		public double Lerp(double point)
		{
			if (!(point < startStart) && point <= endEnd)
			{
				if (point < startEnd)
				{
					return (point - startStart) * startDelta;
				}
				if (point <= endStart)
				{
					return 1.0;
				}
				if (point < endEnd)
				{
					return 1.0 - (point - endStart) * endDelta;
				}
				return 0.0;
			}
			return 0.0;
		}
	}

	[Serializable]
	public class LandClassScatterAmount
	{
		public string scatterName;

		public double density;

		[HideInInspector]
		public int scatterIndex;
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

		public LandClass()
		{
			Reset();
		}

		public LandClass(string name)
		{
			Reset();
			this.name = name;
		}

		public void Reset()
		{
			name = "LandClass";
			strength = 1f;
			coverageNoiseBlend = 0.5f;
			coverageNoiseSeed = 0;
			coverageNoiseOctaves = 4;
			coverageNoisePersistance = 0.5f;
			coverageNoiseFrequency = 12f;
			colorNoiseBlend = 0.5f;
			colorNoiseSeed = 1;
			colorNoiseOctaves = 4;
			colorNoisePersistance = 0.5f;
			colorNoiseFrequency = 12f;
		}

		public void Setup()
		{
			altitudeRange.Setup();
			latitudeRange.Setup();
			longitudeRange.Setup();
			if (useColorNoise)
			{
				colorNoiseSimplex = new Simplex(colorNoiseSeed, colorNoiseOctaves, colorNoisePersistance, colorNoiseFrequency);
			}
		}
	}

	public int landClassSelectedIndex;

	public int guiMainToolbar;

	public string guiNewLCName = "";

	public bool drawDefaultInspector;

	public List<LandClass> landClasses;

	public double terrainMaxAltitude;

	public LandClass landClassSelected => landClasses[landClassSelectedIndex];

	public void Reset()
	{
		LandClass landClass = new LandClass();
		landClass.name = "Base";
		landClass.latitudeRange = new LerpRange(0f, 0f, 1f, 1f);
		landClass.latitudeRange = new LerpRange(0f, 0f, 1f, 1f);
		landClass.longitudeRange = new LerpRange(0f, 0f, 1f, 1f);
		landClasses = new List<LandClass>();
		landClasses.Add(landClass);
		terrainMaxAltitude = 10000.0;
		landClassSelectedIndex = 0;
	}
}

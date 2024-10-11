using System;
using System.Runtime.CompilerServices;
using LibNoise;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Fractal Planet")]
public class PQSMod_VertexPlanet : PQSMod
{
	[Serializable]
	public class SimplexWrapper
	{
		public double deformity;

		public double octaves;

		public double persistance;

		public double frequency;

		public Simplex simplex
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			private set
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public SimplexWrapper(SimplexWrapper copyFrom)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public SimplexWrapper(double deformity, double octaves, double persistance, double frequency)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Setup(int seed)
		{
			throw null;
		}
	}

	[Serializable]
	public class NoiseModWrapper
	{
		public double deformity;

		public int octaves;

		public double persistance;

		public double frequency;

		public int seed
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			set
			{
				throw null;
			}
		}

		public IModule noise
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			private set
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public NoiseModWrapper(NoiseModWrapper copyFrom)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public NoiseModWrapper(double deformity, int octaves, double persistance, double frequency)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Setup(IModule mod)
		{
			throw null;
		}
	}

	[Serializable]
	public class LandClass
	{
		public string name;

		public double fractalStart;

		public double fractalEnd;

		public Color baseColor;

		public Color colorNoise;

		public double colorNoiseAmount;

		public SimplexWrapper colorNoiseMap;

		public bool lerpToNext;

		public double startHeight
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			set
			{
				throw null;
			}
		}

		public double endHeight
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			set
			{
				throw null;
			}
		}

		public double fractalDelta
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			set
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LandClass(string name, double fractalStart, double fractalEnd, Color baseColor, Color colorNoise, double colorNoiseAmount)
		{
			throw null;
		}
	}

	public int seed;

	public double deformity;

	public double colorDeformity;

	public double oceanLevel;

	public double oceanStep;

	public double oceanDepth;

	public bool oceanSnap;

	public double terrainSmoothing;

	public double terrainShapeStart;

	public double terrainShapeEnd;

	public double terrainRidgesMin;

	public double terrainRidgesMax;

	public SimplexWrapper continental;

	public SimplexWrapper continentalRuggedness;

	private SimplexWrapper continentalSmoothing;

	public SimplexWrapper continentalSharpnessMap;

	public NoiseModWrapper continentalSharpness;

	public SimplexWrapper terrainType;

	public bool buildHeightColors;

	private double continentialHeight;

	private double continentialSharpnessValue;

	private double continentialSharpnessMapValue;

	private double continentialHeightSmoothed;

	private double continentialTerrainNoise;

	private double continentalRHeight;

	private double continental2Height;

	private double continentalDelta;

	private int itr;

	private int lcCount;

	private LandClass lcSelected;

	private int lcSelectedIndex;

	private double ct2;

	private double ct3;

	private double tHeight;

	private Color c1;

	private Color c2;

	private LandClass lcLerp;

	private double vHeight;

	public LandClass[] landClasses;

	public double terrainRidgeBalance;

	private double continentalDeformity;

	private double d1;

	private double continentialHeightPreSmooth;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_VertexPlanet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Clamp(double v, double low, double high)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnVertexBuildHeight(PQS.VertexBuildData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnVertexBuild(PQS.VertexBuildData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override double GetVertexMaxHeight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override double GetVertexMinHeight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SelectLandClassByHeight(double height)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Lerp(double v2, double v1, double dt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double CubicHermite(double start, double end, double startTangent, double endTangent, double t)
	{
		throw null;
	}
}

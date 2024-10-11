using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Color/Height-based (Noise)")]
public class PQSMod_HeightColorMapNoise : PQSMod
{
	[Serializable]
	public class LandClass
	{
		public string name;

		public double altStart;

		public double altEnd;

		public Color color;

		public bool lerpToNext;

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

	public LandClass[] landClasses;

	public float blend;

	[HideInInspector]
	public int lcCount;

	private int itr;

	private LandClass lcSelected;

	private int lcSelectedIndex;

	private double vHeight;

	private double ct2;

	private double ct3;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_HeightColorMapNoise()
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
	public override void OnVertexBuild(PQS.VertexBuildData data)
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Clamp(double v, double low, double high)
	{
		throw null;
	}
}

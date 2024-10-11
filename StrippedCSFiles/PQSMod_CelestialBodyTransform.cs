using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Misc/KSP Celestial Body Link")]
public class PQSMod_CelestialBodyTransform : PQSMod
{
	[Serializable]
	public class AltitudeFade
	{
		public string fadeFloatName;

		private int fadeIntID;

		public float fadeStart;

		public float fadeEnd;

		public float valueStart;

		public float valueEnd;

		public float highQualityShaderFadeStart;

		public float highQualityShaderFadeEnd;

		public List<GameObject> secondaryRenderers;

		private List<Renderer> renderers;

		private List<PQS> pqs;

		private float a;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public AltitudeFade()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void DoFade(double alt)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void DoFade(bool fade)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Setup()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Setup(PQS pqs)
		{
			throw null;
		}
	}

	[HideInInspector]
	public CelestialBody body;

	public double deactivateAltitude;

	public double highQualityShaderDeactivateAltitude;

	public bool forceRebuildOnTargetChange;

	public AltitudeFade planetFade;

	public AltitudeFade[] secondaryFades;

	public bool forceActivate;

	public bool overrideFade;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_CelestialBodyTransform()
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
	public override bool OnSphereStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnPreUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSphereTransformUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUpdateFinished()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSphereReset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSphereInactive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FadeOutSphere()
	{
		throw null;
	}
}

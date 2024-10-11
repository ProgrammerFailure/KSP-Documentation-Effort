using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace VehiclePhysics;

[Serializable]
public class TireFriction : ISerializationCallbackReceiver
{
	public enum Model
	{
		Flat,
		Lineal,
		Smooth,
		Parametric,
		Pacejka
	}

	[Serializable]
	public class Settings
	{
		public Vector2 adherent;

		public Vector2 peak;

		public Vector2 limit;

		[Range(0f, 1f)]
		public float a;

		[Range(0f, 1f)]
		public float b;

		[Range(0f, 1f)]
		public float c;

		[Range(0f, 1f)]
		public float d;

		[Range(0.01f, 2f)]
		public float A;

		[Range(0.2f, 4f)]
		public float B;

		[Range(1f, 2f)]
		public float C;

		[Range(0f, 2f)]
		public float D;

		[Range(-20f, 1f)]
		public float E;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Settings()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void ApplyAPLConstraints()
		{
			throw null;
		}
	}

	public class ContactPatch
	{
		public Vector2 slip;

		public float load;

		public float groundGrip;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ContactPatch()
		{
			throw null;
		}
	}

	public class CurveBase
	{
		protected Settings m_params;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public CurveBase(Settings settings)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual float EvaluateForce(float slip, ContactPatch cp)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual float GetAdherentSlip(ContactPatch cp)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual float GetPeakSlip(ContactPatch cp)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual float GetLimitSlip(ContactPatch cp)
		{
			throw null;
		}
	}

	public class FlatFriction : CurveBase
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public FlatFriction(Settings settings)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override float EvaluateForce(float s, ContactPatch cp)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override float GetAdherentSlip(ContactPatch cp)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override float GetPeakSlip(ContactPatch cp)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override float GetLimitSlip(ContactPatch cp)
		{
			throw null;
		}
	}

	public class LinealFriction : CurveBase
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public LinealFriction(Settings settings)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static float FastLerp(Vector2 P0, Vector2 P1, float x)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override float EvaluateForce(float s, ContactPatch cp)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override float GetAdherentSlip(ContactPatch cp)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override float GetPeakSlip(ContactPatch cp)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override float GetLimitSlip(ContactPatch cp)
		{
			throw null;
		}
	}

	public class SmoothFriction : CurveBase
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public SmoothFriction(Settings settings)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static float CubicLerp(float x, Vector2 P0, Vector2 P1)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override float EvaluateForce(float s, ContactPatch cp)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override float GetAdherentSlip(ContactPatch cp)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override float GetPeakSlip(ContactPatch cp)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override float GetLimitSlip(ContactPatch cp)
		{
			throw null;
		}
	}

	public class ParametricFriction : CurveBase
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public ParametricFriction(Settings settings)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static float TangentLerp(float x, Vector2 P0, Vector2 P1, float a, float b)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override float EvaluateForce(float s, ContactPatch cp)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override float GetAdherentSlip(ContactPatch cp)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override float GetPeakSlip(ContactPatch cp)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override float GetLimitSlip(ContactPatch cp)
		{
			throw null;
		}
	}

	public class PacejkaFriction : CurveBase
	{
		private Vector2 m_adherent;

		private Vector2 m_peak;

		private Vector2 m_limit;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public PacejkaFriction(Settings settings)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void GetPacejkaPoints(float A, float B, float C, float D, float E, ref Vector2 adherent, ref Vector2 peak, ref Vector2 limit)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static float Pacejka(float slip, float B, float C, float D, float E)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override float EvaluateForce(float s, ContactPatch cp)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override float GetAdherentSlip(ContactPatch cp)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override float GetPeakSlip(ContactPatch cp)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override float GetLimitSlip(ContactPatch cp)
		{
			throw null;
		}
	}

	[FormerlySerializedAs("type")]
	public Model model;

	public Settings settings;

	public float frictionMultiplier;

	private CurveBase m_curve;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TireFriction()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnBeforeSerialize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnAfterDeserialize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupFrictionCurves()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector2 GetForce(ContactPatch cp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsAdherentSlip(ContactPatch cp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetAdherentSlipForward(ContactPatch cp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetAdherentSlipSideways(ContactPatch cp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector2 GetAdherentSlipBounds(ContactPatch cp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector2 GetPeakSlipBounds(ContactPatch cp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector2 GetLimitSlipBounds(ContactPatch cp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector2 GetAdherentForce(ContactPatch cp)
	{
		throw null;
	}
}

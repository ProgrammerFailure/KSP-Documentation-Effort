using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VehiclePhysics;

public class Inertia
{
	public enum Mode
	{
		Default,
		Parametric,
		InertiaColliders,
		Explicit
	}

	[Serializable]
	public class Settings
	{
		public Mode mode;

		public Collider[] inertiaColliders;

		public Vector3 chassisDimensions;

		[Range(-5f, 5f)]
		public float inertiaBias;

		public Vector3 inertiaTensor;

		public Vector3 inertiaTensorRotation;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Settings()
		{
			throw null;
		}
	}

	public Settings settings;

	public static float lowerLimitRatio;

	public static Color inertiaGizmosColor;

	private Mode m_lastMode;

	private Vector3 m_lastDimensions;

	private float m_lastBias;

	private Vector3 m_lastInertiaTensor;

	private Vector3 m_lastInertiaRotation;

	private float m_lastMass;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Inertia()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static Inertia()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Apply(Rigidbody rigidbody)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DoUpdate(Rigidbody rigidbody)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ApplyInertiaFromColliders(Rigidbody rigidbody, Collider[] inertiaColliders)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ApplyParametricInertia(Rigidbody rigidbody, Vector3 dimensions, float inertiaBias)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ApplyExplicitInertia(Rigidbody rigidbody, Vector3 inertiaTensor, Vector3 inertiaTensorRotation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void VerifyInertiaAndShowWarning(Rigidbody rigidbody)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 ComputeBoxInertia(Vector3 dimensions, float mass)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void DrawGizmo(Settings settings, Rigidbody rigidbody)
	{
		throw null;
	}
}

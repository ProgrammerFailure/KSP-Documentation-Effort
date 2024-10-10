using System;
using EdyCommonTools;
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

		public Collider[] inertiaColliders = new Collider[0];

		public Vector3 chassisDimensions = new Vector3(1.5f, 0.5f, 4f);

		[Range(-5f, 5f)]
		public float inertiaBias;

		public Vector3 inertiaTensor = new Vector3(2000f, 2200f, 400f);

		public Vector3 inertiaTensorRotation = Vector3.zero;
	}

	public Settings settings = new Settings();

	public static float lowerLimitRatio = 0.05f;

	public static Color inertiaGizmosColor = GColor.Alpha(GColor.orange, 0.25f);

	public Mode m_lastMode;

	public Vector3 m_lastDimensions;

	public float m_lastBias;

	public Vector3 m_lastInertiaTensor;

	public Vector3 m_lastInertiaRotation;

	public float m_lastMass;

	public void Apply(Rigidbody rigidbody)
	{
		switch (settings.mode)
		{
		default:
		{
			Collider[] inertiaColliders = settings.inertiaColliders;
			foreach (Collider collider in inertiaColliders)
			{
				if (collider != null)
				{
					collider.enabled = false;
				}
			}
			rigidbody.ResetInertiaTensor();
			break;
		}
		case Mode.Parametric:
			ApplyParametricInertia(rigidbody, settings.chassisDimensions, settings.inertiaBias);
			break;
		case Mode.InertiaColliders:
			ApplyInertiaFromColliders(rigidbody, settings.inertiaColliders);
			break;
		case Mode.Explicit:
			ApplyExplicitInertia(rigidbody, settings.inertiaTensor, settings.inertiaTensorRotation);
			break;
		}
		m_lastMode = settings.mode;
		m_lastDimensions = settings.chassisDimensions;
		m_lastBias = settings.inertiaBias;
		m_lastInertiaTensor = settings.inertiaTensor;
		m_lastInertiaRotation = settings.inertiaTensorRotation;
		m_lastMass = rigidbody.mass;
	}

	public void DoUpdate(Rigidbody rigidbody)
	{
		if (settings.mode != m_lastMode || rigidbody.mass != m_lastMass || (settings.mode == Mode.Parametric && (settings.chassisDimensions != m_lastDimensions || settings.inertiaBias != m_lastBias)) || (settings.mode == Mode.Explicit && (settings.inertiaTensor != m_lastInertiaTensor || settings.inertiaTensorRotation != m_lastInertiaRotation)))
		{
			Apply(rigidbody);
			VerifyInertiaAndShowWarning(rigidbody);
		}
	}

	public static void ApplyInertiaFromColliders(Rigidbody rigidbody, Collider[] inertiaColliders)
	{
		Collider[] solidColliders = ColliderUtility.GetSolidColliders(rigidbody.transform, includeInactive: false);
		rigidbody.inertiaTensor = rigidbody.inertiaTensor;
		Collider[] array = solidColliders;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}
		int num = 0;
		array = inertiaColliders;
		foreach (Collider collider in array)
		{
			if (collider != null && collider.gameObject.activeInHierarchy)
			{
				collider.enabled = true;
				if (collider is MeshCollider && !(collider as MeshCollider).convex)
				{
					Debug.LogWarning(rigidbody.gameObject.name + ": This mesh collider should be convex to have effect in the inertia: " + collider.gameObject.name);
				}
				else
				{
					num++;
				}
			}
		}
		if (num > 0)
		{
			rigidbody.ResetInertiaTensor();
			rigidbody.inertiaTensor = rigidbody.inertiaTensor;
		}
		array = solidColliders;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = true;
		}
		array = inertiaColliders;
		foreach (Collider collider2 in array)
		{
			if (collider2 != null)
			{
				collider2.enabled = false;
			}
		}
		if (num == 0)
		{
			Debug.LogWarning(rigidbody.gameObject.name + ": No suitable inertia colliders found. Inertia left unchanged.\nEnsure the Inertia Colliders list contains enabled colliders.", rigidbody);
		}
	}

	public static void ApplyParametricInertia(Rigidbody rigidbody, Vector3 dimensions, float inertiaBias)
	{
		rigidbody.inertiaTensor = ComputeBoxInertia(dimensions, rigidbody.mass);
		rigidbody.inertiaTensorRotation = Quaternion.AngleAxis(0f - inertiaBias, Vector3.right);
	}

	public static void ApplyExplicitInertia(Rigidbody rigidbody, Vector3 inertiaTensor, Vector3 inertiaTensorRotation)
	{
		if (inertiaTensor.x < 1f)
		{
			inertiaTensor.x = 1f;
		}
		if (inertiaTensor.y < 1f)
		{
			inertiaTensor.y = 1f;
		}
		if (inertiaTensor.z < 1f)
		{
			inertiaTensor.z = 1f;
		}
		rigidbody.inertiaTensor = inertiaTensor;
		rigidbody.inertiaTensorRotation = Quaternion.Euler(inertiaTensorRotation);
	}

	public static void VerifyInertiaAndShowWarning(Rigidbody rigidbody)
	{
		float num = rigidbody.mass * lowerLimitRatio;
		Vector3 inertiaTensor = rigidbody.inertiaTensor;
		if (inertiaTensor.x < num || inertiaTensor.y < num || inertiaTensor.z < num)
		{
			Debug.LogWarning(rigidbody.gameObject.name + ": Inertia is too small and may cause instabilities.\nEnsure the vehicle has proper colliders and/or increase the inertia parameters in the Inertia settings.", rigidbody.gameObject);
		}
	}

	public static Vector3 ComputeBoxInertia(Vector3 dimensions, float mass)
	{
		Vector3 result = Vector3.zero;
		float num = 1f / 12f * mass;
		float num2 = dimensions.x * dimensions.x;
		float num3 = dimensions.y * dimensions.y;
		float num4 = dimensions.z * dimensions.z;
		result.x = num * (num3 + num4);
		result.y = num * (num2 + num4);
		result.z = num * (num2 + num3);
		if (result.sqrMagnitude < 3f)
		{
			result = Vector3.one;
		}
		return result;
	}

	public static void DrawGizmo(Settings settings, Rigidbody rigidbody)
	{
		switch (settings.mode)
		{
		case Mode.InertiaColliders:
			ColliderUtility.DrawColliderGizmos(settings.inertiaColliders, inertiaGizmosColor, includeInactiveInHierarchy: false, includeNonConvex: false);
			break;
		case Mode.Parametric:
		{
			Vector3 pos = rigidbody.transform.TransformPoint(rigidbody.centerOfMass);
			Gizmos.color = inertiaGizmosColor;
			Gizmos.matrix = Matrix4x4.TRS(pos, rigidbody.transform.rotation, Vector3.one);
			Gizmos.DrawCube(Vector3.zero, settings.chassisDimensions);
			Gizmos.DrawWireCube(Vector3.zero, settings.chassisDimensions);
			break;
		}
		}
	}
}

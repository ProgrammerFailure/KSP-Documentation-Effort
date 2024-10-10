using System;
using UnityEngine;

namespace CommNet.Occluders;

public class OccluderHorizonCulling : Occluder
{
	public Transform transform;

	public CelestialBody body;

	public bool useBody;

	public double radiusXRecip;

	public double radiusYRecip;

	public double radiusZRecip;

	public bool anyZero;

	public QuaternionD invRotation;

	public OccluderHorizonCulling(Transform transform, double radiusX, double radiusY, double radiusZ)
	{
		this.transform = transform;
		if (radiusX != 0.0 && radiusY != 0.0 && radiusZ != 0.0)
		{
			anyZero = false;
			radiusXRecip = 1.0 / radiusX;
			radiusYRecip = 1.0 / radiusY;
			radiusZRecip = 1.0 / radiusZ;
			radius = Math.Max(Math.Max(radiusX, radiusY), radiusZ);
			body = transform.GetComponent<CelestialBody>();
			if (body != null)
			{
				useBody = true;
			}
		}
		else
		{
			anyZero = true;
		}
	}

	public override bool InRange(Vector3d source, double distance)
	{
		if (anyZero)
		{
			return false;
		}
		return base.InRange(source, distance);
	}

	public override bool Raycast(Vector3d source, Vector3d dest)
	{
		if (anyZero)
		{
			return false;
		}
		source -= position;
		dest -= position;
		double sqrMagnitude = source.sqrMagnitude;
		double sqrMagnitude2 = dest.sqrMagnitude;
		if (sqrMagnitude > sqrMagnitude2)
		{
			Vector3d vector3d = source;
			source = dest;
			dest = vector3d;
		}
		source = invRotation * source;
		dest = invRotation * dest;
		source.x *= radiusXRecip;
		source.y *= radiusYRecip;
		source.z *= radiusZRecip;
		dest.x *= radiusXRecip;
		dest.y *= radiusYRecip;
		dest.z *= radiusZRecip;
		double num = source.sqrMagnitude - 1.0;
		Vector3d lhs = dest - source;
		double num2 = 0.0 - Vector3d.Dot(lhs, source);
		if (num < 0.0)
		{
			return num2 > 0.0;
		}
		double sqrMagnitude3 = lhs.sqrMagnitude;
		if (num2 > num && num2 * num2 / sqrMagnitude3 > num)
		{
			return true;
		}
		return false;
	}

	public override void Update()
	{
		if (useBody)
		{
			position = body.position;
		}
		else
		{
			position = transform.position;
		}
		invRotation = QuaternionD.Inverse(transform.rotation);
	}
}

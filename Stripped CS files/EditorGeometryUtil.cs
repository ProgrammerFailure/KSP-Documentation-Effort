using UnityEngine;

public static class EditorGeometryUtil
{
	public static bool TestPartBoundsIntersect(Part p1, Part p2)
	{
		return GetPartBoundsSeparation(p1, p2).sqrMagnitude < 0.1f;
	}

	public static bool TestPartBoundsSeparate(Part p1, Part p2, float threshold, out Vector3 gap)
	{
		Vector3 partBoundsSeparation = GetPartBoundsSeparation(p1, p2);
		gap = partBoundsSeparation - partBoundsSeparation.normalized * threshold;
		return partBoundsSeparation.sqrMagnitude > threshold * threshold;
	}

	public static Vector3 GetPartBoundsSeparation(Part p1, Part p2)
	{
		Collider[] partColliders = p1.GetPartColliders();
		Collider[] partColliders2 = p2.GetPartColliders();
		float num = float.MaxValue;
		Vector3 vector = Vector3.zero;
		Vector3 vector2 = Vector3.zero;
		int num2 = partColliders.Length;
		while (num2-- > 0)
		{
			Collider collider = partColliders[num2];
			if (!isValidPartCollider(collider))
			{
				continue;
			}
			for (int num3 = partColliders2.Length - 1; num3 >= 0; num3--)
			{
				Collider collider2 = partColliders2[num3];
				if (!(collider2 == collider) || isValidPartCollider(collider2))
				{
					Vector3 vector3 = collider2.ClosestPointOnBounds(p1.GetReferenceTransform().position);
					Vector3 vector4 = collider.ClosestPointOnBounds(vector3);
					float sqrMagnitude = (vector4 - vector3).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						vector = vector4;
						vector2 = vector3;
					}
				}
			}
		}
		Debug.DrawLine(vector, vector2, Color.green);
		DebugDrawUtil.DrawCrosshairs(vector2, 0.4f, Color.magenta, 0f);
		DebugDrawUtil.DrawCrosshairs(vector, 0.2f, Color.green, 0f);
		return vector - vector2;
	}

	public static bool isValidPartCollider(Collider c)
	{
		if (!c.gameObject.CompareTag("Airlock") && !c.gameObject.CompareTag("Ladder"))
		{
			if (c is WheelCollider)
			{
				return false;
			}
			if (c.gameObject.layer != 0)
			{
				return false;
			}
			return true;
		}
		return false;
	}

	public static Quaternion MirrorRotation(Quaternion input, Transform mirrorRoot, Transform setRoot)
	{
		Vector3 direction = setRoot.InverseTransformDirection(input * Vector3.up);
		Vector3 direction2 = setRoot.InverseTransformDirection(input * Vector3.forward);
		direction.x *= -1f;
		direction2.x *= -1f;
		direction = setRoot.TransformDirection(direction);
		direction2 = setRoot.TransformDirection(direction2);
		Quaternion quaternion = Quaternion.LookRotation(direction2, direction);
		return Quaternion.AngleAxis(180f, direction) * quaternion;
	}

	public static Vector3 MirrorPos(Vector3 input, Transform mirrorRoot, Transform setRoot)
	{
		Vector3 vector = input - mirrorRoot.position;
		Vector3 vector2 = Vector3.ProjectOnPlane(vector, setRoot.up * Vector3.Dot(setRoot.up, vector));
		return mirrorRoot.position + (vector - vector2) + Quaternion.AngleAxis(180f, -setRoot.forward) * vector2;
	}

	public static Vector3 MirrorDirection(Vector3 input, Transform setRoot)
	{
		Vector3 vector = Vector3.ProjectOnPlane(input, setRoot.up * Vector3.Dot(setRoot.up, input));
		return input - vector + Quaternion.AngleAxis(180f, -setRoot.forward) * vector;
	}

	public static float GetPixelDistance(Vector3 sPos1, Vector3 sPos2)
	{
		return Mathf.Sqrt((sPos1 - sPos2).sqrMagnitude);
	}

	public static float GetPixelDistance(Vector3 wPos, Vector3 sPos, Camera refCamera)
	{
		return GetPixelDistance(refCamera.WorldToScreenPoint(wPos), sPos);
	}
}

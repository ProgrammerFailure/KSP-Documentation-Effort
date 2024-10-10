using System;
using UnityEngine;

public class OcclusionData : IComparable<OcclusionData>
{
	public Part part;

	public PartThermalData ptd;

	public double projectedArea;

	public double projectedRadius;

	public double invFineness;

	public double minimumDot;

	public double maximumDot;

	public double centroidDot;

	public double maxWidthDepth;

	public Vector3 boundsCenter;

	public Vector3[] boundsVertices = new Vector3[8];

	public Vector3 projectedCenter;

	public Vector3[] projectedVertices = new Vector3[8];

	public float[] projectedDots = new float[8];

	public Vector2 center;

	public Vector2 minExtents;

	public Vector2 maxExtents;

	public Vector2 extents;

	public OcclusionCone convCone;

	public OcclusionCylinder sunCyl;

	public OcclusionCylinder bodyCyl;

	public OcclusionData(PartThermalData data)
	{
		ptd = data;
		part = ptd.part;
		convCone = new OcclusionCone();
		sunCyl = new OcclusionCylinder();
		bodyCyl = new OcclusionCylinder();
	}

	public void CreateCornerArray()
	{
		Bounds bounds = new Bounds(part.DragCubes.WeightedCenter, part.DragCubes.WeightedSize);
		boundsCenter = bounds.center;
		boundsVertices[0] = bounds.min;
		boundsVertices[1] = bounds.max;
		boundsVertices[2] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
		boundsVertices[3] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
		boundsVertices[4] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
		boundsVertices[5] = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
		boundsVertices[6] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
		boundsVertices[7] = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
	}

	public void Update(Vector3 velocity, bool useDragArea = true)
	{
		if (part == null || part.partTransform == null)
		{
			return;
		}
		CreateCornerArray();
		Matrix4x4 localToWorldMatrix = part.partTransform.localToWorldMatrix;
		Vector3 vector = localToWorldMatrix.MultiplyPoint3x4(boundsCenter);
		centroidDot = Vector3d.Dot(vector, velocity);
		projectedCenter = (Vector3d)vector - centroidDot * (Vector3d)velocity;
		Quaternion quaternion = Quaternion.FromToRotation(velocity, Vector3.up);
		minimumDot = double.MaxValue;
		maximumDot = double.MinValue;
		minExtents = new Vector2(float.MaxValue, float.MaxValue);
		maxExtents = new Vector2(float.MinValue, float.MinValue);
		for (int i = 0; i < 8; i++)
		{
			Vector3 vector2 = localToWorldMatrix.MultiplyPoint3x4(boundsVertices[i]);
			double num = Vector3d.Dot(vector2, velocity);
			projectedVertices[i] = (Vector3d)vector2 - num * (Vector3d)velocity;
			Vector3 vector3 = quaternion * vector2;
			maxExtents.x = Math.Max(maxExtents.x, vector3.x);
			maxExtents.y = Math.Max(maxExtents.y, vector3.z);
			minExtents.x = Math.Min(minExtents.x, vector3.x);
			minExtents.y = Math.Min(minExtents.y, vector3.z);
			if (num < minimumDot)
			{
				minimumDot = num;
			}
			if (num > maximumDot)
			{
				maximumDot = num;
			}
			projectedDots[i] = (float)num;
		}
		extents = (maxExtents - minExtents) * 0.5f;
		center = minExtents + extents;
		if (useDragArea)
		{
			projectedArea = part.DragCubes.CrossSectionalArea;
			invFineness = part.DragCubes.TaperDot;
			maxWidthDepth = part.DragCubes.Depth;
		}
		else
		{
			projectedArea = part.DragCubes.GetCubeAreaDir(velocity);
			invFineness = part.DragCubes.GetCubeCoeffDir(velocity);
			maxWidthDepth = part.DragCubes.GetCubeDepthDir(velocity);
		}
		projectedRadius = Math.Sqrt(projectedArea / Math.PI);
	}

	public double GetConvectionMultVerts(OcclusionCone cone)
	{
		double num = 0.0;
		double num2 = 0.0;
		double radius = cone.radius;
		radius *= radius;
		for (int i = 0; i < 8; i++)
		{
			double num3 = projectedDots[i];
			double shockRadius = cone.GetShockRadius(num3);
			shockRadius *= shockRadius;
			double num4 = (projectedVertices[i] - cone.center).sqrMagnitude;
			if (num4 <= radius && num3 < cone.cylNoseDot)
			{
				num += 0.125;
				num2 += 0.125;
			}
			else if (num4 <= shockRadius && num3 < cone.shockNoseDot)
			{
				num2 += 0.125;
			}
		}
		return 1.0 - num2 + (num2 - num) * cone.shockConvectionTempMult + num * cone.occludeConvectionTempMult;
	}

	public double GetShockStats(OcclusionCone cone, ref double newTempMult, ref double newCoeffMult)
	{
		Vector2 vector = cone.offset + minExtents;
		Vector2 vector2 = cone.offset + maxExtents;
		double num = RectRectIntersection(cone.extents.x, cone.extents.y, vector.x, vector2.x, vector.y, vector2.y);
		if (double.IsNaN(num))
		{
			if (GameSettings.FI_LOG_TEMP_ERROR)
			{
				Debug.LogError("[FlightIntegrator]: For part " + ptd.part.name + ", rectRect is NaN");
			}
			num = 0.0;
		}
		double num2 = num;
		double num3 = 1.0;
		if (num2 < 0.99)
		{
			num3 = AreaOfIntersection(cone.GetShockRadius(centroidDot), projectedRadius, (projectedCenter - cone.center).sqrMagnitude);
		}
		else
		{
			num2 = 1.0;
		}
		double num4 = 1.0 - num3;
		double num5 = num3 - num2;
		newTempMult = num4 * newTempMult + num5 * cone.shockConvectionTempMult + num2 * cone.occludeConvectionTempMult;
		newCoeffMult = num4 * newCoeffMult + num5 * cone.shockConvectionCoeffMult + num2 * cone.occludeConvectionCoeffMult;
		return 1.0 - num2 + num2 * cone.occludeConvectionAreaMult;
	}

	public double GetCylinderOcclusion(OcclusionCylinder cyl)
	{
		Vector2 vector = cyl.offset + minExtents;
		Vector2 vector2 = cyl.offset + maxExtents;
		double num = RectRectIntersection(cyl.extents.x, cyl.extents.y, vector.x, vector2.x, vector.y, vector2.y);
		if (double.IsNaN(num))
		{
			if (GameSettings.FI_LOG_TEMP_ERROR)
			{
				Debug.LogError("[FlightIntegrator]: For part " + ptd.part.name + ", rectRect is NaN");
			}
			num = 0.0;
		}
		return num;
	}

	public double GetCylinderOcclusionVerts(OcclusionCylinder cyl)
	{
		double num = 0.0;
		double radius = cyl.radius;
		radius *= radius;
		for (int i = 0; i < 8; i++)
		{
			double num2 = projectedDots[i];
			if ((double)(projectedVertices[i] - cyl.center).sqrMagnitude <= radius && num2 < cyl.cylNoseDot)
			{
				num += 0.125;
			}
		}
		return num;
	}

	public static double sA(double r, double x, double y)
	{
		if (x < 0.0)
		{
			return 0.0 - sA(r, 0.0 - x, y);
		}
		if (y < 0.0)
		{
			return 0.0 - sA(r, x, 0.0 - y);
		}
		if (x > r)
		{
			x = r;
		}
		if (y > r)
		{
			y = r;
		}
		if (x * x + y * y > r * r)
		{
			double num = r * r * Math.Asin(x / r) + x * Math.Sqrt(r * r - x * x) + r * r * Math.Asin(y / r) + y * Math.Sqrt(r * r - y * y) - r * r * Math.PI * 0.5;
			return num * 0.5;
		}
		return x * y;
	}

	public static double RectRectIntersection(double centralExtentX, double centralExtentY, double minX, double maxX, double minY, double maxY)
	{
		double num = (maxX - minX) * (maxY - minY);
		if (!(maxX < 0.0 - centralExtentX) && !(minX > centralExtentX) && !(maxY < 0.0 - centralExtentY) && !(minY > centralExtentY) && num != 0.0)
		{
			return Math.Max(0.0, Math.Min(centralExtentX, maxX) - Math.Max(0.0 - centralExtentX, minX)) * Math.Max(0.0, Math.Min(centralExtentY, maxY) - Math.Max(0.0 - centralExtentY, minY)) / ((maxX - minX) * (maxY - minY));
		}
		return 0.0;
	}

	public static double AreaOfIntersection(double existingConeRadius, double potentialConeRadius, double sqrDistance)
	{
		double num = existingConeRadius + potentialConeRadius;
		if (sqrDistance >= num * num)
		{
			return 0.0;
		}
		if (potentialConeRadius == 0.0)
		{
			return 1.0;
		}
		if (existingConeRadius == 0.0)
		{
			return 0.0;
		}
		double num2 = Math.Sqrt(sqrDistance);
		if (existingConeRadius >= num2 + potentialConeRadius)
		{
			return 1.0;
		}
		double num3 = potentialConeRadius * potentialConeRadius;
		if (potentialConeRadius >= num2 + existingConeRadius)
		{
			return UtilMath.Clamp01(existingConeRadius * existingConeRadius / num3);
		}
		double num4 = existingConeRadius;
		double num5 = potentialConeRadius;
		if (num5 < num4)
		{
			num4 = potentialConeRadius;
			num5 = existingConeRadius;
		}
		double num6 = num4 * num4;
		double num7 = num5 * num5;
		double num8 = num6 * Math.Acos((sqrDistance + num6 - num7) / (2.0 * num2 * num4));
		double num9 = num7 * Math.Acos((sqrDistance + num7 - num6) / (2.0 * num2 * num5));
		double num10 = 0.5 * Math.Sqrt((0.0 - num2 + num) * (num2 + num4 - num5) * (num2 - num4 + num5) * (num2 + num));
		if (!double.IsNaN(num8) && !double.IsNaN(num9) && !double.IsNaN(num10))
		{
			return (num8 + num9 - num10) / (Math.PI * num3);
		}
		if (GameSettings.FI_LOG_TEMP_ERROR)
		{
			Debug.Log("Occlusion area test is NAN! Args " + existingConeRadius + " " + potentialConeRadius + " " + sqrDistance);
			if (double.IsNaN(num8))
			{
				Debug.Log("part1: " + (sqrDistance + num6 - num7) / (2.0 * num2 * num4));
			}
			if (double.IsNaN(num9))
			{
				Debug.Log("part2: " + (sqrDistance + num7 - num6) / (2.0 * num2 * num5));
			}
			if (double.IsNaN(num10))
			{
				Debug.Log("part3: " + (0.0 - num2 + num) * (num2 + num4 - num5) * (num2 - num4 + num5) * (num2 + num));
			}
		}
		return 0.0;
	}

	public int CompareTo(OcclusionData b)
	{
		if (b.maximumDot < maximumDot)
		{
			return 1;
		}
		if (b.maximumDot == maximumDot)
		{
			return 0;
		}
		return -1;
	}
}

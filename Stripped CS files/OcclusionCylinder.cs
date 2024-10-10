using UnityEngine;

public class OcclusionCylinder
{
	public OcclusionData part;

	public Vector3 center;

	public double radius;

	public Vector2 extents;

	public Vector2 offset;

	public double cylNoseDot;

	public void Setup(OcclusionData part)
	{
		this.part = part;
		center = part.projectedCenter;
		radius = part.projectedRadius;
		offset = -part.center;
		extents = part.extents;
		cylNoseDot = part.maximumDot - part.maxWidthDepth;
	}
}

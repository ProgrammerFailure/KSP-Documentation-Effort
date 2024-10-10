using System;
using UnityEngine;

public class OcclusionCone
{
	public OcclusionData part;

	public Vector3 center;

	public Vector2 extents;

	public Vector2 offset;

	public double radius;

	public double cylNoseDot;

	public double shockNoseDot;

	public double shockAngle;

	public double shockConvectionTempMult;

	public double shockConvectionCoeffMult;

	public double occludeConvectionTempMult;

	public double occludeConvectionCoeffMult;

	public double occludeConvectionAreaMult;

	public static double detachedShockHeatMult = 0.5;

	public static double detachedShockCoeffMult = 1.0;

	public static double detachedBehindShockHeatMult = 0.4;

	public static double detachedBehindShockCoeffMult = 1.0;

	public static double detachedShockMachAngleMult = 0.05;

	public static double detachedShockStartAngle = Math.PI * 49.0 / 100.0;

	public static double detachedShockEndAngle = Math.PI * 9.0 / 25.0;

	public static double obliqueShockAngleMult = 0.8;

	public static double obliqueShockPartAngleMult = 0.25;

	public static double obliqueShockMinAngleMult = 1.05;

	public static double obliqueShockConeHeatMult = 0.75;

	public static double obliqueShockConeCoeffMult = 1.0;

	public static double obliqueShockCylHeatMult = 0.55;

	public static double obliqueShockCylCoeffMult = 1.0;

	public void Setup(OcclusionData part, double sqrtMach, double sqrtMachAngle, double detachAngle)
	{
		this.part = part;
		center = part.projectedCenter;
		radius = part.projectedRadius;
		offset = -part.center;
		extents = part.extents;
		double num = part.ptd.convectionTempMultiplier;
		double num2 = part.ptd.convectionCoeffMultiplier;
		cylNoseDot = part.maximumDot - part.maxWidthDepth;
		double num3 = Math.Asin(part.invFineness);
		if (!(part.invFineness >= 1.0) && num3 <= detachAngle)
		{
			shockNoseDot = part.maximumDot;
			shockAngle = Math.Max(num3 * obliqueShockMinAngleMult, sqrtMachAngle * obliqueShockAngleMult + num3 * obliqueShockPartAngleMult);
			occludeConvectionAreaMult = 1.0;
			if (!(num < detachedShockHeatMult - 0.05) && num2 >= detachedShockCoeffMult - 0.05)
			{
				occludeConvectionTempMult = obliqueShockCylHeatMult;
				shockConvectionTempMult = obliqueShockConeHeatMult;
				occludeConvectionCoeffMult = obliqueShockCylCoeffMult;
				shockConvectionCoeffMult = obliqueShockConeCoeffMult;
			}
			else
			{
				double val = Math.Max(num, detachedBehindShockHeatMult);
				occludeConvectionTempMult = Math.Min(val, obliqueShockCylHeatMult);
				shockConvectionTempMult = Math.Min(val, obliqueShockConeHeatMult);
				val = Math.Max(num2, detachedBehindShockCoeffMult);
				occludeConvectionCoeffMult = Math.Min(val, obliqueShockCylCoeffMult);
				shockConvectionCoeffMult = Math.Min(val, obliqueShockConeCoeffMult);
			}
		}
		else
		{
			shockNoseDot = part.maximumDot + radius * part.invFineness;
			shockAngle = UtilMath.Lerp(detachedShockEndAngle, detachedShockStartAngle, sqrtMach * detachedShockMachAngleMult);
			occludeConvectionTempMult = 0.0;
			occludeConvectionAreaMult = 0.0;
			if (!(num < detachedShockHeatMult - 0.05) && num2 >= detachedShockCoeffMult - 0.05)
			{
				num = detachedShockHeatMult;
				num2 = detachedShockCoeffMult;
				shockConvectionTempMult = detachedBehindShockHeatMult;
				shockConvectionCoeffMult = detachedBehindShockCoeffMult;
			}
			else
			{
				num = (shockConvectionTempMult = detachedBehindShockHeatMult);
				num2 = (shockConvectionCoeffMult = detachedBehindShockCoeffMult);
			}
		}
		part.ptd.convectionTempMultiplier = num;
		part.ptd.convectionCoeffMultiplier = num2;
	}

	public double GetShockRadius(double dot)
	{
		double num = shockNoseDot - dot;
		return radius + num * Math.Tan(shockAngle);
	}
}

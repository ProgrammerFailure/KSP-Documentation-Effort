using System;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Terrain/Flatten Area Tangential")]
public class PQSMod_FlattenAreaTangential : PQSMod
{
	public double outerRadius;

	public double innerRadius;

	public Vector3 position;

	public double flattenTo;

	public double smoothStart;

	public double smoothEnd;

	public bool DEBUG_showColors;

	public double angleInner;

	public double angleOuter;

	public double angleQuadInclusion;

	public double angleDelta;

	public bool quadActive;

	public Vector3d posNorm;

	public double testAngle;

	public double aDelta;

	public double flattenToRadius;

	public double vHeight;

	public double ct2;

	public double ct3;

	public void Reset()
	{
		outerRadius = 1000.0;
		innerRadius = 100.0;
		position = Vector3.back;
	}

	public override void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.MeshColorChannel | GClass4.ModiferRequirements.MeshCustomNormals;
		posNorm = position.normalized;
		angleInner = Math.Atan(innerRadius / sphere.radius);
		angleOuter = Math.Atan(outerRadius / sphere.radius);
		angleQuadInclusion = angleOuter * 2.0;
		angleDelta = angleOuter - angleInner;
		flattenToRadius = sphere.radius + flattenTo;
	}

	public override void OnQuadPreBuild(GClass3 quad)
	{
		testAngle = Math.Acos(Vector3d.Dot(quad.positionPlanetRelative, posNorm));
		if (testAngle < angleQuadInclusion)
		{
			quadActive = true;
		}
		else
		{
			quadActive = false;
		}
	}

	public override void OnQuadBuilt(GClass3 quad)
	{
		quadActive = true;
	}

	public override void OnVertexBuildHeight(GClass4.VertexBuildData vbData)
	{
		if (!quadActive)
		{
			return;
		}
		if (DEBUG_showColors)
		{
			vbData.vertColor = Color.green;
		}
		testAngle = Math.Acos(Vector3d.Dot(vbData.directionFromCenter, posNorm));
		vHeight = flattenToRadius / Math.Cos(testAngle);
		if (!(testAngle < angleOuter))
		{
			return;
		}
		if (testAngle < angleInner)
		{
			vbData.vertHeight = vHeight;
			if (DEBUG_showColors)
			{
				vbData.vertColor = Color.yellow;
			}
			return;
		}
		aDelta = (testAngle - angleInner) / angleDelta;
		vbData.vertHeight = CubicHermite(vHeight, vbData.vertHeight, smoothStart, smoothEnd, aDelta);
		if (DEBUG_showColors)
		{
			vbData.vertColor = Color.Lerp(Color.blue, Color.yellow, (float)(1.0 - aDelta));
		}
	}

	public double CubicHermite(double start, double end, double startTangent, double endTangent, double t)
	{
		ct2 = t * t;
		ct3 = ct2 * t;
		return start * (2.0 * ct3 - 3.0 * ct2 + 1.0) + startTangent * (ct3 - 2.0 * ct2 + t) + end * (-2.0 * ct3 + 3.0 * ct2) + endTangent * (ct3 - ct2);
	}
}

using System;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Terrain/Flatten Area")]
public class PQSMod_FlattenArea : PQSMod
{
	public double outerRadius;

	public double innerRadius;

	public Vector3 position;

	public double flattenTo;

	public bool removeScatter;

	public bool DEBUG_showColors;

	public double flattenToRadius;

	public double angleInner;

	public double angleOuter;

	public double angleQuadInclusion;

	public double angleDelta;

	public bool quadActive;

	public Vector3d posNorm;

	public double testAngle;

	public double aDelta;

	public double smoothStart;

	public double smoothEnd;

	public bool useLatLon;

	public string bodyName;

	public CelestialBody body;

	public double lat;

	public double lon;

	public double alt;

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
		if (useLatLon)
		{
			Planetarium.CelestialFrame cf = default(Planetarium.CelestialFrame);
			Planetarium.CelestialFrame.SetFrame(0.0, 0.0, 0.0, ref cf);
			Vector3d vector3d = LatLon.GetSurfaceNVector(cf, lat, lon) * (sphere.radius + alt);
			position = vector3d;
			if (base.gameObject.GetComponent<PQSCity>() == null && base.gameObject.GetComponent<PQSCity2>() == null)
			{
				base.transform.position = position;
			}
		}
		if (modEnabled)
		{
			posNorm = position.normalized;
			angleInner = ((sphere != null) ? Math.Atan(innerRadius / sphere.radius) : Math.Atan(innerRadius));
			angleOuter = ((sphere != null) ? Math.Atan(outerRadius / sphere.radius) : Math.Atan(outerRadius));
			angleQuadInclusion = angleOuter * 2.0;
			angleDelta = angleOuter - angleInner;
			flattenToRadius = ((sphere != null) ? (sphere.radius + flattenTo) : flattenTo);
		}
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
		if (!overrideQuadBuildCheck && !quadActive)
		{
			return;
		}
		testAngle = Math.Acos(Vector3d.Dot(vbData.directionFromCenter, posNorm));
		if (!(testAngle < angleQuadInclusion))
		{
			return;
		}
		if (removeScatter)
		{
			vbData.allowScatter = false;
		}
		if (DEBUG_showColors)
		{
			vbData.vertColor = Color.green;
		}
		if (!(testAngle < angleOuter))
		{
			return;
		}
		if (testAngle < angleInner)
		{
			vbData.vertHeight = flattenToRadius;
			if (DEBUG_showColors)
			{
				vbData.vertColor = Color.yellow;
			}
			return;
		}
		aDelta = (testAngle - angleInner) / angleDelta;
		vbData.vertHeight = CubicHermite(flattenToRadius, vbData.vertHeight, smoothStart, smoothEnd, aDelta);
		if (DEBUG_showColors)
		{
			vbData.vertColor = Color.Lerp(Color.blue, Color.yellow, (float)aDelta);
		}
	}

	public static double Lerp(double v2, double v1, double dt)
	{
		return v1 * dt + v2 * (1.0 - dt);
	}

	public double CubicHermite(double start, double end, double startTangent, double endTangent, double t)
	{
		ct2 = t * t;
		ct3 = ct2 * t;
		return start * (2.0 * ct3 - 3.0 * ct2 + 1.0) + startTangent * (ct3 - 2.0 * ct2 + t) + end * (-2.0 * ct3 + 3.0 * ct2) + endTangent * (ct3 - ct2);
	}
}

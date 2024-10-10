using System;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Terrain/Decal")]
public class PQSMod_MapDecal : PQSMod
{
	public double radius;

	public Vector3 position;

	public float angle;

	public MapSO heightMap;

	public double heightMapDeformity;

	public bool cullBlack;

	public bool useAlphaHeightSmoothing;

	public bool absolute;

	public float absoluteOffset;

	public MapSO colorMap;

	public float smoothHeight;

	public float smoothColor;

	public bool removeScatter;

	public bool DEBUG_HighlightInclusion;

	public double inclusionAngle;

	public bool quadActive;

	public bool vertActive;

	public Vector3d posNorm;

	public double quadAngle;

	public Vector3d vertRot;

	public Quaternion rot;

	public float u;

	public float v;

	public Color c1;

	public Color c2;

	public MapSO.HeightAlpha ha;

	public float smoothCR;

	public float smoothC1M;

	public float smoothHR;

	public float smoothH1M;

	public float smoothU;

	public float smoothV;

	public float smoothFactor;

	public bool buildHeight;

	public void Reset()
	{
		radius = 100.0;
		position = Vector3.back;
		angle = 0f;
		heightMapDeformity = 10.0;
		absolute = false;
		absoluteOffset = 0f;
		smoothHeight = 0.125f;
		smoothColor = 0.125f;
		removeScatter = false;
	}

	public override void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.MeshColorChannel | GClass4.ModiferRequirements.MeshCustomNormals;
		posNorm = position.normalized;
		rot = Quaternion.AngleAxis(angle, Vector3.up) * Quaternion.FromToRotation(posNorm, Vector3.up);
		inclusionAngle = Math.Atan(radius / sphere.radius) * 4.0;
		smoothC1M = 1f - smoothColor;
		smoothCR = 1f / smoothColor;
		smoothH1M = 1f - smoothHeight;
		smoothHR = 1f / smoothHeight;
	}

	public override void OnQuadPreBuild(GClass3 quad)
	{
		quadAngle = Math.Acos(Vector3d.Dot(quad.positionPlanetRelative, posNorm));
		if (quadAngle < inclusionAngle)
		{
			quadActive = true;
			buildHeight = true;
		}
		else
		{
			quadActive = false;
			buildHeight = true;
		}
	}

	public override void OnVertexBuildHeight(GClass4.VertexBuildData vbData)
	{
		if (!quadActive && buildHeight)
		{
			return;
		}
		if (sphere.isBuildingMaps)
		{
			quadAngle = Math.Acos(Vector3d.Dot(vbData.directionFromCenter, posNorm));
			if (quadAngle > inclusionAngle)
			{
				return;
			}
		}
		vertRot = rot * vbData.directionFromCenter;
		u = (float)((vertRot.x * sphere.radius / radius + 1.0) * 0.5);
		v = (float)((vertRot.z * sphere.radius / radius + 1.0) * 0.5);
		if (!(u < 0f) && !(u > 1f) && !(v < 0f) && v <= 1f)
		{
			if (buildHeight || sphere.isBuildingMaps)
			{
				vertActive = true;
			}
			if (!(heightMap != null))
			{
				return;
			}
			ha = heightMap.GetPixelHeightAlpha(u, v);
			smoothFactor = GetHeightSmoothing(u, v);
			if (useAlphaHeightSmoothing)
			{
				smoothFactor *= ha.alpha;
			}
			if (!(smoothFactor > 0f))
			{
				return;
			}
			if (removeScatter)
			{
				vbData.allowScatter = false;
			}
			if (cullBlack)
			{
				if (ha.height > 0f)
				{
					if (absolute)
					{
						vbData.vertHeight = Lerp(vbData.vertHeight, sphere.radius + (double)absoluteOffset + heightMapDeformity * (double)ha.height, smoothFactor);
					}
					else
					{
						vbData.vertHeight = Lerp(vbData.vertHeight, vbData.vertHeight + heightMapDeformity * (double)ha.height, smoothFactor);
					}
				}
			}
			else if (absolute)
			{
				vbData.vertHeight = Lerp(vbData.vertHeight, sphere.radius + (double)absoluteOffset + heightMapDeformity * (double)ha.height, smoothFactor);
			}
			else
			{
				vbData.vertHeight = Lerp(vbData.vertHeight, vbData.vertHeight + heightMapDeformity * (double)ha.height, smoothFactor);
			}
		}
		else
		{
			vertActive = false;
		}
	}

	public float GetHeightSmoothing(float u, float v)
	{
		if (u < smoothHeight)
		{
			smoothU = u * smoothHR;
		}
		else if (u > smoothH1M)
		{
			smoothU = (1f - u) * smoothHR;
		}
		else
		{
			smoothU = 1f;
		}
		if (v < smoothHeight)
		{
			smoothV = v * smoothHR;
		}
		else if (v > smoothH1M)
		{
			smoothV = (1f - v) * smoothHR;
		}
		else
		{
			smoothV = 1f;
		}
		return Mathf.Min(smoothU, smoothV);
	}

	public float GetColorSmoothing(float u, float v)
	{
		if (u < smoothColor)
		{
			smoothU = u * smoothCR;
		}
		else if (u > smoothC1M)
		{
			smoothU = (1f - u) * smoothCR;
		}
		else
		{
			smoothU = 1f;
		}
		if (v < smoothColor)
		{
			smoothV = v * smoothCR;
		}
		else if (v > smoothC1M)
		{
			smoothV = (1f - v) * smoothCR;
		}
		else
		{
			smoothV = 1f;
		}
		return Mathf.Min(smoothU, smoothV);
	}

	public override void OnVertexBuild(GClass4.VertexBuildData vbData)
	{
		if (!vertActive)
		{
			if (DEBUG_HighlightInclusion && quadActive)
			{
				vbData.vertColor = Color.red;
			}
			return;
		}
		if (colorMap != null)
		{
			c1 = colorMap.GetPixelColor(u, v);
			smoothFactor = c1.a * GetColorSmoothing(u, v);
			if (smoothFactor > 0f)
			{
				c2.r = Mathf.Lerp(vbData.vertColor.r, c1.r, smoothFactor);
				c2.g = Mathf.Lerp(vbData.vertColor.g, c1.g, smoothFactor);
				c2.b = Mathf.Lerp(vbData.vertColor.b, c1.b, smoothFactor);
				c2.a = vbData.vertColor.a;
				vbData.vertColor = c2;
			}
		}
		if (DEBUG_HighlightInclusion)
		{
			vbData.vertColor = Color.green;
		}
		vertActive = false;
	}

	public override void OnQuadBuilt(GClass3 quad)
	{
		quadActive = true;
		buildHeight = false;
	}

	public static double Lerp(double v2, double v1, double dt)
	{
		return v1 * dt + v2 * (1.0 - dt);
	}
}

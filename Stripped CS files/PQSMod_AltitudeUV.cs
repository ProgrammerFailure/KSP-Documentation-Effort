using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Material/Altitude to UV3")]
public class PQSMod_AltitudeUV : PQSMod
{
	public double atmosphereHeight;

	public double oceanDepth;

	public bool invert;

	public double h;

	public void Reset()
	{
		atmosphereHeight = 10000.0;
		oceanDepth = 5000.0;
	}

	public override void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.MeshUV3;
	}

	public override void OnVertexBuild(GClass4.VertexBuildData vbData)
	{
		h = vbData.vertHeight - sphere.radius;
		if (h >= 0.0)
		{
			h /= atmosphereHeight;
		}
		else
		{
			h /= oceanDepth;
		}
		h = UtilMath.Clamp(h, -1.0, 1.0);
		if (invert)
		{
			vbData.u3 = 1.0 - h;
		}
		else
		{
			vbData.u3 = h;
		}
	}
}

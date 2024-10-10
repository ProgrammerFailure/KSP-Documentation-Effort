using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Material/Altitude to Alpha")]
public class PQSMod_AltitudeAlpha : PQSMod
{
	public double atmosphereDepth;

	public bool invert;

	public double h;

	public void Reset()
	{
		atmosphereDepth = 10000.0;
	}

	public override void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.MeshColorChannel;
	}

	public override void OnVertexBuild(GClass4.VertexBuildData vbData)
	{
		h = (vbData.vertHeight - sphere.radius) / atmosphereDepth;
		if (invert)
		{
			vbData.vertColor.a = (float)(1.0 - h);
		}
		else
		{
			vbData.vertColor.a = (float)h;
		}
	}
}

using System;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Material/Tangent Texture Ranges")]
public class PQSMod_TangentTextureRanges : PQSMod
{
	public double modulo = 1024.0;

	public double lowStart;

	public double lowEnd = 100.0;

	public double highStart = 1000.0;

	public double highEnd = 10000.0;

	public static float[] tangentX;

	public static double height;

	public static double low;

	public static double med;

	public static double high;

	public static double t;

	public override void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.MeshAssignTangents;
		tangentX = new float[GClass4.cacheVertCount];
	}

	public override void OnVertexBuild(GClass4.VertexBuildData vbData)
	{
		low = 1.0 - SmoothStep(lowStart, lowEnd, vbData.vertHeight);
		high = SmoothStep(highStart, highEnd, vbData.vertHeight);
		med = 1.0 - low - high;
		low = Math.Round(low * modulo);
		med = Math.Round(med * modulo) * 2.0;
		high = Math.Round(high * modulo) * 3.0;
		tangentX[vbData.vertIndex] = (float)(high + med + low);
	}

	public override void OnMeshBuild()
	{
		for (int i = 0; i < GClass4.cacheVertCount; i++)
		{
			GClass4.cacheTangents[i].x = tangentX[i];
		}
	}

	public static double SmoothStep(double a, double b, double x)
	{
		t = (x - a) / (b - a);
		if (t < 0.0)
		{
			t = 0.0;
		}
		else if (t > 1.0)
		{
			t = 1.0;
		}
		return t * t * (3.0 - 2.0 * t);
	}
}

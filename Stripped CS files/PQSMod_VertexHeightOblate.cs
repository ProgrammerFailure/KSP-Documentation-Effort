using System;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Height/Oblate")]
public class PQSMod_VertexHeightOblate : PQSMod
{
	public double height;

	public double pow;

	public double a;

	public void Reset()
	{
		height = 1000.0;
		pow = 4.0;
	}

	public override void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.UVSphereCoords;
	}

	public override void OnVertexBuildHeight(GClass4.VertexBuildData data)
	{
		a = Math.Sin(Math.PI * data.v);
		a = Math.Pow(a, pow);
		data.vertHeight += a * height;
	}
}

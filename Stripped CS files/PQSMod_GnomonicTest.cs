using System;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Gnomonic Test")]
public class PQSMod_GnomonicTest : PQSMod
{
	public double x;

	public double y;

	public double xu;

	public double yu;

	public float u;

	public float v;

	public Vector3d planePos;

	public override void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.VertexGnomonicMapCoords | GClass4.ModiferRequirements.MeshColorChannel;
	}

	public void OnVertexBuild2(GClass4.VertexBuildData data)
	{
		planePos = data.buildQuad.positionPlanePosition;
		x = 0.0;
		y = 0.0;
		xu = data.buildQuad.scalePlaneRelative * GClass4.cacheUVQuad[data.vertIndex].x;
		yu = data.buildQuad.scalePlaneRelative * GClass4.cacheUVQuad[data.vertIndex].y;
		switch (data.buildQuad.plane)
		{
		case GClass4.QuadPlane.const_0:
			x = (planePos.z + 1.0) * 0.5 + yu;
			y = (planePos.y - 1.0) * -0.5 + xu;
			break;
		case GClass4.QuadPlane.const_1:
			x = (planePos.z + 1.0) * 0.5 + yu;
			y = (planePos.y + 1.0) * 0.5 + xu;
			break;
		case GClass4.QuadPlane.const_2:
			x = (planePos.x + 1.0) * 0.5 + xu;
			y = (planePos.z + 1.0) * 0.5 + yu;
			break;
		case GClass4.QuadPlane.const_3:
			x = (planePos.x + 1.0) * 0.5 + xu;
			y = (planePos.z - 1.0) * -0.5 + yu;
			break;
		case GClass4.QuadPlane.const_4:
			x = (planePos.x + 1.0) * 0.5 + xu;
			y = (planePos.y - 1.0) * -0.5 + yu;
			break;
		case GClass4.QuadPlane.const_5:
			x = (planePos.x + 1.0) * 0.5 + xu;
			y = (planePos.y + 1.0) * 0.5 + yu;
			break;
		}
		data.vertColor = new Color((float)x, (float)y, 0f);
	}

	public override void OnVertexBuild(GClass4.VertexBuildData data)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		if (data.gnomonicUVs[0].acceptable)
		{
			num += (float)data.gnomonicUVs[0].gnomonicU;
			num2 += (float)data.gnomonicUVs[0].gnomonicV;
		}
		if (data.gnomonicUVs[1].acceptable)
		{
			num += (float)data.gnomonicUVs[1].gnomonicU;
			num2 += (float)data.gnomonicUVs[1].gnomonicV;
		}
		if (data.gnomonicUVs[2].acceptable)
		{
			num2 += (float)data.gnomonicUVs[2].gnomonicU;
			num3 += (float)data.gnomonicUVs[2].gnomonicV;
		}
		if (data.gnomonicUVs[3].acceptable)
		{
			num2 += (float)data.gnomonicUVs[3].gnomonicU;
			num3 += (float)data.gnomonicUVs[3].gnomonicV;
		}
		if (data.gnomonicUVs[4].acceptable)
		{
			num3 += (float)data.gnomonicUVs[4].gnomonicU;
			num += (float)data.gnomonicUVs[4].gnomonicV;
		}
		if (data.gnomonicUVs[5].acceptable)
		{
			num3 += (float)data.gnomonicUVs[5].gnomonicU;
			num += (float)data.gnomonicUVs[5].gnomonicV;
		}
		data.vertColor = new Color(Mathf.Min(num, 1f), Mathf.Min(num2, 1f), Mathf.Min(num3, 1f));
	}

	public static void GetGnomonicMapCoords(Vector3d radial, out GClass4.QuadPlane plane, out double u, out double v)
	{
		double num = radial.x;
		double num2 = radial.y;
		double z = radial.z;
		u = 0.0;
		v = 0.0;
		plane = GClass4.QuadPlane.const_0;
		if (num >= num2 && num >= z)
		{
			double num3 = 0.0 - z;
			double num4 = 0.0 - num2;
			double num5 = Math.Abs(num);
			v = (num3 / num5 + 1.0) / 2.0;
			u = 1.0 - (num4 / num5 + 1.0) / 2.0;
			plane = GClass4.QuadPlane.const_0;
			if (u >= 0.0 && u <= 1.0 && v >= 0.0 && v <= 1.0)
			{
				return;
			}
		}
		if (num <= num2 && num <= z)
		{
			double num3 = z;
			double num4 = 0.0 - num2;
			double num5 = Math.Abs(num);
			v = 1.0 - (num3 / num5 + 1.0) / 2.0;
			u = (num4 / num5 + 1.0) / 2.0;
			plane = GClass4.QuadPlane.const_1;
			if (u >= 0.0 && u <= 1.0 && v >= 0.0 && v <= 1.0)
			{
				return;
			}
		}
		if (num2 >= z && num2 >= num)
		{
			double num3 = num;
			double num4 = z;
			double num5 = Math.Abs(num2);
			u = 1.0 - (num3 / num5 + 1.0) / 2.0;
			v = 1.0 - (num4 / num5 + 1.0) / 2.0;
			plane = GClass4.QuadPlane.const_2;
			if (u >= 0.0 && u <= 1.0 && v >= 0.0 && v <= 1.0)
			{
				return;
			}
		}
		if (num2 <= z && num2 <= num)
		{
			double num3 = num;
			double num4 = 0.0 - z;
			double num5 = Math.Abs(num2);
			u = 1.0 - (num3 / num5 + 1.0) / 2.0;
			v = 1.0 - (num4 / num5 + 1.0) / 2.0;
			plane = GClass4.QuadPlane.const_3;
			if (u >= 0.0 && u <= 1.0 && v >= 0.0 && v <= 1.0)
			{
				return;
			}
		}
		if (z >= num2 && z >= num)
		{
			double num3 = num;
			double num4 = 0.0 - num2;
			double num5 = Math.Abs(z);
			u = 1.0 - (num3 / num5 + 1.0) / 2.0;
			v = 1.0 - (num4 / num5 + 1.0) / 2.0;
			plane = GClass4.QuadPlane.const_4;
			if (u >= 0.0 && u <= 1.0 && v >= 0.0 && v <= 1.0)
			{
				return;
			}
		}
		if (z <= num2 && z <= num)
		{
			double num3 = 0.0 - num;
			double num4 = 0.0 - num2;
			double num5 = Math.Abs(z);
			u = (num3 / num5 + 1.0) / 2.0;
			v = (num4 / num5 + 1.0) / 2.0;
			plane = GClass4.QuadPlane.const_5;
			if (u >= 0.0 && u <= 1.0 && !(v >= 0.0))
			{
			}
		}
	}

	public void Update()
	{
		if (Input.GetKey(KeyCode.F10))
		{
			for (int i = 0; i < GClass4.cacheVertCount; i++)
			{
				Debug.Log(i + " " + GClass4.cacheUVs[i]);
			}
		}
	}
}

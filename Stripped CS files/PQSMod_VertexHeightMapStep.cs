using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Height/Map (Coastal Step)")]
public class PQSMod_VertexHeightMapStep : PQSMod
{
	public Texture2D heightMap;

	public double heightMapDeformity;

	public double heightMapOffset;

	public bool scaleDeformityByRadius;

	public double coastHeight;

	public double heightDeformity;

	public double h;

	public void Reset()
	{
		heightMapDeformity = 10.0;
		heightMapOffset = 0.0;
		coastHeight = 0.10000000149011612;
	}

	public override void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.VertexMapCoords | GClass4.ModiferRequirements.MeshCustomNormals;
		if (scaleDeformityByRadius)
		{
			heightDeformity = (float)(sphere.radius * heightMapDeformity);
		}
		else
		{
			heightDeformity = heightMapDeformity;
		}
	}

	public override void OnVertexBuildHeight(GClass4.VertexBuildData data)
	{
		h = sphere.BilinearInterpFloatMap(heightMap);
		if (h >= coastHeight)
		{
			data.vertHeight += heightMapOffset + h * heightDeformity;
		}
		else
		{
			data.vertHeight += heightMapOffset;
		}
	}

	public override double GetVertexMaxHeight()
	{
		return heightMapOffset + heightDeformity;
	}

	public override double GetVertexMinHeight()
	{
		return heightMapOffset;
	}

	public static double Lerp(double v2, double v1, double dt)
	{
		return v1 * dt + v2 * (1.0 - dt);
	}
}

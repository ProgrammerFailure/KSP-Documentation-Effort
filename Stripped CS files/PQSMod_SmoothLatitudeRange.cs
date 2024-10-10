using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Height/Smooth Latitude Range")]
public class PQSMod_SmoothLatitudeRange : PQSMod
{
	public PQSLandControl.LerpRange latitudeRange;

	public double smoothToAltitude;

	public double alt;

	public double smooth;

	public double result;

	public void Reset()
	{
		latitudeRange = new PQSLandControl.LerpRange();
		smoothToAltitude = 1000.0;
	}

	public override void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.MeshCustomNormals;
		latitudeRange.Setup();
	}

	public override void OnVertexBuildHeight(GClass4.VertexBuildData data)
	{
		smooth = latitudeRange.Lerp(sphere.sy);
		if (smooth != 0.0)
		{
			alt = data.vertHeight - sphere.radius;
			result = alt * (1.0 - smooth) + smoothToAltitude * smooth;
			data.vertHeight = sphere.radius + result;
		}
	}

	public override double GetVertexMaxHeight()
	{
		return smoothToAltitude;
	}

	public override double GetVertexMinHeight()
	{
		return 0.0;
	}
}

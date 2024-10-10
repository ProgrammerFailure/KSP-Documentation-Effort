using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Height/Offset")]
public class PQSMod_VertexHeightOffset : PQSMod
{
	public double offset;

	public void Reset()
	{
		offset = 0.0;
	}

	public override void OnVertexBuildHeight(GClass4.VertexBuildData data)
	{
		data.vertHeight += offset;
	}
}

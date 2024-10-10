using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Material/Quad Standard UV")]
public class PQSMod_UV_Quad : PQSMod
{
	public override void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.UVQuadCoords;
	}
}

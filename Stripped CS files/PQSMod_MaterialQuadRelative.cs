using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Material/Quad Projective UV")]
public class PQSMod_MaterialQuadRelative : PQSMod
{
	public static int shaderPropertyUpMatrix;

	public static int shaderPropertyLocalMatrix;

	public static int shaderPropertySubDiv;

	public Matrix4x4 matUp;

	public Matrix4x4 matW2L;

	public override void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.UniqueMaterialInstances;
		shaderPropertyUpMatrix = Shader.PropertyToID("upMatrix");
		shaderPropertyLocalMatrix = Shader.PropertyToID("localMatrix");
		shaderPropertySubDiv = Shader.PropertyToID("_subdiv");
	}

	public override void OnQuadBuilt(GClass3 quad)
	{
		matUp = Matrix4x4.TRS(Vector3.zero, Quaternion.FromToRotation(Vector3.up, quad.positionPlanetRelative), Vector3.one);
		matW2L = Matrix4x4.TRS(quad.positionPlanet, Quaternion.FromToRotation(Vector3.up, quad.positionPlanetRelative), quad.quadScale);
		quad.GetComponent<Renderer>().material.SetMatrix(shaderPropertyUpMatrix, matUp);
		quad.GetComponent<Renderer>().material.SetMatrix(shaderPropertyLocalMatrix, matW2L);
		quad.GetComponent<Renderer>().material.SetFloat(shaderPropertySubDiv, quad.subdivision);
	}
}

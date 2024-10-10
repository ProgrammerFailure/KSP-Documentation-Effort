using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Material/Shader - Set Direction")]
public class PQSMod_MaterialSetDirection : PQSMod
{
	public Transform target;

	public string valueName;

	public void Reset()
	{
		valueName = "_sunLightDirection";
	}

	public override void OnUpdateFinished()
	{
		if (target != null)
		{
			Shader.SetGlobalVector(valueName, sphere.transform.InverseTransformDirection(target.forward).normalized);
		}
		else if (Sun.Instance != null)
		{
			Shader.SetGlobalVector(valueName, sphere.transform.InverseTransformDirection(Sun.Instance.transform.forward).normalized);
		}
	}
}

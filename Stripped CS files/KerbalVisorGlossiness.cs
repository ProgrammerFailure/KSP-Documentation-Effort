using UnityEngine;

public class KerbalVisorGlossiness : MonoBehaviour
{
	public Material visorMaterial;

	public void Start()
	{
		visorMaterial = base.gameObject.GetComponent<SkinnedMeshRenderer>().material;
		visorMaterial.SetFloat("_GlossMapScale", 0.75f);
	}
}

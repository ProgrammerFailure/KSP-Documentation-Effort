using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ShaderTimeOffset : MonoBehaviour
{
	public float frequency;

	public string valueName;

	[HideInInspector]
	public Material mat;

	public void Reset()
	{
		frequency = 1f;
		valueName = "_Offset";
	}

	public void Start()
	{
		mat = GetComponent<Renderer>().sharedMaterial;
	}

	public void Update()
	{
		mat.SetFloat(valueName, frequency * Time.realtimeSinceStartup);
	}
}

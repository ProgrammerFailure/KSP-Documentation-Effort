using UnityEngine;

namespace ns14;

[RequireComponent(typeof(Camera))]
public class CameraShaderReplacement : MonoBehaviour
{
	[SerializeField]
	public Shader shader;

	[SerializeField]
	public string replacementTag = string.Empty;

	[SerializeField]
	public Camera cam;

	public void Reset()
	{
		cam = base.gameObject.GetComponent<Camera>();
	}

	public void Start()
	{
		SetShader();
	}

	[ContextMenu("Reset Shader")]
	public void SetShader()
	{
		cam = base.gameObject.GetComponent<Camera>();
		cam.SetReplacementShader(shader, replacementTag);
	}
}

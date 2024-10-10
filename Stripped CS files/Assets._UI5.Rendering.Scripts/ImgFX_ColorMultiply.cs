using UnityEngine;

namespace Assets._UI5.Rendering.Scripts;

public class ImgFX_ColorMultiply : MonoBehaviour
{
	public Color imgColor = Color.white;

	public Material mat;

	public int dstTexID;

	[SerializeField]
	public Shader shader;

	public void Awake()
	{
		mat = new Material(shader);
		dstTexID = Shader.PropertyToID("_DstTex");
	}

	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (imgColor == Color.white)
		{
			Graphics.Blit(source, destination);
			return;
		}
		mat.color = imgColor;
		mat.SetTexture(dstTexID, destination);
		Graphics.Blit(source, destination, mat);
	}
}

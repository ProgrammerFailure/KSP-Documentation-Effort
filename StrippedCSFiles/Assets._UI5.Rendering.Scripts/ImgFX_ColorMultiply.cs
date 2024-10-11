using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets._UI5.Rendering.Scripts;

public class ImgFX_ColorMultiply : MonoBehaviour
{
	public Color imgColor;

	private Material mat;

	private int dstTexID;

	[SerializeField]
	private Shader shader;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ImgFX_ColorMultiply()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		throw null;
	}
}

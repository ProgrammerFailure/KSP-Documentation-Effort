using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Noise")]
public class NoiseEffect : MonoBehaviour
{
	public bool monochrome;

	private bool rgbFallback;

	public float grainIntensityMin;

	public float grainIntensityMax;

	public float grainSize;

	public float scratchIntensityMin;

	public float scratchIntensityMax;

	public float scratchFPS;

	public float scratchJitter;

	public Texture grainTexture;

	public Texture scratchTexture;

	public Shader shaderRGB;

	public Shader shaderYUV;

	private Material m_MaterialRGB;

	private Material m_MaterialYUV;

	private float scratchTimeLeft;

	private float scratchX;

	private float scratchY;

	protected Material material
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public NoiseEffect()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SanitizeParameters()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		throw null;
	}
}

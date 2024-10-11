using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteInEditMode]
public class ConeVolumeOcclusion : VolumetricObjectBase
{
	public float coneHeight;

	public float coneAngle;

	public float startOffset;

	public int occlusionTextureSize;

	private float previousConeHeight;

	private float previousConeAngle;

	private float previousStartOffset;

	private int previousOcclusionTextureSize;

	private Transform coneCameraTransform;

	private Camera coneCamera;

	private RenderTexture coneCameraRT;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConeVolumeOcclusion()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void CleanUp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void PopulateShaderName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool HasChanged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void SetChangedValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void UpdateVolume()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateDepthCamera()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private RenderTexture ConeRenderTexture()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupCamera()
	{
		throw null;
	}
}

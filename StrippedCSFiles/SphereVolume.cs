using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteInEditMode]
public class SphereVolume : VolumetricObjectBase
{
	public float radius;

	private float previousRadius;

	public float Contrast;

	private float previousContrast;

	public float Brightness;

	private float previousBrightness;

	public float radiusPower;

	private float previousRadiusPower;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SphereVolume()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnEnable()
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
}

using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteInEditMode]
public class ConeVolume : VolumetricObjectBase
{
	public float coneHeight;

	public float coneAngle;

	public float startOffset;

	private float previousConeHeight;

	private float previousConeAngle;

	private float previousStartOffset;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConeVolume()
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

using System.Runtime.CompilerServices;
using UnityEngine;

public class FloatArrayMaterialProperty : MaterialPropertyExtensions.MaterialProperty
{
	private readonly float[] Values;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FloatArrayMaterialProperty(string name, float[] values)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Apply(ref MaterialPropertyBlock block)
	{
		throw null;
	}
}

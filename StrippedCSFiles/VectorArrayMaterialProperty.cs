using System.Runtime.CompilerServices;
using UnityEngine;

public class VectorArrayMaterialProperty : MaterialPropertyExtensions.MaterialProperty
{
	private readonly Vector4[] Vectors;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VectorArrayMaterialProperty(string name, Vector4[] vectors)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Apply(ref MaterialPropertyBlock block)
	{
		throw null;
	}
}

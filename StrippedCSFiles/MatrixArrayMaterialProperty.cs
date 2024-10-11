using System.Runtime.CompilerServices;
using UnityEngine;

public class MatrixArrayMaterialProperty : MaterialPropertyExtensions.MaterialProperty
{
	private readonly Matrix4x4[] Matrices;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MatrixArrayMaterialProperty(string name, Matrix4x4[] matrices)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Apply(ref MaterialPropertyBlock block)
	{
		throw null;
	}
}

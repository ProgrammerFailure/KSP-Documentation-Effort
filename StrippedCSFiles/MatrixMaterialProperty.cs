using System.Runtime.CompilerServices;
using UnityEngine;

public class MatrixMaterialProperty : MaterialPropertyExtensions.MaterialProperty
{
	private readonly Matrix4x4 Matrix;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MatrixMaterialProperty(string name, Matrix4x4 matrix)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Apply(ref MaterialPropertyBlock block)
	{
		throw null;
	}
}

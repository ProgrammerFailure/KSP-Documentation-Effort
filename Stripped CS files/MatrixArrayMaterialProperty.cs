using UnityEngine;

public class MatrixArrayMaterialProperty : MaterialPropertyExtensions.MaterialProperty
{
	public readonly Matrix4x4[] Matrices;

	public MatrixArrayMaterialProperty(string name, Matrix4x4[] matrices)
	{
		Name = name;
		Matrices = matrices;
	}

	public override void Apply(ref MaterialPropertyBlock block)
	{
		block.SetMatrixArray(Name, Matrices);
	}
}

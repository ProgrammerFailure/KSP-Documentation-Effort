using UnityEngine;

public class MatrixMaterialProperty : MaterialPropertyExtensions.MaterialProperty
{
	public readonly Matrix4x4 Matrix;

	public MatrixMaterialProperty(string name, Matrix4x4 matrix)
	{
		Name = name;
		Matrix = matrix;
	}

	public override void Apply(ref MaterialPropertyBlock block)
	{
		block.SetMatrix(Name, Matrix);
	}
}

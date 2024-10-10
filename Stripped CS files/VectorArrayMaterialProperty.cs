using UnityEngine;

public class VectorArrayMaterialProperty : MaterialPropertyExtensions.MaterialProperty
{
	public readonly Vector4[] Vectors;

	public VectorArrayMaterialProperty(string name, Vector4[] vectors)
	{
		Name = name;
		Vectors = vectors;
	}

	public override void Apply(ref MaterialPropertyBlock block)
	{
		block.SetVectorArray(Name, Vectors);
	}
}

using UnityEngine;

public class VectorMaterialProperty : MaterialPropertyExtensions.MaterialProperty
{
	public readonly Vector4 Vector;

	public VectorMaterialProperty(string name, Vector4 vector)
	{
		Name = name;
		Vector = vector;
	}

	public override void Apply(ref MaterialPropertyBlock block)
	{
		block.SetVector(Name, Vector);
	}
}

using UnityEngine;

public class FloatMaterialProperty : MaterialPropertyExtensions.MaterialProperty
{
	public readonly float Value;

	public FloatMaterialProperty(string name, float value)
	{
		Name = name;
		Value = value;
	}

	public override void Apply(ref MaterialPropertyBlock block)
	{
		block.SetFloat(Name, Value);
	}
}

using UnityEngine;

public class FloatArrayMaterialProperty : MaterialPropertyExtensions.MaterialProperty
{
	public readonly float[] Values;

	public FloatArrayMaterialProperty(string name, float[] values)
	{
		Name = name;
		Values = values;
	}

	public override void Apply(ref MaterialPropertyBlock block)
	{
		block.SetFloatArray(Name, Values);
	}
}

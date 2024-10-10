using UnityEngine;

public class TextureMaterialProperty : MaterialPropertyExtensions.MaterialProperty
{
	public readonly Texture Texture;

	public TextureMaterialProperty(string name, Texture texture)
	{
		Name = name;
		Texture = texture;
	}

	public override void Apply(ref MaterialPropertyBlock block)
	{
		block.SetTexture(Name, Texture);
	}
}

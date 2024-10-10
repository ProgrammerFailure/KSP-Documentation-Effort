using UnityEngine;

namespace SoftMasking.TextMeshPro;

[GlobalMaterialReplacer]
public class MaterialReplacer : IMaterialReplacer
{
	public int order => 10;

	public Material Replace(Material material)
	{
		if ((bool)material && (bool)material.shader && material.shader.name.StartsWith("TextMeshPro/"))
		{
			Shader shader = Shader.Find("Soft Mask/" + material.shader.name);
			if ((bool)shader)
			{
				Material material2 = new Material(shader);
				material2.CopyPropertiesFromMaterial(material);
				return material2;
			}
		}
		return null;
	}
}

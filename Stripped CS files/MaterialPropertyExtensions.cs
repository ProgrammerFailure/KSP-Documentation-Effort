using UnityEngine;

public static class MaterialPropertyExtensions
{
	public abstract class MaterialProperty
	{
		public string Name;

		public MaterialProperty()
		{
		}

		public abstract void Apply(ref MaterialPropertyBlock block);
	}

	public static MaterialPropertyBlock block = new MaterialPropertyBlock();

	public static void SetupProperties(this Renderer renderer, params MaterialProperty[] properties)
	{
		block.Clear();
		int num = properties.Length;
		for (int i = 0; i < num; i++)
		{
			properties[i].Apply(ref block);
		}
		renderer.SetPropertyBlock(block);
	}

	public static void UpdateProperties(this Renderer renderer, params MaterialProperty[] properties)
	{
		renderer.GetPropertyBlock(block);
		int num = properties.Length;
		for (int i = 0; i < num; i++)
		{
			properties[i].Apply(ref block);
		}
		renderer.SetPropertyBlock(block);
	}
}

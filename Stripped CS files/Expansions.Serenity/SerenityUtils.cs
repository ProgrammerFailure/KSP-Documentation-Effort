using UnityEngine;

namespace Expansions.Serenity;

public static class SerenityUtils
{
	public static Texture2D SerenityTexture(string prefabPath)
	{
		return BundleLoader.LoadTextureAsset("serenity_assets", "Assets/Expansions/Serenity/" + prefabPath);
	}

	public static GameObject SerenityPrefab(string prefabPath)
	{
		return (GameObject)BundleLoader.LoadAsset("serenity_assets", "Assets/Expansions/Serenity/" + prefabPath);
	}
}

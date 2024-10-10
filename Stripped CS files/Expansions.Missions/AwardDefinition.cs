using System;
using UnityEngine;

namespace Expansions.Missions;

[Serializable]
public class AwardDefinition : IConfigNode
{
	public string id;

	public string name;

	public string displayName;

	public string description;

	public Sprite icon;

	public void Load(ConfigNode node)
	{
		node.TryGetValue("id", ref id);
		node.TryGetValue("awardName", ref name);
		node.TryGetValue("displayName", ref displayName);
		node.TryGetValue("description", ref description);
		string value = "";
		if (node.TryGetValue("icon", ref value))
		{
			GameDatabase.TextureInfo textureInfo = GameDatabase.Instance.GetTextureInfo(value);
			if (textureInfo != null)
			{
				icon = Sprite.Create(textureInfo.texture, new Rect(0f, 0f, textureInfo.texture.width, textureInfo.texture.height), Vector2.zero);
			}
		}
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("id", id);
		node.AddValue("awardName", name);
		node.AddValue("displayName", displayName);
		node.AddValue("description", description);
	}
}

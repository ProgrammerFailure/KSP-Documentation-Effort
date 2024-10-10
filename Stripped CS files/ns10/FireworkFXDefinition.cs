using System;
using UnityEngine;

namespace ns10;

[Serializable]
public class FireworkFXDefinition
{
	[Persistent]
	[SerializeField]
	public string name = "bareTrail";

	[SerializeField]
	[Persistent]
	public FireworkEffectType fwType;

	[SerializeField]
	[Persistent]
	public string prefabName = "fx_fwTrail1";

	[SerializeField]
	[Persistent]
	public string displayName = "Bare";

	[SerializeField]
	[Persistent]
	public string color1Name = "Color 1";

	[Persistent]
	[SerializeField]
	public string color2Name = "Color 2";

	[SerializeField]
	[Persistent]
	public string color3Name = "none";

	[Persistent]
	[SerializeField]
	public string crackleSFX = "none";

	public int id = -1;

	public bool randomizeBurstOrientation;

	public float minTrailLifetime = -1f;

	public float maxTrailLifetime = -1f;

	public void Load(ConfigNode node)
	{
		node.TryGetValue("name", ref name);
		id = name.GetHashCode();
		node.TryGetValue("prefabName", ref prefabName);
		node.TryGetValue("displayName", ref displayName);
		node.TryGetValue("color1Name", ref color1Name);
		node.TryGetValue("color2Name", ref color2Name);
		node.TryGetValue("color3Name", ref color3Name);
		node.TryGetValue("crackleSFX", ref crackleSFX);
		node.TryGetValue("randomizeBurstOrientation", ref randomizeBurstOrientation);
		node.TryGetValue("minTrailLifetime", ref minTrailLifetime);
		node.TryGetValue("maxTrailLifetime", ref maxTrailLifetime);
		if (node.HasValue("fwType"))
		{
			fwType = (FireworkEffectType)Enum.Parse(typeof(FireworkEffectType), node.GetValue("fwType"));
		}
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("name", name);
		node.AddValue("prefabName", prefabName);
		node.AddValue("displayName", displayName);
		node.AddValue("color1Name", color1Name);
		node.AddValue("color2Name", color2Name);
		node.AddValue("color3Name", color3Name);
		node.AddValue("fwType", fwType);
		node.AddValue("crackleSFX", crackleSFX);
		node.AddValue("randomizeBurstOrientation", randomizeBurstOrientation);
		node.AddValue("minTrailLifetime", minTrailLifetime);
		node.AddValue("maxTrailLifetime", maxTrailLifetime);
	}
}

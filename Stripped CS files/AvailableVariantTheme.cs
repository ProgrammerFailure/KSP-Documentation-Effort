using System;
using System.Collections.Generic;
using ns9;

[Serializable]
public class AvailableVariantTheme : IConfigNode
{
	[Persistent]
	public string name;

	[Persistent]
	public string displayName;

	[Persistent]
	public string description;

	[Persistent]
	public string primaryColor;

	[Persistent]
	public string secondaryColor;

	[Persistent]
	public List<AvailablePart> parts;

	public static AvailableVariantTheme CreateVariantTheme()
	{
		return new AvailableVariantTheme
		{
			name = Localizer.Format("#autoLOC_168872"),
			displayName = Localizer.Format("#autoLOC_168872"),
			description = Localizer.Format("#autoLOC_168872"),
			primaryColor = "#ffffff",
			secondaryColor = "#f0f0f0",
			parts = new List<AvailablePart>()
		};
	}

	public void Load(ConfigNode node)
	{
		node.TryGetValue("name", ref name);
		node.TryGetValue("displayName", ref displayName);
		node.TryGetValue("description", ref description);
		node.TryGetValue("primaryColor", ref primaryColor);
		node.TryGetValue("secondaryColor", ref secondaryColor);
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("name", name);
		node.AddValue("displayName", displayName);
		node.AddValue("description", description);
		node.AddValue("primaryColor", primaryColor);
		node.AddValue("secondaryColor", secondaryColor);
	}
}

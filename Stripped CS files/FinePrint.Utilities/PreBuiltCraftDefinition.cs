using System;
using System.Collections.Generic;
using ns11;

namespace FinePrint.Utilities;

[Serializable]
public class PreBuiltCraftDefinition
{
	public string craftURL;

	public ConfigNode craftNode;

	public CraftProfileInfo craftInfo;

	public List<string> contractTypes;

	public bool allowGround = true;

	public bool allowOrbit;

	public bool allowWater;

	public bool usePreBuiltPositions = true;

	public List<string> brokenPartNames => craftInfo.BrokenShipParts;

	public bool ContainsBrokenParts => craftInfo.BrokenShipParts.Count > 0;

	public PreBuiltCraftDefinition(string url, ConfigNode node)
	{
		craftURL = url;
		craftNode = node;
		craftInfo = new CraftProfileInfo();
		contractTypes = new List<string>();
	}
}

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI.Screens;

namespace FinePrint.Utilities;

[Serializable]
public class PreBuiltCraftDefinition
{
	public string craftURL;

	public ConfigNode craftNode;

	public CraftProfileInfo craftInfo;

	public List<string> contractTypes;

	public bool allowGround;

	public bool allowOrbit;

	public bool allowWater;

	public bool usePreBuiltPositions;

	public List<string> brokenPartNames
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool ContainsBrokenParts
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PreBuiltCraftDefinition(string url, ConfigNode node)
	{
		throw null;
	}
}

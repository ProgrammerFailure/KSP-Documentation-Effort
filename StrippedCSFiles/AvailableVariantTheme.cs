using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AvailableVariantTheme()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static AvailableVariantTheme CreateVariantTheme()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}
}

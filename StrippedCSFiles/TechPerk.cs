using System.Runtime.CompilerServices;
using UnityEngine;

public class TechPerk : ITechPerk
{
	public string perkID;

	public string description;

	public Texture2D icon;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TechPerk()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPerk(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GetPerkState()
	{
		throw null;
	}
}

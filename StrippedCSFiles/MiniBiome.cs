using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class MiniBiome : ScriptableObject
{
	public string TagKey;

	public string LocalizedTag;

	public string TagKeyID;

	public string GetTagKeyString
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string GetDisplayName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string GetLocalizedTag
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MiniBiome()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string ConvertTagtoLandedAt(string tagname)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string ConvertTagtoBiome(string tagname)
	{
		throw null;
	}
}

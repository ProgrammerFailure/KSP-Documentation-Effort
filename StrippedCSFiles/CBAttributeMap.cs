using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class CBAttributeMap
{
	[Serializable]
	public class MapAttribute
	{
		public Color mapColor;

		public string name;

		public float value;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public MapAttribute()
		{
			throw null;
		}
	}

	public Texture2D Map;

	public MapAttribute[] Attributes;

	public MapAttribute defaultAttribute;

	public bool exactSearch;

	public float nonExactThreshold;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CBAttributeMap()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MapAttribute GetAtt(double lat, double lon)
	{
		throw null;
	}
}

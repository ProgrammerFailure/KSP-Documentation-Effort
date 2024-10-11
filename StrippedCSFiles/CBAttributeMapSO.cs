using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class CBAttributeMapSO : MapSO
{
	[Serializable]
	public class MapAttribute
	{
		public Color mapColor;

		public string name;

		public string localizationTag;

		public string displayname;

		public float value;

		public bool notNear;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public MapAttribute()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public MapAttribute(MapAttribute copyFrom)
		{
			throw null;
		}
	}

	public MapAttribute[] Attributes;

	public bool exactSearch;

	public float nonExactThreshold;

	public float neighborColorThresh;

	public Color[] nearColors;

	private Color lerpColor;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CBAttributeMapSO()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FormatLocalizationTags()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override Color GetPixelColor(double x, double y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual MapAttribute GetAtt(double lat, double lon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static float RGBColorSqrMag(Color colA, Color colB)
	{
		throw null;
	}
}

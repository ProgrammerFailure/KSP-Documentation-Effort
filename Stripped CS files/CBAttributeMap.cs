using System;
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
	}

	public Texture2D Map;

	public MapAttribute[] Attributes;

	public MapAttribute defaultAttribute;

	public bool exactSearch;

	public float nonExactThreshold = -1f;

	public MapAttribute GetAtt(double lat, double lon)
	{
		if (Map != null)
		{
			lon -= Math.PI / 2.0;
			lon = UtilMath.WrapAround(lon, 0.0, Math.PI * 2.0);
			float v = (float)(lat / Math.PI) + 0.5f;
			float u = (float)(1.0 - lon / (Math.PI * 2.0));
			Color pixelBilinear = Map.GetPixelBilinear(u, v);
			MapAttribute result = defaultAttribute;
			if (exactSearch)
			{
				int i = 0;
				for (int num = Attributes.Length; i < num; i++)
				{
					if (pixelBilinear == Attributes[i].mapColor)
					{
						result = Attributes[i];
						break;
					}
				}
			}
			else
			{
				float num2 = float.MaxValue;
				int j = 0;
				for (int num3 = Attributes.Length; j < num3; j++)
				{
					float sqrMagnitude = ((Vector4)Attributes[j].mapColor - (Vector4)pixelBilinear).sqrMagnitude;
					if (sqrMagnitude < num2 && (nonExactThreshold == -1f || sqrMagnitude < nonExactThreshold))
					{
						result = Attributes[j];
						num2 = sqrMagnitude;
					}
				}
			}
			return result;
		}
		return defaultAttribute;
	}
}

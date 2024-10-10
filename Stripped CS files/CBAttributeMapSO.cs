using System;
using ns9;
using UnityEngine;

[Serializable]
public class CBAttributeMapSO : MapSO
{
	[Serializable]
	public class MapAttribute
	{
		public Color mapColor = Color.white;

		public string name = "";

		public string localizationTag;

		public string displayname;

		public float value;

		public bool notNear;

		public MapAttribute()
		{
			mapColor = Color.white;
			name = "";
			localizationTag = "";
			displayname = "";
			value = 0f;
		}

		public MapAttribute(MapAttribute copyFrom)
		{
			mapColor = copyFrom.mapColor;
			name = copyFrom.name;
			localizationTag = copyFrom.localizationTag;
			displayname = copyFrom.displayname;
			value = copyFrom.value;
		}
	}

	public MapAttribute[] Attributes = new MapAttribute[0];

	public bool exactSearch;

	public float nonExactThreshold = -1f;

	public float neighborColorThresh = 0.0009765625f;

	public Color[] nearColors = new Color[4];

	public Color lerpColor;

	public void FormatLocalizationTags()
	{
		int num = Attributes.Length;
		while (num-- > 0)
		{
			Attributes[num].displayname = Localizer.Format(Attributes[num].localizationTag);
		}
	}

	public override Color GetPixelColor(double x, double y)
	{
		ConstructBilinearCoords(x, y);
		nearColors[0] = GetPixelColor(minX, minY);
		nearColors[1] = GetPixelColor(maxX, minY);
		nearColors[2] = GetPixelColor(minX, maxY);
		nearColors[3] = GetPixelColor(maxX, maxY);
		bool flag = true;
		int num = Attributes.Length;
		while (num-- > 0)
		{
			Color mapColor = Attributes[num].mapColor;
			if (!(RGBColorSqrMag(mapColor, nearColors[0]) < neighborColorThresh) && !(RGBColorSqrMag(mapColor, nearColors[1]) < neighborColorThresh) && !(RGBColorSqrMag(mapColor, nearColors[2]) < neighborColorThresh) && RGBColorSqrMag(mapColor, nearColors[3]) >= neighborColorThresh)
			{
				Attributes[num].notNear = true;
				continue;
			}
			Attributes[num].notNear = false;
			flag = false;
		}
		lerpColor = Color.Lerp(Color.Lerp(nearColors[0], nearColors[1], midX), Color.Lerp(nearColors[2], nearColors[3], midX), midY);
		if (flag)
		{
			int num2 = Attributes.Length;
			while (num2-- > 0)
			{
				Attributes[num2].notNear = false;
			}
			return lerpColor;
		}
		Color result = nearColors[0];
		float num3 = float.MaxValue;
		int i = 0;
		for (int num4 = 3; i < num4; i++)
		{
			Color color = nearColors[i];
			float sqrMagnitude = ((Vector4)color - (Vector4)lerpColor).sqrMagnitude;
			if (sqrMagnitude < num3)
			{
				num3 = sqrMagnitude;
				result = color;
			}
		}
		return result;
	}

	public virtual MapAttribute GetAtt(double lat, double lon)
	{
		lon -= Math.PI / 2.0;
		lon = UtilMath.WrapAround(lon, 0.0, Math.PI * 2.0);
		double y = lat * (1.0 / Math.PI) + 0.5;
		double x = 1.0 - lon * (1.0 / (2.0 * Math.PI));
		Color pixelColor = GetPixelColor(x, y);
		MapAttribute result = Attributes[0];
		if (exactSearch)
		{
			int i = 0;
			for (int num = Attributes.Length; i < num; i++)
			{
				if (!Attributes[i].notNear && pixelColor == Attributes[i].mapColor)
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
				if (!Attributes[j].notNear)
				{
					Color mapColor = Attributes[j].mapColor;
					float num4 = RGBColorSqrMag(pixelColor, mapColor);
					if (num4 < num2 && (nonExactThreshold == -1f || num4 < nonExactThreshold))
					{
						result = Attributes[j];
						num2 = num4;
					}
				}
			}
		}
		return result;
	}

	public static float RGBColorSqrMag(Color colA, Color colB)
	{
		float num = colA.r - colB.r;
		float num2 = num * num;
		num = colA.g - colB.g;
		float num3 = num2 + num * num;
		num = colA.b - colB.b;
		return num3 + num * num;
	}
}

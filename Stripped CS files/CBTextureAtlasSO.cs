using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CBTextureAtlasSO : MapSO
{
	public class CBTextureAtlasPoint
	{
		public List<int> TextureAtlasIndices = new List<int>();

		public List<float> TextureAtlasStrengths = new List<float>();
	}

	public CBTextureAtlasPoint GetPixelCBTextureAtlasPoint(double x, double y)
	{
		ConstructBilinearCoords(x, y);
		Vector2[] array = new Vector2[4]
		{
			new Vector2(minX, minY),
			new Vector2(maxX, minY),
			new Vector2(minX, maxY),
			new Vector2(maxX, maxY)
		};
		Color32[] array2 = new Color32[4]
		{
			GetPixelColor32(minX, minY),
			GetPixelColor32(maxX, minY),
			GetPixelColor32(minX, maxY),
			GetPixelColor32(maxX, maxY)
		};
		CBTextureAtlasPoint cBTextureAtlasPoint = new CBTextureAtlasPoint();
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].r = (byte)(Mathf.RoundToInt((float)(int)array2[i].r / 10f) * 10);
			cBTextureAtlasPoint.TextureAtlasIndices.AddUnique(array2[i].r);
			array2[i].b = (byte)(Mathf.RoundToInt((float)(int)array2[i].b / 10f) * 10);
			cBTextureAtlasPoint.TextureAtlasIndices.AddUnique(array2[i].b);
		}
		for (int j = 0; j < cBTextureAtlasPoint.TextureAtlasIndices.Count; j++)
		{
			cBTextureAtlasPoint.TextureAtlasStrengths.Add(0f);
		}
		for (int k = 0; k < cBTextureAtlasPoint.TextureAtlasIndices.Count; k++)
		{
			for (int l = 0; l < array2.Length; l++)
			{
				if (cBTextureAtlasPoint.TextureAtlasIndices[k] == array2[l].r)
				{
					float num = 1f - Mathf.Abs((float)centerXD - array[l][0]);
					float num2 = 1f - Mathf.Abs((float)centerYD - array[l][1]);
					cBTextureAtlasPoint.TextureAtlasStrengths[k] += (float)(int)array2[l].g * num * num2;
				}
				if (cBTextureAtlasPoint.TextureAtlasIndices[k] == array2[l].b)
				{
					float num3 = 1f - Mathf.Abs((float)centerXD - array[l][0]);
					float num4 = 1f - Mathf.Abs((float)centerYD - array[l][1]);
					cBTextureAtlasPoint.TextureAtlasStrengths[k] += (255f - (float)(int)array2[l].g) * num3 * num4;
				}
			}
		}
		float num5 = 0f;
		for (int m = 0; m < cBTextureAtlasPoint.TextureAtlasStrengths.Count; m++)
		{
			num5 += cBTextureAtlasPoint.TextureAtlasStrengths[m];
		}
		for (int n = 0; n < cBTextureAtlasPoint.TextureAtlasStrengths.Count; n++)
		{
			cBTextureAtlasPoint.TextureAtlasStrengths[n] = cBTextureAtlasPoint.TextureAtlasStrengths[n] / num5;
		}
		return cBTextureAtlasPoint;
	}

	public virtual CBTextureAtlasPoint GetCBTextureAtlasPoint(double lat, double lon)
	{
		lon -= 1.5707963705062866;
		lon = UtilMath.WrapAround(lon, 0.0, 6.2831854820251465);
		double y = lat * 0.31830987732601135 + 0.5;
		double x = 1.0 - lon * 0.15915493866300567;
		return GetPixelCBTextureAtlasPoint(x, y);
	}
}

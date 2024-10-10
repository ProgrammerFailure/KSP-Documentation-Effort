using System;
using System.IO;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Tools/Altitude Multitexture Ramp Creator")]
public class AltitudeMultiTextureRamp : MonoBehaviour
{
	[Serializable]
	public class LerpRange
	{
		public string name;

		public float startStart;

		public float startEnd;

		public float endStart;

		public float endEnd;

		[HideInInspector]
		public float startDelta;

		[HideInInspector]
		public float endDelta;

		public LerpRange()
		{
		}

		public LerpRange(float startStart, float startEnd, float endStart, float endEnd)
		{
			this.startStart = startStart;
			this.startEnd = startEnd;
			this.endStart = endStart;
			this.endEnd = endEnd;
		}

		public void Setup()
		{
			startDelta = 1f / (startEnd - startStart);
			endDelta = 1f / (endEnd - endStart);
		}

		public float Lerp(float point)
		{
			if (!(point <= startStart) && point < endEnd)
			{
				if (point < startEnd)
				{
					return (point - startStart) * startDelta;
				}
				if (point <= endStart)
				{
					return 1f;
				}
				if (point < endEnd)
				{
					return 1f - (point - endStart) * endDelta;
				}
				return 0f;
			}
			return 0f;
		}
	}

	public int resolution;

	public string filename;

	public LerpRange[] textures;

	public void Reset()
	{
		resolution = 1024;
		filename = "amtr_test";
		textures = new LerpRange[4];
		textures[0] = new LerpRange(0f, 0.1f, 0.2f, 0.3f);
		textures[1] = new LerpRange(0.2f, 0.3f, 0.4f, 0.5f);
		textures[2] = new LerpRange(0.4f, 0.5f, 0.6f, 0.7f);
		textures[3] = new LerpRange(0.6f, 0.7f, 0.9f, 1f);
	}

	[ContextMenu("Create Texture ARGB")]
	public void CreateTextureARGB()
	{
		ProfileTimer.Push("AltitudeMultiTextureRamp: Create Texture ARGB");
		int num = textures.Length;
		int num2 = Mathf.FloorToInt((float)num / 4f) + ((textures.Length % 4 != 0) ? 1 : 0);
		float num3 = 0f;
		float num4 = 1f / (float)(resolution + 1);
		int i = 0;
		int num5 = 0;
		int j = 0;
		int num6 = 0;
		Color[] array = new Color[num2 * resolution];
		for (num5 = 0; num5 < num; num5++)
		{
			textures[num5].Setup();
			num3 = 0f;
			for (i = 0; i < resolution; i++)
			{
				switch (j)
				{
				case 0:
					array[num6 * resolution + i].r = textures[num5].Lerp(num3);
					break;
				case 1:
					array[num6 * resolution + i].g = textures[num5].Lerp(num3);
					break;
				case 2:
					array[num6 * resolution + i].b = textures[num5].Lerp(num3);
					break;
				case 3:
					array[num6 * resolution + i].a = textures[num5].Lerp(num3);
					break;
				}
				num3 += num4;
			}
			j++;
			if (j == 4)
			{
				j = 0;
				num6++;
			}
		}
		if (j != 0)
		{
			i--;
			num6--;
			for (; j < 4; j++)
			{
				switch (j)
				{
				case 1:
					array[num6 * resolution + i].g = 0f;
					break;
				case 2:
					array[num6 * resolution + i].b = 0f;
					break;
				case 3:
					array[num6 * resolution + i].a = 0f;
					break;
				}
			}
		}
		Texture2D texture2D = new Texture2D(resolution, num2, TextureFormat.ARGB32, mipChain: false);
		texture2D.SetPixels(array);
		texture2D.Apply();
		File.WriteAllBytes(Application.dataPath + filename + ".png", texture2D.EncodeToPNG());
		UnityEngine.Object.DestroyImmediate(texture2D);
		array = null;
		texture2D = null;
		ProfileTimer.Pop();
	}

	[ContextMenu("Create Texture GS")]
	public void CreateTextureGS()
	{
		ProfileTimer.Push("AltitudeMultiTextureRamp: Create Texture GS");
		int num = textures.Length;
		int num2 = num;
		float num3 = 0f;
		float num4 = 1f / (float)(resolution + 1);
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		Color[] array = new Color[num2 * resolution];
		for (num6 = 0; num6 < num; num6++)
		{
			textures[num6].Setup();
			num3 = 0f;
			for (num5 = 0; num5 < resolution; num5++)
			{
				array[num7 * resolution + num5] = Color.white * textures[num6].Lerp(num3);
				num3 += num4;
			}
			num7++;
		}
		Texture2D texture2D = new Texture2D(resolution, num2, TextureFormat.RGB24, mipChain: false);
		texture2D.SetPixels(array);
		texture2D.Apply();
		File.WriteAllBytes(Application.dataPath + "\\" + filename + ".png", texture2D.EncodeToPNG());
		UnityEngine.Object.DestroyImmediate(texture2D);
		array = null;
		texture2D = null;
		ProfileTimer.Pop();
	}
}

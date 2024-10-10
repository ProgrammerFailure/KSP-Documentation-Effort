using UnityEngine;

public class TextureScale
{
	public static Color[] texColors;

	public static Color[] newColors;

	public static int w;

	public static float ratioX;

	public static float ratioY;

	public static int w2;

	public static int finishCount;

	public static void Point(Texture2D tex, int newWidth, int newHeight)
	{
		Scale(tex, newWidth, newHeight, useBilinear: false);
	}

	public static void Bilinear(Texture2D tex, int newWidth, int newHeight)
	{
		Scale(tex, newWidth, newHeight, useBilinear: true);
	}

	public static void Scale(Texture2D tex, int newWidth, int newHeight, bool useBilinear)
	{
		texColors = tex.GetPixels();
		newColors = new Color[newWidth * newHeight];
		if (useBilinear)
		{
			ratioX = 1f / ((float)newWidth / (float)(tex.width - 1));
			ratioY = 1f / ((float)newHeight / (float)(tex.height - 1));
		}
		else
		{
			ratioX = (float)tex.width / (float)newWidth;
			ratioY = (float)tex.height / (float)newHeight;
		}
		w = tex.width;
		w2 = newWidth;
		if (useBilinear)
		{
			BilinearScale(newHeight);
		}
		else
		{
			PointScale(newHeight);
		}
		tex.Resize(newWidth, newHeight);
		tex.SetPixels(newColors);
		tex.Apply();
	}

	public static void BilinearScale(int end)
	{
		for (int i = 0; i < end; i++)
		{
			int num = (int)Mathf.Floor((float)i * ratioY);
			int num2 = num * w;
			int num3 = (num + 1) * w;
			int num4 = i * w2;
			for (int j = 0; j < w2; j++)
			{
				int num5 = (int)Mathf.Floor((float)j * ratioX);
				float value = (float)j * ratioX - (float)num5;
				newColors[num4 + j] = ColorLerpUnclamped(ColorLerpUnclamped(texColors[num2 + num5], texColors[num2 + num5 + 1], value), ColorLerpUnclamped(texColors[num3 + num5], texColors[num3 + num5 + 1], value), (float)i * ratioY - (float)num);
			}
		}
	}

	public static void PointScale(int end)
	{
		for (int i = 0; i < end; i++)
		{
			int num = (int)(ratioY * (float)i) * w;
			int num2 = i * w2;
			for (int j = 0; j < w2; j++)
			{
				newColors[num2 + j] = texColors[(int)((float)num + ratioX * (float)j)];
			}
		}
	}

	public static Color ColorLerpUnclamped(Color c1, Color c2, float value)
	{
		return new Color(c1.r + (c2.r - c1.r) * value, c1.g + (c2.g - c1.g) * value, c1.b + (c2.b - c1.b) * value, c1.a + (c2.a - c1.a) * value);
	}
}

using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace ns2;

public static class U5Util
{
	public static Color HexToColor(string hex)
	{
		hex = hex.Replace("0x", "");
		hex = hex.Replace("#", "");
		byte a = byte.MaxValue;
		byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
		if (hex.Length == 8)
		{
			a = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
		}
		return new Color32(r, g, b, a);
	}

	public static string ColorToHex(Color32 color)
	{
		return color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
	}

	public static void SetLayerRecursive(GameObject obj, int layer)
	{
		obj.layer = layer;
		for (int i = 0; i < obj.transform.childCount; i++)
		{
			SetLayerRecursive(obj.transform.GetChild(i).gameObject, layer);
		}
	}

	public static float GetSimulatedScrollMultiplier(ScrollRect scrollRect, bool vertical)
	{
		if (vertical)
		{
			float result = 1f;
			if (scrollRect.content.rect.height > (scrollRect.transform as RectTransform).rect.height && scrollRect.content.rect.height != 0f)
			{
				result = (scrollRect.transform as RectTransform).rect.height / scrollRect.content.rect.height;
			}
			return result;
		}
		float result2 = 1f;
		if (scrollRect.content.rect.width > (scrollRect.transform as RectTransform).rect.width && scrollRect.content.rect.width != 0f)
		{
			result2 = (scrollRect.transform as RectTransform).rect.width / scrollRect.content.rect.width;
		}
		return result2;
	}
}

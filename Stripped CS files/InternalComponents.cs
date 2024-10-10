using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InternalComponents : MonoBehaviour
{
	public List<InternalText> textPrefabs;

	public static InternalComponents Instance { get; set; }

	public void Awake()
	{
		Instance = this;
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public InternalText CreateText(string fontName, float fontSize, Transform position)
	{
		int num = 0;
		int count = textPrefabs.Count;
		InternalText internalText;
		while (true)
		{
			if (num < count)
			{
				internalText = textPrefabs[num];
				if (internalText.fontName == fontName)
				{
					break;
				}
				num++;
				continue;
			}
			Debug.LogError("InternalText: Cannot create text, prefab not found");
			return null;
		}
		InternalText internalText2 = Object.Instantiate(internalText);
		internalText2.gameObject.SetActive(value: true);
		internalText2.transform.SetParent(position, worldPositionStays: false);
		internalText2.transform.localScale = Vector3.one * (fontSize / internalText2.text.fontSize) * 0.7f;
		internalText2.transform.localPosition = Vector3.zero;
		internalText2.transform.localRotation = Quaternion.identity;
		return internalText2;
	}

	public InternalText CreateText(string fontName, float fontSize, Transform position, string textString, Color color, bool enablewordWrapping, string alignment)
	{
		InternalText internalText = CreateText(fontName, fontSize, position);
		if (internalText != null)
		{
			if (!string.IsNullOrEmpty(alignment))
			{
				TextAlignmentOptions tMPAlignment = getTMPAlignment(alignment);
				internalText.text.alignment = tMPAlignment;
			}
			internalText.text.enableWordWrapping = enablewordWrapping;
			internalText.text.color = color;
			internalText.text.text = textString;
		}
		return internalText;
	}

	public TextAlignmentOptions getTMPAlignment(string alignment)
	{
		TextAlignmentOptions result = TextAlignmentOptions.Center;
		switch (alignment)
		{
		case "Top":
			result = TextAlignmentOptions.Top;
			break;
		case "BottomLeft":
			result = TextAlignmentOptions.BottomLeft;
			break;
		case "Center":
			result = TextAlignmentOptions.Center;
			break;
		case "CenterGeoAligned":
			result = TextAlignmentOptions.CenterGeoAligned;
			break;
		case "Left":
			result = TextAlignmentOptions.Left;
			break;
		case "BottomGeoAligned":
			result = TextAlignmentOptions.BottomGeoAligned;
			break;
		case "MidlineLeft":
			result = TextAlignmentOptions.MidlineLeft;
			break;
		case "TopGeoAligned":
			result = TextAlignmentOptions.TopGeoAligned;
			break;
		case "TopLeft":
			result = TextAlignmentOptions.TopLeft;
			break;
		}
		return result;
	}
}

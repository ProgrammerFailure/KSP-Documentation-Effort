using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogGUIImage : DialogGUIBase
{
	public Texture image;

	public Rect uvRect = new Rect(0f, 0f, 1f, 1f);

	public DialogGUIImage(Vector2 s, Vector2 p, Color t, Texture i)
	{
		image = i;
		size = s;
		position = p;
		tint = t;
	}

	public DialogGUIImage(Vector2 s, Vector2 p, Color t, Texture i, Rect uv)
	{
		image = i;
		size = s;
		position = p;
		tint = t;
		uvRect = uv;
	}

	public override GameObject Create(ref Stack<Transform> layouts, UISkinDef skin)
	{
		uiItem = Object.Instantiate(UISkinManager.GetPrefab("UIRawImagePrefab"));
		uiItem.SetActive(value: true);
		uiItem.GetComponent<RawImage>().texture = image;
		uiItem.GetComponent<RawImage>().uvRect = uvRect;
		uiItem.GetComponent<RawImage>().color = tint;
		uiItem.transform.SetParent(layouts.Peek(), worldPositionStays: false);
		SetupTransformAndLayout();
		return base.Create(ref layouts, skin);
	}
}

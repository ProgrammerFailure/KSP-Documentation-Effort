using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogGUISprite : DialogGUIBase
{
	public Sprite sprite;

	public DialogGUISprite(Vector2 s, Vector2 p, Color t, Sprite sprite)
	{
		this.sprite = sprite;
		size = s;
		position = p;
		tint = t;
	}

	public override GameObject Create(ref Stack<Transform> layouts, UISkinDef skin)
	{
		uiItem = Object.Instantiate(UISkinManager.GetPrefab("UIImagePrefab"));
		uiItem.SetActive(value: true);
		uiItem.GetComponent<Image>().sprite = sprite;
		uiItem.GetComponent<Image>().color = tint;
		uiItem.transform.SetParent(layouts.Peek(), worldPositionStays: false);
		SetupTransformAndLayout();
		return base.Create(ref layouts, skin);
	}
}

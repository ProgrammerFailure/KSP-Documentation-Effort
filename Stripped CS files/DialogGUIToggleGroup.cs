using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogGUIToggleGroup : DialogGUIBase
{
	public ToggleGroup group;

	public ToggleGroup Group => group;

	public DialogGUIToggleGroup(params DialogGUIToggle[] toggles)
		: base(toggles)
	{
	}

	public override GameObject Create(ref Stack<Transform> layouts, UISkinDef skin)
	{
		uiItem = layouts.Peek().gameObject;
		group = uiItem.AddComponent<ToggleGroup>();
		foreach (DialogGUIBase child in children)
		{
			child.Create(ref layouts, skin).GetComponent<Toggle>().group = group;
		}
		return uiItem;
	}
}

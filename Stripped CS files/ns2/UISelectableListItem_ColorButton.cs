using System;
using UnityEngine;
using UnityEngine.UI;

namespace ns2;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class UISelectableListItem_ColorButton : UISelectableListItem
{
	[Serializable]
	public class ButtonState
	{
		public Color normal = Color.white;

		public Color highlight = Color.white;

		public Color active = Color.white;

		public Color disabled = Color.white;
	}

	public ButtonState selected;

	public ButtonState unSelected;

	public Button button;

	public void Awake()
	{
		button = GetComponent<Button>();
		button.transition = Selectable.Transition.ColorTint;
		SetState(state: false);
	}

	public void SetState(bool state)
	{
		if (state)
		{
			ColorBlock colors = default(ColorBlock);
			colors.normalColor = selected.normal;
			colors.highlightedColor = selected.highlight;
			colors.pressedColor = selected.active;
			colors.disabledColor = selected.disabled;
			colors.colorMultiplier = 1f;
			button.colors = colors;
		}
		else
		{
			ColorBlock colors2 = default(ColorBlock);
			colors2.normalColor = unSelected.normal;
			colors2.highlightedColor = unSelected.highlight;
			colors2.pressedColor = unSelected.active;
			colors2.disabledColor = unSelected.disabled;
			colors2.colorMultiplier = 1f;
			button.colors = colors2;
		}
	}

	public override void Select()
	{
		SetState(state: true);
	}

	public override void Deselect()
	{
		SetState(state: false);
	}

	public override void SetupListItem(UISelectableList selectableList, int index)
	{
		base.SetupListItem(selectableList, index);
		Button component = GetComponent<Button>();
		component.onClick.RemoveAllListeners();
		component.onClick.AddListener(delegate
		{
			selectableList.ClickItem(index);
		});
	}
}

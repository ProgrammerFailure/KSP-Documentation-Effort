using System;
using UnityEngine;
using UnityEngine.UI;

namespace ns2;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class UISelectableListItem_StateButton : UISelectableListItem
{
	[Serializable]
	public class ButtonState
	{
		public Sprite normal;

		public Sprite highlight;

		public Sprite active;

		public Sprite disabled;
	}

	public ButtonState selected;

	public ButtonState unSelected;

	public Image image;

	public Button button;

	public void Awake()
	{
		image = GetComponent<Image>();
		button = GetComponent<Button>();
		button.transition = Selectable.Transition.SpriteSwap;
		SetState(state: false);
	}

	public void SetState(bool state)
	{
		if (state)
		{
			image.sprite = selected.normal;
			SpriteState spriteState = default(SpriteState);
			spriteState.highlightedSprite = selected.highlight;
			spriteState.pressedSprite = selected.active;
			spriteState.disabledSprite = selected.disabled;
			if (spriteState.selectedSprite == null)
			{
				spriteState.selectedSprite = selected.highlight;
			}
			button.spriteState = spriteState;
		}
		else
		{
			image.sprite = unSelected.normal;
			SpriteState spriteState2 = default(SpriteState);
			spriteState2.highlightedSprite = unSelected.highlight;
			spriteState2.pressedSprite = unSelected.active;
			spriteState2.disabledSprite = unSelected.disabled;
			if (spriteState2.selectedSprite == null)
			{
				spriteState2.selectedSprite = unSelected.highlight;
			}
			button.spriteState = spriteState2;
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

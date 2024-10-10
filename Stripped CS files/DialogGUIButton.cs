using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogGUIButton : DialogGUIBase
{
	public class TextLabelOptions
	{
		public bool resizeBestFit;

		public int resizeMinFontSize;

		public int resizeMaxFontSize;

		public bool enableWordWrapping = true;

		public TextOverflowModes OverflowMode;
	}

	public Callback onOptionSelected = delegate
	{
	};

	public bool DismissOnSelect;

	public Func<string> GetString;

	public Sprite image;

	public TextMeshProUGUI textItem;

	public TextLabelOptions textLabelOptions;

	public bool clearButtonImage;

	public DialogGUIButton(string optionText, Callback onSelected)
	{
		onOptionSelected = onSelected;
		OptionText = optionText;
		DismissOnSelect = true;
		OptionInteractableCondition = () => true;
	}

	public DialogGUIButton(string optionText, Callback onSelected, bool dismissOnSelect)
	{
		onOptionSelected = onSelected;
		OptionText = optionText;
		DismissOnSelect = dismissOnSelect;
		OptionInteractableCondition = () => true;
	}

	public DialogGUIButton(string optionText, Callback onSelected, float w, float h, bool dismissOnSelect, UIStyle style)
	{
		onOptionSelected = onSelected;
		OptionText = optionText;
		DismissOnSelect = dismissOnSelect;
		OptionInteractableCondition = () => true;
		size = new Vector2(w, h);
		guiStyle = style;
	}

	public DialogGUIButton(string optionText, Callback onSelected, float w, float h, bool dismissOnSelect, params DialogGUIBase[] options)
		: base(options)
	{
		onOptionSelected = onSelected;
		OptionText = optionText;
		DismissOnSelect = dismissOnSelect;
		OptionInteractableCondition = () => true;
		size = new Vector2(w, h);
	}

	public DialogGUIButton(Func<string> getString, Callback onSelected, float w, float h, bool dismissOnSelect, params DialogGUIBase[] options)
		: base(options)
	{
		onOptionSelected = onSelected;
		OptionText = getString();
		DismissOnSelect = dismissOnSelect;
		OptionInteractableCondition = () => true;
		size = new Vector2(w, h);
		GetString = getString;
	}

	public DialogGUIButton(Func<string> getString, Callback onSelected, Func<bool> EnabledCondition, float w, float h, bool dismissOnSelect, params DialogGUIBase[] options)
		: base(options)
	{
		onOptionSelected = onSelected;
		OptionText = getString();
		DismissOnSelect = dismissOnSelect;
		size = new Vector2(w, h);
		GetString = getString;
		OptionInteractableCondition = EnabledCondition;
	}

	public DialogGUIButton(string optionText, Callback onSelected, Func<bool> EnabledCondition, bool dismissOnSelect)
	{
		onOptionSelected = onSelected;
		OptionText = optionText;
		DismissOnSelect = dismissOnSelect;
		OptionInteractableCondition = EnabledCondition;
	}

	public DialogGUIButton(string optionText, Callback onSelected, Func<bool> EnabledCondition, float w, float h, bool dismissOnSelect, UIStyle style = null)
	{
		onOptionSelected = onSelected;
		OptionText = optionText;
		DismissOnSelect = dismissOnSelect;
		OptionInteractableCondition = EnabledCondition;
		size = new Vector2(w, h);
		guiStyle = style;
	}

	public DialogGUIButton(Func<string> getString, Callback onSelected, Func<bool> EnabledCondition, float w, float h, bool dismissOnSelect, UIStyle style)
	{
		onOptionSelected = onSelected;
		OptionText = getString();
		DismissOnSelect = dismissOnSelect;
		OptionInteractableCondition = EnabledCondition;
		size = new Vector2(w, h);
		guiStyle = style;
		GetString = getString;
	}

	public DialogGUIButton(Sprite image, Callback onSelected, float w, float h, bool dismissOnSelect = false)
	{
		DismissOnSelect = dismissOnSelect;
		this.image = image;
		onOptionSelected = onSelected;
		OptionText = "";
		size = new Vector2(w, h);
	}

	public DialogGUIButton(Sprite image, string text, Callback onSelected, float w, float h, bool dismissOnSelect = false)
	{
		DismissOnSelect = dismissOnSelect;
		this.image = image;
		onOptionSelected = onSelected;
		OptionText = text;
		size = new Vector2(w, h);
	}

	public virtual void OptionSelected()
	{
		onOptionSelected();
	}

	public void ClearButtonImage()
	{
		clearButtonImage = true;
	}

	public override void Update()
	{
		base.Update();
		if (!(uiItem != null) || !uiItem.activeInHierarchy)
		{
			return;
		}
		if (GetString != null)
		{
			textItem.text = GetString();
			Resize();
		}
		else if (textItem != null)
		{
			textItem.text = OptionText;
		}
		if (uiItem.activeSelf && OptionInteractableCondition != null && lastInteractibleState != OptionInteractableCondition())
		{
			lastInteractibleState = !lastInteractibleState;
			Selectable[] componentsInChildren = uiItem.GetComponentsInChildren<Selectable>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].interactable = lastInteractibleState;
			}
			Graphic[] componentsInChildren2 = uiItem.GetComponentsInChildren<Graphic>();
			foreach (Graphic graphic in componentsInChildren2)
			{
				graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, lastInteractibleState ? 1f : 0.5f);
			}
		}
	}

	public override void Resize()
	{
		base.Resize();
		if (uiItem.activeSelf && size.x == -1f && textItem != null)
		{
			float preferredWidth = LayoutUtility.GetPreferredWidth(textItem.GetComponent<RectTransform>());
			preferredWidth = Mathf.Clamp(preferredWidth, 30f, 100f);
			uiItem.GetComponent<LayoutElement>().minWidth = preferredWidth + 8f;
			if (uiItem.GetComponent<LayoutElement>().flexibleWidth != 0f)
			{
				uiItem.GetComponent<LayoutElement>().flexibleWidth = 0f;
			}
		}
		if (uiItem.activeSelf && size.y == -1f && textItem != null)
		{
			uiItem.GetComponent<LayoutElement>().minHeight = 32f;
			if (uiItem.GetComponent<LayoutElement>().flexibleHeight != 0f)
			{
				uiItem.GetComponent<LayoutElement>().flexibleHeight = 0f;
			}
		}
	}

	public override GameObject Create(ref Stack<Transform> layouts, UISkinDef skin)
	{
		if (DismissOnSelect)
		{
			onOptionSelected = (Callback)Delegate.Combine(onOptionSelected, (Callback)delegate
			{
				PopupDialog componentInParent = uiItem.GetComponentInParent<PopupDialog>();
				if (componentInParent != null)
				{
					componentInParent.Dismiss();
				}
			});
		}
		uiItem = UnityEngine.Object.Instantiate(UISkinManager.GetPrefab("UIButtonPrefab"));
		Button component = uiItem.GetComponent<Button>();
		uiItem.transform.SetParent(layouts.Peek(), worldPositionStays: false);
		uiItem.SetActive(value: true);
		SetupTransformAndLayout();
		UIStyle uIStyle = guiStyle ?? skin.button;
		if (!(OptionText != string.Empty) && GetString == null)
		{
			GameObject child = uiItem.GetChild("Text");
			if (child != null)
			{
				UnityEngine.Object.Destroy(child);
			}
		}
		else
		{
			GameObject child2 = uiItem.GetChild("Text");
			textItem = child2.GetComponent<TextMeshProUGUI>();
			if (textLabelOptions != null)
			{
				textItem.enableAutoSizing = textLabelOptions.resizeBestFit;
				textItem.fontSizeMax = textLabelOptions.resizeMaxFontSize;
				textItem.fontSizeMin = textLabelOptions.resizeMinFontSize;
				textItem.enableWordWrapping = textLabelOptions.enableWordWrapping;
				textItem.overflowMode = textLabelOptions.OverflowMode;
			}
			DialogGUIBase.SetUpTextObject(textItem, OptionText, guiStyle ?? skin.button, skin);
		}
		if (image != null)
		{
			uiItem.GetComponent<Image>().sprite = image;
		}
		if (uIStyle.normal.background != null)
		{
			SpriteState spriteState = default(SpriteState);
			spriteState.disabledSprite = uIStyle.disabled.background;
			spriteState.highlightedSprite = uIStyle.highlight.background;
			spriteState.pressedSprite = uIStyle.active.background;
			if (spriteState.selectedSprite == null)
			{
				spriteState.selectedSprite = uIStyle.highlight.background;
			}
			component.spriteState = spriteState;
		}
		else
		{
			component.transition = Selectable.Transition.ColorTint;
		}
		if (clearButtonImage)
		{
			Image component2 = uiItem.GetComponent<Image>();
			if (component2 != null)
			{
				component2.color = uiItem.GetComponent<Image>().color.smethod_0(0f);
			}
		}
		component.onClick.AddListener(delegate
		{
			OptionSelected();
		});
		return base.Create(ref layouts, skin);
	}
}
public class DialogGUIButton<T> : DialogGUIButton
{
	public Callback<T> onParamOptionSelected = delegate
	{
	};

	public T parameter;

	public DialogGUIButton(string optionText, Callback<T> onSelected, T parameter)
		: base(optionText, delegate
		{
		}, dismissOnSelect: true)
	{
		onParamOptionSelected = onSelected;
		this.parameter = parameter;
	}

	public DialogGUIButton(string optionText, Callback<T> onSelected, T parameter, bool dismissOnSelect)
		: base(optionText, delegate
		{
		}, dismissOnSelect)
	{
		onParamOptionSelected = onSelected;
		this.parameter = parameter;
	}

	public DialogGUIButton(string optionText, Callback<T> onSelected, T parameter, Func<bool> EnabledCondition, bool dismissOnSelect)
		: base(optionText, delegate
		{
		}, EnabledCondition, dismissOnSelect)
	{
		onParamOptionSelected = onSelected;
		this.parameter = parameter;
	}

	public override void OptionSelected()
	{
		onParamOptionSelected(parameter);
		onOptionSelected();
	}
}

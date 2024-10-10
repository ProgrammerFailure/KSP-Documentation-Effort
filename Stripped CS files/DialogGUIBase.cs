using System;
using System.Collections.Generic;
using ns12;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogGUIBase
{
	public GameObject uiItem;

	public List<DialogGUIBase> children = new List<DialogGUIBase>();

	public string OptionText = "No Function";

	public float width = -1f;

	public float height = -1f;

	public Vector2 size = new Vector2(-1f, -1f);

	public Vector2 position;

	public bool useColor;

	public Color tint;

	public UIStyle guiStyle;

	public string tooltipText;

	public bool flexibleHeight = true;

	public TooltipController_Text toolTip;

	public bool dirty = true;

	public Func<bool> OptionEnabledCondition = () => true;

	public Func<bool> OptionInteractableCondition = () => true;

	public Callback OnUpdate = delegate
	{
	};

	public Callback OnFixedUpdate = delegate
	{
	};

	public Callback OnLateUpdate = delegate
	{
	};

	public Callback OnRenderObject = delegate
	{
	};

	public Callback OnResize = delegate
	{
	};

	public bool lastEnabledState;

	public bool lastInteractibleState = true;

	public bool Dirty
	{
		get
		{
			return dirty;
		}
		set
		{
			dirty = value;
		}
	}

	public DialogGUIBase(params DialogGUIBase[] list)
	{
		if (list != null && list.Length != 0)
		{
			children.AddRange(list);
		}
	}

	public void AddChild(DialogGUIBase child)
	{
		children.Add(child);
	}

	public void AddChildren(DialogGUIBase[] c)
	{
		children.AddRange(c);
	}

	public virtual void Update()
	{
		if (!uiItem)
		{
			return;
		}
		OnUpdate();
		foreach (DialogGUIBase child in children)
		{
			child.Update();
		}
		if (toolTip != null)
		{
			toolTip.textString = tooltipText;
		}
		if (OptionInteractableCondition != null && !OptionInteractableCondition() && lastInteractibleState && uiItem.activeSelf)
		{
			lastInteractibleState = false;
			Selectable[] componentsInChildren = uiItem.GetComponentsInChildren<Selectable>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].interactable = false;
			}
			Graphic[] componentsInChildren2 = uiItem.GetComponentsInChildren<Graphic>();
			foreach (Graphic graphic in componentsInChildren2)
			{
				graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, 0.5f);
			}
		}
		else if (OptionInteractableCondition != null && OptionInteractableCondition() && !lastInteractibleState && uiItem.activeSelf)
		{
			lastInteractibleState = true;
			Selectable[] componentsInChildren = uiItem.GetComponentsInChildren<Selectable>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].interactable = true;
			}
			Graphic[] componentsInChildren2 = uiItem.GetComponentsInChildren<Graphic>();
			foreach (Graphic graphic2 in componentsInChildren2)
			{
				graphic2.color = new Color(graphic2.color.r, graphic2.color.g, graphic2.color.b, 1f);
			}
		}
		if (OptionEnabledCondition != null && !OptionEnabledCondition() && uiItem.gameObject.activeSelf)
		{
			uiItem.gameObject.SetActive(value: false);
		}
		else if (OptionEnabledCondition != null && OptionEnabledCondition() && !uiItem.gameObject.activeSelf)
		{
			uiItem.gameObject.SetActive(value: true);
		}
		if (lastEnabledState != uiItem.activeInHierarchy || Dirty)
		{
			lastEnabledState = uiItem.activeInHierarchy;
			if (lastEnabledState)
			{
				Resize();
			}
		}
		if (Dirty)
		{
			Dirty = false;
		}
	}

	public virtual void Resize()
	{
		if (!uiItem || !uiItem.activeInHierarchy)
		{
			return;
		}
		foreach (DialogGUIBase child in children)
		{
			child.Resize();
			if (child.Dirty)
			{
				Dirty = true;
			}
		}
		OnResize();
	}

	public virtual GameObject Create(ref Stack<Transform> layouts, UISkinDef skin)
	{
		if (uiItem == null)
		{
			Debug.Log("uiItem null");
		}
		if (uiItem != null)
		{
			toolTip = uiItem.GetComponent<TooltipController_Text>();
		}
		lastEnabledState = false;
		lastInteractibleState = true;
		dirty = true;
		if (children.Count > 0)
		{
			layouts.Push(uiItem.transform);
			foreach (DialogGUIBase child in children)
			{
				child.Create(ref layouts, skin);
			}
			layouts.Pop();
		}
		return uiItem;
	}

	public void SetupTransformAndLayout()
	{
		uiItem.GetComponent<RectTransform>().localScale = Vector3.one;
		uiItem.GetComponent<RectTransform>().sizeDelta = size;
		LayoutElement layoutElement = uiItem.GetComponent<LayoutElement>();
		if (layoutElement == null)
		{
			layoutElement = uiItem.AddComponent<LayoutElement>();
		}
		if (size.y != -1f && size.y != 0f)
		{
			layoutElement.minHeight = ((layoutElement.minHeight < size.y) ? size.y : layoutElement.minHeight);
			layoutElement.preferredHeight = ((layoutElement.preferredHeight < size.y) ? size.y : layoutElement.preferredHeight);
		}
		else if (size.y != 0f && layoutElement.minHeight <= 1f)
		{
			layoutElement.flexibleHeight = 1f;
		}
		if (size.x != -1f && size.x != 0f)
		{
			layoutElement.minWidth = ((layoutElement.minWidth < size.x) ? size.x : layoutElement.minWidth);
			layoutElement.preferredWidth = ((layoutElement.preferredWidth < size.x) ? size.x : layoutElement.preferredWidth);
		}
		else if (size.x != 0f && layoutElement.minWidth <= 1f)
		{
			layoutElement.flexibleWidth = 1f;
		}
		if (!flexibleHeight)
		{
			layoutElement.flexibleHeight = 0f;
		}
	}

	public static void SetUpTextObject(TextMeshProUGUI text, string value, UIStyle style, UISkinDef skin, bool ignoreStyleColor = false)
	{
		text.text = value;
		text.font = UISkinManager.TMPFont;
		text.fontSize = ((style.fontSize == 0) ? 12 : style.fontSize);
		text.fontStyle = TMPProUtil.FontStyle(style.fontStyle);
		if (!ignoreStyleColor)
		{
			text.color = style.normal.textColor;
		}
		text.alignment = TMPProUtil.TextAlignment(style.alignment);
	}

	public static bool SelectFirstItem(DialogGUIBase[] items)
	{
		DialogGUIBase[] array = items;
		int num = 0;
		DialogGUIBase dialogGUIBase;
		while (true)
		{
			if (num < array.Length)
			{
				dialogGUIBase = array[num];
				if (dialogGUIBase is DialogGUIButton || dialogGUIBase is DialogGUISlider || dialogGUIBase is DialogGUIToggle || dialogGUIBase is DialogGUIToggleButton)
				{
					break;
				}
				num++;
				continue;
			}
			array = items;
			num = 0;
			while (true)
			{
				if (num < array.Length)
				{
					if (SelectFirstItem(array[num].children.ToArray()))
					{
						break;
					}
					num++;
					continue;
				}
				return false;
			}
			return true;
		}
		EventSystem.current.SetSelectedGameObject(dialogGUIBase.uiItem);
		return true;
	}

	public void SetOptionText(string text)
	{
		TextMeshProUGUI textObject = GetTextObject();
		if (textObject != null)
		{
			textObject.text = text;
		}
		OptionText = text;
	}

	public virtual TextMeshProUGUI GetTextObject()
	{
		return null;
	}
}

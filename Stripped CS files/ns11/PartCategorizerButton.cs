using System;
using ns12;
using ns2;
using ns5;
using ns9;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns11;

public class PartCategorizerButton : MonoBehaviour
{
	public UIListItem container;

	public UIRadioButton btnToggleGeneric;

	public Button btnGeneric;

	public PointerEnterExitHandler hoverHandler;

	public Image divider;

	public Image dividerBottom;

	public Image dividerOverlay;

	public Image dividerSpaceUnder;

	public RawImage iconSpriteToggle;

	public RawImage iconSprite;

	public TooltipController_Text tooltipController;

	[NonSerialized]
	public string categoryName;

	[NonSerialized]
	public string categorydisplayName;

	[NonSerialized]
	public Icon icon;

	public Callback OnBtnTap = delegate
	{
	};

	public bool lastBtn;

	public bool isHovering;

	public Color colorWhite = new Color(1f, 1f, 1f, 0.7f);

	public Color colorBlack = new Color(0f, 0f, 0f, 0.7f);

	public string displayCategoryName => Localizer.Format(categorydisplayName);

	public UIRadioButton activeButton { get; set; }

	public Color color { get; set; }

	public Color iconColor { get; set; }

	public bool IsToggleBtn { get; set; }

	public bool LastBtn
	{
		get
		{
			return lastBtn;
		}
		set
		{
			lastBtn = value;
			divider.gameObject.SetActive(!value);
			dividerBottom.gameObject.SetActive(value);
		}
	}

	public void InitializeToggleBtn(string categoryName, string categorydisplayName, Icon icon, Color color, Color iconColor, bool last = false)
	{
		IsToggleBtn = true;
		activeButton = btnToggleGeneric;
		btnToggleGeneric.gameObject.SetActive(value: true);
		btnGeneric.gameObject.SetActive(value: false);
		this.categoryName = categoryName;
		this.categorydisplayName = categorydisplayName;
		tooltipController.textString = categorydisplayName;
		this.iconColor = iconColor;
		this.icon = icon;
		SetIcon(selected: false);
		this.color = color;
		activeButton.Image.color = color;
		activeButton.onTrueBtn.AddListener(OnTrue);
		activeButton.onFalseBtn.AddListener(OnFalse);
		LastBtn = last;
	}

	public void InitializeBtn(string categoryName, string categorydisplayName, Icon icon, Color color, Color iconColor)
	{
		IsToggleBtn = false;
		btnToggleGeneric.gameObject.SetActive(value: false);
		btnGeneric.gameObject.SetActive(value: true);
		this.categoryName = categoryName;
		this.categorydisplayName = categorydisplayName;
		tooltipController.textString = categorydisplayName;
		this.iconColor = iconColor;
		this.icon = icon;
		SetIcon(selected: false);
		this.color = color;
		btnGeneric.image.color = color;
		btnGeneric.onClick.AddListener(MouseInput_click);
		hoverHandler.onPointerEnter.AddListener(MouseInput_pointerEnter);
		hoverHandler.onPointerEnter.AddListener(MouseInput_pointerExit);
		LastBtn = true;
	}

	public void OnTrue(UIRadioButton button, UIRadioButton.CallType callType, PointerEventData data)
	{
		SetIcon(selected: true);
	}

	public void OnFalse(UIRadioButton button, UIRadioButton.CallType callType, PointerEventData data)
	{
		SetIcon(selected: false);
	}

	public void SetIcon(Icon icon)
	{
		this.icon = icon;
		UpdateIconState();
	}

	public void UpdateIconState()
	{
		if (activeButton.Value)
		{
			SetIcon(selected: true);
		}
		else
		{
			SetIcon(selected: false);
		}
	}

	public void ForceAspect()
	{
		if (!(icon.iconSelected == null))
		{
			float num = (float)icon.iconSelected.width / (float)icon.iconSelected.height;
			if (num < 1f)
			{
				iconSprite.rectTransform.sizeDelta = new Vector2(Mathf.Floor(32f * num), 32f);
				iconSpriteToggle.rectTransform.sizeDelta = new Vector2(Mathf.Floor(32f * num), 32f);
			}
			else
			{
				num = (float)icon.iconSelected.height / (float)icon.iconSelected.width;
				iconSprite.rectTransform.sizeDelta = new Vector2(32f, Mathf.Floor(32f * num));
				iconSpriteToggle.rectTransform.sizeDelta = new Vector2(32f, Mathf.Floor(32f * num));
			}
		}
	}

	public void SetRadioGroup(int group)
	{
		activeButton.SetGroup(group);
	}

	public void DisableDividers()
	{
		divider.gameObject.SetActive(value: false);
		dividerBottom.gameObject.SetActive(value: false);
		dividerOverlay.gameObject.SetActive(value: false);
		dividerSpaceUnder.gameObject.SetActive(value: false);
	}

	public void EnableDividerOverlay()
	{
		divider.gameObject.SetActive(value: false);
		dividerBottom.gameObject.SetActive(value: false);
		dividerOverlay.gameObject.SetActive(value: true);
		dividerSpaceUnder.gameObject.SetActive(value: false);
		container.GetComponent<LayoutElement>().preferredHeight = 70f;
		dividerOverlay.rectTransform.anchoredPosition = new Vector2(dividerOverlay.rectTransform.anchoredPosition.x, 0f);
		RectTransform obj = activeButton.transform as RectTransform;
		obj.anchoredPosition = new Vector2(obj.anchoredPosition.x, -17f);
	}

	public void EnableDividerSpaceUnder()
	{
		divider.gameObject.SetActive(value: false);
		dividerBottom.gameObject.SetActive(value: false);
		dividerOverlay.gameObject.SetActive(value: false);
		dividerSpaceUnder.gameObject.SetActive(value: true);
		container.GetComponent<LayoutElement>().preferredHeight = 53f;
		dividerSpaceUnder.rectTransform.anchoredPosition = new Vector2(dividerSpaceUnder.rectTransform.anchoredPosition.x, 0f);
		RectTransform obj = activeButton.transform as RectTransform;
		obj.anchoredPosition = new Vector2(obj.anchoredPosition.x, 0f);
	}

	public void MouseInput_click()
	{
		OnBtnTap();
	}

	public void MouseInput_pointerEnter(PointerEventData eventData)
	{
		if (!isHovering)
		{
			isHovering = true;
			SetIcon(selected: true);
		}
	}

	public void MouseInput_pointerExit(PointerEventData eventData)
	{
		isHovering = false;
		SetIcon(selected: false);
	}

	public void SetIcon(bool selected)
	{
		if (icon.simple)
		{
			if (selected)
			{
				iconSprite.texture = icon.iconSelected;
				iconSprite.color = colorWhite;
				iconSpriteToggle.texture = icon.iconSelected;
				iconSpriteToggle.color = colorWhite;
			}
			else
			{
				iconSprite.texture = icon.iconSelected;
				iconSprite.color = colorBlack;
				iconSpriteToggle.texture = icon.iconSelected;
				iconSpriteToggle.color = colorBlack;
			}
		}
		else if (selected)
		{
			iconSprite.texture = icon.iconSelected;
			iconSprite.color = iconColor;
			iconSpriteToggle.texture = icon.iconSelected;
			iconSpriteToggle.color = iconColor;
		}
		else
		{
			iconSprite.texture = icon.iconNormal;
			iconSprite.color = iconColor;
			iconSpriteToggle.texture = icon.iconNormal;
			iconSpriteToggle.color = iconColor;
		}
	}
}

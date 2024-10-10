using ns12;
using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns11;

public class EditorActionPartItem : UISelectableGridLayoutGroupItem, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler, ISubmitHandler
{
	public TextMeshProUGUI text;

	public LayoutElement layoutElement;

	public UIButtonToggle invertButton;

	public UIButtonToggle modeButton;

	public Slider speedMultiplierSlider;

	public TextMeshProUGUI speedMultiplierText;

	public TooltipController_Text invertTooltip;

	public TooltipController_Text modeTooltip;

	[SerializeField]
	public TooltipController_Text incrementalSpeedTooltip;

	public string[] invertTooltipText;

	public string[] modeTooltipText;

	public BaseAction evt { get; set; }

	public BaseAxisField axisField { get; set; }

	public EditorActionPartSelector selector { get; set; }

	public int selectedGroup { get; set; }

	public int groupOverride { get; set; }

	public uint SelectedControllerId { get; set; }

	public EditorActionGroupType selectedGroupType { get; set; }

	public bool addToGroup { get; set; }

	public void OnDestroy()
	{
		if (speedMultiplierSlider != null)
		{
			speedMultiplierSlider.onValueChanged.RemoveListener(OnSpeedSliderValueChanged);
			if (axisField != null)
			{
				axisField.OnAxisSpeedChanged -= UpdateSliderValue;
			}
		}
	}

	public static KSPAxisGroup GetAxisIncremental(BaseAxisField axisField, int groupOverride)
	{
		if (groupOverride > 0)
		{
			return axisField.overrideIncremental[groupOverride - 1];
		}
		return axisField.axisIncremental;
	}

	public static KSPAxisGroup GetAxisInverted(BaseAxisField axisField, int groupOverride)
	{
		if (groupOverride > 0)
		{
			return axisField.overrideInverted[groupOverride - 1];
		}
		return axisField.axisInverted;
	}

	public static void SetAxisIncremental(BaseAxisField axisField, int groupOverride, KSPAxisGroup value)
	{
		if (groupOverride > 0)
		{
			axisField.overrideIncremental[groupOverride - 1] = value;
		}
		else
		{
			axisField.axisIncremental = value;
		}
	}

	public static void SetAxisInverted(BaseAxisField axisField, int groupOverride, KSPAxisGroup value)
	{
		if (groupOverride > 0)
		{
			axisField.overrideInverted[groupOverride - 1] = value;
		}
		else
		{
			axisField.axisInverted = value;
		}
	}

	public void Setup(string text, KSPActionGroup selectedGroup, int groupOverride, EditorActionPartSelector selector, BaseAction evt, bool addToGroup)
	{
		this.text.text = text;
		this.selectedGroup = (int)selectedGroup;
		this.groupOverride = groupOverride;
		SelectedControllerId = 0u;
		selectedGroupType = EditorActionGroupType.Action;
		this.selector = selector;
		this.evt = evt;
		axisField = null;
		this.addToGroup = addToGroup;
		if (invertButton != null)
		{
			invertButton.gameObject.SetActive(value: false);
		}
		if (modeButton != null)
		{
			modeButton.gameObject.SetActive(value: false);
		}
		RefreshAxisField();
	}

	public void Setup(string text, KSPAxisGroup selectedGroup, int groupOverride, EditorActionPartSelector selector, BaseAxisField axisField, bool addToGroup)
	{
		this.text.text = text;
		this.selectedGroup = (int)selectedGroup;
		this.groupOverride = groupOverride;
		SelectedControllerId = 0u;
		selectedGroupType = EditorActionGroupType.Axis;
		this.selector = selector;
		evt = null;
		this.axisField = axisField;
		this.addToGroup = addToGroup;
		if (invertButton != null)
		{
			invertButton.gameObject.SetActive(value: true);
			invertButton.onToggle.AddListener(ToggleInvert);
			invertButton.SetState((GetAxisInverted(axisField, groupOverride) & selectedGroup) != 0);
			invertTooltipText = new string[2];
			invertTooltipText[0] = Localizer.Format("#autoLOC_6013020", "#autoLOC_6013021");
			invertTooltipText[1] = Localizer.Format("#autoLOC_6013020", "#autoLOC_6013022");
			invertTooltip = invertButton.gameObject.GetComponent<TooltipController_Text>();
			ToggleInvert();
		}
		if (modeButton != null)
		{
			if (selectedGroup != KSPAxisGroup.MainThrottle)
			{
				modeButton.gameObject.SetActive(value: true);
				modeButton.onToggle.AddListener(ToggleMode);
				modeButton.SetState((GetAxisIncremental(axisField, groupOverride) & selectedGroup) != 0);
				modeTooltipText = new string[2];
				modeTooltipText[0] = Localizer.Format("#autoLOC_6013023", Localizer.Format("#autoLOC_6013024", "#autoLOC_6013026"), "#autoLOC_6013027");
				modeTooltipText[1] = Localizer.Format("#autoLOC_6013023", Localizer.Format("#autoLOC_6013025", "#autoLOC_6013027"), "#autoLOC_6013026");
				modeTooltip = modeButton.gameObject.GetComponent<TooltipController_Text>();
				ToggleMode();
			}
			else
			{
				modeButton.gameObject.SetActive(value: false);
			}
		}
		if (speedMultiplierSlider != null)
		{
			axisField.OnAxisSpeedChanged += UpdateSliderValue;
			speedMultiplierSlider.gameObject.SetActive(value: true);
			if (incrementalSpeedTooltip != null)
			{
				incrementalSpeedTooltip.SetText(Localizer.Format("#autoLOC_6006091"));
			}
			speedMultiplierSlider.onValueChanged.AddListener(OnSpeedSliderValueChanged);
			speedMultiplierSlider.minValue = 0f;
			if (axisField.incrementalSpeedMultiplier == 0f)
			{
				speedMultiplierSlider.value = GameSettings.GetAxiSpeedMultiplierIndex(GameSettings.AXIS_INCREMENTAL_SPEED_MULTIPLIER_DEFAULT);
			}
			else
			{
				speedMultiplierSlider.value = GameSettings.GetAxiSpeedMultiplierIndex(axisField.incrementalSpeedMultiplier);
			}
			if (GameSettings.AXIS_INCREMENTAL_SPEED_MULTIPLIER_VALUES != null && GameSettings.AXIS_INCREMENTAL_SPEED_MULTIPLIER_VALUES.Count >= 1)
			{
				speedMultiplierSlider.maxValue = GameSettings.AXIS_INCREMENTAL_SPEED_MULTIPLIER_VALUES.Count - 1;
			}
			else
			{
				speedMultiplierSlider.maxValue = 0f;
			}
		}
		if (speedMultiplierText != null)
		{
			speedMultiplierText.gameObject.SetActive(value: true);
		}
	}

	public void Setup(string text, uint selectedControllerId, EditorActionPartSelector selector, BaseAxisField axisField, BaseAction evt, bool addToGroup)
	{
		this.text.text = text;
		selectedGroup = 0;
		SelectedControllerId = selectedControllerId;
		selectedGroupType = EditorActionGroupType.Controller;
		this.selector = selector;
		this.evt = evt;
		this.axisField = axisField;
		this.addToGroup = addToGroup;
		if (invertButton != null)
		{
			invertButton.gameObject.SetActive(value: false);
		}
		if (modeButton != null)
		{
			modeButton.gameObject.SetActive(value: false);
		}
		RefreshAxisField();
	}

	public void Setup(string text, uint selectedControllerId, EditorActionPartSelector selector, BaseAxisField axisField, bool addToGroup)
	{
		this.text.text = text;
		selectedGroup = 0;
		SelectedControllerId = selectedControllerId;
		selectedGroupType = EditorActionGroupType.Controller;
		this.selector = selector;
		evt = null;
		this.axisField = axisField;
		this.addToGroup = addToGroup;
		if (invertButton != null)
		{
			invertButton.gameObject.SetActive(value: false);
		}
		if (modeButton != null)
		{
			modeButton.gameObject.SetActive(value: false);
		}
		RefreshAxisField();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		selector.HighLight(highLight: true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		selector.HighLight(highLight: false);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		ItemClicked();
	}

	public void ToggleInvert()
	{
		KSPAxisGroup axisInverted = GetAxisInverted(axisField, groupOverride);
		if (invertButton.state)
		{
			axisInverted = (KSPAxisGroup)((int)axisInverted | selectedGroup);
			invertTooltip.textString = invertTooltipText[1];
		}
		else
		{
			axisInverted = (KSPAxisGroup)((int)axisInverted & ~selectedGroup);
			invertTooltip.textString = invertTooltipText[0];
		}
		SetAxisInverted(axisField, groupOverride, axisInverted);
		Part part = selector.part;
		int num = part.Modules.IndexOf(axisField.module);
		int count = part.symmetryCounterparts.Count;
		while (count-- > 0)
		{
			PartModule partModule = part.symmetryCounterparts[count].Modules[num];
			if (!(partModule == null) && partModule.Fields[axisField.name] is BaseAxisField baseAxisField)
			{
				SetAxisInverted(baseAxisField, groupOverride, axisInverted);
			}
		}
	}

	public void ToggleMode()
	{
		KSPAxisGroup axisIncremental = GetAxisIncremental(axisField, groupOverride);
		if (modeButton.state)
		{
			axisIncremental = (KSPAxisGroup)((int)axisIncremental | selectedGroup);
			modeTooltip.textString = modeTooltipText[1];
		}
		else
		{
			axisIncremental = (KSPAxisGroup)((int)axisIncremental & ~selectedGroup);
			modeTooltip.textString = modeTooltipText[0];
		}
		SetAxisIncremental(axisField, groupOverride, axisIncremental);
		Part part = selector.part;
		int num = part.Modules.IndexOf(axisField.module);
		int count = part.symmetryCounterparts.Count;
		while (count-- > 0)
		{
			PartModule partModule = part.symmetryCounterparts[count].Modules[num];
			if (!(partModule == null) && partModule.Fields[axisField.name] is BaseAxisField baseAxisField)
			{
				SetAxisIncremental(baseAxisField, groupOverride, axisIncremental);
			}
		}
	}

	public void OnSubmit(BaseEventData eventData)
	{
		ItemClicked();
	}

	public void ItemClicked()
	{
		if ((selectedGroupType == EditorActionGroupType.Controller && SelectedControllerId == 0) || (axisField == null && evt == null))
		{
			selector.Select();
			return;
		}
		if (addToGroup)
		{
			EditorActionGroups.Instance.AddActionToGroup(this);
			return;
		}
		EditorActionGroups.Instance.RemoveActionFromGroup(this);
		selector.HighLight(highLight: false);
	}

	public void OnSpeedSliderValueChanged(float value)
	{
		float axisSpeedMultiplier = GameSettings.GetAxisSpeedMultiplier((int)value);
		axisField.incrementalSpeedMultiplier = axisSpeedMultiplier;
		if (speedMultiplierText != null)
		{
			speedMultiplierText.text = Localizer.Format("#autoLOC_6006092", axisSpeedMultiplier * 100f);
		}
		Part part = selector.part;
		int num = part.Modules.IndexOf(axisField.module);
		int count = part.symmetryCounterparts.Count;
		while (count-- > 0)
		{
			PartModule partModule = part.symmetryCounterparts[count].Modules[num];
			if (!(partModule == null) && partModule.Fields[axisField.name] is BaseAxisField baseAxisField)
			{
				baseAxisField.SetIncrementalSpeedMultiplier(axisSpeedMultiplier);
			}
		}
	}

	public void UpdateSliderValue()
	{
		if (speedMultiplierSlider != null)
		{
			speedMultiplierSlider.value = GameSettings.GetAxiSpeedMultiplierIndex(axisField.incrementalSpeedMultiplier);
		}
	}

	public void RefreshAxisField()
	{
		if (speedMultiplierSlider != null)
		{
			speedMultiplierSlider.gameObject.SetActive(value: false);
			RectTransform rectTransform = speedMultiplierSlider.transform as RectTransform;
			layoutElement.preferredHeight -= rectTransform.sizeDelta.y;
			if (axisField != null)
			{
				axisField.OnAxisSpeedChanged += UpdateSliderValue;
			}
		}
		if (speedMultiplierText != null)
		{
			speedMultiplierText.gameObject.SetActive(value: false);
		}
	}
}

using ns12;
using ns2;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPartActionResourceEditor : UIPartActionResourceItem
{
	public TextMeshProUGUI resourceName;

	public TextMeshProUGUI resourceAmnt;

	public TextMeshProUGUI resourceMax;

	public Slider slider;

	public GameObject sliderContainer;

	public GameObject numericContainer;

	public TextMeshProUGUI fieldNameNumeric;

	public TMP_InputField inputField;

	public UIButtonToggle flowBtn;

	public static float StepIncrement = 0.1f;

	public int displayNameLimit = 14;

	public TooltipController_Text tooltip;

	public string displayText;

	public bool bypassSliderRounding;

	public override void Setup(UIPartActionWindow window, Part part, UI_Scene scene, UI_Control control, PartResource resource)
	{
		base.Setup(window, part, scene, control, resource);
		tooltip = base.gameObject.GetComponent<TooltipController_Text>();
		if (resource.info.displayName.LocalizeRemoveGender().Length > displayNameLimit && resource.info.displayName != "Electric Charge")
		{
			resourceName.text = resource.info.abbreviation;
			if (tooltip != null)
			{
				tooltip.SetText(resource.info.displayName.LocalizeRemoveGender());
			}
		}
		else
		{
			resourceName.text = resource.info.displayName.LocalizeRemoveGender();
			if (tooltip != null)
			{
				tooltip.SetText(string.Empty);
				tooltip.enabled = false;
			}
		}
		displayText = resourceName.text;
		resourceMax.text = KSPUtil.LocalizeNumber(resource.maxAmount, "F1");
		slider.value = (float)(resource.amount / resource.maxAmount);
		bypassSliderRounding = true;
		slider.onValueChanged.AddListener(OnSliderChanged);
		inputField.onEndEdit.AddListener(OnFieldInput);
		inputField.onSelect.AddListener(AddInputFieldLock);
		GameEvents.onPartActionNumericSlider.Add(ToggleNumericSlider);
		ToggleNumericSlider(GameSettings.PAW_NUMERIC_SLIDERS);
		window.usingNumericValue = true;
		if (resource.info.resourceFlowMode == ResourceFlowMode.NO_FLOW)
		{
			flowBtn.gameObject.SetActive(value: false);
		}
		else
		{
			flowBtn.onToggle.AddListener(FlowBtnToggled);
			SetButtonState(resource.flowState, forceButton: true);
		}
		onSliderChangeProcess();
	}

	public void OnDestroy()
	{
		GameEvents.onPartActionNumericSlider.Remove(ToggleNumericSlider);
	}

	public override bool IsItemValid()
	{
		if (!(part == null) && part.State != PartStates.DEAD && resource != null)
		{
			if (scene == UI_Scene.Flight)
			{
				return false;
			}
			return true;
		}
		return false;
	}

	public override void UpdateItem()
	{
		if ((flowBtn.state && !resource.flowState) || (!flowBtn.state && resource.flowState))
		{
			SetButtonState(resource.flowState, forceButton: true);
		}
		slider.value = (float)(resource.amount / resource.maxAmount);
		fieldNameNumeric.text = displayText;
		if (!inputField.isFocused)
		{
			inputField.text = KSPUtil.LocalizeNumber(resource.amount, "#.0##");
		}
	}

	public void OnSliderChanged(float obj)
	{
		onSliderChangeProcess();
		GameEvents.onEditorShipModified.Fire(EditorLogic.fetch.ship);
	}

	public void onSliderChangeProcess()
	{
		float num = slider.value;
		if (!bypassSliderRounding)
		{
			num = Mathf.Round(slider.value * 10f) / 10f;
		}
		bypassSliderRounding = false;
		resource.amount = (double)num * resource.maxAmount;
		if (scene == UI_Scene.Editor)
		{
			SetSymCounterpartsAmount(resource.amount);
		}
		resourceAmnt.text = KSPUtil.LocalizeNumber(resource.amount, "F1");
	}

	public void FlowBtnToggled()
	{
		if (InputLockManager.IsUnlocked((control == null || !control.requireFullControl) ? ControlTypes.TWEAKABLES_ANYCONTROL : ControlTypes.TWEAKABLES_FULLONLY))
		{
			SetButtonState(!resource.flowState, forceButton: false);
		}
	}

	public void SetButtonState(bool state, bool forceButton)
	{
		if (resource.flowState != state)
		{
			resource.flowState = state;
			if (scene == UI_Scene.Editor)
			{
				SetSymCounterpartsFlowState(resource.flowState);
			}
		}
		if (forceButton)
		{
			flowBtn.SetState(state);
		}
	}

	public void OnFieldInput(string input)
	{
		float result = 0f;
		if (float.TryParse(input, out result))
		{
			resource.amount = Mathf.Clamp(result, 0f, (float)resource.maxAmount);
			bypassSliderRounding = true;
			UpdateItem();
		}
		RemoveInputfieldLock();
	}

	public void ToggleNumericSlider(bool numeric)
	{
		sliderContainer.SetActive(!numeric);
		numericContainer.SetActive(numeric);
		UpdateItem();
	}
}

using ns12;
using ns2;
using TMPro;
using UnityEngine.UI;

public class UIPartActionResource : UIPartActionResourceItem
{
	public TextMeshProUGUI resourceName;

	public TextMeshProUGUI resourceAmnt;

	public TextMeshProUGUI resourceMax;

	public Slider progBar;

	public UIButtonToggle flowBtn;

	public int displayNameLimit = 14;

	public TooltipController_Text tooltip;

	public override void Setup(UIPartActionWindow window, Part part, UI_Scene scene, UI_Control control, PartResource resource)
	{
		base.Setup(window, part, scene, control, resource);
		tooltip = base.gameObject.GetComponent<TooltipController_Text>();
		if (resource.info.displayName.Length > displayNameLimit && resource.info.displayName != "Electric Charge")
		{
			resourceName.text = resource.info.abbreviation;
			if (tooltip != null)
			{
				tooltip.SetText(resource.info.displayName);
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
		resourceMax.text = resource.maxAmount.ToString("F0");
		if (resource.info.resourceFlowMode != 0 && !resource.hideFlow)
		{
			flowBtn.onToggle.AddListener(FlowBtnToggled);
			SetButtonState(resource.flowState, forceButton: true);
		}
		else
		{
			flowBtn.gameObject.SetActive(value: false);
		}
	}

	public override bool IsItemValid()
	{
		if (!(part == null) && part.State != PartStates.DEAD && resource != null && resource.isVisible)
		{
			if (scene == UI_Scene.Flight && part.vessel != FlightGlobals.ActiveVessel)
			{
				return false;
			}
			if (window.Display == UIPartActionWindow.DisplayType.ResourceOnly && !UIPartActionController.Instance.resourcesShown.Contains(resource.info.id))
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
		if (resource.amount < 100.0)
		{
			resourceAmnt.text = KSPUtil.LocalizeNumber(resource.amount, "F2");
		}
		else
		{
			resourceAmnt.text = KSPUtil.LocalizeNumber(resource.amount, "F0");
		}
		if (resource.maxAmount < 100.0)
		{
			resourceMax.text = KSPUtil.LocalizeNumber(resource.maxAmount, "F2");
		}
		else
		{
			resourceMax.text = KSPUtil.LocalizeNumber(resource.maxAmount, "F0");
		}
		progBar.value = (float)(resource.amount / resource.maxAmount);
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
		resource.flowState = state;
		flowBtn.SetState(state);
	}
}

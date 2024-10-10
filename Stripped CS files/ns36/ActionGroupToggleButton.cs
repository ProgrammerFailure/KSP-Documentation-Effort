using ns2;
using UnityEngine;
using UnityEngine.UI;

namespace ns36;

public class ActionGroupToggleButton : MonoBehaviour
{
	public UIButtonToggle toggle;

	public Button toggleBtn;

	public KSPActionGroup group;

	public bool state;

	public bool stLast;

	public bool ready;

	public bool isUnlocked;

	public void Awake()
	{
		toggle = GetComponent<UIButtonToggle>();
		if (toggle != null)
		{
			toggle.onToggle.AddListener(SetToggle);
		}
		if (toggleBtn != null)
		{
			toggleBtn.onClick.AddListener(SetToggle);
		}
		ready = false;
	}

	public void SetToggle()
	{
		if (!FlightGlobals.ready)
		{
			return;
		}
		if (FlightGlobals.ActiveVessel.ActionGroups.CooldownTimeRemaining(group) <= 0.0)
		{
			if (toggle != null)
			{
				state = toggle.state;
				toggle.interactable = false;
			}
			else
			{
				state = !state;
			}
			if (toggleBtn != null)
			{
				toggleBtn.interactable = false;
			}
			FlightGlobals.ActiveVessel.ActionGroups.SetGroup(group, state);
		}
		if (FlightGlobals.ActiveVessel.isEVA)
		{
			if (group == KSPActionGroup.flag_5)
			{
				FlightGlobals.ActiveVessel.evaController.ToggleJetpack();
			}
			else if (group == KSPActionGroup.Light)
			{
				FlightGlobals.ActiveVessel.evaController.ToggleLamp();
			}
		}
	}

	public void LateUpdate()
	{
		if (toggle == null && toggleBtn == null)
		{
			return;
		}
		if (!ready)
		{
			if (!FlightGlobals.ready || FlightGlobals.ActiveVessel == null)
			{
				return;
			}
			state = FlightGlobals.ActiveVessel.ActionGroups[group];
			stLast = !state;
			ready = true;
		}
		state = FlightGlobals.ActiveVessel.ActionGroups[group];
		if (state != stLast)
		{
			stLast = state;
			if (toggle != null)
			{
				toggle.SetState(state);
			}
		}
		if (FlightGlobals.ActiveVessel.ActionGroups.CooldownTimeRemaining(group) > 0.0)
		{
			if (toggle != null && toggle.interactable)
			{
				toggle.interactable = false;
			}
			if (toggleBtn != null && toggleBtn.interactable)
			{
				toggleBtn.interactable = false;
			}
			return;
		}
		switch (group)
		{
		case KSPActionGroup.flag_5:
			isUnlocked = InputLockManager.IsUnlocked(ControlTypes.flag_27);
			break;
		case KSPActionGroup.Stage:
			isUnlocked = InputLockManager.IsUnlocked(ControlTypes.GROUP_STAGE);
			break;
		case KSPActionGroup.Gear:
			isUnlocked = InputLockManager.IsUnlocked(ControlTypes.GROUP_GEARS);
			break;
		case KSPActionGroup.Light:
			isUnlocked = InputLockManager.IsUnlocked(ControlTypes.GROUP_LIGHTS);
			break;
		case KSPActionGroup.Abort:
			isUnlocked = InputLockManager.IsUnlocked(ControlTypes.GROUP_ABORT);
			break;
		default:
			isUnlocked = true;
			break;
		case KSPActionGroup.Brakes:
			isUnlocked = InputLockManager.IsUnlocked(ControlTypes.GROUP_BRAKES);
			break;
		case KSPActionGroup.flag_6:
			if (FlightGlobals.ActiveVessel.isEVA)
			{
				isUnlocked = true;
			}
			else
			{
				isUnlocked = InputLockManager.IsUnlocked(ControlTypes.flag_8);
			}
			break;
		}
		if (toggle != null)
		{
			toggle.interactable = isUnlocked;
		}
		if (toggleBtn != null)
		{
			toggleBtn.interactable = isUnlocked;
		}
	}
}

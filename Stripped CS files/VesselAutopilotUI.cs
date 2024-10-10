using System.Collections.Generic;
using ns2;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class VesselAutopilotUI : MonoBehaviour
{
	public UIStateToggleButton[] modeButtons;

	public static int updateWaitFrames = 20;

	public int requiresUpdate = updateWaitFrames;

	public VesselAutopilot.AutopilotMode[] activeModes = new VesselAutopilot.AutopilotMode[0];

	public Canvas canvas;

	public bool uiIsActive = true;

	public void Awake()
	{
		canvas = GetComponent<Canvas>();
	}

	public void Start()
	{
		int i = 0;
		for (int num = modeButtons.Length; i < num; i++)
		{
			int itr = i;
			modeButtons[i].onClick.AddListener(delegate
			{
				OnClickButton(itr);
			});
		}
		SetButtonTrue(0);
		GameEvents.onVesselChange.Add(OnVesselChange);
		GameEvents.onKerbalLevelUp.Add(OnKerbalLevelUp);
		GameEvents.OnGameSettingsApplied.Add(onGameParametersChanged);
	}

	public void OnDestroy()
	{
		GameEvents.onVesselChange.Remove(OnVesselChange);
		GameEvents.onKerbalLevelUp.Remove(OnKerbalLevelUp);
	}

	public void LateUpdate()
	{
		if (requiresUpdate > 0)
		{
			requiresUpdate--;
			if (requiresUpdate == 0)
			{
				SetPilotSkill();
			}
		}
		if (!ToggleUI(FlightGlobals.ActiveVessel.ActionGroups[KSPActionGroup.flag_6] && FlightGlobals.ActiveVessel.IsControllable && !FlightGlobals.ActiveVessel.isEVA))
		{
			return;
		}
		if (activeModes.Length == 0)
		{
			if (FlightGlobals.ActiveVessel.Autopilot.Enabled)
			{
				FlightGlobals.ActiveVessel.Autopilot.Disable();
			}
			ToggleUI(enabledState: false);
			return;
		}
		if (activeModes.Length == 1)
		{
			if (!modeButtons[0].gameObject.activeSelf)
			{
				modeButtons[0].gameObject.SetActive(value: true);
			}
			if (!modeButtons[0].StateBool)
			{
				modeButtons[0].SetState(state: true);
			}
			for (int i = 1; i < modeButtons.Length; i++)
			{
				if (modeButtons[i].gameObject.activeSelf)
				{
					modeButtons[i].gameObject.SetActive(value: false);
				}
			}
			if (FlightGlobals.ActiveVessel.Autopilot.Mode != 0)
			{
				FlightGlobals.ActiveVessel.Autopilot.SetMode(VesselAutopilot.AutopilotMode.StabilityAssist);
			}
			return;
		}
		int j = 0;
		for (int num = modeButtons.Length; j < num; j++)
		{
			VesselAutopilot.AutopilotMode autopilotMode = (VesselAutopilot.AutopilotMode)j;
			if (IsSkillActive(autopilotMode))
			{
				if (!modeButtons[j].gameObject.activeSelf)
				{
					modeButtons[j].gameObject.SetActive(value: true);
				}
				if (FlightGlobals.ActiveVessel.Autopilot.CanSetMode(autopilotMode))
				{
					if (!modeButtons[j].interactable)
					{
						modeButtons[j].interactable = true;
					}
					if (FlightGlobals.ActiveVessel.Autopilot.Mode == autopilotMode)
					{
						if (!modeButtons[j].StateBool)
						{
							modeButtons[j].SetState(state: true);
						}
					}
					else if (modeButtons[j].StateBool)
					{
						modeButtons[j].SetState(state: false);
					}
				}
				else if (modeButtons[j].interactable)
				{
					modeButtons[j].interactable = false;
				}
			}
			else if (modeButtons[j].gameObject.activeSelf)
			{
				modeButtons[j].gameObject.SetActive(value: false);
			}
		}
	}

	public bool ToggleUI(bool enabledState)
	{
		uiIsActive = enabledState;
		if (canvas.enabled != uiIsActive)
		{
			canvas.enabled = uiIsActive;
		}
		return uiIsActive;
	}

	public void OnVesselChange(Vessel v)
	{
		requiresUpdate = updateWaitFrames;
		OnClickButton(0);
	}

	public void OnKerbalLevelUp(ProtoCrewMember pcm)
	{
		requiresUpdate = 2;
	}

	public void onGameParametersChanged()
	{
		requiresUpdate = 2;
	}

	public void OnClickButton(int buttonIndex)
	{
		int mode = (int)FlightGlobals.ActiveVessel.Autopilot.Mode;
		if (buttonIndex == mode)
		{
			SetButtonTrue(buttonIndex);
		}
		else if (FlightGlobals.ActiveVessel.Autopilot.SetMode((VesselAutopilot.AutopilotMode)buttonIndex))
		{
			SetButtonTrue(buttonIndex);
		}
	}

	public void SetButtonTrue(int btn)
	{
		int i = 0;
		for (int num = modeButtons.Length; i < num; i++)
		{
			if (i == btn)
			{
				if (modeButtons[i].gameObject.activeSelf && modeButtons[i].interactable && !modeButtons[i].StateBool)
				{
					modeButtons[i].SetState(state: true);
				}
			}
			else if (modeButtons[i].gameObject.activeSelf && modeButtons[i].interactable && modeButtons[i].StateBool)
			{
				modeButtons[i].SetState(state: false);
			}
		}
	}

	public int GetButtonIndex(UIStateToggleButton btn)
	{
		int num = 0;
		int num2 = modeButtons.Length;
		while (true)
		{
			if (num < num2)
			{
				if (modeButtons[num] == btn)
				{
					break;
				}
				num++;
				continue;
			}
			return -1;
		}
		return num;
	}

	public void SetPilotSkill()
	{
		List<VesselAutopilot.AutopilotMode> list = new List<VesselAutopilot.AutopilotMode>();
		int i = 0;
		for (int num = modeButtons.Length; i < num; i++)
		{
			VesselAutopilot.AutopilotMode autopilotMode = (VesselAutopilot.AutopilotMode)i;
			if (autopilotMode.AvailableAtLevel(FlightGlobals.ActiveVessel))
			{
				list.Add(autopilotMode);
			}
		}
		activeModes = list.ToArray();
	}

	public bool IsSkillActive(VesselAutopilot.AutopilotMode mode)
	{
		int num = 0;
		int num2 = activeModes.Length;
		while (true)
		{
			if (num < num2)
			{
				if (mode == activeModes[num])
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
}

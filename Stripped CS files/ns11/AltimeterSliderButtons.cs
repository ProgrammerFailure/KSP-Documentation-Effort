using System.Collections;
using System.Collections.Generic;
using Expansions.Missions;
using ns12;
using ns2;
using ns9;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns11;

public class AltimeterSliderButtons : MonoBehaviour
{
	public Button vesselRecoveryButton;

	public Button spaceCenterButton;

	public UIPanelTransition slidingTab;

	public XSelectable hoverArea;

	public GClass9 led;

	public float slidingHoverTriggerHeight = 15f;

	public int state;

	public Coroutine recoverCoroutine;

	public Coroutine returnToKSCCoroutine;

	public ClearToSaveStatus clearToSaveStatus;

	public bool recoverButtonMissionAllowed = true;

	public float combinedAltimiterUIScale = 1f;

	public bool hover;

	public void Awake()
	{
		GameEvents.onUIScaleChange.Add(UpdateUIScale);
	}

	public void Start()
	{
		TooltipController_Text component = vesselRecoveryButton.GetComponent<TooltipController_Text>();
		if (HighLogic.CurrentGame.Mode == Game.Modes.MISSION && HighLogic.CurrentGame.Parameters.CustomParams<MissionParamsGeneral>().preventVesselRecovery)
		{
			vesselRecoveryButton.gameObject.transform.SetParent(vesselRecoveryButton.gameObject.transform.parent.parent);
			vesselRecoveryButton.interactable = false;
			recoverButtonMissionAllowed = false;
			EventTrigger[] components = vesselRecoveryButton.GetComponents<EventTrigger>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].enabled = false;
			}
		}
		else
		{
			vesselRecoveryButton.onClick.AddListener(recoverVessel);
			if (component != null)
			{
				component.enabled = false;
			}
			vesselRecoveryButton.interactable = true;
			recoverButtonMissionAllowed = true;
			EventTrigger[] components2 = vesselRecoveryButton.GetComponents<EventTrigger>();
			for (int j = 0; j < components2.Length; j++)
			{
				components2[j].enabled = true;
			}
		}
		slidingHoverTriggerHeight = GameSettings.UI_POS_ALTIMETER_SLIDEDOWN_HOVER_HEIGHT;
		UpdateUIScale();
		spaceCenterButton.onClick.AddListener(returnToSpaceCenter);
		GameEvents.onFlightReady.Add(OnFlightStarted);
		GameEvents.onVesselChange.Add(OnVesselFocusChanged);
		GameEvents.onVesselSituationChange.Add(OnVesselSituationChange);
	}

	public void OnDestroy()
	{
		GameEvents.onFlightReady.Remove(OnFlightStarted);
		GameEvents.onVesselChange.Remove(OnVesselFocusChanged);
		GameEvents.onVesselSituationChange.Remove(OnVesselSituationChange);
		GameEvents.onUIScaleChange.Remove(UpdateUIScale);
	}

	public void OnFlightStarted()
	{
		UpdateForVessel(FlightGlobals.ActiveVessel);
	}

	public void OnVesselFocusChanged(Vessel v)
	{
		if (recoverCoroutine != null)
		{
			StopCoroutine(recoverCoroutine);
			recoverCoroutine = null;
		}
		if (returnToKSCCoroutine != null)
		{
			StopCoroutine(returnToKSCCoroutine);
			returnToKSCCoroutine = null;
		}
		UpdateForVessel(v);
	}

	public void OnVesselSituationChange(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> vcs)
	{
		UpdateForVessel(vcs.host);
	}

	public void UpdateForVessel(Vessel v)
	{
		if (!(v == FlightGlobals.ActiveVessel))
		{
			return;
		}
		if (v.LandedOrSplashed && v.mainBody.isHomeWorld)
		{
			if (returnToKSCCoroutine != null)
			{
				StopCoroutine(returnToKSCCoroutine);
			}
			returnToKSCCoroutine = StartCoroutine(UnlockRecovery(v));
		}
		else if (v.IsClearToSave() == ClearToSaveStatus.CLEAR)
		{
			if (recoverCoroutine != null)
			{
				StopCoroutine(recoverCoroutine);
			}
			recoverCoroutine = StartCoroutine(UnlockReturnToKSC(v));
		}
		else
		{
			setUnlock(0);
			led.setOff();
		}
	}

	public IEnumerator UnlockRecovery(Vessel v)
	{
		do
		{
			yield return null;
			if (v.horizontalSrfSpeed < 0.30000001192092896 && !FlightDriver.Pause)
			{
				if (state != 2)
				{
					setUnlock(2);
				}
			}
			else if (state != 0)
			{
				setUnlock(0);
			}
		}
		while (v.LandedOrSplashed && v.mainBody.isHomeWorld && v == FlightGlobals.ActiveVessel);
		setUnlock(0);
		led.setOff();
		recoverCoroutine = null;
	}

	public IEnumerator UnlockReturnToKSC(Vessel v)
	{
		do
		{
			yield return null;
			if (FlightInputHandler.state.isIdle && (!v.isEVA || !v.evaController.OnALadder) && (v.situation != Vessel.Situations.SUB_ORBITAL || v.heightFromTerrain > 500f || v.heightFromTerrain == -1f) && (v.LandedOrSplashed || v.geeForce < 0.1) && !FlightDriver.Pause)
			{
				if (state != 1)
				{
					setUnlock(1);
				}
			}
			else if (state != 0)
			{
				setUnlock(0);
			}
		}
		while (v.IsClearToSave() == ClearToSaveStatus.CLEAR && v == FlightGlobals.ActiveVessel);
		setUnlock(0);
		led.setOff();
		returnToKSCCoroutine = null;
	}

	public void recoverVessel()
	{
		clearToSaveStatus = FlightGlobals.ClearToSave();
		if (clearToSaveStatus == ClearToSaveStatus.CLEAR && HighLogic.CurrentGame.Parameters.Flight.CanLeaveToSpaceCenter)
		{
			GameEvents.OnVesselRecoveryRequested.Fire(FlightGlobals.ActiveVessel);
		}
		else if (clearToSaveStatus == ClearToSaveStatus.NOT_WHILE_ON_A_LADDER)
		{
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("LeaveToSpaceCenter", null, Localizer.Format("#autoLOC_360593"), HighLogic.UISkin, 450f, drawExitWithoutSaveOptions(GameScenes.SPACECENTER)), persistAcrossScenes: false, HighLogic.UISkin);
		}
	}

	public void returnToSpaceCenter()
	{
		clearToSaveStatus = FlightGlobals.ClearToSave();
		if (clearToSaveStatus == ClearToSaveStatus.CLEAR && HighLogic.CurrentGame.Parameters.Flight.CanLeaveToSpaceCenter)
		{
			GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
			HighLogic.LoadScene(GameScenes.SPACECENTER);
		}
		else if (clearToSaveStatus == ClearToSaveStatus.NOT_WHILE_ON_A_LADDER)
		{
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("LeaveToSpaceCenter", null, Localizer.Format("#autoLOC_360593"), HighLogic.UISkin, 450f, drawExitWithoutSaveOptions(GameScenes.SPACECENTER)), persistAcrossScenes: false, HighLogic.UISkin);
		}
	}

	public DialogGUIBase[] drawExitWithoutSaveOptions(GameScenes sceneToLeaveTo)
	{
		List<DialogGUIBase> list = new List<DialogGUIBase>();
		DialogGUILabel item = new DialogGUILabel(Localizer.Format("#autoLOC_360762"));
		list.Add(item);
		if (HighLogic.CurrentGame.Parameters.Flight.CanRestart)
		{
			item = new DialogGUILabel(Localizer.Format("#autoLOC_360774", KSPUtil.PrintTime(Planetarium.GetUniversalTime() - HighLogic.CurrentGame.UniversalTime, 3, explicitPositive: false)));
			list.Add(item);
			DialogGUIButton item2 = new DialogGUIButton(Localizer.Format("#autoLOC_360779"), delegate
			{
				HighLogic.LoadScene(sceneToLeaveTo);
			}, dismissOnSelect: true);
			list.Add(item2);
		}
		else
		{
			item = new DialogGUILabel(Localizer.Format("#autoLOC_360788"));
			list.Add(item);
			DialogGUIButton item3 = new DialogGUIButton(Localizer.Format("#autoLOC_360791"), delegate
			{
				saveAndExit(sceneToLeaveTo, HighLogic.CurrentGame.Updated());
			}, dismissOnSelect: true);
			list.Add(item3);
		}
		DialogGUIButton item4 = new DialogGUIButton(Localizer.Format("#autoLOC_360821"), delegate
		{
		}, dismissOnSelect: true);
		list.Add(item4);
		return list.ToArray();
	}

	public void saveAndExit(GameScenes sceneToLoad, Game stateToSave)
	{
		GamePersistence.SaveGame(stateToSave, "persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
		HighLogic.LoadScene(sceneToLoad);
	}

	public void setUnlock(int unlockState)
	{
		switch (unlockState)
		{
		default:
			led.SetColor(GClass9.colorIndices.yellow);
			led.SetOn();
			vesselRecoveryButton.Lock();
			spaceCenterButton.Lock();
			break;
		case 1:
			led.SetColor(GClass9.colorIndices.blue);
			led.SetOn();
			vesselRecoveryButton.Lock();
			spaceCenterButton.Unlock();
			break;
		case 2:
			led.SetColor(GClass9.colorIndices.green);
			led.SetOn();
			if (recoverButtonMissionAllowed)
			{
				vesselRecoveryButton.Unlock();
			}
			spaceCenterButton.Unlock();
			break;
		}
		state = unlockState;
	}

	public MonoBehaviour GetInstance()
	{
		return this;
	}

	public void LateUpdate()
	{
		if (InputLockManager.IsLocked(ControlTypes.UI_DRAGGING))
		{
			return;
		}
		if (hoverArea.Hover)
		{
			if ((!hover || state != slidingTab.StateIndex) && (float)Screen.height - Input.mousePosition.y < slidingHoverTriggerHeight * combinedAltimiterUIScale)
			{
				hover = true;
				slidingTab.Transition(state);
			}
		}
		else if (hover)
		{
			slidingTab.Transition(0);
			hover = false;
		}
	}

	public void Expand()
	{
		slidingTab.Transition(state);
	}

	public void Collapse()
	{
		slidingTab.Transition(0);
	}

	public void UpdateUIScale()
	{
		combinedAltimiterUIScale = ((UIMasterController.Instance != null) ? UIMasterController.Instance.uiScale : GameSettings.UI_SCALE) * GameSettings.UI_SCALE_ALTIMETER;
	}
}

using System.Collections;
using System.Collections.Generic;
using ns11;
using ns2;
using UnityEngine;
using UnityEngine.UI;

public class ActionGroupsFlightController : MonoBehaviour
{
	public Button saveButton;

	public Button exitButton;

	public UIPanelTransition actionGroupsTransition;

	public List<GameObject> flightUI;

	public bool isOpen;

	public float previousTimeScale;

	public ActionGroupsPanel actionGroupsPanel;

	public static ActionGroupsFlightController Instance;

	public bool IsOpen => isOpen;

	public void Awake()
	{
		if ((bool)Instance)
		{
			Object.Destroy(this);
		}
		else
		{
			Instance = this;
		}
	}

	public void Start()
	{
		exitButton.onClick.AddListener(ExitPanelActions);
		saveButton.onClick.AddListener(SaveActions);
		if (ApplicationLauncher.Instance != null && ApplicationLauncher.Instance.launcherSpace != null)
		{
			GameObject item = ApplicationLauncher.Instance.launcherSpace.gameObject;
			flightUI.Add(item);
		}
		if (ApplicationLauncher.Instance != null && ApplicationLauncher.Instance.appSpace != null)
		{
			GameObject item2 = ApplicationLauncher.Instance.appSpace.gameObject;
			flightUI.Add(item2);
		}
		isOpen = false;
	}

	public void Update()
	{
		if (GameSettings.PAUSE.GetKeyUp() && isOpen)
		{
			ExitPanelActions();
			isOpen = false;
		}
	}

	public void SelectPanelActions()
	{
		if (FlightGlobals.ActiveVessel.state == Vessel.State.DEAD || isOpen)
		{
			return;
		}
		GameEvents.onGUIActionGroupFlightShowing.Fire();
		GameEvents.onGUIActionGroupShowing.Fire();
		ActionGroupsApp.Instance.HideUnpinApp();
		for (int i = 0; i < flightUI.Count; i++)
		{
			if (flightUI[i] != null)
			{
				flightUI[i].SetActive(value: false);
			}
		}
		if (PartItemTransfer.Instance != null)
		{
			PartItemTransfer.Instance.Dismiss(PartItemTransfer.DismissAction.Cancelled, null);
		}
		if (CrewHatchController.fetch != null)
		{
			CrewHatchController.fetch.ShowHatchTooltip(show: false);
		}
		UIPartActionController.Instance.Show(show: false);
		EditorActionGroups.Instance.ActivateInFlightInterface(FlightGlobals.ActiveVessel);
		actionGroupsTransition.Transition("In");
		StartCoroutine(PauseTime());
		InputLockManager.RemoveControlLock("MenuNavigation");
		InputLockManager.SetControlLock(ControlTypes.STAGING | ControlTypes.VESSEL_SWITCHING | ControlTypes.MAP_TOGGLE, "ActionGroupsFlightControllerPanel");
		isOpen = true;
	}

	public void ExitPanelActions()
	{
		for (int i = 0; i < flightUI.Count; i++)
		{
			if (flightUI[i] != null)
			{
				flightUI[i].SetActive(value: true);
			}
		}
		EditorActionGroups.Instance.DectivateInFlightInterface(FlightGlobals.ActiveVessel);
		EditorActionPartSelector.DestroyPartActions(FlightGlobals.ActiveVessel.parts);
		if (actionGroupsTransition != null)
		{
			actionGroupsTransition.Transition("Out");
		}
		InputLockManager.RemoveControlLock("MenuNavigation");
		InputLockManager.RemoveControlLock("ActionGroupsFlightControllerPanel");
		Time.timeScale = previousTimeScale;
		isOpen = false;
		if (actionGroupsPanel != null)
		{
			actionGroupsPanel.UpdateActionGroups();
		}
		UIPartActionController.Instance.Show(show: true);
		if (CrewHatchController.fetch != null)
		{
			CrewHatchController.fetch.ShowHatchTooltip(show: true);
		}
		GameEvents.onGameUnpause.Fire();
		GameEvents.onGUIActionGroupFlightClosed.Fire();
		GameEvents.onGUIActionGroupClosed.Fire();
		GameEvents.onVesselWasModified.Fire(FlightGlobals.ActiveVessel);
	}

	public IEnumerator PauseTime()
	{
		yield return new WaitForSeconds(0.5f);
		previousTimeScale = Time.timeScale;
		Time.timeScale = 0f;
		GameEvents.onGUIActionGroupFlightShown.Fire();
		GameEvents.onGUIActionGroupShown.Fire();
		GameEvents.onGamePause.Fire();
	}

	public void SaveActions()
	{
		ConfigNode configNode = new ConfigNode();
		int count = FlightGlobals.ActiveVessel.parts.Count;
		for (int i = 0; i < count; i++)
		{
			Part part = FlightGlobals.ActiveVessel.parts[i];
			part.onBackup();
			ConfigNode configNode2 = configNode.AddNode("PART");
			part.Actions.OnSave(configNode2.AddNode("ACTIONS"));
		}
		GameEvents.onVesselWasModified.Fire(FlightGlobals.ActiveVessel);
	}

	public void GetActionGroupPanel(ActionGroupsPanel panel)
	{
		actionGroupsPanel = panel;
	}
}

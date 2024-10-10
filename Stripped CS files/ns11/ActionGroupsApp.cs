using ns2;
using TMPro;
using UnityEngine;

namespace ns11;

public class ActionGroupsApp : UIApp
{
	[SerializeField]
	public TMP_Text overrideGroupTextPrefab;

	[SerializeField]
	public GenericAppFrame appFramePrefab;

	public GenericAppFrame appFrame;

	public ActionGroupsPanel agPanel;

	public RectTransform appFrameRectTransform;

	public static ActionGroupsApp Instance { get; set; }

	public TMP_Text overrideGroupText { get; set; }

	public override bool OnAppAboutToStart()
	{
		return true;
	}

	public override ApplicationLauncher.AppScenes GetAppScenes()
	{
		SpaceCenterFacility spaceCenterFacility = SpaceCenterFacility.VehicleAssemblyBuilding;
		if (HighLogic.LoadedScene == GameScenes.EDITOR)
		{
			spaceCenterFacility = EditorDriver.editorFacility.ToFacility();
		}
		else if (HighLogic.LoadedScene == GameScenes.FLIGHT)
		{
			if (ScenarioUpgradeableFacilities.IsLaunchPad(FlightGlobals.ActiveVessel.launchedFrom))
			{
				spaceCenterFacility = SpaceCenterFacility.VehicleAssemblyBuilding;
			}
			else if (ScenarioUpgradeableFacilities.IsRunway(FlightGlobals.ActiveVessel.launchedFrom))
			{
				spaceCenterFacility = SpaceCenterFacility.SpaceplaneHangar;
			}
		}
		if (!GameVariables.Instance.UnlockedActionGroupsStock(ScenarioUpgradeableFacilities.GetFacilityLevel(spaceCenterFacility), spaceCenterFacility == SpaceCenterFacility.VehicleAssemblyBuilding))
		{
			return ApplicationLauncher.AppScenes.NEVER;
		}
		return ApplicationLauncher.AppScenes.FLIGHT | ApplicationLauncher.AppScenes.MAPVIEW;
	}

	public override Vector3 GetAppScreenPos(Vector3 defaultAnchorPos)
	{
		return defaultAnchorPos;
	}

	public override void OnAppInitialized()
	{
		Debug.Log("[ActionGroupsApp] OnAppStarted(): id: " + GetInstanceID());
		if (Instance != null)
		{
			Debug.Log("ActionGroupsApp already exist, destroying this instance");
			Object.DestroyImmediate(base.gameObject);
			return;
		}
		Instance = this;
		GameEvents.onVesselWasModified.Add(OnVesselWasModified);
		GameEvents.onVesselChange.Add(OnVesselChanged);
		GameEvents.onVesselDestroy.Add(OnVesselDestroy);
		GameEvents.onVesselGoOffRails.Add(OnVesselGoOffRails);
		GameEvents.OnVesselOverrideGroupChanged.Add(OnVesselOverrideGroupChanged);
		appFrame = Object.Instantiate(appFramePrefab);
		appFrame.transform.SetParent(base.transform, worldPositionStays: false);
		appFrame.transform.localPosition = Vector3.zero;
		appFrame.anchorToAppButton = true;
		appFrameRectTransform = appFrame.transform as RectTransform;
		UpdateOverrideGroupText();
		appFrame.Setup(base.appLauncherButton, base.name, base.name, 266, 48, scaleHeightToContainList: false);
		appFrame.AddGlobalInputDelegate(base.MouseInput_PointerEnter, base.MouseInput_PointerExit);
		ApplicationLauncher.Instance.AddOnRepositionCallback(Reposition);
		ReadjustPosition();
		agPanel = appFrame.GetComponent<ActionGroupsPanel>();
		HideApp();
	}

	public override void OnAppDestroy()
	{
		if (appFrame != null)
		{
			if ((bool)ApplicationLauncher.Instance)
			{
				ApplicationLauncher.Instance.RemoveOnRepositionCallback(Reposition);
			}
			appFrame.gameObject.DestroyGameObject();
		}
		GameEvents.onVesselWasModified.Remove(OnVesselWasModified);
		GameEvents.onVesselChange.Remove(OnVesselChanged);
		GameEvents.onVesselDestroy.Remove(OnVesselDestroy);
		GameEvents.onVesselGoOffRails.Remove(OnVesselGoOffRails);
		GameEvents.OnVesselOverrideGroupChanged.Remove(OnVesselOverrideGroupChanged);
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public override void Reposition()
	{
		appFrame.Reposition();
		ReadjustPosition();
	}

	public override void DisplayApp()
	{
		if (appFrame != null)
		{
			appFrame.gameObject.SetActive(value: true);
			agPanel.UpdateButtons();
		}
	}

	public override void HideApp()
	{
		if (appFrame != null)
		{
			appFrame.gameObject.SetActive(value: false);
		}
	}

	public void HideAppFromInput()
	{
		base.appLauncherButton.SetFalse();
	}

	public void OnVesselWasModified(Vessel v)
	{
		base.appLauncherButton.VisibleInScenes = GetAppScenes();
		ApplicationLauncher.Instance.DetermineVisibility(base.appLauncherButton);
	}

	public void OnVesselChanged(Vessel v)
	{
		base.appLauncherButton.VisibleInScenes = GetAppScenes();
		ApplicationLauncher.Instance.DetermineVisibility(base.appLauncherButton);
	}

	public void OnVesselDestroy(Vessel v)
	{
	}

	public void OnVesselGoOffRails(Vessel v)
	{
	}

	public void OnVesselOverrideGroupChanged(Vessel v)
	{
		if (v == FlightGlobals.ActiveVessel)
		{
			UpdateOverrideGroupText();
			agPanel.UpdateButtons();
		}
	}

	public void UpdateOverrideGroupText()
	{
		Vessel activeVessel = FlightGlobals.ActiveVessel;
		if (!(activeVessel == null) && !(base.appLauncherButton == null))
		{
			if (overrideGroupText == null)
			{
				overrideGroupText = Object.Instantiate(overrideGroupTextPrefab);
				overrideGroupText.transform.SetParent(base.appLauncherButton.gameObject.GetChild("Image").transform, worldPositionStays: false);
			}
			int groupOverride = activeVessel.GroupOverride;
			if (groupOverride == 0)
			{
				overrideGroupText.text = "";
			}
			else
			{
				overrideGroupText.text = groupOverride.ToString();
			}
		}
	}

	public void SelectNext()
	{
		Vessel activeVessel = FlightGlobals.ActiveVessel;
		if (!(activeVessel == null))
		{
			int num = activeVessel.GroupOverride + 1;
			if (num > Vessel.NumOverrideGroups)
			{
				num = 0;
			}
			activeVessel.SetGroupOverride(num);
		}
	}

	public void SelectPrev()
	{
		Vessel activeVessel = FlightGlobals.ActiveVessel;
		if (!(activeVessel == null))
		{
			int num = activeVessel.GroupOverride - 1;
			if (num < 0)
			{
				num = Vessel.NumOverrideGroups;
			}
			activeVessel.SetGroupOverride(num);
		}
	}

	public void HideUnpinApp()
	{
		if (!base.pinned)
		{
			HideApp();
			MouseInput_PointerExit(null);
		}
	}

	public void ReadjustPosition()
	{
		appFrameRectTransform.anchoredPosition = new Vector2(appFrameRectTransform.anchoredPosition.x - 3f, appFrameRectTransform.anchoredPosition.y);
	}
}

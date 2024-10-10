using System.Collections;
using System.Collections.Generic;
using System.IO;
using Expansions;
using Expansions.Missions;
using Expansions.Missions.Runtime;
using ns2;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Upgradeables;

public class EditorDriver : MonoBehaviour
{
	public enum StartupBehaviours
	{
		START_CLEAN,
		LOAD_FROM_CACHE,
		LOAD_FROM_FILE
	}

	public bool canRun = true;

	public bool restartingEditor = true;

	public static EditorDriver fetch;

	public static StartupBehaviours StartupBehaviour = StartupBehaviours.LOAD_FROM_CACHE;

	public static string filePathToLoad = "";

	public static EditorFacility editorFacility = EditorFacility.None;

	public VABCamera vabCamera;

	public SPHCamera sphCamera;

	public string editorSceneryRootName;

	public Transform editorSceneryRoot;

	public UpgradeableInteriorScene interiorScene;

	public UpgradeableInterior interior;

	[SerializeField]
	public static List<string> validLaunchSites = null;

	public static string selectedlaunchSiteName;

	public static bool CanRun => fetch.canRun;

	public static string SelectedLaunchSiteName => selectedlaunchSiteName;

	public static List<string> ValidLaunchSites
	{
		get
		{
			if (validLaunchSites == null)
			{
				validLaunchSites = new List<string>();
			}
			return validLaunchSites;
		}
	}

	public static string DefaultCraftSavePath { get; set; }

	public void Awake()
	{
		fetch = this;
		validLaunchSites = new List<string>();
		if (editorFacility == EditorFacility.None)
		{
			editorFacility = EditorFacility.const_1;
		}
		if (PartLoader.LoadedPartsList == null)
		{
			Debug.LogError("[Editor Driver]: Game does not seem to be loaded.");
			canRun = false;
		}
		RenderSettings.defaultReflectionMode = DefaultReflectionMode.Custom;
		EditorCamera editorCamera = Object.FindObjectOfType<EditorCamera>();
		if (editorCamera != null)
		{
			vabCamera = editorCamera.GetComponent<VABCamera>();
			sphCamera = editorCamera.GetComponent<SPHCamera>();
		}
		GameEvents.onEditorStarted.Add(onEditorStarted);
		switch (editorFacility)
		{
		case EditorFacility.const_1:
			SceneManager.LoadScene("VABscenery", LoadSceneMode.Additive);
			vabCamera.enabled = true;
			sphCamera.enabled = false;
			break;
		case EditorFacility.const_2:
			SceneManager.LoadScene("SPHscenery", LoadSceneMode.Additive);
			vabCamera.enabled = false;
			sphCamera.enabled = true;
			break;
		case EditorFacility.None:
			break;
		}
	}

	public IEnumerator Start()
	{
		if (!canRun)
		{
			yield break;
		}
		Debug.Log("------------------- initializing editor mode... ------------------");
		Debug.Log("editor started");
		GamePersistence.LoadGame("persistent", HighLogic.SaveFolder, nullIfIncompatible: true, suppressIncompatibleMessage: false).Load();
		SetInputLockFromGameParameters();
		string CrewObjectName = "";
		GameObject CrewObject4 = null;
		setupValidLaunchSites();
		switch (editorFacility)
		{
		case EditorFacility.const_1:
			EditorLogic.fetch.symmetryMethod = SymmetryMethod.Radial;
			EditorLogic.fetch.vesselRotation = Quaternion.identity;
			setLaunchSite(HighLogic.CurrentGame.defaultVABLaunchSite);
			if (string.IsNullOrEmpty(selectedlaunchSiteName))
			{
				selectedlaunchSiteName = "LaunchPad";
			}
			EditorLogic.fetch.launchSiteName = selectedlaunchSiteName;
			editorSceneryRootName = "VABscenery";
			CrewObjectName = "VABCrew";
			RenderSettings.customReflection = vabCamera.VABReflection;
			break;
		case EditorFacility.const_2:
			EditorLogic.fetch.symmetryMethod = SymmetryMethod.Mirror;
			EditorLogic.fetch.vesselRotation = Quaternion.Euler(90f, 0f, 0f);
			setLaunchSite(HighLogic.CurrentGame.defaultSPHLaunchSite);
			if (string.IsNullOrEmpty(selectedlaunchSiteName))
			{
				selectedlaunchSiteName = "Runway";
			}
			EditorLogic.fetch.launchSiteName = selectedlaunchSiteName;
			editorSceneryRootName = "SPHscenery";
			CrewObjectName = "SPHCrew";
			RenderSettings.customReflection = sphCamera.SPHReflection;
			break;
		}
		while (CrewObject4 == null)
		{
			CrewObject4 = GameObject.Find(CrewObjectName);
			if (CrewObject4 == null)
			{
				yield return null;
			}
		}
		CrewObject4.SetActive(GameSettings.SHOW_SPACE_CENTER_CREW);
		while (editorSceneryRoot == null)
		{
			editorSceneryRoot = GameObject.Find(editorSceneryRootName).transform;
			if (editorSceneryRoot == null)
			{
				yield return null;
			}
		}
		interiorScene = editorSceneryRoot.GetComponentInChildren<UpgradeableInteriorScene>(includeInactive: true);
		if (editorFacility == EditorFacility.const_1 && interiorScene.FacilityLevel == 2)
		{
			CrewObject4 = null;
			while (CrewObject4 == null)
			{
				yield return null;
				CrewObject4 = GameObject.Find("model_props");
			}
			CrewObject4.SetActive(GameSettings.SHOW_SPACE_CENTER_CREW);
			if (editorFacility == EditorFacility.const_1)
			{
				CrewObject4 = GameObject.Find("model_vab_prop_truck_01");
				if (CrewObject4 != null)
				{
					CrewObject4.SetActive(GameSettings.SHOW_SPACE_CENTER_CREW);
				}
				CrewObject4 = GameObject.Find("model_vab_elevators");
				if (CrewObject4 != null)
				{
					CrewObject4.SetActive(GameSettings.SHOW_SPACE_CENTER_CREW);
				}
			}
		}
		EditorBounds.CenterSceneryOrigin(editorSceneryRoot);
		Canvas.ForceUpdateCanvases();
	}

	public void SetInputLockFromGameParameters()
	{
		if (HighLogic.CurrentGame == null)
		{
			return;
		}
		ControlTypes controlTypes = ControlTypes.None;
		if (HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER)
		{
			controlTypes |= ControlTypes.EDITOR_LAUNCH;
			controlTypes |= ControlTypes.EDITOR_CREW;
			InputLockManager.SetControlLock(controlTypes, "EditorDriver_GameParameters");
			return;
		}
		if (HighLogic.CurrentGame.Mode == Game.Modes.MISSION)
		{
			InputLockManager.SetControlLock(controlTypes, "EditorDriver_GameParameters");
			return;
		}
		if (!HighLogic.CurrentGame.Parameters.Editor.CanLaunch)
		{
			controlTypes |= ControlTypes.EDITOR_LAUNCH;
		}
		if (!HighLogic.CurrentGame.Parameters.Editor.CanLoad)
		{
			controlTypes |= ControlTypes.EDITOR_LOAD;
		}
		if (!HighLogic.CurrentGame.Parameters.Editor.CanStartNew)
		{
			controlTypes |= ControlTypes.EDITOR_NEW;
		}
		if (!HighLogic.CurrentGame.Parameters.Editor.CanSave)
		{
			controlTypes |= ControlTypes.EDITOR_SAVE;
		}
		if (!HighLogic.CurrentGame.Parameters.Editor.CanLeaveToMainMenu && !HighLogic.CurrentGame.Parameters.Editor.CanLeaveToSpaceCenter)
		{
			controlTypes |= ControlTypes.EDITOR_EXIT;
		}
		InputLockManager.SetControlLock(controlTypes, "EditorDriver_GameParameters");
	}

	public void OnApplicationFocus(bool focus)
	{
		if (focus)
		{
			InputLockManager.RemoveControlLock("EditorDriver_AppFocus");
		}
		else
		{
			InputLockManager.SetControlLock("EditorDriver_AppFocus");
		}
	}

	public void OnDestroy()
	{
		InputLockManager.RemoveControlLock("EditorDriver_AppFocus");
		InputLockManager.RemoveControlLock("EditorDriver_GameParameters");
		if (fetch != null && fetch == this)
		{
			fetch = null;
		}
		GameEvents.onEditorStarted.Remove(onEditorStarted);
	}

	public void onEditorStarted()
	{
		restartingEditor = false;
	}

	public static void StartAndLoadVessel(string fullFilePath, EditorFacility facility)
	{
		StartupBehaviour = StartupBehaviours.LOAD_FROM_FILE;
		filePathToLoad = fullFilePath;
		editorFacility = facility;
		HighLogic.LoadScene(GameScenes.EDITOR);
	}

	public static void StartEditor(EditorFacility facility)
	{
		editorFacility = facility;
		HighLogic.LoadScene(GameScenes.EDITOR);
	}

	public static void SwitchEditor(EditorFacility facility)
	{
		if (!(fetch == null) && facility != editorFacility)
		{
			fetch.restartingEditor = true;
			fetch.StartCoroutine(initEditor(facility));
		}
	}

	public static IEnumerator initEditor(EditorFacility facility)
	{
		Image mask = new GameObject("Mask", typeof(Image)).GetComponent<Image>();
		mask.color = Color.black;
		mask.canvasRenderer.SetAlpha(0f);
		mask.CrossFadeAlpha(1f, 0.3f, ignoreTimeScale: true);
		RectTransform rectTransform = mask.rectTransform;
		rectTransform.SetParent(UIMasterController.Instance.mainCanvas.transform, worldPositionStays: false);
		rectTransform.anchorMin = Vector2.zero;
		rectTransform.anchorMax = Vector2.one;
		rectTransform.offsetMin = Vector2.zero;
		rectTransform.offsetMax = Vector2.zero;
		rectTransform.anchoredPosition = Vector3.zero;
		rectTransform.SetAsFirstSibling();
		yield return new WaitForSeconds(0.3f);
		GameObject CrewObject4 = null;
		foreach (KeyValuePair<string, ScenarioUpgradeableFacilities.ProtoUpgradeable> protoUpgradeable in ScenarioUpgradeableFacilities.protoUpgradeables)
		{
			for (int i = 0; i < protoUpgradeable.Value.facilityRefs.Count; i++)
			{
				protoUpgradeable.Value.facilityRefs[i].UnregisterInstance();
			}
		}
		switch (editorFacility)
		{
		case EditorFacility.const_1:
			if ((bool)GameObject.Find("VABscenery"))
			{
				if (fetch.interiorScene != null)
				{
					string sceneName2 = fetch.interiorScene.Scenes[fetch.interiorScene.FacilityLevel];
					SceneManager.UnloadSceneAsync(sceneName2);
					Object.Destroy(GameObject.Find(sceneName2));
				}
				SceneManager.UnloadSceneAsync("VABscenery");
				Object.Destroy(GameObject.Find("VABscenery"));
			}
			break;
		case EditorFacility.const_2:
			if ((bool)GameObject.Find("SPHscenery"))
			{
				if (fetch.interiorScene != null)
				{
					string sceneName = fetch.interiorScene.Scenes[fetch.interiorScene.FacilityLevel];
					SceneManager.UnloadSceneAsync(sceneName);
					Object.Destroy(GameObject.Find(sceneName));
				}
				SceneManager.UnloadSceneAsync("SPHscenery");
				Object.Destroy(GameObject.Find("SPHscenery"));
			}
			break;
		}
		yield return null;
		string CrewObjectName;
		if ((uint)facility > 1u && facility == EditorFacility.const_2)
		{
			yield return SceneManager.LoadSceneAsync("SPHscenery", LoadSceneMode.Additive);
			yield return null;
			while (EditorBounds.Instance == null)
			{
				yield return null;
			}
			fetch.vabCamera.enabled = false;
			fetch.sphCamera.enabled = true;
			RenderSettings.customReflection = fetch.sphCamera.SPHReflection;
			EditorLogic.fetch.symmetryMethod = SymmetryMethod.Mirror;
			EditorLogic.fetch.vesselRotation = Quaternion.Euler(90f, 0f, 0f);
			if (string.IsNullOrEmpty(selectedlaunchSiteName))
			{
				selectedlaunchSiteName = "Runway";
			}
			EditorLogic.fetch.launchSiteName = selectedlaunchSiteName;
			fetch.editorSceneryRootName = "SPHscenery";
			CrewObjectName = "SPHCrew";
		}
		else
		{
			yield return SceneManager.LoadSceneAsync("VABscenery", LoadSceneMode.Additive);
			yield return null;
			while (EditorBounds.Instance == null)
			{
				yield return null;
			}
			fetch.vabCamera.enabled = true;
			fetch.sphCamera.enabled = false;
			RenderSettings.customReflection = fetch.vabCamera.VABReflection;
			EditorLogic.fetch.symmetryMethod = SymmetryMethod.Radial;
			EditorLogic.fetch.vesselRotation = Quaternion.identity;
			if (string.IsNullOrEmpty(selectedlaunchSiteName))
			{
				selectedlaunchSiteName = "LaunchPad";
			}
			EditorLogic.fetch.launchSiteName = selectedlaunchSiteName;
			fetch.editorSceneryRootName = "VABscenery";
			CrewObjectName = "VABCrew";
		}
		while (CrewObject4 == null)
		{
			CrewObject4 = GameObject.Find(CrewObjectName);
			if (CrewObject4 == null)
			{
				yield return null;
			}
		}
		CrewObject4.SetActive(GameSettings.SHOW_SPACE_CENTER_CREW);
		while (fetch.editorSceneryRoot == null)
		{
			fetch.editorSceneryRoot = GameObject.Find(fetch.editorSceneryRootName).transform;
			if (fetch.editorSceneryRoot == null)
			{
				yield return null;
			}
		}
		fetch.interiorScene = fetch.editorSceneryRoot.GetComponentInChildren<UpgradeableInteriorScene>(includeInactive: true);
		if (editorFacility == EditorFacility.const_1 && fetch.interiorScene.FacilityLevel == 2)
		{
			CrewObject4 = null;
			while (CrewObject4 == null)
			{
				yield return null;
				CrewObject4 = GameObject.Find("model_props");
			}
			CrewObject4.SetActive(GameSettings.SHOW_SPACE_CENTER_CREW);
			if (editorFacility == EditorFacility.const_1)
			{
				CrewObject4 = GameObject.Find("model_vab_prop_truck_01");
				if (CrewObject4 != null)
				{
					CrewObject4.SetActive(GameSettings.SHOW_SPACE_CENTER_CREW);
				}
				CrewObject4 = GameObject.Find("model_vab_elevators");
				if (CrewObject4 != null)
				{
					CrewObject4.SetActive(GameSettings.SHOW_SPACE_CENTER_CREW);
				}
			}
		}
		UpgradeableFacility[] componentsInChildren = fetch.editorSceneryRoot.GetComponentsInChildren<UpgradeableFacility>();
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			componentsInChildren[j].RegisterInstance();
		}
		EditorBounds.CenterSceneryOrigin(fetch.editorSceneryRoot);
		editorFacility = facility;
		mask.CrossFadeAlpha(0f, 0.3f, ignoreTimeScale: true);
		Object.Destroy(mask.gameObject, 1f);
		setupValidLaunchSites();
		EditorLogic.fetch.StartEditor(isRestart: true);
		if (MusicLogic.fetch != null)
		{
			MusicLogic.fetch.HandleEditorSwitch();
		}
	}

	public static bool setLaunchSite(string siteName)
	{
		if (ValidLaunchSite(siteName))
		{
			if (EditorLogic.fetch != null)
			{
				EditorLogic.fetch.launchSiteName = siteName;
			}
			selectedlaunchSiteName = siteName;
			return true;
		}
		setDefaultLaunchSite();
		return false;
	}

	public static void saveselectedLaunchSite()
	{
		if (HighLogic.CurrentGame != null)
		{
			if (editorFacility == EditorFacility.const_2)
			{
				HighLogic.CurrentGame.defaultSPHLaunchSite = selectedlaunchSiteName;
			}
			else if (editorFacility == EditorFacility.const_1)
			{
				HighLogic.CurrentGame.defaultVABLaunchSite = selectedlaunchSiteName;
			}
		}
		else if (HighLogic.CurrentGame != null)
		{
			HighLogic.CurrentGame.defaultVABLaunchSite = "LaunchPad";
			HighLogic.CurrentGame.defaultSPHLaunchSite = "Runway";
		}
	}

	public static void setDefaultLaunchSite()
	{
		if (editorFacility == EditorFacility.const_2)
		{
			if (EditorLogic.fetch != null)
			{
				EditorLogic.fetch.launchSiteName = "Runway";
			}
			selectedlaunchSiteName = "Runway";
		}
		else
		{
			if (EditorLogic.fetch != null)
			{
				EditorLogic.fetch.launchSiteName = "LaunchPad";
			}
			selectedlaunchSiteName = "LaunchPad";
		}
	}

	public static bool ValidLaunchSite(string siteName)
	{
		int num = 0;
		while (true)
		{
			if (num < validLaunchSites.Count)
			{
				if (validLaunchSites[num] == siteName)
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

	public static void setupValidLaunchSites()
	{
		if (validLaunchSites == null)
		{
			validLaunchSites = new List<string>();
		}
		else
		{
			validLaunchSites.Clear();
		}
		int num = PSystemSetup.Instance.SpaceCenterFacilities.Length;
		for (int i = 0; i < num; i++)
		{
			PSystemSetup.SpaceCenterFacility spaceCenterFacility = PSystemSetup.Instance.SpaceCenterFacilities[i];
			if (spaceCenterFacility.editorFacility == editorFacility)
			{
				validLaunchSites.Add(spaceCenterFacility.name);
			}
		}
		bool flag = OtherLaunchSitesValid();
		bool flag2 = HighLogic.CurrentGame.Mode == Game.Modes.SANDBOX || HighLogic.CurrentGame.Mode == Game.Modes.SCIENCE_SANDBOX || HighLogic.CurrentGame.Mode == Game.Modes.CAREER;
		for (int j = 0; j < PSystemSetup.Instance.LaunchSites.Count; j++)
		{
			if (PSystemSetup.Instance.LaunchSites[j].editorFacility == editorFacility)
			{
				bool flag3 = false;
				if (flag && string.Equals(PSystemSetup.Instance.LaunchSites[j].BundleName, "makinghistory_assets"))
				{
					flag3 = true;
				}
				else if ((flag2 && string.Equals(PSystemSetup.Instance.LaunchSites[j].BundleName, string.Empty)) || string.Equals(PSystemSetup.Instance.LaunchSites[j].BundleName, "stock"))
				{
					flag3 = !PSystemSetup.Instance.LaunchSites[j].requiresPOIVisit || !(ProgressTracking.Instance != null) || ProgressTracking.Instance.NodeComplete(PSystemSetup.Instance.LaunchSites[j].poiName);
				}
				if (flag3)
				{
					validLaunchSites.Add(PSystemSetup.Instance.LaunchSites[j].name);
				}
			}
		}
	}

	public static bool OtherLaunchSitesValid()
	{
		if (ExpansionsLoader.IsExpansionInstalled("MakingHistory") && HighLogic.CurrentGame != null)
		{
			if (HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER)
			{
				return false;
			}
			if (HighLogic.CurrentGame.Mode == Game.Modes.MISSION && MissionSystem.Instance != null && MissionSystem.missions.Count > 0 && MissionSystem.missions[0].situation.VesselsArePending)
			{
				return false;
			}
			if (((HighLogic.CurrentGame.Mode == Game.Modes.SANDBOX || HighLogic.CurrentGame.Mode == Game.Modes.SCIENCE_SANDBOX || HighLogic.CurrentGame.Mode == Game.Modes.CAREER) && HighLogic.CurrentGame.Parameters.Difficulty.AllowOtherLaunchSites) || (HighLogic.CurrentGame.Mode == Game.Modes.MISSION && HighLogic.CurrentGame.Parameters.CustomParams<MissionParamsExtras>().facilityOpenEditor && HighLogic.CurrentGame.Parameters.CustomParams<MissionParamsExtras>().launchSitesOpen))
			{
				return true;
			}
		}
		return false;
	}

	public static bool SetDefaultSaveFolder(string path)
	{
		if (!IsValidSaveFolder(path))
		{
			return false;
		}
		DefaultCraftSavePath = path;
		return true;
	}

	public static bool IsValidSaveFolder(string path)
	{
		return new DirectoryInfo(path).Exists;
	}
}

using System.Collections.Generic;
using System.IO;
using Expansions.Missions.Editor;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Runtime;

public class MissionEndDialog : MonoBehaviour
{
	public static MissionEndDialog Instance;

	public bool display;

	public static bool showExitControls;

	public static bool allowClosingDialog;

	[SerializeField]
	public AwardWidget awardWidgetPrefab;

	[SerializeField]
	public Button Btn_SaveAndQuit;

	[SerializeField]
	public Button Btn_Restart;

	[SerializeField]
	public Button Btn_Close;

	[SerializeField]
	public Button Btn_Revert;

	[SerializeField]
	public Button Tab_Score;

	[SerializeField]
	public Button Tab_Details;

	[SerializeField]
	public RawImage image_Result;

	[SerializeField]
	public TextMeshProUGUI txt_EndMessage;

	[SerializeField]
	public TextMeshProUGUI scoreDetailsText;

	[SerializeField]
	public TextMeshProUGUI totalScoreText;

	[SerializeField]
	public TextMeshProUGUI statusMessageText;

	[SerializeField]
	public GameObject content_Score;

	[SerializeField]
	public GameObject content_Details;

	[SerializeField]
	public Transform awardsContent;

	public Mission finishedMission;

	public static bool isDisplaying
	{
		get
		{
			if (!(Instance == null))
			{
				return Instance.display;
			}
			return false;
		}
	}

	public string lastLogEntries
	{
		get
		{
			string text = string.Empty;
			for (int i = 0; i < FlightLogger.eventLog.Count; i++)
			{
				text = text + FlightLogger.eventLog[i] + "\n";
			}
			return text;
		}
	}

	public void Awake()
	{
		if (Instance != null)
		{
			Object.Destroy(this);
		}
		else
		{
			Instance = this;
		}
	}

	public void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			Close();
		}
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public static MissionEndDialog Display(Mission finishedMission)
	{
		if (PauseMenu.exists && PauseMenu.isOpen)
		{
			PauseMenu.Close();
		}
		FlightDriver.SetPause(pauseState: true);
		bool flag = !ExpansionsLoader.IsExpansionInstalled("MakingHistory");
		if (Instance == null)
		{
			Object @object = null;
			@object = ((!flag) ? MissionsUtils.MEPrefab("_UI5/Dialogs/MissionEndDialog/prefabs/MissionEndDialog.prefab") : AssetBase.GetPrefab("BaseMissionEndDialog"));
			if (@object == null)
			{
				Debug.LogError("[MissionEndDialog]: Unable to load the Asset");
				return null;
			}
			Instance = ((GameObject)Object.Instantiate(@object)).GetComponent<MissionEndDialog>();
			Instance.name = "Mission End Dialog Handler";
			(Instance.transform as RectTransform).SetParent(DialogCanvasUtil.DialogCanvasRect, worldPositionStays: false);
		}
		Instance.gameObject.SetActive(value: true);
		Instance.finishedMission = finishedMission;
		Instance.display = true;
		Instance.SetupGUI();
		MEBannerType bannerType = (finishedMission.isSuccesful ? MEBannerType.Success : MEBannerType.Fail);
		Instance.image_Result.texture = finishedMission.GetBanner(bannerType).texture;
		Instance.txt_EndMessage.text = finishedMission.activeNode.message;
		Instance.scoreDetailsText.text = finishedMission.PrintScoreObjectives(onlyPrintActivatedNodes: true, startWithActiveNode: false, onlyAwardedScores: true);
		Instance.totalScoreText.text = KSPUtil.LocalizeNumber(finishedMission.currentScore, "F0");
		Instance.statusMessageText.text = (finishedMission.isSuccesful ? Localizer.Format("#autoLOC_8003100") : Localizer.Format("#autoLOC_8003101"));
		if (!flag)
		{
			int i = 0;
			for (int count = finishedMission.awards.awardedAwards.Count; i < count; i++)
			{
				Instance.awardWidgetPrefab.Create(finishedMission.awards.awardedAwards[i], Instance.awardsContent);
			}
		}
		if ((bool)LoadingBufferMask.Instance)
		{
			LoadingBufferMask.Instance.Hide();
		}
		return Instance;
	}

	public static void Close()
	{
		if (!(Instance == null))
		{
			Instance.display = false;
			FlightDriver.SetPause(pauseState: false);
			Object.Destroy(Instance.gameObject);
		}
	}

	public static void Restart()
	{
		ScreenMessages.PostScreenMessage("#autoLOC_8006013", 4f, ScreenMessageStyle.UPPER_CENTER);
		MissionSystem.RemoveMissionObjects(removeAll: true);
		InputLockManager.ClearControlLocks();
		string saveName = "persistent";
		string saveFolder = HighLogic.SaveFolder;
		if (Directory.GetFiles(KSPUtil.ApplicationRootPath + "saves/" + saveFolder + "/", "lastcreatevesseleditor.missionsfs").Length == 1)
		{
			File.Delete(KSPUtil.ApplicationRootPath + "saves/" + saveFolder + "/lastcreatevesseleditor.missionsfs");
		}
		if (Directory.GetFiles(KSPUtil.ApplicationRootPath + "saves/" + saveFolder + "/", "lastcreatevesselspawn.missionsfs").Length == 1)
		{
			File.Delete(KSPUtil.ApplicationRootPath + "saves/" + saveFolder + "/lastcreatevesselspawn.missionsfs");
		}
		if (Directory.GetFiles(KSPUtil.ApplicationRootPath + "saves/" + saveFolder + "/", "startcreatevesselspawn.missionsfs").Length == 1)
		{
			File.Copy(KSPUtil.ApplicationRootPath + "saves/" + saveFolder + "/startcreatevesselspawn.missionsfs", KSPUtil.ApplicationRootPath + "saves/" + saveFolder + "/lastcreatevesselspawn.missionsfs", overwrite: true);
		}
		string text = KSPUtil.ApplicationRootPath + "saves/" + saveFolder + "/reset.missionsfs";
		if (File.Exists(text))
		{
			ConfigNode configNode = ConfigNode.Load(text);
			HighLogic.CurrentGame = GamePersistence.LoadGameCfg(configNode, saveName, nullIfIncompatible: true, suppressIncompatibleMessage: false);
			HighLogic.SaveFolder = saveFolder;
			GamePersistence.UpdateScenarioModules(HighLogic.CurrentGame);
			if (configNode != null)
			{
				GameEvents.onGameStatePostLoad.Fire(configNode);
			}
			HighLogic.CurrentGame.missionToStart.InitMission();
			GamePersistence.SaveGame(HighLogic.CurrentGame, "persistent", saveFolder, SaveMode.OVERWRITE);
			HighLogic.CurrentGame.Start();
			Close();
		}
		else
		{
			Debug.Log("[Mission] Couldn't restart mission " + text + " reset file not found.");
		}
	}

	public static void Revert()
	{
		Instance.gameObject.SetActive(value: false);
		PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("RevertingFlight", null, Localizer.Format("#autoLOC_360548"), HighLogic.UISkin, 350f, drawRevertOptions()), persistAcrossScenes: false, HighLogic.UISkin).OnDismiss = Close;
	}

	public static DialogGUIBase[] drawRevertOptions()
	{
		DialogGUIButton dialogGUIButton = null;
		List<DialogGUIBase> list = new List<DialogGUIBase>();
		DialogGUILabel item = new DialogGUILabel(Localizer.Format("#autoLOC_360683"));
		list.Add(item);
		PauseMenu.drawMissionsRevertOptions(null, list);
		dialogGUIButton = new DialogGUIButton(Localizer.Format("#autoLOC_360725"), delegate
		{
			Instance.gameObject.SetActive(value: true);
		}, dismissOnSelect: true);
		list.Add(dialogGUIButton);
		return list.ToArray();
	}

	public static void DisplayScore()
	{
		if (Instance != null)
		{
			Instance.content_Score.SetActive(value: true);
			Instance.content_Details.SetActive(value: false);
		}
	}

	public static void DisplayDetails()
	{
		if (Instance != null)
		{
			Instance.content_Details.SetActive(value: true);
			Instance.content_Score.SetActive(value: false);
		}
	}

	public void SetupGUI()
	{
		Btn_SaveAndQuit.gameObject.SetActive(value: false);
		Btn_SaveAndQuit.onClick.RemoveAllListeners();
		if (HighLogic.CurrentGame.Parameters.Flight.CanLeaveToMainMenu)
		{
			Btn_SaveAndQuit.gameObject.SetActive(value: true);
			Btn_SaveAndQuit.GetComponentInChildren<TextMeshProUGUI>().text = Localizer.Format("#autoLOC_8001022");
			Btn_SaveAndQuit.onClick.AddListener(delegate
			{
				SaveAndQuit();
			});
		}
		Btn_Restart.gameObject.SetActive(value: true);
		Btn_Restart.GetComponentInChildren<TextMeshProUGUI>().text = "#autoLOC_8006013";
		Btn_Restart.onClick.AddListener(delegate
		{
			Restart();
		});
		Btn_Revert.gameObject.SetActive(value: true);
		Btn_Revert.GetComponentInChildren<TextMeshProUGUI>().text = "#autoLOC_360545";
		Btn_Revert.onClick.AddListener(delegate
		{
			Revert();
		});
		Btn_Close.gameObject.SetActive(value: true);
		Btn_Close.GetComponentInChildren<TextMeshProUGUI>().text = "#autoLOC_8001021";
		Btn_Close.onClick.AddListener(delegate
		{
			Close();
		});
		Tab_Score.gameObject.SetActive(value: true);
		Tab_Score.GetComponentInChildren<TextMeshProUGUI>().text = "#autoLOC_8200056";
		Tab_Score.onClick.AddListener(delegate
		{
			DisplayScore();
		});
		Tab_Details.gameObject.SetActive(value: true);
		Tab_Details.GetComponentInChildren<TextMeshProUGUI>().text = "#autoLOC_8005056";
		Tab_Details.onClick.AddListener(delegate
		{
			DisplayDetails();
		});
	}

	public void SaveAndQuit()
	{
		InputLockManager.ClearControlLocks();
		GamePersistence.SaveGame(HighLogic.CurrentGame.Updated(), "persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
		if (MissionSystem.IsTestMode)
		{
			FlightDriver.SetPause(pauseState: true);
			FlightGlobals.ClearpersistentIdDictionaries();
			if (FlightGlobals.fetch != null && FlightGlobals.ActiveVessel != null)
			{
				FlightGlobals.fetch.activeVessel = null;
			}
			MissionEditorLogic.StartUpMissionEditor(finishedMission.MissionInfo.FilePath);
		}
		else
		{
			HighLogic.LoadScene(GameScenes.MAINMENU);
		}
		Close();
	}
}

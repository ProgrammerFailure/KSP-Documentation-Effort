using ns9;
using PreFlightTests;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns17;

public class FlightResultsDialog : MonoBehaviour
{
	public static FlightResultsDialog Instance;

	public bool display;

	public static bool showExitControls;

	public static bool allowClosingDialog;

	[SerializeField]
	public Button Btn_revLaunch;

	[SerializeField]
	public Button Btn_revEditor;

	[SerializeField]
	public Button Btn_returnEditor;

	[SerializeField]
	public Button Btn_TS;

	[SerializeField]
	public Button Btn_KSC;

	[SerializeField]
	public Button Btn_Menu;

	[SerializeField]
	public Button Btn_Close;

	[SerializeField]
	public TextMeshProUGUI txt_Outcome;

	[SerializeField]
	public TextMeshProUGUI txt_Results;

	[SerializeField]
	public TextMeshProUGUI txt_Achievements;

	public string titleMsg = string.Empty;

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
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public static void SetMissionOutcome(string msg)
	{
		Instance.titleMsg = msg;
	}

	public static FlightResultsDialog Display(string outcomeMsg)
	{
		if (PauseMenu.isOpen)
		{
			PauseMenu.Close();
		}
		FlightDriver.SetPause(pauseState: true);
		if (Instance == null)
		{
			Instance = Object.Instantiate(AssetBase.GetPrefab("FlightResultsDialog")).GetComponent<FlightResultsDialog>();
			Instance.name = "Flight Result Dialog Handler";
			(Instance.transform as RectTransform).SetParent(DialogCanvasUtil.DialogCanvasRect, worldPositionStays: false);
		}
		Instance.display = true;
		if (outcomeMsg != string.Empty)
		{
			Instance.titleMsg = outcomeMsg;
		}
		Instance.SetupGUI();
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

	public void Update()
	{
		if (GameSettings.TOGGLE_STATUS_SCREEN.GetKeyDown())
		{
			if (!display)
			{
				Display("Flight Status: " + Vessel.GetSituationString(FlightGlobals.ActiveVessel));
			}
			else if (!showExitControls)
			{
				Close();
			}
		}
	}

	public void SetupGUI()
	{
		txt_Outcome.text = titleMsg;
		txt_Results.text = Localizer.Format("#autoLOC_418635", lastLogEntries);
		txt_Achievements.text = Localizer.Format("#autoLOC_418636", FlightLogger.getMissionStats());
		Btn_revLaunch.gameObject.SetActive(value: false);
		Btn_revLaunch.onClick.RemoveAllListeners();
		Btn_revEditor.gameObject.SetActive(value: false);
		Btn_revEditor.onClick.RemoveAllListeners();
		Btn_returnEditor.gameObject.SetActive(value: false);
		Btn_returnEditor.onClick.RemoveAllListeners();
		Btn_TS.gameObject.SetActive(value: false);
		Btn_TS.onClick.RemoveAllListeners();
		Btn_KSC.gameObject.SetActive(value: false);
		Btn_KSC.onClick.RemoveAllListeners();
		Btn_Menu.gameObject.SetActive(value: false);
		Btn_Menu.onClick.RemoveAllListeners();
		Btn_Close.gameObject.SetActive(value: false);
		Btn_Close.onClick.RemoveAllListeners();
		if (showExitControls)
		{
			if (FlightDriver.CanRevertToPostInit && HighLogic.CurrentGame.Parameters.Flight.CanRestart)
			{
				Btn_revLaunch.gameObject.SetActive(value: true);
				Btn_revLaunch.GetComponentInChildren<TextMeshProUGUI>().text = Localizer.Format("#autoLOC_418666", KSPUtil.PrintTime(Planetarium.GetUniversalTime() - FlightDriver.PostInitState.UniversalTime, 3, explicitPositive: false));
				Btn_revLaunch.onClick.AddListener(delegate
				{
					FlightDriver.RevertToLaunch();
					Close();
				});
			}
			if (HighLogic.CurrentGame.Parameters.Flight.CanLeaveToEditor)
			{
				if (FlightDriver.CanRevertToPrelaunch && ShipConstruction.ShipConfig != null)
				{
					Btn_revEditor.gameObject.SetActive(value: true);
					TextMeshProUGUI componentInChildren = Btn_revEditor.GetComponentInChildren<TextMeshProUGUI>();
					switch (ShipConstruction.ShipType)
					{
					case EditorFacility.const_2:
						componentInChildren.text = Localizer.Format("#autoLOC_418687", KSPUtil.PrintTime(Planetarium.GetUniversalTime() - FlightDriver.PostInitState.UniversalTime, 3, explicitPositive: false));
						Btn_revEditor.onClick.AddListener(delegate
						{
							FlightDriver.RevertToPrelaunch(EditorFacility.const_2);
							Close();
						});
						break;
					case EditorFacility.const_1:
						componentInChildren.text = Localizer.Format("#autoLOC_418682", KSPUtil.PrintTime(Planetarium.GetUniversalTime() - FlightDriver.PostInitState.UniversalTime, 3, explicitPositive: false));
						Btn_revEditor.onClick.AddListener(delegate
						{
							FlightDriver.RevertToPrelaunch(EditorFacility.const_1);
							Close();
						});
						break;
					}
				}
			}
			else if (ShipConstruction.ShipConfig != null)
			{
				Btn_returnEditor.gameObject.SetActive(value: true);
				TextMeshProUGUI componentInChildren2 = Btn_returnEditor.GetComponentInChildren<TextMeshProUGUI>();
				switch (ShipConstruction.ShipType)
				{
				case EditorFacility.const_2:
					componentInChildren2.text = Localizer.Format("#autoLOC_418710");
					Btn_returnEditor.onClick.AddListener(delegate
					{
						onLeavingFlight(GameScenes.EDITOR, EditorFacility.const_2);
						Close();
					});
					break;
				case EditorFacility.const_1:
					componentInChildren2.text = Localizer.Format("#autoLOC_418706");
					Btn_returnEditor.onClick.AddListener(delegate
					{
						onLeavingFlight(GameScenes.EDITOR, EditorFacility.const_1);
						Close();
					});
					break;
				}
			}
			if (HighLogic.CurrentGame.Parameters.Flight.CanLeaveToTrackingStation)
			{
				Btn_TS.gameObject.SetActive(value: true);
				Btn_TS.GetComponentInChildren<TextMeshProUGUI>().text = Localizer.Format("#autoLOC_418723");
				Btn_TS.onClick.AddListener(delegate
				{
					onLeavingFlight(GameScenes.TRACKSTATION, EditorFacility.None);
					Close();
				});
			}
			if (HighLogic.CurrentGame.Parameters.Flight.CanLeaveToSpaceCenter)
			{
				Btn_KSC.gameObject.SetActive(value: true);
				Btn_KSC.GetComponentInChildren<TextMeshProUGUI>().text = Localizer.Format("#autoLOC_418731");
				Btn_KSC.onClick.AddListener(delegate
				{
					HighLogic.LoadScene(GameScenes.SPACECENTER);
					Close();
				});
			}
			if (HighLogic.CurrentGame.Parameters.Flight.CanLeaveToMainMenu)
			{
				Btn_Menu.gameObject.SetActive(value: true);
				Btn_Menu.GetComponentInChildren<TextMeshProUGUI>().text = Localizer.Format("#autoLOC_418739");
				Btn_Menu.onClick.AddListener(delegate
				{
					HighLogic.LoadScene(GameScenes.MAINMENU);
					Close();
				});
			}
		}
		if (allowClosingDialog)
		{
			Btn_Close.gameObject.SetActive(value: true);
			Btn_Close.GetComponentInChildren<TextMeshProUGUI>().text = Localizer.Format("#autoLOC_418748");
			Btn_Close.onClick.AddListener(delegate
			{
				Close();
			});
		}
	}

	public void onLeavingFlight(GameScenes destination, EditorFacility facility)
	{
		display = false;
		string text = "";
		string text2 = "";
		switch (facility)
		{
		default:
			if (destination == GameScenes.TRACKSTATION)
			{
				text = "TrackingStation";
				text2 = Localizer.Format("#autoLOC_418777");
				break;
			}
			Debug.LogError("[LeavingFlight]: Invalid Destination. Only VAB, SPH and TrackingStation require calling this method.");
			return;
		case EditorFacility.const_1:
			text = "VAB";
			text2 = Localizer.Format("#autoLOC_418766");
			break;
		case EditorFacility.const_2:
			text = "SPH";
			text2 = Localizer.Format("#autoLOC_418770");
			break;
		}
		InputLockManager.SetControlLock("LeavingFlightCheck");
		PreFlightCheck preFlightCheck = new PreFlightCheck(delegate
		{
			onLeavingFlightProceed(destination, facility);
		}, onLeavingFlightDismiss);
		preFlightCheck.AddTest(new FacilityOperational(text, text2));
		preFlightCheck.RunTests();
	}

	public void onLeavingFlightProceed(GameScenes scn, EditorFacility facility)
	{
		onLeavingFlightDismiss();
		if (scn == GameScenes.EDITOR)
		{
			FlightDriver.ReturnToEditor(facility);
		}
		else
		{
			HighLogic.LoadScene(scn);
		}
	}

	public void onLeavingFlightDismiss()
	{
		InputLockManager.RemoveControlLock("LeavingFlightCheck");
		display = true;
	}
}

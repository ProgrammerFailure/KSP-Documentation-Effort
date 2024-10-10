using ns2;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

public class UISpaceCenter : MonoBehaviour
{
	public Button quitBtn;

	public TextMeshProUGUI buildingText;

	public KSCPauseMenu pauseMenu;

	public bool _spawnedAC;

	public bool _spawnedRD;

	public bool _spawnedMC;

	public bool _spawnedVSD;

	public bool _spawnedADM;

	public static UISpaceCenter Instance { get; set; }

	public bool SpawnedAC => _spawnedAC;

	public bool SpawnedRD => _spawnedRD;

	public bool SpawnedMC => _spawnedMC;

	public bool SpawnedVSD => _spawnedVSD;

	public bool SpawnedADM => _spawnedADM;

	public void Awake()
	{
		Instance = this;
		SetBuildingText("");
		quitBtn.onClick.AddListener(QuitToMenu);
		GameEvents.onInputLocksModified.Add(onInputLockModified);
		GameEvents.onGUIAstronautComplexSpawn.Add(OnFacilitySpawn_AC);
		GameEvents.onGUIRnDComplexSpawn.Add(OnFacilitySpawn_RD);
		GameEvents.onGUIMissionControlSpawn.Add(OnFacilitySpawn_MC);
		GameEvents.onGUILaunchScreenSpawn.Add(OnFacilitySpawn_VSD);
		GameEvents.onGUIAdministrationFacilitySpawn.Add(OnFacilitySpawn_ADM);
		GameEvents.onGUIAstronautComplexDespawn.Add(OnFacilityDespawn_AC);
		GameEvents.onGUIRnDComplexDespawn.Add(OnFacilityDespawn_RD);
		GameEvents.onGUIMissionControlDespawn.Add(OnFacilityDespawn_MC);
		GameEvents.onGUILaunchScreenDespawn.Add(OnFacilityDespawn_VSD);
		GameEvents.onGUIAdministrationFacilityDespawn.Add(OnFacilityDespawn_ADM);
	}

	public void OnFacilitySpawn_AC()
	{
		_spawnedAC = true;
		Pause();
	}

	public void OnFacilitySpawn_RD()
	{
		_spawnedRD = true;
		Pause();
	}

	public void OnFacilitySpawn_MC()
	{
		_spawnedMC = true;
		Pause();
	}

	public void OnFacilitySpawn_VSD(GameEvents.VesselSpawnInfo info)
	{
		_spawnedVSD = true;
	}

	public void OnFacilitySpawn_ADM()
	{
		_spawnedADM = true;
		Pause();
	}

	public void OnFacilityDespawn_AC()
	{
		UnPause();
		_spawnedAC = false;
	}

	public void OnFacilityDespawn_RD()
	{
		UnPause();
		_spawnedRD = false;
	}

	public void OnFacilityDespawn_MC()
	{
		UnPause();
		_spawnedMC = false;
	}

	public void OnFacilityDespawn_VSD()
	{
		_spawnedVSD = false;
	}

	public void OnFacilityDespawn_ADM()
	{
		UnPause();
		_spawnedADM = false;
	}

	public void Pause()
	{
		if (!FlightDriver.Pause)
		{
			if (TimeWarp.CurrentRateIndex != 0)
			{
				TimeWarp.SetRate(0, instant: true);
			}
			FlightDriver.SetPause(pauseState: true, postScreenMessage: false);
		}
	}

	public void UnPause()
	{
		if (FlightDriver.Pause)
		{
			FlightDriver.SetPause(pauseState: false, postScreenMessage: false);
		}
	}

	public void OnDestroy()
	{
		GameEvents.onInputLocksModified.Remove(onInputLockModified);
		GameEvents.onGUIAstronautComplexSpawn.Remove(OnFacilitySpawn_AC);
		GameEvents.onGUIRnDComplexSpawn.Remove(OnFacilitySpawn_RD);
		GameEvents.onGUIMissionControlSpawn.Remove(OnFacilitySpawn_MC);
		GameEvents.onGUILaunchScreenSpawn.Remove(OnFacilitySpawn_VSD);
		GameEvents.onGUIAdministrationFacilitySpawn.Remove(OnFacilitySpawn_ADM);
		GameEvents.onGUIAstronautComplexDespawn.Remove(OnFacilityDespawn_AC);
		GameEvents.onGUIRnDComplexDespawn.Remove(OnFacilityDespawn_RD);
		GameEvents.onGUIMissionControlDespawn.Remove(OnFacilityDespawn_MC);
		GameEvents.onGUILaunchScreenDespawn.Remove(OnFacilityDespawn_VSD);
		GameEvents.onGUIAdministrationFacilityDespawn.Remove(OnFacilityDespawn_ADM);
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			if (PopupDialog.CheckForOpenDialogs())
			{
				return;
			}
			if (_spawnedAC)
			{
				GameEvents.onGUIAstronautComplexDespawn.Fire();
				return;
			}
			if (_spawnedRD)
			{
				GameEvents.onGUIRnDComplexDespawn.Fire();
				return;
			}
			if (_spawnedMC)
			{
				GameEvents.onGUIMissionControlDespawn.Fire();
				return;
			}
			if (_spawnedVSD)
			{
				if (InputLockManager.IsUnlocked(ControlTypes.UI_DRAGGING) && !VesselSpawnDialog.Instance.HasSearchText)
				{
					GameEvents.onGUILaunchScreenDespawn.Fire();
				}
				return;
			}
			if (_spawnedADM)
			{
				GameEvents.onGUIAdministrationFacilityDespawn.Fire();
				return;
			}
			QuitToMenu();
		}
		if (pauseMenu == null && !InputLockManager.IsLocked(ControlTypes.KSC_UI))
		{
			if (GameSettings.QUICKSAVE.GetKeyDown())
			{
				QuitToMenu();
				pauseMenu.InitiateSave();
			}
			else if (GameSettings.QUICKLOAD.GetKeyDown())
			{
				QuitToMenu();
				pauseMenu.InitiateLoad();
			}
		}
	}

	public void OnApplicationFocus(bool focus)
	{
		if (!focus)
		{
			InputLockManager.SetControlLock("ksc_ApplicationFocus");
		}
		else
		{
			InputLockManager.RemoveControlLock("ksc_ApplicationFocus");
		}
	}

	public void onInputLockModified(GameEvents.FromToAction<ControlTypes, ControlTypes> ctrls)
	{
		if (InputLockManager.IsLocking(ControlTypes.KSC_UI, ctrls))
		{
			lockUI();
		}
		if (InputLockManager.IsUnlocking(ControlTypes.KSC_UI, ctrls))
		{
			unlockUI();
		}
	}

	public void lockUI()
	{
		quitBtn.Lock();
	}

	public void unlockUI()
	{
		quitBtn.Unlock();
	}

	public void QuitToMenu()
	{
		if (!InputLockManager.IsLocked(ControlTypes.KSC_UI) && !(pauseMenu != null))
		{
			pauseMenu = KSCPauseMenu.Create(OnPauseMenuDismiss);
			pauseMenu.BuildButtonList(pauseMenu.dialogObj);
		}
	}

	public void OnPauseMenuDismiss()
	{
		pauseMenu = null;
	}

	public void SetBuildingText(string text)
	{
		if (buildingText != null && buildingText.gameObject.activeInHierarchy)
		{
			buildingText.text = text;
		}
	}
}

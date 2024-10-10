using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns29;

public class ScreenPhysics : MonoBehaviour
{
	public TMP_InputField databaseField;

	public Button saveButton;

	public Button loadButton;

	public Toggle autostrutVisualizeToggle;

	public Toggle legacyOrbitTargetingToggle;

	public void Start()
	{
		saveButton.onClick.AddListener(OnSaveClicked);
		loadButton.onClick.AddListener(OnLoadClicked);
		autostrutVisualizeToggle.onValueChanged.AddListener(OnAutostrutVisualizeToggled);
		legacyOrbitTargetingToggle.onValueChanged.AddListener(OnLegacyOrbitTargetingToggled);
		autostrutVisualizeToggle.isOn = PhysicsGlobals.AutoStrutDisplay;
		legacyOrbitTargetingToggle.isOn = GameSettings.LEGACY_ORBIT_TARGETING;
		try
		{
			databaseField.text = Path.GetFullPath(PhysicsGlobals.PhysicsDatabaseFilename);
		}
		catch (Exception)
		{
			databaseField.text = PhysicsGlobals.PhysicsDatabaseFilename;
		}
	}

	public void OnSaveClicked()
	{
		PhysicsGlobals.PhysicsDatabaseFilename = databaseField.text;
		PhysicsGlobals.Instance.SaveDatabase();
	}

	public void OnLoadClicked()
	{
		PhysicsGlobals.PhysicsDatabaseFilename = databaseField.text;
		PhysicsGlobals.Instance.LoadDatabase();
	}

	public void OnAutostrutVisualizeToggled(bool on)
	{
		PhysicsGlobals.AutoStrutDisplay = on;
	}

	public void OnLegacyOrbitTargetingToggled(bool on)
	{
		GameSettings.LEGACY_ORBIT_TARGETING = on;
	}
}

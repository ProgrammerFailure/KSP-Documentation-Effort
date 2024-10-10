using System;
using EdyCommonTools;
using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Utility/Settings Switcher", 42)]
public class VPSettingsSwitcher : VehicleBehaviour
{
	[Serializable]
	public class SettingsGroup
	{
		public string name = "Settings";

		public Color uiColor = Color.white;

		public bool setSteeringAids;

		public bool setTractionControl;

		public bool setStabilityControl;

		public bool setAntiSpin;

		public bool setDifferential;

		public SteeringAids.Settings steeringAids = new SteeringAids.Settings();

		public TractionControl.Settings tractionControl = new TractionControl.Settings();

		public StabilityControl.Settings stabilityControl = new StabilityControl.Settings();

		public AntiSpin.Settings antiSpin = new AntiSpin.Settings();

		public Differential.Settings differential = new Differential.Settings();

		public void LoadFromVehicle(VPVehicleController vehicle)
		{
			ObjectUtility.CopyObjectOverwrite(vehicle.steeringAids, ref steeringAids);
			ObjectUtility.CopyObjectOverwrite(vehicle.tractionControl, ref tractionControl);
			ObjectUtility.CopyObjectOverwrite(vehicle.stabilityControl, ref stabilityControl);
			ObjectUtility.CopyObjectOverwrite(vehicle.antiSpin, ref antiSpin);
			ObjectUtility.CopyObjectOverwrite(vehicle.differential, ref differential);
		}

		public void SaveToVehicle(VPVehicleController vehicle, bool force = false)
		{
			if (force || setSteeringAids)
			{
				ObjectUtility.CopyObjectOverwrite(steeringAids, ref vehicle.steeringAids);
			}
			if (force || setTractionControl)
			{
				ObjectUtility.CopyObjectOverwrite(tractionControl, ref vehicle.tractionControl);
			}
			if (force || setStabilityControl)
			{
				ObjectUtility.CopyObjectOverwrite(stabilityControl, ref vehicle.stabilityControl);
			}
			if (force || setAntiSpin)
			{
				ObjectUtility.CopyObjectOverwrite(antiSpin, ref vehicle.antiSpin);
			}
			if (force || setDifferential)
			{
				ObjectUtility.CopyObjectOverwrite(differential, ref vehicle.differential);
			}
		}
	}

	public int selectedGroup;

	public SettingsGroup[] settingsGroups = new SettingsGroup[0];

	public SettingsGroup m_currentGroup;

	public VPVehicleController m_vehicle;

	public SettingsGroup m_originalSettings = new SettingsGroup();

	public SettingsGroup currentGroup => m_currentGroup;

	public override void OnEnableVehicle()
	{
		m_vehicle = GetComponent<VPVehicleController>();
		if (m_vehicle == null)
		{
			DebugLogWarning("This component requires a VPVehicleController-based vehicle. Component disabled.");
			base.enabled = false;
		}
		m_currentGroup = null;
	}

	public override void OnDisableVehicle()
	{
		if (m_currentGroup != null)
		{
			m_originalSettings.SaveToVehicle(m_vehicle, force: true);
		}
		m_currentGroup = null;
	}

	public override void FixedUpdateVehicle()
	{
		Refresh();
	}

	public void Refresh()
	{
		if (!base.vehicle.initialized || !base.isActiveAndEnabled)
		{
			return;
		}
		SettingsGroup settingsGroup = null;
		if (selectedGroup >= 0 && selectedGroup < settingsGroups.Length)
		{
			settingsGroup = settingsGroups[selectedGroup];
		}
		if (m_currentGroup != settingsGroup)
		{
			if (m_currentGroup == null)
			{
				m_originalSettings.LoadFromVehicle(m_vehicle);
			}
			else
			{
				m_originalSettings.SaveToVehicle(m_vehicle, force: true);
			}
			settingsGroup?.SaveToVehicle(m_vehicle);
			m_currentGroup = settingsGroup;
		}
	}
}

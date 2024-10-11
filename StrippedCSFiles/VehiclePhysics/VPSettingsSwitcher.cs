using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Utility/Settings Switcher", 42)]
public class VPSettingsSwitcher : VehicleBehaviour
{
	[Serializable]
	public class SettingsGroup
	{
		public string name;

		public Color uiColor;

		public bool setSteeringAids;

		public bool setTractionControl;

		public bool setStabilityControl;

		public bool setAntiSpin;

		public bool setDifferential;

		public SteeringAids.Settings steeringAids;

		public TractionControl.Settings tractionControl;

		public StabilityControl.Settings stabilityControl;

		public AntiSpin.Settings antiSpin;

		public Differential.Settings differential;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public SettingsGroup()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void LoadFromVehicle(VPVehicleController vehicle)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SaveToVehicle(VPVehicleController vehicle, bool force = false)
		{
			throw null;
		}
	}

	public int selectedGroup;

	public SettingsGroup[] settingsGroups;

	private SettingsGroup m_currentGroup;

	private VPVehicleController m_vehicle;

	private SettingsGroup m_originalSettings;

	public SettingsGroup currentGroup
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VPSettingsSwitcher()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnEnableVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnDisableVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void FixedUpdateVehicle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Refresh()
	{
		throw null;
	}
}

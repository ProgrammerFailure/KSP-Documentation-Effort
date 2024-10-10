using System;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[MEGUI_VesselLocation]
public class MEGUIParameterVesselLocation : MEGUICompoundParameter
{
	public RectTransform containerHeader;

	public Image imageHeader;

	public Image imageBackGround;

	public MEGUIParameterDropdownList situationDropDown;

	public MEGUIParameterDropdownList facilityDropDown;

	public MEGUIParameterDropdownList launchsiteDropDown;

	public MEGUIParameterCheckbox brakesOn;

	public MEGUIParameterCheckbox splashed;

	public VerticalLayoutGroup dropDownLayoutGroup;

	public VesselLocation vesselLocation;

	public MEGUIParameterCelestialBodyOrbit vesselOrbitLocation;

	public MEGUIParameterCelestialBody_VesselGroundLocation vesselGroundLocation;

	public EditorFacility selectedFacility;

	public MissionSituation.VesselStartSituations selectedStartSituation = MissionSituation.VesselStartSituations.PRELAUNCH;

	public VesselLocation FieldValue
	{
		get
		{
			return vesselLocation;
		}
		set
		{
			vesselLocation = value;
			if (field != null)
			{
				field.SetValue(value);
			}
		}
	}

	public static MEGUIParameterVesselLocation Create(VesselLocation vesselLocation, Transform parent)
	{
		MEGUIParameterVesselLocation obj = MEGUIParametersController.Instance.GetControl(typeof(MEGUI_VesselLocation)).Create(null, parent) as MEGUIParameterVesselLocation;
		obj.Setup("LocationInstance", vesselLocation);
		return obj;
	}

	public override void Setup(string name, object value)
	{
		subParametersSelectable = true;
		base.Setup(name, value);
		vesselLocation = value as VesselLocation;
		if (subParameters.ContainsKey("situation"))
		{
			situationDropDown = subParameters["situation"] as MEGUIParameterDropdownList;
			situationDropDown.dropdownList.onValueChanged.AddListener(DropDownAction_Situation);
		}
		if (subParameters.ContainsKey("facility"))
		{
			facilityDropDown = subParameters["facility"] as MEGUIParameterDropdownList;
			facilityDropDown.dropdownList.onValueChanged.AddListener(DropDownAction_Facility);
		}
		if (subParameters.ContainsKey("launchSite"))
		{
			launchsiteDropDown = subParameters["launchSite"] as MEGUIParameterDropdownList;
			launchsiteDropDown.dropdownList.onValueChanged.AddListener(DropDownAction_LaunchSite);
			if (string.IsNullOrEmpty(vesselLocation.launchSite))
			{
				DropDownAction_LaunchSite(0);
			}
		}
		if (subParameters.ContainsKey("brakesOn"))
		{
			brakesOn = subParameters["brakesOn"] as MEGUIParameterCheckbox;
		}
		if (subParameters.ContainsKey("splashed"))
		{
			splashed = subParameters["splashed"] as MEGUIParameterCheckbox;
		}
		title.text = name;
		vesselGroundLocation = subParameters["vesselGroundLocation"] as MEGUIParameterCelestialBody_VesselGroundLocation;
		if (FieldValue.situation == MissionSituation.VesselStartSituations.LANDED)
		{
			vesselGroundLocation.gameObject.SetActive(value: true);
		}
		else
		{
			vesselGroundLocation.gameObject.SetActive(value: false);
		}
		if (brakesOn != null)
		{
			if (FieldValue.situation != MissionSituation.VesselStartSituations.LANDED && FieldValue.situation != MissionSituation.VesselStartSituations.PRELAUNCH)
			{
				brakesOn.gameObject.SetActive(value: false);
			}
			else
			{
				brakesOn.gameObject.SetActive(value: true);
			}
		}
		if (splashed != null)
		{
			if (FieldValue.situation == MissionSituation.VesselStartSituations.LANDED)
			{
				splashed.gameObject.SetActive(value: true);
			}
			else
			{
				splashed.gameObject.SetActive(value: false);
			}
		}
		vesselOrbitLocation = subParameters["orbitSnapShot"] as MEGUIParameterCelestialBodyOrbit;
		vesselOrbitLocation.gameObject.SetActive(FieldValue.situation == MissionSituation.VesselStartSituations.ORBITING);
		if (FieldValue.situation == MissionSituation.VesselStartSituations.ORBITING)
		{
			vesselOrbitLocation.gameObject.SetActive(value: true);
		}
		else
		{
			vesselOrbitLocation.gameObject.SetActive(value: false);
		}
		selectedStartSituation = FieldValue.situation;
	}

	public override void Display()
	{
		if (FieldValue.situation == MissionSituation.VesselStartSituations.LANDED)
		{
			vesselGroundLocation.Display();
		}
		else if (FieldValue.situation == MissionSituation.VesselStartSituations.ORBITING)
		{
			vesselOrbitLocation.Display();
		}
	}

	public override void OnEnable()
	{
		base.OnEnable();
		if (launchsiteDropDown != null)
		{
			launchsiteDropDown.RebuildDropDown();
			UpdateNodeBodyUI();
		}
	}

	public void DropDownAction_Situation(int index)
	{
		try
		{
			selectedStartSituation = (MissionSituation.VesselStartSituations)situationDropDown.SelectedValue;
		}
		catch
		{
			selectedStartSituation = MissionSituation.VesselStartSituations.PRELAUNCH;
		}
		setSituationType();
	}

	public void DropDownAction_Facility(int index)
	{
		try
		{
			selectedFacility = (EditorFacility)facilityDropDown.SelectedValue;
		}
		catch
		{
			selectedFacility = EditorFacility.None;
		}
	}

	public void DropDownAction_LaunchSite(int index)
	{
		FieldValue.launchSite = "LaunchPad";
		string launchSite = launchsiteDropDown.SelectedValue as string;
		FieldValue.launchSite = launchSite;
		FieldValue.vesselGroundLocation.latitude = 0.0;
		FieldValue.vesselGroundLocation.longitude = 0.0;
		FieldValue.vesselGroundLocation.altitude = 0.0;
		FieldValue.vesselGroundLocation.rotation = new MissionQuaternion();
		CelestialBody launchSiteBody = PSystemSetup.Instance.GetLaunchSiteBody(FieldValue.launchSite);
		if (launchSiteBody != null)
		{
			FieldValue.vesselGroundLocation.targetBody = launchSiteBody;
		}
	}

	public void setSituationType()
	{
		switch (selectedStartSituation)
		{
		case MissionSituation.VesselStartSituations.ORBITING:
			if (facilityDropDown != null)
			{
				facilityDropDown.gameObject.SetActive(value: false);
			}
			if (launchsiteDropDown != null)
			{
				launchsiteDropDown.gameObject.SetActive(value: false);
			}
			if (brakesOn != null)
			{
				brakesOn.gameObject.SetActive(value: false);
			}
			if (splashed != null)
			{
				splashed.gameObject.SetActive(value: false);
			}
			vesselGroundLocation.gameObject.SetActive(value: false);
			vesselOrbitLocation.Display();
			return;
		case MissionSituation.VesselStartSituations.LANDED:
			if (facilityDropDown != null)
			{
				facilityDropDown.gameObject.SetActive(value: false);
			}
			if (launchsiteDropDown != null)
			{
				launchsiteDropDown.gameObject.SetActive(value: false);
			}
			if (brakesOn != null)
			{
				brakesOn.gameObject.SetActive(value: true);
			}
			if (splashed != null)
			{
				splashed.gameObject.SetActive(value: true);
			}
			FieldValue.vesselGroundLocation.targetBody = FlightGlobals.GetHomeBody();
			vesselOrbitLocation.gameObject.SetActive(value: false);
			vesselGroundLocation.Display();
			return;
		}
		if (facilityDropDown != null)
		{
			facilityDropDown.gameObject.SetActive(value: true);
		}
		if (launchsiteDropDown != null)
		{
			launchsiteDropDown.gameObject.SetActive(value: true);
			if (string.IsNullOrEmpty(vesselLocation.launchSite))
			{
				DropDownAction_LaunchSite(0);
			}
		}
		if (brakesOn != null)
		{
			brakesOn.gameObject.SetActive(value: true);
		}
		if (splashed != null)
		{
			splashed.gameObject.SetActive(value: false);
		}
		MissionEditorLogic.Instance.actionPane.OnParameterClick(null);
		vesselGroundLocation.gameObject.SetActive(value: false);
		vesselOrbitLocation.gameObject.SetActive(value: false);
	}

	public override ConfigNode GetState()
	{
		ConfigNode configNode = new ConfigNode();
		configNode.AddValue("facility", FieldValue.facility);
		configNode.AddValue("situation", FieldValue.situation);
		configNode.AddValue("launchSite", FieldValue.launchSite);
		configNode.AddValue("latitude", FieldValue.vesselGroundLocation.latitude);
		configNode.AddValue("longitude", FieldValue.vesselGroundLocation.longitude);
		configNode.AddValue("altitude", FieldValue.vesselGroundLocation.altitude);
		configNode.AddValue("rotation", FieldValue.vesselGroundLocation.rotation.quaternion);
		return configNode;
	}

	public void OnHistoryUpdateDropdowns(ConfigNode data, HistoryType type)
	{
		string value = "";
		data.TryGetValue("facility", ref value);
		FieldValue.facility = (EditorFacility)Enum.Parse(typeof(EditorFacility), value);
		string value2 = "";
		data.TryGetValue("situation", ref value2);
		if (!string.IsNullOrEmpty(value2))
		{
			FieldValue.situation = (MissionSituation.VesselStartSituations)Enum.Parse(typeof(MissionSituation.VesselStartSituations), value2);
		}
		data.TryGetValue("launchSite", ref FieldValue.launchSite);
		data.TryGetValue("latitude", ref FieldValue.vesselGroundLocation.latitude);
		data.TryGetValue("longitude", ref FieldValue.vesselGroundLocation.longitude);
		data.TryGetValue("altitude", ref FieldValue.vesselGroundLocation.altitude);
		Vector3 value3 = default(Vector3);
		data.TryGetValue("rotation", ref value3);
		FieldValue.vesselGroundLocation.rotation = new MissionQuaternion(value3);
	}
}

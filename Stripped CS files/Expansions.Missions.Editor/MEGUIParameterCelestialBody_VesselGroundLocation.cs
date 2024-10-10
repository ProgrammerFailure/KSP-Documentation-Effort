using System;
using UnityEngine;

namespace Expansions.Missions.Editor;

[MEGUI_VesselGroundLocation]
public class MEGUIParameterCelestialBody_VesselGroundLocation : MEGUICompoundParameter
{
	public GAPCelestialBody gapCB;

	public GAPCelestialBody_SurfaceGizmo_PlaceVessel gizmoRef;

	public MEGUIParameterDropdownList dropDownBodies;

	public MEGUIParameterNumberRange rangeLatitude;

	public MEGUIParameterNumberRange rangeLongitude;

	public MEGUIParameterInputField inputAltitude;

	public MEGUIParameterQuaternion quaternionVessel;

	public bool disableRotateX;

	public bool disableRotateY;

	public bool disableRotateZ;

	public VesselGroundLocation FieldValue
	{
		get
		{
			return (VesselGroundLocation)field.GetValue();
		}
		set
		{
			field.SetValue(value);
		}
	}

	public override void Setup(string name, object value)
	{
		base.Setup(name, value);
		title.text = name;
		dropDownBodies = subParameters["targetBody"] as MEGUIParameterDropdownList;
		dropDownBodies.dropdownList.onValueChanged.AddListener(OnDropDownBody);
		rangeLatitude = subParameters["latitude"] as MEGUIParameterNumberRange;
		rangeLatitude.slider.onValueChanged.AddListener(OnInputLatitude);
		rangeLongitude = subParameters["longitude"] as MEGUIParameterNumberRange;
		rangeLongitude.slider.onValueChanged.AddListener(OnInputLongitude);
		inputAltitude = subParameters["altitude"] as MEGUIParameterInputField;
		inputAltitude.inputField.interactable = false;
		quaternionVessel = subParameters["rotation"] as MEGUIParameterQuaternion;
		MEGUIParameterQuaternion mEGUIParameterQuaternion = quaternionVessel;
		mEGUIParameterQuaternion.onValueChanged = (MEGUIParameterQuaternion.OnValueChanged)Delegate.Combine(mEGUIParameterQuaternion.onValueChanged, new MEGUIParameterQuaternion.OnValueChanged(OnInputRot));
		(subParameters["splashed"] as MEGUIParameterCheckbox).gameObject.SetActive(((MEGUI_VesselGroundLocation)field.Attribute).AllowWaterSurfacePlacement);
		disableRotateX = ((MEGUI_VesselGroundLocation)field.Attribute).DisableRotationX;
		disableRotateY = ((MEGUI_VesselGroundLocation)field.Attribute).DisableRotationY;
		disableRotateZ = ((MEGUI_VesselGroundLocation)field.Attribute).DisableRotationZ;
		quaternionVessel.ToggleEulerAxis(MEGUIParameterQuaternion.EulerAxis.x, !disableRotateX);
		quaternionVessel.ToggleEulerAxis(MEGUIParameterQuaternion.EulerAxis.y, !disableRotateY);
		quaternionVessel.ToggleEulerAxis(MEGUIParameterQuaternion.EulerAxis.z, !disableRotateZ);
		if (disableRotateX && disableRotateY && disableRotateZ)
		{
			quaternionVessel.gameObject.SetActive(value: false);
		}
		subParameters["gizmoIcon"].gameObject.SetActive(((MEGUI_VesselGroundLocation)field.Attribute).DisplayVesselGizmoOptions);
		VesselGroundLocation fieldValue = FieldValue;
		fieldValue.OnGizmoIconChange = (VesselGroundLocation.OnGizmoIconChangeDelegate)Delegate.Combine(fieldValue.OnGizmoIconChange, new VesselGroundLocation.OnGizmoIconChangeDelegate(OnGizmoIconChange));
	}

	public void OnGizmoIconChange(VesselGroundLocation.GizmoIcon newIcon)
	{
		if (gizmoRef != null)
		{
			gizmoRef.SetGizmoIcon(newIcon);
		}
	}

	public void OnInputLatitude(float inputValue)
	{
		FieldValue.altitude = FieldValue.targetBody.TerrainAltitude(FieldValue.latitude, FieldValue.longitude, allowNegative: true);
		inputAltitude.inputField.text = FieldValue.altitude.ToString();
		UpdateGizmo();
		UpdateNodeBodyUI();
	}

	public void OnInputLongitude(float inputValue)
	{
		FieldValue.altitude = FieldValue.targetBody.TerrainAltitude(FieldValue.latitude, FieldValue.longitude, allowNegative: true);
		inputAltitude.inputField.text = FieldValue.altitude.ToString();
		UpdateGizmo();
		UpdateNodeBodyUI();
	}

	public void OnInputRot(Quaternion quaternion)
	{
		UpdateGizmo();
	}

	public void OnDropDownBody(int value)
	{
		if (gapCB == null)
		{
			gapCB = MissionEditorLogic.Instance.actionPane.GAPInitialize<GAPCelestialBody>();
		}
		gapCB.Load(FieldValue.targetBody);
		UpdateGizmo();
	}

	public void OnGapLeftArrow()
	{
		int itemIndex = dropDownBodies.GetItemIndex(FieldValue.targetBody);
		itemIndex = (itemIndex + dropDownBodies.dropdownList.options.Count - 1) % dropDownBodies.dropdownList.options.Count;
		dropDownBodies.dropdownList.value = itemIndex;
	}

	public void OnGapRightArrow()
	{
		int itemIndex = dropDownBodies.GetItemIndex(FieldValue.targetBody);
		itemIndex = (itemIndex + 1) % dropDownBodies.dropdownList.options.Count;
		dropDownBodies.dropdownList.value = itemIndex;
	}

	public void OnGAPPointTranslate(double latitude, double longitude)
	{
		rangeLatitude.slider.value = (float)latitude;
		rangeLongitude.slider.value = (float)longitude;
	}

	public void OnGapPointRotate(Quaternion quaternion)
	{
		quaternionVessel.SetQuaternion(quaternion);
	}

	public void UpdateGizmo()
	{
		if (gizmoRef != null)
		{
			gizmoRef.SetGizmoPosition(FieldValue.latitude, FieldValue.longitude, FieldValue.altitude);
			gizmoRef.SetGizmoRotation(FieldValue.rotation.quaternion);
			if (!gizmoRef.IsActive())
			{
				gapCB.CelestialCamera.SetPosition(FieldValue.latitude, FieldValue.longitude);
			}
			gapCB.SurfaceGizmo.UpdateFooterText();
		}
	}

	public override void DisplayGAP()
	{
		base.DisplayGAP();
		if (gapCB == null)
		{
			gapCB = MissionEditorLogic.Instance.actionPane.GAPInitialize<GAPCelestialBody>();
		}
		gapCB.Load(FieldValue.targetBody);
		gapCB.SetState(GAPCelestialBodyState.POINT);
		gapCB.TogglePQS(usePQS: true);
		gapCB.SurfaceGizmo.InstantiateGizmo("GAP_CelestialBody_SurfaceGizmo_Vessel");
		gapCB.Load(FieldValue.targetBody);
		gapCB.SuscribeToLeftButton(OnGapLeftArrow);
		gapCB.SuscribeToRightButton(OnGapRightArrow);
		gizmoRef = gapCB.SurfaceGizmo.GizmoEntity as GAPCelestialBody_SurfaceGizmo_PlaceVessel;
		if (gizmoRef == null)
		{
			Debug.Log("[MissionBuilder]: Failed to get Vessel Ground Location Gizmo");
			return;
		}
		if (disableRotateX)
		{
			for (int i = 0; i < gizmoRef.handlesRotation.Count; i++)
			{
				if (gizmoRef.handlesRotation[i].name.Contains("Handle X"))
				{
					gizmoRef.handlesRotation[i].transform.parent.gameObject.SetActive(value: false);
					gizmoRef.handlesRotation.RemoveAt(i);
					break;
				}
			}
		}
		if (disableRotateY)
		{
			for (int j = 0; j < gizmoRef.handlesRotation.Count; j++)
			{
				if (gizmoRef.handlesRotation[j].name.Contains("Handle Y"))
				{
					gizmoRef.handlesRotation[j].transform.parent.gameObject.SetActive(value: false);
					gizmoRef.handlesRotation.RemoveAt(j);
					break;
				}
			}
		}
		if (disableRotateZ)
		{
			for (int k = 0; k < gizmoRef.handlesRotation.Count; k++)
			{
				if (gizmoRef.handlesRotation[k].name.Contains("Handle Z"))
				{
					gizmoRef.handlesRotation[k].transform.parent.gameObject.SetActive(value: false);
					gizmoRef.handlesRotation.RemoveAt(k);
					break;
				}
			}
		}
		OnGizmoIconChange(FieldValue.GAPGizmoIcon);
		GAPCelestialBody_SurfaceGizmo_PlaceVessel gAPCelestialBody_SurfaceGizmo_PlaceVessel = gizmoRef;
		gAPCelestialBody_SurfaceGizmo_PlaceVessel.OnGAPGizmoTranslate = (GAPCelestialBody_InteractiveSurfaceGizmo.OnPointTranslate)Delegate.Combine(gAPCelestialBody_SurfaceGizmo_PlaceVessel.OnGAPGizmoTranslate, new GAPCelestialBody_InteractiveSurfaceGizmo.OnPointTranslate(OnGAPPointTranslate));
		GAPCelestialBody_SurfaceGizmo_PlaceVessel gAPCelestialBody_SurfaceGizmo_PlaceVessel2 = gizmoRef;
		gAPCelestialBody_SurfaceGizmo_PlaceVessel2.OnGAPGizmoRotate = (GAPCelestialBody_SurfaceGizmo_PlaceVessel.OnPointRotate)Delegate.Combine(gAPCelestialBody_SurfaceGizmo_PlaceVessel2.OnGAPGizmoRotate, new GAPCelestialBody_SurfaceGizmo_PlaceVessel.OnPointRotate(OnGapPointRotate));
		_ = FieldValue.rotation.eulerAngles;
		gizmoRef.SetGizmoPosition(FieldValue.latitude, FieldValue.longitude, FieldValue.altitude);
		gizmoRef.SetGizmoRotation(FieldValue.rotation.quaternion);
		if (!disableRotateX || !disableRotateY || !disableRotateZ)
		{
			gizmoRef.AddTransformButtons();
		}
		gapCB.CelestialCamera.OverridePosition(FieldValue.latitude, FieldValue.longitude);
		UpdateGizmo();
	}
}

using System;
using UnityEngine;

namespace Expansions.Missions.Editor;

[MEGUI_SurfaceVolume]
public class MEGUIParameterCelestialBody_Volume : MEGUICompoundParameter
{
	public GAPCelestialBody gapCB;

	public GAPCelestialBody_SurfaceGizmo_Volume gizmoRef;

	public MEGUIParameterDropdownList dropDownBodies;

	public MEGUIParameterNumberRange rangeLatitude;

	public MEGUIParameterNumberRange rangeLongitude;

	public MEGUIParameterDropdownList dropDownShape;

	public MEGUIParameterNumberRange rangeRadius;

	public MEGUIParameterNumberRange rangeHeightConeMin;

	public MEGUIParameterNumberRange rangeHeightConeMax;

	public MEGUIParameterNumberRange rangeHeightSphere;

	public CelestialBody pastBody;

	public SurfaceVolume FieldValue
	{
		get
		{
			return (SurfaceVolume)field.GetValue();
		}
		set
		{
			field.SetValue(value);
		}
	}

	public override void Setup(string name)
	{
		base.Setup(name);
		title.text = name;
		dropDownBodies = subParameters["body"] as MEGUIParameterDropdownList;
		rangeLatitude = subParameters["latitude"] as MEGUIParameterNumberRange;
		rangeLongitude = subParameters["longitude"] as MEGUIParameterNumberRange;
		dropDownShape = subParameters["shape"] as MEGUIParameterDropdownList;
		rangeRadius = subParameters["radius"] as MEGUIParameterNumberRange;
		rangeHeightConeMin = subParameters["heightConeMin"] as MEGUIParameterNumberRange;
		rangeHeightConeMax = subParameters["heightConeMax"] as MEGUIParameterNumberRange;
		rangeHeightSphere = subParameters["heightSphere"] as MEGUIParameterNumberRange;
		AttachCallbacks();
		pastBody = FieldValue.body;
		switch (FieldValue.shape)
		{
		case SurfaceVolume.VolumeShape.Sphere:
			rangeHeightConeMax.gameObject.SetActive(value: false);
			rangeHeightConeMin.gameObject.SetActive(value: false);
			rangeHeightSphere.gameObject.SetActive(value: true);
			break;
		case SurfaceVolume.VolumeShape.Cone:
			rangeHeightConeMax.gameObject.SetActive(value: true);
			rangeHeightConeMin.gameObject.SetActive(value: true);
			rangeHeightSphere.gameObject.SetActive(value: false);
			break;
		}
	}

	public void AttachCallbacks()
	{
		dropDownBodies.dropdownList.onValueChanged.AddListener(OnDropDownBody);
		rangeLatitude.slider.onValueChanged.AddListener(OnInputLatitude);
		rangeLongitude.slider.onValueChanged.AddListener(OnInputLongitude);
		dropDownShape.dropdownList.onValueChanged.AddListener(OnDropDownShape);
		rangeRadius.slider.onValueChanged.AddListener(OnInputRadius);
		rangeHeightConeMin.slider.onValueChanged.AddListener(OnInputHeightConeMin);
		rangeHeightConeMax.slider.onValueChanged.AddListener(OnInputHeightConeMax);
		rangeHeightSphere.slider.onValueChanged.AddListener(OnInputHeightSphere);
	}

	public void OnDragRadius(float newRadius)
	{
		rangeRadius.slider.value = newRadius;
	}

	public void OnInputLatitude(float newValue)
	{
		MinValuesUpdate();
		UpdateGizmo();
		UpdateNodeBodyUI();
	}

	public void OnInputLongitude(float newValue)
	{
		MinValuesUpdate();
		UpdateGizmo();
		UpdateNodeBodyUI();
	}

	public void MinValuesUpdate()
	{
		RefreshMinHeightValues();
		switch (FieldValue.shape)
		{
		default:
			FieldValue.altitude = FieldValue.body.TerrainAltitude(FieldValue.latitude, FieldValue.longitude, allowNegative: true);
			break;
		case SurfaceVolume.VolumeShape.Sphere:
			if ((double)FieldValue.heightSphere < rangeHeightSphere.MinValue)
			{
				FieldValue.heightSphere = (float)rangeHeightSphere.MinValue;
			}
			FieldValue.altitude = FieldValue.heightSphere;
			rangeHeightSphere.RefreshUI();
			break;
		case SurfaceVolume.VolumeShape.Cone:
			if ((double)FieldValue.heightConeMin < rangeHeightConeMin.MinValue)
			{
				FieldValue.heightConeMin = (float)rangeHeightConeMin.MinValue;
			}
			rangeHeightConeMax.MinValue = FieldValue.heightConeMin;
			FieldValue.altitude = (FieldValue.heightConeMin + FieldValue.heightConeMax) / 2f;
			rangeHeightConeMin.RefreshUI();
			break;
		}
	}

	public void RefreshMinHeightValues()
	{
		rangeHeightConeMin.MinValue = (float)FieldValue.body.TerrainAltitude(FieldValue.latitude, FieldValue.longitude, allowNegative: true);
		rangeHeightSphere.MinValue = (float)FieldValue.body.TerrainAltitude(FieldValue.latitude, FieldValue.longitude, allowNegative: true);
		rangeHeightConeMax.MinValue = FieldValue.heightConeMin;
	}

	public void OnInputRadius(float newValue)
	{
		if (newValue > gizmoRef.MaxRadius)
		{
			newValue = gizmoRef.MaxRadius;
		}
		gizmoRef.Radius = newValue;
		UpdateGizmoShape();
		UpdateNodeBodyUI();
	}

	public void OnInputHeightConeMin(float newValue)
	{
		RefreshMinHeightValues();
		if (newValue > gizmoRef.HeightConeMax)
		{
			rangeHeightConeMin.slider.value = gizmoRef.HeightConeMax;
		}
		UpdateGizmoShape();
		UpdateNodeBodyUI();
	}

	public void OnInputHeightConeMax(float newValue)
	{
		RefreshMinHeightValues();
		if (newValue < gizmoRef.HeightConeMin)
		{
			rangeHeightConeMax.slider.value = gizmoRef.HeightConeMin;
		}
		else if ((double)newValue > FieldValue.body.Radius)
		{
			rangeHeightConeMax.slider.value = (float)FieldValue.body.Radius;
		}
		UpdateGizmoShape();
		UpdateNodeBodyUI();
	}

	public void OnInputHeightSphere(float newValue)
	{
		RefreshMinHeightValues();
		UpdateGizmoShape();
		UpdateNodeBodyUI();
	}

	public void OnDropDownBody(int value)
	{
		gapCB.Load(FieldValue.body);
		float radius = FieldValue.radius;
		float heightSphere = FieldValue.heightSphere;
		float heightConeMin = FieldValue.heightConeMin;
		float heightConeMax = FieldValue.heightConeMax;
		UpdateMaxValues();
		FieldValue.AjustToBody(radius, heightSphere, heightConeMin, heightConeMax, FieldValue.body, pastBody);
		rangeRadius.slider.value = FieldValue.radius;
		rangeHeightSphere.slider.value = FieldValue.heightSphere;
		rangeHeightConeMin.slider.value = FieldValue.heightConeMin;
		rangeHeightConeMax.slider.value = FieldValue.heightConeMax;
		pastBody = FieldValue.body;
		UpdateGizmo();
	}

	public void OnDropDownShape(int value)
	{
		switch ((SurfaceVolume.VolumeShape)value)
		{
		case SurfaceVolume.VolumeShape.Sphere:
			rangeHeightConeMax.gameObject.SetActive(value: false);
			rangeHeightConeMin.gameObject.SetActive(value: false);
			rangeHeightSphere.gameObject.SetActive(value: true);
			break;
		case SurfaceVolume.VolumeShape.Cone:
			rangeHeightConeMax.gameObject.SetActive(value: true);
			rangeHeightConeMin.gameObject.SetActive(value: true);
			rangeHeightSphere.gameObject.SetActive(value: false);
			break;
		}
		UpdateGizmoShape();
	}

	public void UpdateGizmo()
	{
		if (gizmoRef != null)
		{
			if (rangeHeightConeMin.MinValue < 0.0)
			{
				gizmoRef.SetGizmoPosition(FieldValue.latitude, FieldValue.longitude, Mathf.Abs((float)rangeHeightConeMin.MinValue));
			}
			else
			{
				gizmoRef.SetGizmoPosition(FieldValue.latitude, FieldValue.longitude, 0.0);
			}
			if (!gizmoRef.IsActive())
			{
				gapCB.CelestialCamera.SetPosition(FieldValue.latitude, FieldValue.longitude);
			}
			gapCB.SurfaceGizmo.UpdateFooterText();
		}
	}

	public void UpdateGizmoShape()
	{
		if (gizmoRef != null)
		{
			gizmoRef.Shape = FieldValue.shape;
			gizmoRef.HeightSphere = FieldValue.heightSphere;
			gizmoRef.SetConeBounds(FieldValue.heightConeMin, FieldValue.heightConeMax);
			gizmoRef.Radius = FieldValue.radius;
		}
		gapCB.SurfaceGizmo.UpdateFooterText();
	}

	public void OnLeftGapClick()
	{
		int itemIndex = dropDownBodies.GetItemIndex(FieldValue.body);
		itemIndex = (itemIndex + dropDownBodies.dropdownList.options.Count - 1) % dropDownBodies.dropdownList.options.Count;
		dropDownBodies.dropdownList.value = itemIndex;
	}

	public void OnRightGapClick()
	{
		int itemIndex = dropDownBodies.GetItemIndex(FieldValue.body);
		itemIndex = (itemIndex + 1) % dropDownBodies.dropdownList.options.Count;
		dropDownBodies.dropdownList.value = itemIndex;
	}

	public void OnGAPPointTranslate(double latitude, double longitude)
	{
		rangeLatitude.slider.value = (float)latitude;
		rangeLongitude.slider.value = (float)longitude;
	}

	public void OnGAPSliderSphere(float newHeight)
	{
		rangeHeightSphere.slider.value = newHeight;
	}

	public void OnGAPSliderConeBounds(float minValue, float maxValue)
	{
		rangeHeightConeMax.slider.value = maxValue;
	}

	public void OnGAPShapeChange(SurfaceVolume.VolumeShape newShape)
	{
		dropDownShape.dropdownList.value = (int)newShape;
	}

	public override void DisplayGAP()
	{
		base.DisplayGAP();
		if (gapCB == null)
		{
			gapCB = MissionEditorLogic.Instance.actionPane.GAPInitialize<GAPCelestialBody>();
		}
		gapCB.Load(FieldValue.body);
		gapCB.SetState(GAPCelestialBodyState.POINT);
		gapCB.TogglePQS(usePQS: true);
		gapCB.SurfaceGizmo.InstantiateGizmo("GAP_CelestialBody_SurfaceGizmo_Volume");
		gapCB.SuscribeToLeftButton(OnLeftGapClick);
		gapCB.SuscribeToRightButton(OnRightGapClick);
		gapCB.Load(FieldValue.body);
		gizmoRef = gapCB.SurfaceGizmo.GizmoEntity as GAPCelestialBody_SurfaceGizmo_Volume;
		GAPCelestialBody_SurfaceGizmo_Volume gAPCelestialBody_SurfaceGizmo_Volume = gizmoRef;
		gAPCelestialBody_SurfaceGizmo_Volume.OnGAPGizmoTranslate = (GAPCelestialBody_InteractiveSurfaceGizmo.OnPointTranslate)Delegate.Combine(gAPCelestialBody_SurfaceGizmo_Volume.OnGAPGizmoTranslate, new GAPCelestialBody_InteractiveSurfaceGizmo.OnPointTranslate(OnGAPPointTranslate));
		GAPCelestialBody_SurfaceGizmo_Volume gAPCelestialBody_SurfaceGizmo_Volume2 = gizmoRef;
		gAPCelestialBody_SurfaceGizmo_Volume2.OnGAPGizmoRadiusChange = (GAPCelestialBody_SurfaceGizmo_Volume.OnGAPValueChange)Delegate.Combine(gAPCelestialBody_SurfaceGizmo_Volume2.OnGAPGizmoRadiusChange, new GAPCelestialBody_SurfaceGizmo_Volume.OnGAPValueChange(OnDragRadius));
		GAPCelestialBody_SurfaceGizmo_Volume gAPCelestialBody_SurfaceGizmo_Volume3 = gizmoRef;
		gAPCelestialBody_SurfaceGizmo_Volume3.OnGAPGizmoHeightSphere = (GAPCelestialBody_SurfaceGizmo_Volume.OnGAPValueChange)Delegate.Combine(gAPCelestialBody_SurfaceGizmo_Volume3.OnGAPGizmoHeightSphere, new GAPCelestialBody_SurfaceGizmo_Volume.OnGAPValueChange(OnGAPSliderSphere));
		GAPCelestialBody_SurfaceGizmo_Volume gAPCelestialBody_SurfaceGizmo_Volume4 = gizmoRef;
		gAPCelestialBody_SurfaceGizmo_Volume4.OnGAPGizmoShapeChange = (GAPCelestialBody_SurfaceGizmo_Volume.OnGAPShapeChange)Delegate.Combine(gAPCelestialBody_SurfaceGizmo_Volume4.OnGAPGizmoShapeChange, new GAPCelestialBody_SurfaceGizmo_Volume.OnGAPShapeChange(OnGAPShapeChange));
		GAPCelestialBody_SurfaceGizmo_Volume gAPCelestialBody_SurfaceGizmo_Volume5 = gizmoRef;
		gAPCelestialBody_SurfaceGizmo_Volume5.OnGAPGizmoConeBoundsChange = (GAPCelestialBody_SurfaceGizmo_Volume.OnGAPShapeBoundsChange)Delegate.Combine(gAPCelestialBody_SurfaceGizmo_Volume5.OnGAPGizmoConeBoundsChange, new GAPCelestialBody_SurfaceGizmo_Volume.OnGAPShapeBoundsChange(OnGAPSliderConeBounds));
		double num = FieldValue.body.TerrainAltitude(FieldValue.latitude, FieldValue.longitude, allowNegative: true);
		if (num < 0.0)
		{
			gizmoRef.SetGizmoPosition(FieldValue.latitude, FieldValue.longitude, Mathf.Abs((float)num));
		}
		else
		{
			gizmoRef.SetGizmoPosition(FieldValue.latitude, FieldValue.longitude, 0.0);
		}
		UpdateMaxValues();
		OnDropDownShape((int)FieldValue.shape);
	}

	public void UpdateMaxValues()
	{
		float num = (float)FieldValue.body.Radius * 0.6f;
		gizmoRef.MaxRadius = num;
		rangeRadius.slider.maxValue = num;
		rangeHeightConeMin.slider.maxValue = (float)FieldValue.body.Radius;
		rangeHeightConeMax.slider.maxValue = (float)FieldValue.body.Radius;
		rangeHeightSphere.slider.maxValue = (float)FieldValue.body.Radius;
		rangeRadius.slider.onValueChanged.AddListener(OnInputRadius);
		rangeHeightConeMin.slider.onValueChanged.AddListener(OnInputHeightConeMin);
		rangeHeightConeMax.slider.onValueChanged.AddListener(OnInputHeightConeMax);
		rangeHeightSphere.slider.onValueChanged.AddListener(OnInputHeightSphere);
		rangeHeightConeMin.inputFieldButton.onClick.AddListener(RefreshMinHeightValues);
		rangeHeightConeMax.inputFieldButton.onClick.AddListener(RefreshMinHeightValues);
		rangeHeightSphere.inputFieldButton.onClick.AddListener(RefreshMinHeightValues);
	}
}

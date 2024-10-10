using System;

namespace Expansions.Missions.Editor;

[MEGUI_SurfaceArea]
public class MEGUIParameterCelestialBody_Area : MEGUICompoundParameter
{
	public GAPCelestialBody gapCB;

	public GAPCelestialBody_SurfaceGizmo_Area gizmoRef;

	public MEGUIParameterDropdownList dropDownBodies;

	public MEGUIParameterNumberRange rangeLatitude;

	public MEGUIParameterNumberRange rangeLongitude;

	public MEGUIParameterNumberRange rangeRadius;

	public CelestialBody pastBody;

	public SurfaceArea FieldValue
	{
		get
		{
			return (SurfaceArea)field.GetValue();
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
		dropDownBodies.dropdownList.onValueChanged.AddListener(OnDropDownBody);
		rangeLatitude = subParameters["latitude"] as MEGUIParameterNumberRange;
		rangeLatitude.slider.onValueChanged.AddListener(OnInputLatitude);
		rangeLongitude = subParameters["longitude"] as MEGUIParameterNumberRange;
		rangeLongitude.slider.onValueChanged.AddListener(OnInputLongitude);
		rangeRadius = subParameters["radius"] as MEGUIParameterNumberRange;
		rangeRadius.slider.onValueChanged.AddListener(OnInputRadius);
		pastBody = FieldValue.body;
	}

	public void OnDragRadius(float newRadius)
	{
		rangeRadius.slider.value = newRadius;
	}

	public void OnInputLatitude(float newValue)
	{
		FieldValue.altitude = FieldValue.body.TerrainAltitude(FieldValue.latitude, FieldValue.longitude, allowNegative: true);
		UpdateGizmo();
		UpdateNodeBodyUI();
	}

	public void OnInputLongitude(float newValue)
	{
		FieldValue.altitude = FieldValue.body.TerrainAltitude(FieldValue.latitude, FieldValue.longitude, allowNegative: true);
		UpdateGizmo();
		UpdateNodeBodyUI();
	}

	public void OnInputRadius(float newValue)
	{
		if (newValue > gizmoRef.MaxRadius)
		{
			newValue = gizmoRef.MaxRadius;
			rangeRadius.slider.value = gizmoRef.MaxRadius;
		}
		gizmoRef.Radius = newValue;
		UpdateGizmo();
		UpdateNodeBodyUI();
	}

	public void OnDropDownBody(int value)
	{
		float radius = FieldValue.radius;
		gapCB.Load(FieldValue.body);
		UpdateMaxRadiusValues();
		double num = (double)radius / pastBody.Radius;
		FieldValue.radius = (float)(FieldValue.body.Radius * num);
		rangeRadius.slider.value = FieldValue.radius;
		pastBody = FieldValue.body;
		UpdateGizmo();
	}

	public void UpdateGizmo()
	{
		if (gizmoRef != null)
		{
			gizmoRef.SetGizmoPosition(FieldValue.latitude, FieldValue.longitude, 0.0);
			gizmoRef.Radius = FieldValue.radius;
			if (!gizmoRef.IsActive())
			{
				gapCB.CelestialCamera.SetPosition(FieldValue.latitude, FieldValue.longitude);
			}
			gapCB.SurfaceGizmo.UpdateFooterText();
		}
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
		gapCB.SurfaceGizmo.InstantiateGizmo("GAP_CelestialBody_SurfaceGizmo_Area");
		gapCB.SuscribeToLeftButton(OnLeftGapClick);
		gapCB.SuscribeToRightButton(OnRightGapClick);
		gapCB.Load(FieldValue.body);
		gizmoRef = gapCB.SurfaceGizmo.GizmoEntity as GAPCelestialBody_SurfaceGizmo_Area;
		GAPCelestialBody_SurfaceGizmo_Area gAPCelestialBody_SurfaceGizmo_Area = gizmoRef;
		gAPCelestialBody_SurfaceGizmo_Area.OnGAPGizmoTranslate = (GAPCelestialBody_InteractiveSurfaceGizmo.OnPointTranslate)Delegate.Combine(gAPCelestialBody_SurfaceGizmo_Area.OnGAPGizmoTranslate, new GAPCelestialBody_InteractiveSurfaceGizmo.OnPointTranslate(OnGAPPointTranslate));
		GAPCelestialBody_SurfaceGizmo_Area gAPCelestialBody_SurfaceGizmo_Area2 = gizmoRef;
		gAPCelestialBody_SurfaceGizmo_Area2.OnGAPGizmoRadiusChange = (GAPCelestialBody_SurfaceGizmo_Area.OnRadiusChange)Delegate.Combine(gAPCelestialBody_SurfaceGizmo_Area2.OnGAPGizmoRadiusChange, new GAPCelestialBody_SurfaceGizmo_Area.OnRadiusChange(OnDragRadius));
		gizmoRef.SetGizmoPosition(FieldValue.latitude, FieldValue.longitude, 0.0);
		UpdateMaxRadiusValues();
		gizmoRef.Radius = FieldValue.radius;
		gapCB.CelestialCamera.OverridePosition(FieldValue.latitude, FieldValue.longitude);
		UpdateGizmo();
	}

	public void UpdateMaxRadiusValues()
	{
		float num = (float)FieldValue.body.Radius * 0.7f;
		gizmoRef.MaxRadius = num;
		rangeRadius.slider.maxValue = num;
	}
}

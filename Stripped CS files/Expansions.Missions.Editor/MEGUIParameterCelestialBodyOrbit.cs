using System;
using System.Collections.Generic;
using UnityEngine;

namespace Expansions.Missions.Editor;

[MEGUI_CelestialBody_Orbit]
public class MEGUIParameterCelestialBodyOrbit : MEGUICompoundParameter
{
	public MEGUIParameterDropdownList dropDownBodies;

	public MEGUIParameterNumberRange rangeMeanAnomaly;

	public MEGUIParameterNumberRange rangeSemiMajorAxis;

	public CelestialBody pastBody;

	public GAPCelestialBody gapCelestialBody;

	public bool isDirty;

	public MissionOrbit FieldValue
	{
		get
		{
			return field.GetValue<MissionOrbit>();
		}
		set
		{
			MissionEditorHistory.PushUndoAction(this, OnHistoryValueChange);
			field.SetValue(value);
		}
	}

	public override void Setup(string name)
	{
		base.Setup(name);
		title.text = name;
		dropDownBodies = subParameters["Body"] as MEGUIParameterDropdownList;
		dropDownBodies.dropdownList.onValueChanged.AddListener(OnDropDownBody);
		Dictionary<string, MEGUIParameter>.KeyCollection.Enumerator enumerator = subParameters.Keys.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				MEGUIParameter mEGUIParameter = subParameters[enumerator.Current];
				if (mEGUIParameter.GetType() == typeof(MEGUIParameterInputField))
				{
					((MEGUIParameterInputField)mEGUIParameter).inputField.onEndEdit.AddListener(OnUpdateValues);
					((MEGUIParameterInputField)mEGUIParameter).inputField.onValueChanged.AddListener(OnSubParameterValueChange);
					mEGUIParameter.resetButton.onClick.AddListener(OnSubParameterResetValue);
				}
				else if (mEGUIParameter.GetType() == typeof(MEGUIParameterNumberRange))
				{
					((MEGUIParameterNumberRange)mEGUIParameter).slider.onValueChanged.AddListener(OnUpdateSliderValues);
				}
				mEGUIParameter.gameObject.SetActive(value: true);
			}
		}
		finally
		{
			enumerator.Dispose();
		}
		rangeMeanAnomaly = subParameters["MeanAnomalyAtEpoch"] as MEGUIParameterNumberRange;
		rangeSemiMajorAxis = subParameters["SemiMajorAxis"] as MEGUIParameterNumberRange;
		rangeMeanAnomaly.gameObject.SetActive(((MEGUI_CelestialBody_Orbit)field.Attribute).displayMeanAnomaly);
		UpdateSemiMajorAxisLimits(FieldValue.Body);
		pastBody = FieldValue.Body;
	}

	public override void DisplayGAP()
	{
		base.DisplayGAP();
		gapCelestialBody = MissionEditorLogic.Instance.actionPane.GAPInitialize<GAPCelestialBody>();
		gapCelestialBody.BodyOrbit = FieldValue.Orbit;
		gapCelestialBody.Load(FieldValue.Body);
		gapCelestialBody.SetState(GAPCelestialBodyState.ORBIT);
		gapCelestialBody.Load(FieldValue.Body);
		gapCelestialBody.SuscribeToLeftButton(OnGapLeftArrow);
		gapCelestialBody.SuscribeToRightButton(OnGapRightArrow);
		GAPCelestialBodyState_Orbit orbits = gapCelestialBody.Orbits;
		orbits.OnOrbitReset = (Callback)Delegate.Combine(orbits.OnOrbitReset, new Callback(OnOrbitReset));
		GAPCelestialBodyState_Orbit orbits2 = gapCelestialBody.Orbits;
		orbits2.OnPointGizmoUpdate = (OrbitGizmo.HandlesUpdatedCallback)Delegate.Combine(orbits2.OnPointGizmoUpdate, new OrbitGizmo.HandlesUpdatedCallback(OnPointGizmoUpdate));
		GAPCelestialBodyState_Orbit orbits3 = gapCelestialBody.Orbits;
		orbits3.OnGlobalGizmoUpdate = (OrbitGizmo.HandlesUpdatedCallback)Delegate.Combine(orbits3.OnGlobalGizmoUpdate, new OrbitGizmo.HandlesUpdatedCallback(OnGlobalGizmoUpdate));
		gapCelestialBody.Orbits.ToggleMeanAnomalyIcon(((MEGUI_CelestialBody_Orbit)field.Attribute).displayMeanAnomaly);
	}

	public override void RefreshUI()
	{
		Dictionary<string, MEGUIParameter>.KeyCollection.Enumerator enumerator = subParameters.Keys.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				subParameters[enumerator.Current].RefreshUI();
			}
		}
		finally
		{
			enumerator.Dispose();
		}
		if (gapCelestialBody != null)
		{
			gapCelestialBody.Orbits.UpdateOrbit();
		}
	}

	public void OnDropDownBody(int value)
	{
		OnUpdateValues(null);
		if (gapCelestialBody != null)
		{
			double num = 1.0 + (FieldValue.semiMajorAxis - pastBody.Radius) / pastBody.Radius;
			FieldValue.SemiMajorAxis = FieldValue.Body.Radius * num;
			gapCelestialBody.Load(FieldValue.Body);
			pastBody = FieldValue.Body;
			UpdateSemiMajorAxisLimits(FieldValue.Body);
			RefreshUI();
		}
		UpdateNodeBodyUI();
	}

	public void OnUpdateValues(string value)
	{
		if (isDirty)
		{
			isDirty = false;
			RefreshUI();
			UpdateNodeBodyUI();
		}
	}

	public void OnUpdateSliderValues(float value)
	{
		RefreshUI();
		UpdateNodeBodyUI();
	}

	public void OnSubParameterValueChange(string value)
	{
		isDirty = true;
	}

	public void OnSubParameterResetValue()
	{
		RefreshUI();
		UpdateNodeBodyUI();
	}

	public void OnGapLeftArrow()
	{
		int itemIndex = dropDownBodies.GetItemIndex(FieldValue.Body);
		itemIndex = (itemIndex + dropDownBodies.dropdownList.options.Count - 1) % dropDownBodies.dropdownList.options.Count;
		dropDownBodies.dropdownList.value = itemIndex;
	}

	public void OnGapRightArrow()
	{
		int itemIndex = dropDownBodies.GetItemIndex(FieldValue.Body);
		itemIndex = (itemIndex + 1) % dropDownBodies.dropdownList.options.Count;
		dropDownBodies.dropdownList.value = itemIndex;
	}

	public void OnOrbitReset()
	{
		MissionEditorHistory.PushUndoAction(this, OnHistoryValueChange);
		FieldValue.Reset();
		RefreshUI();
	}

	public void OnPointGizmoUpdate(Vector3d delta, double double_0)
	{
		Vector3d xzy = FieldValue.Orbit.getRelativePositionAtUT(double_0).xzy;
		Vector3d xzy2 = FieldValue.Orbit.getOrbitalVelocityAtUT(double_0).xzy;
		Vector3d vector3d = (QuaternionD)Quaternion.LookRotation(xzy2, Vector3d.Cross(-xzy, xzy2)) * delta;
		Orbit orbit = new Orbit(FieldValue.Orbit);
		orbit.UpdateFromStateVectors(xzy.xzy, xzy2.xzy + vector3d.xzy, FieldValue.Body, double_0);
		if (orbit.ApA <= FieldValue.Body.sphereOfInfluence && orbit.ApA > 0.0)
		{
			FieldValue.Orbit.UpdateFromStateVectors(xzy.xzy, xzy2.xzy + vector3d.xzy, FieldValue.Body, double_0);
			FieldValue.Eccentricity = FieldValue.Orbit.eccentricity;
			FieldValue.SemiMajorAxis = FieldValue.Orbit.semiMajorAxis;
			FieldValue.Inclination = FieldValue.Orbit.inclination;
			FieldValue.Lan = FieldValue.Orbit.double_0;
			FieldValue.ArgumentOfPeriapsis = FieldValue.Orbit.argumentOfPeriapsis;
			RefreshUI();
		}
	}

	public void OnGlobalGizmoUpdate(Vector3d delta, double double_0)
	{
		double num = (1.0 + (FieldValue.Eccentricity + delta.x)) * (FieldValue.SemiMajorAxis + delta.z) - FieldValue.Body.Radius;
		if (num <= FieldValue.Body.sphereOfInfluence && num > 0.0)
		{
			FieldValue.Eccentricity = UtilMath.Clamp(FieldValue.eccentricity + delta.x, 1E-05, 0.9999);
			FieldValue.SemiMajorAxis += delta.z;
			FieldValue.Inclination += delta.y;
			RefreshUI();
		}
	}

	public void UpdateSemiMajorAxisLimits(CelestialBody body)
	{
		rangeSemiMajorAxis.slider.onValueChanged.RemoveListener(OnUpdateSliderValues);
		double semiMajorAxis = FieldValue.SemiMajorAxis;
		rangeSemiMajorAxis.MinValue = (float)body.Radius;
		rangeSemiMajorAxis.MaxValue = (float)MissionOrbit.GetEditorMaxOrbitRadius(body);
		rangeSemiMajorAxis.FieldValue = semiMajorAxis;
		rangeSemiMajorAxis.slider.onValueChanged.AddListener(OnUpdateSliderValues);
	}

	public void OnHistoryValueChange(ConfigNode data, HistoryType type)
	{
		ConfigNode node = new ConfigNode();
		if (!data.TryGetNode("ORBIT", ref node))
		{
			return;
		}
		int referenceBodyIndex = FieldValue.ReferenceBodyIndex;
		FieldValue.Load(node);
		if (gapCelestialBody != null)
		{
			if (referenceBodyIndex != FieldValue.ReferenceBodyIndex)
			{
				gapCelestialBody.Load(FieldValue.Body);
			}
			else
			{
				gapCelestialBody.Orbits.UpdateOrbit();
			}
		}
	}

	public override ConfigNode GetState()
	{
		ConfigNode configNode = new ConfigNode();
		configNode.AddValue("pinned", isPinned);
		FieldValue.Save(configNode.AddNode("ORBIT"));
		return configNode;
	}
}

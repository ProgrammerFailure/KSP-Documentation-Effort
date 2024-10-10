using System;
using Expansions.Missions.Editor;
using FinePrint.Utilities;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time),
	typeof(ScoreModule_Accuracy)
})]
public class TestOrbitParams : TestVessel, IScoreableObjective
{
	[MEGUI_Checkbox(group = "Apoapsis", onValueChange = "onCheckApoapsisValueChanged", order = 10, groupDisplayName = "#autoLOC_8100059", onControlCreated = "onCheckApoapsisCreated", guiName = "#autoLOC_8002064", Tooltip = "#autoLOC_8002065")]
	public bool checkApoapsis;

	[MEGUI_NumberRange(onControlSetupComplete = "onApoapsisMinCreated", groupDisplayName = "#autoLOC_8200059", canBePinned = false, group = "Apoapsis", resetValue = "70000", onValueChange = "onApMinValueChanged", order = 11, hideOnSetup = true, guiName = "#autoLOC_8002068", Tooltip = "#autoLOC_8002069")]
	public double apMinValue;

	[MEGUI_NumberRange(onControlSetupComplete = "onApoapsisMaxCreated", groupDisplayName = "#autoLOC_8200059", canBePinned = false, group = "Apoapsis", resetValue = "100000", onValueChange = "onApMaxValueChanged", order = 12, hideOnSetup = true, guiName = "#autoLOC_8002070", Tooltip = "#autoLOC_8002071")]
	public double apMaxValue;

	[MEGUI_Checkbox(group = "Periapsis", onValueChange = "onCheckPeriapsisValueChanged", order = 13, groupDisplayName = "#autoLOC_8100060", guiName = "#autoLOC_8002066", Tooltip = "#autoLOC_8002067")]
	public bool checkPeriapsis;

	[MEGUI_NumberRange(onControlSetupComplete = "onPeriapsisMinCreated", groupDisplayName = "#autoLOC_8100060", canBePinned = false, group = "Periapsis", resetValue = "70000", onValueChange = "onPeMinValueChanged", order = 14, hideOnSetup = true, guiName = "#autoLOC_8002072", Tooltip = "#autoLOC_8002073")]
	public double peMinValue;

	[MEGUI_NumberRange(onControlSetupComplete = "onPeriapsisMaxCreated", groupDisplayName = "#autoLOC_8100060", canBePinned = false, group = "Periapsis", resetValue = "100000", onValueChange = "onPeMaxValueChanged", order = 15, hideOnSetup = true, guiName = "#autoLOC_8002074", Tooltip = "#autoLOC_8002075")]
	public double peMaxValue;

	[MEGUI_Checkbox(group = "Inclination", onValueChange = "onCheckIncValueChanged", order = 16, groupDisplayName = "#autoLOC_8100062", guiName = "#autoLOC_8002076", Tooltip = "#autoLOC_8002077")]
	public bool checkInclination;

	[MEGUI_NumberRange(resetValue = "0", onControlSetupComplete = "onInclinationCreated", clampTextInput = true, maxValue = 180f, groupDisplayName = "#autoLOC_8100062", canBePinned = false, displayUnits = "°", minValue = 0f, group = "Inclination", displayFormat = "0.##", onValueChange = "onIncValueChanged", order = 17, hideOnSetup = true, roundToPlaces = 0, guiName = "#autoLOC_8100062", Tooltip = "#autoLOC_8002078")]
	public double inclination;

	[MEGUI_NumberRange(resetValue = "90", onControlCreated = "onInclinationAccuracyCreated", clampTextInput = true, maxValue = 100f, groupDisplayName = "#autoLOC_8100062", canBePinned = false, displayUnits = "%", minValue = 0f, group = "Inclination", displayFormat = "0.##", onValueChange = "OnInclinationAccuracyChanged", order = 18, hideOnSetup = true, roundToPlaces = 0, guiName = "#autoLOC_8002079", Tooltip = "#autoLOC_8002080")]
	public double inclinationAccuracy;

	[MEGUI_Checkbox(group = "LAN", onValueChange = "onCheckLANValueChanged", order = 19, groupDisplayName = "#autoLOC_900371", guiName = "#autoLOC_8002081", Tooltip = "#autoLOC_8002082")]
	public bool checkLAN;

	[MEGUI_NumberRange(resetValue = "90", onControlSetupComplete = "onLANCreated", clampTextInput = true, maxValue = 360f, groupDisplayName = "#autoLOC_900371", canBePinned = false, displayUnits = "°", minValue = 0f, group = "LAN", displayFormat = "0.##", onValueChange = "onLANValueChanged", order = 20, hideOnSetup = true, roundToPlaces = 0, guiName = "#autoLOC_900371", Tooltip = "#autoLOC_8002083")]
	public double double_0;

	[MEGUI_NumberRange(resetValue = "90", onControlCreated = "onLANAccuracyCreated", clampTextInput = true, maxValue = 100f, groupDisplayName = "#autoLOC_900371", canBePinned = false, displayUnits = "%", minValue = 0f, group = "LAN", displayFormat = "0.##", onValueChange = "OnLANAccuracyChanged", order = 21, hideOnSetup = true, roundToPlaces = 0, guiName = "#autoLOC_8002084", Tooltip = "#autoLOC_8002085")]
	public double LANAccuracy;

	[MEGUI_CelestialBody(gapDisplay = false, order = 22, onControlSetupComplete = "onBodyCreated", canBePinned = true, resetValue = "0", showAnySOIoption = true, guiName = "#autoLOC_8000263", Tooltip = "#autoLOC_8000157")]
	public MissionCelestialBody body;

	[MEGUI_Time(order = 23, canBePinned = true, resetValue = "5", guiName = "#autoLOC_8003019", Tooltip = "#autoLOC_8003061")]
	public double stabilizationTime = 5.0;

	[MEGUI_Checkbox(order = 24, canBePinned = true, resetValue = "False", guiName = "#autoLOC_8003062", Tooltip = "#autoLOC_8003063")]
	public bool underThrust;

	public bool situationSuccess;

	public bool orbitSuccess;

	public double successStartTime = -1.0;

	public MEGUIParameterNumberRange apMinInstance;

	public MEGUIParameterNumberRange apMaxInstance;

	public MEGUIParameterNumberRange peMinInstance;

	public MEGUIParameterNumberRange peMaxInstance;

	public MEGUIParameterNumberRange incInstance;

	public MEGUIParameterNumberRange inclinationAccuracyInstance;

	public MEGUIParameterNumberRange lanInstance;

	public MEGUIParameterNumberRange lanAccuracyInstance;

	public MEGUIParameterCelestialBody bodyInstance;

	public MEGUIParameterDropdownList bodyDropdownInstance;

	public double InclinationDeviation => Mathf.Clamp(100f - (float)inclinationAccuracy, 0f, 100f);

	public double LANDeviation => Mathf.Clamp(100f - (float)LANAccuracy, 0f, 100f);

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8003152");
		body = new MissionCelestialBody();
		apMinValue = 75000.0;
		apMaxValue = 80000.0;
		peMinValue = 70000.0;
		peMaxValue = 75000.0;
		inclination = 0.0;
		inclinationAccuracy = 90.0;
		double_0 = 0.0;
		LANAccuracy = 90.0;
		useActiveVessel = true;
	}

	public override void OnDestroy()
	{
		if (bodyDropdownInstance != null)
		{
			bodyInstance.dropDownBodies.dropdownList.onValueChanged.RemoveListener(OnBodyChanged);
		}
		base.OnDestroy();
	}

	public override void Initialized()
	{
		base.Initialized();
	}

	public override void Cleared()
	{
		base.Cleared();
	}

	public void onApoapsisMinCreated(MEGUIParameterNumberRange parameter)
	{
		apMinInstance = parameter;
	}

	public void onApoapsisMaxCreated(MEGUIParameterNumberRange parameter)
	{
		apMaxInstance = parameter;
	}

	public void onPeriapsisMinCreated(MEGUIParameterNumberRange parameter)
	{
		peMinInstance = parameter;
	}

	public void onPeriapsisMaxCreated(MEGUIParameterNumberRange parameter)
	{
		peMaxInstance = parameter;
	}

	public void onInclinationCreated(MEGUIParameterNumberRange parameter)
	{
		incInstance = parameter;
	}

	public void onLANCreated(MEGUIParameterNumberRange parameter)
	{
		lanInstance = parameter;
	}

	public void onInclinationAccuracyCreated(MEGUIParameterNumberRange parameter)
	{
		inclinationAccuracyInstance = parameter;
	}

	public void onLANAccuracyCreated(MEGUIParameterNumberRange parameter)
	{
		lanAccuracyInstance = parameter;
	}

	public void onBodyCreated(MEGUIParameterCelestialBody parameter)
	{
		bodyInstance = parameter;
		bodyDropdownInstance = bodyInstance.dropDownBodies;
		if (bodyDropdownInstance != null)
		{
			bodyInstance.dropDownBodies.dropdownList.onValueChanged.AddListener(OnBodyChanged);
		}
	}

	public void onCheckApoapsisValueChanged(bool newValue)
	{
		toggleApoapsisParms(newValue);
	}

	public void onCheckPeriapsisValueChanged(bool newValue)
	{
		togglePeriapsisParms(newValue);
	}

	public void onCheckIncValueChanged(bool newValue)
	{
		toggleIncParms(newValue);
	}

	public void onCheckLANValueChanged(bool newValue)
	{
		togglelLANParms(newValue);
	}

	public void onApMinValueChanged(double newValue)
	{
		if (apMinValue > apMaxValue && apMaxInstance != null)
		{
			apMaxInstance.FieldValue = apMinValue;
			apMaxInstance.slider.value = (float)apMinValue;
		}
		UpdateNodeBodyUI();
	}

	public void onApMaxValueChanged(double newValue)
	{
		if (apMinValue > apMaxValue && apMinInstance != null)
		{
			apMinInstance.FieldValue = apMaxValue;
			apMinInstance.slider.value = (float)apMaxValue;
		}
		UpdateNodeBodyUI();
	}

	public void onPeMinValueChanged(double newValue)
	{
		if (peMinValue > peMaxValue && peMaxInstance != null)
		{
			peMaxInstance.FieldValue = peMinValue;
			peMaxInstance.slider.value = (float)peMinValue;
		}
		UpdateNodeBodyUI();
	}

	public void onPeMaxValueChanged(double newValue)
	{
		if (peMinValue > peMaxValue && peMinInstance != null)
		{
			peMinInstance.FieldValue = peMaxValue;
			peMinInstance.slider.value = (float)peMaxValue;
		}
		UpdateNodeBodyUI();
	}

	public void onIncValueChanged(double newValue)
	{
		UpdateNodeBodyUI();
	}

	public void onLANValueChanged(double newValue)
	{
		UpdateNodeBodyUI();
	}

	public void OnInclinationAccuracyChanged(double newValue)
	{
		inclinationAccuracy = Mathf.Clamp((float)newValue, 0f, 100f);
		UpdateNodeBodyUI();
	}

	public void OnLANAccuracyChanged(double newValue)
	{
		LANAccuracy = Mathf.Clamp((float)newValue, 0f, 100f);
		UpdateNodeBodyUI();
	}

	public void OnBodyChanged(int newValue)
	{
		CelestialBody celestialBody = bodyDropdownInstance.SelectedValue as CelestialBody;
		if (celestialBody == null)
		{
			celestialBody = FlightGlobals.GetHomeBody();
		}
		if (celestialBody != null)
		{
			if (apMinInstance != null)
			{
				apMinInstance.slider.minValue = 0f;
				apMinInstance.slider.maxValue = (float)GetMaxAp(celestialBody);
			}
			if (apMaxInstance != null)
			{
				apMaxInstance.slider.minValue = 0f;
				apMaxInstance.slider.maxValue = (float)GetMaxAp(celestialBody);
			}
			if (peMinInstance != null)
			{
				peMinInstance.slider.minValue = 0f;
				peMinInstance.slider.maxValue = (float)GetMaxAp(celestialBody);
			}
			if (peMaxInstance != null)
			{
				peMaxInstance.slider.minValue = 0f;
				peMaxInstance.slider.maxValue = (float)GetMaxAp(celestialBody);
			}
		}
		else
		{
			if (apMinInstance != null)
			{
				apMinInstance.slider.maxValue = 1500000f;
			}
			if (apMaxInstance != null)
			{
				apMaxInstance.slider.maxValue = 1500000f;
			}
			if (peMinInstance != null)
			{
				peMinInstance.slider.maxValue = 1500000f;
			}
			if (peMaxInstance != null)
			{
				peMaxInstance.slider.maxValue = 1500000f;
			}
		}
	}

	public double GetMaxAp(CelestialBody body)
	{
		if (double.IsInfinity(body.sphereOfInfluence))
		{
			double num = 0.0;
			int count = body.orbitingBodies.Count;
			for (int i = 0; i < count; i++)
			{
				if (body.orbitingBodies[i].orbit.radius > num)
				{
					num = body.orbitingBodies[i].orbit.radius;
				}
			}
			if (num == 0.0)
			{
				Debug.LogWarning("Unable to find a maxorbit radius - using body radius times 10");
				return body.Radius * 10.0;
			}
			return (num - body.Radius) * 1.2;
		}
		return body.sphereOfInfluence - body.Radius - 1.0;
	}

	public void toggleApoapsisParms(bool on)
	{
		if (apMinInstance != null)
		{
			apMinInstance.gameObject.SetActive(on);
		}
		if (apMaxInstance != null)
		{
			apMaxInstance.gameObject.SetActive(on);
		}
	}

	public void togglePeriapsisParms(bool on)
	{
		if (peMinInstance != null)
		{
			peMinInstance.gameObject.SetActive(on);
		}
		if (peMaxInstance != null)
		{
			peMaxInstance.gameObject.SetActive(on);
		}
	}

	public void toggleIncParms(bool on)
	{
		if (incInstance != null)
		{
			incInstance.gameObject.SetActive(on);
		}
		if (inclinationAccuracyInstance != null)
		{
			inclinationAccuracyInstance.gameObject.SetActive(on);
		}
	}

	public void togglelLANParms(bool on)
	{
		if (lanInstance != null)
		{
			lanInstance.gameObject.SetActive(on);
		}
		if (lanAccuracyInstance != null)
		{
			lanAccuracyInstance.gameObject.SetActive(on);
		}
	}

	public override void ParameterSetupComplete()
	{
		OnBodyChanged(0);
		toggleApoapsisParms(checkApoapsis);
		togglePeriapsisParms(checkPeriapsis);
		toggleIncParms(checkInclination);
		togglelLANParms(checkLAN);
	}

	public override bool Test()
	{
		base.Test();
		situationSuccess = vessel != null;
		if (vessel != null && body != null && body.Body != null)
		{
			situationSuccess = situationSuccess && body.Body.bodyName == vessel.mainBody.bodyName;
		}
		if (!situationSuccess)
		{
			successStartTime = -1.0;
			return false;
		}
		if (!underThrust && vessel.geeForce > 0.10000000149011612)
		{
			successStartTime = -1.0;
			return false;
		}
		Orbit orbit = (vessel.loaded ? vessel.orbit : vessel.protoVessel.orbitSnapShot.Load());
		Vessel.Situations situations = (vessel.loaded ? vessel.situation : vessel.protoVessel.situation);
		if (situations != Vessel.Situations.ORBITING && situations != Vessel.Situations.ESCAPING && situations != Vessel.Situations.SUB_ORBITAL)
		{
			return false;
		}
		orbitSuccess = true;
		if (orbit != null)
		{
			if (checkApoapsis)
			{
				orbitSuccess = orbitSuccess && orbit.ApA >= apMinValue && orbit.ApA <= apMaxValue;
			}
			if (checkPeriapsis)
			{
				orbitSuccess = orbitSuccess && orbit.PeA >= peMinValue && orbit.PeA <= peMaxValue;
			}
			if (checkInclination)
			{
				orbitSuccess = orbitSuccess && SystemUtilities.WithinDeviationByValue(Math.Abs(orbit.inclination), Math.Abs(inclination), InclinationDeviation, 90.0);
			}
			if (checkLAN && orbitSuccess && Math.Abs(orbit.inclination) % 180.0 > 1.0)
			{
				bool flag = false;
				bool flag2 = false;
				double num = orbit.double_0;
				double num2 = double_0;
				if (orbit.inclination < 0.0)
				{
					num = (num + 180.0) % 360.0;
				}
				if (checkInclination && inclination < 0.0)
				{
					num2 = (num2 + 180.0) % 360.0;
				}
				float num3 = (float)Math.Abs(num - num2) % 360f;
				if (num3 > 180f)
				{
					num3 = 360f - num3;
				}
				if ((double)(num3 / 180f) < LANDeviation / 100.0)
				{
					flag = true;
				}
				if (checkInclination)
				{
					float num4 = (float)(inclination - InclinationDeviation / 100.0 * 90.0);
					if (num4 < 0f)
					{
						num4 = Mathf.Abs(num4);
						if (Math.Abs(orbit.inclination) < (double)num4)
						{
							num3 = 180f - num3;
							if ((double)(num3 / 180f) < LANDeviation / 100.0)
							{
								flag2 = true;
							}
						}
					}
				}
				orbitSuccess = flag || flag2;
			}
		}
		if (!orbitSuccess)
		{
			successStartTime = -1.0;
			return false;
		}
		if (successStartTime < 0.0)
		{
			successStartTime = Planetarium.GetUniversalTime();
		}
		return successStartTime + stabilizationTime < Planetarium.GetUniversalTime();
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "checkApoapsis")
		{
			if (checkApoapsis)
			{
				return Localizer.Format("#autoLOC_8002086", apMinValue.ToString("N0"), apMaxValue.ToString("N0"));
			}
			return string.Empty;
		}
		if (field.name == "checkPeriapsis")
		{
			if (checkPeriapsis)
			{
				return Localizer.Format("#autoLOC_8002087", peMinValue.ToString("N0"), peMaxValue.ToString("N0"));
			}
			return string.Empty;
		}
		if (field.name == "checkInclination")
		{
			if (checkInclination)
			{
				return Localizer.Format("#autoLOC_8002088", inclinationAccuracy.ToString("0.##"), inclination.ToString("0.##"));
			}
			return string.Empty;
		}
		if (field.name == "checkLAN")
		{
			if (checkLAN)
			{
				return Localizer.Format("#autoLOC_8002089", LANAccuracy.ToString("0.##"), double_0.ToString("0.##"));
			}
			return string.Empty;
		}
		if (field.name == "body")
		{
			string text = ((body == null || body.Body == null) ? Localizer.Format("#autoLOC_8000273") : body.Body.displayName.LocalizeRemoveGender());
			return Localizer.Format("#autoLOC_8100316", field.guiName, text);
		}
		if (!(field.name == "apMinValue") && !(field.name == "apMaxValue") && !(field.name == "peMinValue") && !(field.name == "peMaxValue") && !(field.name == "inclination") && !(field.name == "LAN") && !(field.name == "LANAccuracy") && !(field.name == "InclinationAccuracy"))
		{
			return base.GetNodeBodyParameterString(field);
		}
		return string.Empty;
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8003153");
	}

	public override void RunValidation(MissionEditorValidator validator)
	{
		if (checkApoapsis && apMinValue > apMaxValue)
		{
			validator.AddNodeWarn(base.node, Localizer.Format("#autoLOC_8002090"));
		}
		if (checkPeriapsis && peMinValue > peMaxValue)
		{
			validator.AddNodeWarn(base.node, Localizer.Format("#autoLOC_8002091"));
		}
		if (!checkApoapsis && !checkPeriapsis && !checkInclination && !checkLAN)
		{
			validator.AddNodeFail(base.node, Localizer.Format("#autoLOC_8002092"));
		}
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("checkApoapsis", checkApoapsis);
		node.AddValue("checkPeriapsis", checkPeriapsis);
		node.AddValue("checkInclination", checkInclination);
		node.AddValue("checkLAN", checkLAN);
		node.AddValue("apMaxValue", apMaxValue);
		node.AddValue("apMinValue", apMinValue);
		node.AddValue("peMaxValue", peMaxValue);
		node.AddValue("peMinValue", peMinValue);
		node.AddValue("inclination", inclination);
		node.AddValue("inclinationAccuracy", inclinationAccuracy);
		node.AddValue("LAN", double_0);
		node.AddValue("LANAccuracy", LANAccuracy);
		node.AddValue("stabilizationTime", stabilizationTime);
		node.AddValue("underThrust", underThrust);
		body.Save(node);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("checkApoapsis", ref checkApoapsis);
		node.TryGetValue("checkPeriapsis", ref checkPeriapsis);
		node.TryGetValue("checkInclination", ref checkInclination);
		node.TryGetValue("checkLAN", ref checkLAN);
		node.TryGetValue("apMaxValue", ref apMaxValue);
		node.TryGetValue("apMinValue", ref apMinValue);
		node.TryGetValue("peMaxValue", ref peMaxValue);
		node.TryGetValue("peMinValue", ref peMinValue);
		node.TryGetValue("inclination", ref inclination);
		node.TryGetValue("inclinationAccuracy", ref inclinationAccuracy);
		node.TryGetValue("LAN", ref double_0);
		node.TryGetValue("LANAccuracy", ref LANAccuracy);
		node.TryGetValue("stabilizationTime", ref stabilizationTime);
		node.TryGetValue("underThrust", ref underThrust);
		body.Load(node);
	}

	public object GetScoreModifier(Type scoreModule)
	{
		if (scoreModule == typeof(ScoreModule_Resource))
		{
			if (vesselID == 0)
			{
				return FlightGlobals.ActiveVessel;
			}
			uint num = base.node.mission.CurrentVesselID(base.node, vesselID);
			if (FlightGlobals.PersistentVesselIds.ContainsKey(num))
			{
				return FlightGlobals.PersistentVesselIds[num];
			}
			Debug.LogErrorFormat("[TestOrbitParams] Unable to find VesselID ({0}) for Score Modifier.", num);
			return null;
		}
		if (scoreModule == typeof(ScoreModule_Accuracy))
		{
			int num2 = 0;
			double num3 = 0.0;
			Orbit orbit = (vessel.loaded ? vessel.orbit : vessel.protoVessel.orbitSnapShot.Load());
			if (orbit != null)
			{
				if (checkApoapsis)
				{
					num2++;
					num3 += (double)Mathf.Abs((float)orbit.ApA / (((float)apMinValue + (float)apMaxValue) / 2f) - 1f);
				}
				if (checkPeriapsis)
				{
					num2++;
					num3 += (double)Mathf.Abs((float)orbit.PeA / (((float)peMinValue + (float)peMaxValue) / 2f) - 1f);
				}
				if (checkInclination)
				{
					num2++;
					num3 += (double)Mathf.Clamp01((float)SystemUtilities.MeasureDeviationByValue(orbit.inclination, inclination, 90.0));
				}
				if (checkLAN)
				{
					num2++;
					if (Math.Abs(orbit.inclination) % 180.0 > 1.0)
					{
						double num4 = orbit.double_0;
						double num5 = double_0;
						if (orbit.inclination < 0.0)
						{
							num4 = (num4 + 180.0) % 360.0;
						}
						if (checkInclination && inclination < 0.0)
						{
							num5 = (num5 + 180.0) % 360.0;
						}
						float num6 = (float)Math.Abs(num4 - num5) % 360f;
						if (num6 > 180f)
						{
							num6 = 360f - num6;
						}
						if ((double)(num6 / 180f) < LANDeviation / 100.0)
						{
							num3 += (double)Mathf.Clamp01(num6 / 180f);
						}
						if (checkInclination)
						{
							float num7 = (float)(inclination - InclinationDeviation / 100.0 * 90.0);
							if (num7 < 0f)
							{
								num7 = Mathf.Abs(num7);
								if (Math.Abs(orbit.inclination) < (double)num7)
								{
									num6 = 180f - num6;
									if ((double)(num6 / 180f) < LANDeviation / 100.0)
									{
										num3 += (double)Mathf.Clamp01(num6 / 180f);
									}
								}
							}
						}
					}
					else
					{
						num3 += 1.0;
					}
				}
			}
			if (num2 == 0)
			{
				return 1;
			}
			return num3 / (double)num2;
		}
		return null;
	}
}

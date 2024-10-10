using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns29;

public class ScreenThermal : MonoBehaviour
{
	public Toggle dataActionMenus;

	public Toggle dataGUI;

	public Toggle debugColors;

	public Toggle radiationEnabled;

	public float radiationFactorMin = 0.1f;

	public float radiationFactorMax = 100f;

	public Slider radiationFactorSlider;

	public TextMeshProUGUI radiationFactorText;

	public Toggle conductionEnabled;

	public float conductionFactorMin = 0.1f;

	public float conductionFactorMax = 200f;

	public Slider conductionFactorSlider;

	public TextMeshProUGUI conductionFactorText;

	public Toggle convectionEnabled;

	public float convectionFactorMin = 0.1f;

	public float convectionFactorMax = 100f;

	public Slider convectionFactorSlider;

	public TextMeshProUGUI convectionFactorText;

	public float generationFactorMin;

	public float generationFactorMax = 2f;

	public Slider generationFactorSlider;

	public TextMeshProUGUI generationFactorText;

	public float newtonianFactorMin = 0.01f;

	public float newtonianFactorMax = 5f;

	public Slider newtonianFactorSlider;

	public TextMeshProUGUI newtonianFactorText;

	public float luminosityFactorMin = 1f;

	public float luminosityFactorMax = 10000f;

	public Slider luminosityFactorSlider;

	public TextMeshProUGUI luminosityFactorText;

	public float insolationFactorMin = 0.01f;

	public float insolationFactorMax = 1f;

	public Slider insolationFactorSlider;

	public TextMeshProUGUI insolationFactorText;

	public void Start()
	{
		dataActionMenus.isOn = PhysicsGlobals.ThermalDataDisplay;
		dataGUI.isOn = PhysicsGlobals.ThermoGUIDisplay;
		debugColors.isOn = PhysicsGlobals.ThermalColorsDebug;
		radiationEnabled.isOn = PhysicsGlobals.ThermalRadiationEnabled;
		radiationFactorSlider.minValue = radiationFactorMin;
		radiationFactorSlider.maxValue = radiationFactorMax;
		radiationFactorSlider.value = (float)PhysicsGlobals.RadiationFactor;
		radiationFactorSlider.interactable = PhysicsGlobals.ThermalRadiationEnabled;
		conductionEnabled.isOn = PhysicsGlobals.ThermalConductionEnabled;
		conductionFactorSlider.minValue = conductionFactorMin;
		conductionFactorSlider.maxValue = conductionFactorMax;
		conductionFactorSlider.value = (float)PhysicsGlobals.ConductionFactor;
		conductionFactorSlider.interactable = PhysicsGlobals.ThermalConductionEnabled;
		convectionEnabled.isOn = PhysicsGlobals.ThermalConvectionEnabled;
		convectionFactorSlider.minValue = convectionFactorMin;
		convectionFactorSlider.maxValue = convectionFactorMax;
		convectionFactorSlider.value = (float)PhysicsGlobals.MachConvectionFactor;
		convectionFactorSlider.interactable = PhysicsGlobals.ThermalConvectionEnabled;
		generationFactorSlider.minValue = generationFactorMin;
		generationFactorSlider.maxValue = generationFactorMax;
		generationFactorSlider.value = (float)PhysicsGlobals.InternalHeatProductionFactor;
		newtonianFactorSlider.minValue = newtonianFactorMin;
		newtonianFactorSlider.maxValue = newtonianFactorMax;
		newtonianFactorSlider.value = (float)PhysicsGlobals.NewtonianTemperatureFactor;
		luminosityFactorSlider.minValue = luminosityFactorMin;
		luminosityFactorSlider.maxValue = luminosityFactorMax;
		luminosityFactorSlider.value = (float)PhysicsGlobals.SolarLuminosityAtHome;
		insolationFactorSlider.minValue = insolationFactorMin;
		insolationFactorSlider.maxValue = insolationFactorMax;
		insolationFactorSlider.value = (float)PhysicsGlobals.SolarInsolationAtHome;
		radiationFactorText.text = PhysicsGlobals.RadiationFactor.ToString("F3");
		conductionFactorText.text = PhysicsGlobals.ConductionFactor.ToString("F3");
		convectionFactorText.text = PhysicsGlobals.MachConvectionFactor.ToString("F3");
		generationFactorText.text = PhysicsGlobals.InternalHeatProductionFactor.ToString("F3");
		newtonianFactorText.text = PhysicsGlobals.NewtonianTemperatureFactor.ToString("F3");
		luminosityFactorText.text = PhysicsGlobals.SolarLuminosityAtHome.ToString();
		insolationFactorText.text = PhysicsGlobals.SolarInsolationAtHome.ToString();
		AddListeners();
	}

	public void AddListeners()
	{
		dataActionMenus.onValueChanged.AddListener(OnDataActionMenusToggle);
		dataGUI.onValueChanged.AddListener(OnDataGUIToggle);
		debugColors.onValueChanged.AddListener(OnDebugColorsToggle);
		radiationEnabled.onValueChanged.AddListener(OnRadiationEnabledToggle);
		radiationFactorSlider.onValueChanged.AddListener(OnRadiationFactorSet);
		conductionEnabled.onValueChanged.AddListener(OnConductionEnabledToggle);
		conductionFactorSlider.onValueChanged.AddListener(OnConductionFactorSet);
		convectionEnabled.onValueChanged.AddListener(OnConvectionEnabledToggle);
		convectionFactorSlider.onValueChanged.AddListener(OnConvectionFactorSet);
		generationFactorSlider.onValueChanged.AddListener(OnGenerationFactorSet);
		newtonianFactorSlider.onValueChanged.AddListener(OnNewtonianFactorSet);
		luminosityFactorSlider.onValueChanged.AddListener(OnLuminosityFactorSet);
		insolationFactorSlider.onValueChanged.AddListener(OnInsolationFactorSet);
	}

	public void OnDataActionMenusToggle(bool on)
	{
		PhysicsGlobals.ThermalDataDisplay = on;
	}

	public void OnDataGUIToggle(bool on)
	{
		PhysicsGlobals.ThermoGUIDisplay = on;
	}

	public void OnDebugColorsToggle(bool on)
	{
		PhysicsGlobals.ThermalColorsDebug = on;
	}

	public void OnRadiationEnabledToggle(bool on)
	{
		PhysicsGlobals.ThermalRadiationEnabled = on;
		radiationFactorSlider.interactable = on;
	}

	public void OnRadiationFactorSet(float value)
	{
		PhysicsGlobals.RadiationFactor = value;
		radiationFactorText.text = PhysicsGlobals.RadiationFactor.ToString("F3");
	}

	public void OnConductionEnabledToggle(bool on)
	{
		PhysicsGlobals.ThermalConductionEnabled = on;
		conductionFactorSlider.interactable = on;
	}

	public void OnConductionFactorSet(float value)
	{
		PhysicsGlobals.ConductionFactor = value;
		conductionFactorText.text = PhysicsGlobals.ConductionFactor.ToString("F3");
	}

	public void OnConvectionEnabledToggle(bool on)
	{
		PhysicsGlobals.ThermalConvectionEnabled = on;
		convectionFactorSlider.interactable = on;
	}

	public void OnConvectionFactorSet(float value)
	{
		PhysicsGlobals.MachConvectionFactor = value;
		convectionFactorText.text = PhysicsGlobals.MachConvectionFactor.ToString("F3");
	}

	public void OnGenerationFactorSet(float value)
	{
		PhysicsGlobals.InternalHeatProductionFactor = value;
		generationFactorText.text = PhysicsGlobals.InternalHeatProductionFactor.ToString("F3");
	}

	public void OnNewtonianFactorSet(float value)
	{
		PhysicsGlobals.NewtonianTemperatureFactor = value;
		newtonianFactorText.text = PhysicsGlobals.NewtonianTemperatureFactor.ToString("F3");
	}

	public void OnLuminosityFactorSet(float value)
	{
		PhysicsGlobals.SolarLuminosityAtHome = value;
		luminosityFactorText.text = PhysicsGlobals.SolarLuminosityAtHome.ToString();
	}

	public void OnInsolationFactorSet(float value)
	{
		PhysicsGlobals.SolarInsolationAtHome = value;
		insolationFactorText.text = PhysicsGlobals.SolarInsolationAtHome.ToString();
	}
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns29;

public class ScreenAero : MonoBehaviour
{
	public Toggle dataActionMenus;

	public Toggle dataGUI;

	public Toggle forcesInFlight;

	public float forceDisplayScaleMin = 0.01f;

	public float forceDisplayScaleMax = 1f;

	public Slider forceDisplayScaleSlider;

	public TextMeshProUGUI forceDisplayScaleText;

	public float globalLiftMin = 0.0001f;

	public float globalLiftMax = 0.1f;

	public Slider globalLiftSlider;

	public TextMeshProUGUI globalLiftText;

	public float liftDragMin = 0.001f;

	public float liftDragMax = 0.25f;

	public Slider liftDragSlider;

	public TextMeshProUGUI liftDragText;

	public float bodyLiftMin;

	public float bodyLiftMax = 100f;

	public Slider bodyLiftSlider;

	public TextMeshProUGUI bodyLiftText;

	public void Start()
	{
		dataActionMenus.isOn = PhysicsGlobals.AeroDataDisplay;
		dataGUI.isOn = PhysicsGlobals.AeroGUIDisplay;
		forcesInFlight.isOn = PhysicsGlobals.AeroForceDisplay;
		forceDisplayScaleSlider.minValue = forceDisplayScaleMin;
		forceDisplayScaleSlider.maxValue = forceDisplayScaleMax;
		forceDisplayScaleSlider.value = PhysicsGlobals.AeroForceDisplayScale;
		forceDisplayScaleSlider.interactable = PhysicsGlobals.AeroForceDisplay;
		globalLiftSlider.minValue = globalLiftMin;
		globalLiftSlider.maxValue = globalLiftMax;
		globalLiftSlider.value = PhysicsGlobals.LiftMultiplier;
		liftDragSlider.minValue = liftDragMin;
		liftDragSlider.maxValue = liftDragMax;
		liftDragSlider.value = PhysicsGlobals.LiftDragMultiplier;
		bodyLiftSlider.minValue = bodyLiftMin;
		bodyLiftSlider.maxValue = bodyLiftMax;
		bodyLiftSlider.value = PhysicsGlobals.BodyLiftMultiplier;
		forceDisplayScaleText.text = KSPUtil.LocalizeNumber(PhysicsGlobals.AeroForceDisplayScale, "F2");
		globalLiftText.text = KSPUtil.LocalizeNumber(PhysicsGlobals.LiftMultiplier, "F3");
		liftDragText.text = KSPUtil.LocalizeNumber(PhysicsGlobals.LiftDragMultiplier, "F3");
		bodyLiftText.text = KSPUtil.LocalizeNumber(PhysicsGlobals.BodyLiftMultiplier, "F3");
		AddListeners();
	}

	public void AddListeners()
	{
		dataActionMenus.onValueChanged.AddListener(OnDataActionMenusToggle);
		dataGUI.onValueChanged.AddListener(OnDataGUIToggle);
		forcesInFlight.onValueChanged.AddListener(OnForcesInFlightToggle);
		forceDisplayScaleSlider.onValueChanged.AddListener(OnForceDisplayScaleSet);
		globalLiftSlider.onValueChanged.AddListener(OnGlobalLiftSet);
		liftDragSlider.onValueChanged.AddListener(OnLiftDragSet);
		bodyLiftSlider.onValueChanged.AddListener(OnBodyLiftSet);
	}

	public void OnDataActionMenusToggle(bool on)
	{
		PhysicsGlobals.AeroDataDisplay = on;
	}

	public void OnDataGUIToggle(bool on)
	{
		PhysicsGlobals.AeroGUIDisplay = on;
	}

	public void OnForcesInFlightToggle(bool on)
	{
		PhysicsGlobals.AeroForceDisplay = on;
		forceDisplayScaleSlider.interactable = on;
	}

	public void OnForceDisplayScaleSet(float value)
	{
		PhysicsGlobals.AeroForceDisplayScale = value;
		forceDisplayScaleText.text = KSPUtil.LocalizeNumber(PhysicsGlobals.AeroForceDisplayScale, "F2");
	}

	public void OnGlobalLiftSet(float value)
	{
		PhysicsGlobals.LiftMultiplier = value;
		globalLiftText.text = KSPUtil.LocalizeNumber(PhysicsGlobals.LiftMultiplier, "F3");
	}

	public void OnLiftDragSet(float value)
	{
		PhysicsGlobals.LiftDragMultiplier = value;
		liftDragText.text = KSPUtil.LocalizeNumber(PhysicsGlobals.LiftDragMultiplier, "F3");
	}

	public void OnBodyLiftSet(float value)
	{
		PhysicsGlobals.BodyLiftMultiplier = value;
		bodyLiftText.text = KSPUtil.LocalizeNumber(PhysicsGlobals.BodyLiftMultiplier, "F3");
	}
}

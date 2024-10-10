using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns29;

public class ScreenDrag : MonoBehaviour
{
	public Toggle applyDrag;

	public Toggle dragSpheres;

	public Toggle dragAcceleration;

	public Toggle nonPhysicalDrag;

	public Toggle nonPhysicalCoM;

	public float globalDragMin = 0.1f;

	public float globalDragMax = 10f;

	public Slider globalDragSlider;

	public TextMeshProUGUI globalDragText;

	public float cubeMultiplierMin = 0.001f;

	public float cubeMultiplierMax = 0.1f;

	public Slider cubeMultiplierSlider;

	public TextMeshProUGUI cubeMultiplierText;

	public float angularDragMin = 0.01f;

	public float angularDragMax = 10f;

	public Slider angularDragSlider;

	public TextMeshProUGUI angularDragText;

	public void Start()
	{
		applyDrag.isOn = PhysicsGlobals.ApplyDrag;
		dragSpheres.isOn = PhysicsGlobals.DragCubesUseSpherical;
		dragAcceleration.isOn = PhysicsGlobals.DragUsesAcceleration;
		nonPhysicalDrag.isOn = PhysicsGlobals.ApplyDragToNonPhysicsParts;
		nonPhysicalCoM.isOn = PhysicsGlobals.ApplyDragToNonPhysicsPartsAtParentCoM;
		globalDragSlider.minValue = globalDragMin;
		globalDragSlider.maxValue = globalDragMax;
		globalDragSlider.value = PhysicsGlobals.DragMultiplier;
		cubeMultiplierSlider.minValue = cubeMultiplierMin;
		cubeMultiplierSlider.maxValue = cubeMultiplierMax;
		cubeMultiplierSlider.value = PhysicsGlobals.DragCubeMultiplier;
		angularDragSlider.minValue = angularDragMin;
		angularDragSlider.maxValue = angularDragMax;
		angularDragSlider.value = PhysicsGlobals.AngularDragMultiplier;
		globalDragText.text = PhysicsGlobals.DragMultiplier.ToString("F3");
		cubeMultiplierText.text = PhysicsGlobals.DragCubeMultiplier.ToString("F3");
		angularDragText.text = PhysicsGlobals.AngularDragMultiplier.ToString("F3");
		CheckInteractable();
		AddListeners();
	}

	public void AddListeners()
	{
		applyDrag.onValueChanged.AddListener(OnApplyDragToggle);
		dragSpheres.onValueChanged.AddListener(OnDragSpheresToggle);
		dragAcceleration.onValueChanged.AddListener(OnDragAccelerationToggle);
		nonPhysicalDrag.onValueChanged.AddListener(OnNonPhysicalDragToggle);
		nonPhysicalCoM.onValueChanged.AddListener(OnNonPhysicalCoMToggle);
		globalDragSlider.onValueChanged.AddListener(OnGlobalDragSet);
		cubeMultiplierSlider.onValueChanged.AddListener(OnCubeMultiplierSet);
		angularDragSlider.onValueChanged.AddListener(OnAngularDragSet);
	}

	public void CheckInteractable()
	{
		dragSpheres.interactable = PhysicsGlobals.ApplyDrag;
		dragAcceleration.interactable = PhysicsGlobals.ApplyDrag;
		nonPhysicalDrag.interactable = PhysicsGlobals.ApplyDrag;
		nonPhysicalCoM.interactable = PhysicsGlobals.ApplyDrag && PhysicsGlobals.ApplyDragToNonPhysicsParts;
		globalDragSlider.interactable = PhysicsGlobals.ApplyDrag;
		cubeMultiplierSlider.interactable = PhysicsGlobals.ApplyDrag;
		angularDragSlider.interactable = PhysicsGlobals.ApplyDrag;
	}

	public void OnApplyDragToggle(bool on)
	{
		PhysicsGlobals.ApplyDrag = on;
		CheckInteractable();
	}

	public void OnDragSpheresToggle(bool on)
	{
		PhysicsGlobals.DragCubesUseSpherical = on;
	}

	public void OnDragAccelerationToggle(bool on)
	{
		PhysicsGlobals.DragUsesAcceleration = on;
	}

	public void OnNonPhysicalDragToggle(bool on)
	{
		PhysicsGlobals.ApplyDragToNonPhysicsParts = on;
		nonPhysicalCoM.interactable = PhysicsGlobals.ApplyDrag && PhysicsGlobals.ApplyDragToNonPhysicsParts;
	}

	public void OnNonPhysicalCoMToggle(bool on)
	{
		PhysicsGlobals.ApplyDragToNonPhysicsPartsAtParentCoM = on;
	}

	public void OnGlobalDragSet(float value)
	{
		PhysicsGlobals.DragMultiplier = value;
		globalDragText.text = PhysicsGlobals.DragMultiplier.ToString("F3");
	}

	public void OnCubeMultiplierSet(float value)
	{
		PhysicsGlobals.DragCubeMultiplier = value;
		cubeMultiplierText.text = PhysicsGlobals.DragCubeMultiplier.ToString("F3");
	}

	public void OnAngularDragSet(float value)
	{
		PhysicsGlobals.AngularDragMultiplier = value;
		angularDragText.text = PhysicsGlobals.AngularDragMultiplier.ToString("F3");
	}
}

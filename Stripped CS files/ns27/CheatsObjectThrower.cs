using System;
using System.Collections.Generic;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns27;

public class CheatsObjectThrower : MonoBehaviour
{
	[Serializable]
	public class ObjectType
	{
		public string name;

		public string displayName;

		public GameObject obj;
	}

	public List<ObjectType> objectTypes = new List<ObjectType>();

	public Button objectPrev;

	public Button objectNext;

	public TextMeshProUGUI objectText;

	public float sizeMin = 1f;

	public float sizeMax = 5f;

	public float sizeDefault = 1f;

	public Slider sizeSlider;

	public TextMeshProUGUI sizeText;

	public float massMin = 1f;

	public float massMax = 5f;

	public float massDefault = 1f;

	public Slider massSlider;

	public TextMeshProUGUI massText;

	public float speedMin = 1f;

	public float speedMax = 5f;

	public float speedDefault = 1f;

	public Slider speedSlider;

	public TextMeshProUGUI speedText;

	public Toggle armedToggle;

	public TextMeshProUGUI armedText;

	public int objectTypeSelected;

	public float sizeValue;

	public float massValue;

	public float speedValue;

	public bool isArmed;

	public static float projectileDuration = 10f;

	public void Awake()
	{
		objectPrev.onClick.AddListener(OnObjectPrev);
		objectNext.onClick.AddListener(OnObjectNext);
		sizeSlider.onValueChanged.AddListener(OnSizeSet);
		massSlider.onValueChanged.AddListener(OnMassSet);
		speedSlider.onValueChanged.AddListener(OnSpeedSet);
		armedToggle.isOn = false;
		armedToggle.onValueChanged.AddListener(OnArmedToggle);
	}

	public void Start()
	{
		sizeSlider.minValue = sizeMin;
		sizeSlider.maxValue = sizeMax;
		sizeSlider.value = sizeDefault;
		sizeValue = sizeDefault;
		massSlider.minValue = massMin;
		massSlider.maxValue = massMax;
		massSlider.value = massDefault;
		massValue = massDefault;
		speedSlider.minValue = speedMin;
		speedSlider.maxValue = speedMax;
		speedSlider.value = speedDefault;
		speedValue = speedDefault;
		armedToggle.isOn = false;
		SetUI();
	}

	public void Update()
	{
		if (!Input.GetMouseButtonDown(2) || !FlightCamera.fetch.gameObject.activeInHierarchy || !isArmed)
		{
			return;
		}
		Vector3 vector = Vector3.zero;
		if (FlightCamera.fetch.Target != null)
		{
			Rigidbody componentInParent = FlightCamera.fetch.Target.gameObject.GetComponentInParent<Rigidbody>();
			if (componentInParent != null)
			{
				vector = componentInParent.velocity;
			}
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(objectTypes[objectTypeSelected].obj);
		gameObject.transform.localScale = Vector3.one * sizeValue;
		gameObject.transform.position = FlightCamera.fetch.mainCamera.ScreenToWorldPoint(Input.mousePosition);
		gameObject.layer = 15;
		physicalObject physicalObject = gameObject.AddComponent<physicalObject>();
		physicalObject.rb = gameObject.AddComponent<Rigidbody>();
		gameObject.SetLayerRecursive(LayerMask.NameToLayer("PhysicalObjects"));
		physicalObject.rb.useGravity = false;
		physicalObject.rb.mass = massValue;
		physicalObject.rb.AddForce(vector + FlightCamera.fetch.mainCamera.ScreenPointToRay(Input.mousePosition).direction * speedValue, ForceMode.VelocityChange);
		gameObject.AddComponent<DestroyAfterTime>().time = projectileDuration;
	}

	public void SetUI()
	{
		ObjectType objectType = objectTypes[objectTypeSelected];
		objectText.text = objectType.displayName;
		sizeText.text = KSPUtil.LocalizeNumber(sizeValue, "F1");
		massText.text = KSPUtil.LocalizeNumber(massValue, "F1");
		speedText.text = KSPUtil.LocalizeNumber(speedValue, "F1");
		armedText.text = (isArmed ? Localizer.Format("#autoLOC_900375") : Localizer.Format("#autoLOC_900378"));
	}

	public void OnObjectPrev()
	{
		if (objectTypeSelected > 0)
		{
			objectTypeSelected--;
		}
		else
		{
			objectTypeSelected = objectTypes.Count - 1;
		}
		SetUI();
	}

	public void OnObjectNext()
	{
		if (objectTypeSelected < objectTypes.Count - 1)
		{
			objectTypeSelected++;
		}
		else
		{
			objectTypeSelected = 0;
		}
		SetUI();
	}

	public void OnSizeSet(float value)
	{
		sizeValue = value;
		SetUI();
	}

	public void OnSpeedSet(float value)
	{
		speedValue = value;
		SetUI();
	}

	public void OnMassSet(float value)
	{
		massValue = value;
		SetUI();
	}

	public void OnArmedToggle(bool armed)
	{
		isArmed = armed;
		SetUI();
	}
}

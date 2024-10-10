using TMPro;
using UnityEngine;

namespace ns11;

[ExecuteInEditMode]
public class RotationalGaugeOffsetMarker : RotationalGauge
{
	[SerializeField]
	public RectTransform offsetPointer;

	[SerializeField]
	public float nextToCurrentAngle;

	[SerializeField]
	public float offsetAngle;

	[SerializeField]
	public TextMeshProUGUI offsetTextField;

	[SerializeField]
	public GameObject stageMarker;

	[SerializeField]
	public GameObject offsetObject;

	[SerializeField]
	public GameObject offsetGameObjectPrefab;

	[SerializeField]
	public double stageDV;

	public double StageDV
	{
		get
		{
			return stageDV;
		}
		set
		{
			stageDV = value;
		}
	}

	public override void Awake()
	{
		base.Awake();
		offsetObject = Object.Instantiate(offsetGameObjectPrefab);
		offsetTextField = offsetObject.GetComponentInChildren<TextMeshProUGUI>();
		offsetPointer = offsetObject.GetComponent<RectTransform>();
	}

	public void Start()
	{
		offsetObject.transform.localScale = Vector3.one;
		offsetObject.transform.localPosition = Vector3.zero;
		offsetObject.transform.SetParent(base.gameObject.transform.parent);
	}

	public void OnDestroy()
	{
		if (offsetObject != null)
		{
			offsetObject.DestroyGameObject();
		}
	}

	public override void LateUpdate()
	{
		base.LateUpdate();
		offsetAngle = currentAngle + (nextToCurrentAngle - currentAngle) / 2f;
		offsetPointer.localRotation = Quaternion.AngleAxis(offsetAngle, rotationAxis);
	}

	public void SetNextToValue(float nextAngle)
	{
		nextToCurrentAngle = nextAngle;
	}

	public void SetTextField(string text, int opacity)
	{
		opacity = Mathf.Clamp(opacity, 25, 255);
		offsetTextField.text = string.Format("<color=#FFFFFF{1:X}>{0}</color>", text, opacity);
	}

	public void ToggleOffsetMarker(bool active)
	{
		stageMarker.SetActive(active);
	}
}

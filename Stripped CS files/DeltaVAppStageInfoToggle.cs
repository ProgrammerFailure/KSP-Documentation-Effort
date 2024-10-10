using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeltaVAppStageInfoToggle : MonoBehaviour
{
	[SerializeField]
	public Toggle checkbox;

	[SerializeField]
	public TextMeshProUGUI label;

	public DeltaVAppValues.InfoLine info;

	public void Start()
	{
		checkbox.onValueChanged.AddListener(ToggleCheckbox);
	}

	public void OnDestroy()
	{
		checkbox.onValueChanged.RemoveListener(ToggleCheckbox);
	}

	public void Update()
	{
	}

	public void Setup(DeltaVAppValues.InfoLine info, Transform parent)
	{
		this.info = info;
		base.transform.SetParent(parent);
		base.transform.localScale = Vector3.one;
		label.text = info.displayAppName;
		checkbox.isOn = info.Enabled;
	}

	public void ToggleCheckbox(bool value)
	{
		info.Enabled = value;
		if (DeltaVApp.Ready)
		{
			DeltaVApp.Instance.usage.displayFields++;
		}
	}
}

using UnityEngine;
using UnityEngine.UI;

namespace ns33;

public class ScreenResources : MonoBehaviour
{
	public Toggle heatGenerationToggle;

	public Toggle debugInfoToggle;

	public void Start()
	{
		if (ResourceSetup.Instance != null)
		{
			heatGenerationToggle.isOn = ResourceSetup.Instance.ResConfig.HeatEnabled;
			debugInfoToggle.isOn = ResourceSetup.Instance.ResConfig.ShowDebugOptions;
		}
		AddListeners();
	}

	public void AddListeners()
	{
		heatGenerationToggle.onValueChanged.AddListener(OnHeatGenerationToggle);
		debugInfoToggle.onValueChanged.AddListener(OnDebugInfoToggle);
	}

	public void OnHeatGenerationToggle(bool on)
	{
		if (ResourceSetup.Instance != null)
		{
			ResourceSetup.Instance.ResConfig.HeatEnabled = on;
		}
	}

	public void OnDebugInfoToggle(bool on)
	{
		if (ResourceSetup.Instance != null)
		{
			ResourceSetup.Instance.ResConfig.ShowDebugOptions = on;
		}
	}
}

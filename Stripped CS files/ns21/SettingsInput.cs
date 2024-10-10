using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using ns20;
using UnityEngine;

namespace ns21;

public class SettingsInput : SettingsWindow
{
	[Serializable]
	public class InputSettings
	{
		[XmlArray("Input Tabs")]
		[XmlArrayItem("Tab")]
		public InputTab[] tabs;
	}

	[Serializable]
	public class InputTab
	{
		[XmlAttribute("Name")]
		public string name;

		[XmlArray("KeyGroups")]
		[XmlArrayItem("KeyGroup")]
		public InputGroup[] keyBindings;

		[XmlArray("AxisGroups")]
		[XmlArrayItem("AxisGroup")]
		public InputGroup[] axisBindings;
	}

	[Serializable]
	public class InputGroup
	{
		[XmlAttribute("Name")]
		public string name;

		[XmlAttribute("UseModes")]
		public bool useModes;

		[XmlArray("Settings")]
		[XmlArrayItem("Settings")]
		public InputSetting[] settings;
	}

	[Serializable]
	public class InputSetting
	{
		[XmlAttribute("Name")]
		public string name;

		[XmlAttribute("Setting")]
		public string settingName;
	}

	public InputSettings inputSettings = new InputSettings();

	public string xmlPath = "InputSettings.xml";

	public SettingsInputGroupTitle groupTitlePrefab;

	public SettingsInputKey keyPrefab;

	public SettingsInputAxis axisPrefab;

	public SettingsInputBinding bindingWindowPrefab;

	[SettingsValue("Default")]
	public string settingsTab;

	public List<GameObject> spawnedObjects = new List<GameObject>();

	public RectTransform keyLayoutGroup => (base.Template as SettingsTemplateDualLayouts).layout;

	public RectTransform axisLayoutGroup => (base.Template as SettingsTemplateDualLayouts).layout2;

	[ContextMenu("Load XML")]
	public void LoadFile()
	{
		using FileStream stream = File.OpenRead(xmlPath);
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(InputSettings));
		inputSettings = xmlSerializer.Deserialize(stream) as InputSettings;
	}

	[ContextMenu("Save XML")]
	public void SaveFile()
	{
		using StreamWriter streamWriter = new StreamWriter(xmlPath);
		new XmlSerializer(typeof(InputSettings)).Serialize(streamWriter, inputSettings);
		streamWriter.Flush();
	}

	public void Start()
	{
		if (inputSettings.tabs.Length == 0)
		{
			return;
		}
		if (string.IsNullOrEmpty(settingsTab))
		{
			settingsTab = inputSettings.tabs[0].name;
		}
		for (int i = 0; i < inputSettings.tabs.Length; i++)
		{
			InputTab inputTab = inputSettings.tabs[i];
			if (inputTab.name != settingsTab)
			{
				continue;
			}
			for (int j = 0; j < inputTab.keyBindings.Length; j++)
			{
				InputGroup inputGroup = inputTab.keyBindings[j];
				SettingsInputGroupTitle settingsInputGroupTitle = UnityEngine.Object.Instantiate(groupTitlePrefab);
				settingsInputGroupTitle.transform.SetParent(keyLayoutGroup.transform, worldPositionStays: false);
				spawnedObjects.Add(settingsInputGroupTitle.gameObject);
				settingsInputGroupTitle.SetTitleText(inputGroup.name);
				if (inputGroup.settings == null)
				{
					continue;
				}
				for (int k = 0; k < inputGroup.settings.Length; k++)
				{
					InputSetting inputSetting = inputGroup.settings[k];
					SettingsInputKey settingsInputKey = UnityEngine.Object.Instantiate(keyPrefab);
					settingsInputKey.transform.SetParent(keyLayoutGroup.transform, worldPositionStays: false);
					spawnedObjects.Add(settingsInputKey.gameObject);
					settingsInputKey.settingName = inputSetting.settingName;
					settingsInputKey.SetTitleText(inputSetting.name);
					if (inputGroup.useModes)
					{
						settingsInputKey.OnTryBind.Add(OnBindingRequestModes);
					}
					else
					{
						settingsInputKey.OnTryBind.Add(OnBindingRequest);
					}
				}
			}
			for (int l = 0; l < inputTab.axisBindings.Length; l++)
			{
				InputGroup inputGroup2 = inputTab.axisBindings[l];
				SettingsInputGroupTitle settingsInputGroupTitle2 = UnityEngine.Object.Instantiate(groupTitlePrefab);
				settingsInputGroupTitle2.transform.SetParent(axisLayoutGroup.transform, worldPositionStays: false);
				spawnedObjects.Add(settingsInputGroupTitle2.gameObject);
				settingsInputGroupTitle2.SetTitleText(inputGroup2.name);
				for (int m = 0; m < inputGroup2.settings.Length; m++)
				{
					InputSetting inputSetting2 = inputGroup2.settings[m];
					SettingsInputAxis settingsInputAxis = UnityEngine.Object.Instantiate(axisPrefab);
					settingsInputAxis.transform.SetParent(axisLayoutGroup.transform, worldPositionStays: false);
					spawnedObjects.Add(settingsInputAxis.gameObject);
					settingsInputAxis.settingName = inputSetting2.settingName;
					settingsInputAxis.SetTitleText(inputSetting2.name);
					if (inputGroup2.useModes)
					{
						settingsInputAxis.OnTryBind.Add(OnBindingRequestModes);
					}
					else
					{
						settingsInputAxis.OnTryBind.Add(OnBindingRequest);
					}
				}
			}
		}
	}

	public void OnBindingRequest(object binding, string name, SettingsInputBinding.BindingType type, SettingsInputBinding.BindingVariant variant)
	{
		SettingsInputBinding settingsInputBinding = UnityEngine.Object.Instantiate(bindingWindowPrefab);
		settingsInputBinding.transform.SetParent(SettingsScreen.Instance.transform, worldPositionStays: false);
		settingsInputBinding.transform.SetAsLastSibling();
		settingsInputBinding.binding = binding;
		settingsInputBinding.bindingName = name;
		settingsInputBinding.bindingType = type;
		settingsInputBinding.bindingVariant = variant;
		settingsInputBinding.bindingModes = false;
		settingsInputBinding.OnFinished.Add(OnBindingRequestFinished);
	}

	public void OnBindingRequestModes(object binding, string name, SettingsInputBinding.BindingType type, SettingsInputBinding.BindingVariant variant)
	{
		SettingsInputBinding settingsInputBinding = UnityEngine.Object.Instantiate(bindingWindowPrefab);
		settingsInputBinding.transform.SetParent(SettingsScreen.Instance.transform, worldPositionStays: false);
		settingsInputBinding.transform.SetAsLastSibling();
		settingsInputBinding.binding = binding;
		settingsInputBinding.bindingName = name;
		settingsInputBinding.bindingType = type;
		settingsInputBinding.bindingVariant = variant;
		settingsInputBinding.bindingModes = true;
		settingsInputBinding.OnFinished.Add(OnBindingRequestFinished);
	}

	public void OnBindingRequestFinished(bool bindingChanged)
	{
		if (bindingChanged)
		{
			keyLayoutGroup.gameObject.BroadcastMessage("UpdateInputSettings", SendMessageOptions.DontRequireReceiver);
			axisLayoutGroup.gameObject.BroadcastMessage("UpdateInputSettings", SendMessageOptions.DontRequireReceiver);
		}
	}
}

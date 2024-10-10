using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ns21;
using UnityEngine;

namespace ns20;

public class ReflectedSettingsWindow : SettingsWindow
{
	[Serializable]
	public class TabWrapper
	{
		[XmlAttribute("Name")]
		public string name = "";

		[XmlAttribute("Platform")]
		[UI_Enum(typeof(SettingsPlatform.Platform), 0)]
		public SettingsPlatform.Platform platform = SettingsPlatform.Platform.Everything;

		[XmlAttribute("Expansion")]
		[UI_Enum(typeof(SettingsExpansion.Expansion), 0)]
		public SettingsExpansion.Expansion expansion = SettingsExpansion.Expansion.Everything;

		[XmlArrayItem("Screen")]
		[XmlArray("Screens")]
		public SubTabWrapper[] tabs = new SubTabWrapper[0];

		public bool IsValid()
		{
			if (SettingsPlatform.IsPlatform(platform) && SettingsExpansion.IsExpansion(expansion))
			{
				int num = 0;
				int num2 = tabs.Length;
				while (true)
				{
					if (num < num2)
					{
						if (tabs[num].IsValid())
						{
							break;
						}
						num++;
						continue;
					}
					return false;
				}
				return true;
			}
			return false;
		}
	}

	[Serializable]
	public class SubTabWrapper
	{
		[XmlAttribute("Name")]
		public string name = "";

		[XmlAttribute("Platform")]
		[UI_Enum(typeof(SettingsPlatform.Platform), 0)]
		public SettingsPlatform.Platform platform = SettingsPlatform.Platform.Everything;

		[UI_Enum(typeof(SettingsExpansion.Expansion), 0)]
		[XmlAttribute("Expansion")]
		public SettingsExpansion.Expansion expansion = SettingsExpansion.Expansion.Everything;

		[XmlArray("Controls")]
		[XmlArrayItem("Control")]
		public ControlWrapper[] controls = new ControlWrapper[0];

		public bool IsValid()
		{
			if (SettingsPlatform.IsPlatform(platform) && SettingsExpansion.IsExpansion(expansion))
			{
				int num = 0;
				int num2 = controls.Length;
				while (true)
				{
					if (num < num2)
					{
						if (controls[num].IsValid())
						{
							break;
						}
						num++;
						continue;
					}
					return false;
				}
				return true;
			}
			return false;
		}
	}

	[Serializable]
	public class ControlWrapper
	{
		[XmlAttribute("Name")]
		public string name = "";

		[XmlAttribute("Platform")]
		[UI_Enum(typeof(SettingsPlatform.Platform), 0)]
		public SettingsPlatform.Platform platform = SettingsPlatform.Platform.Everything;

		[XmlAttribute("Expansion")]
		[UI_Enum(typeof(SettingsExpansion.Expansion), 0)]
		public SettingsExpansion.Expansion expansion = SettingsExpansion.Expansion.Everything;

		[XmlAttribute("Setting")]
		public string settingName = "";

		[XmlIgnore]
		public SettingsControlBase prefab;

		[XmlAttribute("Prefab")]
		public string prefabPath = "";

		[XmlArrayItem("V")]
		[XmlArray("Values")]
		public List<ValueWrapper> values = new List<ValueWrapper>();

		public bool IsValid()
		{
			if (SettingsPlatform.IsPlatform(platform) && SettingsExpansion.IsExpansion(expansion) && prefab.IsValid())
			{
				return true;
			}
			return false;
		}
	}

	[Serializable]
	public class ValueWrapper
	{
		[XmlAttribute("Name")]
		public string name;

		[XmlAttribute("Value")]
		public string value;
	}

	public TabWrapper[] tabs;

	public ReflectedSettingsWindowTab subTabPrefab;

	public string xmlPath = "ReflectedSettingsSetup.xml";

	public List<GameObject> spawnedObjects = new List<GameObject>();

	[SettingsValue("Default")]
	public string settingsTab;

	[SettingsValue("Default")]
	public string settingsSubTab;

	public SettingsTemplateDualLayouts dualTemplate;

	public List<GameObject> SpawnedObjects => spawnedObjects;

	public void Start()
	{
		Setup();
		OnOpenWindow();
	}

	public void Setup()
	{
		dualTemplate = base.Template as SettingsTemplateDualLayouts;
		int num = 0;
		int num2 = tabs.Length;
		TabWrapper tabWrapper;
		while (true)
		{
			if (num < num2)
			{
				tabWrapper = tabs[num];
				if (tabWrapper != null && !(tabWrapper.name != settingsTab) && tabWrapper.IsValid())
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		Setup(tabWrapper);
	}

	public void Setup(TabWrapper tab)
	{
		for (int i = 0; i < tab.tabs.Length; i++)
		{
			SubTabWrapper subTabWrapper = tab.tabs[i];
			if (subTabWrapper != null && SettingsPlatform.IsPlatform(subTabWrapper.platform) && SettingsExpansion.IsExpansion(subTabWrapper.expansion) && (string.IsNullOrEmpty(settingsSubTab) || !(subTabWrapper.name != settingsSubTab)) && subTabWrapper.IsValid())
			{
				ReflectedSettingsWindowTab reflectedSettingsWindowTab = UnityEngine.Object.Instantiate(subTabPrefab);
				reflectedSettingsWindowTab.transform.SetParent(GetTabParent(), worldPositionStays: false);
				SpawnedObjects.Add(reflectedSettingsWindowTab.gameObject);
				Setup(reflectedSettingsWindowTab, subTabWrapper);
			}
		}
	}

	public virtual void Setup(ReflectedSettingsWindowTab tab, SubTabWrapper subTab)
	{
		tab.titleText.text = subTab.name;
		for (int i = 0; i < subTab.controls.Length; i++)
		{
			ControlWrapper controlWrapper = subTab.controls[i];
			if (controlWrapper != null && SettingsPlatform.IsPlatform(controlWrapper.platform) && SettingsExpansion.IsExpansion(controlWrapper.expansion) && controlWrapper.IsValid())
			{
				SettingsControlBase settingsControlBase = UnityEngine.Object.Instantiate(controlWrapper.prefab);
				settingsControlBase.transform.SetParent(tab.layoutGroup.transform, worldPositionStays: false);
				SpawnedObjects.Add(settingsControlBase.gameObject);
				settingsControlBase.SetTitleText(controlWrapper.name);
				SettingsControlReflection settingsControlReflection = settingsControlBase as SettingsControlReflection;
				if (settingsControlReflection != null)
				{
					settingsControlReflection.settingName = controlWrapper.settingName;
				}
				SetupReflectionValues(settingsControlBase, controlWrapper.values);
			}
		}
	}

	public static void SetupReflectionValues(object instance, List<ValueWrapper> values)
	{
		SettingsSetup.FieldWrapper[] controlTypeFields = SettingsSetup.GetControlTypeFields(instance.GetType());
		int i = 0;
		for (int count = values.Count; i < count; i++)
		{
			ValueWrapper valueWrapper = values[i];
			SettingsSetup.FieldWrapper field = GetField(controlTypeFields, valueWrapper);
			if (field == null)
			{
				continue;
			}
			if (field.field.FieldType == typeof(float))
			{
				float result = (float)field.defaultValue;
				if (!float.TryParse(valueWrapper.value, out result))
				{
					result = (float)field.defaultValue;
				}
				field.field.SetValue(instance, result);
			}
			else if (field.field.FieldType == typeof(int))
			{
				int result2 = (int)field.defaultValue;
				if (!int.TryParse(valueWrapper.value, out result2))
				{
					result2 = (int)field.defaultValue;
				}
				field.field.SetValue(instance, result2);
			}
			else if (field.field.FieldType == typeof(bool))
			{
				bool result3 = (bool)field.defaultValue;
				if (!bool.TryParse(valueWrapper.value, out result3))
				{
					result3 = (bool)field.defaultValue;
				}
				field.field.SetValue(instance, result3);
			}
			else if (field.field.FieldType == typeof(string[]))
			{
				field.field.SetValue(instance, SettingsSetup.StringToStringArray(valueWrapper.value));
			}
			else if (field.field.FieldType == typeof(int[]))
			{
				field.field.SetValue(instance, SettingsSetup.StringToIntArray(valueWrapper.value));
			}
			else if (field.field.FieldType == typeof(float[]))
			{
				field.field.SetValue(instance, SettingsSetup.StringToFloatArray(valueWrapper.value));
			}
			else if (field.field.FieldType == typeof(string))
			{
				field.field.SetValue(instance, valueWrapper.value);
			}
			else
			{
				if (!(field.field.FieldType == typeof(Color)))
				{
					continue;
				}
				Color color = new Color(1f, 0.92f, 0.016f, 1f);
				if (!ColorUtility.TryParseHtmlString(valueWrapper.value, out color))
				{
					ColorUtility.TryParseHtmlString((string)field.defaultValue, out color);
					if (!ColorUtility.TryParseHtmlString(valueWrapper.value, out color))
					{
						color = new Color(1f, 0.92f, 0.016f, 1f);
					}
				}
				field.field.SetValue(instance, color);
			}
		}
	}

	public static SettingsSetup.FieldWrapper GetField(SettingsSetup.FieldWrapper[] wrappers, ValueWrapper value)
	{
		int num = 0;
		int num2 = wrappers.Length;
		while (true)
		{
			if (num < num2)
			{
				if (wrappers[num].field.Name == value.name)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return wrappers[num];
	}

	public RectTransform GetTabParent()
	{
		if (dualTemplate != null)
		{
			return dualTemplate.GetLayoutFlipFlop();
		}
		return base.Template.layout;
	}

	public void OnOpenWindow()
	{
	}

	public void OnCloseWindow()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public override bool IsValid()
	{
		if (!IsValidSecondary())
		{
			return false;
		}
		int num = 0;
		int num2 = tabs.Length;
		while (true)
		{
			if (num < num2)
			{
				if (tabs[num].IsValid())
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public virtual bool IsValidSecondary()
	{
		return true;
	}
}

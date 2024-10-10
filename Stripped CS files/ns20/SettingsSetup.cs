using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using UnityEngine;

namespace ns20;

[Serializable]
[XmlRoot("SettingsSetup")]
public class SettingsSetup
{
	[Serializable]
	public class MainTab
	{
		[XmlAttribute("Name")]
		public string name = "";

		[XmlArrayItem("SubTab")]
		[XmlArray("SubTabs")]
		public SubTab[] subTabs = new SubTab[0];

		[XmlElement("Window")]
		public Window window;

		[XmlAttribute("Platform")]
		[UI_Enum(typeof(SettingsPlatform.Platform), 0)]
		public SettingsPlatform.Platform platform = SettingsPlatform.Platform.Everything;

		[UI_Enum(typeof(SettingsExpansion.Expansion), 0)]
		[XmlAttribute("Expansion")]
		public SettingsExpansion.Expansion expansion = SettingsExpansion.Expansion.Everything;

		public bool IsValid()
		{
			if (SettingsPlatform.IsPlatform(platform) && SettingsExpansion.IsExpansion(expansion))
			{
				if (subTabs.Length != 0)
				{
					int i = 0;
					for (int num = subTabs.Length; i < num; i++)
					{
						if (subTabs[i].IsValid())
						{
							return true;
						}
					}
				}
				else if (window != null)
				{
					return window.IsValid();
				}
				return false;
			}
			return false;
		}
	}

	[Serializable]
	public class SubTab
	{
		[XmlAttribute("Name")]
		public string name = "";

		[XmlElement("Window")]
		public Window window;

		[UI_Enum(typeof(SettingsPlatform.Platform), 0)]
		[XmlAttribute("Platform")]
		public SettingsPlatform.Platform platform = SettingsPlatform.Platform.Everything;

		[XmlAttribute("Expansion")]
		[UI_Enum(typeof(SettingsExpansion.Expansion), 0)]
		public SettingsExpansion.Expansion expansion = SettingsExpansion.Expansion.Everything;

		public bool IsValid()
		{
			if (SettingsPlatform.IsPlatform(platform) && SettingsExpansion.IsExpansion(expansion))
			{
				if (window != null)
				{
					return window.IsValid();
				}
				return false;
			}
			return false;
		}
	}

	[Serializable]
	public class Window
	{
		[XmlIgnore]
		public SettingsWindow prefab;

		[XmlAttribute("Prefab")]
		public string prefabPath;

		[XmlIgnore]
		public SettingsTemplate templatePrefab;

		[XmlAttribute("Template")]
		public string templatePrefabPath;

		[XmlArray("Values")]
		[XmlArrayItem("Value")]
		public WindowValue[] values = new WindowValue[0];

		public bool IsValid()
		{
			return prefab.IsValid();
		}
	}

	[Serializable]
	public class WindowValue
	{
		[XmlAttribute("Name")]
		public string name;

		[XmlAttribute("Value")]
		public string value;
	}

	public class FieldWrapper
	{
		public FieldInfo field;

		public object defaultValue;

		public FieldWrapper(FieldInfo field, SettingsValue attr)
		{
			this.field = field;
			defaultValue = attr.defaultValue;
		}
	}

	[XmlArray("MainTabs")]
	[XmlArrayItem("MainTab")]
	public MainTab[] tabs;

	public static string StringArrayToString(string[] array)
	{
		string text = "";
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			text += array[i];
			if (i < array.Length - 1)
			{
				text += ",";
			}
		}
		return text;
	}

	public static string[] StringToStringArray(string str)
	{
		return str.Split(new char[1] { ',' }, StringSplitOptions.None);
	}

	public static string IntArrayToString(int[] array)
	{
		string text = "";
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			text += array[i];
			if (i < array.Length - 1)
			{
				text += ",";
			}
		}
		return text;
	}

	public static int[] StringToIntArray(string str)
	{
		string[] array = str.Split(new char[1] { ',' }, StringSplitOptions.None);
		int[] array2 = new int[array.Length];
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			int result = 0;
			if (!string.IsNullOrEmpty(array[i]) && int.TryParse(array[i], out result))
			{
				array2[i] = result;
			}
			else
			{
				array2[i] = 0;
			}
		}
		return array2;
	}

	public static string FloatArrayToString(float[] array)
	{
		string text = "";
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			text += array[i];
			if (i < array.Length - 1)
			{
				text += ",";
			}
		}
		return text;
	}

	public static float[] StringToFloatArray(string str)
	{
		string[] array = str.Split(new char[1] { ',' }, StringSplitOptions.None);
		float[] array2 = new float[array.Length];
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			float result = 0f;
			if (!string.IsNullOrEmpty(array[i]) && float.TryParse(array[i], out result))
			{
				array2[i] = result;
			}
			else
			{
				array2[i] = 0f;
			}
		}
		return array2;
	}

	public static void SetupReflectionValues(object instance, WindowValue[] values)
	{
		FieldWrapper[] controlTypeFields = GetControlTypeFields(instance.GetType());
		int i = 0;
		for (int num = values.Length; i < num; i++)
		{
			WindowValue windowValue = values[i];
			FieldWrapper field = GetField(controlTypeFields, windowValue);
			if (field == null)
			{
				continue;
			}
			if (field.field.FieldType == typeof(float))
			{
				float result = (float)field.defaultValue;
				if (!float.TryParse(windowValue.value, out result))
				{
					result = (float)field.defaultValue;
				}
				field.field.SetValue(instance, result);
			}
			else if (field.field.FieldType == typeof(int))
			{
				int result2 = (int)field.defaultValue;
				if (!int.TryParse(windowValue.value, out result2))
				{
					result2 = (int)field.defaultValue;
				}
				field.field.SetValue(instance, result2);
			}
			else if (field.field.FieldType == typeof(bool))
			{
				bool result3 = (bool)field.defaultValue;
				if (!bool.TryParse(windowValue.value, out result3))
				{
					result3 = (bool)field.defaultValue;
				}
				field.field.SetValue(instance, result3);
			}
			else if (field.field.FieldType == typeof(string[]))
			{
				field.field.SetValue(instance, StringToStringArray(windowValue.value));
			}
			else if (field.field.FieldType == typeof(int[]))
			{
				field.field.SetValue(instance, StringToIntArray(windowValue.value));
			}
			else if (field.field.FieldType == typeof(float[]))
			{
				field.field.SetValue(instance, StringToFloatArray(windowValue.value));
			}
			else if (field.field.FieldType == typeof(string))
			{
				field.field.SetValue(instance, windowValue.value);
			}
			else if (field.field.FieldType == typeof(Color))
			{
				field.field.SetValue(instance, windowValue.value);
			}
		}
	}

	public static FieldWrapper GetField(FieldWrapper[] wrappers, WindowValue value)
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

	public static FieldWrapper[] GetControlTypeFields(Type controlType)
	{
		List<FieldWrapper> list = new List<FieldWrapper>();
		FieldInfo[] fields = controlType.GetFields();
		int i = 0;
		for (int num = fields.Length; i < num; i++)
		{
			object[] customAttributes = fields[i].GetCustomAttributes(inherit: true);
			int j = 0;
			for (int num2 = customAttributes.Length; j < num2; j++)
			{
				if (customAttributes[j] is SettingsValue)
				{
					list.Add(new FieldWrapper(fields[i], customAttributes[j] as SettingsValue));
					break;
				}
			}
		}
		return list.ToArray();
	}
}

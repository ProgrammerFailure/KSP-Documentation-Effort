using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace KSP.UI.Screens.Settings;

[Serializable]
[XmlRoot("SettingsSetup")]
public class SettingsSetup
{
	[Serializable]
	public class MainTab
	{
		[XmlAttribute("Name")]
		public string name;

		[XmlArrayItem("SubTab")]
		[XmlArray("SubTabs")]
		public SubTab[] subTabs;

		[XmlElement("Window")]
		public Window window;

		[XmlAttribute("Platform")]
		[UI_Enum(typeof(SettingsPlatform.Platform), 0)]
		public SettingsPlatform.Platform platform;

		[UI_Enum(typeof(SettingsExpansion.Expansion), 0)]
		[XmlAttribute("Expansion")]
		public SettingsExpansion.Expansion expansion;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public MainTab()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool IsValid()
		{
			throw null;
		}
	}

	[Serializable]
	public class SubTab
	{
		[XmlAttribute("Name")]
		public string name;

		[XmlElement("Window")]
		public Window window;

		[UI_Enum(typeof(SettingsPlatform.Platform), 0)]
		[XmlAttribute("Platform")]
		public SettingsPlatform.Platform platform;

		[XmlAttribute("Expansion")]
		[UI_Enum(typeof(SettingsExpansion.Expansion), 0)]
		public SettingsExpansion.Expansion expansion;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public SubTab()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool IsValid()
		{
			throw null;
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
		public WindowValue[] values;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Window()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool IsValid()
		{
			throw null;
		}
	}

	[Serializable]
	public class WindowValue
	{
		[XmlAttribute("Name")]
		public string name;

		[XmlAttribute("Value")]
		public string value;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public WindowValue()
		{
			throw null;
		}
	}

	public class FieldWrapper
	{
		public FieldInfo field;

		public object defaultValue;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public FieldWrapper(FieldInfo field, SettingsValue attr)
		{
			throw null;
		}
	}

	[XmlArray("MainTabs")]
	[XmlArrayItem("MainTab")]
	public MainTab[] tabs;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SettingsSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string StringArrayToString(string[] array)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string[] StringToStringArray(string str)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string IntArrayToString(int[] array)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int[] StringToIntArray(string str)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string FloatArrayToString(float[] array)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float[] StringToFloatArray(string str)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetupReflectionValues(object instance, WindowValue[] values)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static FieldWrapper GetField(FieldWrapper[] wrappers, WindowValue value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static FieldWrapper[] GetControlTypeFields(Type controlType)
	{
		throw null;
	}
}

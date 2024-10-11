using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using KSP.UI.Screens.Settings.Controls;
using UnityEngine;

namespace KSP.UI.Screens.Settings;

public class ReflectedSettingsWindow : SettingsWindow
{
	[Serializable]
	public class TabWrapper
	{
		[XmlAttribute("Name")]
		public string name;

		[XmlAttribute("Platform")]
		[UI_Enum(typeof(SettingsPlatform.Platform), 0)]
		public SettingsPlatform.Platform platform;

		[XmlAttribute("Expansion")]
		[UI_Enum(typeof(SettingsExpansion.Expansion), 0)]
		public SettingsExpansion.Expansion expansion;

		[XmlArrayItem("Screen")]
		[XmlArray("Screens")]
		public SubTabWrapper[] tabs;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public TabWrapper()
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
	public class SubTabWrapper
	{
		[XmlAttribute("Name")]
		public string name;

		[XmlAttribute("Platform")]
		[UI_Enum(typeof(SettingsPlatform.Platform), 0)]
		public SettingsPlatform.Platform platform;

		[UI_Enum(typeof(SettingsExpansion.Expansion), 0)]
		[XmlAttribute("Expansion")]
		public SettingsExpansion.Expansion expansion;

		[XmlArray("Controls")]
		[XmlArrayItem("Control")]
		public ControlWrapper[] controls;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public SubTabWrapper()
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
	public class ControlWrapper
	{
		[XmlAttribute("Name")]
		public string name;

		[XmlAttribute("Platform")]
		[UI_Enum(typeof(SettingsPlatform.Platform), 0)]
		public SettingsPlatform.Platform platform;

		[XmlAttribute("Expansion")]
		[UI_Enum(typeof(SettingsExpansion.Expansion), 0)]
		public SettingsExpansion.Expansion expansion;

		[XmlAttribute("Setting")]
		public string settingName;

		[XmlIgnore]
		public SettingsControlBase prefab;

		[XmlAttribute("Prefab")]
		public string prefabPath;

		[XmlArrayItem("V")]
		[XmlArray("Values")]
		public List<ValueWrapper> values;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ControlWrapper()
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
	public class ValueWrapper
	{
		[XmlAttribute("Name")]
		public string name;

		[XmlAttribute("Value")]
		public string value;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ValueWrapper()
		{
			throw null;
		}
	}

	public TabWrapper[] tabs;

	public ReflectedSettingsWindowTab subTabPrefab;

	public string xmlPath;

	private List<GameObject> spawnedObjects;

	[SettingsValue("Default")]
	public string settingsTab;

	[SettingsValue("Default")]
	public string settingsSubTab;

	private SettingsTemplateDualLayouts dualTemplate;

	public List<GameObject> SpawnedObjects
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ReflectedSettingsWindow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Setup(TabWrapper tab)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Setup(ReflectedSettingsWindowTab tab, SubTabWrapper subTab)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetupReflectionValues(object instance, List<ValueWrapper> values)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static SettingsSetup.FieldWrapper GetField(SettingsSetup.FieldWrapper[] wrappers, ValueWrapper value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private RectTransform GetTabParent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnOpenWindow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCloseWindow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsValid()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool IsValidSecondary()
	{
		throw null;
	}
}

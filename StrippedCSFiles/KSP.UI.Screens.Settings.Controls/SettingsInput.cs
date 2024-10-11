using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using UnityEngine;

namespace KSP.UI.Screens.Settings.Controls;

public class SettingsInput : SettingsWindow
{
	[Serializable]
	public class InputSettings
	{
		[XmlArray("Input Tabs")]
		[XmlArrayItem("Tab")]
		public InputTab[] tabs;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public InputSettings()
		{
			throw null;
		}
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

		[MethodImpl(MethodImplOptions.NoInlining)]
		public InputTab()
		{
			throw null;
		}
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

		[MethodImpl(MethodImplOptions.NoInlining)]
		public InputGroup()
		{
			throw null;
		}
	}

	[Serializable]
	public class InputSetting
	{
		[XmlAttribute("Name")]
		public string name;

		[XmlAttribute("Setting")]
		public string settingName;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public InputSetting()
		{
			throw null;
		}
	}

	public InputSettings inputSettings;

	public string xmlPath;

	public SettingsInputGroupTitle groupTitlePrefab;

	public SettingsInputKey keyPrefab;

	public SettingsInputAxis axisPrefab;

	public SettingsInputBinding bindingWindowPrefab;

	[SettingsValue("Default")]
	public string settingsTab;

	private List<GameObject> spawnedObjects;

	public RectTransform keyLayoutGroup
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public RectTransform axisLayoutGroup
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SettingsInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Load XML")]
	public void LoadFile()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Save XML")]
	public void SaveFile()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnBindingRequest(object binding, string name, SettingsInputBinding.BindingType type, SettingsInputBinding.BindingVariant variant)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnBindingRequestModes(object binding, string name, SettingsInputBinding.BindingType type, SettingsInputBinding.BindingVariant variant)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnBindingRequestFinished(bool bindingChanged)
	{
		throw null;
	}
}

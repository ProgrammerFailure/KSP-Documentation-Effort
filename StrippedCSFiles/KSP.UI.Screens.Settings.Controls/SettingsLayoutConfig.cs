using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace KSP.UI.Screens.Settings.Controls;

public class SettingsLayoutConfig : SettingsWindow
{
	public SettingsInputGroupTitle groupTitlePrefab;

	public SettingsKeyboardLayoutOs keyboardLayoutOsPrefab;

	public SettingsKeyboardLayoutInput keyboardLayoutInputPrefab;

	public SettingsKeyboardLayoutApply keyboardLayoutApplyPrefab;

	public GameObject rebindPopUp;

	public TextMeshProUGUI rebindPopUpDescription;

	public GameObject layoutPopUp;

	public TextMeshProUGUI layoutPopUpDescription;

	private SettingsKeyboardLayoutInput keyboardLayoutInput;

	private ConfigNode currentLayout;

	private Action<bool> callback;

	protected static SettingsLayoutConfig Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public static ConfigNode LayoutConfig
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SettingsLayoutConfig()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SaveChanges()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void OnRevert()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnApply()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ShowLayoutChangePopUp(Action<bool> popUpCallback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnLayoutChange()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnRebindResult(bool result)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnLayoutResult(bool result)
	{
		throw null;
	}
}

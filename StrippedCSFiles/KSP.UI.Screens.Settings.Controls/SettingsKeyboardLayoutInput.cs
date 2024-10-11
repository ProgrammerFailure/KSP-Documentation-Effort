using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine.UI;

namespace KSP.UI.Screens.Settings.Controls;

public class SettingsKeyboardLayoutInput : SettingsControlBase
{
	public TextMeshProUGUI layoutName;

	public Button nextButton;

	public Button prevButton;

	private int currentLayout;

	private Dictionary<string, ConfigNode> layoutCollection;

	private List<string> layoutNames;

	private ConfigNode layoutConfig;

	private string selectedLayout;

	protected static bool layoutChangeWarning;

	protected SettingsLayoutConfig settingsLayoutConfig;

	public ConfigNode LayoutConfig
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string SelectedLayout
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SettingsKeyboardLayoutInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static SettingsKeyboardLayoutInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Init(SettingsLayoutConfig config)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadCurrentLayout()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetKeysLayout()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnApply()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnRevert()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CleanLayout()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ChangeLayout()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnNext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPrev()
	{
		throw null;
	}
}

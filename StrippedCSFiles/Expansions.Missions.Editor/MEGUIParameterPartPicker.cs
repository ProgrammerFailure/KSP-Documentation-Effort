using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[MEGUI_PartPicker]
public class MEGUIParameterPartPicker : MEGUIParameter, IMEHistoryTarget
{
	public TextMeshProUGUI selectedPartsText;

	public Button openPartPickerButton;

	public MEPartSelectorBrowser partSelectorPrefab;

	protected GAPPartPicker gapPartPicker;

	private MethodInfo getExcludedParts;

	private MethodInfo updatePartnerExcludedPartsFilter;

	public List<string> FieldValue
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public Dictionary<string, List<string>> ExcludedParts
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Color SelectedPartsColor
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string DialogTitle
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameterPartPicker()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Setup(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void ResetDefaultValue(string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void RefreshUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnPartPickerOpen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void DisplayGAP()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnPartSelectionChange(AvailablePart part, bool status)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateDisplayedParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override ConfigNode GetState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnHistoryValueChange(ConfigNode data, HistoryType type)
	{
		throw null;
	}
}

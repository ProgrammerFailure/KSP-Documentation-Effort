using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class MEGUIFooterAdditionalButton : MEGUIParameter
{
	public TMP_Dropdown additionalParametrsDropdown;

	public TMP_Dropdown removeParametersDropdown;

	protected BaseAPFieldList fieldList;

	private List<MEGUIDropDownItem> addList;

	private List<MEGUIDropDownItem> removeList;

	public override bool IsInteractable
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIFooterAdditionalButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameter Create(BaseAPFieldList fieldList, Transform parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Setup(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAdditionalValueChanged(int value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRemovevalueChanged(int value)
	{
		throw null;
	}
}

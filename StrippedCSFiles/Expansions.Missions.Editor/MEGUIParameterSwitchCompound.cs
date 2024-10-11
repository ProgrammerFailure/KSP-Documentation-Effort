using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Expansions.Missions.Editor;

[MEGUI_ParameterSwitchCompound]
public class MEGUIParameterSwitchCompound : MEGUICompoundParameter
{
	private MEGUIParameterDropdownList dropDownSelector;

	private BaseAPField selectorEnum;

	private string activeControlName;

	private Enum listenum;

	private List<string> valuesToExclude;

	private List<string> activeControlNames;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameterSwitchCompound()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Setup(string name, object value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void OnControlToDisplaySet(int ddlIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Display()
	{
		throw null;
	}
}

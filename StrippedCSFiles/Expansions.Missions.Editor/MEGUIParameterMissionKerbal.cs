using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions.Editor;

[MEGUI_MissionKerbal]
public class MEGUIParameterMissionKerbal : MEGUICompoundParameter
{
	private GAPPrefabDisplay gapKerbal;

	private GameObject prefabToDisplay;

	private MEGUIParameterDropdownList dropDownKerbals;

	private MEGUIParameterDropdownList dropDownKerbalType;

	public MissionKerbal FieldValue
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
	public MEGUIParameterMissionKerbal()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Setup(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Display()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnParameterValueChanged(int value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GameObject SetGAPKerbal()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void DisplayGAP()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshGapPrefab()
	{
		throw null;
	}
}

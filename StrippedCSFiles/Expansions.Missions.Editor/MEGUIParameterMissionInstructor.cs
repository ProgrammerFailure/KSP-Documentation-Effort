using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions.Editor;

[MEGUI_MissionInstructor]
public class MEGUIParameterMissionInstructor : MEGUICompoundParameter
{
	private GAPPrefabDisplay gapKerbal;

	private GameObject prefabToDisplay;

	private MEGUIParameterDropdownList dropDownInstructors;

	private MEGUIParameterCheckbox checkboxVintageSuit;

	private bool duringSetup;

	public MissionInstructor FieldValue
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
	public MEGUIParameterMissionInstructor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Setup(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnParameterValueChanged(int value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVintageSuitChanged(bool value)
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

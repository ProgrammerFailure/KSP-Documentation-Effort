using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;
using UnityEngine;

namespace Expansions.Missions;

[Serializable]
public class Comet : IConfigNode
{
	[MEGUI_InputField(tabStop = true, order = 1, guiName = "#autoLOC_8100112")]
	public string name;

	public uint seed;

	public float radius;

	[MEGUI_Dropdown(onDropDownValueChange = "OnCometTypeChanged", order = 2, SetDropDownItems = "SetCometTypes", addDefaultOption = false, gapDisplay = true, guiName = "#autoLOC_8100113")]
	public string cometType;

	public string cometDisplayType;

	[MEGUI_Dropdown(order = 3, gapDisplay = true, guiName = "#autoLOC_8100114")]
	public UntrackedObjectClass cometClass;

	public uint persistentId;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Comet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Randomize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static uint GetRandomCometSeed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private GameObject SetGAPComet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGAPPrefabInstantiated(GameObject prefabInstance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCometTypeChanged(MEGUIParameterDropdownList sender, int newIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<MEGUIDropDownItem> SetCometTypes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}
}

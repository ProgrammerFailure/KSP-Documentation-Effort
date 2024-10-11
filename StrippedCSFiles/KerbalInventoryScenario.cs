using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[KSPScenario((ScenarioCreationOptions)3198, new GameScenes[]
{
	GameScenes.FLIGHT,
	GameScenes.TRACKSTATION,
	GameScenes.SPACECENTER,
	GameScenes.MISSIONBUILDER,
	GameScenes.EDITOR
})]
public class KerbalInventoryScenario : ScenarioModule
{
	private Dictionary<string, ModuleInventoryPart> kerbalInventories;

	private static GameObject kerbalInventoryGameObject;

	public static KerbalInventoryScenario Instance
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

	internal static GameObject KerbalInventoryGameObject
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerbalInventoryScenario()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnKerbalNameChanged(ProtoCrewMember crew, string prevName, string newName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddKerbalInventoryInstance(string crewName, ModuleInventoryPart inventory)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ContainsCrew(string crewName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveKerbalInventoryInstance(string crewName)
	{
		throw null;
	}
}

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;
using UnityEngine;

namespace Expansions.Missions;

[Serializable]
public class Asteroid : IConfigNode
{
	public enum AsteroidType
	{
		[Description("#autoLOC_8100110")]
		Regular,
		[Description("#autoLOC_8100111")]
		Glimmeroid
	}

	[MEGUI_InputField(tabStop = true, order = 1, guiName = "#autoLOC_8100112")]
	public string name;

	public uint seed;

	[MEGUI_Dropdown(order = 2, gapDisplay = true, guiName = "#autoLOC_8100113")]
	public AsteroidType asteroidType;

	[MEGUI_Dropdown(order = 3, gapDisplay = true, guiName = "#autoLOC_8100114")]
	public UntrackedObjectClass asteroidClass;

	public uint persistentId;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Asteroid()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Randomize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private GameObject SetGAPAsteroid()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGAPPrefabInstantiated(GameObject prefabInstance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static uint GetAsteroidSeed()
	{
		throw null;
	}
}

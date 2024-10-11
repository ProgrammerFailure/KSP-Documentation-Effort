using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class ShipTemplate
{
	public string shipName;

	public string shipDescription;

	public string filename;

	public int shipType;

	public int partCount;

	public bool isControllable;

	public ConfigNode config;

	public ConfigNode rootPartNode;

	public bool shipPartsUnlocked;

	public bool shipPartsExperimental;

	public bool duplicatedParts;

	public float totalCost;

	public float fuelCost;

	public float dryCost;

	public float totalMass;

	public int stageCount;

	public Vector3 shipSize;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ShipTemplate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetShipSize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadShip(string filename, ConfigNode root)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadShip(ConfigNode root)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetPartName(ConfigNode partNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetPartId(ConfigNode partNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetIfControllable()
	{
		throw null;
	}
}

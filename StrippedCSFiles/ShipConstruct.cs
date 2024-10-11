using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RUI.Algorithms;
using UnityEngine;

[Serializable]
public class ShipConstruct : IEnumerable<Part>, IEnumerable, IShipconstruct
{
	public static int lastCompatibleMajor;

	public static int lastCompatibleMinor;

	public static int lastCompatibleRev;

	public string shipName;

	public string shipDescription;

	public uint persistentId;

	public Quaternion rotation;

	private string highestPriorityName;

	public bool[] OverrideDefault;

	public KSPActionGroup[] OverrideActionControl;

	public KSPAxisGroup[] OverrideAxisControl;

	public string[] OverrideGroupNames;

	public EditorFacility shipFacility;

	public bool shipPartsUnlocked;

	public Vector3 shipSize;

	public VesselDeltaV vesselDeltaV;

	[NonSerialized]
	public PartSet resourcePartSet;

	private HashSet<Part> cachedResourcePartSetParts;

	public ulong steamPublishedFileId;

	public string missionFlag;

	public List<Part> parts;

	internal Part vesselNamedBy;

	internal VesselType vesselType;

	public List<Part> Parts
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Part this[int index]
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

	public int Count
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ShipConstruct()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ShipConstruct(EditorFacility facility)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ShipConstruct(string shipName, EditorFacility facility, List<Part> parts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ShipConstruct(string shipName, string shipDescription, Part rootPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ShipConstruct(string shipName, string shipDescription, List<Part> parts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ShipConstruct()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode SaveShip()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool LoadShip(ConfigNode root, uint persistentID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool LoadShip(ConfigNode root, uint persistentID, bool returnErrors, out string errorString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool LoadShip(ConfigNode root)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AreAllPartsConnected()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("because it would only work in flight. In the editor you have to check the manifest for isControlSource parts. If these parts have ModuleCommand check if it has enough crew through the manifest.")]
	public bool isControllable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Clear()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Add(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Remove(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Contains(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public IEnumerator<Part> GetEnumerator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddToConstruct(Part rootPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetShipCosts(out float dryCost, out float fuelCost)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetShipCosts(out float dryCost, out float fuelCost, VesselCrewManifest vcm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetShipMass(out float dryMass, out float fuelMass)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetShipMass(out float dryMass, out float fuelMass, VesselCrewManifest vcm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetTotalMass()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool UpdateVesselNaming(bool noGameEvent = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void RunVesselNamingUpdates(Part pNewName, bool noGameEvent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GetConnectedResourceTotals(int id, bool simulate, out double amount, out double maxAmount, bool pulling = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double RequestResource(Part part, int id, double demand, bool usePriority, bool simulate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateResourceSets()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void UpdateResourceSets(List<Part> inputParts, SCCFlowGraph sccGraph)
	{
		throw null;
	}
}

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RUI.Algorithms;
using UnityEngine;

[Serializable]
public class PartSet
{
	public class ResourcePrioritySet
	{
		public List<List<PartResource>> lists;

		public HashSet<PartResource> set;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ResourcePrioritySet()
		{
			throw null;
		}
	}

	[Flags]
	public enum PartBuildSetOptions
	{
		Real = 1,
		Simulate = 2,
		Both = 3
	}

	public static int _id;

	public int setId;

	protected HashSet<Part> targetParts;

	protected Vessel vessel;

	[NonSerialized]
	protected ShipConstruct ship;

	[SerializeField]
	protected bool vesselWide;

	[SerializeField]
	protected bool simulationSet;

	[SerializeField]
	protected Dictionary<int, ResourcePrioritySet> pullList;

	[SerializeField]
	protected Dictionary<int, ResourcePrioritySet> pushList;

	protected static Dictionary<HashSet<Part>, PartSet> setsHolder;

	public static int NextID
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool ListsExist
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartSet(HashSet<Part> parts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartSet(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartSet(PartSet set)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartSet(ShipConstruct shipRef)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartSet(ShipConstruct shipRef, HashSet<Part> newParts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static PartSet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void GetConnectedResourceTotals(int id, out double amount, out double maxAmount, bool pulling)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void GetConnectedResourceTotals(int id, out double amount, out double maxAmount, bool pulling, bool simulate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void GetConnectedResourceTotals(int id, out double amount, out double maxAmount, double threshold, bool pulling)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void GetConnectedResourceTotals(int id, out double amount, out double maxAmount, double threshold, bool pulling, bool simulate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double RequestResource(Part part, int id, double demand, bool usePri)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double RequestResource(Part part, int id, double demand, bool usePri, bool simulate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected ResourcePrioritySet GetOrCreateList(int id, bool pulling)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ResourcePrioritySet GetResourceList(int id, bool pulling, bool simulate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected ResourcePrioritySet GetOrCreateList(int id, bool pulling, bool simulate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected double ProcessRequest(Part part, ResourcePrioritySet resList, double demand, bool usePri, bool pulling)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected double ProcessRequest(Part part, ResourcePrioritySet resList, double demand, bool usePri, bool pulling, bool simulate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected ResourcePrioritySet BuildResList(int id, bool pull)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected ResourcePrioritySet BuildResList(int id, bool pull, bool simulate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void AddPartResource(PartResource r, Dictionary<int, ResourcePrioritySet> dict)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void RemovePartResource(PartResource r, Dictionary<int, ResourcePrioritySet> dict)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HookEvents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	~PartSet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void RebuildVessel(Vessel newVessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void RebuildVessel(ShipConstruct newShip)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void RebuildVessel(ShipConstruct newShip, HashSet<Part> newParts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void RebuildParts(HashSet<Part> newParts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void RemovePart(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void RebuildInPlace()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void BuildPartSets(List<Part> parts, Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void BuildPartSets(List<Part> parts, Vessel v, bool simulate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void BuildPartSets(List<Part> parts, Vessel v, PartBuildSetOptions buildoptions)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void BuildPartSets(List<Part> parts, Vessel v, PartBuildSetOptions buildoptions, SCCFlowGraph sccGraph)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static HashSet<PartSet> BuildPartSimulationSets(List<Part> parts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static HashSet<PartSet> BuildPartSimulationSets(List<Part> parts, SCCFlowGraph sccGraph)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Use FlowGraph (i.e. BuildPartSets) instead, unless you just need one set, not the whole vessel")]
	public static void AddPartToSet(HashSet<Part> set, Part p, Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ContainsPart(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public HashSet<Part> GetParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetsEqual(HashSet<Part> set)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnFlowStateChange(GameEvents.HostedFromToAction<PartResource, bool> data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnFlowModeChange(GameEvents.HostedFromToAction<PartResource, PartResource.FlowMode> data)
	{
		throw null;
	}
}

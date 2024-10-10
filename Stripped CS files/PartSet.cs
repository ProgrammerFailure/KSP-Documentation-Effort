using System;
using System.Collections.Generic;
using ns3;
using UnityEngine;

[Serializable]
public class PartSet
{
	public class ResourcePrioritySet
	{
		public List<List<PartResource>> lists;

		public HashSet<PartResource> set;
	}

	[Flags]
	public enum PartBuildSetOptions
	{
		Real = 1,
		Simulate = 2,
		Both = 3
	}

	public static int _id = 0;

	public int setId;

	public HashSet<Part> targetParts;

	public Vessel vessel;

	[NonSerialized]
	public ShipConstruct ship;

	[SerializeField]
	public bool vesselWide;

	[SerializeField]
	public bool simulationSet;

	[SerializeField]
	public Dictionary<int, ResourcePrioritySet> pullList = new Dictionary<int, ResourcePrioritySet>();

	[SerializeField]
	public Dictionary<int, ResourcePrioritySet> pushList = new Dictionary<int, ResourcePrioritySet>();

	public static Dictionary<HashSet<Part>, PartSet> setsHolder = new Dictionary<HashSet<Part>, PartSet>();

	public static int NextID => _id++;

	public bool ListsExist
	{
		get
		{
			if (pullList != null && pushList != null)
			{
				return true;
			}
			return false;
		}
	}

	public PartSet(HashSet<Part> parts)
	{
		setId = NextID;
		targetParts = parts;
		vesselWide = false;
		HookEvents();
	}

	public PartSet(Vessel v)
	{
		setId = NextID;
		vesselWide = true;
		ship = null;
		vessel = v;
		HookEvents();
	}

	public PartSet(PartSet set)
	{
		setId = NextID;
		vesselWide = set.vesselWide;
		if (vesselWide)
		{
			vessel = set.vessel;
			ship = set.ship;
		}
		else
		{
			targetParts = new HashSet<Part>(set.targetParts);
		}
		HookEvents();
	}

	public PartSet(ShipConstruct shipRef)
	{
		setId = NextID;
		vesselWide = true;
		vessel = null;
		ship = shipRef;
		HookEvents();
	}

	public PartSet(ShipConstruct shipRef, HashSet<Part> newParts)
	{
		setId = NextID;
		vesselWide = true;
		vessel = null;
		ship = shipRef;
		if (newParts != null)
		{
			targetParts = newParts;
		}
		HookEvents();
	}

	public virtual void GetConnectedResourceTotals(int id, out double amount, out double maxAmount, bool pulling)
	{
		GetConnectedResourceTotals(id, out amount, out maxAmount, pulling, simulate: false);
	}

	public virtual void GetConnectedResourceTotals(int id, out double amount, out double maxAmount, bool pulling, bool simulate)
	{
		double num = 0.0;
		maxAmount = 0.0;
		amount = num;
		ResourcePrioritySet orCreateList = GetOrCreateList(id, pulling, simulate);
		if (orCreateList == null)
		{
			return;
		}
		int count = orCreateList.lists.Count;
		while (count-- > 0)
		{
			List<PartResource> list = orCreateList.lists[count];
			int count2 = list.Count;
			while (count2-- > 0)
			{
				PartResource partResource = list[count2];
				if (pulling)
				{
					amount += partResource.amount;
				}
				else
				{
					amount += partResource.maxAmount - partResource.amount;
				}
				maxAmount += partResource.maxAmount;
			}
		}
	}

	public virtual void GetConnectedResourceTotals(int id, out double amount, out double maxAmount, double threshold, bool pulling)
	{
		GetConnectedResourceTotals(id, out amount, out maxAmount, threshold, pulling, simulate: false);
	}

	public virtual void GetConnectedResourceTotals(int id, out double amount, out double maxAmount, double threshold, bool pulling, bool simulate)
	{
		double num = 0.0;
		maxAmount = 0.0;
		amount = num;
		ResourcePrioritySet orCreateList = GetOrCreateList(id, pulling, simulate);
		if (orCreateList == null)
		{
			return;
		}
		int count = orCreateList.lists.Count;
		while (count-- > 0)
		{
			List<PartResource> list = orCreateList.lists[count];
			int count2 = list.Count;
			while (count2-- > 0)
			{
				PartResource partResource = list[count2];
				if (pulling)
				{
					double num2 = partResource.maxAmount * threshold;
					double num3 = partResource.maxAmount * (1.0 - num2);
					if (!(num3 < 1E-09))
					{
						double num4 = partResource.amount - num2;
						if (num4 > 1E-09)
						{
							amount += num4;
						}
						maxAmount += num3;
					}
					continue;
				}
				double num5 = partResource.maxAmount * threshold;
				if (!(num5 < 1E-09))
				{
					double num6 = num5 - partResource.amount;
					if (num6 > 1E-09)
					{
						amount += num6;
					}
					maxAmount += num5;
				}
			}
		}
	}

	public virtual double RequestResource(Part part, int id, double demand, bool usePri)
	{
		return RequestResource(part, id, demand, usePri, simulate: false);
	}

	public virtual double RequestResource(Part part, int id, double demand, bool usePri, bool simulate)
	{
		bool pulling = demand > 0.0;
		return ProcessRequest(part, GetOrCreateList(id, pulling, simulate), demand, usePri, pulling, simulate);
	}

	public ResourcePrioritySet GetOrCreateList(int id, bool pulling)
	{
		return GetOrCreateList(id, pulling, simulate: false);
	}

	public ResourcePrioritySet GetResourceList(int id, bool pulling, bool simulate)
	{
		return GetOrCreateList(id, pulling, simulate);
	}

	public ResourcePrioritySet GetOrCreateList(int id, bool pulling, bool simulate)
	{
		simulationSet = simulate;
		ResourcePrioritySet value;
		if (pulling)
		{
			if (pullList == null)
			{
				Debug.Log("[PartSet]: PullList does not exist.");
				return null;
			}
			if (!pullList.TryGetValue(id, out value))
			{
				value = BuildResList(id, pull: true, simulate);
				pullList[id] = value;
			}
		}
		else
		{
			if (pushList == null)
			{
				Debug.Log("[PartSet]: PushList does not exist.");
				return null;
			}
			if (!pushList.TryGetValue(id, out value))
			{
				value = BuildResList(id, pull: false, simulate);
				pushList[id] = value;
			}
		}
		return value;
	}

	public double ProcessRequest(Part part, ResourcePrioritySet resList, double demand, bool usePri, bool pulling)
	{
		return ProcessRequest(part, resList, demand, usePri, pulling, simulate: false);
	}

	public double ProcessRequest(Part part, ResourcePrioritySet resList, double demand, bool usePri, bool pulling, bool simulate)
	{
		if (resList == null)
		{
			Debug.Log("PArtSet]: Unable to process resource request. PartSet has not been setup correctly.");
			return 0.0;
		}
		double num = demand;
		if (usePri)
		{
			int count = resList.lists.Count;
			while (count-- > 0)
			{
				List<PartResource> list = resList.lists[count];
				double num2 = 0.0;
				double num3 = num;
				double num4 = 0.0;
				int count2 = list.Count;
				while (count2-- > 0)
				{
					PartResource partResource = list[count2];
					num2 = ((!pulling) ? (num2 + (partResource.amount - partResource.maxAmount)) : (num2 + partResource.amount));
				}
				if (num2 == 0.0)
				{
					continue;
				}
				bool flag = false;
				if (pulling)
				{
					if (num2 > num)
					{
						flag = true;
					}
				}
				else if (num2 < num)
				{
					flag = true;
				}
				if (flag)
				{
					double num5 = (0.0 - num) / num2;
					int count3 = list.Count;
					while (count3-- > 0)
					{
						PartResource partResource = list[count3];
						double num6 = num5;
						num6 = ((!pulling) ? (num6 * (partResource.amount - partResource.maxAmount)) : (num6 * partResource.amount));
						num4 += partResource.part.TransferResource(partResource, num6, part, simulate);
					}
					num -= num4;
				}
				bool flag2 = true;
				if (!flag || num == num3)
				{
					num4 = 0.0;
					flag2 = false;
					int count4 = list.Count;
					while (count4-- > 0)
					{
						PartResource partResource = list[count4];
						num4 += partResource.part.TransferResource(partResource, 0.0 - demand, part, simulate);
						double num7 = num - num4;
						if (pulling)
						{
							if (num7 <= part.resourceRequestRemainingThreshold)
							{
								return demand - num7;
							}
						}
						else if (!(num7 < 0.0 - part.resourceRequestRemainingThreshold))
						{
							return demand - num7;
						}
					}
					num -= num4;
				}
				if (!flag2)
				{
					continue;
				}
				if (pulling)
				{
					if (num <= part.resourceRequestRemainingThreshold)
					{
						return demand - num;
					}
				}
				else if (!(num < 0.0 - part.resourceRequestRemainingThreshold))
				{
					return demand - num;
				}
			}
		}
		else
		{
			double num8 = 0.0;
			int count5 = resList.lists.Count;
			while (count5-- > 0)
			{
				List<PartResource> list = resList.lists[count5];
				int count6 = list.Count;
				while (count6-- > 0)
				{
					PartResource partResource = list[count6];
					num8 = ((!pulling) ? (num8 + (partResource.amount - partResource.maxAmount)) : (num8 + partResource.amount));
				}
			}
			if (num8 == 0.0)
			{
				return 0.0;
			}
			bool flag3 = false;
			if (pulling)
			{
				if (num8 > num)
				{
					flag3 = true;
				}
			}
			else if (num8 < num)
			{
				flag3 = true;
			}
			double num9 = num;
			double num10 = 0.0;
			if (flag3)
			{
				double num11 = (0.0 - num) / num8;
				int count7 = resList.lists.Count;
				while (count7-- > 0)
				{
					List<PartResource> list = resList.lists[count7];
					int count8 = list.Count;
					while (count8-- > 0)
					{
						PartResource partResource = list[count8];
						double num12 = num11;
						num12 = ((!pulling) ? (num12 * (partResource.amount - partResource.maxAmount)) : (num12 * partResource.amount));
						num10 += partResource.part.TransferResource(partResource, num12, part, simulate);
					}
				}
				num -= num10;
			}
			if (!flag3 || num == num9)
			{
				num10 = 0.0;
				int count9 = resList.lists.Count;
				while (count9-- > 0)
				{
					List<PartResource> list = resList.lists[count9];
					int count10 = list.Count;
					while (count10-- > 0)
					{
						PartResource partResource = list[count10];
						num10 += partResource.part.TransferResource(partResource, 0.0 - demand, part, simulate);
						double num13 = num - num10;
						if (pulling)
						{
							if (num13 <= part.resourceRequestRemainingThreshold)
							{
								return demand - num13;
							}
						}
						else if (!(num13 < 0.0 - part.resourceRequestRemainingThreshold))
						{
							return demand - num13;
						}
					}
				}
				num -= num10;
			}
		}
		return demand - num;
	}

	public ResourcePrioritySet BuildResList(int id, bool pull)
	{
		return BuildResList(id, pull, simulate: false);
	}

	public ResourcePrioritySet BuildResList(int id, bool pull, bool simulate)
	{
		simulationSet = simulate;
		IEnumerator<Part> enumerator = null;
		if (simulate && targetParts != null && targetParts.Count > 0)
		{
			enumerator = targetParts.GetEnumerator();
		}
		else if (vesselWide)
		{
			if (vessel != null)
			{
				enumerator = vessel.parts.GetEnumerator();
			}
			else if (ship != null)
			{
				enumerator = ship.parts.GetEnumerator();
			}
		}
		else
		{
			enumerator = targetParts.GetEnumerator();
		}
		ListDictionary<int, PartResource> listDictionary = new ListDictionary<int, PartResource>();
		ResourcePrioritySet resourcePrioritySet = new ResourcePrioritySet();
		resourcePrioritySet.lists = new List<List<PartResource>>();
		resourcePrioritySet.set = new HashSet<PartResource>();
		if (enumerator != null)
		{
			while (enumerator.MoveNext())
			{
				Part current = enumerator.Current;
				PartResource partResource = (simulate ? current.SimulationResources.Get(id) : current.Resources.Get(id));
				if (partResource != null && partResource.Flowing(pull))
				{
					int resourcePriority = current.GetResourcePriority();
					listDictionary.Add(resourcePriority, partResource);
					resourcePrioritySet.set.Add(partResource);
				}
			}
			enumerator.Dispose();
		}
		List<int> list = new List<int>(listDictionary.Keys);
		list.Sort();
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			resourcePrioritySet.lists.Add(listDictionary[list[i]]);
		}
		return resourcePrioritySet;
	}

	public void AddPartResource(PartResource r, Dictionary<int, ResourcePrioritySet> dict)
	{
		if (simulationSet && !r.simulationResource)
		{
			r.part.ResetSimulationResources();
			PartResource partResource = r.part.SimulationResources.Get(r.info.id);
			if (partResource == null)
			{
				Debug.LogFormat("[PartSet]: Failed to add Resource {0} to Simulation PartSet:{1} as corresponding Part {2}-{3} SimulationResource was not found.", r.info.id, setId, r.part.partInfo.title, r.part.persistentId);
				return;
			}
			r = partResource;
		}
		if (!dict.TryGetValue(r.info.id, out var value) || value.set.Contains(r))
		{
			return;
		}
		int resourcePriority = r.part.GetResourcePriority();
		int num = -1;
		int count = value.lists.Count;
		while (count-- > 0)
		{
			int resourcePriority2 = value.lists[count][0].part.GetResourcePriority();
			if (resourcePriority2 < resourcePriority)
			{
				break;
			}
			if (resourcePriority2 != resourcePriority)
			{
				num = count;
				continue;
			}
			value.lists[count].Add(r);
			value.set.Add(r);
			if (!simulationSet)
			{
				r.part.ResetSimulationResources();
			}
			return;
		}
		List<PartResource> list = new List<PartResource>();
		list.Add(r);
		if (num < 0)
		{
			value.lists.Add(list);
		}
		else
		{
			value.lists.Insert(num, list);
		}
		value.set.Add(r);
		if (!simulationSet)
		{
			r.part.ResetSimulationResources();
		}
	}

	public void RemovePartResource(PartResource r, Dictionary<int, ResourcePrioritySet> dict)
	{
		if (simulationSet && !r.simulationResource)
		{
			r.part.ResetSimulationResources();
			PartResource partResource = r.part.SimulationResources.Get(r.info.id);
			if (partResource == null)
			{
				Debug.LogFormat("[PartSet]: Failed to remove Resource {0} from Simulation PartSet:{1} as corresponding Part {2}-{3} SimulationResource was not found.", r.info.id, setId, r.part.partInfo.title, r.part.persistentId);
				return;
			}
			r = partResource;
		}
		if (!dict.TryGetValue(r.info.id, out var value) || !value.set.Contains(r))
		{
			return;
		}
		int resourcePriority = r.part.GetResourcePriority();
		int count = value.lists.Count;
		List<PartResource> list;
		do
		{
			if (count-- > 0)
			{
				list = value.lists[count];
				continue;
			}
			return;
		}
		while (list.Count <= 0 || list[0].part.GetResourcePriority() != resourcePriority);
		int count2 = list.Count;
		do
		{
			if (count2-- <= 0)
			{
				return;
			}
		}
		while (list[count2] != r);
		list.RemoveAt(count2);
		value.set.Remove(r);
		if (list.Count == 0)
		{
			value.lists.RemoveAt(count);
		}
		if (!simulationSet)
		{
			r.part.ResetSimulationResources();
		}
	}

	public void HookEvents()
	{
		GameEvents.onPartResourceFlowStateChange.Add(OnFlowStateChange);
		GameEvents.onPartResourceFlowModeChange.Add(OnFlowModeChange);
	}

	~PartSet()
	{
		GameEvents.onPartResourceFlowStateChange.Remove(OnFlowStateChange);
		GameEvents.onPartResourceFlowModeChange.Remove(OnFlowModeChange);
	}

	public virtual void RebuildVessel(Vessel newVessel)
	{
		vessel = newVessel;
		vesselWide = true;
		setId = NextID;
		pushList.Clear();
		pullList.Clear();
	}

	public virtual void RebuildVessel(ShipConstruct newShip)
	{
		ship = newShip;
		vesselWide = true;
		setId = NextID;
		pushList.Clear();
		pullList.Clear();
	}

	public virtual void RebuildVessel(ShipConstruct newShip, HashSet<Part> newParts)
	{
		ship = newShip;
		vesselWide = true;
		setId = NextID;
		if (newParts != null)
		{
			targetParts = newParts;
		}
		pushList.Clear();
		pullList.Clear();
	}

	public virtual void RebuildParts(HashSet<Part> newParts)
	{
		targetParts = newParts;
		setId = NextID;
		vesselWide = false;
		pushList.Clear();
		pullList.Clear();
	}

	public virtual void RemovePart(Part part)
	{
		HashSet<Part>.Enumerator enumerator = targetParts.GetEnumerator();
		HashSet<Part> hashSet = new HashSet<Part>();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.persistentId != part.persistentId)
			{
				hashSet.Add(enumerator.Current);
			}
		}
		enumerator.Dispose();
		RebuildParts(hashSet);
	}

	public virtual void RebuildInPlace()
	{
		setId = NextID;
		pushList.Clear();
		pullList.Clear();
	}

	public static void BuildPartSets(List<Part> parts, Vessel v)
	{
		BuildPartSets(parts, v, simulate: false);
	}

	public static void BuildPartSets(List<Part> parts, Vessel v, bool simulate)
	{
		BuildPartSets(parts, v, (!simulate) ? PartBuildSetOptions.Real : PartBuildSetOptions.Simulate);
	}

	public static void BuildPartSets(List<Part> parts, Vessel v, PartBuildSetOptions buildoptions)
	{
		BuildPartSets(parts, v, buildoptions, null);
	}

	public static void BuildPartSets(List<Part> parts, Vessel v, PartBuildSetOptions buildoptions, SCCFlowGraph sccGraph)
	{
		if (sccGraph == null)
		{
			sccGraph = new SCCFlowGraph(parts);
		}
		bool flag;
		if (flag = v != null)
		{
			if ((buildoptions & PartBuildSetOptions.Real) == PartBuildSetOptions.Real)
			{
				if (v.crossfeedSets == null)
				{
					v.crossfeedSets = new List<PartSet>();
				}
				else
				{
					v.crossfeedSets.Clear();
				}
			}
			if ((buildoptions & PartBuildSetOptions.Simulate) == PartBuildSetOptions.Simulate)
			{
				if (v.simulationCrossfeedSets == null)
				{
					v.simulationCrossfeedSets = new List<PartSet>();
				}
				else
				{
					v.simulationCrossfeedSets.Clear();
				}
			}
		}
		int count = parts.Count;
		while (count-- > 0)
		{
			Part part = parts[count];
			HashSet<Part> allRequests = sccGraph.GetAllRequests(part);
			if (setsHolder.TryGetValue(allRequests, out var value))
			{
				if ((buildoptions & PartBuildSetOptions.Real) == PartBuildSetOptions.Real)
				{
					value.simulationSet = false;
					part.crossfeedPartSet = value;
				}
				if ((buildoptions & PartBuildSetOptions.Simulate) == PartBuildSetOptions.Simulate)
				{
					PartSet partSet = ((buildoptions != PartBuildSetOptions.Both) ? value : new PartSet(value));
					partSet.simulationSet = true;
					part.simulationCrossfeedPartSet = partSet;
				}
				continue;
			}
			value = new PartSet(allRequests);
			if (flag)
			{
				if ((buildoptions & PartBuildSetOptions.Real) == PartBuildSetOptions.Real)
				{
					value.simulationSet = false;
					v.crossfeedSets.Add(value);
				}
				if ((buildoptions & PartBuildSetOptions.Simulate) == PartBuildSetOptions.Simulate)
				{
					PartSet partSet = ((buildoptions != PartBuildSetOptions.Both) ? value : new PartSet(value));
					partSet.simulationSet = true;
					v.simulationCrossfeedSets.Add(partSet);
				}
			}
			if ((buildoptions & PartBuildSetOptions.Real) == PartBuildSetOptions.Real)
			{
				value.simulationSet = false;
				part.crossfeedPartSet = value;
			}
			if ((buildoptions & PartBuildSetOptions.Simulate) == PartBuildSetOptions.Simulate)
			{
				PartSet partSet = ((buildoptions != PartBuildSetOptions.Both) ? value : new PartSet(value));
				partSet.simulationSet = true;
				part.simulationCrossfeedPartSet = partSet;
			}
			setsHolder.Add(allRequests, value);
		}
		setsHolder.Clear();
	}

	public static HashSet<PartSet> BuildPartSimulationSets(List<Part> parts)
	{
		return BuildPartSimulationSets(parts, null);
	}

	public static HashSet<PartSet> BuildPartSimulationSets(List<Part> parts, SCCFlowGraph sccGraph)
	{
		if (sccGraph == null)
		{
			sccGraph = new SCCFlowGraph(parts);
		}
		HashSet<PartSet> hashSet = new HashSet<PartSet>();
		int count = parts.Count;
		while (count-- > 0)
		{
			Part part = parts[count];
			HashSet<Part> allRequests = sccGraph.GetAllRequests(part);
			if (setsHolder.TryGetValue(allRequests, out var value))
			{
				hashSet.Add(value);
				part.simulationCrossfeedPartSet = value;
				value.simulationSet = true;
			}
			else
			{
				value = new PartSet(allRequests);
				value.simulationSet = true;
				hashSet.Add(value);
				part.simulationCrossfeedPartSet = value;
				setsHolder.Add(allRequests, value);
			}
		}
		setsHolder.Clear();
		return hashSet;
	}

	[Obsolete("Use FlowGraph (i.e. BuildPartSets) instead, unless you just need one set, not the whole vessel")]
	public static void AddPartToSet(HashSet<Part> set, Part p, Vessel v)
	{
		if (set.Contains(p))
		{
			return;
		}
		set.Add(p);
		int count = p.fuelLookupTargets.Count;
		if (count > 0)
		{
			int index = count;
			while (index-- > 0)
			{
				Part part = p.fuelLookupTargets[index];
				if (part != null && (part.vessel == v || HighLogic.LoadedSceneIsEditor) && ((part == p.parent && p.isAttached) || (part != p.parent && part.isAttached)))
				{
					AddPartToSet(set, part, v);
				}
			}
		}
		if (!p.fuelCrossFeed)
		{
			return;
		}
		int count2 = p.children.Count;
		while (count2-- > 0)
		{
			Part part = p.children[count2];
			if (part.srfAttachNode.attachedPart == p && part.fuelCrossFeed)
			{
				AddPartToSet(set, part, v);
			}
		}
		int count3 = p.attachNodes.Count;
		while (count3-- > 0)
		{
			AttachNode attachNode = p.attachNodes[count3];
			if (string.IsNullOrEmpty(p.NoCrossFeedNodeKey) || !attachNode.id.Contains(p.NoCrossFeedNodeKey))
			{
				Part part = attachNode.attachedPart;
				if (attachNode.ResourceXFeed && !(part == null))
				{
					AddPartToSet(set, part, v);
				}
			}
		}
		if (p.parent != null)
		{
			AttachNode attachNode2 = p.FindAttachNodeByPart(p.parent);
			if (attachNode2 != null && (string.IsNullOrEmpty(p.NoCrossFeedNodeKey) || !attachNode2.id.Contains(p.NoCrossFeedNodeKey)))
			{
				AddPartToSet(set, p.parent, v);
			}
		}
	}

	public bool ContainsPart(Part p)
	{
		if (vesselWide)
		{
			if (vessel != null)
			{
				return p.vessel == vessel;
			}
			if (ship != null)
			{
				return ship.parts.ContainsId(p);
			}
			return false;
		}
		return targetParts.ContainsId(p);
	}

	public HashSet<Part> GetParts()
	{
		if (targetParts == null)
		{
			return new HashSet<Part>();
		}
		return targetParts;
	}

	public bool SetsEqual(HashSet<Part> set)
	{
		if (vesselWide)
		{
			return false;
		}
		return targetParts.DeepCompare(set);
	}

	public void OnFlowStateChange(GameEvents.HostedFromToAction<PartResource, bool> data)
	{
		PartResource host = data.host;
		Part part = host.part;
		if ((!vesselWide || !(part.vessel == vessel)) && (vesselWide || !targetParts.Contains(part)))
		{
			return;
		}
		if (data.from)
		{
			RemovePartResource(host, pullList);
			RemovePartResource(host, pushList);
			return;
		}
		if (host.Flowing(pulling: true))
		{
			AddPartResource(host, pullList);
		}
		if (host.Flowing(pulling: false))
		{
			AddPartResource(host, pushList);
		}
	}

	public void OnFlowModeChange(GameEvents.HostedFromToAction<PartResource, PartResource.FlowMode> data)
	{
		PartResource host = data.host;
		Part part = host.part;
		if ((!vesselWide || !(part.vessel == vessel)) && (vesselWide || !targetParts.Contains(part)))
		{
			return;
		}
		switch (data.from)
		{
		case PartResource.FlowMode.None:
			switch (data.to)
			{
			default:
				if (host.Flowing(pulling: true))
				{
					AddPartResource(host, pullList);
				}
				if (host.Flowing(pulling: false))
				{
					AddPartResource(host, pushList);
				}
				break;
			case PartResource.FlowMode.In:
				if (host.Flowing(pulling: false))
				{
					AddPartResource(host, pushList);
				}
				break;
			case PartResource.FlowMode.Out:
				if (host.Flowing(pulling: true))
				{
					AddPartResource(host, pullList);
				}
				break;
			}
			return;
		case PartResource.FlowMode.In:
			switch (data.to)
			{
			case PartResource.FlowMode.Both:
				if (host.Flowing(pulling: true))
				{
					AddPartResource(host, pullList);
				}
				break;
			default:
				if (host.Flowing(pulling: true))
				{
					AddPartResource(host, pullList);
				}
				RemovePartResource(host, pushList);
				break;
			case PartResource.FlowMode.None:
				RemovePartResource(host, pushList);
				break;
			}
			return;
		case PartResource.FlowMode.Both:
			switch (data.to)
			{
			default:
				RemovePartResource(host, pushList);
				RemovePartResource(host, pullList);
				break;
			case PartResource.FlowMode.In:
				RemovePartResource(host, pullList);
				break;
			case PartResource.FlowMode.Out:
				RemovePartResource(host, pushList);
				break;
			}
			return;
		}
		switch (data.to)
		{
		case PartResource.FlowMode.Both:
			if (host.Flowing(pulling: false))
			{
				AddPartResource(host, pushList);
			}
			return;
		case PartResource.FlowMode.None:
			RemovePartResource(host, pullList);
			return;
		}
		if (host.Flowing(pulling: false))
		{
			AddPartResource(host, pushList);
		}
		RemovePartResource(host, pullList);
	}
}

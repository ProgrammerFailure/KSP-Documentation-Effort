using System;
using System.Collections.Generic;
using System.Text;
using CommNet.Network;

namespace CommNet;

public class CommNetwork : Net<CommNetwork, CommNode, CommLink, CommPath>
{
	public bool isDirty;

	public bool IsDirty => isDirty;

	public override void Rebuild()
	{
		isDirty = false;
		base.Rebuild();
	}

	public virtual bool FindHome(CommNode from, CommPath path = null)
	{
		if (FindClosestWhere(from, path, IsHome) != null)
		{
			return true;
		}
		return false;
	}

	public virtual bool FindClosestControlSource(CommNode from, CommPath path = null)
	{
		if (FindClosestWhere(from, path, IsControlSource) != null)
		{
			return true;
		}
		return false;
	}

	public override bool FindPath(CommNode start, CommPath path, CommNode end)
	{
		if (path != null)
		{
			path.Clear();
		}
		if (end == null)
		{
			return false;
		}
		if (start == end)
		{
			return false;
		}
		if (isDirty)
		{
			Rebuild();
		}
		CreateShortestPathTree(start, end);
		CommNode commNode = end;
		if (commNode.bestLinkNode != null)
		{
			while (commNode != start)
			{
				CommNode b = commNode;
				CommLink bestLink = commNode.bestLink;
				commNode = commNode.bestLinkNode;
				CommLink commLink = new CommLink();
				commLink.Set(commNode, b, bestLink.signalStrength, bestLink.GetSignalStrength(start));
				path.Insert(0, commLink);
			}
		}
		if (path.Count == 0)
		{
			return false;
		}
		if (!path[0].Contains(start))
		{
			path.Clear();
			return false;
		}
		path.UpdateFromPath();
		return true;
	}

	public override CommNode FindClosestWhere(CommNode start, CommPath path, Func<CommNode, CommNode, bool> where)
	{
		if (path != null)
		{
			path.Clear();
		}
		if (start != null && where != null)
		{
			if (isDirty)
			{
				Rebuild();
			}
			double num = double.MinValue;
			CommNode commNode = null;
			IncrementPathingID();
			int count = nodes.Count;
			CommNode commNode2;
			while (count-- > 0)
			{
				commNode2 = base[count];
				commNode2.bestCost = double.MinValue;
				commNode2.bestLink = null;
				commNode2.bestLinkNode = null;
				commNode2.isInCandidateList = false;
			}
			candidates.Clear();
			candidates.Enqueue(start);
			start.bestCost = 1.0;
			start.isInCandidateList = true;
			start.pathingID = base.pathingID;
			while (candidates.Count > 0)
			{
				commNode2 = candidates.Dequeue();
				commNode2.isInCandidateList = false;
				nodeLinkEnum = commNode2.GetEnumerator();
				while (nodeLinkEnum.MoveNext())
				{
					nodeLink = nodeLinkEnum.Current;
					CommLink value = nodeLink.Value;
					if (value.pathingID != base.pathingID)
					{
						value.pathingID = base.pathingID;
						CommNode commNode3 = UpdateShortestWhere(commNode2, nodeLink.Key, value, commNode2.bestCost, start, where);
						if (commNode3 != null && commNode3.bestCost > num)
						{
							commNode = commNode3;
							num = commNode3.bestCost;
						}
					}
				}
			}
			if (commNode == null)
			{
				return null;
			}
			commNode2 = commNode;
			if (path != null)
			{
				path.Clear();
				if (commNode2.bestLinkNode != null)
				{
					while (commNode2 != start)
					{
						CommNode b = commNode2;
						CommLink value = commNode2.bestLink;
						commNode2 = commNode2.bestLinkNode;
						CommLink commLink = new CommLink();
						commLink.Set(commNode2, b, value.signalStrength, value.GetSignalStrength(start));
						path.Insert(0, commLink);
					}
				}
				if (!path[0].Contains(start))
				{
					path.Clear();
					return null;
				}
				path.UpdateFromPath();
				return commNode;
			}
			return commNode;
		}
		return null;
	}

	public override CommNode Add(CommNode conn)
	{
		if (base.Add(conn) == null)
		{
			return null;
		}
		isDirty = true;
		return conn;
	}

	public override bool Remove(CommNode conn)
	{
		if (!base.Remove(conn))
		{
			return false;
		}
		isDirty = true;
		return true;
	}

	public virtual bool TryConnect(CommNode a, CommNode b, double distance, bool aCanRelay, bool bCanRelay, bool bothRelay)
	{
		bool flag = false;
		double num = a.GetSignalStrengthMultiplier(b) * b.GetSignalStrengthMultiplier(a);
		double num2;
		if (bothRelay)
		{
			double normalizedRange = CommNetScenario.RangeModel.GetNormalizedRange(a.antennaRelay.power, b.antennaRelay.power, distance);
			if (normalizedRange > 0.0)
			{
				num2 = Math.Sqrt(a.antennaRelay.rangeCurve.Evaluate(normalizedRange) * b.antennaRelay.rangeCurve.Evaluate(normalizedRange));
				num2 *= num;
				if (num2 > 0.0)
				{
					flag = true;
				}
			}
			else
			{
				bothRelay = false;
				num2 = 0.0;
			}
		}
		else
		{
			num2 = 0.0;
		}
		double num3;
		if (aCanRelay)
		{
			double normalizedRange2 = CommNetScenario.RangeModel.GetNormalizedRange(a.antennaRelay.power, b.antennaTransmit.power, distance);
			if (normalizedRange2 > 0.0)
			{
				num3 = Math.Sqrt(a.antennaRelay.rangeCurve.Evaluate(normalizedRange2) * b.antennaTransmit.rangeCurve.Evaluate(normalizedRange2));
				num3 *= num;
				if (num3 > 0.0)
				{
					flag = true;
				}
			}
			else
			{
				aCanRelay = false;
				num3 = 0.0;
			}
		}
		else
		{
			aCanRelay = false;
			num3 = 0.0;
		}
		double num4;
		if (bCanRelay)
		{
			double normalizedRange3 = CommNetScenario.RangeModel.GetNormalizedRange(a.antennaTransmit.power, b.antennaRelay.power, distance);
			if (normalizedRange3 > 0.0)
			{
				num4 = Math.Sqrt(b.antennaRelay.rangeCurve.Evaluate(normalizedRange3) * a.antennaTransmit.rangeCurve.Evaluate(normalizedRange3));
				num4 *= num;
				if (num4 > 0.0)
				{
					flag = true;
				}
			}
			else
			{
				bCanRelay = false;
				num4 = 0.0;
			}
		}
		else
		{
			bCanRelay = false;
			num4 = 0.0;
		}
		if (flag)
		{
			CommLink commLink = Connect(a, b, distance);
			commLink.strengthRR = num2;
			commLink.strengthAR = num3;
			commLink.strengthBR = num4;
			commLink.aCanRelay = aCanRelay;
			commLink.bCanRelay = bCanRelay;
			commLink.bothRelay = bothRelay;
			return true;
		}
		Disconnect(a, b);
		return false;
	}

	public override bool SetNodeConnection(CommNode a, CommNode b)
	{
		if (a.isHome && b.isHome)
		{
			Disconnect(a, b);
			return false;
		}
		if (a.antennaRelay.power + a.antennaTransmit.power != 0.0 && b.antennaRelay.power + b.antennaTransmit.power != 0.0)
		{
			Vector3d precisePosition = a.precisePosition;
			Vector3d precisePosition2 = b.precisePosition;
			double num = (precisePosition2 - precisePosition).sqrMagnitude;
			double num2 = a.distanceOffset + b.distanceOffset;
			if (num2 != 0.0)
			{
				num2 = Math.Sqrt(num) + num2;
				if (num2 > 0.0)
				{
					num = num2 * num2;
				}
				else
				{
					num2 = 0.0;
					num = 0.0;
				}
			}
			bool flag;
			bool flag2 = (flag = CommNetScenario.RangeModel.InRange(a.antennaRelay.power, b.antennaRelay.power, num));
			bool flag3 = flag;
			if (!flag)
			{
				flag2 = CommNetScenario.RangeModel.InRange(a.antennaRelay.power, b.antennaTransmit.power, num);
				flag3 = CommNetScenario.RangeModel.InRange(a.antennaTransmit.power, b.antennaRelay.power, num);
			}
			if (!flag2 && !flag3)
			{
				Disconnect(a, b);
				return false;
			}
			if (num == 0.0 && (flag || flag2 || flag3))
			{
				return TryConnect(a, b, 1E-07, flag2, flag3, flag);
			}
			if (num2 == 0.0)
			{
				num2 = Math.Sqrt(num);
			}
			if (TestOcclusion(precisePosition, a.occluder, precisePosition2, b.occluder, num2))
			{
				return TryConnect(a, b, num2, flag2, flag3, flag);
			}
			Disconnect(a, b);
			return false;
		}
		Disconnect(a, b);
		return false;
	}

	public virtual bool TestOcclusion(Vector3d aPos, Occluder a, Vector3d bPos, Occluder b, double distance)
	{
		int count = occluders.Count;
		Occluder occluder;
		do
		{
			if (count-- > 0)
			{
				occluder = occluders[count];
				continue;
			}
			return true;
		}
		while (occluder == a || occluder == b || !occluder.InRange(aPos, distance) || !occluder.Raycast(aPos, bPos));
		return false;
	}

	public static bool IsControlSource(CommNode start, CommNode a)
	{
		if (!a.isControlSourceMultiHop)
		{
			if (a.isControlSource)
			{
				return a.ContainsKey(start);
			}
			return false;
		}
		return a.isControlSource;
	}

	public static bool IsHome(CommNode start, CommNode a)
	{
		return a.isHome;
	}

	public override void CreateShortestPathTree(CommNode start, CommNode end)
	{
		IncrementPathingID();
		int count = nodes.Count;
		while (count-- > 0)
		{
			CommNode commNode = base[count];
			commNode.bestCost = double.MinValue;
			commNode.bestLinkNode = null;
			commNode.isInCandidateList = false;
			commNode.bestLink = null;
		}
		candidates.Clear();
		candidates.Enqueue(start);
		start.bestCost = 1.0;
		start.isInCandidateList = true;
		start.pathingID = base.pathingID;
		while (candidates.Count > 0)
		{
			CommNode commNode = candidates.Dequeue();
			commNode.isInCandidateList = false;
			nodeLinkEnum = commNode.GetEnumerator();
			while (nodeLinkEnum.MoveNext())
			{
				nodeLink = nodeLinkEnum.Current;
				UpdateShortestPath(commNode, nodeLink.Key, nodeLink.Value, commNode.bestCost, start, end);
			}
		}
	}

	public override void UpdateShortestPath(CommNode a, CommNode b, CommLink link, double bestCost, CommNode startNode, CommNode endNode)
	{
		double num = 0.0;
		num = link.GetSignalStrength(startNode);
		double num2 = bestCost * num;
		if (num > 0.0 && num2 > b.bestCost)
		{
			b.bestCost = num2;
			b.bestLinkNode = a;
			b.bestLink = link;
			if (!b.isInCandidateList && b.pathingID != base.pathingID)
			{
				candidates.Enqueue(b);
				b.isInCandidateList = true;
				b.pathingID = base.pathingID;
			}
		}
	}

	public override CommNode UpdateShortestWhere(CommNode a, CommNode b, CommLink link, double bestCost, CommNode startNode, Func<CommNode, CommNode, bool> whereClause)
	{
		double num = 0.0;
		num = link.GetSignalStrength(startNode);
		double num2 = bestCost * num;
		if (num > 0.0 && num2 > b.bestCost)
		{
			b.bestCost = num2;
			b.bestLinkNode = a;
			b.bestLink = link;
			if (!b.isInCandidateList && b.pathingID != base.pathingID)
			{
				candidates.Enqueue(b);
				b.isInCandidateList = true;
				b.pathingID = base.pathingID;
			}
			if (whereClause(startNode, b))
			{
				return b;
			}
			return null;
		}
		return null;
	}

	public string CreateDebug()
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < base.Count; i++)
		{
			stringBuilder.AppendLine(nodes[i].ToString());
			Dictionary<CommNode, CommLink>.ValueCollection.Enumerator enumerator = nodes[i].Values.GetEnumerator();
			while (enumerator.MoveNext())
			{
				stringBuilder.AppendLine("      " + enumerator.Current.ToString());
			}
		}
		return stringBuilder.ToString();
	}
}

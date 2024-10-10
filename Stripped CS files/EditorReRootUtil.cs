using System.Collections.Generic;
using UnityEngine;

public static class EditorReRootUtil
{
	public static List<Part> GetRootCandidates(IEnumerable<Part> fromSet)
	{
		List<Part> list = new List<Part>(fromSet);
		int count = list.Count;
		while (count-- > 0)
		{
			if (!isRootCandidate(list[count]))
			{
				list.RemoveAt(count);
			}
		}
		return list;
	}

	public static bool isRootCandidate(Part p)
	{
		if (!(p == p.localRoot) && !(p == EditorLogic.SelectedPart))
		{
			if (!p.attachRules.allowRoot)
			{
				return false;
			}
			if (!p.attachRules.allowSrfAttach && !p.attachRules.allowStack)
			{
				return false;
			}
			if (p.attachRules.srfAttach && p.srfAttachNode != null && p.srfAttachNode.attachedPart == null)
			{
				return true;
			}
			if (p.attachRules.stack)
			{
				if (EditorLogic.fetch.ship.parts.Contains(p))
				{
					return p.attachNodes.Count > 0;
				}
				int count = p.attachNodes.Count;
				do
				{
					if (count-- <= 0)
					{
						return false;
					}
				}
				while (!(p.attachNodes[count].attachedPart == null));
				return true;
			}
			return false;
		}
		return false;
	}

	public static bool MakeRoot(Part newRoot, Part oldRoot)
	{
		if (!isRootCandidate(newRoot))
		{
			Debug.LogWarning("[Editor Root Utils]: Cannot set " + newRoot.name + " as root.", newRoot.gameObject);
			return false;
		}
		if (!HierarchyUtil.isDescendant(newRoot, oldRoot))
		{
			Debug.LogWarning("[Editor Root Utils]: Cannot set " + newRoot.name + " as root as it is not a descendant in " + oldRoot.name + "'s hierarchy.", newRoot.gameObject);
			return false;
		}
		newRoot.SetHierarchyRoot(newRoot);
		StripInvalidSymmetrySets(newRoot);
		return true;
	}

	public static void StripInvalidSymmetrySets(Part setRoot)
	{
		StripInvalidSymmetrySets_recurseChildren(setRoot);
	}

	public static void StripInvalidSymmetrySets_recurseChildren(Part p)
	{
		if (!SymmetrySetIsValid(p))
		{
			int count = p.symmetryCounterparts.Count;
			while (count-- > 0)
			{
				p.symmetryCounterparts[count].symmetryCounterparts.Clear();
			}
			p.symmetryCounterparts.Clear();
		}
		int count2 = p.children.Count;
		while (count2-- > 0)
		{
			StripInvalidSymmetrySets_recurseChildren(p.children[count2]);
		}
	}

	public static bool SymmetrySetIsValid(Part p)
	{
		int num = p.symmetryCounterparts.Count - 1;
		if (num >= 0)
		{
			int hierarchyDepth = HierarchyUtil.GetHierarchyDepth(p);
			int num2 = num;
			while (num2 >= 0)
			{
				if (HierarchyUtil.GetHierarchyDepth(p.symmetryCounterparts[num2]) == hierarchyDepth)
				{
					num2--;
					continue;
				}
				return false;
			}
		}
		return true;
	}
}

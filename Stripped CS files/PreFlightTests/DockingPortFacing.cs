using System.Collections.Generic;
using ns9;
using UnityEngine;

namespace PreFlightTests;

public class DockingPortFacing : DesignConcernBase
{
	public List<Part> failedParts = new List<Part>();

	public static string cacheAutoLOC_251638;

	public static string cacheAutoLOC_251639;

	public static string cacheAutoLOC_251645;

	public static string cacheAutoLOC_251646;

	public override bool TestCondition()
	{
		failedParts.Clear();
		if (EditorLogic.RootPart != null)
		{
			RecurseHierarchy(EditorLogic.RootPart);
		}
		return failedParts.Count == 0;
	}

	public bool RecurseHierarchy(Part part)
	{
		bool flag = false;
		for (int i = 0; i < part.children.Count; i++)
		{
			if (RecurseHierarchy(part.children[i]))
			{
				flag = true;
			}
		}
		bool result = false;
		part.isDockingPort(out var dockingPort);
		ModuleCommand moduleCommand = part.FindModuleImplementing<ModuleCommand>();
		if (flag || moduleCommand != null)
		{
			result = true;
		}
		if (dockingPort != null)
		{
			AttachNode attachNode = part.FindAttachNode(dockingPort.referenceAttachNode);
			if (attachNode != null && attachNode.attachedPart == part.parent)
			{
				if (part.children.Count == 0)
				{
					AddToFailedParts(part, "---Docking  : 1");
					return false;
				}
				if (part.children.Count == 1 && part.children[0].FindModuleImplementing<IStageSeparator>() != null)
				{
					part.children[0].isDockingPort(out var dockingPort2);
					if (!(dockingPort2 != null))
					{
						AddToFailedParts(part, "---Docking  : 2b");
						return false;
					}
					if (part.children[0].FindAttachNode(dockingPort2.referenceAttachNode) != null)
					{
						AddToFailedParts(part, "---Docking  : 2");
						return false;
					}
				}
			}
			else
			{
				if (part.parent != null && part.parent.FindModuleImplementing<IStageSeparator>() != null)
				{
					part.parent.isDockingPort(out var dockingPort3);
					if (dockingPort3 != null)
					{
						if (part.parent.FindAttachNode(dockingPort3.referenceAttachNode) != null && attachNode != null)
						{
							AddToFailedParts(part, "---Docking  : 3");
							return false;
						}
					}
					else if (attachNode != null)
					{
						AddToFailedParts(part, "---Docking  : 3b");
						return false;
					}
				}
				if (part.srfAttachNode != null && part.srfAttachNode.attachedPart != null && attachNode != null)
				{
					Vector3 normalized = (part.srfAttachNode.attachedPart.transform.position - part.transform.position).normalized;
					Vector3 normalized2 = part.transform.TransformDirection(attachNode.orientation).normalized;
					if (Vector3.Angle(normalized, normalized2) < 100f)
					{
						AddToFailedParts(part, "---Docking  : 4");
						return false;
					}
				}
			}
		}
		return result;
	}

	public void AddToFailedParts(Part part, string desc)
	{
		if (!failedParts.Contains(part))
		{
			failedParts.Add(part);
		}
		else
		{
			Debug.Log(desc + " ERROR");
		}
	}

	public override string GetConcernTitle()
	{
		if (failedParts.Count == 1)
		{
			return cacheAutoLOC_251638;
		}
		return cacheAutoLOC_251639;
	}

	public override string GetConcernDescription()
	{
		if (failedParts.Count == 1)
		{
			return cacheAutoLOC_251645;
		}
		return cacheAutoLOC_251646;
	}

	public override DesignConcernSeverity GetSeverity()
	{
		return DesignConcernSeverity.CRITICAL;
	}

	public override List<Part> GetAffectedParts()
	{
		return failedParts;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_251638 = Localizer.Format("#autoLOC_251638");
		cacheAutoLOC_251639 = Localizer.Format("#autoLOC_251639");
		cacheAutoLOC_251645 = Localizer.Format("#autoLOC_251645");
		cacheAutoLOC_251646 = Localizer.Format("#autoLOC_251646");
	}
}

using System.Collections.Generic;
using ns9;
using UnityEngine;

namespace PreFlightTests;

public class DockingPortAsDecoupler : DesignConcernBase
{
	public List<Part> failedParts = new List<Part>();

	public static string cacheAutoLOC_251475;

	public static string cacheAutoLOC_251476;

	public static string cacheAutoLOC_251482;

	public static string cacheAutoLOC_251483;

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
		bool flag2 = false;
		part.isDockingPort(out var dockingPort);
		ModuleCommand moduleCommand = part.FindModuleImplementing<ModuleCommand>();
		if (flag || moduleCommand != null)
		{
			flag2 = true;
		}
		if (dockingPort != null)
		{
			AttachNode attachNode = part.FindAttachNode(dockingPort.referenceAttachNode);
			if (attachNode != null && attachNode.attachedPart == part.parent)
			{
				if (part.children.Count == 1)
				{
					if (part.children[0].FindModuleImplementing<IStageSeparator>() != null)
					{
						part.children[0].isDockingPort(out var dockingPort2);
						if (!(dockingPort2 != null))
						{
							return flag2;
						}
						if (part.children[0].FindAttachNode(dockingPort2.referenceAttachNode) != null)
						{
							return flag2;
						}
					}
					if (!flag2)
					{
						AddToFailedParts(part, "---DockingAsDecoupler  : 1");
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
							return flag2;
						}
					}
					else if (attachNode != null)
					{
						return flag2;
					}
				}
				if (attachNode != null && attachNode.attachedPart != null && !flag2)
				{
					AddToFailedParts(part, "---DockingAsDecoupler  : 2");
					return false;
				}
			}
		}
		return flag2;
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
			return cacheAutoLOC_251475;
		}
		return cacheAutoLOC_251476;
	}

	public override string GetConcernDescription()
	{
		if (failedParts.Count == 1)
		{
			return cacheAutoLOC_251482;
		}
		return cacheAutoLOC_251483;
	}

	public override DesignConcernSeverity GetSeverity()
	{
		return DesignConcernSeverity.WARNING;
	}

	public override List<Part> GetAffectedParts()
	{
		return failedParts;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_251475 = Localizer.Format("#autoLOC_251475");
		cacheAutoLOC_251476 = Localizer.Format("#autoLOC_251476");
		cacheAutoLOC_251482 = Localizer.Format("#autoLOC_251482");
		cacheAutoLOC_251483 = Localizer.Format("#autoLOC_251483");
	}
}

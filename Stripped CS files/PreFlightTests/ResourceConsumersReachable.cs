using System.Collections.Generic;
using ns11;
using ns9;
using UnityEngine;

namespace PreFlightTests;

public class ResourceConsumersReachable : DesignConcernBase
{
	public PartResourceDefinition resourceDefinition;

	public ShipConstruct ship;

	public List<Part> failedParts = new List<Part>();

	public bool exceptionCommandPod;

	public ResourceConsumersReachable(PartResourceDefinition resourceDefinition)
	{
		this.resourceDefinition = resourceDefinition;
		if (resourceDefinition.resourceFlowMode == ResourceFlowMode.NO_FLOW)
		{
			Debug.LogError("[ResourceConsumersReachable] NO-FLOW resources not allowed.");
		}
	}

	public override bool TestCondition()
	{
		ship = EditorLogic.fetch.ship;
		exceptionCommandPod = false;
		failedParts.Clear();
		if (resourceDefinition.resourceFlowMode != ResourceFlowMode.STACK_PRIORITY_SEARCH && resourceDefinition.resourceFlowMode != ResourceFlowMode.STAGE_STACK_FLOW && resourceDefinition.resourceFlowMode != ResourceFlowMode.STAGE_STACK_FLOW_BALANCE)
		{
			int count = ship.Count;
			for (int i = 0; i < count; i++)
			{
				Part part = ship[i];
				if (part.Resources.Contains(resourceDefinition.id) && part.Resources.Get(resourceDefinition.id).amount > 0.0)
				{
					if (!(resourceDefinition.name == "MonoPropellant") || !(part.FindModuleImplementing<ModuleCommand>() != null))
					{
						failedParts.Clear();
						return true;
					}
					exceptionCommandPod = true;
				}
				if (RUIutils.Any(part.Modules.GetModules<IResourceConsumer>(), (IResourceConsumer a) => a.GetConsumedResources().Contains(resourceDefinition)))
				{
					failedParts.Add(part);
				}
			}
		}
		else
		{
			failedParts = new List<Part>(EngineersReport.sccFlowGraphUCFinder.GetUnreachableFuelRequests(resourceDefinition.id));
		}
		return failedParts.Count == 0;
	}

	public override List<Part> GetAffectedParts()
	{
		return failedParts;
	}

	public override string GetConcernTitle()
	{
		if (exceptionCommandPod)
		{
			if (failedParts.Count == 1)
			{
				return Localizer.Format("#autoLOC_252942", resourceDefinition.displayName);
			}
			return Localizer.Format("#autoLOC_252943", resourceDefinition.displayName);
		}
		if (failedParts.Count == 1)
		{
			return Localizer.Format("#autoLOC_252948", resourceDefinition.displayName);
		}
		return Localizer.Format("#autoLOC_252949", resourceDefinition.displayName);
	}

	public override string GetConcernDescription()
	{
		if (exceptionCommandPod)
		{
			if (failedParts.Count == 1)
			{
				return Localizer.Format("#autoLOC_252958", resourceDefinition.displayName, resourceDefinition.displayName);
			}
			return Localizer.Format("#autoLOC_252959", failedParts.Count.ToString(), resourceDefinition.displayName, resourceDefinition.displayName);
		}
		if (failedParts.Count == 1)
		{
			return Localizer.Format("#autoLOC_252964", resourceDefinition.displayName);
		}
		return Localizer.Format("#autoLOC_252965", failedParts.Count.ToString(), resourceDefinition.displayName);
	}

	public override DesignConcernSeverity GetSeverity()
	{
		return DesignConcernSeverity.WARNING;
	}
}

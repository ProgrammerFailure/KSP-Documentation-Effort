using System.Collections.Generic;
using ns11;
using ns9;
using UnityEngine;

namespace PreFlightTests;

public class ResourceContainersReachable : DesignConcernBase
{
	public PartResourceDefinition resourceDefinition;

	public ShipConstruct ship;

	public List<Part> failedParts = new List<Part>();

	public ResourceContainersReachable(PartResourceDefinition resourceDefinition)
	{
		this.resourceDefinition = resourceDefinition;
		if (resourceDefinition.resourceFlowMode == ResourceFlowMode.NO_FLOW)
		{
			Debug.LogError("[ResourceContainersReachable] NO-FLOW resources not allowed.");
		}
	}

	public override bool TestCondition()
	{
		ship = EditorLogic.fetch.ship;
		failedParts.Clear();
		if (resourceDefinition.resourceFlowMode != ResourceFlowMode.STACK_PRIORITY_SEARCH && resourceDefinition.resourceFlowMode != ResourceFlowMode.STAGE_STACK_FLOW && resourceDefinition.resourceFlowMode != ResourceFlowMode.STAGE_STACK_FLOW_BALANCE)
		{
			int count = ship.Count;
			for (int i = 0; i < count; i++)
			{
				Part part = ship[i];
				if (part.Resources.Contains(resourceDefinition.id) && part.Resources.Get(resourceDefinition.id).amount > 0.0)
				{
					failedParts.Add(part);
				}
				if (RUIutils.Any(part.Modules.GetModules<IResourceConsumer>(), (IResourceConsumer a) => a.GetConsumedResources().Contains(resourceDefinition)))
				{
					failedParts.Clear();
					return true;
				}
			}
		}
		else
		{
			failedParts = new List<Part>(EngineersReport.sccFlowGraphUCFinder.GetUnreachableFuelDeliveries(resourceDefinition.id));
		}
		return failedParts.Count == 0;
	}

	public override List<Part> GetAffectedParts()
	{
		return failedParts;
	}

	public override string GetConcernTitle()
	{
		return Localizer.Format("#autoLOC_6001044", resourceDefinition.displayName, failedParts.Count);
	}

	public override string GetConcernDescription()
	{
		if (failedParts.Count == 1)
		{
			return Localizer.Format("#autoLOC_253062", resourceDefinition.displayName, resourceDefinition.displayName);
		}
		return Localizer.Format("#autoLOC_253063", failedParts.Count.ToString(), resourceDefinition.displayName, resourceDefinition.displayName);
	}

	public override DesignConcernSeverity GetSeverity()
	{
		return DesignConcernSeverity.NOTICE;
	}
}

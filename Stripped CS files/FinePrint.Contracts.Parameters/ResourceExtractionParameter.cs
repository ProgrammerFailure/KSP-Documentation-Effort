using System;
using System.Collections.Generic;
using System.Globalization;
using Contracts;
using FinePrint.Utilities;
using ns9;

namespace FinePrint.Contracts.Parameters;

public class ResourceExtractionParameter : ContractParameter
{
	public CelestialBody targetBody;

	public int successCounter;

	public double totalHarvested;

	public double goalHarvested;

	public string resourceName;

	public string resourceTitle;

	public List<string> modules;

	public bool eventsAdded;

	public float notifyLevel;

	public ResourceExtractionParameter()
	{
		targetBody = Planetarium.fetch.Home;
		successCounter = 0;
		totalHarvested = 0.0;
		goalHarvested = 1000.0;
		resourceName = "Matter";
		resourceTitle = "matter";
		eventsAdded = false;
		notifyLevel = 0.25f;
		modules = new List<string>();
	}

	public ResourceExtractionParameter(string resourceName, string resourceTitle, double goalHarvested, CelestialBody targetBody, List<string> modules)
	{
		this.targetBody = targetBody;
		successCounter = 0;
		totalHarvested = 0.0;
		this.goalHarvested = goalHarvested;
		this.resourceName = resourceName;
		this.resourceTitle = resourceTitle;
		eventsAdded = false;
		notifyLevel = 0.25f;
		this.modules = modules;
	}

	public override string GetHashString()
	{
		return SystemUtilities.SuperSeed(base.Root).ToString(CultureInfo.InvariantCulture) + base.String_0;
	}

	public override string GetTitle()
	{
		return Localizer.Format("#autoLOC_284531", Convert.ToDecimal(Math.Round(goalHarvested)).ToString("#,###"), resourceTitle, targetBody.displayName);
	}

	public override void OnRegister()
	{
		eventsAdded = false;
		if (base.Root.ContractState == Contract.State.Active)
		{
			GameEvents.OnResourceConverterOutput.Add(ResourcesHarvested);
			eventsAdded = true;
		}
	}

	public override void OnUnregister()
	{
		if (eventsAdded)
		{
			GameEvents.OnResourceConverterOutput.Remove(ResourcesHarvested);
		}
	}

	public override void OnReset()
	{
		SetIncomplete();
	}

	public void ResourcesHarvested(PartModule module, string resource, double amount)
	{
		if (!(resource != resourceName) && amount > 0.0 && !(module == null) && !(module.vessel == null) && !(module.vessel.mainBody == null) && modules.Contains(module.moduleName) && !(module.vessel.mainBody != targetBody))
		{
			totalHarvested += amount;
			if (totalHarvested / goalHarvested >= (double)notifyLevel)
			{
				string name = base.Root.Agent.Name;
				name += "^M";
				string text = Localizer.Format("<<g:2,1>>", name, resourceTitle);
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_284574", Math.Round(notifyLevel * 100f), text), 5f, ScreenMessageStyle.UPPER_LEFT);
				notifyLevel += 0.25f;
			}
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("targetBody", targetBody.flightGlobalsIndex);
		node.AddValue("totalHarvested", totalHarvested);
		node.AddValue("goalHarvested", goalHarvested);
		node.AddValue("resourceName", resourceName);
		node.AddValue("resourceTitle", resourceTitle);
		node.AddValue("notifyLevel", notifyLevel);
		SystemUtilities.SaveNodeList(node, "modules", modules);
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "ResourceExtractionParameter", "targetBody", ref targetBody, Planetarium.fetch.Home);
		SystemUtilities.LoadNode(node, "ResourceExtractionParameter", "totalHarvested", ref totalHarvested, 0.0);
		SystemUtilities.LoadNode(node, "ResourceExtractionParameter", "goalHarvested", ref goalHarvested, 1000.0);
		SystemUtilities.LoadNode(node, "ResourceExtractionParameter", "resourceName", ref resourceName, "Matter");
		SystemUtilities.LoadNode(node, "ResourceExtractionParameter", "resourceTitle", ref resourceTitle, "matter");
		SystemUtilities.LoadNode(node, "ResourceExtractionParameter", "notifyLevel", ref notifyLevel, 0.25f);
		SystemUtilities.LoadNodeList(node, "ResourceExtractionParameter", "modules", ref modules);
	}

	public override void OnUpdate()
	{
		if (!SystemUtilities.FlightIsReady(base.Root.ContractState, Contract.State.Active))
		{
			return;
		}
		bool flag = Math.Ceiling(totalHarvested) >= goalHarvested;
		if (base.State == ParameterState.Incomplete)
		{
			if (flag)
			{
				successCounter++;
			}
			else
			{
				successCounter = 0;
			}
			if (successCounter >= 5)
			{
				SetComplete();
			}
		}
		if (base.State == ParameterState.Complete && !flag)
		{
			SetIncomplete();
		}
	}
}

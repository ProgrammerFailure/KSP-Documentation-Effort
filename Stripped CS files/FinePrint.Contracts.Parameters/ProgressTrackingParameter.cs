using System;
using System.Globalization;
using Contracts;
using FinePrint.Utilities;
using ns9;

namespace FinePrint.Contracts.Parameters;

public class ProgressTrackingParameter : ContractParameter
{
	public ProgressMilestone milestone;

	public bool eventsAdded;

	public bool IsTutorial
	{
		get
		{
			if (!(milestone.body == null) && !(milestone.body != Planetarium.fetch.Home))
			{
				switch (milestone.type)
				{
				default:
					return false;
				case ProgressType.FIRSTLAUNCH:
				case ProgressType.ORBIT:
				case ProgressType.REACHSPACE:
				case ProgressType.SCIENCE:
					return true;
				}
			}
			return false;
		}
	}

	public string InstructionalNote
	{
		get
		{
			switch (milestone.type)
			{
			case ProgressType.BASECONSTRUCTION:
				return Localizer.Format("#autoLOC_284365", milestone.body.displayName);
			case ProgressType.CREWTRANSFER:
				return Localizer.Format("#autoLOC_284367", milestone.body.displayName);
			case ProgressType.DOCKING:
				return Localizer.Format("#autoLOC_284369", milestone.body.displayName);
			case ProgressType.ESCAPE:
				return Localizer.Format("#autoLOC_284371", milestone.body.displayName);
			case ProgressType.FIRSTLAUNCH:
				return Localizer.Format("#autoLOC_284373");
			case ProgressType.FLAGPLANT:
				return Localizer.Format("#autoLOC_284375", milestone.body.displayName);
			case ProgressType.FLIGHT:
				return Localizer.Format("#autoLOC_284377", milestone.body.displayName);
			case ProgressType.FLYBY:
				return Localizer.Format("#autoLOC_284379", milestone.body.displayName);
			case ProgressType.FLYBYRETURN:
				return Localizer.Format("#autoLOC_284381", milestone.body.displayName, Planetarium.fetch.Home.displayName);
			case ProgressType.LANDING:
				return Localizer.Format(milestone.body.ocean ? "#autoLOC_6001960" : "#autoLOC_6001961", milestone.body.displayName);
			case ProgressType.LANDINGRETURN:
				return Localizer.Format("#autoLOC_284385", milestone.body.displayName, Planetarium.fetch.Home.displayName);
			case ProgressType.ORBIT:
				if (milestone.body == Planetarium.fetch.Home)
				{
					return Localizer.Format("#autoLOC_284388");
				}
				return Localizer.Format("#autoLOC_284390", milestone.body.displayName);
			case ProgressType.ORBITRETURN:
				if (milestone.body == Planetarium.fetch.Home)
				{
					return Localizer.Format("#autoLOC_284393");
				}
				return Localizer.Format("#autoLOC_284395", milestone.body.displayName, Planetarium.fetch.Home.displayName);
			case ProgressType.REACHSPACE:
				return Localizer.Format("#autoLOC_284397", Convert.ToDecimal(Math.Round(Planetarium.fetch.Home.atmosphereDepth)).ToString("#,###"));
			case ProgressType.RENDEZVOUS:
				return Localizer.Format("#autoLOC_284399", milestone.body.displayName);
			case ProgressType.SCIENCE:
				return Localizer.Format("#autoLOC_284401", milestone.body.displayName);
			case ProgressType.SPACEWALK:
				return Localizer.Format("#autoLOC_284403", milestone.body.displayName);
			case ProgressType.SPLASHDOWN:
				return Localizer.Format("#autoLOC_284405", milestone.body.displayName);
			case ProgressType.STATIONCONSTRUCTION:
				return Localizer.Format("#autoLOC_284407", milestone.body.displayName);
			default:
				return Localizer.Format("#autoLOC_284411");
			case ProgressType.SURFACEEVA:
				return Localizer.Format("#autoLOC_284409", milestone.body.displayName);
			}
		}
	}

	public string FlavorNote
	{
		get
		{
			switch (milestone.type)
			{
			case ProgressType.BASECONSTRUCTION:
				return Localizer.Format("#autoLOC_284423", milestone.body.displayName);
			case ProgressType.CREWTRANSFER:
				return Localizer.Format("#autoLOC_284425", milestone.body.displayName);
			case ProgressType.DOCKING:
				return Localizer.Format("#autoLOC_284427", milestone.body.displayName);
			case ProgressType.ESCAPE:
				return Localizer.Format("#autoLOC_284429", milestone.body.displayName);
			case ProgressType.FIRSTLAUNCH:
				return Localizer.Format("#autoLOC_284431");
			case ProgressType.FLAGPLANT:
				return Localizer.Format("#autoLOC_284433", milestone.body.displayName);
			case ProgressType.FLIGHT:
				return Localizer.Format("#autoLOC_284435", milestone.body.displayName, Planetarium.fetch.Home.displayName);
			case ProgressType.FLYBY:
				return Localizer.Format("#autoLOC_284437", milestone.body.displayName);
			case ProgressType.FLYBYRETURN:
				return Localizer.Format("#autoLOC_284439", milestone.body.displayName, Planetarium.fetch.Home.displayName);
			case ProgressType.LANDING:
				return Localizer.Format("#autoLOC_284441", milestone.body.displayName);
			case ProgressType.LANDINGRETURN:
				return Localizer.Format("#autoLOC_284443", Planetarium.fetch.Home.displayName, milestone.body.displayName);
			case ProgressType.ORBIT:
				if (milestone.body == Planetarium.fetch.Home)
				{
					return Localizer.Format("#autoLOC_284446", Planetarium.fetch.Home.displayName);
				}
				return Localizer.Format("#autoLOC_284448", milestone.body.displayName);
			case ProgressType.ORBITRETURN:
				if (milestone.body == Planetarium.fetch.Home)
				{
					return Localizer.Format("#autoLOC_284451");
				}
				return Localizer.Format("#autoLOC_284453", milestone.body.displayName, Planetarium.fetch.Home.displayName);
			case ProgressType.REACHSPACE:
				return Localizer.Format("#autoLOC_284455");
			case ProgressType.RENDEZVOUS:
				return Localizer.Format("#autoLOC_284457", milestone.body.displayName);
			case ProgressType.SCIENCE:
				if (milestone.body == Planetarium.fetch.Home)
				{
					return Localizer.Format("#autoLOC_284460");
				}
				return Localizer.Format("#autoLOC_284462", milestone.body.displayName);
			case ProgressType.SPACEWALK:
				if (milestone.body == Planetarium.fetch.Home)
				{
					return Localizer.Format("#autoLOC_6001963", milestone.body.displayName);
				}
				return Localizer.Format("#autoLOC_6001962");
			case ProgressType.SPLASHDOWN:
				return Localizer.Format("#autoLOC_284466", milestone.body.displayName);
			case ProgressType.STATIONCONSTRUCTION:
				return Localizer.Format("#autoLOC_284468", milestone.body.displayName);
			default:
				return Localizer.Format("#autoLOC_284472");
			case ProgressType.SURFACEEVA:
				return Localizer.Format("#autoLOC_284470", milestone.body.displayName);
			}
		}
	}

	public ProgressTrackingParameter()
	{
		milestone = new ProgressMilestone();
	}

	public ProgressTrackingParameter(ProgressMilestone milestone)
	{
		this.milestone = new ProgressMilestone(milestone.body, milestone.type);
	}

	public override string GetHashString()
	{
		return SystemUtilities.SuperSeed(base.Root).ToString(CultureInfo.InvariantCulture) + base.String_0;
	}

	public override string GetTitle()
	{
		if (IsTutorial)
		{
			return Localizer.Format("#autoLOC_6001924");
		}
		return milestone.type switch
		{
			ProgressType.BASECONSTRUCTION => Localizer.Format("#autoLOC_284203", milestone.body.displayName), 
			ProgressType.CREWTRANSFER => Localizer.Format("#autoLOC_284205", milestone.body.displayName), 
			ProgressType.DOCKING => Localizer.Format("#autoLOC_284207", milestone.body.displayName), 
			ProgressType.ESCAPE => Localizer.Format("#autoLOC_284209", milestone.body.displayName), 
			ProgressType.FIRSTLAUNCH => Localizer.Format("#autoLOC_284211"), 
			ProgressType.FLAGPLANT => Localizer.Format("#autoLOC_284213", milestone.body.displayName), 
			ProgressType.FLIGHT => Localizer.Format("#autoLOC_284215", milestone.body.displayName), 
			ProgressType.FLYBY => Localizer.Format("#autoLOC_284217", milestone.body.displayName), 
			ProgressType.FLYBYRETURN => Localizer.Format("#autoLOC_284219", Planetarium.fetch.Home.displayName, milestone.body.displayName), 
			ProgressType.LANDING => Localizer.Format(milestone.body.ocean ? "#autoLOC_6001964" : "#autoLOC_269752", milestone.body.displayName), 
			ProgressType.LANDINGRETURN => Localizer.Format("#autoLOC_284223", Planetarium.fetch.Home.displayName, milestone.body.displayName), 
			ProgressType.ORBIT => Localizer.Format("#autoLOC_284225", milestone.body.displayName), 
			ProgressType.ORBITRETURN => Localizer.Format((milestone.body == Planetarium.fetch.Home) ? "#autoLOC_6001965" : "#autoLOC_6001966", Planetarium.fetch.Home.displayName, milestone.body.displayName), 
			ProgressType.REACHSPACE => Localizer.Format("#autoLOC_284229"), 
			ProgressType.RENDEZVOUS => Localizer.Format("#autoLOC_284231", milestone.body.displayName), 
			ProgressType.SCIENCE => Localizer.Format("#autoLOC_284233", milestone.body.displayName), 
			ProgressType.SPACEWALK => Localizer.Format("#autoLOC_284235", milestone.body.displayName), 
			ProgressType.SPLASHDOWN => Localizer.Format("#autoLOC_284237", milestone.body.displayName), 
			ProgressType.STATIONCONSTRUCTION => Localizer.Format("#autoLOC_284239", milestone.body.displayName), 
			ProgressType.SURFACEEVA => Localizer.Format("#autoLOC_284241", milestone.body.displayName), 
			_ => Localizer.Format("#autoLOC_284243"), 
		};
	}

	public override string GetNotes()
	{
		if (HighLogic.LoadedScene == GameScenes.SPACECENTER && !IsTutorial)
		{
			string text = FlavorNote;
			int parameterCount = base.Root.ParameterCount;
			if (parameterCount > 0 && this != base.Root.GetParameter(parameterCount - 1))
			{
				text += "\n";
			}
			return text;
		}
		return InstructionalNote;
	}

	public override string GetMessageComplete()
	{
		if (IsTutorial)
		{
			return Localizer.Format("#autoLOC_284264", base.Title);
		}
		switch (milestone.type)
		{
		case ProgressType.BASECONSTRUCTION:
			return Localizer.Format("#autoLOC_284269", milestone.body.displayName);
		case ProgressType.CREWTRANSFER:
			return Localizer.Format("#autoLOC_284271");
		case ProgressType.DOCKING:
			return Localizer.Format("#autoLOC_284273");
		case ProgressType.ESCAPE:
			return Localizer.Format("#autoLOC_284275");
		case ProgressType.FIRSTLAUNCH:
			return Localizer.Format("#autoLOC_284277");
		case ProgressType.FLAGPLANT:
			return Localizer.Format("#autoLOC_284279");
		case ProgressType.FLIGHT:
			return Localizer.Format("#autoLOC_284281");
		case ProgressType.FLYBY:
			return Localizer.Format("#autoLOC_284283");
		case ProgressType.FLYBYRETURN:
			return Localizer.Format("#autoLOC_284285", milestone.body.displayName);
		case ProgressType.LANDING:
			return Localizer.Format("#autoLOC_284287", milestone.body.displayName);
		case ProgressType.LANDINGRETURN:
			return Localizer.Format("#autoLOC_284289", milestone.body.displayName);
		case ProgressType.ORBIT:
			if (milestone.body == Planetarium.fetch.Home)
			{
				return Localizer.Format("#autoLOC_284292");
			}
			return Localizer.Format("#autoLOC_284294");
		case ProgressType.ORBITRETURN:
			if (milestone.body == Planetarium.fetch.Home)
			{
				return Localizer.Format("#autoLOC_284297");
			}
			return Localizer.Format("#autoLOC_284299", milestone.body.displayName);
		case ProgressType.REACHSPACE:
			return Localizer.Format("#autoLOC_284301");
		case ProgressType.RENDEZVOUS:
			return Localizer.Format("#autoLOC_284303");
		case ProgressType.SCIENCE:
			if (milestone.body == Planetarium.fetch.Home)
			{
				return Localizer.Format("#autoLOC_284306");
			}
			return Localizer.Format("#autoLOC_284308", milestone.body.displayName);
		case ProgressType.SPACEWALK:
			return Localizer.Format("#autoLOC_284310");
		case ProgressType.SPLASHDOWN:
			return Localizer.Format("#autoLOC_284312");
		case ProgressType.STATIONCONSTRUCTION:
			return Localizer.Format("#autoLOC_284314", milestone.body.displayName);
		default:
			return Localizer.Format("#autoLOC_284318");
		case ProgressType.SURFACEEVA:
			return Localizer.Format("#autoLOC_284316", Planetarium.fetch.Home.displayName);
		}
	}

	public override void OnRegister()
	{
		base.DisableOnStateChange = true;
		eventsAdded = false;
		if (base.Root.ContractState == Contract.State.Active)
		{
			GameEvents.OnProgressComplete.Add(ProgressCheck);
			eventsAdded = true;
		}
	}

	public override void OnUnregister()
	{
		if (eventsAdded)
		{
			GameEvents.OnProgressComplete.Remove(ProgressCheck);
		}
	}

	public void ProgressCheck(ProgressNode node)
	{
		if (milestone.complete)
		{
			SetComplete();
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("targetBody", milestone.body.flightGlobalsIndex);
		node.AddValue("targetType", milestone.type);
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "ProgressTrackingParameter", "targetBody", ref milestone.body, Planetarium.fetch.Home);
		SystemUtilities.LoadNode(node, "ProgressTrackingParameter", "targetType", ref milestone.type, ProgressType.FIRSTLAUNCH);
	}
}

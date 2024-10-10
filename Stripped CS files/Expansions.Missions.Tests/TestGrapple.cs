using System;
using System.Collections.Generic;
using System.ComponentModel;
using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Tests;

[MEScoreModule(new Type[]
{
	typeof(ScoreModule_Resource),
	typeof(ScoreModule_Time)
})]
public class TestGrapple : TestModule
{
	public enum SpaceObjectChoices
	{
		[Description("#autoLOC_8000046")]
		Asteroid,
		[Description("#autoLOC_6005065")]
		Comet,
		[Description("#autoLOC_8000001")]
		Vessel
	}

	[MEGUI_VesselSelect(resetValue = "0", gapDisplay = true, guiName = "#autoLOC_8005456", Tooltip = "#autoLOC_8005457")]
	public uint grapplingVesselID;

	public ModuleGrappleNode grabbingUnit;

	[MEGUI_Dropdown(onDropDownValueChange = "OnSpaceObjectChoiceValueChange", onControlSetupComplete = "OnSpaceObjectChoiceControlSetup", onControlCreated = "OnSpaceObjectChoiceControlCreated", gapDisplay = false, guiName = "#autoLOC_8005458", Tooltip = "#autoLOC_8005459")]
	public SpaceObjectChoices grappledSpaceObject;

	public MEGUIParameterDropdownList spaceObjectParameterReference;

	[MEGUI_AsteroidSelect(canBePinned = false, onControlCreated = "OnAsteroidControlCreated", resetValue = "0", guiName = "#autoLOC_8000046", Tooltip = "#autoLOC_8005460")]
	public uint grappleAsteroidID;

	[MEGUI_CometSelect(hideOnSetup = true, canBePinned = false, onControlCreated = "OnCometControlCreated", resetValue = "0", guiName = "#autoLOC_6005065", Tooltip = "#autoLOC_8005461")]
	public uint grappleCometID;

	[MEGUI_VesselSelect(addDefaultOption = false, hideOnSetup = true, canBePinned = false, onControlCreated = "OnVesselControlCreated", resetValue = "0", gapDisplay = true, guiName = "#autoLOC_8000001", Tooltip = "#autoLOC_6002525")]
	public uint grappleVesselID;

	public uint grappleSOPartID;

	public MEGUIParameterAsteroidDropdownList grappleAsteroidParameterReference;

	public MEGUIParameterCometDropdownList grappleCometParameterReference;

	public MEGUIParameterVesselDropdownList grappleVesselParameterReference;

	[MEGUI_Dropdown(SetDropDownItems = "SetDockedUndockedDropdown", canBePinned = true, guiName = "#autoLOC_8002012")]
	public string dockedUndocked;

	public bool firstRunTest;

	public bool testSuccess;

	public override void Awake()
	{
		base.Awake();
		title = "#autoLOC_8002013";
		firstRunTest = true;
		testSuccess = false;
	}

	public void OnSpaceObjectChoiceControlCreated(MEGUIParameterDropdownList parameter)
	{
		spaceObjectParameterReference = parameter;
	}

	public void OnSpaceObjectChoiceControlSetup(MEGUIParameterDropdownList parameter)
	{
	}

	public void OnAsteroidControlCreated(MEGUIParameterAsteroidDropdownList parameter)
	{
		grappleAsteroidParameterReference = parameter;
	}

	public void OnCometControlCreated(MEGUIParameterCometDropdownList parameter)
	{
		grappleCometParameterReference = parameter;
	}

	public void OnVesselControlCreated(MEGUIParameterVesselDropdownList parameter)
	{
		grappleVesselParameterReference = parameter;
	}

	public override void ParameterSetupComplete()
	{
		OnSpaceObjectChoiceValueChange(spaceObjectParameterReference, spaceObjectParameterReference.FieldValue);
	}

	public void OnSpaceObjectChoiceValueChange(MEGUIParameterDropdownList sender, int newIndex)
	{
		if (grappleAsteroidParameterReference != null)
		{
			grappleAsteroidParameterReference.gameObject.SetActive(value: false);
		}
		if (grappleCometParameterReference != null)
		{
			grappleCometParameterReference.gameObject.SetActive(value: false);
		}
		if (grappleVesselParameterReference != null)
		{
			grappleVesselParameterReference.gameObject.SetActive(value: false);
		}
		if (sender.SelectedValue != null && base.node != null)
		{
			switch ((SpaceObjectChoices)sender.SelectedValue)
			{
			default:
				if (grappleAsteroidParameterReference != null)
				{
					grappleAsteroidParameterReference.gameObject.SetActive(value: true);
				}
				break;
			case SpaceObjectChoices.Comet:
				if (grappleCometParameterReference != null)
				{
					grappleCometParameterReference.gameObject.SetActive(value: true);
				}
				break;
			case SpaceObjectChoices.Vessel:
				if (grappleVesselParameterReference != null)
				{
					grappleVesselParameterReference.gameObject.SetActive(value: true);
				}
				break;
			}
		}
		UpdateNodeBodyUI();
	}

	public List<MEGUIDropDownItem> SetDockedUndockedDropdown()
	{
		return new List<MEGUIDropDownItem>
		{
			new MEGUIDropDownItem("Docked", "Docked", "#autoLOC_8002014"),
			new MEGUIDropDownItem("Undocked", "Undocked", "#autoLOC_8002015")
		};
	}

	public override void Initialized()
	{
		testSuccess = false;
		if (dockedUndocked == "Docked")
		{
			GameEvents.onDockingComplete.Add(onPartCouple);
			GameEvents.onPartCoupleComplete.Add(onPartCouple);
		}
		else
		{
			GameEvents.onPartUndockComplete.Add(onPartUndock);
			GameEvents.onPartDeCoupleComplete.Add(onPartUndock);
		}
	}

	public override void Cleared()
	{
		GameEvents.onDockingComplete.Remove(onPartCouple);
		GameEvents.onPartCoupleComplete.Remove(onPartCouple);
		GameEvents.onPartUndockComplete.Remove(onPartUndock);
		GameEvents.onPartDeCoupleComplete.Remove(onPartUndock);
	}

	public void onPartCouple(GameEvents.FromToAction<Part, Part> partAction)
	{
		testPartVessels();
	}

	public void onPartUndock(Part part)
	{
		testPartVessels();
	}

	public void testPartVessels()
	{
		uint num = 0u;
		uint num2 = 0u;
		num2 = grappledSpaceObject switch
		{
			SpaceObjectChoices.Comet => grappleCometID, 
			SpaceObjectChoices.Vessel => base.node.mission.CurrentVesselID(base.node, grappleVesselID), 
			_ => grappleAsteroidID, 
		};
		if (grapplingVesselID == 0)
		{
			if (!(FlightGlobals.ActiveVessel != null))
			{
				return;
			}
			num = FlightGlobals.ActiveVessel.persistentId;
		}
		else
		{
			num = base.node.mission.CurrentVesselID(base.node, grapplingVesselID);
		}
		if (FlightGlobals.PersistentVesselIds.ContainsKey(num) & FlightGlobals.PersistentVesselIds.ContainsKey(num2))
		{
			if (dockedUndocked == "Docked")
			{
				if (num == num2)
				{
					testSuccess = true;
				}
			}
			else if (num != num2)
			{
				testSuccess = true;
			}
		}
		else
		{
			if ((grappledSpaceObject != 0 && grappledSpaceObject != SpaceObjectChoices.Comet) || !(FlightGlobals.ActiveVessel != null))
			{
				return;
			}
			int num3 = FlightGlobals.ActiveVessel.Parts.Count - 1;
			while (num3 >= 0)
			{
				Part part = FlightGlobals.ActiveVessel.Parts[num3];
				if (!(part != null) || (!(part.FindModuleImplementing<ModuleAsteroid>() != null) && !(part.FindModuleImplementing<ModuleComet>() != null)))
				{
					num3--;
					continue;
				}
				grappleSOPartID = part.persistentId;
				break;
			}
			if (grabbingUnit == null && FlightGlobals.ActiveVessel != null)
			{
				for (int i = 0; i < FlightGlobals.ActiveVessel.Parts.Count; i++)
				{
					Part part2 = FlightGlobals.ActiveVessel.Parts[i];
					if ((bool)part2.FindModuleImplementing<ModuleGrappleNode>())
					{
						grabbingUnit = part2.FindModuleImplementing<ModuleGrappleNode>();
						break;
					}
				}
			}
			if (grappleSOPartID != 0 && grabbingUnit != null && (grabbingUnit.state.Contains("Grappled") || grabbingUnit.state.Contains("Ready")))
			{
				testSuccess = true;
			}
		}
	}

	public override bool Test()
	{
		if (firstRunTest)
		{
			testPartVessels();
			firstRunTest = false;
		}
		return testSuccess;
	}

	public override void OnVesselPersistentIdChanged(uint oldId, uint newId)
	{
		if (grapplingVesselID == oldId)
		{
			grapplingVesselID = newId;
			Debug.LogFormat("[TestPartDocking]: Node ({0}) Grappling VesselId changed from {1} to {2}", base.node.id, oldId, newId);
		}
		if (grappleAsteroidID == oldId)
		{
			grappleAsteroidID = newId;
			Debug.LogFormat("[TestPartDocking]: Node ({0}) Grappled Asteroid VesselId changed from {1} to {2}", base.node.id, oldId, newId);
		}
		if (grappleCometID == oldId)
		{
			grappleCometID = newId;
			Debug.LogFormat("[TestPartDocking]: Node ({0}) Grappled Comet VesselId changed from {1} to {2}", base.node.id, oldId, newId);
		}
		if (grappleVesselID == oldId)
		{
			grappleVesselID = newId;
			Debug.LogFormat("[TestPartDocking]: Node ({0}) Grappled Vessel VesselId changed from {1} to {2}", base.node.id, oldId, newId);
		}
	}

	public override void OnVesselDocking(uint oldId, uint newId)
	{
		if (grapplingVesselID == oldId)
		{
			grapplingVesselID = newId;
			Debug.LogFormat("[TestPartDocking]: Node ({0}) Grappling VesselId changed from {1} to {2}", base.node.id, oldId, newId);
		}
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8005462");
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (base.node != null)
		{
			base.node.UpdateInfoOnRefresh = true;
		}
		if (field.name == "grapplingVesselID" && field.GetValue() != null)
		{
			string text = "";
			string vesselName = "";
			bool flag = false;
			VesselSituation vesselSituation = base.node.mission.GetVesselSituationByVesselID(grapplingVesselID);
			if (vesselSituation != null)
			{
				vesselName = vesselSituation.vesselName;
			}
			else if (base.node.mission.UpdateFromMappedVesselIDs(grapplingVesselID, ref vesselName, ref vesselSituation))
			{
				flag = true;
			}
			text = ((vesselSituation != null || flag) ? vesselName : ((grapplingVesselID != 0) ? Localizer.Format("#autoLOC_8100159") : Localizer.Format("#autoLOC_8004217")));
			string text2 = Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8000001"), text);
			if (flag && vesselSituation != null)
			{
				text2 = text2 + "\n" + Localizer.Format("#autoLOC_6002491", vesselSituation.vesselName) + "\n";
			}
			return text2;
		}
		if (field.name == "grappledSpaceObject")
		{
			string text3 = base.GetNodeBodyParameterString(field) + "\n";
			switch (grappledSpaceObject)
			{
			case SpaceObjectChoices.Asteroid:
			{
				string text6 = "";
				if (grappleAsteroidID != 0 && FlightGlobals.PersistentVesselIds[grappleAsteroidID] != null)
				{
					text6 = text3 + Localizer.Format("#autoLOC_8004190", "#autoLOC_8000046", FlightGlobals.PersistentVesselIds[grappleAsteroidID].GetDisplayName());
				}
				return text3 + text6;
			}
			case SpaceObjectChoices.Comet:
			{
				string text5 = "";
				Comet cometByPersistentID = base.node.mission.GetCometByPersistentID(grappleCometID);
				text5 = ((cometByPersistentID != null) ? Localizer.Format(cometByPersistentID.name) : Localizer.Format("#autoLOC_6003000"));
				return text3 + text5;
			}
			case SpaceObjectChoices.Vessel:
			{
				string text4 = "";
				uint num = base.node.mission.CurrentVesselID(base.node, grapplingVesselID);
				if (num != 0 && FlightGlobals.PersistentVesselIds[num] != null)
				{
					text4 = text3 + Localizer.Format("#autoLOC_8000069", FlightGlobals.PersistentVesselIds[num].GetDisplayName());
				}
				return text3 + text4;
			}
			}
		}
		if (field.name == "dockedUndocked" && field.GetValue() != null)
		{
			if (field.GetValue().ToString() == "Docked")
			{
				return Localizer.Format("#autoLOC_8004190", field.guiName, Localizer.Format("#autoLOC_8002014"));
			}
			return Localizer.Format("#autoLOC_8004190", field.guiName, Localizer.Format("#autoLOC_8002015"));
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override void RunValidation(MissionEditorValidator validator)
	{
		VesselSituation vesselSituationByVesselID = base.node.mission.GetVesselSituationByVesselID(grapplingVesselID);
		if (vesselSituationByVesselID != null)
		{
			if (grappledSpaceObject == SpaceObjectChoices.Vessel && vesselSituationByVesselID.persistentId == grappleVesselID)
			{
				validator.AddNodeFail(base.node, Localizer.Format("#autoLOC_6002526"));
			}
			else if (grappledSpaceObject == SpaceObjectChoices.Asteroid && vesselSituationByVesselID.persistentId == grappleAsteroidID)
			{
				validator.AddNodeFail(base.node, Localizer.Format("#autoLOC_6002526"));
			}
			else if (grappledSpaceObject == SpaceObjectChoices.Comet && vesselSituationByVesselID.persistentId == grappleAsteroidID)
			{
				validator.AddNodeFail(base.node, Localizer.Format("#autoLOC_6002526"));
			}
		}
		else if (grappledSpaceObject == SpaceObjectChoices.Vessel && grapplingVesselID == 0 && grappleVesselID == 0)
		{
			validator.AddNodeFail(base.node, Localizer.Format("#autoLOC_6002526"));
		}
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("grapplingVesselID", grapplingVesselID);
		node.AddValue("grappleAsteroidID", grappleAsteroidID);
		node.AddValue("grappleCometID", grappleCometID);
		node.AddValue("grappleVesselID", grappleVesselID);
		node.AddValue("grappledSpaceObject", grappledSpaceObject);
		node.AddValue("dockedUndocked", dockedUndocked);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		ConfigNode configNode = new ConfigNode("PARTONE");
		if (node.TryGetNode("PARTONE", ref configNode))
		{
			VesselPartIDPair vesselPartIDPair = new VesselPartIDPair();
			vesselPartIDPair.Load(configNode);
			grapplingVesselID = vesselPartIDPair.VesselID;
		}
		dockedUndocked = "Docked";
		node.TryGetValue("dockedUndocked", ref dockedUndocked);
		node.TryGetValue("grapplingVesselID", ref grapplingVesselID);
		node.TryGetValue("grappleAsteroidID", ref grappleAsteroidID);
		node.TryGetValue("grappleCometID", ref grappleCometID);
		node.TryGetValue("grappleVesselID", ref grappleVesselID);
		node.TryGetEnum("grappledSpaceObject", ref grappledSpaceObject, SpaceObjectChoices.Asteroid);
	}
}

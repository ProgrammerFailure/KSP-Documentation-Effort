using System.Collections;
using System.Collections.Generic;
using System.IO;
using Expansions.Missions.Editor;
using Expansions.Missions.Runtime;
using FinePrint.Utilities;
using ns11;
using ns9;
using PreFlightTests;
using UnityEngine;

namespace Expansions.Missions.Actions;

public class ActionCreateVessel : ActionModule, INodeOrbit
{
	[MEGUI_VesselSituation(guiName = "#autoLOC_8000001")]
	public VesselSituation vesselSituation;

	public ScreenMessage activeMessage;

	public VesselSituation VesselToBuild => vesselSituation;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000028");
	}

	public override void Initialize(MENode node)
	{
		base.Initialize(node);
		vesselSituation = new VesselSituation(node.mission, node);
	}

	public override IEnumerator Fire()
	{
		if (vesselSituation.launched)
		{
			yield break;
		}
		if (vesselSituation.playerCreated)
		{
			node.mission.situation.AddNodeWithVesselToBuild(node);
			CreateVesselScreenMessage();
		}
		else if (!string.IsNullOrEmpty(vesselSituation.craftFile))
		{
			for (int i = 0; i < node.fromNodes.Count; i++)
			{
				if (node.fromNodes[i].isStartNode)
				{
					yield return new WaitForSeconds(1f);
					break;
				}
			}
			bool switchActiveVslImmediately = false;
			Vessel activeVessel = null;
			List<ProtoVessel> list = new List<ProtoVessel>();
			HighLogic.CurrentGame.Updated();
			if (vesselSituation.location.situation == MissionSituation.VesselStartSituations.PRELAUNCH)
			{
				if (HighLogic.CurrentGame.flightState != null && HighLogic.CurrentGame.flightState.protoVessels != null)
				{
					list = ShipConstruction.FindVesselsLandedAt(HighLogic.CurrentGame.flightState, vesselSituation.location.launchSite);
				}
				int j = 0;
				for (int count = list.Count; j < count; j++)
				{
					if (HighLogic.LoadedSceneIsFlight && list[j].vesselRef != null && FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel == list[j].vesselRef)
					{
						activeVessel = FlightGlobals.fetch.activeVessel;
						MissionSystem.ToggleVesselComponents(activeVessel, toggleValue: false);
						vesselSituation.focusonSpawn = true;
						switchActiveVslImmediately = true;
						if ((bool)LoadingBufferMask.Instance)
						{
							LoadingBufferMask.Instance.Show();
						}
					}
					else
					{
						ShipConstruction.RecoverVesselFromFlight(list[j], HighLogic.CurrentGame.flightState);
					}
				}
			}
			node.mission.situation.AddNodeWithVesselToBuild(node);
			yield return StartCoroutine(MissionSystem.Instance.ConstructShip(vesselSituation, node.mission, instantiateVessel: true));
			node.mission.situation.RemoveLaunchedVessels();
			if (MissionSystem.Instance.returnVessel == null)
			{
				Debug.LogWarning("[Missions]: Unable to spawn Vessel :" + vesselSituation.vesselName + " Ship Construction Failed");
				Debug.LogWarning(MissionSystem.Instance.errorString);
			}
			else if (vesselSituation.focusonSpawn)
			{
				TimeWarp.SetRate(0, instant: true);
				if (HighLogic.LoadedSceneIsFlight)
				{
					if (CameraManager.Instance != null && (CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.const_3 || CameraManager.Instance.currentCameraMode == CameraManager.CameraMode.Internal))
					{
						CameraManager.Instance.SetCameraFlight();
					}
					node.mission.SwitchActiveVessel(MissionSystem.Instance.returnVessel, saveRevertOnSwitch: true, switchActiveVslImmediately);
					if (switchActiveVslImmediately)
					{
						ShipConstruction.RecoverVesselFromFlight(activeVessel.protoVessel, HighLogic.CurrentGame.flightState);
					}
					yield return new WaitForSeconds(1f);
					if ((bool)LoadingBufferMask.Instance)
					{
						LoadingBufferMask.Instance.Hide();
					}
					TimeWarp.SetRate(TimeWarp.CurrentRateIndex, instant: true);
				}
				else if (HighLogic.LoadedScene == GameScenes.TRACKSTATION)
				{
					yield return null;
					yield return null;
					SpaceTracking.Instance.SetVessel(MissionSystem.Instance.returnVessel, keepFocus: true);
				}
			}
			else
			{
				node.mission.saveRevertOnSwitchActiveVessel = true;
			}
		}
		else
		{
			Debug.LogWarning("[Missions]: Unable to spawn Vessel :" + vesselSituation.vesselName + " Craftfile is blank");
		}
	}

	public void CreateVesselScreenMessage()
	{
		if (activeMessage == null || activeMessage.textInstance == null)
		{
			string message = Localizer.Format("#autoLOC_8000302", vesselSituation.vesselName);
			activeMessage = ScreenMessages.PostScreenMessage(message, float.MaxValue, ScreenMessageStyle.UPPER_CENTER, persist: true);
		}
		if (MissionsApp.Instance != null)
		{
			MissionsApp.Instance.UpdateLauncherButtonPlayAnim();
		}
	}

	public override Vector3 ActionLocation()
	{
		Vector3 zero = Vector3.zero;
		MissionSituation.VesselStartSituations situation = vesselSituation.location.situation;
		if (situation != MissionSituation.VesselStartSituations.LANDED)
		{
			switch (situation)
			{
			case MissionSituation.VesselStartSituations.ORBITING:
				return vesselSituation.location.orbitSnapShot.Orbit.getPositionAtUT((HighLogic.CurrentGame != null) ? HighLogic.CurrentGame.UniversalTime : 0.0);
			case MissionSituation.VesselStartSituations.PRELAUNCH:
			{
				PSystemSetup.SpaceCenterFacility spaceCenterFacility = PSystemSetup.Instance.GetSpaceCenterFacility(vesselSituation.location.launchSite);
				if (spaceCenterFacility != null)
				{
					return spaceCenterFacility.facilityTransform.position;
				}
				LaunchSite launchSite = PSystemSetup.Instance.GetLaunchSite(vesselSituation.location.launchSite);
				if (launchSite != null)
				{
					return launchSite.GetWorldPos().position;
				}
				break;
			}
			}
			return zero;
		}
		return vesselSituation.location.vesselGroundLocation.GetWorldPosition();
	}

	public override void OnCloned(ref ActionModule actionModuleBase)
	{
		base.OnCloned(ref actionModuleBase);
		vesselSituation.persistentId = FlightGlobals.GetUniquepersistentId();
	}

	public bool HasNodeOrbit()
	{
		return true;
	}

	public Orbit GetNodeOrbit()
	{
		Orbit orbit = null;
		orbit = vesselSituation.location.orbitSnapShot.Orbit;
		switch (vesselSituation.location.situation)
		{
		case MissionSituation.VesselStartSituations.LANDED:
		case MissionSituation.VesselStartSituations.PRELAUNCH:
			orbit.referenceBody = vesselSituation.location.vesselGroundLocation.targetBody;
			break;
		}
		return orbit;
	}

	public void CreatePlaceholderCraft()
	{
		ShipConstruct obj = new ShipConstruct(vesselSituation.location.facility)
		{
			shipName = vesselSituation.vesselName,
			missionFlag = vesselSituation.mission.flagURL
		};
		AvailablePart partInfoByName = PartLoader.getPartInfoByName("mk1pod.v2");
		Part part = Object.Instantiate(partInfoByName.partPrefab);
		part.gameObject.SetActive(value: true);
		part.name = partInfoByName.name;
		part.partInfo = partInfoByName;
		part.persistentId = FlightGlobals.CheckPartpersistentId(0u, part, removeOldId: false, addNewId: true);
		part.transform.position = new Vector3(0f, 5f, 0f);
		part.attPos0 = new Vector3(0f, 5f, 0f);
		part.transform.rotation = Quaternion.identity;
		part.attRotation0 = Quaternion.identity;
		part.symMethod = SymmetryMethod.Mirror;
		obj.Add(part);
		EditorDriver.editorFacility = vesselSituation.location.facility;
		ShipConstruction.CreateBackup(obj);
		string text = KSPUtil.SanitizeString(obj.shipName, '_', replaceEmpty: true);
		string text2 = text + ".craft";
		ShipConstruction.SaveShip(obj, text);
		MissionCraft val = new MissionCraft(KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Ships/" + ShipConstruction.GetShipsSubfolderFor(vesselSituation.location.facility) + "/", text2);
		if (!vesselSituation.mission.craftFileList.ContainsKey(text2))
		{
			vesselSituation.mission.craftFileList.Add(text2, val);
		}
		vesselSituation.craftFile = text2;
	}

	public void DeletePlaceholderCraft()
	{
		string path = KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/Ships/" + ShipConstruction.GetShipsSubfolderFor(vesselSituation.location.facility) + "/" + vesselSituation.vesselName + ".craft";
		if (File.Exists(path))
		{
			File.Delete(path);
		}
		vesselSituation.craftFile = "";
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "vesselSituation")
		{
			string text = "";
			if (vesselSituation == null)
			{
				text = Localizer.Format("#autoLOC_8000068");
			}
			else
			{
				string text2 = vesselSituation.vesselName;
				if (HighLogic.LoadedSceneIsMissionBuilder)
				{
					MissionCraft craftBySituationsVesselID = MissionEditorLogic.Instance.EditorMission.GetCraftBySituationsVesselID(vesselSituation.persistentId);
					if (craftBySituationsVesselID != null && MissionEditorLogic.Instance.incompatibleCraft.Contains(craftBySituationsVesselID.craftFile))
					{
						text2 = Localizer.Format("#autoLOC_8004245", text2);
					}
				}
				text = Localizer.Format("#autoLOC_8000069", text2);
			}
			if (vesselSituation.vesselCrew.Count > 0)
			{
				List<string> list = new List<string>();
				for (int i = 0; i < vesselSituation.vesselCrew.Count; i++)
				{
					for (int j = 0; j < vesselSituation.vesselCrew[i].crewNames.Count; j++)
					{
						list.Add(vesselSituation.vesselCrew[i].LocalizeCrewMember(j));
					}
				}
				if (list.Count > 0)
				{
					text += Localizer.Format("#autoLOC_8000059");
					text += StringUtilities.ThisThisAndThat(list);
					text += "\n";
				}
			}
			switch (vesselSituation.location.situation)
			{
			case MissionSituation.VesselStartSituations.ORBITING:
				if (vesselSituation.location.orbitSnapShot.Body != null)
				{
					text = text + Localizer.Format("#autoLOC_8100316", field.guiName, vesselSituation.location.orbitSnapShot.Body.displayName.LocalizeRemoveGender()) + "\n";
					text = text + Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8100058"), vesselSituation.location.orbitSnapShot.SemiMajorAxis.ToString("N0")) + "\n";
					text = text + Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8100059"), vesselSituation.location.orbitSnapShot.Apoapsis.ToString("N0")) + "\n";
					text = text + Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8100060"), vesselSituation.location.orbitSnapShot.Periapsis.ToString("N0")) + "\n";
					if (vesselSituation.location.orbitSnapShot.eccentricity > 0.0001)
					{
						text = text + Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8100061"), vesselSituation.location.orbitSnapShot.Eccentricity.ToString("0.##")) + "\n";
					}
					if (vesselSituation.location.orbitSnapShot.inclination != 0.0)
					{
						text = text + Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8100062"), vesselSituation.location.orbitSnapShot.Inclination.ToString("0.##°")) + "\n";
					}
					if (vesselSituation.location.orbitSnapShot.Lan != 0.0)
					{
						text = text + Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8100063"), vesselSituation.location.orbitSnapShot.double_0.ToString("0.##")) + "\n";
					}
				}
				break;
			case MissionSituation.VesselStartSituations.PRELAUNCH:
				if (!string.IsNullOrEmpty(vesselSituation.location.launchSite))
				{
					text += Localizer.Format("#autoLOC_8000283", PSystemSetup.Instance.GetLaunchSiteDisplayName(vesselSituation.location.launchSite));
				}
				break;
			case MissionSituation.VesselStartSituations.LANDED:
				if (vesselSituation.location.vesselGroundLocation.targetBody != null)
				{
					text += Localizer.Format("#autoLOC_8000284", vesselSituation.location.vesselGroundLocation.targetBody.displayName.LocalizeRemoveGender());
					text += Localizer.Format("#autoLOC_8000285", vesselSituation.location.vesselGroundLocation.longitude.ToString("0.##"), vesselSituation.location.vesselGroundLocation.latitude.ToString("0.##"));
				}
				break;
			}
			return text;
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override string GetAppObjectiveInfo()
	{
		string text = "";
		BaseAPFieldList baseAPFieldList = new BaseAPFieldList(this);
		int i = 0;
		for (int count = baseAPFieldList.Count; i < count; i++)
		{
			text += StringBuilderCache.Format(GetNodeBodyParameterString(baseAPFieldList[i]));
		}
		return text;
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004010");
	}

	public override void RunValidation(MissionEditorValidator validator)
	{
		if (!vesselSituation.playerCreated && string.IsNullOrEmpty(vesselSituation.craftFile))
		{
			validator.AddNodeFail(node, Localizer.Format("#autoLOC_8000293"));
		}
		if (vesselSituation.location.situation == MissionSituation.VesselStartSituations.PRELAUNCH && string.IsNullOrEmpty(PSystemSetup.Instance.GetLaunchSiteDisplayName(vesselSituation.location.launchSite)) && !node.mission.MissionHasLaunchSite(vesselSituation.location.launchSite))
		{
			validator.AddNodeFail(node, Localizer.Format("#autoLOC_8000294"));
		}
		bool autoGenerateCrew = vesselSituation.autoGenerateCrew;
		if (!vesselSituation.playerCreated && !string.IsNullOrEmpty(vesselSituation.craftFile))
		{
			MissionCraft craftBySituationsVesselID = vesselSituation.mission.GetCraftBySituationsVesselID(vesselSituation.persistentId);
			if (craftBySituationsVesselID != null)
			{
				if (MissionEditorLogic.Instance.incompatibleCraft.Contains(craftBySituationsVesselID.craftFile))
				{
					validator.AddNodeFail(node, Localizer.Format("#autoLOC_8004244", vesselSituation.vesselName));
				}
				ShipConstruct shipOut = MissionEditorLogic.Instance.LoadVessel(craftBySituationsVesselID);
				if (!autoGenerateCrew && vesselSituation.vesselCrew.Count == 0)
				{
					VesselCrewManifest vesselCrewManifest = VesselCrewManifest.FromConfigNode(craftBySituationsVesselID.CraftNode);
					vesselCrewManifest.AddCrewMembers(ref vesselSituation.vesselCrew, vesselSituation.mission.situation.crewRoster);
					if (!new NoControlSources(vesselCrewManifest, vesselSituation.mission.situation.crewRoster).TestCondition())
					{
						validator.AddNodeWarn(node, Localizer.Format("#autoLOC_8000295"));
					}
				}
				if (shipOut != null && shipOut.parts.Count > 0 && !EditorLogic.MissionCheckLaunchClamps(shipOut, shipOut.parts[0].localRoot, vesselSituation, out shipOut))
				{
					validator.AddNodeFail(node, Localizer.Format("#autoLOC_8002096"));
				}
			}
		}
		if (vesselSituation.playerCreated)
		{
			List<VesselRestriction> activeRestrictions = vesselSituation.vesselRestrictionList.ActiveRestrictions;
			if (activeRestrictions.Count > 0)
			{
				for (int i = 0; i < activeRestrictions.Count; i++)
				{
					VesselRestriction_Mass vesselRestriction_Mass = activeRestrictions[i] as VesselRestriction_Mass;
					VesselRestriction_Size vesselRestriction_Size = activeRestrictions[i] as VesselRestriction_Size;
					VesselRestriction_PartCount vesselRestriction_PartCount = activeRestrictions[i] as VesselRestriction_PartCount;
					if (vesselRestriction_Mass != null && vesselRestriction_Mass.targetMass > 0f && (vesselSituation.location.launchSite == "Runway" || vesselSituation.location.launchSite == "LaunchPad"))
					{
						float num = 0f;
						if (vesselSituation.location.launchSite == "Runway")
						{
							float editorNormLevel = ((float)node.mission.situation.gameParameters.CustomParams<MissionParamsFacilities>().facilityLevelRunway - 1f) / 2f;
							num = GameVariables.Instance.GetCraftMassLimit(editorNormLevel, isPad: false);
						}
						else
						{
							float editorNormLevel2 = ((float)node.mission.situation.gameParameters.CustomParams<MissionParamsFacilities>().facilityLevelLaunchpad - 1f) / 2f;
							num = GameVariables.Instance.GetCraftMassLimit(editorNormLevel2, isPad: true);
						}
						if (num > 0f)
						{
							if (vesselRestriction_Mass.comparisonOperator == TestComparisonLessGreaterEqual.GreaterOrEqual)
							{
								if (vesselRestriction_Mass.targetMass >= num)
								{
									validator.AddNodeWarn(node, Localizer.Format("#autoLOC_8000296", vesselSituation.location.launchSite, num.ToString("N0")));
								}
							}
							else if (vesselRestriction_Mass.targetMass > num)
							{
								validator.AddNodeWarn(node, Localizer.Format("#autoLOC_8000296", vesselSituation.location.launchSite, num.ToString("N0")));
							}
						}
					}
					if (vesselRestriction_Size != null && (vesselRestriction_Size.tempInputX > 0f || vesselRestriction_Size.tempInputY > 0f || (vesselRestriction_Size.tempInputZ > 0f && (vesselSituation.location.launchSite == "Runway" || vesselSituation.location.launchSite == "LaunchPad"))))
					{
						Vector3 zero = Vector3.zero;
						if (vesselSituation.location.launchSite == "Runway")
						{
							float editorNormLevel3 = ((float)node.mission.situation.gameParameters.CustomParams<MissionParamsFacilities>().facilityLevelRunway - 1f) / 2f;
							zero = GameVariables.Instance.GetCraftSizeLimit(editorNormLevel3, isPad: false);
						}
						else
						{
							float editorNormLevel4 = ((float)node.mission.situation.gameParameters.CustomParams<MissionParamsFacilities>().facilityLevelLaunchpad - 1f) / 2f;
							zero = GameVariables.Instance.GetCraftSizeLimit(editorNormLevel4, isPad: true);
						}
						if (zero.x > 0f)
						{
							if (vesselRestriction_Size.comparisonOperator == TestComparisonLessGreaterEqual.GreaterOrEqual)
							{
								if (vesselRestriction_Size.tempInputX >= zero.x)
								{
									validator.AddNodeWarn(node, Localizer.Format("#autoLOC_8000298", vesselSituation.location.launchSite, zero.x.ToString("N0")));
								}
							}
							else if (vesselRestriction_Size.tempInputX > zero.x)
							{
								validator.AddNodeWarn(node, Localizer.Format("#autoLOC_8000298", vesselSituation.location.launchSite, zero.x.ToString("N0")));
							}
						}
						if (zero.y > 0f)
						{
							if (vesselRestriction_Size.comparisonOperator == TestComparisonLessGreaterEqual.GreaterOrEqual)
							{
								if (vesselRestriction_Size.tempInputY >= zero.y)
								{
									validator.AddNodeWarn(node, Localizer.Format("#autoLOC_8000299", vesselSituation.location.launchSite, zero.y.ToString("N0")));
								}
							}
							else if (vesselRestriction_Size.tempInputY > zero.y)
							{
								validator.AddNodeWarn(node, Localizer.Format("#autoLOC_8000299", vesselSituation.location.launchSite, zero.y.ToString("N0")));
							}
						}
						if (zero.z > 0f)
						{
							if (vesselRestriction_Size.comparisonOperator == TestComparisonLessGreaterEqual.GreaterOrEqual)
							{
								if (vesselRestriction_Size.tempInputZ >= zero.z)
								{
									validator.AddNodeWarn(node, Localizer.Format("#autoLOC_8000300", vesselSituation.location.launchSite, zero.z.ToString("N0")));
								}
							}
							else if (vesselRestriction_Size.tempInputZ > zero.z)
							{
								validator.AddNodeWarn(node, Localizer.Format("#autoLOC_8000300", vesselSituation.location.launchSite, zero.z.ToString("N0")));
							}
						}
					}
					if (vesselRestriction_PartCount != null && vesselRestriction_PartCount.targetParts > 0)
					{
						int num2 = 0;
						if (vesselSituation.location.facility == EditorFacility.const_2)
						{
							float editorNormLevel5 = ((float)node.mission.situation.gameParameters.CustomParams<MissionParamsFacilities>().facilityLevelSPH - 1f) / 2f;
							num2 = GameVariables.Instance.GetPartCountLimit(editorNormLevel5, isVAB: false);
						}
						else
						{
							float editorNormLevel6 = ((float)node.mission.situation.gameParameters.CustomParams<MissionParamsFacilities>().facilityLevelVAB - 1f) / 2f;
							num2 = GameVariables.Instance.GetPartCountLimit(editorNormLevel6, isVAB: true);
						}
						if (num2 > 0)
						{
							if (vesselRestriction_PartCount.comparisonOperator == TestComparisonLessGreaterEqual.GreaterOrEqual)
							{
								if (vesselRestriction_PartCount.targetParts >= num2)
								{
									validator.AddNodeWarn(node, Localizer.Format("#autoLOC_8000275", (vesselSituation.location.facility == EditorFacility.const_2) ? EditorFacility.const_2.displayDescription() : EditorFacility.const_1.displayDescription(), num2.ToString(Localizer.Format("#autoLOC_8000301"))));
								}
							}
							else if (vesselRestriction_PartCount.targetParts > num2)
							{
								validator.AddNodeWarn(node, Localizer.Format("#autoLOC_8000275", (vesselSituation.location.facility == EditorFacility.const_2) ? EditorFacility.const_2.displayDescription() : EditorFacility.const_1.displayDescription(), num2.ToString(Localizer.Format("#autoLOC_8000301"))));
							}
						}
					}
					for (int j = 0; j < activeRestrictions.Count; j++)
					{
						if (i != j && activeRestrictions[i].SameComparatorWrapper(activeRestrictions[j]))
						{
							validator.AddNodeFail(node, Localizer.Format("#autoLOC_8003110", activeRestrictions[i].GetDisplayName()));
						}
					}
				}
			}
		}
		if (vesselSituation.location.situation != MissionSituation.VesselStartSituations.PRELAUNCH || string.IsNullOrEmpty(vesselSituation.location.launchSite))
		{
			return;
		}
		List<VesselSituation> allVesselSituations = vesselSituation.mission.GetAllVesselSituations();
		for (int k = 0; k < allVesselSituations.Count; k++)
		{
			if (allVesselSituations[k].location.situation == MissionSituation.VesselStartSituations.PRELAUNCH && allVesselSituations[k].location.launchSite == vesselSituation.location.launchSite && allVesselSituations[k].persistentId != vesselSituation.persistentId)
			{
				validator.AddNodeWarn(node, Localizer.Format("#autoLOC_8002019", allVesselSituations[k].vesselName, vesselSituation.location.launchSite));
			}
		}
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		vesselSituation.Save(node);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		ConfigNode configNode = node.GetNode("VESSELSITUATION");
		if (configNode != null)
		{
			vesselSituation.Load(configNode);
		}
	}
}

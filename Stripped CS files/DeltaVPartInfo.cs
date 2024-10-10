using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DeltaVPartInfo
{
	public class PartStageFuelMass
	{
		public float startMass;

		public float endMass;

		public PartResourceList resources;

		public PartStageFuelMass(Part part)
		{
			resources = new PartResourceList(part, part.Resources, simulationSet: true);
		}
	}

	public VesselDeltaV vesselDeltaV;

	public Part part;

	public int decoupleStage;

	public bool decoupleBeforeBurn;

	public int activationStage;

	public bool isDecoupler;

	public bool isStageSeparator;

	public bool isFairing;

	public bool isDockingPort;

	public bool isEngine;

	public bool isJettison;

	public bool isIntake;

	public bool isSolarPanel;

	public bool isGenerator;

	public ModuleResourceIntake moduleResourceIntake;

	public ModuleDeployableSolarPanel moduleDeployableSolarPanel;

	public ModuleGenerator moduleGenerator;

	public ModuleDockingNode moduleDockingNode;

	public ModuleDecouple moduleDecoupler;

	public ModuleAnchoredDecoupler moduleAnchoredDecoupler;

	public IStageSeparator moduleStageSeparator;

	public List<ModuleEngines> engines;

	public float dryMass;

	public float fuelMass;

	public float jettisonMass;

	public DictionaryValueList<int, PartStageFuelMass> stageFuelMass;

	public Part jettisonPart;

	public DeltaVPartInfo(Part part, VesselDeltaV vesselDeltaV)
	{
		this.part = part;
		this.vesselDeltaV = vesselDeltaV;
		engines = new List<ModuleEngines>();
		stageFuelMass = new DictionaryValueList<int, PartStageFuelMass>();
		isDecoupler = part.isDecoupler(out moduleDecoupler) || part.isAnchoredDecoupler(out moduleAnchoredDecoupler);
		moduleStageSeparator = part.FindModuleImplementing<IStageSeparator>();
		if (moduleStageSeparator != null)
		{
			isStageSeparator = true;
		}
		isFairing = part.isFairing();
		isDockingPort = part.isDockingPort(out moduleDockingNode);
		isEngine = part.isEngine(out engines);
		isIntake = part.isAirIntake(out moduleResourceIntake);
		isSolarPanel = part.isSolarPanel(out moduleDeployableSolarPanel);
		isGenerator = part.isGenerator(out moduleGenerator);
		decoupleBeforeBurn = false;
		CalculateMassValues();
		CalculateStagingValues();
	}

	public int CheckDecoupler(int chkStage)
	{
		part.isDecoupler(out var moduleDecouple);
		if (moduleDecouple != null)
		{
			AttachNode attachNode = ((moduleDecouple.ExplosiveNode == null) ? part.FindAttachNode(moduleDecouple.explosiveNodeID) : moduleDecouple.ExplosiveNode);
			if (attachNode != null && attachNode.attachedPart != null)
			{
				jettisonPart = attachNode.attachedPart;
			}
			if (moduleDecouple.partDecoupled && part.parent != null && attachNode != null && (attachNode.attachedPart == null || (attachNode.attachedPart != null && attachNode.attachedPart != part.parent && !part.parent.isFairing())))
			{
				if (GameSettings.LOG_DELTAV_VERBOSE)
				{
					Debug.LogFormat("[StageInfo]: Decoupler {0}-{1} facing away from parent. Was set to stage {2} now set to parent inverse stage {3}", part.partInfo.title, part.persistentId, chkStage, part.parent.inverseStage);
				}
				return part.parent.inverseStage;
			}
			if (part.parent != null && !moduleDecouple.stagingEnabled)
			{
				if (GameSettings.LOG_DELTAV_VERBOSE)
				{
					Debug.LogFormat("[StageInfo]: Decoupler {0}-{1} has staging disabled. Was set to stage {2} now set to parent inverse stage {3}", part.partInfo.title, part.persistentId, chkStage, part.parent.separationIndex);
				}
				activationStage = part.parent.separationIndex;
				return part.parent.separationIndex;
			}
		}
		else
		{
			part.isAnchoredDecoupler(out var moduleAnchoredDecoupler);
			if (moduleAnchoredDecoupler != null)
			{
				AttachNode attachNode = ((moduleAnchoredDecoupler.ExplosiveNode == null) ? part.FindAttachNode(moduleAnchoredDecoupler.explosiveNodeID) : moduleAnchoredDecoupler.ExplosiveNode);
				if (attachNode != null && attachNode.attachedPart != null)
				{
					jettisonPart = attachNode.attachedPart;
				}
				if (moduleAnchoredDecoupler.partDecoupled && part.parent != null && attachNode != null && attachNode.attachedPart != null && attachNode.attachedPart != part.parent && !part.parent.isFairing())
				{
					if (GameSettings.LOG_DELTAV_VERBOSE)
					{
						Debug.LogFormat("[StageInfo]: Decoupler {0}-{1} facing away from parent. Was set to stage {2} now set to parent inverse stage {3}", part.partInfo.title, part.persistentId, chkStage, part.parent.inverseStage);
					}
					return part.parent.inverseStage;
				}
				if (part.parent != null && !moduleAnchoredDecoupler.stagingEnabled)
				{
					if (GameSettings.LOG_DELTAV_VERBOSE)
					{
						Debug.LogFormat("[StageInfo]: Decoupler {0}-{1} has staging disabled. Was set to stage {2} now set to parent inverse stage {3}", part.partInfo.title, part.persistentId, chkStage, part.parent.separationIndex);
					}
					activationStage = part.parent.separationIndex;
					return part.parent.separationIndex;
				}
			}
		}
		return chkStage;
	}

	public int GetSeparationStage(int chkStage)
	{
		if (vesselDeltaV.SeparationStageIndexes.Count == 0)
		{
			return -1;
		}
		int num = -1;
		for (int i = 0; i < vesselDeltaV.SeparationStageIndexes.Count; i++)
		{
			if (vesselDeltaV.SeparationStageIndexes[i] > num && vesselDeltaV.SeparationStageIndexes[i] <= chkStage)
			{
				num = Math.Max(num, vesselDeltaV.SeparationStageIndexes[i]);
			}
		}
		return num;
	}

	public void StageStartFuelMass(int stage)
	{
		PartStageFuelMass partStageFuelMass = null;
		if (stageFuelMass.ContainsKey(stage))
		{
			partStageFuelMass = stageFuelMass[stage];
		}
		else
		{
			partStageFuelMass = new PartStageFuelMass(part);
			stageFuelMass.Add(stage, partStageFuelMass);
		}
		partStageFuelMass.startMass = GetCurrentFuelMass();
	}

	public void StageEndFuelMass(int stage)
	{
		PartStageFuelMass partStageFuelMass = null;
		partStageFuelMass = ((!stageFuelMass.ContainsKey(stage)) ? new PartStageFuelMass(part) : stageFuelMass[stage]);
		partStageFuelMass.endMass = GetCurrentFuelMass();
	}

	public void CalculateMassValues()
	{
		dryMass = 0f;
		fuelMass = 0f;
		jettisonMass = 0f;
		float num = 0f;
		num = (num = part.mass);
		for (int i = 0; i < part.Modules.Count; i++)
		{
			if (part.Modules[i] is IPartMassModifier)
			{
				IPartMassModifier partMassModifier = part.Modules[i] as IPartMassModifier;
				if (partMassModifier.GetModuleMassChangeWhen() == ModifierChangeWhen.STAGED)
				{
					float moduleMass = partMassModifier.GetModuleMass(num, ModifierStagingSituation.UNSTAGED);
					jettisonMass += moduleMass;
					isJettison = true;
				}
			}
		}
		float num2 = 0f;
		PartResourceList simulationResources = part.SimulationResources;
		int count = simulationResources.Count;
		while (count-- > 0)
		{
			PartResource partResource = simulationResources[count];
			PartResourceDefinition info = partResource.info;
			num2 += info.density * (float)partResource.amount;
		}
		dryMass += num;
		fuelMass += num2;
	}

	public void CalculateStagingValues()
	{
		activationStage = part.inverseStage;
		if (isEngine)
		{
			DeltaVEngineInfo deltaVEngineInfo = vesselDeltaV.WorkingEngineInfo.Get(part);
			if (deltaVEngineInfo != null)
			{
				activationStage = Math.Max(activationStage, deltaVEngineInfo.startBurnStage);
			}
		}
		int num = part.separationIndex;
		if (num > activationStage)
		{
			num = activationStage;
		}
		if (isDecoupler)
		{
			num = CheckDecoupler(num);
			if (!vesselDeltaV.SeparationStageIndexes.Contains(num))
			{
				vesselDeltaV.ResetSeparationIndexes();
			}
		}
		decoupleStage = GetSeparationStage(num);
		if (activationStage < decoupleStage)
		{
			decoupleStage = activationStage;
		}
		if (isEngine && activationStage == 0 && decoupleStage == activationStage)
		{
			Part decouplerParent = null;
			bool flag = false;
			if (FindDecouplerParent(part, out decouplerParent))
			{
				DeltaVPartInfo deltaVPartInfo = vesselDeltaV.PartInfo.Get(decouplerParent);
				if (deltaVPartInfo != null && deltaVPartInfo.activationStage == 0)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				decoupleStage = -1;
			}
		}
		if (isEngine)
		{
			if (activationStage <= decoupleStage)
			{
				decoupleBeforeBurn = true;
			}
			else
			{
				decoupleBeforeBurn = false;
			}
		}
	}

	public bool FindDecouplerParent(Part inPart, out Part decouplerParent)
	{
		decouplerParent = null;
		if (inPart.parent != null)
		{
			if (inPart.parent.HasModuleImplementing<ModuleDecouple>())
			{
				decouplerParent = inPart.parent;
				return true;
			}
			return FindDecouplerParent(inPart.parent, out decouplerParent);
		}
		return false;
	}

	public void ResetStartSimulationResources(int stage)
	{
		if (stageFuelMass.ContainsKey(stage))
		{
			part.ResetSimulationResources(stageFuelMass[stage].resources);
		}
		else
		{
			part.ResetSimulationResources();
		}
	}

	public bool JettisonInStage(int stage)
	{
		if (isJettison && decoupleStage < stage && activationStage >= stage)
		{
			return true;
		}
		return false;
	}

	public float GetCurrentFuelMass()
	{
		float num = 0f;
		PartResourceList simulationResources = part.SimulationResources;
		int count = simulationResources.Count;
		while (count-- > 0)
		{
			PartResource partResource = simulationResources[count];
			PartResourceDefinition info = partResource.info;
			num += info.density * (float)partResource.amount;
		}
		return num;
	}

	public float GetStageStartMass(int stage)
	{
		if (stageFuelMass.ContainsKey(stage))
		{
			return stageFuelMass[stage].startMass;
		}
		return fuelMass;
	}

	public float GetStageEndMass(int stage)
	{
		if (stageFuelMass.ContainsKey(stage))
		{
			return stageFuelMass[stage].endMass;
		}
		return fuelMass;
	}
}

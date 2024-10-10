using System;
using System.Collections;
using System.Collections.Generic;
using CommNet;
using Experience.Effects;
using ns37;
using ns9;
using UnityEngine;

public class ModuleScienceExperiment : PartModule, IScienceDataContainer
{
	public delegate void ResetCallback();

	[Serializable]
	public class EVASituation : ScriptableObject, IConfigNode
	{
		public int priority = 1;

		public uint situationMask;

		public bool RequiresAtmosphere;

		public bool RequiresNoAtmosphere;

		public float MinTemp = float.MinValue;

		public float MaxTemp = float.MaxValue;

		public string KerbalAction;

		public List<string> ResultString;

		public float dialogDelay;

		public void Load(ConfigNode node)
		{
			ResultString = new List<string>();
			node.TryGetValue("priority", ref priority);
			node.TryGetValue("situationMask", ref situationMask);
			node.TryGetValue("requiresAtmosphere", ref RequiresAtmosphere);
			node.TryGetValue("requiresNoAtmosphere", ref RequiresNoAtmosphere);
			node.TryGetValue("minTemp", ref MinTemp);
			node.TryGetValue("maxTemp", ref MaxTemp);
			node.TryGetValue("kerbalAction", ref KerbalAction);
			ResultString = node.GetValuesList("resultString");
			node.TryGetValue("dialogDelay", ref dialogDelay);
		}

		public void Save(ConfigNode node)
		{
		}
	}

	[KSPField]
	public string experimentID;

	[KSPField]
	public string experimentActionName = "#autoLOC_6001437";

	[KSPField]
	public string resetActionName = "#autoLOC_6001438";

	[KSPField]
	public string reviewActionName = "#autoLOC_6001439";

	[KSPField]
	public bool useStaging;

	[KSPField]
	public bool useActionGroups;

	[KSPField]
	public bool hideUIwhenUnavailable;

	[KSPField]
	public bool rerunnable;

	[KSPField]
	public bool resettable = true;

	[KSPField]
	public bool resettableOnEVA = true;

	[KSPField]
	public bool hideFxModuleUI = true;

	[KSPField]
	public string transmitWarningText = Localizer.Format("#autoLOC_6001026");

	[KSPField]
	public string collectWarningText = Localizer.Format("#autoLOC_6001027");

	[KSPField]
	public string resourceToReset = "ElectricCharge";

	[KSPField]
	public float resourceResetCost = 1f;

	public ResetCallback OnExperimentReset;

	[KSPField]
	public float xmitDataScalar = 0.5f;

	[KSPField]
	public float scienceValueRatio = 1f;

	[KSPField]
	public bool showScienceValueRatio;

	[KSPField]
	public bool dataIsCollectable;

	[KSPField]
	public string collectActionName = "#autoLOC_238018";

	[KSPField]
	public float interactionRange = 1.5f;

	[KSPField]
	public int usageReqMaskInternal = 1;

	[KSPField]
	public int usageReqMaskExternal = -1;

	[KSPField]
	public bool availableShielded = true;

	[KSPField]
	public bool deployableSeated = true;

	[KSPField]
	public bool requiresInventoryPart;

	[KSPField]
	public string requiredInventoryPart = "evaScienceKit";

	[KSPField]
	public bool requiresEVASituation;

	public List<EVASituation> evaSituations;

	[KSPField]
	public float dialogDelay;

	[KSPField]
	public string extraResultString;

	public bool kerbalInSeat;

	[KSPField(isPersistant = true)]
	public bool Deployed;

	[KSPField(isPersistant = true)]
	public bool Inoperable;

	[KSPField(guiActive = false, guiName = "")]
	public string usageReqMessage = "";

	[KSPField]
	public double cooldownTimer;

	public bool useCooldown;

	[KSPField(isPersistant = true)]
	public double cooldownToGo;

	[KSPField(guiActive = false, guiName = "#autoLOC_6001861")]
	public string cooldownString;

	public ScienceExperiment experiment;

	public ScienceSubject subject;

	public ScienceData experimentData;

	public bool DeployEventDisabled;

	public ExperimentsResultDialog resultsDialog;

	public PopupDialog overwriteDialog;

	public int[] fxModuleIndices;

	public List<IScalarModule> fxModules;

	public ModuleInventoryPart inventory;

	public EVASituation validEvaSituation;

	public bool resettingExperiment;

	public double dialogTimer;

	public bool showDialogAfter;

	public ExperimentSituations situation;

	public bool containersDirty;

	public bool hasContainer;

	public bool HasExperimentData => experimentData != null;

	public override void OnAwake()
	{
		GameEvents.onVesselSituationChange.Add(OnVesselSituationChange);
		GameEvents.onVesselStandardModification.Add(OnVesselModified);
		GameEvents.onGamePause.Add(OnGamePause);
		GameEvents.onGameUnpause.Add(OnGameUnpause);
		GameEvents.onCommandSeatInteraction.Add(OnCommandSeatInteraction);
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("FxModules"))
		{
			fxModuleIndices = KSPUtil.ParseArray(node.GetValue("FxModules"), int.Parse);
		}
		if (node.HasNode("ScienceData"))
		{
			experimentData = new ScienceData(node.GetNode("ScienceData"));
		}
		if (!node.HasNode("EVASITUATION"))
		{
			return;
		}
		evaSituations = new List<EVASituation>();
		ConfigNode[] nodes = node.GetNodes("EVASITUATION");
		for (int i = 0; i < nodes.Length; i++)
		{
			EVASituation eVASituation = (EVASituation)ScriptableObject.CreateInstance(typeof(EVASituation));
			eVASituation.Load(nodes[i]);
			if (!string.IsNullOrEmpty(eVASituation.KerbalAction) && typeof(KerbalEVA).GetMethod(eVASituation.KerbalAction) != null)
			{
				evaSituations.Add(eVASituation);
			}
		}
	}

	public override void OnSave(ConfigNode node)
	{
		if (experimentData != null)
		{
			experimentData.Save(node.AddNode("ScienceData"));
		}
	}

	public override void OnStart(StartState state)
	{
		if (requiresInventoryPart)
		{
			inventory = base.part.FindModuleImplementing<ModuleInventoryPart>();
			GameEvents.onModuleInventoryChanged.Add(OnModuleInventoryChanged);
		}
		useCooldown = cooldownTimer > 0.0;
		base.Fields["cooldownString"].guiActive = useCooldown;
		if (!DeployEventDisabled)
		{
			experiment = ResearchAndDevelopment.GetExperiment(experimentID);
		}
		FindContainer();
		updateModuleUI();
		if (useStaging && base.part.stagingIcon == string.Empty && overrideStagingIconIfBlank)
		{
			base.part.stagingIcon = "SCIENCE_GENERIC";
		}
		if (fxModuleIndices == null)
		{
			return;
		}
		fxModules = new List<IScalarModule>();
		int i = 0;
		for (int num = fxModuleIndices.Length; i < num; i++)
		{
			int num2 = fxModuleIndices[i];
			if (base.part.Modules[num2] is IScalarModule)
			{
				IScalarModule scalarModule = (IScalarModule)base.part.Modules[num2];
				if (hideFxModuleUI)
				{
					scalarModule.SetUIWrite(state: false);
					scalarModule.SetUIRead(state: false);
				}
				fxModules.Add(scalarModule);
			}
			else
			{
				Debug.LogError("[ModuleScienceExperiment]: Part Module " + num2 + " doesn't implement IScalarModule", base.gameObject);
			}
		}
	}

	public void OnDestroy()
	{
		GameEvents.onGamePause.Remove(OnGamePause);
		GameEvents.onGameUnpause.Remove(OnGameUnpause);
		GameEvents.onVesselSituationChange.Remove(OnVesselSituationChange);
		GameEvents.onVesselStandardModification.Remove(OnVesselModified);
		GameEvents.onCommandSeatInteraction.Remove(OnCommandSeatInteraction);
		GameEvents.onModuleInventoryChanged.Remove(OnModuleInventoryChanged);
	}

	public void Update()
	{
		if (useCooldown && HighLogic.LoadedSceneIsFlight)
		{
			double num = TimeWarp.deltaTime;
			if (cooldownToGo > num)
			{
				cooldownToGo -= num;
				cooldownString = KSPUtil.PrintTimeCompact(cooldownToGo, explicitPositive: false);
			}
			else
			{
				cooldownToGo = 0.0;
				cooldownString = "Ready";
			}
		}
		if (containersDirty)
		{
			FindContainer();
			updateModuleUI();
		}
	}

	public override void OnInventoryModeDisable()
	{
		GameEvents.onModuleInventoryChanged.Remove(OnModuleInventoryChanged);
	}

	public override void OnInventoryModeEnable()
	{
		containersDirty = true;
	}

	public void OnVesselSituationChange(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> vcs)
	{
		if (vcs.host == base.vessel)
		{
			if (requiresInventoryPart && inventory == null)
			{
				inventory = base.part.FindModuleImplementing<ModuleInventoryPart>();
			}
			updateModuleUI();
		}
	}

	public void OnModuleInventoryChanged(ModuleInventoryPart inventoryPart)
	{
		if (requiresInventoryPart && inventory == null)
		{
			inventory = base.part.FindModuleImplementing<ModuleInventoryPart>();
		}
		if (inventory != null && inventoryPart != null && inventory.part.persistentId == inventoryPart.part.persistentId)
		{
			updateModuleUI();
		}
	}

	public void OnCommandSeatInteraction(KerbalEVA eva, bool entered)
	{
		if (!(eva == null) && !(eva.part == null) && !(eva.part != base.part))
		{
			kerbalInSeat = entered;
			updateModuleUI();
		}
	}

	public void OnGameUnpause()
	{
		if (overwriteDialog != null)
		{
			overwriteDialog.gameObject.SetActive(value: true);
		}
		if (resultsDialog != null)
		{
			resultsDialog.gameObject.SetActive(value: true);
		}
	}

	public void OnGamePause()
	{
		if (overwriteDialog != null)
		{
			overwriteDialog.gameObject.SetActive(value: false);
		}
		if (resultsDialog != null)
		{
			resultsDialog.gameObject.SetActive(value: false);
		}
	}

	public void updateModuleUI()
	{
		if (base.vessel != null)
		{
			if (experiment != null && experiment.IsUnlocked() && (!hideUIwhenUnavailable || experiment.IsAvailableWhile(ScienceUtil.GetExperimentSituation(base.vessel.EVALadderVessel), base.vessel.mainBody)) && (!requiresInventoryPart || (inventory != null && inventory.ContainsPart(requiredInventoryPart))) && (deployableSeated || !kerbalInSeat) && resultsDialog == null)
			{
				base.Events["DeployExperiment"].active = (!Deployed || rerunnable) && !DeployEventDisabled && usageReqMaskInternal != -1;
				base.Events["DeployExperiment"].guiName = experimentActionName;
				base.Actions["DeployAction"].active = useActionGroups && !DeployEventDisabled;
				base.Actions["DeployAction"].guiName = experimentActionName;
				base.Events["DeployExperimentExternal"].active = (!Deployed || rerunnable) && !DeployEventDisabled && usageReqMaskExternal != -1;
				base.Events["DeployExperimentExternal"].guiName = experimentActionName;
				base.Events["DeployExperimentExternal"].unfocusedRange = interactionRange;
			}
			else
			{
				base.Events["DeployExperiment"].active = false;
				base.Actions["DeployAction"].active = false;
				base.Actions["DeployAction"].guiName = experimentActionName;
				base.Events["DeployExperimentExternal"].active = false;
			}
			base.Events["ResetExperiment"].active = Deployed && !Inoperable && resettable && !rerunnable && resultsDialog == null;
			base.Events["ResetExperiment"].guiName = resetActionName;
			base.Events["ResetExperimentExternal"].active = Deployed && !Inoperable && resettableOnEVA && !rerunnable && resultsDialog == null;
			base.Events["ResetExperimentExternal"].guiName = resetActionName;
			base.Events["ResetExperimentExternal"].unfocusedRange = interactionRange;
			base.Actions["ResetAction"].active = useActionGroups && !Inoperable && resultsDialog == null;
			base.Actions["ResetAction"].guiName = resetActionName;
			base.Events["ReviewDataEvent"].active = Deployed && experimentData != null && resultsDialog == null;
			base.Events["ReviewDataEvent"].guiName = reviewActionName;
			base.Events["CollectDataExternalEvent"].guiName = Localizer.Format(collectActionName);
			bool flag = dataIsCollectable && experimentData != null;
			base.Events["CollectDataExternalEvent"].active = flag;
			base.Events["TransferDataEvent"].active = hasContainer && flag;
			base.Events["CollectDataExternalEvent"].unfocusedRange = interactionRange;
			base.Events["CleanUpExperimentExternal"].active = Deployed && Inoperable && resettableOnEVA;
			base.Events["CleanUpExperimentExternal"].unfocusedRange = interactionRange;
		}
		else
		{
			base.Actions["DeployAction"].active = useActionGroups && !DeployEventDisabled;
			base.Actions["DeployAction"].guiName = experimentActionName;
			base.Actions["ResetAction"].active = useActionGroups;
			base.Actions["ResetAction"].guiName = resetActionName;
		}
	}

	public bool ValidEVASituation()
	{
		validEvaSituation = null;
		ExperimentSituations experimentSituation = ScienceUtil.GetExperimentSituation(base.vessel.EVALadderVessel);
		for (int i = 0; i < evaSituations.Count; i++)
		{
			bool flag = true;
			EVASituation eVASituation = evaSituations[i];
			if ((eVASituation.situationMask & (uint)experimentSituation) == 0)
			{
				flag = false;
			}
			if (eVASituation.RequiresAtmosphere && base.vessel.EVALadderVessel.atmDensity <= 0.0)
			{
				flag = false;
			}
			if (eVASituation.RequiresNoAtmosphere && base.vessel.EVALadderVessel.atmDensity > 0.0)
			{
				flag = false;
			}
			if (eVASituation.MinTemp > float.MinValue && base.vessel != null && base.vessel.mainBody != null && (float)base.vessel.mainBody.bodyTemperature < eVASituation.MinTemp)
			{
				flag = false;
			}
			if (eVASituation.MaxTemp < float.MaxValue && base.vessel != null && base.vessel.mainBody != null && (float)base.vessel.mainBody.bodyTemperature > eVASituation.MaxTemp)
			{
				flag = false;
			}
			if (base.vessel != null && base.vessel.isEVA && base.vessel.evaController.OnALadder)
			{
				flag = false;
			}
			if (base.vessel != null && base.vessel.isEVA && base.vessel.evaController.InConstructionMode)
			{
				flag = false;
			}
			if (flag && (validEvaSituation == null || (validEvaSituation != null && eVASituation.priority < validEvaSituation.priority)))
			{
				validEvaSituation = eVASituation;
			}
		}
		if (validEvaSituation != null)
		{
			return true;
		}
		return false;
	}

	[KSPEvent(active = true, guiActive = true, guiName = "#autoLOC_502050")]
	public void DeployExperiment()
	{
		if (!(base.part == null) && !(base.part.vessel == null))
		{
			if (experiment == null)
			{
				Debug.LogErrorFormat("[ModuleScienceExperiment]: Cannot deploy experiment '{0}'. ScienceExperiment reference is Null.", experimentID);
			}
			else if (!availableShielded && base.part.ShieldedFromAirstream)
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_238290"), 6f, ScreenMessageStyle.UPPER_LEFT);
			}
			else if (ScienceUtil.RequiredUsageInternalAvailable(base.vessel, base.part, (ExperimentUsageReqs)usageReqMaskInternal, experiment, ref usageReqMessage))
			{
				if (useCooldown && cooldownTimer > 0.0)
				{
					ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_238298", KSPUtil.PrintTimeCompact(cooldownToGo, explicitPositive: false)), 6f, ScreenMessageStyle.UPPER_LEFT);
				}
				else
				{
					base.Events["DeployExperiment"].active = false;
					StartCoroutine(gatherData(showDialog: true));
				}
			}
			else
			{
				ScreenMessages.PostScreenMessage("<b><color=orange>" + usageReqMessage + "</color></b>", 6f, ScreenMessageStyle.UPPER_LEFT);
			}
		}
		else
		{
			Debug.LogErrorFormat("[ModuleScienceExperiment]: Cannot deploy experiment '{0}'. Part/Vessel reference is Null.", experimentID);
		}
	}

	[KSPAction("Deploy")]
	public void DeployAction(KSPActionParam actParams)
	{
		if (!(base.part == null) && !(base.part.vessel == null))
		{
			if (experiment == null)
			{
				Debug.LogErrorFormat("[ModuleScienceExperiment]: Cannot deploy experiment '{0}'. ScienceExperiment reference is Null.", experimentID);
			}
			else if (!availableShielded && base.part.ShieldedFromAirstream)
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_238318"), 6f, ScreenMessageStyle.UPPER_LEFT);
			}
			else if (ScienceUtil.RequiredUsageInternalAvailable(base.vessel, base.part, (ExperimentUsageReqs)usageReqMaskInternal, experiment, ref usageReqMessage))
			{
				if (!Deployed)
				{
					if (useCooldown && cooldownTimer > 0.0)
					{
						ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_238298", KSPUtil.PrintTimeCompact(cooldownToGo, explicitPositive: false)), 6f, ScreenMessageStyle.UPPER_LEFT);
					}
					else
					{
						StartCoroutine(gatherData(showDialog: true));
					}
				}
			}
			else
			{
				ScreenMessages.PostScreenMessage("<b><color=orange>" + usageReqMessage + "</color></b>", 6f, ScreenMessageStyle.UPPER_LEFT);
			}
		}
		else
		{
			Debug.LogErrorFormat("[ModuleScienceExperiment]: Cannot deploy experiment '{0}'. Part/Vessel reference is Null.", experimentID);
		}
	}

	public override void OnActive()
	{
		if (!useStaging || !stagingEnabled)
		{
			return;
		}
		if (experiment == null)
		{
			Debug.LogErrorFormat("[ModuleScienceExperiment]: Cannot deploy experiment '{0}'. ScienceExperiment reference is Null.", experimentID);
		}
		else if (ScienceUtil.RequiredUsageInternalAvailable(base.vessel, base.part, (ExperimentUsageReqs)usageReqMaskInternal, experiment, ref usageReqMessage))
		{
			if (!Deployed)
			{
				StartCoroutine(gatherData(showDialog: false));
			}
			else
			{
				base.part.stackIcon.SetIconColor(XKCDColors.Cyan);
			}
		}
		else
		{
			ScreenMessages.PostScreenMessage("<b><color=orange>" + usageReqMessage + "</color></b>", 6f, ScreenMessageStyle.UPPER_LEFT);
		}
	}

	public IEnumerator gatherData(bool showDialog)
	{
		if (requiresInventoryPart)
		{
			if (!(inventory != null) || !inventory.ContainsPart(requiredInventoryPart))
			{
				updateModuleUI();
				yield break;
			}
			if (!ValidEVASituation())
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_238424", experiment.experimentTitle), 5f, ScreenMessageStyle.UPPER_CENTER);
				updateModuleUI();
				yield break;
			}
		}
		if (requiresEVASituation && validEvaSituation == null)
		{
			Debug.LogErrorFormat("[ModuleScienceExperiment]: Cannot deploy experiment '{0}'. requires EVA situation and no valid Situation defined to Module.", experimentID);
			updateModuleUI();
			yield break;
		}
		if (HasExperimentData)
		{
			ScienceExperiment scienceExperiment = ResearchAndDevelopment.GetExperiment(experimentData.subjectID.Split('@')[0]);
			overwriteDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("OverwriteExperiment", Localizer.Format("#autoLOC_238370", scienceExperiment.experimentTitle), Localizer.Format("#autoLOC_238371", scienceExperiment.experimentTitle), HighLogic.UISkin, new DialogGUIButton(Localizer.Format("#autoLOC_238371", scienceExperiment.experimentTitle), delegate
			{
				dumpData();
				StartCoroutine(gatherData(showDialog));
			}), new DialogGUIButton(Localizer.Format("#autoLOC_253501"), updateModuleUI)), persistAcrossScenes: false, HighLogic.UISkin);
			overwriteDialog.OnDismiss = updateModuleUI;
			yield break;
		}
		if (experiment.requireAtmosphere && base.part.ShieldedFromAirstream)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_238379", base.part.partInfo.title), 5f, ScreenMessageStyle.UPPER_LEFT);
		}
		else
		{
			situation = ScienceUtil.GetExperimentSituation(base.vessel);
			if (experiment.IsAvailableWhile(situation, base.vessel.mainBody))
			{
				yield return StartCoroutine(SetFXModules(1f));
				if (requiresEVASituation)
				{
					if (validEvaSituation.KerbalAction == "PlayGolf" && base.vessel.evaController != null)
					{
						base.vessel.evaController.PlayGolf(OnScienceComplete);
						showDialogAfter = showDialog;
						yield break;
					}
					if (validEvaSituation.KerbalAction == "Banana" && base.vessel.evaController != null)
					{
						base.vessel.evaController.Banana(OnScienceComplete);
						showDialogAfter = showDialog;
						yield break;
					}
					if (validEvaSituation.KerbalAction == "Dzhanibekov" && base.vessel.evaController != null)
					{
						base.vessel.evaController.Dzhanibekov(OnScienceComplete);
						showDialogAfter = showDialog;
						yield break;
					}
				}
				showDialogAfter = showDialog;
				OnScienceComplete();
			}
			else
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_238424", experiment.experimentTitle), 5f, ScreenMessageStyle.UPPER_CENTER);
			}
		}
		updateModuleUI();
	}

	public void OnScienceComplete()
	{
		StartCoroutine(OnScienceCompleteDelay());
	}

	public IEnumerator OnScienceCompleteDelay()
	{
		Deployed = true;
		if (useStaging)
		{
			base.part.stackIcon.SetIconColor(XKCDColors.Cyan);
		}
		string text = "";
		string text2 = string.Empty;
		if (experiment.BiomeIsRelevantWhile(situation))
		{
			Vessel eVALadderVessel = base.vessel.EVALadderVessel;
			if (eVALadderVessel.landedAt != string.Empty)
			{
				text = Vessel.GetLandedAtString(eVALadderVessel.landedAt);
				text2 = Localizer.Format(eVALadderVessel.displaylandedAt);
			}
			else
			{
				text = ScienceUtil.GetExperimentBiome(eVALadderVessel.mainBody, eVALadderVessel.latitude, eVALadderVessel.longitude);
				text2 = ScienceUtil.GetBiomedisplayName(eVALadderVessel.mainBody, text);
			}
			if (text2 == string.Empty)
			{
				text2 = text;
			}
		}
		subject = ResearchAndDevelopment.GetExperimentSubject(experiment, situation, base.vessel.mainBody, text, text2);
		if (requiresEVASituation && validEvaSituation.ResultString.Count > 0)
		{
			if (validEvaSituation.ResultString.Count > 1)
			{
				int index = new System.Random().Next(0, validEvaSituation.ResultString.Count - 1);
				extraResultString = validEvaSituation.ResultString[index];
			}
			else
			{
				extraResultString = validEvaSituation.ResultString[0];
			}
		}
		experimentData = new ScienceData(experiment.baseValue * experiment.dataScale, scienceValueRatio, xmitDataScalar, 0f, subject.id, subject.title, triggered: false, base.part.flightID, extraResultString);
		GameEvents.OnExperimentDeployed.Fire(experimentData);
		if (showDialogAfter)
		{
			if (requiresEVASituation && validEvaSituation != null)
			{
				dialogDelay = validEvaSituation.dialogDelay;
			}
			dialogTimer = Planetarium.GetUniversalTime() + (double)dialogDelay;
			while (Planetarium.GetUniversalTime() <= dialogTimer)
			{
				yield return null;
			}
			reviewData();
		}
		else
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_238419", base.part.partInfo.title, experimentData.dataAmount.ToString(), subject.title), 8f, ScreenMessageStyle.UPPER_LEFT);
		}
		yield return null;
		updateModuleUI();
	}

	public IEnumerator SetFXModules(float tgt)
	{
		Debug.Log("[Experiments]: Setting FX Modules to " + tgt + "...", base.gameObject);
		if (fxModules != null)
		{
			bool allFXDone = false;
			while (!allFXDone)
			{
				allFXDone = true;
				int i = 0;
				for (int count = fxModules.Count; i < count; i++)
				{
					if (!(Mathf.Abs(fxModules[i].GetScalar - tgt) < 0.01f))
					{
						allFXDone = false;
						fxModules[i].SetScalar(tgt);
					}
				}
				if (!allFXDone)
				{
					yield return null;
				}
			}
		}
		Debug.Log("[Experiments]: FX Modules set: " + tgt, base.gameObject);
	}

	public void endExperiment(ScienceData pageData)
	{
		if (resettable && !Inoperable)
		{
			StartCoroutine(resetExperiment());
			return;
		}
		dumpData();
		updateModuleUI();
	}

	public void dumpData()
	{
		subject = null;
		experimentData = null;
		resultsDialog = null;
	}

	public void sendDataToComms(ScienceData pageData)
	{
		resultsDialog = null;
		IScienceDataTransmitter bestTransmitter = ScienceUtil.GetBestTransmitter(base.vessel);
		if (bestTransmitter != null)
		{
			List<ScienceData> list = new List<ScienceData>();
			list.Add(pageData);
			bestTransmitter.TransmitData(list);
			if (!rerunnable)
			{
				SetInoperable();
			}
			endExperiment(pageData);
			if (useCooldown)
			{
				cooldownToGo = cooldownTimer;
			}
		}
		else
		{
			if (CommNetScenario.CommNetEnabled)
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_238505"), 3f, ScreenMessageStyle.UPPER_CENTER);
			}
			else
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_238507"), 3f, ScreenMessageStyle.UPPER_CENTER);
			}
			updateModuleUI();
		}
	}

	public void sendDataToLab(ScienceData pageData)
	{
		resultsDialog = null;
		ScienceLabSearch scienceLabSearch = new ScienceLabSearch(base.vessel, experimentData);
		if (scienceLabSearch.NextLabForDataFound)
		{
			StartCoroutine(scienceLabSearch.NextLabForData.ProcessData(experimentData));
			if (!rerunnable)
			{
				SetInoperable();
			}
			endExperiment(pageData);
			if (useCooldown)
			{
				cooldownToGo = cooldownTimer;
			}
		}
		else
		{
			scienceLabSearch.PostErrorToScreen();
			updateModuleUI();
		}
	}

	[KSPEvent(active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = false, unfocusedRange = 1.5f)]
	public void CollectDataExternalEvent()
	{
		TransferToContainer(FlightGlobals.ActiveVessel.FindPartModuleImplementing<ModuleScienceContainer>(), FlightGlobals.ActiveVessel.GetDisplayName());
		updateModuleUI();
	}

	public void TransferToContainer(ModuleScienceContainer container, string destName)
	{
		if (container != null)
		{
			if (experimentData != null)
			{
				if (!rerunnable && GameSettings.SCIENCE_EXPERIMENT_SHOW_TRANSFER_WARNING)
				{
					UIConfirmDialog.Spawn(Localizer.Format("#autoLOC_236416"), collectWarningText, Localizer.Format("#autoLOC_226976"), Localizer.Format("#autoLOC_7003412"), Localizer.Format("#autoLOC_360842"), delegate(bool b)
					{
						OnTransferWarning(b);
						onCollectData(container);
					}, delegate
					{
					});
				}
				else
				{
					onCollectData(container);
				}
			}
			else
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_238567", base.part.partInfo.title), 3f, ScreenMessageStyle.UPPER_CENTER);
			}
		}
		else
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_238572", base.part.partInfo.title, destName), 5f, ScreenMessageStyle.UPPER_CENTER);
		}
	}

	public void OnTransferWarning(bool dontShowAgain)
	{
		if (dontShowAgain == GameSettings.SCIENCE_EXPERIMENT_SHOW_TRANSFER_WARNING)
		{
			GameSettings.SCIENCE_EXPERIMENT_SHOW_TRANSFER_WARNING = !dontShowAgain;
			GameSettings.SaveGameSettingsOnly();
		}
	}

	public void onCollectData(ModuleScienceContainer vesselContainer)
	{
		if (vesselContainer != null)
		{
			if (vesselContainer.StoreData(new List<IScienceDataContainer> { this }, dumpRepeats: false))
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_238582", vesselContainer.part.partInfo.title), 5f, ScreenMessageStyle.UPPER_LEFT);
				if (useCooldown)
				{
					cooldownToGo = cooldownTimer;
				}
			}
			else
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_238589", vesselContainer.part.partInfo.title), 5f, ScreenMessageStyle.UPPER_LEFT);
			}
		}
		else
		{
			Debug.LogWarning("[ScienceExperiment]: This container module reference was non-null just a few seconds ago. This is why we can't have nice things.");
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_238595", base.part.partInfo.title), 5f, ScreenMessageStyle.UPPER_LEFT);
		}
	}

	[KSPEvent(active = true, guiActive = true, guiName = "#autoLOC_6001439")]
	public void ReviewDataEvent()
	{
		reviewData();
		updateModuleUI();
	}

	public void reviewData()
	{
		resultsDialog = ExperimentsResultDialog.DisplayResult(new ExperimentResultDialogPage(base.part, experimentData, experimentData.baseTransmitValue, experimentData.transmitBonus, !rerunnable, transmitWarningText, showResetOption: true, new ScienceLabSearch(base.vessel, experimentData), endExperiment, keepData, sendDataToComms, sendDataToLab));
		PartItemTransfer.DismissActive();
	}

	public void keepData(ScienceData pageData)
	{
		resultsDialog = null;
		updateModuleUI();
	}

	[KSPEvent(active = true, guiActive = true, guiName = "#autoLOC_6001436")]
	public void ResetExperiment()
	{
		base.Events["ResetExperiment"].active = false;
		StartCoroutine(resetExperiment());
	}

	[KSPAction("#autoLOC_6001436")]
	public void ResetAction(KSPActionParam actParams)
	{
		StartCoroutine(resetExperiment());
	}

	[KSPEvent(active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = false, guiName = "#autoLOC_6002397")]
	public void DeployExperimentExternal()
	{
		if (!(base.part == null) && !(base.part.vessel == null))
		{
			if (experiment == null)
			{
				Debug.LogErrorFormat("[ModuleScienceExperiment]: Cannot deploy experiment '{0}'. ScienceExperiment reference is Null.", experimentID);
			}
			else if (!availableShielded && base.part.ShieldedFromAirstream)
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_238643"), 6f, ScreenMessageStyle.UPPER_LEFT);
			}
			else if (ScienceUtil.RequiredUsageExternalAvailable(base.vessel, FlightGlobals.ActiveVessel, (ExperimentUsageReqs)usageReqMaskExternal, experiment, ref usageReqMessage))
			{
				if (useCooldown && cooldownTimer > 0.0)
				{
					ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_238298", KSPUtil.PrintTimeCompact(cooldownToGo, explicitPositive: false)), 6f, ScreenMessageStyle.UPPER_LEFT);
				}
				else
				{
					base.Events["DeployExperimentExternal"].active = false;
					StartCoroutine(gatherData(showDialog: true));
				}
			}
			else
			{
				ScreenMessages.PostScreenMessage("<b><color=orange>" + usageReqMessage + "</color></b>", 6f, ScreenMessageStyle.UPPER_LEFT);
			}
		}
		else
		{
			Debug.LogErrorFormat("[ModuleScienceExperiment]: Cannot deploy experiment '{0}'. Part/Vessel reference is Null.", experimentID);
		}
	}

	[KSPEvent(active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = false, guiName = "#autoLOC_900305")]
	public void ResetExperimentExternal()
	{
		base.Events["ResetExperimentExternal"].active = false;
		StartCoroutine(resetExperiment());
	}

	public IEnumerator resetExperiment()
	{
		base.Events["ReviewDataEvent"].active = false;
		resettingExperiment = true;
		yield return StartCoroutine(SetFXModules(0f));
		if (useStaging)
		{
			base.part.stackIcon.SetIconColor(Color.white);
		}
		if (experimentData != null)
		{
			dumpData();
		}
		Deployed = false;
		Inoperable = false;
		updateModuleUI();
		if (OnExperimentReset != null)
		{
			OnExperimentReset();
			OnExperimentReset = null;
		}
		resettingExperiment = false;
	}

	public ScienceData[] GetData()
	{
		if (experimentData != null)
		{
			return new ScienceData[1] { experimentData };
		}
		return new ScienceData[0];
	}

	public void ReturnData(ScienceData data)
	{
		if (experimentData == null)
		{
			experimentData = data;
			Deployed = true;
			Inoperable = false;
			updateModuleUI();
		}
	}

	public void DumpData(ScienceData data)
	{
		if (!rerunnable)
		{
			SetInoperable();
		}
		if (experimentData != null && data != null && experimentData.subjectID == data.subjectID)
		{
			endExperiment(experimentData);
		}
	}

	public void ReviewData()
	{
		if (Deployed && experimentData != null && resultsDialog == null)
		{
			reviewData();
			updateModuleUI();
		}
	}

	public int GetScienceCount()
	{
		if (experimentData == null)
		{
			return 0;
		}
		return 1;
	}

	public bool IsRerunnable()
	{
		return rerunnable;
	}

	public void SetInoperable()
	{
		ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_238754", base.part.partInfo.title), 6f, ScreenMessageStyle.UPPER_LEFT);
		Inoperable = true;
		updateModuleUI();
	}

	[KSPEvent(active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = false, guiName = "#autoLOC_6001862")]
	public void CleanUpExperimentExternal()
	{
		if (FlightGlobals.ActiveVessel.isEVA)
		{
			if (FlightGlobals.ActiveVessel.parts[0].protoModuleCrew[0].HasEffect<ScienceResetSkill>())
			{
				Inoperable = false;
				ResetExperiment();
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_238770", base.part.partInfo.title), 6f, ScreenMessageStyle.UPPER_LEFT);
			}
			else
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_238776"), 5f, ScreenMessageStyle.UPPER_CENTER);
			}
		}
		else
		{
			Debug.LogError("[ModuleScienceExperiment]: The CleanUp Event should only be availabel for EVAs!", base.gameObject);
		}
	}

	public void ReviewDataItem(ScienceData data)
	{
		if (data == experimentData)
		{
			reviewData();
		}
	}

	public override string GetInfo()
	{
		string text = "";
		text = text + "<color=" + XKCDColors.HexFormat.Cyan + ">" + experimentActionName + "</color>\n";
		text += Localizer.Format("#autoLOC_238797", RUIutils.GetYesNoUIString(dataIsCollectable));
		text += Localizer.Format("#autoLOC_238798", RUIutils.GetYesNoUIString(rerunnable));
		text += Localizer.Format("#autoLOC_238799", RUIutils.GetYesNoUIString(resettable));
		if (!rerunnable)
		{
			PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(resourceToReset.GetHashCode());
			string text2 = "";
			text2 = ((definition == null) ? resourceToReset : definition.displayName);
			text += Localizer.Format("#autoLOC_238802", text2);
			text += Localizer.Format("#autoLOC_238803", resourceResetCost);
		}
		if (showScienceValueRatio)
		{
			text += Localizer.Format("#autoLOC_8004390", (scienceValueRatio * 100f).ToString("N0"));
		}
		return text;
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLoc_6003059");
	}

	[KSPEvent(active = true, guiActive = true, guiName = "#autoLoc_6001496")]
	public void TransferDataEvent()
	{
		if (PartItemTransfer.Instance != null)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_238816"), 3f, ScreenMessageStyle.UPPER_CENTER);
		}
		else
		{
			ExperimentTransfer.Create(base.part, this, OnDataTransfer);
		}
	}

	public void FindContainer()
	{
		containersDirty = false;
		hasContainer = false;
		if (!(base.vessel != null))
		{
			return;
		}
		ModuleScienceContainer moduleScienceContainer = null;
		int count = base.vessel.Parts.Count;
		while (true)
		{
			if (count-- <= 0)
			{
				return;
			}
			Part part = base.vessel.Parts[count];
			if (part.State != PartStates.DEAD)
			{
				moduleScienceContainer = part.FindModuleImplementing<ModuleScienceContainer>();
				if ((object)moduleScienceContainer != null && moduleScienceContainer.canBeTransferredToInVessel)
				{
					break;
				}
			}
		}
		hasContainer = true;
	}

	public void OnDataTransfer(PartItemTransfer.DismissAction dma, Part containerPart)
	{
		if (dma == PartItemTransfer.DismissAction.ItemMoved && !(containerPart == null))
		{
			TransferToContainer(containerPart.FindModuleImplementing<ModuleScienceContainer>(), containerPart.partInfo.title);
			updateModuleUI();
		}
	}

	public void OnVesselModified(Vessel v)
	{
		if (v == base.vessel && HighLogic.LoadedSceneIsFlight)
		{
			containersDirty = true;
		}
	}

	public override bool IsStageable()
	{
		return useStaging;
	}

	public override bool StagingEnabled()
	{
		if (useStaging)
		{
			return stagingEnabled;
		}
		return false;
	}

	public override bool StagingToggleEnabledEditor()
	{
		return base.StagingToggleEnabledEditor();
	}

	public override bool StagingToggleEnabledFlight()
	{
		return base.StagingToggleEnabledFlight();
	}

	public override string GetStagingEnableText()
	{
		if (!string.IsNullOrEmpty(stagingEnableText))
		{
			return stagingEnableText;
		}
		return Localizer.Format("#autoLOC_238885");
	}

	public override string GetStagingDisableText()
	{
		if (!string.IsNullOrEmpty(stagingDisableText))
		{
			return stagingDisableText;
		}
		return Localizer.Format("#autoLOC_238886");
	}

	public virtual bool CanBeDetached()
	{
		return !resettingExperiment;
	}

	public virtual bool CanBeOffset()
	{
		return !resettingExperiment;
	}

	public virtual bool CanBeRotated()
	{
		return !resettingExperiment;
	}
}

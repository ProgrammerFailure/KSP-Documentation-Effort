using System.Collections;
using System.Collections.Generic;
using ns9;
using UnityEngine;

public class ModuleAsteroid : PartModule, IVesselAutoRename, IPartMassModifier
{
	public FlightCoMTracker CoMnode;

	[KSPField(isPersistant = true)]
	public int seed = -1;

	[KSPField(isPersistant = true)]
	public string AsteroidName = "";

	[KSPField(isPersistant = true)]
	public string prefabBaseURL = "";

	public const int nullState = 0;

	public const int primaryState = 1;

	public const int secondaryState = 2;

	[KSPField(isPersistant = true)]
	public int currentState;

	[KSPField]
	public float secondaryRate = 0.05f;

	[KSPField]
	public float minRadiusMultiplier = 0.75f;

	[KSPField]
	public float maxRadiusMultiplier = 1.25f;

	[KSPField]
	public float density = 0.03f;

	[KSPField]
	public string sampleExperimentId = "asteroidSample";

	[KSPField]
	public float sampleExperimentXmitScalar = 0.3f;

	[KSPField]
	public int experimentUsageMask;

	[KSPField]
	public bool forceProceduralDrag;

	public ProceduralAsteroid paPrefab;

	public PAsteroid paGenerated;

	public float radius;

	public float asteroidMass = 1f;

	public Transform modelTransform;

	public bool wasStarted;

	public ScienceExperiment experiment;

	public ScienceSubject subject;

	public ScienceData experimentData;

	public PopupDialog renameDialog;

	public string newName;

	public static string cacheAutoLOC_230121;

	public override void OnLoad(ConfigNode node)
	{
	}

	public override void OnSave(ConfigNode node)
	{
	}

	public override void OnAwake()
	{
		base.OnAwake();
		wasStarted = false;
	}

	public override void OnStart(StartState state)
	{
		if (wasStarted)
		{
			return;
		}
		wasStarted = true;
		bool flag = true;
		if (seed == -1)
		{
			flag = false;
			seed = Random.Range(-100000000, 100000000);
		}
		Random.InitState(seed);
		if (currentState == 0)
		{
			if (!flag && HighLogic.CurrentGame.Mode != Game.Modes.SCENARIO && HighLogic.CurrentGame.Mode != Game.Modes.SCENARIO_NON_RESUMABLE)
			{
				KSPRandom kSPRandom = new KSPRandom(seed + 1);
				currentState = ((!((float)kSPRandom.NextDouble() < secondaryRate)) ? 1 : 2);
			}
			else
			{
				currentState = 1;
			}
		}
		if (prefabBaseURL == string.Empty)
		{
			prefabBaseURL = "Procedural/PA_" + base.vessel.DiscoveryInfo.objectSize;
		}
		paPrefab = Resources.Load<ProceduralAsteroid>(prefabBaseURL);
		if ((bool)paPrefab)
		{
			radius = paPrefab.radius * Random.Range(minRadiusMultiplier, maxRadiusMultiplier);
			paGenerated = paPrefab.Generate(seed, radius, base.transform, Rangefinder, OnGenComplete, currentState == 2);
			base.part.mass = (base.part.prefabMass = (asteroidMass = paGenerated.volume * density));
			modelTransform = base.part.FindModelTransform("potatoroid");
			if ((bool)modelTransform)
			{
				paGenerated.transform.parent = modelTransform.parent;
				Object.Destroy(modelTransform.gameObject);
			}
			paGenerated.SetupPartParameters();
			base.Events["TakeSampleEVAEvent"].active = false;
			base.Events["TakeSampleEVAEvent"].unfocusedRange = paGenerated.highestPoint + 0.5f;
			experiment = ResearchAndDevelopment.GetExperiment(sampleExperimentId);
			if (experiment != null)
			{
				base.Events["TakeSampleEVAEvent"].active = true;
			}
			else
			{
				Debug.LogError("[ModuleAsteroid]: Experiment Definition not found for id " + sampleExperimentId, base.gameObject);
			}
			if (HighLogic.LoadedSceneIsFlight)
			{
				if (AsteroidName == string.Empty)
				{
					AsteroidName = base.vessel.vesselName;
				}
				renameAsteroid(AsteroidName);
				base.Events["RenameAsteroidEvent"].unfocusedRange = paGenerated.highestPoint + 0.5f;
				GameEvents.onVesselsUndocking.Add(VesselsUndockingAsteroid);
			}
			UpdateDragCube();
			base.vessel.UpdateVesselSize();
			if (HighLogic.LoadedSceneIsFlight)
			{
				CoMnode = FlightCoMTracker.Create(this, activeTargetable: true);
				StartCoroutine(PostInit());
			}
		}
		else
		{
			Debug.LogError("[ModuleAsteroid]: Cannot find prefab at URL '" + prefabBaseURL + "'", base.gameObject);
		}
	}

	public IEnumerator PostInit()
	{
		while (!base.part.started)
		{
			yield return null;
		}
		base.vessel.DiscoveryInfo.SetLevel(base.vessel.DiscoveryInfo.Level | (DiscoveryLevels.Presence | DiscoveryLevels.StateVectors | DiscoveryLevels.Appearance));
		UpdateDragCube();
	}

	public void OnDestroy()
	{
		GameEvents.onVesselsUndocking.Remove(VesselsUndockingAsteroid);
	}

	public float Rangefinder(Transform t)
	{
		if (FlightGlobals.ActiveVessel == base.vessel)
		{
			return 0f;
		}
		return (t.position - FlightGlobals.ActiveVessel.vesselTransform.position).magnitude;
	}

	public void OnGenComplete()
	{
	}

	public void UpdateDragCube()
	{
		if (forceProceduralDrag)
		{
			base.part.DragCubes.Procedural = true;
			base.part.DragCubes.ForceUpdate(weights: true, occlusion: true, resetProcTiming: true);
			base.part.DragCubes.SetDragWeights();
			return;
		}
		DragCubeList dragCubes = PartLoader.LoadedPartsList.Find((AvailablePart x) => x.name == "PotatoRoid").partPrefab.GetComponent<Part>().DragCubes;
		switch (DiscoveryInfo.GetObjectClass(base.vessel.DiscoveryInfo.size.Value))
		{
		default:
			base.part.DragCubes.LoadCube(dragCubes, "TypeC");
			break;
		case UntrackedObjectClass.const_0:
			base.part.DragCubes.LoadCube(dragCubes, "TypeA");
			break;
		case UntrackedObjectClass.const_1:
			base.part.DragCubes.LoadCube(dragCubes, "TypeB");
			break;
		case UntrackedObjectClass.const_2:
			base.part.DragCubes.LoadCube(dragCubes, "TypeC");
			break;
		case UntrackedObjectClass.const_3:
			base.part.DragCubes.LoadCube(dragCubes, "TypeD");
			break;
		case UntrackedObjectClass.const_4:
			base.part.DragCubes.LoadCube(dragCubes, "TypeE");
			break;
		}
	}

	public float GetModuleMass(float defaultMass, ModifierStagingSituation sit)
	{
		return asteroidMass - defaultMass;
	}

	public ModifierChangeWhen GetModuleMassChangeWhen()
	{
		return ModifierChangeWhen.CONSTANTLY;
	}

	public void SetAsteroidMass(float nMass)
	{
		asteroidMass = nMass;
	}

	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = false, guiActive = true, unfocusedRange = 500f, guiName = "#autoLOC_6001849")]
	public void MakeTarget()
	{
		if (!CoMnode)
		{
			Debug.LogError("[ModuleAsteroid]: Cannot target center of mass, no CoM node instance (for some reason)", base.gameObject);
		}
		else
		{
			CoMnode.MakeTarget();
		}
	}

	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, unfocusedRange = 1f, guiName = "#autoLOC_6001850")]
	public void TakeSampleEVAEvent()
	{
		ModuleScienceContainer collector = FlightGlobals.ActiveVessel.FindPartModuleImplementing<ModuleScienceContainer>();
		performSampleExperiment(collector);
	}

	public void performSampleExperiment(ModuleScienceContainer collector)
	{
		ExperimentSituations experimentSituation = ScienceUtil.GetExperimentSituation(base.vessel);
		string message = "";
		if (ScienceUtil.RequiredUsageExternalAvailable(base.vessel, FlightGlobals.ActiveVessel, (ExperimentUsageReqs)experimentUsageMask, experiment, ref message))
		{
			if (experiment.IsAvailableWhile(experimentSituation, base.vessel.mainBody))
			{
				string text = string.Empty;
				string text2 = string.Empty;
				if (experiment.BiomeIsRelevantWhile(experimentSituation))
				{
					if (base.vessel.landedAt != string.Empty)
					{
						text = Vessel.GetLandedAtString(base.vessel.landedAt);
						text2 = Localizer.Format(base.vessel.displaylandedAt);
					}
					else
					{
						text = ScienceUtil.GetExperimentBiome(base.vessel.mainBody, base.vessel.latitude, base.vessel.longitude);
						text2 = ScienceUtil.GetBiomedisplayName(base.vessel.mainBody, text);
					}
					if (text2 == string.Empty)
					{
						text2 = text;
					}
				}
				subject = ResearchAndDevelopment.GetExperimentSubject(experiment, experimentSituation, base.part.partInfo.name + base.part.flightID, base.part.partInfo.title, base.vessel.mainBody, text, text2);
				experimentData = new ScienceData(experiment.baseValue * experiment.dataScale, sampleExperimentXmitScalar, 0f, subject.id, subject.title, triggered: false, base.part.flightID);
				if (collector.HasData(experimentData))
				{
					ScreenMessages.PostScreenMessage("<color=orange>[" + collector.part.partInfo.title + "]: <i>" + experimentData.title + cacheAutoLOC_230121, 5f, ScreenMessageStyle.UPPER_LEFT);
				}
				else
				{
					GameEvents.OnExperimentDeployed.Fire(experimentData);
					if (collector.AddData(experimentData))
					{
						collector.ReviewData();
						AnalyticsUtil.LogAsteroidEvent(this, AnalyticsUtil.SpaceObjectEventTypes.sampled, HighLogic.CurrentGame, base.vessel);
					}
				}
			}
			else
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_230133", experiment.experimentTitle), 5f, ScreenMessageStyle.UPPER_CENTER);
			}
		}
		else
		{
			ScreenMessages.PostScreenMessage("<b><color=orange>" + message + "</color></b>", 6f, ScreenMessageStyle.UPPER_LEFT);
		}
	}

	[KSPEvent(guiActiveUnfocused = true, guiActive = true, unfocusedRange = 50f, guiName = "#autoLOC_6001851")]
	public void RenameAsteroidEvent()
	{
		newName = AsteroidName;
		InputLockManager.SetControlLock("RenameDialog");
		renameDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("AsteroidRename", "#autoLOC_230159", "#autoLOC_6001851", HighLogic.UISkin, 180f, drawRenameWindow()), persistAcrossScenes: false, HighLogic.UISkin);
		renameDialog.OnDismiss = onRenameDismiss;
	}

	public DialogGUIBase[] drawRenameWindow()
	{
		List<DialogGUIBase> list = new List<DialogGUIBase>();
		DialogGUITextInput item = new DialogGUITextInput(newName, multiline: false, 64, delegate(string n)
		{
			newName = n;
			return newName;
		}, 28f);
		list.Add(item);
		DialogGUIButton item2 = new DialogGUIButton(Localizer.Format("#autoLOC_230186"), delegate
		{
			onRenameConfirm();
		}, () => Vessel.IsValidVesselName(newName), dismissOnSelect: true);
		list.Add(item2);
		item2 = new DialogGUIButton(Localizer.Format("#autoLOC_230196"), delegate
		{
			onRenameDismiss();
		}, dismissOnSelect: true);
		list.Add(item2);
		return list.ToArray();
	}

	public void onRenameConfirm()
	{
		if (Vessel.IsValidVesselName(newName))
		{
			renameAsteroid(newName);
		}
		onRenameDismiss();
	}

	public void onRenameDismiss()
	{
		renameDialog.Dismiss();
		InputLockManager.RemoveControlLock("RenameDialog");
	}

	public void renameAsteroid(string newName)
	{
		string text = base.part.partInfo.name;
		base.part.partInfo = new AvailablePart();
		base.part.partInfo.name = text;
		base.part.partInfo.title = newName;
		if (base.part._partActionWindow != null)
		{
			base.part._partActionWindow.titleText.text = base.part.partInfo.title;
		}
		AsteroidName = newName;
		if (base.vessel.vesselType <= VesselType.SpaceObject && !string.Equals(base.vessel.vesselName, AsteroidName))
		{
			base.vessel.vesselName = AsteroidName;
			GameEvents.onVesselRename.Fire(new GameEvents.HostedFromToAction<Vessel, string>(base.vessel, AsteroidName, AsteroidName));
		}
	}

	public void VesselsUndockingAsteroid(Vessel oldVessel, Vessel newVessel)
	{
		if (newVessel.persistentId == base.vessel.persistentId)
		{
			renameAsteroid(AsteroidName);
		}
	}

	public VesselType GetVesselType()
	{
		return base.part.vesselType;
	}

	public string GetVesselName()
	{
		return AsteroidName;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_230121 = Localizer.Format("#autoLOC_230121");
	}
}

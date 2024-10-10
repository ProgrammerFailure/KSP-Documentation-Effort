using System.Collections;
using System.Collections.Generic;
using ns9;
using UnityEngine;

public class ModuleComet : PartModule, IVesselAutoRename, IPartMassModifier
{
	public CometVessel cometVessel;

	public FlightCoMTracker CoMnode;

	[KSPField(isPersistant = true)]
	public int seed = -1;

	[KSPField(isPersistant = true)]
	public string CometName = "";

	[KSPField(isPersistant = true)]
	public string CometType = "";

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
	public string sampleExperimentId = "cometSample";

	[KSPField]
	public float sampleExperimentXmitScalar = 0.6f;

	[KSPField]
	public int experimentUsageMask;

	[KSPField]
	public bool forceProceduralDrag;

	public ProceduralComet pcPrefab;

	public PComet pcGenerated;

	public float radius;

	public float cometMass = 1f;

	public Transform modelTransform;

	[KSPField]
	public float fragmentDynamicPressureModifier;

	public bool optimizedCollider;

	public int numGeysers;

	public int numNearDustEmitters = 5;

	public List<GameObject> geysers = new List<GameObject>();

	public List<ParticleSystem> geyserParticles = new List<ParticleSystem>();

	public List<float> geyserParticleOriginalRateOverTime = new List<float>();

	public bool geyserParticlesActive;

	public List<GameObject> nearDustParticleObjects = new List<GameObject>();

	public List<ParticleSystem> nearDustParticles = new List<ParticleSystem>();

	public List<float> nearDustParticleOriginalRateOverTime = new List<float>();

	public bool nearDustParticlesActive;

	public Vector3d cometOffset = Vector3d.zero;

	public Vector3d lastPosition = Vector3d.zero;

	public Renderer cometRenderer;

	public bool offsetMaterialUpdates;

	[SerializeField]
	public float offsetMaterialCamDistance = 10000f;

	public bool changedNearFXSimRate;

	public bool canSpawnFragments;

	public float debugGeyserEmitterMultiplier = 1f;

	public float debugDustEmitterMultiplier = 1f;

	public bool wasStarted;

	[KSPField(isPersistant = true)]
	public double utFragmentSpawned;

	public double fragmentSafeTime = 5.0;

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
		if (base.vessel != null)
		{
			for (int i = 0; i < base.vessel.vesselModules.Count; i++)
			{
				cometVessel = base.vessel.vesselModules[i] as CometVessel;
				if (cometVessel != null)
				{
					optimizedCollider = cometVessel.optimizedCollider;
					fragmentDynamicPressureModifier = cometVessel.fragmentDynamicPressureModifier;
					break;
				}
			}
		}
		if (cometVessel != null)
		{
			seed = cometVessel.seed;
		}
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
			prefabBaseURL = "Procedural/PC_" + base.vessel.DiscoveryInfo.objectSize;
		}
		pcPrefab = Resources.Load<ProceduralComet>(prefabBaseURL);
		if ((bool)pcPrefab)
		{
			if (cometVessel != null)
			{
				radius = cometVessel.radius;
			}
			radius = pcPrefab.radius * Random.Range(minRadiusMultiplier, maxRadiusMultiplier);
			if (prefabBaseURL.Contains("Procedural/PC_"))
			{
				pcGenerated = pcPrefab.Generate(seed, radius, base.transform, Rangefinder, OnGenComplete, optimizedCollider, currentState == 2);
			}
			else
			{
				ProceduralComet proceduralComet = Object.Instantiate(pcPrefab);
				pcGenerated = proceduralComet.GetComponent<PComet>();
				proceduralComet.transform.parent = base.transform;
				proceduralComet.transform.localPosition = Vector3.zero;
				proceduralComet.transform.localRotation = Quaternion.identity;
			}
			base.part.mass = (base.part.prefabMass = (cometMass = pcGenerated.volume * density));
			modelTransform = base.part.FindModelTransform("potatoroid");
			if ((bool)modelTransform)
			{
				pcGenerated.transform.parent = modelTransform.parent;
				Object.Destroy(modelTransform.gameObject);
			}
			pcGenerated.SetupPartParameters();
			if (pcGenerated.VisualRenderer != null && pcGenerated.ConvexColliderMesh != null)
			{
				pcGenerated.ConvexColliderMesh.RecalculateBounds();
				base.part.CoMOffset = pcGenerated.ConvexColliderMesh.bounds.center;
			}
			base.Events["TakeSampleEVAEvent"].active = false;
			base.Events["TakeSampleEVAEvent"].unfocusedRange = pcGenerated.highestPoint + 0.5f;
			if (string.IsNullOrEmpty(CometType))
			{
				CometType = "intermediate";
				if (cometVessel != null)
				{
					CometType = cometVessel.typeName;
				}
			}
			sampleExperimentId = sampleExperimentId + "_" + CometType;
			experiment = ResearchAndDevelopment.GetExperiment(sampleExperimentId);
			if (experiment != null)
			{
				base.Events["TakeSampleEVAEvent"].active = true;
			}
			else
			{
				Debug.LogError("[ModuleComet]: Experiment Definition not found for id " + sampleExperimentId, base.gameObject);
			}
			if (HighLogic.LoadedSceneIsFlight)
			{
				if (CometName == string.Empty)
				{
					CometName = base.vessel.vesselName;
				}
				renameComet(CometName);
				base.Events["RenameCometEvent"].unfocusedRange = pcGenerated.highestPoint + 0.5f;
			}
			UpdateDragCube();
			base.vessel.UpdateVesselSize();
			if (HighLogic.LoadedSceneIsFlight)
			{
				CoMnode = FlightCoMTracker.Create(this, activeTargetable: true);
				StartCoroutine(PostInit());
			}
			if (cometVessel != null)
			{
				numGeysers = cometVessel.numGeysers;
				numNearDustEmitters = cometVessel.numNearDustEmitters;
			}
			if (fragmentDynamicPressureModifier > 0f)
			{
				utFragmentSpawned = Planetarium.GetUniversalTime();
				fragmentSafeTime = Random.Range(CometManager.Instance.fragmentMinSafeTime, CometManager.Instance.fragmentMaxSafeTime);
			}
			lastPosition = base.vessel.GetWorldPos3D();
			cometRenderer = pcGenerated.GetVisualObjectRenderer();
			if (cometRenderer != null && cometRenderer.material.shader.name == "Comet Triplanar")
			{
				offsetMaterialUpdates = true;
			}
			canSpawnFragments = DiscoveryInfo.GetObjectClass(base.vessel.DiscoveryInfo.size.Value) > UntrackedObjectClass.const_1;
			GameEvents.onTimeWarpRateChanged.Add(OnTimeWarpRateChanged);
		}
		else
		{
			Debug.LogError("[ModuleComet]: Cannot find prefab at URL '" + prefabBaseURL + "'", base.gameObject);
		}
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		if (CometManager.Instance == null)
		{
			return;
		}
		float num = Vector3.Distance(base.transform.position, FlightCamera.fetch.GetCameraTransform().position);
		if (GameSettings.COMET_SHOW_GEYSERS || GameSettings.COMET_SHOW_NEAR_DUST)
		{
			bool flag = MapView.MapIsEnabled || !(cometVessel != null) || cometVessel.geyserVFXRatio == 0f;
			bool flag2 = MapView.MapIsEnabled || !(cometVessel != null) || cometVessel.nearDustVFXRatio == 0f;
			if (GameSettings.COMET_SHOW_GEYSERS)
			{
				float geyserVisibleDistance = CometManager.Instance.geyserVisibleDistance;
				if (!flag && !geyserParticlesActive && num < geyserVisibleDistance)
				{
					ActivateGeysers(activate: true);
				}
				else if (geyserParticlesActive && (flag || num > geyserVisibleDistance))
				{
					ActivateGeysers(activate: false);
				}
				if (geyserParticlesActive && cometVessel != null)
				{
					float num2 = cometVessel.geyserVFXRatio * cometVessel.atmosphereVFXRatio;
					for (int i = 0; i < geyserParticles.Count; i++)
					{
						ParticleSystem.EmissionModule emission = geyserParticles[i].emission;
						ParticleSystem.MinMaxCurve rateOverTime = emission.rateOverTime;
						rateOverTime.constant = geyserParticleOriginalRateOverTime[i] * num2 * debugGeyserEmitterMultiplier;
						emission.rateOverTime = rateOverTime;
					}
				}
			}
			if (GameSettings.COMET_SHOW_NEAR_DUST)
			{
				float num3 = CometManager.Instance.nearDustVisibleRadiusMultiplier * radius;
				if (!flag2 && !nearDustParticlesActive && num < num3)
				{
					ActivateNearDust(activate: true);
				}
				else if (nearDustParticlesActive && (flag2 || num > num3))
				{
					ActivateNearDust(activate: false);
				}
				if (nearDustParticlesActive && cometVessel != null)
				{
					float num4 = cometVessel.nearDustVFXRatio * cometVessel.atmosphereVFXRatio;
					for (int j = 0; j < nearDustParticles.Count; j++)
					{
						ParticleSystem.EmissionModule emission2 = nearDustParticles[j].emission;
						ParticleSystem.MinMaxCurve rateOverTime2 = emission2.rateOverTime;
						rateOverTime2.constant = nearDustParticleOriginalRateOverTime[j] * num4 * debugDustEmitterMultiplier;
						emission2.rateOverTime = rateOverTime2;
					}
				}
			}
			if (changedNearFXSimRate)
			{
				for (int k = 0; k < nearDustParticles.Count; k++)
				{
					ParticleSystem.MainModule main = nearDustParticles[k].main;
					main.simulationSpeed = TimeWarp.CurrentRate;
				}
				for (int l = 0; l < geyserParticles.Count; l++)
				{
					ParticleSystem.MainModule main2 = geyserParticles[l].main;
					main2.simulationSpeed = TimeWarp.CurrentRate;
				}
				if (TimeWarp.CurrentRateIsTargetRate)
				{
					changedNearFXSimRate = false;
				}
			}
		}
		if (offsetMaterialUpdates && cometRenderer != null)
		{
			if (!MapView.MapIsEnabled && cometRenderer.isVisible && num <= offsetMaterialCamDistance)
			{
				cometOffset += base.vessel.GetWorldPos3D() - lastPosition;
				cometRenderer.material.SetVector("_cometOriginOffset", (Vector3)cometOffset);
			}
			else
			{
				cometOffset = Vector3d.zero;
			}
			lastPosition = base.vessel.GetWorldPos3D();
		}
	}

	public override void OnFixedUpdate()
	{
		base.OnFixedUpdate();
		CheckExplosion();
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
		for (int i = 0; i < nearDustParticleObjects.Count; i++)
		{
			if (nearDustParticleObjects[i] != null)
			{
				Object.Destroy(nearDustParticleObjects[i]);
			}
		}
		for (int j = 0; j < geysers.Count; j++)
		{
			if (geysers[j] != null)
			{
				Object.Destroy(geysers[j]);
			}
		}
		GameEvents.onTimeWarpRateChanged.Remove(OnTimeWarpRateChanged);
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
		DragCubeList dragCubes = PartLoader.LoadedPartsList.Find((AvailablePart x) => x.name == "PotatoComet").partPrefab.GetComponent<Part>().DragCubes;
		switch (DiscoveryInfo.GetObjectClass(base.vessel.DiscoveryInfo.size.Value))
		{
		default:
			base.part.DragCubes.LoadCube(dragCubes, "TypeF");
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
		case UntrackedObjectClass.const_5:
			base.part.DragCubes.LoadCube(dragCubes, "TypeF");
			break;
		case UntrackedObjectClass.const_6:
			base.part.DragCubes.LoadCube(dragCubes, "TypeG");
			break;
		case UntrackedObjectClass.const_7:
			base.part.DragCubes.LoadCube(dragCubes, "TypeH");
			break;
		case UntrackedObjectClass.const_8:
			base.part.DragCubes.LoadCube(dragCubes, "TypeI");
			break;
		}
	}

	public float GetModuleMass(float defaultMass, ModifierStagingSituation sit)
	{
		return Mathf.Clamp(cometMass - defaultMass, Mathf.Epsilon, Mathf.Abs(cometMass - defaultMass));
	}

	public ModifierChangeWhen GetModuleMassChangeWhen()
	{
		return ModifierChangeWhen.CONSTANTLY;
	}

	public void SetCometMass(float nMass)
	{
		cometMass = nMass;
	}

	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = false, guiActive = true, unfocusedRange = 500f, guiName = "#autoLOC_6001849")]
	public void MakeTarget()
	{
		if (!CoMnode)
		{
			Debug.LogError("[ModuleComet]: Cannot target center of mass, no CoM node instance (for some reason)", base.gameObject);
		}
		else
		{
			CoMnode.MakeTarget();
		}
	}

	public void CheckExplosion()
	{
		if (GameSettings.COMET_REENTRY_FRAGMENT && !CheatOptions.NoCrashDamage && CometManager.Instance != null && cometVessel != null && (fragmentDynamicPressureModifier == 0f || Planetarium.GetUniversalTime() > utFragmentSpawned + fragmentSafeTime) && base.part.dynamicPressurekPa >= (double)CometManager.Instance.fragmentdynamicPressurekPa)
		{
			base.part.explode();
			if (canSpawnFragments)
			{
				CometManager.Instance.SpawnCometFragments(cometVessel);
			}
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
				subject = ResearchAndDevelopment.GetExperimentSubject(experiment, experimentSituation, base.part.partInfo.title, base.part.partInfo.title, base.vessel.mainBody, text, text2);
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
						AnalyticsUtil.LogCometEvent(this, AnalyticsUtil.SpaceObjectEventTypes.sampled, HighLogic.CurrentGame, base.vessel);
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

	public List<GameObject> CreateEmittersOnSurface(GameObject emitterPrefab, int numberOfEmitters)
	{
		List<GameObject> list = new List<GameObject>();
		if (emitterPrefab == null)
		{
			Debug.LogWarning("[ModuleComet.CreateEmittersOnSurface]: Emitter prefab is null!");
			return list;
		}
		Mesh visualMesh = pcGenerated.visualMesh;
		for (int i = 0; i < numberOfEmitters; i++)
		{
			uint num = (uint)(Random.Range(0, visualMesh.triangles.Length / 3) * 3);
			int num2 = visualMesh.triangles[num];
			Vector3 vector = visualMesh.vertices[num2];
			int num3 = visualMesh.triangles[num + 1];
			Vector3 vector2 = visualMesh.vertices[num3];
			int num4 = visualMesh.triangles[num + 2];
			Vector3 vector3 = visualMesh.vertices[num4];
			if (num2 != num3 && num3 != num4 && num2 != num4)
			{
				Vector3 vector4 = (vector + vector2 + vector3) / 3f;
				Vector3 vector5 = vector2 - vector;
				Vector3 rhs = vector3 - vector;
				Vector3 vector6 = Vector3.Cross(vector5, rhs);
				if (Vector3.Dot(vector4, vector6) < 0f)
				{
					vector6 *= -1f;
				}
				Vector3 position = base.transform.TransformPoint(vector4);
				Quaternion rotation = Quaternion.LookRotation(base.transform.TransformDirection(vector5), base.transform.TransformDirection(vector6));
				GameObject gameObject = Object.Instantiate(emitterPrefab, position, rotation, base.transform);
				if (gameObject != null)
				{
					list.Add(gameObject);
				}
				else
				{
					Debug.LogWarning("[ModuleComet.CreateEmittersOnSurface]: Unable to instantiate emitter!");
				}
			}
			else
			{
				i--;
			}
		}
		return list;
	}

	public void ActivateGeysers(bool activate)
	{
		if (CometManager.Instance == null || !GameSettings.COMET_SHOW_GEYSERS)
		{
			return;
		}
		if (activate && geyserParticles.Count == 0)
		{
			geysers = CreateEmittersOnSurface(CometManager.Instance.geyserEmitter, numGeysers);
			for (int i = 0; i < geysers.Count; i++)
			{
				geyserParticles.AddRange(geysers[i].GetComponentsInChildren<ParticleSystem>());
			}
			float num = CometManager.Instance.geyserSizeRadiusMultiplier * radius;
			Vector3 localScale = new Vector3(num, num, num);
			float num2 = cometVessel.geyserVFXRatio * cometVessel.atmosphereVFXRatio;
			for (int j = 0; j < geyserParticles.Count; j++)
			{
				geyserParticles[j].transform.localScale = localScale;
				geyserParticleOriginalRateOverTime.Add(geyserParticles[j].emission.rateOverTime.constant);
				ParticleSystem.EmissionModule emission = geyserParticles[j].emission;
				ParticleSystem.MinMaxCurve rateOverTime = emission.rateOverTime;
				rateOverTime.constant = geyserParticles[j].emission.rateOverTime.constant * num2;
				emission.rateOverTime = rateOverTime;
			}
		}
		int count = geyserParticles.Count;
		for (int k = 0; k < count; k++)
		{
			if (activate)
			{
				geyserParticles[k].Play();
			}
			else
			{
				geyserParticles[k].Stop();
			}
		}
		geyserParticlesActive = activate;
	}

	public void ActivateNearDust(bool activate)
	{
		if (CometManager.Instance == null || !GameSettings.COMET_SHOW_NEAR_DUST)
		{
			return;
		}
		if (activate && nearDustParticles.Count == 0)
		{
			nearDustParticleObjects = CreateEmittersOnSurface(CometManager.Instance.nearDustEmitter, numNearDustEmitters);
			for (int i = 0; i < nearDustParticleObjects.Count; i++)
			{
				nearDustParticles.AddRange(nearDustParticleObjects[i].GetComponentsInChildren<ParticleSystem>());
			}
			float num = cometVessel.nearDustVFXRatio * cometVessel.atmosphereVFXRatio;
			for (int j = 0; j < nearDustParticles.Count; j++)
			{
				nearDustParticleOriginalRateOverTime.Add(nearDustParticles[j].emission.rateOverTime.constant);
				ParticleSystem.EmissionModule emission = nearDustParticles[j].emission;
				ParticleSystem.MinMaxCurve rateOverTime = emission.rateOverTime;
				rateOverTime.constant = nearDustParticles[j].emission.rateOverTime.constant * num;
				emission.rateOverTime = rateOverTime;
			}
		}
		int count = nearDustParticles.Count;
		for (int k = 0; k < count; k++)
		{
			if (activate)
			{
				nearDustParticles[k].Play();
			}
			else
			{
				nearDustParticles[k].Stop();
			}
		}
		nearDustParticlesActive = activate;
	}

	public void OnTimeWarpRateChanged()
	{
		changedNearFXSimRate = true;
	}

	public void SetGeyser(bool enabled, float emissionRate)
	{
		debugGeyserEmitterMultiplier = emissionRate;
		for (int i = 0; i < geyserParticles.Count; i++)
		{
			if (geyserParticles[i].gameObject.activeSelf != enabled)
			{
				geyserParticles[i].gameObject.SetActive(enabled);
				if (enabled)
				{
					geyserParticles[i].Stop();
					geyserParticles[i].Play();
				}
			}
		}
	}

	public void SetDust(bool enabled, float emissionRate)
	{
		debugDustEmitterMultiplier = emissionRate;
		for (int i = 0; i < nearDustParticles.Count; i++)
		{
			if (nearDustParticles[i].gameObject.activeSelf != enabled)
			{
				nearDustParticles[i].gameObject.SetActive(enabled);
				if (enabled)
				{
					nearDustParticles[i].Stop();
					nearDustParticles[i].Play();
				}
			}
		}
	}

	[KSPEvent(guiActiveUnfocused = true, guiActive = true, unfocusedRange = 50f, guiName = "#autoLOC_6006052")]
	public void RenameCometEvent()
	{
		newName = CometName;
		InputLockManager.SetControlLock("RenameDialog");
		renameDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("CometRename", "#autoLOC_6006051", "#autoLOC_6006052", HighLogic.UISkin, 180f, drawRenameWindow()), persistAcrossScenes: false, HighLogic.UISkin);
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
			renameComet(newName);
		}
		onRenameDismiss();
	}

	public void onRenameDismiss()
	{
		renameDialog.Dismiss();
		InputLockManager.RemoveControlLock("RenameDialog");
	}

	public void renameComet(string newName)
	{
		string text = base.part.partInfo.name;
		base.part.partInfo = new AvailablePart();
		base.part.partInfo.name = text;
		base.part.partInfo.title = newName;
		if (base.part._partActionWindow != null)
		{
			base.part._partActionWindow.titleText.text = base.part.partInfo.title;
		}
		string cometName = CometName;
		CometName = newName;
		if (base.vessel.vesselType <= VesselType.SpaceObject)
		{
			base.vessel.vesselName = CometName;
		}
		GameEvents.onVesselRename.Fire(new GameEvents.HostedFromToAction<Vessel, string>(base.vessel, cometName, newName));
	}

	public VesselType GetVesselType()
	{
		return base.part.vesselType;
	}

	public string GetVesselName()
	{
		return CometName;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_230121 = Localizer.Format("#autoLOC_230121");
	}
}

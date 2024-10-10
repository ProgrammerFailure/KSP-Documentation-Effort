using System;
using System.Collections;
using System.Collections.Generic;
using CommNet;
using FinePrint.Utilities;
using ns9;
using UnityEngine;

namespace Expansions.Serenity.DeployedScience.Runtime;

public class DeployedScienceExperiment : MonoBehaviour, IConfigNode
{
	[Serializable]
	public class SeismicPartData
	{
		public float velocity;

		public uint partId;

		public float mass;

		public float resourceMass;

		public double impactDistance;

		public float impactDistanceMultiplier;
	}

	public delegate float SeismicDataCalc(DeployedScienceExperiment scienceExp, float impactJoules, float impactJoulesMaxValue, List<SeismicPartData> seismicPartData);

	[SerializeField]
	public float totalScienceGenerated;

	[SerializeField]
	public float scienceLimit;

	[SerializeField]
	public float scienceValue;

	[SerializeField]
	public float storedScienceData;

	[SerializeField]
	public float transmittedScienceData;

	[SerializeField]
	public uint partId;

	[SerializeField]
	public string experimentId;

	[SerializeField]
	public string experimentName;

	[SerializeField]
	public float scienceModifierRate;

	[SerializeField]
	public float scienceDiminishingModifierRate;

	public ScienceExperiment Experiment;

	public Vessel ExperimentVessel;

	public DeployedScienceCluster Cluster;

	public DeployedSciencePart sciencePart;

	public double LastScienceGeneratedUT;

	[SerializeField]
	public bool experimentSituationValid;

	[SerializeField]
	public ModuleGroundExperiment moduleGroundExperiment;

	public float xmitDataScalar = 1f;

	[SerializeField]
	public ScienceSubject subject;

	[SerializeField]
	public Guid vesselID = Guid.Empty;

	[SerializeField]
	public Vessel ControllerVessel;

	[SerializeField]
	public float solarMultiplier;

	[SerializeField]
	public bool seismicEventDataSendPending;

	[SerializeField]
	public bool noCommNetMsgPosted;

	[SerializeField]
	public bool noPowerMsgPosted;

	public float seismicEventVelocity;

	public List<SeismicPartData> seismicPartList;

	public FloatCurve distanceCurve;

	public double distanceMultiplier = 1.0;

	public double totalJoules;

	public float impactRate;

	public SeismicDataCalc seismicDataCalcOverride;

	public float TotalScienceGenerated => totalScienceGenerated;

	public float ScienceLimit => scienceLimit;

	public float ScienceValue => scienceValue;

	public float StoredScienceData => storedScienceData;

	public float TransmittedScienceData => transmittedScienceData;

	public uint PartId => partId;

	public string ExperimentId => experimentId;

	public string ExperimentName => experimentName;

	public float ScienceModifierRate => scienceModifierRate;

	public float ScienceDiminishingModifierRate
	{
		get
		{
			return scienceDiminishingModifierRate;
		}
		set
		{
			scienceDiminishingModifierRate = value;
		}
	}

	public bool ExperimentSituationValid => experimentSituationValid;

	public ModuleGroundExperiment GroundExperimentModule
	{
		get
		{
			if (moduleGroundExperiment != null)
			{
				return moduleGroundExperiment;
			}
			Part partout = null;
			moduleGroundExperiment = null;
			FlightGlobals.FindLoadedPart(partId, out partout);
			if (partout != null)
			{
				moduleGroundExperiment = partout.FindModuleImplementing<ModuleGroundExperiment>();
			}
			return moduleGroundExperiment;
		}
	}

	public bool SeismicEventDataSendPending => seismicEventDataSendPending;

	public static DeployedScienceExperiment Spawn()
	{
		GameObject obj = new GameObject("DeployedScienceExperiment");
		obj.transform.SetParent(DeployedScience.DeployedScienceGameObject.transform);
		DeployedScienceExperiment deployedScienceExperiment = obj.gameObject.AddComponent<DeployedScienceExperiment>();
		deployedScienceExperiment.experimentName = "";
		deployedScienceExperiment.experimentId = "";
		deployedScienceExperiment.scienceDiminishingModifierRate = 1f;
		deployedScienceExperiment.LastScienceGeneratedUT = Planetarium.GetUniversalTime();
		deployedScienceExperiment.distanceCurve = new FloatCurve();
		return deployedScienceExperiment;
	}

	public static DeployedScienceExperiment Spawn(ModuleGroundExperiment moduleGroundExperiment, DeployedScienceCluster cluster)
	{
		DeployedScienceExperiment deployedScienceExperiment = Spawn();
		if (cluster != null)
		{
			deployedScienceExperiment.gameObject.transform.SetParent(cluster.transform);
		}
		if (moduleGroundExperiment != null)
		{
			deployedScienceExperiment.Cluster = cluster;
			deployedScienceExperiment.partId = moduleGroundExperiment.part.persistentId;
			deployedScienceExperiment.moduleGroundExperiment = moduleGroundExperiment;
			deployedScienceExperiment.experimentName = moduleGroundExperiment.ClassName;
			deployedScienceExperiment.experimentId = moduleGroundExperiment.experimentId;
			deployedScienceExperiment.ExperimentVessel = moduleGroundExperiment.part.vessel;
			deployedScienceExperiment.gameObject.name = "DeployedScienceExperiment " + deployedScienceExperiment.ExperimentName;
			deployedScienceExperiment.UpdateScienceExperiment(moduleGroundExperiment);
			deployedScienceExperiment.distanceCurve = moduleGroundExperiment.distanceCurve;
		}
		return deployedScienceExperiment;
	}

	public static DeployedScienceExperiment SpawnandLoad(ConfigNode node)
	{
		DeployedScienceExperiment deployedScienceExperiment = Spawn();
		deployedScienceExperiment.Load(node);
		deployedScienceExperiment.gameObject.name = "DeployedScienceExperiment " + deployedScienceExperiment.ExperimentName;
		return deployedScienceExperiment;
	}

	public void Awake()
	{
		if (!ExpansionsLoader.IsExpansionInstalled("Serenity"))
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	public void Start()
	{
		GameEvents.onGroundScienceExperimentScienceValueChanged.Add(UpdateScienceValueExperiment);
		GameEvents.onGroundScienceExperimentScienceLimitChanged.Add(UpdateScienceLimitExperiment);
		GameEvents.onGroundSciencePartEnabledStateChanged.Add(GroundSciencePartEnabledStateChanged);
		GameEvents.onGroundScienceClusterPowerStateChanged.Add(GroundScienceClusterPowerStateChanged);
		Experiment = ResearchAndDevelopment.GetExperiment(ExperimentId);
		if (ExperimentVessel == null)
		{
			ExperimentVessel = FlightGlobals.FindVessel(vesselID);
			if (ExperimentVessel == null)
			{
				FlightGlobals.FindLoadedPart(partId, out var partout);
				if (partout != null)
				{
					ExperimentVessel = partout.vessel;
				}
				else
				{
					FlightGlobals.FindUnloadedPart(partId, out var partout2);
					if (partout2 != null && partout2.pVesselRef != null)
					{
						ExperimentVessel = partout2.pVesselRef.vesselRef;
					}
				}
			}
		}
		if (ExperimentVessel != null)
		{
			ExperimentSituations experimentSituation = ScienceUtil.GetExperimentSituation(ExperimentVessel);
			if (Experiment.IsAvailableWhile(experimentSituation, ExperimentVessel.mainBody))
			{
				string biome = "";
				string empty = string.Empty;
				subject = ResearchAndDevelopment.GetExperimentSubject(Experiment, experimentSituation, ExperimentVessel.mainBody, biome, empty);
				subject.applyScienceScale = Experiment.applyScienceScale;
				scienceValue = Experiment.baseValue * Experiment.dataScale * subject.subjectValue * (ScienceModifierRate / 100f);
				scienceLimit = subject.scienceCap;
				if (GroundExperimentModule != null)
				{
					moduleGroundExperiment.ScienceValue = scienceValue;
					moduleGroundExperiment.ScienceLimit = scienceLimit;
				}
				experimentSituationValid = true;
			}
			else
			{
				experimentSituationValid = false;
			}
		}
		else
		{
			Debug.LogWarningFormat("[DeployedScienceExperiment]: Failed to find Vessel for Experiment {0} {1}", experimentId, partId);
		}
	}

	public void FixedUpdate()
	{
		if (FlightGlobals.ready && !ExperimentSituationValid && sciencePart != null && sciencePart.Enabled)
		{
			sciencePart.Enabled = false;
		}
	}

	public void OnDestroy()
	{
		GameEvents.onGroundScienceExperimentScienceValueChanged.Remove(UpdateScienceValueExperiment);
		GameEvents.onGroundScienceExperimentScienceLimitChanged.Remove(UpdateScienceLimitExperiment);
		GameEvents.onGroundSciencePartEnabledStateChanged.Remove(GroundSciencePartEnabledStateChanged);
		GameEvents.onGroundScienceClusterPowerStateChanged.Remove(GroundScienceClusterPowerStateChanged);
		StopAllCoroutines();
	}

	public void SeismicDataCalcOverrideAdd(SeismicDataCalc method)
	{
		if (seismicDataCalcOverride == null)
		{
			seismicDataCalcOverride = method;
		}
		else
		{
			seismicDataCalcOverride = (SeismicDataCalc)Delegate.Combine(seismicDataCalcOverride, method);
		}
	}

	public void SeismicDataCalcOverrideRemove(SeismicDataCalc method)
	{
		if (seismicDataCalcOverride == method)
		{
			seismicDataCalcOverride = null;
		}
		else
		{
			seismicDataCalcOverride = (SeismicDataCalc)Delegate.Remove(seismicDataCalcOverride, method);
		}
	}

	public void GroundScienceClusterPowerStateChanged(DeployedScienceCluster cluster)
	{
		if (!(Cluster != null) || cluster.ControlModulePartId != Cluster.ControlModulePartId)
		{
			return;
		}
		noPowerMsgPosted = false;
		if (cluster.IsPowered)
		{
			if (SeismicEventDataSendPending)
			{
				ProcessSeismicEvent();
			}
			LastScienceGeneratedUT = Planetarium.GetUniversalTime();
		}
		else if (sciencePart != null && sciencePart.Enabled && experimentId != DeployedScience.SeismicExperimentId)
		{
			StartCoroutine(CalcScience());
		}
	}

	public void GroundSciencePartEnabledStateChanged(ModuleGroundSciencePart sciencePart)
	{
		if (!(Cluster != null) || sciencePart.part.persistentId != PartId)
		{
			return;
		}
		if (sciencePart.Enabled && sciencePart.DeployedOnGround)
		{
			if (SeismicEventDataSendPending)
			{
				ProcessSeismicEvent();
			}
			LastScienceGeneratedUT = Planetarium.GetUniversalTime();
		}
		else if (experimentId != DeployedScience.SeismicExperimentId)
		{
			StartCoroutine(CalcScience());
		}
	}

	public IEnumerator CalcScience()
	{
		while (Cluster.UpdatingScience)
		{
			yield return null;
		}
		CalculateScience(Cluster.PartialPowerMultiplier);
		if (ExperimentSituationValid && TimeToSendStoredData())
		{
			SendDataToComms();
		}
		yield return null;
	}

	public void UpdateScienceLimitExperiment(ModuleGroundExperiment moduleGroundExperiment)
	{
		if (moduleGroundExperiment != null && PartId == moduleGroundExperiment.part.persistentId)
		{
			this.moduleGroundExperiment = moduleGroundExperiment;
			scienceLimit = moduleGroundExperiment.ScienceLimit;
		}
	}

	public void UpdateScienceValueExperiment(ModuleGroundExperiment moduleGroundExperiment)
	{
		if (moduleGroundExperiment != null && PartId == moduleGroundExperiment.part.persistentId)
		{
			this.moduleGroundExperiment = moduleGroundExperiment;
			scienceValue = moduleGroundExperiment.ScienceValue;
		}
	}

	public void UpdateScienceExperiment(ModuleGroundExperiment moduleGroundExperiment)
	{
		if (moduleGroundExperiment != null && PartId == moduleGroundExperiment.part.persistentId)
		{
			this.moduleGroundExperiment = moduleGroundExperiment;
			scienceLimit = moduleGroundExperiment.ScienceLimit;
			scienceValue = moduleGroundExperiment.ScienceValue;
			scienceModifierRate = moduleGroundExperiment.ScienceModifierRate;
		}
	}

	public void CalculateScience(float solarMultiplier)
	{
		if (!(sciencePart == null) && sciencePart.Enabled && !(TotalScienceGenerated >= ScienceLimit) && ExperimentSituationValid && !(Cluster == null) && Cluster.IsPowered)
		{
			if (experimentId == DeployedScience.SeismicExperimentId)
			{
				LastScienceGeneratedUT = Planetarium.GetUniversalTime();
				return;
			}
			if (ExperimentVessel != null && Experiment != null && subject != null)
			{
				float num = ScienceValue / 60f / 60f * (float)(Planetarium.GetUniversalTime() - LastScienceGeneratedUT) * ScienceDiminishingModifierRate * solarMultiplier;
				if (TotalScienceGenerated + num > ScienceLimit)
				{
					num = ScienceLimit - TotalScienceGenerated;
				}
				totalScienceGenerated += num;
				storedScienceData += num / subject.subjectValue;
				GameEvents.onGroundScienceGenerated.Fire(this, sciencePart, Cluster, num);
			}
			LastScienceGeneratedUT = Planetarium.GetUniversalTime();
		}
		else
		{
			if (sciencePart != null && TotalScienceGenerated >= ScienceLimit && sciencePart.Enabled)
			{
				sciencePart.Enabled = false;
			}
			LastScienceGeneratedUT = Planetarium.GetUniversalTime();
		}
	}

	public void CalcSendSeismicScience(Vessel vessel, Part part, float solarMultiplier)
	{
		if (!(Cluster == null) && !(vessel == null) && !(experimentId != DeployedScience.SeismicExperimentId))
		{
			this.solarMultiplier = solarMultiplier;
			if (Cluster.IsPowered && sciencePart != null && sciencePart.Enabled)
			{
				if (ExperimentVessel != null && Experiment != null && subject != null)
				{
					double num = (vessel.loaded ? vessel.totalMass : ((double)vessel.GetTotalMass()));
					double num2 = 0.5 * (num * 1000.0) * Math.Pow(vessel.loaded ? ((double)vessel.rb_velocity.magnitude) : vessel.srfSpeed, 2.0);
					seismicEventVelocity = (vessel.loaded ? vessel.rb_velocity.magnitude : ((float)vessel.srfSpeed));
					Debug.LogFormat("[DeployedScience]: Cluster {0} on {1} Impact Detected on {2} - Impact of {3}J. RootPart {4} TotalParts {5} Vessel Mass {6} Velocity {7}", Cluster.ControlModulePartId, (Cluster.DeployedBody != null) ? Cluster.DeployedBody.bodyName : "Unknown", vessel.mainBody.name, num2, vessel.loaded ? vessel.rootPart.partInfo.title : "Unloaded", vessel.loaded ? vessel.parts.Count : vessel.protoVessel.protoPartSnapshots.Count, num, seismicEventVelocity);
					if (!seismicEventDataSendPending)
					{
						if (seismicPartList == null)
						{
							seismicPartList = new List<SeismicPartData>();
						}
						seismicPartList.Clear();
					}
					if (vessel.loaded)
					{
						if (part == null)
						{
							for (int i = 0; i < vessel.parts.Count; i++)
							{
								processImpactVesselPart(vessel, vessel.parts[i]);
							}
						}
						else
						{
							processImpactVesselPart(vessel, part);
						}
					}
					else
					{
						for (int j = 0; j < vessel.protoVessel.protoPartSnapshots.Count; j++)
						{
							if (SeismicListContains(vessel.protoVessel.protoPartSnapshots[j].persistentId))
							{
								continue;
							}
							SeismicPartData seismicPartData = new SeismicPartData();
							seismicPartData.partId = vessel.protoVessel.protoPartSnapshots[j].persistentId;
							seismicPartData.mass = vessel.protoVessel.protoPartSnapshots[j].mass;
							for (int k = 0; k < vessel.protoVessel.protoPartSnapshots[j].resources.Count; k++)
							{
								ProtoPartResourceSnapshot protoPartResourceSnapshot = vessel.protoVessel.protoPartSnapshots[j].resources[k];
								if (protoPartResourceSnapshot != null && protoPartResourceSnapshot.definition != null)
								{
									seismicPartData.resourceMass += (float)(protoPartResourceSnapshot.amount * (double)protoPartResourceSnapshot.definition.density);
								}
							}
							seismicPartData.mass += seismicPartData.resourceMass;
							seismicPartData.velocity = seismicEventVelocity;
							SetControllerVessel();
							if (ControllerVessel != null)
							{
								seismicPartData.impactDistance = CelestialUtilities.GreatCircleDistance(vessel.mainBody, vessel.latitude, vessel.longitude, ControllerVessel.latitude, ControllerVessel.longitude);
								double num3 = seismicPartData.impactDistance / vessel.mainBody.Radius;
								seismicPartData.impactDistanceMultiplier = distanceCurve.Evaluate((float)num3);
							}
							else
							{
								seismicPartData.impactDistance = vessel.mainBody.Radius;
							}
							seismicPartList.Add(seismicPartData);
						}
					}
					if (!seismicEventDataSendPending)
					{
						seismicEventDataSendPending = true;
						StartCoroutine(CallbackUtil.DelayedCallback(DeployedScience.Instance.SeismicScienceProcessingDelay, delegate
						{
							ProcessSeismicEvent();
						}));
					}
				}
				LastScienceGeneratedUT = Planetarium.GetUniversalTime();
			}
			else
			{
				if (!Cluster.IsPowered)
				{
					string text = Localizer.Format("#autoLOC_8002371", (Cluster.DeployedBody != null) ? Cluster.DeployedBody.bodyName : Localizer.Format("#autoLOC_8100159"));
					ScreenMessages.PostScreenMessage(text, 5f, ScreenMessageStyle.UPPER_CENTER, persist: true);
					Debug.Log("[DeployedScienceExperiment]: " + text);
				}
				if (sciencePart == null || (sciencePart != null && !sciencePart.Enabled))
				{
					string text2 = Localizer.Format("#autoLOC_8002372", (Cluster.DeployedBody != null) ? Cluster.DeployedBody.bodyName : Localizer.Format("#autoLOC_8100159"));
					ScreenMessages.PostScreenMessage(text2, 5f, ScreenMessageStyle.UPPER_CENTER, persist: true);
					Debug.Log("[DeployedScienceExperiment]: " + text2);
				}
			}
		}
		else
		{
			Debug.LogFormat("[DeployedScienceExperiment]: Cluster {0} on {1} Cannot calculate and send Seismic Data event - Invalid Call Parameters.", Cluster.ControlModulePartId, (Cluster.DeployedBody != null) ? Cluster.DeployedBody.bodyName : "Unknown");
		}
	}

	public void processImpactVesselPart(Vessel vessel, Part part)
	{
		if (!SeismicListContains(part.persistentId))
		{
			SeismicPartData seismicPartData = new SeismicPartData();
			seismicPartData.partId = part.persistentId;
			seismicPartData.velocity = ((part.rb != null) ? part.rb.velocity.magnitude : seismicEventVelocity);
			seismicPartData.resourceMass = part.GetResourceMass();
			seismicPartData.mass = part.mass + seismicPartData.resourceMass;
			SetControllerVessel();
			if (ControllerVessel != null)
			{
				seismicPartData.impactDistance = CelestialUtilities.GreatCircleDistance(vessel.mainBody, vessel.latitude, vessel.longitude, ControllerVessel.latitude, ControllerVessel.longitude);
				double num = seismicPartData.impactDistance / vessel.mainBody.Radius;
				seismicPartData.impactDistanceMultiplier = Mathf.Clamp01(distanceCurve.Evaluate((float)num));
			}
			else
			{
				seismicPartData.impactDistance = vessel.mainBody.Radius;
			}
			seismicPartList.Add(seismicPartData);
		}
	}

	public bool SeismicListContains(uint partId)
	{
		int num = 0;
		while (true)
		{
			if (num < seismicPartList.Count)
			{
				if (seismicPartList[num].partId == partId)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public void ProcessSeismicEvent()
	{
		Debug.LogFormat("[DeployedScienceExperiment]: Cluster {0} on {1} Processing Seismic Event Data", Cluster.ControlModulePartId, (Cluster.DeployedBody != null) ? Cluster.DeployedBody.bodyName : "Unknown");
		float num = 0f;
		totalJoules = 0.0;
		double num2 = 0.0;
		distanceMultiplier = 1.0;
		List<SeismicPartData> list = new List<SeismicPartData>(seismicPartList);
		for (int i = 0; i < list.Count; i++)
		{
			totalJoules += (double)(0.5f * (list[i].mass * 1000f)) * Math.Pow(list[i].velocity, 2.0);
			num += list[i].mass;
			num2 = list[i].impactDistance;
			distanceMultiplier = list[i].impactDistanceMultiplier;
		}
		impactRate = 0f;
		if (!DeployedScience.Instance.SeismicEnergyRates.TryGetValue(Cluster.DeployedBody.name, out impactRate))
		{
			impactRate = float.MaxValue;
		}
		if (totalJoules < (double)DeployedScience.Instance.MinimumSeismicEnergyRequired)
		{
			Debug.Log("[DeployedScienceExperiment]: Ignoring Impact less than " + DeployedScience.Instance.MinimumSeismicEnergyRequired + "Joules.");
			return;
		}
		Debug.LogFormat("[DeployedScience]: Seismic Event Detected on {0} - Impact of {1}J. Total Mass {2}. Initial Velocity {3}. Distance {4}. Distance Multiplier {5}", Cluster.DeployedBody.name, totalJoules, num, seismicEventVelocity, num2, distanceMultiplier);
		Debug.LogFormat("DeployedScience]: Diminishing Multiplier: {0} SolarPanel Multiplier {1} DataScale {2} Science Limit {3} Impact Percentage {4} Placement Trait Modifier {5}", scienceDiminishingModifierRate, solarMultiplier, subject.dataScale, scienceLimit, (float)totalJoules / impactRate, ScienceModifierRate / 100f);
		float num3 = ScienceDiminishingModifierRate * solarMultiplier * subject.dataScale * scienceLimit * ((float)totalJoules / impactRate) * (ScienceModifierRate / 100f);
		num3 *= (float)distanceMultiplier;
		if (seismicDataCalcOverride != null)
		{
			float num4 = seismicDataCalcOverride(this, (float)totalJoules, impactRate, seismicPartList);
			Debug.LogFormat("[DeployedScience]: Mod Override of SeismicDataCalc. Stock Calc was {0} - Mod Calc is {1}", num3, num4);
			num3 = num4;
		}
		if (TotalScienceGenerated + num3 > scienceLimit)
		{
			num3 = scienceLimit - totalScienceGenerated;
		}
		totalScienceGenerated += num3;
		storedScienceData += num3 / subject.subjectValue;
		Debug.LogFormat("[DeployedScience]: Impact Science Data Generated {0}", num3 / subject.subjectValue);
		GameEvents.onGroundScienceGenerated.Fire(this, sciencePart, Cluster, num3);
		bool flag = SendDataToComms();
		for (int j = 0; j < list.Count; j++)
		{
			seismicPartList.Remove(list[j]);
		}
		if (flag && seismicPartList.Count == 0)
		{
			seismicEventDataSendPending = false;
		}
		if (seismicPartList.Count > 0)
		{
			StartCoroutine(CallbackUtil.DelayedCallback(DeployedScience.Instance.SeismicScienceProcessingDelay, delegate
			{
				ProcessSeismicEvent();
			}));
		}
	}

	public bool TimeToSendStoredData()
	{
		if (experimentId == DeployedScience.SeismicExperimentId)
		{
			return true;
		}
		if (subject == null)
		{
			return false;
		}
		if (totalScienceGenerated >= scienceLimit)
		{
			return true;
		}
		if (StoredScienceData * subject.subjectValue / ScienceLimit * 100f >= 10f)
		{
			return true;
		}
		return false;
	}

	public bool GatherScienceData(out ScienceData experimentData)
	{
		experimentData = null;
		if (Experiment != null && !(ExperimentVessel == null) && subject != null && !(Cluster == null) && !(storedScienceData <= 0f) && ExperimentSituationValid && !(sciencePart == null))
		{
			experimentData = new ScienceData(StoredScienceData, 1f, xmitDataScalar, 0f, subject.id, subject.title);
			storedScienceData = 0f;
			return true;
		}
		if (Experiment == null || sciencePart == null || (ExperimentVessel == null && ExperimentSituationValid))
		{
			Debug.LogWarningFormat("[DeployedScienceExperiment]: Unable to take data from experiment {0} part Id {1} as Experiment, Science Part or Vessel reference are null.", ExperimentName, PartId);
		}
		return false;
	}

	public bool SendDataToComms()
	{
		if (Experiment != null && !(ExperimentVessel == null) && subject != null && !(Cluster == null) && sciencePart.Enabled && !(storedScienceData <= 0f) && ExperimentSituationValid)
		{
			if (CommNetScenario.CommNetEnabled && ControllerVessel == null && Cluster != null)
			{
				SetControllerVessel();
			}
			if ((Cluster.IsPowered && !CommNetScenario.CommNetEnabled) || (ControllerVessel != null && ControllerVessel.connection != null && ControllerVessel.connection.SignalStrength > 0.0 && ControllerVessel.connection.ControlPath.IsLastHopHome()))
			{
				float num = 0f;
				if ((bool)ResearchAndDevelopment.Instance)
				{
					num = ResearchAndDevelopment.Instance.SubmitScienceData(storedScienceData, subject);
					if (num > 0f)
					{
						if (ExperimentId == DeployedScience.SeismicExperimentId)
						{
							string text = Localizer.Format("#autoLOC_8002337", subject.title, num.ToString("F3"), (impactRate > 0f) ? (totalJoules / (double)impactRate * 100.0).ToString("F0") : "0", (distanceMultiplier * 100.0).ToString("F0"), scienceModifierRate.ToString("F0"), num.ToString("F3"), FlightGlobals.GetHomeBody().bodyDisplayName);
							ScreenMessages.PostScreenMessage(text, 5f, ScreenMessageStyle.UPPER_CENTER, persist: true);
							Debug.Log("[DeployedScienceExperiment]: " + text);
						}
						else
						{
							string text2 = Localizer.Format("#autoLOC_8002254", subject.title, num.ToString("F3"), FlightGlobals.GetHomeBody().bodyDisplayName);
							ScreenMessages.PostScreenMessage(text2, 5f, ScreenMessageStyle.UPPER_CENTER, persist: true);
							Debug.Log("[DeployedScienceExperiment]: " + text2);
						}
						if (!num.Equals(storedScienceData * subject.subjectValue))
						{
							Debug.LogFormat("[DeployedScienceExperiment]: scienceGenerated:{0} by RnD not equal what we expect:{1}", num, storedScienceData * subject.subjectValue);
						}
						transmittedScienceData += storedScienceData * subject.subjectValue;
					}
				}
				else if (ExperimentId == DeployedScience.SeismicExperimentId)
				{
					string text3 = Localizer.Format("#autoLOC_8002373", subject.title, num.ToString("F3"), (impactRate > 0f) ? (totalJoules / (double)impactRate * 100.0).ToString("F0") : "0", (distanceMultiplier * 100.0).ToString("F0"), scienceModifierRate.ToString("F0"), num.ToString("F3"), FlightGlobals.GetHomeBody().bodyDisplayName);
					ScreenMessages.PostScreenMessage(text3, 5f, ScreenMessageStyle.UPPER_CENTER, persist: true);
					Debug.Log("[DeployedScienceExperiment]: " + text3);
				}
				storedScienceData = 0f;
				totalJoules = 0.0;
				impactRate = 0f;
				noCommNetMsgPosted = false;
				GameEvents.onGroundScienceTransmitted.Fire(this, sciencePart, Cluster, num);
				return true;
			}
			float num2 = ResearchAndDevelopment.GetScienceValue(StoredScienceData, subject);
			if (num2 > 0f)
			{
				if (Cluster.IsPowered)
				{
					if (!noCommNetMsgPosted)
					{
						if (ExperimentId == DeployedScience.SeismicExperimentId)
						{
							string text4 = Localizer.Format("#autoLOC_8002338", subject.title, num2.ToString("F1"), (impactRate > 0f) ? (totalJoules / (double)impactRate * 100.0).ToString("F0") : "0", (distanceMultiplier * 100.0).ToString("F0"), scienceModifierRate.ToString("F0"), num2.ToString("F3"));
							ScreenMessages.PostScreenMessage(text4, 5f, ScreenMessageStyle.UPPER_CENTER, persist: true);
							Debug.Log("[DeployedScienceExperiment]: " + text4);
						}
						else
						{
							string text5 = Localizer.Format("#autoLOC_8002255", ExperimentVessel.mainBody.displayName);
							ScreenMessages.PostScreenMessage(text5, 3f, ScreenMessageStyle.UPPER_CENTER, persist: true);
							Debug.Log("[DeployedScienceExperiment]: " + text5);
						}
						noCommNetMsgPosted = true;
					}
				}
				else if (!noPowerMsgPosted)
				{
					string text6 = Localizer.Format("#autoLOC_8002256", ExperimentVessel.mainBody.displayName);
					ScreenMessages.PostScreenMessage(text6, 3f, ScreenMessageStyle.UPPER_CENTER, persist: true);
					noPowerMsgPosted = true;
					Debug.Log("[DeployedScienceExperiment]: " + text6);
				}
			}
			return false;
		}
		if (Experiment == null || (ExperimentVessel == null && ExperimentSituationValid))
		{
			Debug.LogWarningFormat("[DeployedScienceExperiment]: Unable to send data from experiment {0} part Id {1} as Experiment or Vessel reference are null.", ExperimentName, PartId);
		}
		return false;
	}

	public void SetControllerVessel()
	{
		FlightGlobals.FindLoadedPart(Cluster.ControlModulePartId, out var partout);
		if (partout != null)
		{
			ControllerVessel = partout.vessel;
			return;
		}
		FlightGlobals.FindUnloadedPart(Cluster.ControlModulePartId, out var partout2);
		if (partout2 != null && partout2.pVesselRef != null)
		{
			ControllerVessel = partout2.pVesselRef.vesselRef;
		}
	}

	public void Load(ConfigNode node)
	{
		node.TryGetValue("PartId", ref partId);
		node.TryGetValue("ExperimentName", ref experimentName);
		node.TryGetValue("ExperimentId", ref experimentId);
		node.TryGetValue("TotalScienceGenerated", ref totalScienceGenerated);
		node.TryGetValue("StoredScienceData", ref storedScienceData);
		node.TryGetValue("TransmittedScienceData", ref transmittedScienceData);
		node.TryGetValue("ScienceLimit", ref scienceLimit);
		node.TryGetValue("ScienceValue", ref scienceValue);
		node.TryGetValue("ScienceModifierRate", ref scienceModifierRate);
		node.TryGetValue("ScienceDiminishingModifierRate", ref scienceDiminishingModifierRate);
		node.TryGetValue("LastScienceGeneratedUT", ref LastScienceGeneratedUT);
		vesselID = Guid.Empty;
		node.TryGetValue("VesselId", ref vesselID);
		node.TryGetValue("noPowerMsgPosted", ref noPowerMsgPosted);
		node.TryGetValue("noCommNetMsgPosted", ref noCommNetMsgPosted);
		node.TryGetValue("totalJoules", ref totalJoules);
		node.TryGetValue("impactRate", ref impactRate);
		if (node.HasNode("distanceCurve"))
		{
			distanceCurve.Load(node.GetNode("distanceCurve"));
		}
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("PartId", PartId);
		if (!string.IsNullOrEmpty(ExperimentName))
		{
			node.AddValue("ExperimentName", ExperimentName);
		}
		if (!string.IsNullOrEmpty(ExperimentId))
		{
			node.AddValue("ExperimentId", ExperimentId);
		}
		node.AddValue("TotalScienceGenerated", TotalScienceGenerated);
		node.AddValue("StoredScienceData", StoredScienceData);
		node.AddValue("TransmittedScienceData", TransmittedScienceData);
		node.AddValue("ScienceLimit", ScienceLimit);
		node.AddValue("ScienceValue", ScienceValue);
		node.AddValue("ScienceModifierRate", ScienceModifierRate);
		node.AddValue("ScienceDiminishingModifierRate", ScienceDiminishingModifierRate);
		node.AddValue("LastScienceGeneratedUT", LastScienceGeneratedUT);
		node.AddValue("noPowerMsgPosted", noPowerMsgPosted);
		node.AddValue("noCommNetMsgPosted", noCommNetMsgPosted);
		if (totalJoules > 0.0)
		{
			node.AddValue("totalJoules", totalJoules);
		}
		if (impactRate > 0f)
		{
			node.AddValue("impactRate", impactRate);
		}
		if (ExperimentVessel != null)
		{
			node.AddValue("VesselId", ExperimentVessel.id);
		}
		ConfigNode node2 = node.AddNode("distanceCurve");
		distanceCurve.Save(node2);
	}
}

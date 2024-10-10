using System.Collections;
using UnityEngine;

public class CometVessel : VesselModule
{
	[KSPField(isPersistant = true)]
	public string typeName = "";

	[KSPField(isPersistant = true)]
	public uint cometPartId;

	public CometOrbitType cometType;

	[KSPField(isPersistant = true)]
	public double vfxStartDistance = 1.5;

	[KSPField(isPersistant = true)]
	public double comaWidthRatio = 10000.0;

	[KSPField(isPersistant = true)]
	public double tailMaxWidthRatio = 1.0;

	[KSPField(isPersistant = true)]
	public double tailMaxLengthRatio = 1000000.0;

	[KSPField(isPersistant = true)]
	public double ionTailMaxWidthRatio = 0.3;

	[KSPField(isPersistant = true)]
	public double comaWidth = 10000.0;

	[KSPField(isPersistant = true)]
	public double tailWidth = 1.0;

	[KSPField(isPersistant = true)]
	public double tailLength = 1000000.0;

	[KSPField(isPersistant = true)]
	public double ionTailWidth = 1.0;

	[KSPField(isPersistant = true)]
	public bool hasName;

	[KSPField(isPersistant = true)]
	public float radius = 115f;

	[KSPField(isPersistant = true)]
	public int seed;

	[KSPField(isPersistant = true)]
	public int numGeysers;

	[KSPField(isPersistant = true)]
	public int numNearDustEmitters;

	[KSPField(isPersistant = true)]
	public bool optimizedCollider;

	[KSPField(isPersistant = true)]
	public float fragmentDynamicPressureModifier;

	[SerializeField]
	public double homeBodySMA;

	public bool isCometTypeReady;

	public CometVFXController cometVFX;

	public WaitForEndOfFrame startWait;

	[SerializeField]
	public double lastUpdateUT;

	[SerializeField]
	public double currentDistanceToSun;

	[SerializeField]
	public float distanceRatio;

	[SerializeField]
	public float vfxRatio;

	[SerializeField]
	public bool vfxRatioSet;

	[SerializeField]
	public float nearDustVFXRatio;

	[SerializeField]
	public float geyserVFXRatio;

	[SerializeField]
	public float atmosphereVFXRatio;

	[SerializeField]
	public bool overrideVFXRatio;

	public CometOrbitType CometType => cometType;

	public double ComaRadius => comaWidth / 2.0;

	public float ComaWidthScaledSpace => (float)comaWidth * ScaledSpace.InverseScaleFactor;

	public float ComaRadiusScaledSpace => (float)ComaRadius * ScaledSpace.InverseScaleFactor;

	public float TailWidthScaledSpace => (float)tailWidth * ScaledSpace.InverseScaleFactor;

	public float TailLengthScaledSpace => (float)tailLength * ScaledSpace.InverseScaleFactor;

	public float IonTailWidthScaledSpace => (float)ionTailWidth * ScaledSpace.InverseScaleFactor;

	public override void OnAwake()
	{
		if ((bool)vessel)
		{
			vessel.Comet = this;
		}
		startWait = new WaitForEndOfFrame();
		GameEvents.onKnowledgeChanged.Add(OnKnowledgeChange);
		GameEvents.onVesselPersistentIdChanged.Add(OnVesselPersistentIdChanged);
		GameEvents.onVesselsUndocking.Add(OnPartDecouple);
		GameEvents.onPartDeCoupleNewVesselComplete.Add(OnPartDecouple);
		GameEvents.onPartPersistentIdChanged.Add(OnPartPersistentIdChanged);
	}

	public override void OnStart()
	{
		bool flag = true;
		if (vessel.parts.Count > 0)
		{
			for (int i = 0; i < vessel.parts.Count; i++)
			{
				if (vessel.parts[i].name.Contains("PotatoComet"))
				{
					if (cometPartId == 0 && !string.IsNullOrEmpty(typeName))
					{
						cometPartId = vessel.parts[i].persistentId;
						flag = false;
						break;
					}
					if (cometPartId == vessel.parts[i].persistentId)
					{
						flag = false;
						break;
					}
				}
			}
		}
		if (flag && vessel.protoVessel != null)
		{
			for (int j = 0; j < vessel.protoVessel.protoPartSnapshots.Count; j++)
			{
				if (!(vessel.protoVessel.protoPartSnapshots[j].partName == "PotatoComet"))
				{
					continue;
				}
				if (cometPartId == 0)
				{
					ConfigNode node = null;
					if (vessel.protoVessel.vesselModules.TryGetNode("CometVessel", ref node))
					{
						string value = "";
						if (node.TryGetValue("typeName", ref value) && !string.IsNullOrEmpty(value))
						{
							cometPartId = vessel.protoVessel.protoPartSnapshots[j].persistentId;
							flag = false;
							break;
						}
					}
				}
				if (cometPartId == vessel.protoVessel.protoPartSnapshots[j].persistentId)
				{
					flag = false;
					break;
				}
			}
		}
		if (flag)
		{
			vessel.vesselModules.Remove(this);
			vessel.Comet = null;
			Object.Destroy(this);
			return;
		}
		cometType = CometManager.GetCometOrbitType(typeName);
		if (cometType == null)
		{
			Debug.LogError("[CometVessel]: Unable to find CometType:" + typeName);
			return;
		}
		isCometTypeReady = true;
		lastUpdateUT = Planetarium.GetUniversalTime();
		if ((bool)FlightGlobals.fetch)
		{
			homeBodySMA = FlightGlobals.GetHomeBody().orbit.semiMajorAxis;
		}
		SetSituationRanges(vessel.vesselRanges.orbit);
		SetSituationRanges(vessel.vesselRanges.subOrbital);
		SetSituationRanges(vessel.vesselRanges.escaping);
		SetSituationRanges(vessel.vesselRanges.flying);
		SetSituationRanges(vessel.vesselRanges.splashed);
		SetSituationRanges(vessel.vesselRanges.landed);
		SetSituationRanges(vessel.vesselRanges.prelaunch);
		StartCoroutine("WaitAndUpdateVFXDimensions");
	}

	public void SetSituationRanges(VesselRanges.Situation sit)
	{
		sit.load = CometManager.Instance.vesselRangeOrbitLoadOverride;
		sit.unload = CometManager.Instance.vesselRangeOrbitUnloadOverride;
		sit.pack = CometManager.Instance.vesselRangeOrbitPackOverride;
		sit.unpack = CometManager.Instance.vesselRangeOrbitUnpackOverride;
	}

	public void OnDestroy()
	{
		GameEvents.onKnowledgeChanged.Remove(OnKnowledgeChange);
		optimizedCollider = false;
		GameEvents.onVesselPersistentIdChanged.Remove(OnVesselPersistentIdChanged);
		GameEvents.onVesselsUndocking.Remove(OnPartDecouple);
		GameEvents.onPartDeCoupleNewVesselComplete.Remove(OnPartDecouple);
		GameEvents.onPartPersistentIdChanged.Remove(OnPartPersistentIdChanged);
	}

	public void OnPartPersistentIdChanged(uint vesselId, uint oldId, uint newId)
	{
		if (cometPartId != 0 && cometPartId == oldId)
		{
			cometPartId = newId;
		}
	}

	public void OnVesselPersistentIdChanged(uint oldId, uint newId)
	{
		if (vessel != null && oldId == vessel.persistentId && FlightGlobals.PersistentVesselIds.ContainsKey(newId))
		{
			Part partout = null;
			FlightGlobals.FindLoadedPart(cometPartId, out partout);
			if (partout != null)
			{
				MoveCometVesselModule(newId, partout);
			}
		}
	}

	public void OnPartDecouple(Vessel oldVessel, Vessel newVessel)
	{
		if (!(vessel != null) || (oldVessel.persistentId != vessel.persistentId && newVessel.persistentId != vessel.persistentId))
		{
			return;
		}
		Part partout = null;
		FlightGlobals.FindLoadedPart(cometPartId, out partout);
		if (!(partout != null) || partout.vessel.persistentId == vessel.persistentId)
		{
			return;
		}
		int count = partout.vessel.vesselModules.Count;
		while (count-- > 0)
		{
			CometVessel cometVessel = partout.vessel.vesselModules[count] as CometVessel;
			if (cometVessel != null && cometVessel.cometPartId == 0 && partout.vessel.vesselModules[count] != this)
			{
				Object.Destroy(partout.vessel.vesselModules[count]);
				partout.vessel.vesselModules.RemoveAt(count);
			}
		}
		MoveCometVesselModule(partout.vessel.persistentId, partout);
	}

	public void MoveCometVesselModule(uint newId, Part cometPart)
	{
		base.vessel.vesselModules.Remove(this);
		base.vessel.Comet = null;
		Vessel vessel = FlightGlobals.PersistentVesselIds[newId];
		CometVessel cometVessel = vessel.gameObject.AddComponent<CometVessel>();
		cometVessel.GetCopyOf(this);
		vessel.vesselModules.Add(cometVessel);
		cometVessel.vessel = vessel;
		ModuleComet moduleComet = cometPart.FindModuleImplementing<ModuleComet>();
		if (moduleComet != null)
		{
			moduleComet.cometVessel = cometVessel;
		}
		Vessel vessel2 = base.vessel;
		base.vessel = null;
		vessel2.vesselModules.Remove(this);
		GameEvents.onCometVesselChanged.Fire(vessel2, vessel, this, cometVessel);
		Object.Destroy(this);
		cometVessel.vfxRatioSet = false;
		cometVessel.StartCoroutine("WaitAndUpdateVFXDimensions");
	}

	public void OnKnowledgeChange(GameEvents.HostedFromToAction<IDiscoverable, DiscoveryLevels> kChg)
	{
		Vessel vessel = kChg.host as Vessel;
		if (base.vessel != null && vessel != null && vessel.persistentId == base.vessel.persistentId && !hasName && vessel.DiscoveryInfo.HaveKnowledgeAbout(DiscoveryLevels.StateVectors))
		{
			string to = (vessel.vesselName = DiscoverableObjectsUtil.GenerateCometName());
			base.gameObject.name = base.gameObject.name.Replace("UnknownComet", vessel.vesselName);
			vessel.mapObject.gameObject.name = vessel.mapObject.gameObject.name.Replace("UnknownComet", vessel.vesselName);
			GameEvents.onVesselRename.Fire(new GameEvents.HostedFromToAction<Vessel, string>(base.vessel, vessel.vesselName, to));
			hasName = true;
		}
	}

	public virtual void Update()
	{
		if (isCometTypeReady)
		{
			double universalTime = Planetarium.GetUniversalTime();
			double num = CometManager.VFXUpdateFrequency;
			if (vessel.mainBody != null && Planetarium.fetch != null && vessel.mainBody != Planetarium.fetch.Sun)
			{
				num = CometManager.VFXUpdateFrequencyInAtmosphere;
			}
			if (universalTime > lastUpdateUT + num)
			{
				lastUpdateUT = universalTime;
				UpdateVFXDimensions();
			}
		}
	}

	public void UpdateVFXDimensions()
	{
		if (homeBodySMA < 1.0 && (bool)FlightGlobals.fetch)
		{
			homeBodySMA = FlightGlobals.GetHomeBody().orbit.semiMajorAxis;
		}
		currentDistanceToSun = (vessel.GetWorldPos3D() - Planetarium.fetch.Sun.position).magnitude;
		distanceRatio = (float)(currentDistanceToSun / (vfxStartDistance * homeBodySMA));
		vfxRatio = (overrideVFXRatio ? vfxRatio : CometManager.Instance.vfxSizeFromDistance.Evaluate(distanceRatio));
		nearDustVFXRatio = (overrideVFXRatio ? nearDustVFXRatio : CometManager.Instance.nearDustVFXSizeFromDistance.Evaluate(distanceRatio));
		geyserVFXRatio = (overrideVFXRatio ? geyserVFXRatio : CometManager.Instance.geyserVFXSizeFromDistance.Evaluate(distanceRatio));
		atmosphereVFXRatio = 1f;
		if (vessel.mainBody != null && Planetarium.fetch != null && vessel.mainBody != Planetarium.fetch.Sun)
		{
			float num = 1f;
			if (vessel.mainBody.atmosphere)
			{
				num = 1f - (float)vessel.mainBody.GetPressure(vessel.altitude) / CometManager.Instance.atmospherePressureForVFXFade;
				num = Mathf.Clamp01(num);
				num = CometManager.Instance.atmospherePressureFadeDistributionForVFXFade.Evaluate(num);
			}
			float num2 = Mathf.Abs((float)vessel.graviticAcceleration.magnitude);
			float num3 = num2 * CometManager.Instance.gravityMultiplierForVFXFadeEnd - CometManager.Instance.gravitySubtractionForVFXFadeEnd;
			float num4 = num3 + num2 * CometManager.Instance.gravityAdditionForVFXFadeStart;
			float value = ((float)vessel.srfSpeed - num3) / (num4 - num3);
			value = Mathf.Clamp01(value);
			atmosphereVFXRatio = num * value;
		}
		comaWidth = (double)vfxRatio * comaWidthRatio * (double)radius;
		tailWidth = tailMaxWidthRatio * comaWidth;
		ionTailWidth = ionTailMaxWidthRatio * comaWidth;
		tailLength = tailMaxLengthRatio * comaWidth;
		vfxRatioSet = true;
		GameEvents.onCometVFXDimensionsModified.Fire(this);
	}

	public IEnumerator WaitAndUpdateVFXDimensions()
	{
		while (vessel.orbitDriver == null || !vessel.orbitDriver.Ready || !vessel.initialPosVelSet)
		{
			yield return startWait;
		}
		yield return null;
		UpdateVFXDimensions();
	}

	public void SetCollider(bool enabled)
	{
		for (int i = 0; i < vessel.Parts.Count; i++)
		{
			ModuleComet component = vessel.Parts[i].GetComponent<ModuleComet>();
			if (component != null && component.pcGenerated != null && (bool)component.pcGenerated.colliderObject && component.pcGenerated.colliderObject.activeSelf != enabled)
			{
				component.pcGenerated.colliderObject.SetActive(enabled);
			}
		}
	}

	public void SetGeyser(bool enabled, float emissionRate)
	{
		for (int i = 0; i < vessel.Parts.Count; i++)
		{
			ModuleComet component = vessel.Parts[i].GetComponent<ModuleComet>();
			if (component != null)
			{
				component.SetGeyser(enabled, emissionRate);
			}
		}
	}

	public void SetDust(bool enabled, float emissionRate)
	{
		for (int i = 0; i < vessel.Parts.Count; i++)
		{
			ModuleComet component = vessel.Parts[i].GetComponent<ModuleComet>();
			if (component != null)
			{
				component.SetDust(enabled, emissionRate);
			}
		}
	}

	public void SetComa(bool enabled)
	{
		if (cometVFX != null && cometVFX.comaEffect != null && cometVFX.comaEffect.enabled != enabled)
		{
			cometVFX.comaEffect.enabled = enabled;
		}
	}

	public void SetIonTail(bool enabled, float emissionRate)
	{
		if (!(cometVFX != null) || !(cometVFX.ionController != null))
		{
			return;
		}
		cometVFX.ionController.debugEmitterMultiplier = emissionRate;
		if (cometVFX.ionController.enabled != enabled)
		{
			cometVFX.ionController.enabled = enabled;
			cometVFX.ionController.pSystem.gameObject.SetActive(enabled);
			if (enabled)
			{
				ReprimeParticles();
			}
		}
	}

	public void SetDustTail(bool enabled, float emissionRate, float size)
	{
		if (!(cometVFX != null) || !(cometVFX.dustController != null))
		{
			return;
		}
		cometVFX.dustController.debugEmitterMultiplier = emissionRate;
		cometVFX.dustController.debugSizeMultiplier = size;
		if (cometVFX.dustController.enabled != enabled)
		{
			cometVFX.dustController.enabled = enabled;
			cometVFX.dustController.pSystem.gameObject.SetActive(enabled);
			if (enabled)
			{
				ReprimeParticles();
			}
		}
	}

	public void ReprimeParticles()
	{
		if (!(cometVFX != null))
		{
			return;
		}
		if (cometVFX.dustController != null)
		{
			if (cometVFX.dustController.pSystem.isPlaying)
			{
				cometVFX.dustController.pSystem.Stop();
			}
			cometVFX.dustController.pSystem.Play();
		}
		if (cometVFX.ionController != null)
		{
			if (cometVFX.ionController.pSystem.isPlaying)
			{
				cometVFX.ionController.pSystem.Stop();
			}
			cometVFX.ionController.pSystem.Play();
		}
	}
}

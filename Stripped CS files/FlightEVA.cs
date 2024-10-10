using System;
using System.Collections;
using ns9;
using UnityEngine;

public class FlightEVA : MonoBehaviour
{
	public delegate KerbalEVA SpawnKerbalEVADelegate(ProtoCrewMember pcm);

	public static FlightEVA fetch;

	public float airlockStandoff = 0.18f;

	public static SpawnKerbalEVADelegate Spawn = _Spawn;

	public ProtoCrewMember pCrew;

	public Part fromPart;

	public Transform fromAirlock;

	public bool overrideEVA;

	public void Awake()
	{
		if ((bool)fetch)
		{
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			fetch = this;
		}
	}

	public void OnDestroy()
	{
		if (fetch != null && fetch == this)
		{
			fetch = null;
		}
	}

	public static KerbalEVA SpawnEVA(Kerbal crew)
	{
		return fetch.spawnEVA(crew.protoCrewMember, crew.InPart, crew.InPart.airlock, tryAllHatches: true);
	}

	public static KerbalEVA _Spawn(ProtoCrewMember pcm)
	{
		return UnityEngine.Object.Instantiate(PartLoader.getPartInfoByName(pcm.GetKerbalEVAPartName()).partPrefab.gameObject).GetComponent<KerbalEVA>();
	}

	[ContextMenu("Spawn EVA")]
	public void spawnEVA()
	{
		ProtoCrewMember nextOrNewKerbal = HighLogic.CurrentGame.CrewRoster.GetNextOrNewKerbal();
		KerbalEVA kerbalEVA = Spawn(nextOrNewKerbal);
		kerbalEVA.gameObject.SetActive(value: true);
		kerbalEVA.part.vessel = kerbalEVA.gameObject.AddComponent<Vessel>();
		kerbalEVA.vessel.Initialize();
		kerbalEVA.vessel.id = Guid.NewGuid();
		kerbalEVA.transform.position = FlightGlobals.ActiveVessel.rootPart.transform.position + FlightGlobals.ActiveVessel.rootPart.transform.rotation * Vector3.right * 2f;
		kerbalEVA.GetComponent<Rigidbody>().velocity = FlightGlobals.ActiveVessel.GetComponent<Rigidbody>().velocity;
		kerbalEVA.part.AddCrewmember(nextOrNewKerbal);
		kerbalEVA.gameObject.name = nextOrNewKerbal.GetKerbalEVAPartName() + " (" + nextOrNewKerbal.name + ")";
		kerbalEVA.vessel.vesselName = nextOrNewKerbal.name;
		kerbalEVA.vessel.vesselType = VesselType.const_11;
		kerbalEVA.vessel.launchedFrom = FlightGlobals.ActiveVessel.launchedFrom;
		kerbalEVA.vessel.orbit.referenceBody = FlightGlobals.getMainBody(kerbalEVA.transform.position);
		kerbalEVA.part.flagURL = FlightGlobals.ActiveVessel.rootPart.flagURL;
		kerbalEVA.part.flightID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
		kerbalEVA.part.missionID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
		kerbalEVA.part.launchID = FlightGlobals.ActiveVessel.rootPart.launchID;
		GameEvents.onCrewOnEva.Fire(new GameEvents.FromToAction<Part, Part>(null, kerbalEVA.part));
		GameEvents.onCrewTransferred.Fire(new GameEvents.HostedFromToAction<ProtoCrewMember, Part>(nextOrNewKerbal, null, kerbalEVA.part));
		Vessel.CrewWasModified(kerbalEVA.vessel);
		StartCoroutine(SwitchToEVAVesselWhenReady(kerbalEVA));
	}

	public static bool HatchIsObstructed(Part fromPart, Transform fromAirlock)
	{
		RaycastHit hitInfo;
		return Physics.Raycast(fromAirlock.transform.position - 0.5f * (fromAirlock.position - fromPart.transform.position).normalized, (fromAirlock.position - fromPart.transform.position).normalized, out hitInfo, fromPart.hatchObstructionCheckOutwardDistance + 0.5f, LayerUtil.DefaultEquivalent | 0x8000 | 0x80000 | 0x4000000, QueryTriggerInteraction.Ignore);
	}

	public static bool HatchIsObstructedMore(Part fromPart, Transform fromAirlock)
	{
		if (HatchIsObstructed(fromPart, fromAirlock))
		{
			return true;
		}
		Vector3 normalized = (fromPart.transform.position - fromAirlock.position).normalized;
		if (Physics.Raycast(fromAirlock.transform.position - normalized * fromPart.hatchObstructionCheckInwardOffset, normalized, out var hitInfo, fromPart.hatchObstructionCheckInwardDistance, LayerUtil.DefaultEquivalent | 0x8000 | 0x80000 | 0x4000000))
		{
			Part component = hitInfo.transform.gameObject.GetComponent<Part>();
			if (component != null && component != fromPart && !component.frozen)
			{
				return true;
			}
		}
		Collider[] array = Physics.OverlapSphere(fromAirlock.transform.position + normalized * fromPart.hatchObstructionCheckSphereOffset, fromPart.hatchObstructionCheckSphereRadius, 67665921);
		int num = 0;
		int num2 = array.Length;
		while (true)
		{
			if (num < num2)
			{
				Part componentInParent = array[num].GetComponentInParent<Part>();
				if (componentInParent != null && componentInParent != fromPart && !componentInParent.frozen && !array[num].isTrigger)
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

	public static bool hatchInsideFairing(Part fromPart)
	{
		if (fromPart.ShieldedFromAirstream && fromPart.airstreamShields.Count > 0)
		{
			ModuleProceduralFairing module = fromPart.airstreamShields[0].GetPart().Modules.GetModule<ModuleProceduralFairing>();
			if (module != null && module.interstageCraftID == 0 && module.isCapped)
			{
				return true;
			}
		}
		return false;
	}

	public KerbalEVA spawnEVA(ProtoCrewMember pCrew, Part fromPart, Transform fromAirlock, bool tryAllHatches = false)
	{
		if (!fromPart.protoModuleCrew.Contains(pCrew))
		{
			Debug.LogError("[FlightEVA]: Tried to bail out " + pCrew.name + " from part " + fromPart.name + " but part doesn't contain that kerbal!");
			return null;
		}
		if (!fromAirlock)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_111934"), 5f, ScreenMessageStyle.UPPER_CENTER);
			return null;
		}
		Transform data = fromAirlock;
		if (tryAllHatches)
		{
			bool flag = false;
			if (!HatchIsObstructedMore(fromPart, data))
			{
				if (!hatchInsideFairing(fromPart))
				{
					flag = true;
					data = fromAirlock;
				}
			}
			else
			{
				Collider[] componentsInChildren = fromPart.GetComponentsInChildren<Collider>();
				int num = componentsInChildren.Length;
				for (int i = 0; i < num; i++)
				{
					Collider collider = componentsInChildren[i];
					if (collider.gameObject.CompareTag("Airlock") && !(fromAirlock == collider.transform) && !HatchIsObstructed(fromPart, collider.transform) && !hatchInsideFairing(fromPart))
					{
						flag = true;
						data = collider.transform;
						break;
					}
				}
			}
			if (!flag)
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_111971"), 5f, ScreenMessageStyle.UPPER_CENTER);
				return null;
			}
		}
		else if (HatchIsObstructed(fromPart, data) || hatchInsideFairing(fromPart))
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_111978"), 5f, ScreenMessageStyle.UPPER_CENTER);
			return null;
		}
		overrideEVA = false;
		GameEvents.onAttemptEva.Fire(pCrew, fromPart, data);
		if (overrideEVA)
		{
			return null;
		}
		this.pCrew = pCrew;
		this.fromPart = fromPart;
		this.fromAirlock = data;
		return onGoForEVA();
	}

	public KerbalEVA onGoForEVA()
	{
		if ((bool)pCrew.KerbalRef)
		{
			pCrew.KerbalRef.state = Kerbal.States.BAILED_OUT;
		}
		KerbalEVA kerbalEVA = Spawn(pCrew);
		CollisionEnhancer.bypass = true;
		kerbalEVA.gameObject.SetActive(value: true);
		kerbalEVA.part.vessel = kerbalEVA.gameObject.AddComponent<Vessel>();
		if ((bool)fromAirlock)
		{
			kerbalEVA.transform.rotation = fromAirlock.rotation;
			kerbalEVA.transform.position = fromAirlock.position + fromAirlock.rotation * (Vector3.back * airlockStandoff);
		}
		else
		{
			kerbalEVA.transform.position = fromPart.transform.position + fromPart.transform.rotation * Vector3.right * 2f;
		}
		kerbalEVA.vessel.Initialize();
		kerbalEVA.vessel.id = Guid.NewGuid();
		if ((bool)fromAirlock)
		{
			kerbalEVA.GetComponent<Rigidbody>().velocity = fromPart.Rigidbody.GetPointVelocity(fromPart.airlock.position);
			kerbalEVA.GetComponent<Rigidbody>().angularVelocity = fromPart.Rigidbody.angularVelocity;
		}
		else
		{
			kerbalEVA.GetComponent<Rigidbody>().velocity = FlightGlobals.ActiveVessel.GetComponent<Rigidbody>().velocity;
		}
		if (fromPart != null)
		{
			double num = fromPart.temperature;
			if (num > kerbalEVA.part.maxTemp - 50.0)
			{
				num = kerbalEVA.part.maxTemp - 50.0;
			}
			if (num < PhysicsGlobals.SpaceTemperature)
			{
				num = PhysicsGlobals.SpaceTemperature;
			}
			if (GameSettings.EVA_INHERIT_PART_TEMPERATURE)
			{
				kerbalEVA.part.skinTemperature = (kerbalEVA.part.skinUnexposedTemperature = (kerbalEVA.part.temperature = num));
			}
			else
			{
				kerbalEVA.part.skinTemperature = (kerbalEVA.part.skinUnexposedTemperature = (kerbalEVA.part.temperature = kerbalEVA.evaExitTemperature));
			}
			kerbalEVA.part.staticPressureAtm = fromPart.staticPressureAtm;
		}
		pCrew.flightLog.AddEntryUnique(FlightLog.EntryType.ExitVessel, fromPart.vessel.orbit.referenceBody.name);
		fromPart.RemoveCrewmember(pCrew);
		kerbalEVA.part.AddCrewmember(pCrew);
		if ((bool)KerbalInventoryScenario.Instance)
		{
			KerbalInventoryScenario.Instance.RemoveKerbalInventoryInstance(pCrew.name);
		}
		kerbalEVA.gameObject.name = pCrew.GetKerbalEVAPartName() + " (" + pCrew.name + ")";
		kerbalEVA.vessel.vesselName = pCrew.name;
		kerbalEVA.vessel.vesselType = VesselType.const_11;
		kerbalEVA.vessel.orbit.referenceBody = FlightGlobals.getMainBody(kerbalEVA.transform.position);
		kerbalEVA.vessel.lastVel = kerbalEVA.vessel.orbit.GetRelativeVel() - kerbalEVA.vessel.orbit.GetRotFrameVel(kerbalEVA.vessel.orbit.referenceBody);
		kerbalEVA.vessel.launchedFrom = fromPart.vessel.launchedFrom;
		kerbalEVA.vessel.IgnoreGForces(10);
		kerbalEVA.part.flightID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
		kerbalEVA.part.missionID = fromPart.missionID;
		kerbalEVA.part.launchID = fromPart.launchID;
		kerbalEVA.part.flagURL = fromPart.flagURL;
		GameEvents.onCrewOnEva.Fire(new GameEvents.FromToAction<Part, Part>(fromPart, kerbalEVA.part));
		GameEvents.onCrewTransferred.Fire(new GameEvents.HostedFromToAction<ProtoCrewMember, Part>(pCrew, fromPart, kerbalEVA.part));
		Vessel.CrewWasModified(fromPart.vessel, kerbalEVA.vessel);
		StartCoroutine(SwitchToEVAVesselWhenReady(kerbalEVA));
		if (!fromPart.vessel.LandedOrSplashed)
		{
			StartCoroutine(kerbalEVA.StartNonCollidePeriod(1f, airlockStandoff, fromPart, fromAirlock));
		}
		pCrew = null;
		fromPart = null;
		return kerbalEVA;
	}

	public void onEVAAborted()
	{
		pCrew = null;
		fromPart = null;
	}

	public IEnumerator SwitchToEVAVesselWhenReady(KerbalEVA eva)
	{
		while (!eva.Ready)
		{
			yield return null;
		}
		if (FlightGlobals.ForceSetActiveVessel(eva.vessel))
		{
			FlightInputHandler.SetNeutralControls();
		}
	}
}

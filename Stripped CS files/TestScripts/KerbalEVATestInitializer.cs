using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestScripts;

public class KerbalEVATestInitializer : MonoBehaviour
{
	public NestedPrefabSpawner[] spawnerObjs;

	public void Start()
	{
		int i = 0;
		for (int num = spawnerObjs.Length; i < num; i++)
		{
			spawnerObjs[i].OnSpawn.Add(OnSpawn);
		}
	}

	public void OnSpawn(NestedPrefabSpawner.NestedPrefab spawned)
	{
		StartCoroutine(InitializeEVA(spawned.Instance));
	}

	public IEnumerator InitializeEVA(GameObject spawnedObject)
	{
		KerbalEVA eva;
		do
		{
			eva = spawnedObject.GetComponent<KerbalEVA>();
			if (eva == null)
			{
				yield return null;
			}
		}
		while (eva == null);
		eva.part = eva.GetComponent<Part>();
		eva.gameObject.SetActive(value: true);
		eva.part.vessel = eva.gameObject.AddComponent<Vessel>();
		eva.vessel.rootPart = eva.part;
		eva.vessel.parts = new List<Part>();
		eva.vessel.parts.Add(eva.part);
		GameEvents.onVesselPartCountChanged.Fire(eva.vessel);
		eva.vessel.id = Guid.NewGuid();
		eva.vessel.vesselName = eva.name;
		eva.vessel.vesselType = VesselType.const_11;
		if (!GetComponent<OrbitDriver>())
		{
			eva.vessel.orbitDriver = eva.gameObject.AddComponent<OrbitDriver>();
		}
		if (!GetComponent<OrbitRenderer>())
		{
			eva.vessel.orbitRenderer = eva.gameObject.AddComponent<OrbitRenderer>();
		}
		FlightGlobals.AddVessel(eva.vessel);
		eva.vessel.launchTime = Planetarium.GetUniversalTime();
		eva.vessel.missionTime = 0.0;
		eva.vessel.SetLoaded(loaded: true);
		eva.part.flagURL = "";
		yield return null;
		eva.vessel.orbitDriver.referenceBody = FlightGlobals.getMainBody(eva.gameObject.transform.position);
		if (FlightGlobals.ActiveVessel == null)
		{
			FlightGlobals.SetActiveVessel(eva.vessel);
		}
		OrbitPhysicsManager.HoldVesselUnpack(10);
		eva.vessel.SetWorldVelocity(Vector3d.zero);
		yield return null;
		if (FlightGlobals.ActiveVessel == eva.vessel)
		{
			FlightCamera.fetch.EnableCamera();
			FlightCamera.fetch.enabled = true;
			FlightCamera.fetch.startDistance = 10f;
			FlightCamera.fetch.SetDistanceImmediate(10f);
		}
	}
}

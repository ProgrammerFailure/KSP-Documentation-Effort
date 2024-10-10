using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VesselRetrieval : MonoBehaviour
{
	public int KSCFrameDelay = 8;

	public HashSet<Guid> IDsOfVesselsToRecover;

	public void Awake()
	{
		IDsOfVesselsToRecover = new HashSet<Guid>();
		GameEvents.OnVesselRecoveryRequested.Add(onVesselRecoveryRequested);
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public void OnDestroy()
	{
		GameEvents.OnVesselRecoveryRequested.Remove(onVesselRecoveryRequested);
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		StartCoroutine(OnLevelLoaded(HighLogic.GetLoadedGameSceneFromBuildIndex(scene.buildIndex)));
	}

	public IEnumerator OnLevelLoaded(GameScenes scene)
	{
		switch (scene)
		{
		case GameScenes.SPACECENTER:
			if (IDsOfVesselsToRecover.Count > 0)
			{
				int i = 0;
				while (i < KSCFrameDelay)
				{
					yield return null;
					int num = i + 1;
					i = num;
				}
				recoverVessels();
			}
			break;
		}
	}

	public void onVesselRecoveryRequested(Vessel v)
	{
		IDsOfVesselsToRecover.Add(v.id);
		GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
		HighLogic.LoadScene(GameScenes.SPACECENTER);
	}

	public void recoverVessels()
	{
		if (IDsOfVesselsToRecover.Count > 0)
		{
			List<Vessel> vesselsToRecover = getVesselsToRecover(IDsOfVesselsToRecover);
			int count = vesselsToRecover.Count;
			for (int i = 0; i < count; i++)
			{
				recoverVessel(vesselsToRecover[i]);
			}
			GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
			IDsOfVesselsToRecover.Clear();
		}
	}

	public List<Vessel> getVesselsToRecover(HashSet<Guid> vesselIDs)
	{
		List<Vessel> list = new List<Vessel>();
		int count = FlightGlobals.Vessels.Count;
		for (int i = 0; i < count; i++)
		{
			Vessel vessel = FlightGlobals.Vessels[i];
			if (vesselIDs.Contains(vessel.id))
			{
				list.Add(vessel);
			}
		}
		return list;
	}

	public void recoverVessel(Vessel v)
	{
		GameEvents.onVesselRecovered.Fire(v.protoVessel, data1: false);
		UnityEngine.Object.DestroyImmediate(v.gameObject);
	}
}

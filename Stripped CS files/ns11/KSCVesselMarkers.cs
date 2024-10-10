using System.Collections.Generic;
using UnityEngine;

namespace ns11;

public class KSCVesselMarkers : MonoBehaviour
{
	public List<KSCVesselMarker> markers;

	public static KSCVesselMarkers fetch;

	public void Awake()
	{
		fetch = null;
		GameEvents.onGUILaunchScreenSpawn.Add(onGUIVesselScreenSpawn);
		GameEvents.onGUIRnDComplexSpawn.Add(onGUIScreenSpawn);
		GameEvents.onGUIAstronautComplexSpawn.Add(onGUIScreenSpawn);
		GameEvents.onGUIMissionControlSpawn.Add(onGUIScreenSpawn);
		GameEvents.onGUILaunchScreenDespawn.Add(onGUIScreenDespawn);
		GameEvents.onGUIRnDComplexDespawn.Add(onGUIScreenDespawn);
		GameEvents.onGUIAstronautComplexDespawn.Add(onGUIACScreenDespawn);
		GameEvents.onGUIMissionControlDespawn.Add(onGUIScreenDespawn);
		GameEvents.onGameSceneLoadRequested.Add(onSceneLoadRequested);
		GameEvents.onVesselWillDestroy.Add(onVesselDestroy);
		markers = new List<KSCVesselMarker>();
		StartCoroutine(CallbackUtil.DelayedCallback(15, SpawnVesselMarkers));
		fetch = this;
	}

	public void SpawnVesselMarkers()
	{
		int count = FlightGlobals.Vessels.Count;
		for (int i = 0; i < count; i++)
		{
			Vessel vessel = FlightGlobals.Vessels[i];
			if (vessel != null && vessel.LandedOrSplashed && vessel.mainBody == Planetarium.fetch.Home && vessel.vesselType != VesselType.DeployedSciencePart && vessel.vesselType != VesselType.DroppedPart)
			{
				markers.Add(KSCVesselMarker.Create(vessel, OnMarkerDismiss));
			}
		}
	}

	public void ClearVesselMarkers()
	{
		int count = markers.Count;
		for (int i = 0; i < count; i++)
		{
			markers[i].Terminate();
		}
		markers.Clear();
	}

	public void OnMarkerDismiss(Vessel v, KSCVesselMarker.DismissAction dma)
	{
		switch (dma)
		{
		case KSCVesselMarker.DismissAction.Fly:
			StartCoroutine(CallbackUtil.DelayedCallback(1, FlyVessel, v));
			break;
		case KSCVesselMarker.DismissAction.Recover:
			StartCoroutine(CallbackUtil.DelayedCallback(1, RecoverVessel, v));
			break;
		case KSCVesselMarker.DismissAction.None:
			break;
		}
	}

	public void FlyVessel(Vessel v)
	{
		GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE, GameScenes.FLIGHT);
		FlightDriver.StartAndFocusVessel("persistent", FlightGlobals.Vessels.IndexOf(v));
		ClearVesselMarkers();
	}

	public void RecoverVessel(Vessel v)
	{
		ShipConstruction.RecoverVesselFromFlight(v.protoVessel, HighLogic.CurrentGame.flightState);
		GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE, GameScenes.SPACECENTER);
		StartCoroutine(CallbackUtil.DelayedCallback(1, RefreshMarkers));
	}

	public void onVesselDestroy(Vessel v)
	{
		RefreshMarkers();
	}

	public void RefreshMarkers()
	{
		ClearVesselMarkers();
		SpawnVesselMarkers();
	}

	public void OnDestroy()
	{
		GameEvents.onGUILaunchScreenSpawn.Remove(onGUIVesselScreenSpawn);
		GameEvents.onGUIRnDComplexSpawn.Remove(onGUIScreenSpawn);
		GameEvents.onGUIAstronautComplexSpawn.Remove(onGUIScreenSpawn);
		GameEvents.onGUIMissionControlSpawn.Remove(onGUIScreenSpawn);
		GameEvents.onGUILaunchScreenDespawn.Remove(onGUIScreenDespawn);
		GameEvents.onGUIRnDComplexDespawn.Remove(onGUIScreenDespawn);
		GameEvents.onGUIAstronautComplexDespawn.Remove(onGUIACScreenDespawn);
		GameEvents.onGUIMissionControlDespawn.Remove(onGUIScreenDespawn);
		GameEvents.onGameSceneLoadRequested.Remove(onSceneLoadRequested);
		GameEvents.onVesselWillDestroy.Remove(onVesselDestroy);
		if (fetch != null && fetch == this)
		{
			fetch = null;
		}
	}

	public void onGUIVesselScreenSpawn(GameEvents.VesselSpawnInfo info)
	{
		onGUIScreenSpawn();
	}

	public void onGUIScreenSpawn()
	{
		ClearVesselMarkers();
	}

	public void onGUIACScreenDespawn()
	{
		if (HighLogic.LoadedScene == GameScenes.SPACECENTER && (!(VesselSpawnDialog.Instance != null) || !VesselSpawnDialog.Instance.Visible))
		{
			SpawnVesselMarkers();
		}
	}

	public void onGUIScreenDespawn()
	{
		if (HighLogic.LoadedScene == GameScenes.SPACECENTER && markers.Count == 0)
		{
			SpawnVesselMarkers();
		}
	}

	public void onSceneLoadRequested(GameScenes scn)
	{
		ClearVesselMarkers();
		Object.Destroy(this);
	}
}

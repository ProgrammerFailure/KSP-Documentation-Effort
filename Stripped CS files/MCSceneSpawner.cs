using ns2;
using UnityEngine;

public class MCSceneSpawner : MonoBehaviour
{
	public UICanvasPrefab missionControlPrefab;

	public void Awake()
	{
		GameEvents.onGUIMissionControlSpawn.Add(OnMCSpawn);
		GameEvents.onGUIMissionControlDespawn.Add(OnMCDespawn);
	}

	public void OnDestroy()
	{
		GameEvents.onGUIMissionControlSpawn.Remove(OnMCSpawn);
		GameEvents.onGUIMissionControlDespawn.Remove(OnMCDespawn);
	}

	public void OnMCSpawn()
	{
		UIMasterController.Instance.AddCanvas(missionControlPrefab);
		MusicLogic.fetch.PauseWithCrossfade(MusicLogic.AdditionalThemes.MissionControl);
	}

	public void OnMCDespawn()
	{
		GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
		MusicLogic.fetch.UnpauseWithCrossfade();
		UIMasterController.Instance.RemoveCanvas(missionControlPrefab);
	}
}

using ns2;
using UnityEngine;

namespace ns11;

public class AdministrationSceneSpawner : MonoBehaviour
{
	public UICanvasPrefab AdministrationScreenPrefab;

	public void Awake()
	{
		GameEvents.onGUIAdministrationFacilitySpawn.Add(onAdminSpawn);
		GameEvents.onGUIAdministrationFacilityDespawn.Add(onAdminDespawn);
	}

	public void OnDestroy()
	{
		GameEvents.onGUIAdministrationFacilitySpawn.Remove(onAdminSpawn);
		GameEvents.onGUIAdministrationFacilityDespawn.Remove(onAdminDespawn);
	}

	public void onAdminSpawn()
	{
		UIMasterController.Instance.AddCanvas(AdministrationScreenPrefab);
		MusicLogic.fetch.PauseWithCrossfade(MusicLogic.AdditionalThemes.Administration);
	}

	public void onAdminDespawn()
	{
		GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
		Debug.Log(AdministrationScreenPrefab);
		UIMasterController.Instance.RemoveCanvas(AdministrationScreenPrefab);
		MusicLogic.fetch.UnpauseWithCrossfade();
	}
}

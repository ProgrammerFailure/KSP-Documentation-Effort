using ns2;
using UnityEngine;

namespace ns11;

public class ACSceneSpawner : MonoBehaviour
{
	public UICanvasPrefab ACScreenPrefab;

	public void Awake()
	{
		GameEvents.onGUIAstronautComplexSpawn.Add(onACSpawn);
		GameEvents.onGUIAstronautComplexDespawn.Add(onACDespawn);
	}

	public void OnDestroy()
	{
		GameEvents.onGUIAstronautComplexSpawn.Remove(onACSpawn);
		GameEvents.onGUIAstronautComplexDespawn.Remove(onACDespawn);
	}

	public void onACSpawn()
	{
		UIMasterController.Instance.AddCanvas(ACScreenPrefab);
		MusicLogic.fetch.PauseWithCrossfade(MusicLogic.AdditionalThemes.AstronautComplex);
	}

	public void onACDespawn()
	{
		GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
		UIMasterController.Instance.RemoveCanvas(ACScreenPrefab);
		MusicLogic.fetch.UnpauseWithCrossfade();
	}
}

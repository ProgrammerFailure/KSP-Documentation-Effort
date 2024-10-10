using ns2;
using UnityEngine;
using UnityEngine.Rendering;

namespace ns11;

public class RDSceneSpawner : MonoBehaviour
{
	public UICanvasPrefab RDScreenPrefab;

	public Cubemap RDSceneReflection;

	public DefaultReflectionMode oldReflectionMode;

	public Cubemap oldReflection;

	public void Awake()
	{
		GameEvents.onGUIRnDComplexSpawn.Add(onRDSpawn);
		GameEvents.onGUIRnDComplexDespawn.Add(onRDDespawn);
	}

	public void OnDestroy()
	{
		GameEvents.onGUIRnDComplexSpawn.Remove(onRDSpawn);
		GameEvents.onGUIRnDComplexDespawn.Remove(onRDDespawn);
	}

	public void onRDSpawn()
	{
		UIMasterController.Instance.AddCanvas(RDScreenPrefab);
		oldReflectionMode = RenderSettings.defaultReflectionMode;
		RenderSettings.defaultReflectionMode = DefaultReflectionMode.Custom;
		oldReflection = RenderSettings.customReflection;
		RenderSettings.customReflection = RDSceneReflection;
		MusicLogic.fetch.PauseWithCrossfade(MusicLogic.AdditionalThemes.ResearchAndDevelopment);
	}

	public void onRDDespawn()
	{
		GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
		UIMasterController.Instance.RemoveCanvas(RDScreenPrefab);
		RenderSettings.defaultReflectionMode = oldReflectionMode;
		RenderSettings.customReflection = oldReflection;
		MusicLogic.fetch.UnpauseWithCrossfade();
	}
}

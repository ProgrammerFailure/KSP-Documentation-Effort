using UnityEngine;

namespace TestScripts;

public class HighLogicLoadedSceneSelector : MonoBehaviour
{
	public GameScenes LoadedSceneIs;

	public GameScenes lastLoadedScene = GameScenes.LOADINGBUFFER;

	public int initDelay;

	public void Update()
	{
		if (lastLoadedScene != LoadedSceneIs && Time.frameCount > initDelay)
		{
			HighLogic.LoadedSceneIsFlight = LoadedSceneIs == GameScenes.FLIGHT;
			HighLogic.LoadedSceneIsEditor = LoadedSceneIs == GameScenes.EDITOR;
			HighLogic.LoadedSceneHasPlanetarium = LoadedSceneIs == GameScenes.FLIGHT || LoadedSceneIs == GameScenes.TRACKSTATION || LoadedSceneIs == GameScenes.SPACECENTER;
			HighLogic.LoadedSceneIsGame = LoadedSceneIs == GameScenes.FLIGHT || LoadedSceneIs == GameScenes.TRACKSTATION || LoadedSceneIs == GameScenes.SPACECENTER || LoadedSceneIs == GameScenes.EDITOR;
			HighLogic.LoadedSceneIsMissionBuilder = LoadedSceneIs == GameScenes.MISSIONBUILDER;
			HighLogic.LoadedScene = LoadedSceneIs;
			lastLoadedScene = LoadedSceneIs;
		}
	}
}

using UnityEngine;

public class AudioMuter : MonoBehaviour
{
	public AudioSource source;

	public void Awake()
	{
		GameEvents.onGameSceneLoadRequested.Add(OnGameSceneLoadRequested);
		GameEvents.onLevelWasLoaded.Add(OnLevelLoaded);
	}

	public void OnDestroy()
	{
		GameEvents.onGameSceneLoadRequested.Remove(OnGameSceneLoadRequested);
		GameEvents.onLevelWasLoaded.Remove(OnLevelLoaded);
	}

	public void OnGameSceneLoadRequested(GameScenes scene)
	{
		source.mute = true;
	}

	public void OnLevelLoaded(GameScenes scene)
	{
		if (scene != GameScenes.MISSIONBUILDER)
		{
			source.mute = false;
		}
	}
}

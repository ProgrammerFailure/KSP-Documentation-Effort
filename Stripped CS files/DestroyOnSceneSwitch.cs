using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyOnSceneSwitch : MonoBehaviour
{
	public void Start()
	{
		GameEvents.onGameSceneLoadRequested.Add(OnSceneSwitch);
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public void OnDestroy()
	{
		GameEvents.onGameSceneLoadRequested.Remove(OnSceneSwitch);
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	public void OnSceneSwitch(GameScenes scene)
	{
		Object.DestroyImmediate(base.gameObject);
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		OnLevelLoaded();
	}

	public void OnLevelLoaded()
	{
		Object.DestroyImmediate(base.gameObject);
	}
}

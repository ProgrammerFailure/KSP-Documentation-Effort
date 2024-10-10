using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetTransformOnSceneSwitch : MonoBehaviour
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
		base.transform.localRotation = Quaternion.identity;
		base.transform.localPosition = Vector3.zero;
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		OnLevelLoaded();
	}

	public void OnLevelLoaded()
	{
		base.transform.localRotation = Quaternion.identity;
		base.transform.localPosition = Vector3.zero;
	}
}

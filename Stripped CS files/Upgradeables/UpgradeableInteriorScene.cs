using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Upgradeables;

public class UpgradeableInteriorScene : UpgradeableInterior
{
	[SerializeField]
	public string[] scenes;

	[SerializeField]
	public Transform spawnParent;

	public GameObject loadedSceneObject;

	[SerializeField]
	public string[] Scenes => scenes;

	public override void UpdateLevel(float normLvl)
	{
		FacilityLevel = Mathf.FloorToInt(normLvl * (float)scenes.Length);
		FacilityLevel = Mathf.Clamp(FacilityLevel, 0, scenes.Length - 1);
		SetLevel(FacilityLevel);
	}

	public override void SetLevel(int level)
	{
		if (loadedSceneObject != null)
		{
			Object.Destroy(loadedSceneObject);
		}
		StartCoroutine(loadInteriorScene(scenes[level]));
		FacilityLevel = level;
	}

	public IEnumerator loadInteriorScene(string sceneName)
	{
		yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		loadedSceneObject = GameObject.Find(sceneName);
		loadedSceneObject.transform.parent = spawnParent;
	}

	public override int GetLevelCount()
	{
		return scenes.Length;
	}
}

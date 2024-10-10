using UnityEngine;
using UnityEngine.SceneManagement;

namespace TestScripts;

public class FacilitySceneryLoader : MonoBehaviour
{
	public EditorFacility facilityScenery;

	public EditorFacility loadedScenery;

	public void Start()
	{
		UnloadScenery();
		LoadScenery(facilityScenery);
	}

	public void LoadScenery(EditorFacility facility)
	{
		switch (facilityScenery)
		{
		case EditorFacility.const_1:
			SceneManager.LoadScene("VABscenery", LoadSceneMode.Additive);
			break;
		case EditorFacility.const_2:
			SceneManager.LoadScene("SPHscenery", LoadSceneMode.Additive);
			break;
		}
		loadedScenery = facility;
	}

	public void UnloadScenery()
	{
		switch (loadedScenery)
		{
		case EditorFacility.const_1:
			Object.DestroyImmediate(GameObject.Find("VABscenery"));
			break;
		case EditorFacility.const_2:
			Object.DestroyImmediate(GameObject.Find("SPHscenery"));
			break;
		}
		loadedScenery = EditorFacility.None;
	}
}

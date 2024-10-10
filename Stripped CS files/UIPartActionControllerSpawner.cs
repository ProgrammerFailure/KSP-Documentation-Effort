using UnityEngine;

public class UIPartActionControllerSpawner : MonoBehaviour
{
	public UIPartActionController controllerPrefab;

	public void Start()
	{
		Object.Instantiate(controllerPrefab);
	}
}

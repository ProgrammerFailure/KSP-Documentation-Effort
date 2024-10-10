using UnityEngine;
using UnityEngine.UI;

public class MCDespawner : MonoBehaviour
{
	public Button exitButton;

	public void Start()
	{
		exitButton.onClick.AddListener(BtnExit);
	}

	public void BtnExit()
	{
		GameEvents.onGUIMissionControlDespawn.Fire();
		Object.Destroy(base.gameObject);
	}
}

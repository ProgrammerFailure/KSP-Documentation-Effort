using UnityEngine;
using UnityEngine.UI;

namespace ns11;

public class ACSceneDespawner : MonoBehaviour
{
	public Button exitButton;

	public void Start()
	{
		exitButton.onClick.AddListener(BtnExit);
	}

	public void BtnExit()
	{
		GameEvents.onGUIAstronautComplexDespawn.Fire();
	}
}

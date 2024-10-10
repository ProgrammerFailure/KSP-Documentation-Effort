using UnityEngine;
using UnityEngine.UI;

namespace ns11;

public class AdministrationSceneDespawner : MonoBehaviour
{
	public Button exitButton;

	public void Start()
	{
		exitButton.onClick.AddListener(BtnExit);
	}

	public void BtnExit()
	{
		GameEvents.onGUIAdministrationFacilityDespawn.Fire();
	}
}

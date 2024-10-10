using UnityEngine;

public class CreditsSceneMain : MonoBehaviour
{
	public void returnToMainMenu()
	{
		HighLogic.LoadScene(GameScenes.MAINMENU);
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			returnToMainMenu();
		}
	}
}

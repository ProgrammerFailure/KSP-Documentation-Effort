using UnityEngine;
using UnityEngine.SceneManagement;

namespace EdyCommonTools;

public class SceneReload : MonoBehaviour
{
	public KeyCode hotkey = KeyCode.R;

	public void Update()
	{
		if (Input.GetKeyDown(hotkey))
		{
			Reload();
		}
	}

	public static void Reload()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
	}
}

using UnityEngine;

namespace EdyCommonTools;

public class ApplicationQuit : MonoBehaviour
{
	public bool desktop = true;

	public bool mobile = true;

	public KeyCode quitKey = KeyCode.Escape;

	public bool m_isMobile;

	public void OnEnable()
	{
		m_isMobile = Application.isMobilePlatform;
	}

	public void Update()
	{
		if (((desktop && !m_isMobile) || (mobile && m_isMobile)) && Input.GetKeyDown(quitKey))
		{
			Application.Quit();
		}
	}
}

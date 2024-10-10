using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EdyCommonTools;

[Obsolete("Deprecated. Use TimeScale and/or SceneReload instead")]
public class SceneTools : MonoBehaviour
{
	public bool slowTimeMode;

	public float slowTime = 0.3f;

	public KeyCode hotkeyReset = KeyCode.R;

	public KeyCode hotkeyTime = KeyCode.T;

	public bool m_prevSlowTimeMode;

	public void OnEnable()
	{
		m_prevSlowTimeMode = !slowTimeMode;
	}

	public void Update()
	{
		if (Input.GetKeyDown(hotkeyReset))
		{
			SceneManager.LoadScene(0, LoadSceneMode.Single);
		}
		if (Input.GetKeyDown(hotkeyTime))
		{
			slowTimeMode = !slowTimeMode;
		}
		if (slowTimeMode != m_prevSlowTimeMode)
		{
			Time.timeScale = (slowTimeMode ? slowTime : 1f);
			m_prevSlowTimeMode = slowTimeMode;
		}
	}
}

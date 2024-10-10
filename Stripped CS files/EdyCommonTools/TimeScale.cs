using UnityEngine;

namespace EdyCommonTools;

public class TimeScale : MonoBehaviour
{
	public bool timeScaleEnabled;

	public float timeScale = 0.3f;

	public KeyCode hotkey = KeyCode.T;

	public bool m_prevTimeScaleEnabled;

	public void OnEnable()
	{
		m_prevTimeScaleEnabled = !timeScaleEnabled;
	}

	public void Update()
	{
		if (Input.GetKeyDown(hotkey))
		{
			timeScaleEnabled = !timeScaleEnabled;
		}
		if (timeScaleEnabled != m_prevTimeScaleEnabled)
		{
			Time.timeScale = (timeScaleEnabled ? timeScale : 1f);
			m_prevTimeScaleEnabled = timeScaleEnabled;
		}
	}
}

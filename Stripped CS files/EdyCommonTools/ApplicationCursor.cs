using UnityEngine;

namespace EdyCommonTools;

public class ApplicationCursor : MonoBehaviour
{
	[Header("On Enable")]
	public bool showCursor = true;

	public bool lockCursor;

	public bool dontChangeInEditor = true;

	[Header("On Update")]
	public bool autoHide;

	public float autoHideTimeout = 5f;

	public float speedThreshold = 200f;

	public GameObject[] skipIfActive;

	[Header("On Disable")]
	public bool restoreHiddenCursor = true;

	public Vector3 m_lastMousePosition;

	public float m_lastMovementTime;

	public void OnEnable()
	{
		if (!Application.isEditor || !dontChangeInEditor)
		{
			Cursor.visible = showCursor;
			Cursor.lockState = (lockCursor ? CursorLockMode.Locked : CursorLockMode.None);
		}
		m_lastMousePosition = Input.mousePosition;
		m_lastMovementTime = 0f;
	}

	public void Update()
	{
		if (!autoHide || !Input.mousePresent)
		{
			return;
		}
		bool flag = ((Vector2)((Input.mousePosition - m_lastMousePosition) / Time.unscaledDeltaTime)).magnitude > speedThreshold || Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2);
		GameObject[] array = skipIfActive;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].activeInHierarchy)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			m_lastMovementTime = Time.unscaledTime;
			Cursor.visible = true;
		}
		if (Cursor.visible && Time.unscaledTime - m_lastMovementTime > autoHideTimeout)
		{
			Cursor.visible = false;
		}
		m_lastMousePosition = Input.mousePosition;
	}

	public void OnDisable()
	{
		if (restoreHiddenCursor)
		{
			Cursor.visible = true;
		}
	}
}

using UnityEngine;

namespace EdyCommonTools;

[RequireComponent(typeof(CameraFovController))]
public class CameraFovInput : MonoBehaviour
{
	public enum InputSource
	{
		StandardInput,
		Messages
	}

	public enum MoveParameter
	{
		Angle,
		Size,
		SizeProportional
	}

	public InputSource source;

	public string axisName = "Mouse ScrollWheel";

	public MoveParameter parameter;

	public float angleSensitivity = 8f;

	public float sizeSensitivity = 0.5f;

	public float angleDefault = 50f;

	public float sizeDefault = 1f;

	public CameraFovController m_fov;

	public void OnEnable()
	{
		m_fov = GetComponent<CameraFovController>();
	}

	public void Move(float delta)
	{
		if (base.enabled)
		{
			delta = 0f - delta;
			switch (parameter)
			{
			case MoveParameter.Angle:
				m_fov.fieldOfView += delta * angleSensitivity;
				break;
			case MoveParameter.Size:
				m_fov.targetSize += delta * sizeSensitivity;
				break;
			case MoveParameter.SizeProportional:
				m_fov.targetSize *= 1f + delta * sizeSensitivity;
				break;
			}
		}
	}

	public void ResetDefaults()
	{
		if (base.enabled)
		{
			switch (parameter)
			{
			case MoveParameter.Size:
			case MoveParameter.SizeProportional:
				m_fov.targetSize = sizeDefault;
				break;
			case MoveParameter.Angle:
				m_fov.fieldOfView = angleDefault;
				break;
			}
		}
	}

	public void ProcessStandardInput()
	{
		float axis = Input.GetAxis(axisName);
		Move(axis);
	}

	public void Update()
	{
		if (source == InputSource.StandardInput)
		{
			ProcessStandardInput();
		}
	}

	public void OnScroll(float delta)
	{
		if (source == InputSource.Messages)
		{
			Move(delta / (float)Screen.height);
		}
	}

	public void Scroll(float delta)
	{
		Move(delta / (float)Screen.height);
	}
}

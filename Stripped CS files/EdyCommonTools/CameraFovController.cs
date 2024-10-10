using UnityEngine;

namespace EdyCommonTools;

[RequireComponent(typeof(Camera))]
public class CameraFovController : MonoBehaviour
{
	public enum Mode
	{
		Free,
		AdjustToTarget,
		AdjustSizeToTargetDistance
	}

	public Transform target;

	public float fieldOfView = 60f;

	public Mode mode;

	public float targetSize = 1f;

	public float targetSizeOffset;

	public bool damped = true;

	public float damping = 4f;

	public bool clampedFov;

	public float minFov = 10f;

	public float maxFov = 75f;

	public bool clampedSize;

	public float minSize = 0.1f;

	public float maxSize = 2f;

	public Camera m_cam;

	public Transform m_trans;

	public float m_currentFov;

	public bool m_firstRun;

	public Transform m_cachedTarget;

	public Renderer m_targetRenderer;

	public void ResetFieldOfView(float fovAngle)
	{
		fieldOfView = fovAngle;
		m_currentFov = fovAngle;
	}

	public void OnEnable()
	{
		m_cam = GetComponent<Camera>();
		m_trans = GetComponent<Transform>();
		if (mode == Mode.Free)
		{
			fieldOfView = m_cam.fieldOfView;
			m_currentFov = fieldOfView;
		}
		m_firstRun = true;
	}

	public float GetFovAngleBySize(float size, float distance)
	{
		if (clampedSize)
		{
			size = Mathf.Clamp(size, minSize, maxSize);
		}
		return Mathf.Atan2(size, distance) * 2f * 57.29578f;
	}

	public void LateUpdate()
	{
		if (target != m_cachedTarget)
		{
			m_targetRenderer = ((target != null) ? target.GetComponent<Renderer>() : null);
			m_cachedTarget = target;
		}
		if (clampedFov)
		{
			fieldOfView = Mathf.Clamp(fieldOfView, minFov, maxFov);
		}
		if (clampedSize)
		{
			targetSize = Mathf.Clamp(targetSize, minSize, maxSize);
		}
		float num = fieldOfView;
		if (target != null && mode != 0)
		{
			if (mode == Mode.AdjustToTarget)
			{
				if (m_targetRenderer != null)
				{
					float size = m_targetRenderer.bounds.extents.magnitude + targetSizeOffset;
					float magnitude = (m_targetRenderer.bounds.center - m_trans.position).magnitude;
					num = GetFovAngleBySize(size, magnitude);
				}
			}
			else if (mode == Mode.AdjustSizeToTargetDistance)
			{
				float size2 = targetSize + targetSizeOffset;
				float magnitude2 = (target.position - m_trans.position).magnitude;
				num = GetFovAngleBySize(size2, magnitude2);
			}
			if (clampedFov)
			{
				num = Mathf.Clamp(num, minFov, maxFov);
			}
			if (m_firstRun)
			{
				m_currentFov = num;
				m_firstRun = false;
			}
		}
		num = Mathf.Clamp(num, 0.05f, 170f);
		m_currentFov = (damped ? Mathf.Lerp(m_currentFov, num, damping * Time.deltaTime) : num);
		m_cam.fieldOfView = m_currentFov;
	}
}

using UnityEngine;

namespace EdyCommonTools;

public class PositionController : MonoBehaviour
{
	public enum Mode
	{
		Free,
		FixedToTarget
	}

	public delegate void OnPositionFinished();

	public Transform target;

	public Vector3 position;

	public Mode mode;

	public Vector3 targetOffset;

	public bool damped = true;

	public float damping = 8f;

	public bool clamped;

	public Bounds limits = new Bounds(Vector3.zero, Vector3.one * 10f);

	public OnPositionFinished onPositionFinished;

	public Transform m_trans;

	public Vector3 m_currentPosition;

	public bool m_positioning;

	public void ResetPosition(Vector3 newPosition)
	{
		position = newPosition;
		m_currentPosition = newPosition;
	}

	public void OnEnable()
	{
		m_trans = GetComponent<Transform>();
		if (mode == Mode.Free)
		{
			position = m_trans.localPosition;
			m_currentPosition = position;
		}
		else
		{
			m_currentPosition = m_trans.localPosition;
		}
		m_positioning = false;
	}

	public void ClampPosition(ref Vector3 pos)
	{
		if (clamped)
		{
			Vector3 min = limits.min;
			Vector3 max = limits.max;
			pos.x = Mathf.Clamp(pos.x, min.x, max.x);
			pos.y = Mathf.Clamp(pos.y, min.y, max.y);
			pos.z = Mathf.Clamp(pos.z, min.z, max.z);
		}
	}

	public void LateUpdate()
	{
		Transform parent = m_trans.parent;
		ClampPosition(ref position);
		Vector3 pos = position;
		if (target != null && mode == Mode.FixedToTarget)
		{
			pos = ((!(parent != null)) ? (target.position + targetOffset) : (parent.InverseTransformPoint(target.position) + targetOffset));
			ClampPosition(ref pos);
		}
		m_currentPosition = (damped ? Vector3.Lerp(m_currentPosition, pos, damping * Time.deltaTime) : pos);
		m_trans.localPosition = m_currentPosition;
		if ((m_currentPosition - pos).magnitude > 0.001f)
		{
			m_positioning = true;
		}
		else if (m_positioning)
		{
			m_positioning = false;
			if (onPositionFinished != null)
			{
				onPositionFinished();
			}
		}
	}
}

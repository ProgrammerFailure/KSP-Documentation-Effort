using System.Runtime.CompilerServices;
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

	public bool damped;

	public float damping;

	public bool clamped;

	public Bounds limits;

	public OnPositionFinished onPositionFinished;

	private Transform m_trans;

	private Vector3 m_currentPosition;

	private bool m_positioning;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PositionController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetPosition(Vector3 newPosition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClampPosition(ref Vector3 pos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}
}

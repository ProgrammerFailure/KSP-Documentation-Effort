using System.Runtime.CompilerServices;
using UnityEngine;

namespace EdyCommonTools;

public class ScaleController : MonoBehaviour
{
	public delegate void OnScaleFinished();

	public Vector3 scale;

	public bool unified;

	public bool damped;

	public float damping;

	public bool clamped;

	public Vector3 min;

	public Vector3 max;

	public OnScaleFinished onScaleFinished;

	private Transform m_trans;

	private Vector3 m_currentScale;

	private bool m_scaling;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScaleController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetScale(Vector3 newScale)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClampScale(ref Vector3 s)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}
}

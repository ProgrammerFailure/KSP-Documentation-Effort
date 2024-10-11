using System.Runtime.CompilerServices;
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

	public float fieldOfView;

	public Mode mode;

	public float targetSize;

	public float targetSizeOffset;

	public bool damped;

	public float damping;

	public bool clampedFov;

	public float minFov;

	public float maxFov;

	public bool clampedSize;

	public float minSize;

	public float maxSize;

	private Camera m_cam;

	private Transform m_trans;

	private float m_currentFov;

	private bool m_firstRun;

	private Transform m_cachedTarget;

	private Renderer m_targetRenderer;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CameraFovController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetFieldOfView(float fovAngle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetFovAngleBySize(float size, float distance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}
}

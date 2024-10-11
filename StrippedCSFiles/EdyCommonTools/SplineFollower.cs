using System.Runtime.CompilerServices;
using UnityEngine;

namespace EdyCommonTools;

[ExecuteInEditMode]
public class SplineFollower : MonoBehaviour
{
	public enum AutoMove
	{
		Off,
		Speed,
		AheadOfTarget
	}

	public Transform target;

	public Spline spline;

	[Header("Follow")]
	public Spline.WrapMode wrapMode;

	public bool followRotation;

	[Space(5f)]
	public float position;

	[Space(5f)]
	public AutoMove autoMove;

	public float speed;

	public Transform aheadOfTarget;

	public float maxAheadSpeed;

	public float minAheadDistance;

	public float maxAheadDistance;

	public bool aheadInPlaneXZ;

	[Header("Display")]
	public bool anchor;

	public bool tangent;

	public bool normal;

	public bool binormal;

	[Space(5f)]
	public bool aheadOfTargetRanges;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SplineFollower()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDrawGizmos()
	{
		throw null;
	}
}

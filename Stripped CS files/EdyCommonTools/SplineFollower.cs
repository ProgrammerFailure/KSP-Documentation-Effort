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

	public bool followRotation = true;

	[Space(5f)]
	public float position;

	[Space(5f)]
	public AutoMove autoMove;

	public float speed = 1f;

	public Transform aheadOfTarget;

	public float maxAheadSpeed = 30f;

	public float minAheadDistance = 5f;

	public float maxAheadDistance = 100f;

	public bool aheadInPlaneXZ = true;

	[Header("Display")]
	public bool anchor = true;

	public bool tangent = true;

	public bool normal;

	public bool binormal;

	[Space(5f)]
	public bool aheadOfTargetRanges = true;

	public void OnEnable()
	{
		if (target == null)
		{
			target = base.transform;
		}
	}

	public void Update()
	{
		if (target == null || spline == null || !spline.isActiveAndEnabled)
		{
			return;
		}
		if (Application.isPlaying && autoMove != 0)
		{
			if (autoMove == AutoMove.Speed)
			{
				position += speed * Time.deltaTime;
			}
			else if (autoMove == AutoMove.AheadOfTarget && aheadOfTarget != null)
			{
				Vector3 a = target.position;
				Vector3 b = aheadOfTarget.position;
				if (aheadInPlaneXZ)
				{
					a.y = 0f;
					b.y = 0f;
				}
				float value = Vector3.Distance(a, b);
				speed = maxAheadSpeed * Mathf.InverseLerp(maxAheadDistance, minAheadDistance, value);
				position += speed * Time.deltaTime;
			}
		}
		if (followRotation)
		{
			target.position = spline.GetPosition(position, out var forward, out var upwards, Vector3.up, wrapMode);
			target.rotation = Quaternion.LookRotation(forward, upwards);
		}
		else
		{
			target.position = spline.GetPosition(position, wrapMode);
		}
	}

	public void OnDrawGizmos()
	{
		if (!(target == null) && !(spline == null) && base.isActiveAndEnabled && anchor)
		{
			DebugUtility.SphereGizmo(target, GColor.accentGreen);
			Gizmos.color = GColor.white;
			DebugUtility.CrossMarkGizmo(target.position, target);
		}
	}
}

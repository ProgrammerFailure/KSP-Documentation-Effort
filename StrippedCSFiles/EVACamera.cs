using System.Runtime.CompilerServices;
using UnityEngine;

public class EVACamera : MonoBehaviour
{
	public float minPitch;

	public float maxPitch;

	public float startDistance;

	public float maxDistance;

	public float minDistance;

	private float distance;

	private GameObject pivot;

	private Vector3 endPos;

	private Quaternion endRot;

	public float orbitSensitivity;

	public float mouseZoomSensitivity;

	public float sharpness;

	public float camPitch;

	public float camHdg;

	private float tgtHdg;

	public Vessel target;

	public float Distance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Quaternion pivotRotation
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3 pivotPosition
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EVACamera()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}
}

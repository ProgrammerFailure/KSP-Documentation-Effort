using System.Runtime.CompilerServices;
using UnityEngine;

public class TrackRigObject : MonoBehaviour
{
	public enum TrackMode
	{
		FixedUpdate,
		Update,
		LateUpdate
	}

	public Transform target;

	public bool keepInitialOffset;

	private bool initialized;

	public TrackMode trackingMode;

	private Transform trf;

	private Vector3 pOff;

	private Quaternion rOff;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TrackRigObject()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Initialize")]
	private void Initialize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Track()
	{
		throw null;
	}
}

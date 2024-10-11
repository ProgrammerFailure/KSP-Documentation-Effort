using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class IVACamera : IGameCamera
{
	public float minZoom;

	public float maxZoom;

	public float minPitch;

	public float maxPitch;

	public float maxRot;

	public float distanceCenter;

	public float distanceMaxRot;

	private float orbitFactor;

	private float currentZoom;

	private float currentPitch;

	private float currentRot;

	private float initialZoom;

	private Vector3 initialPosition;

	private Quaternion initialRotation;

	private Vector3 currentPosition;

	private Quaternion currentRotation;

	private float orbitSensitivity;

	private float mouseZoomSensitivity;

	[HideInInspector]
	public Kerbal kerbal;

	private Camera _camera;

	public bool isActive
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public IVACamera()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResetState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void DeactivateAll()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Activate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Deactivate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void ResetCamera()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void EnableCamera()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void DisableCamera()
	{
		throw null;
	}
}

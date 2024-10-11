using System.Runtime.CompilerServices;
using UnityEngine;

namespace TMPro.Examples;

public class CameraController : MonoBehaviour
{
	public enum CameraModes
	{
		Follow,
		Isometric,
		Free
	}

	private Transform cameraTransform;

	private Transform dummyTarget;

	public Transform CameraTarget;

	public float FollowDistance;

	public float MaxFollowDistance;

	public float MinFollowDistance;

	public float ElevationAngle;

	public float MaxElevationAngle;

	public float MinElevationAngle;

	public float OrbitalAngle;

	public CameraModes CameraMode;

	public bool MovementSmoothing;

	public bool RotationSmoothing;

	private bool previousSmoothing;

	public float MovementSmoothingValue;

	public float RotationSmoothingValue;

	public float MoveSensitivity;

	private Vector3 currentVelocity;

	private Vector3 desiredPosition;

	private float mouseX;

	private float mouseY;

	private Vector3 moveVector;

	private float mouseWheel;

	private const string event_SmoothingValue = "Slider - Smoothing Value";

	private const string event_FollowDistance = "Slider - Camera Zoom";

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CameraController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GetPlayerInput()
	{
		throw null;
	}
}

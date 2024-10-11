using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpaceCenterCamera2 : MonoBehaviour, IKSPCamera
{
	public enum CameraMode
	{
		Rover,
		Overview
	}

	public string pqsName;

	public string initialPositionTransformName;

	public CameraMode mode;

	public KeyCode keyModeSwitch;

	public KeyCode keyReset;

	public bool useAlphaNumKeysFactor;

	public float rotationInitial;

	public float rotateSpeed;

	public float mouseSpeed;

	public float movementRadius;

	public float elevationSpeed;

	public float elevationInitial;

	public float elevationMin;

	public float elevationMax;

	public float zoomSpeed;

	public float zoomInitial;

	public float zoomMin;

	public float zoomMax;

	private float altitudeMin;

	private float altitudeMax;

	public float altitudeInitial;

	private PQS pqs;

	private Transform initialPosition;

	private Transform cameraTransform;

	private Transform t;

	private SurfaceObject srfPivot;

	private bool isActive;

	private float speed;

	private float mousePitch;

	private float zoom;

	private float altitude;

	private float rotationAngle;

	private float elevationAngle;

	private float height;

	private Vector3 pos;

	private float tIRpitch;

	private float tIRyaw;

	private float tIRroll;

	private Transform camPivot;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SpaceCenterCamera2()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneSwitch(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SwitchMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetCamera()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetZoom()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InputSpeedFactor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InputMovement()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InputCamera()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetSpeed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Pitch(float amount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Altitude(float amount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Rotate(float amount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Elevate(float amount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Zoom(float amount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Translate(Vector3 movement)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateTransform()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateTransformRover()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateTransformOverview()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Transform GetPivot()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Transform GetCameraTransform()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetCamCoordsFromPosition(Vector3 wPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Func<bool> OnNavigatorTakeOver(Callback onRequestControl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnNavigatorHandoff()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Quaternion getReferenceFrame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float getPitch()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float getYaw()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool OnNavigatorRequestControl()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[SpecialName]
	bool IKSPCamera.get_enabled()
	{
		throw null;
	}
}

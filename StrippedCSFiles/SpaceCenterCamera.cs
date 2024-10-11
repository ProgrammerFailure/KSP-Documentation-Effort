using System.Runtime.CompilerServices;
using UnityEngine;

public class SpaceCenterCamera : MonoBehaviour
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

	public double rotationInitial;

	public double rotateSpeed;

	public float mouseSpeed;

	public float elevationSpeed;

	public float elevationInitial;

	public float elevationMin;

	public float elevationMax;

	public float zoomSpeed;

	public float zoomInitial;

	public float zoomMin;

	public float zoomMax;

	private double altitudeMin;

	private double altitudeMax;

	public double altitudeInitial;

	private PQS pqs;

	private Transform initialPosition;

	private Transform cameraTransform;

	private double speed;

	private float mousePitch;

	private float zoom;

	private double altitude;

	private double rotationAngle;

	private float elevationAngle;

	private double radius;

	private Vector3d direction;

	private QuaternionD rotation;

	private Vector3d realPos;

	private static double Deg2Rad;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SpaceCenterCamera()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static SpaceCenterCamera()
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
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPosition(Vector3 position)
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
	private double GetSpeed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Pitch(float amount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Altitude(double amount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Rotate(double amount)
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
	private void Translate(double x, double y)
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
}

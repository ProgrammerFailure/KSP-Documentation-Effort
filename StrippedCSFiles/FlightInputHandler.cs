using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FlightInputHandler : MonoBehaviour
{
	public static FlightCtrlState state;

	[Obsolete("Use vessel.OnFlyByWire instead.")]
	public static FlightInputCallback OnFlyByWire;

	public static FlightInputCallback OnRawAxisInput;

	public float throttleResponsiveness;

	public float rcsDeadZone;

	private float throttle;

	private float axisThrottle;

	private float lastAxisThrottle;

	private float precisionPitch;

	private float precisionRoll;

	private float precisionYaw;

	private float precisionX;

	private float precisionY;

	private float precisionZ;

	private float precisionWheelSteer;

	private float precisionWheelThrottle;

	private float[] precision_custom_axis;

	public static FlightInputHandler fetch;

	public static int currentTarget;

	private uint controlLockMask;

	public bool stageLock;

	public bool rcslock;

	public bool precisionMode;

	public bool hasFocus;

	public static bool SPACENAV_USE_AS_FLIGHT_CONTROL;

	private bool hasSpaceNavDevice;

	private bool linRotSwitchHold;

	private bool throttleFocus;

	private bool refocusThrottle;

	public static bool RCSLock
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FlightInputHandler()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static FlightInputHandler()
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
	private void OnVesselOverrideGroupChanged(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetNeutralControls()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ResumeVesselCtrlState(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetLaunchCtrlState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnApplicationFocus(bool focus)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ProcessAxis(AxisBinding axisBinding, KeyBinding plusKeyBinding, KeyBinding minusKeyBinding, ref float axisValue, ref float precisionAxis)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}
}

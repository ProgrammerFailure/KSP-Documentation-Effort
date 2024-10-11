using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetariumCamera : MonoBehaviour, IKSPCamera
{
	private static PlanetariumCamera _fetch;

	public float minPitch;

	public float maxPitch;

	public float startDistance;

	public float maxDistance;

	public float minDistance;

	public float orbitSensitivity;

	public float zoomScaleFactor;

	public float sharpness;

	public float camPitch;

	public float camHdg;

	public MapObject initialTarget;

	public MapObject target;

	public List<MapObject> targets;

	public bool TabSwitchTargets;

	private static Camera camRef;

	private ScreenMessage cameraTgtReadout;

	private float minRadiusDistance;

	private float translateSmooth;

	private float targetHeading;

	private float endHeading;

	private Vector3 cameraVel;

	private float distance;

	private Vector3 endPos;

	private Quaternion testRot;

	private Quaternion endRot;

	private Transform pivot;

	private AudioListener listener;

	private bool externalControl;

	private float tIRpitch;

	private float tIRyaw;

	private float tIRroll;

	private double timeSceneLoadRequested;

	public Callback AbortExternalControl;

	[HideInInspector]
	private CelestialBody b;

	[HideInInspector]
	private double nearest;

	private CelestialBody nearestBody;

	public static PlanetariumCamera fetch
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

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

	public static Camera Camera
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PlanetariumCamera()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static PlanetariumCamera()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreatePivot()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneChange(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResetPivot()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLevelLoaded(GameScenes level)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselDestroy(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onVesselSwitching(Vessel from, Vessel to)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int AddTarget(MapObject tgt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveTarget(MapObject tgt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int SetTarget(CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int SetTarget(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int SetTarget(int tgtIdx)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetTarget(MapObject tgt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MapObject FindNearestTarget()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MapObject FindNearestTarget(Vector3 position)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetDistance(float dist)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetTargetIndex(string targetName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MapObject GetTarget(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Activate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Activate(bool enableAudioListener)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Deactivate()
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
	public bool OnNavigatorRequestControl()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Func<bool> OnNavigatorTakeOver(Callback RequestControl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool updateAbortCondition()
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
	private void updateDistance()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private CelestialBody getNearestBody(Vector3d cameraLocalSpacePos)
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

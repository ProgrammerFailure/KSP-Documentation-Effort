using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class SpaceNavigatorCamera : MonoBehaviour
{
	public bool HasInputControl;

	protected Func<bool> cameraWantsControl;

	protected IKSPCamera kspCam;

	protected List<IKSPCamera> cams;

	public float translateSpeed;

	[SerializeField]
	protected float sharpnessLin;

	[SerializeField]
	protected float sharpnessRot;

	[SerializeField]
	protected float sensLin;

	[SerializeField]
	protected float sensRot;

	public bool collideWithStuff;

	public float standoffRadius;

	private RaycastHit hit;

	public bool UseMaxRadius;

	public float MaxRadius;

	public bool latchToDisabledCamera;

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected SpaceNavigatorCamera()
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
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLevelLoaded(GameScenes lvl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnCameraRequestControl()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Vector3 getNextTranslation(Transform cam, Vector3 nextStep, float epsilon = 0.0001f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Vector3 clampToRadius(Vector3 rPos)
	{
		throw null;
	}

	public abstract void OnGetControl();

	public abstract void OnCameraUpdate();

	public abstract void OnCameraWantsControl();
}

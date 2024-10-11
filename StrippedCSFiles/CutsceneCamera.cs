using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CameraKeyFrameEvents;
using UnityEngine;

public class CutsceneCamera : MonoBehaviour
{
	[Serializable]
	public class CameraKeyFrame
	{
		public enum Easing
		{
			Linear,
			SinLerp,
			CosLerp,
			Hermite,
			Berp
		}

		public Transform refTrf;

		public float duration;

		public int index;

		[HideInInspector]
		public CameraKeyFrame nextFrm;

		public Easing EaseToNextFrameMode;

		public List<CameraKeyFrameEvent> frameEvents;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public CameraKeyFrame()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Vector3 EvalPos(float normT)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Quaternion EvalRot(float normT)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void EvalEvents(float normT)
		{
			throw null;
		}
	}

	public float totalDuration;

	public int i;

	public List<CameraKeyFrame> keyFrames;

	private float T;

	private float normT;

	private float lastFrameT;

	private CameraKeyFrame currentFrame;

	private int cFrameIndex;

	private float speedFactor;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CutsceneCamera()
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
	[ContextMenu("Add Frame Here")]
	public void CreateKeyFrame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int kfCompare(CameraKeyFrame a, CameraKeyFrame b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Sort KeyFrames")]
	public void SortKeyFrames()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Go To Frame i")]
	public void GoToFrame()
	{
		throw null;
	}
}

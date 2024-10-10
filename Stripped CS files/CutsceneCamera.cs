using System;
using System.Collections.Generic;
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

		public float duration = 1f;

		public int index;

		[HideInInspector]
		public CameraKeyFrame nextFrm;

		public Easing EaseToNextFrameMode = Easing.Hermite;

		public List<CameraKeyFrameEvent> frameEvents;

		public Vector3 EvalPos(float normT)
		{
			if (nextFrm != null)
			{
				return EaseToNextFrameMode switch
				{
					Easing.SinLerp => Vector3.Lerp(refTrf.position, nextFrm.refTrf.position, Mathfx.Sinerp(0f, 1f, normT)), 
					Easing.CosLerp => Vector3.Lerp(refTrf.position, nextFrm.refTrf.position, Mathfx.Coserp(0f, 1f, normT)), 
					Easing.Hermite => Vector3.Lerp(refTrf.position, nextFrm.refTrf.position, Mathfx.Hermite(0f, 1f, normT)), 
					Easing.Berp => Vector3.Lerp(refTrf.position, nextFrm.refTrf.position, Mathfx.Berp(0f, 1f, normT)), 
					_ => Vector3.Lerp(refTrf.position, nextFrm.refTrf.position, normT), 
				};
			}
			return refTrf.position;
		}

		public Quaternion EvalRot(float normT)
		{
			if (nextFrm != null)
			{
				return EaseToNextFrameMode switch
				{
					Easing.SinLerp => Quaternion.Lerp(refTrf.rotation, nextFrm.refTrf.rotation, Mathfx.Sinerp(0f, 1f, normT)), 
					Easing.CosLerp => Quaternion.Lerp(refTrf.rotation, nextFrm.refTrf.rotation, Mathfx.Coserp(0f, 1f, normT)), 
					Easing.Hermite => Quaternion.Lerp(refTrf.rotation, nextFrm.refTrf.rotation, Mathfx.Hermite(0f, 1f, normT)), 
					Easing.Berp => Quaternion.Lerp(refTrf.rotation, nextFrm.refTrf.rotation, Mathfx.Berp(0f, 1f, normT)), 
					_ => Quaternion.Lerp(refTrf.rotation, nextFrm.refTrf.rotation, normT), 
				};
			}
			return refTrf.rotation;
		}

		public void EvalEvents(float normT)
		{
			for (int i = 0; i < frameEvents.Count; i++)
			{
				CameraKeyFrameEvent cameraKeyFrameEvent = frameEvents[i];
				if (normT >= cameraKeyFrameEvent.timeIntoFrame && !cameraKeyFrameEvent.done)
				{
					cameraKeyFrameEvent.RunEvent();
					cameraKeyFrameEvent.done = true;
				}
			}
		}
	}

	public float totalDuration = 65f;

	public int i;

	public List<CameraKeyFrame> keyFrames;

	public float float_0;

	public float normT;

	public float lastFrameT;

	public CameraKeyFrame currentFrame;

	public int cFrameIndex;

	public float speedFactor = 1f;

	public void Start()
	{
		float_0 = Time.realtimeSinceStartup;
		lastFrameT = float_0;
		cFrameIndex = 0;
		if (keyFrames.Count > 0)
		{
			currentFrame = keyFrames[cFrameIndex];
			float num = 0f;
			for (int i = 0; i < keyFrames.Count - 1; i++)
			{
				keyFrames[i].nextFrm = keyFrames[i + 1];
				num += keyFrames[i].duration;
			}
			speedFactor = totalDuration / num;
		}
	}

	public void Update()
	{
		if (currentFrame == null)
		{
			return;
		}
		float_0 = Time.realtimeSinceStartup;
		normT = (float_0 - lastFrameT) / (currentFrame.duration * speedFactor);
		currentFrame.EvalEvents(normT);
		if (normT >= 1f)
		{
			if (cFrameIndex + 1 >= keyFrames.Count)
			{
				currentFrame = null;
				return;
			}
			cFrameIndex++;
			currentFrame = keyFrames[cFrameIndex];
			lastFrameT = float_0;
			normT = 1f - normT;
		}
		base.transform.position = currentFrame.EvalPos(normT);
		base.transform.rotation = currentFrame.EvalRot(normT);
	}

	[ContextMenu("Add Frame Here")]
	public void CreateKeyFrame()
	{
		Transform transform = new GameObject("CameraKeyFrame #" + i).transform;
		transform.position = base.transform.position;
		transform.rotation = base.transform.rotation;
		i++;
		CameraKeyFrame cameraKeyFrame = new CameraKeyFrame();
		cameraKeyFrame.refTrf = transform;
		keyFrames.Add(cameraKeyFrame);
	}

	public int kfCompare(CameraKeyFrame a, CameraKeyFrame b)
	{
		return a.index.CompareTo(b.index);
	}

	[ContextMenu("Sort KeyFrames")]
	public void SortKeyFrames()
	{
		keyFrames.Sort(kfCompare);
		for (int i = 0; i < keyFrames.Count; i++)
		{
			keyFrames[i].refTrf.name = "CameraKeyFrame #" + i;
		}
	}

	[ContextMenu("Go To Frame i")]
	public void GoToFrame()
	{
		base.transform.position = keyFrames[i].EvalPos(0f);
		base.transform.rotation = keyFrames[i].EvalRot(0f);
	}
}

using System;
using UnityEngine;

public class ScreenSpaceObjectScaling : MonoBehaviour
{
	public Camera refCamera;

	public float screenSize;

	public float lerpedScreenSize;

	public bool useLerp;

	public float lerpSpeed = 8f;

	public Transform trf;

	public void Start()
	{
		trf = base.transform;
		refCamera = refCamera ?? Camera.main;
	}

	public void LateUpdate()
	{
		if (useLerp)
		{
			lerpedScreenSize = Mathf.Lerp(lerpedScreenSize, screenSize, lerpSpeed * Time.deltaTime);
			trf.localScale = Vector3.one * (lerpedScreenSize / (float)Screen.height) * (Mathf.Tan(refCamera.fieldOfView * ((float)Math.PI / 180f)) * (refCamera.transform.position - trf.position).magnitude);
		}
		else
		{
			trf.localScale = Vector3.one * (screenSize / (float)Screen.height) * (Mathf.Tan(refCamera.fieldOfView * ((float)Math.PI / 180f)) * (refCamera.transform.position - trf.position).magnitude);
		}
	}

	public float GetScreenSize()
	{
		return screenSize;
	}
}

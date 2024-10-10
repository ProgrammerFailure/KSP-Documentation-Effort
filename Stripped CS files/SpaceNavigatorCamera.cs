using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class SpaceNavigatorCamera : MonoBehaviour
{
	public bool HasInputControl;

	public Func<bool> cameraWantsControl;

	public IKSPCamera kspCam;

	public List<IKSPCamera> cams;

	public float translateSpeed = 1f;

	[SerializeField]
	public float sharpnessLin = 18f;

	[SerializeField]
	public float sharpnessRot = 18f;

	[SerializeField]
	public float sensLin = 20f;

	[SerializeField]
	public float sensRot = 30f;

	public bool collideWithStuff = true;

	public float standoffRadius = 1f;

	public RaycastHit hit;

	public bool UseMaxRadius;

	public float MaxRadius = 1000f;

	public bool latchToDisabledCamera;

	public SpaceNavigatorCamera()
	{
	}

	public void Start()
	{
		if (SpaceNavigator.Instance != null && !(SpaceNavigator.Instance is SpaceNavigatorNoDevice))
		{
			Debug.Log("[SpaceNavCamera]: Found 3DConnexion Device.", base.gameObject);
			SpaceNavigator.SetRotationSensitivity(100f);
			SpaceNavigator.SetTranslationSensitivity(100f);
			SceneManager.sceneLoaded += OnSceneLoaded;
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	public void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		OnLevelLoaded(HighLogic.GetLoadedGameSceneFromBuildIndex(scene.buildIndex));
	}

	public void OnLevelLoaded(GameScenes lvl)
	{
		if (!base.enabled)
		{
			base.enabled = true;
		}
	}

	public void LateUpdate()
	{
		if (SpaceNavigator.Instance == null || SpaceNavigator.Instance is SpaceNavigatorNoDevice)
		{
			return;
		}
		if (HighLogic.LoadedSceneIsFlight && FlightInputHandler.SPACENAV_USE_AS_FLIGHT_CONTROL)
		{
			if (HasInputControl)
			{
				OnCameraRequestControl();
			}
		}
		else if (!HasInputControl)
		{
			if (!(SpaceNavigator.Translation != Vector3.zero) && !(SpaceNavigator.Rotation != Quaternion.identity))
			{
				return;
			}
			if (kspCam == null)
			{
				cams = KSPUtil.FindComponentsImplementing<IKSPCamera>(base.gameObject, latchToDisabledCamera);
				kspCam = ((cams.Count > 0) ? cams[0] : null);
			}
			if (kspCam != null)
			{
				if (kspCam.OnNavigatorRequestControl())
				{
					sensLin = GameSettings.SPACENAV_CAMERA_SENS_LIN;
					sensRot = GameSettings.SPACENAV_CAMERA_SENS_ROT;
					sharpnessLin = GameSettings.SPACENAV_CAMERA_SHARPNESS_LIN;
					sharpnessRot = GameSettings.SPACENAV_CAMERA_SHARPNESS_ROT;
					cameraWantsControl = kspCam.OnNavigatorTakeOver(OnCameraRequestControl);
					HasInputControl = true;
					OnGetControl();
				}
			}
			else
			{
				Debug.Log("[SpaceNavCamera]: No enabled IKSPCamera components in " + base.gameObject.name + ".", base.gameObject);
				base.enabled = false;
			}
		}
		else
		{
			OnCameraUpdate();
			if (cameraWantsControl())
			{
				OnCameraRequestControl();
			}
		}
	}

	public void OnCameraRequestControl()
	{
		OnCameraWantsControl();
		kspCam.OnNavigatorHandoff();
		HasInputControl = false;
		kspCam = null;
	}

	public Vector3 getNextTranslation(Transform cam, Vector3 nextStep, float epsilon = 0.0001f)
	{
		if (nextStep.sqrMagnitude < epsilon)
		{
			return Vector3.zero;
		}
		if (collideWithStuff && Physics.SphereCast(cam.position, standoffRadius, cam.rotation * nextStep, out hit, Mathf.Min(standoffRadius, nextStep.magnitude), LayerUtil.DefaultEquivalent | 0x8000))
		{
			nextStep = Vector3.Lerp(Quaternion.Inverse(cam.rotation) * hit.normal, nextStep, hit.distance / Mathf.Max(standoffRadius, nextStep.magnitude));
		}
		return nextStep;
	}

	public Vector3 clampToRadius(Vector3 rPos)
	{
		if (UseMaxRadius && rPos.sqrMagnitude > MaxRadius * MaxRadius)
		{
			return Vector3.ClampMagnitude(rPos, MaxRadius);
		}
		return rPos;
	}

	public abstract void OnGetControl();

	public abstract void OnCameraUpdate();

	public abstract void OnCameraWantsControl();
}

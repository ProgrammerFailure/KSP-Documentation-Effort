using System.Collections.Generic;
using Expansions.Missions.Runtime;
using ns36;
using ns9;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	public enum CameraMode
	{
		Flight,
		Map,
		External,
		const_3,
		Internal
	}

	public float existingFlightFoV = -1f;

	public float existingIVAFoV = -1f;

	[HideInInspector]
	public CameraMode previousCameraMode;

	[HideInInspector]
	public CameraMode currentCameraMode;

	public bool storeFlightCameraIVAOverlayState;

	public Kerbal ivaCameraActiveKerbal;

	public int ivaCameraActiveKerbalIndex = -1;

	public Part activeInternalPart;

	public static CameraManager Instance { get; set; }

	public Kerbal IVACameraActiveKerbal => ivaCameraActiveKerbal;

	public int IVACameraActiveKerbalIndex => ivaCameraActiveKerbalIndex;

	public void Awake()
	{
		Instance = this;
	}

	public void Start()
	{
		SetCameraFlight();
		GameEvents.onVesselWasModified.Add(OnVesselModified);
	}

	public void OnDestroy()
	{
		GameEvents.onVesselWasModified.Remove(OnVesselModified);
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void Update()
	{
		if (!InputLockManager.IsUnlocked(ControlTypes.CAMERAMODES) || FlightDriver.Pause)
		{
			return;
		}
		if (GameSettings.MODIFIER_KEY.GetKey() && GameSettings.CAMERA_MODE.GetKeyDown())
		{
			if (currentCameraMode == CameraMode.Flight)
			{
				KerbalPortraitGallery.ToggleIVAOverlay();
			}
		}
		else if (GameSettings.CAMERA_MODE.GetKeyDown())
		{
			NextCameraMode();
		}
		if (GameSettings.CAMERA_NEXT.GetKeyDown())
		{
			NextCamera();
		}
		MissionSystem.EnforceCameraModeAndLocks();
	}

	public void NextCameraMode()
	{
		switch (currentCameraMode)
		{
		case CameraMode.Flight:
			if (HighLogic.CurrentGame.Parameters.Flight.CanIVA)
			{
				if (!SetCameraIVA())
				{
					SetCameraFlight();
				}
				storeFlightCameraIVAOverlayState = KerbalPortraitGallery.isIVAOverlayVisible;
			}
			break;
		case CameraMode.const_3:
			SetCameraFlight();
			break;
		case CameraMode.Internal:
			SetCameraIVA();
			break;
		case CameraMode.Map:
		case CameraMode.External:
			break;
		}
	}

	public void PreviousCameraMode()
	{
		if (previousCameraMode != currentCameraMode)
		{
			if (currentCameraMode == CameraMode.Flight)
			{
				storeFlightCameraIVAOverlayState = KerbalPortraitGallery.isIVAOverlayVisible;
			}
			SetCameraMode(previousCameraMode);
		}
	}

	public void NextCamera()
	{
		switch (currentCameraMode)
		{
		case CameraMode.Flight:
			FlightCamera.fetch.SetNextMode();
			break;
		case CameraMode.const_3:
			NextCameraIVA();
			break;
		case CameraMode.Map:
		case CameraMode.External:
		case CameraMode.Internal:
			break;
		}
	}

	public void SetCameraMode(CameraMode mode)
	{
		switch (mode)
		{
		case CameraMode.Flight:
			SetCameraFlight();
			break;
		case CameraMode.const_3:
			SetCameraIVA();
			break;
		case CameraMode.Internal:
			SetCameraIVA();
			break;
		case CameraMode.Map:
		case CameraMode.External:
			break;
		}
	}

	public void SetCameraFlight()
	{
		if (!MissionSystem.AllowCameraSwitch(CameraMode.Flight))
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8003105"), 5f, ScreenMessageStyle.UPPER_CENTER);
			return;
		}
		ICameras_DeactivateAll();
		if (ivaCameraActiveKerbal != null)
		{
			ivaCameraActiveKerbal.IVADisable();
		}
		if (FlightGlobals.ActiveVessel != null)
		{
			FlightGlobals.ActiveVessel.RecallReferenceTransform();
		}
		FlightCamera.fetch.gameObject.SetActive(value: true);
		FlightCamera.fetch.EnableCamera();
		FlightCamera.fetch.ResumeFoV();
		FlightCamera.fetch.ActivateUpdate();
		CrewHatchController.fetch.EnableInterface();
		previousCameraMode = currentCameraMode;
		currentCameraMode = CameraMode.Flight;
		activeInternalPart = null;
		if (existingFlightFoV > 0f)
		{
			FlightCamera.fetch.SetFoV(existingFlightFoV);
		}
		GameEvents.OnCameraChange.Fire(CameraMode.Flight);
		if (storeFlightCameraIVAOverlayState)
		{
			KerbalPortraitGallery.ToggleIVAOverlay();
			storeFlightCameraIVAOverlayState = false;
		}
	}

	public void SetCameraMap()
	{
		if (!MissionSystem.AllowCameraSwitch(CameraMode.Map))
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8003103"), 5f, ScreenMessageStyle.UPPER_CENTER);
			return;
		}
		if (currentCameraMode == CameraMode.Flight)
		{
			existingFlightFoV = FlightCamera.fetch.FieldOfView;
		}
		if (!HighLogic.CurrentGame.Parameters.Flight.CanUseMap)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_7003264"), 5f, ScreenMessageStyle.UPPER_CENTER);
			return;
		}
		ICameras_DeactivateAll();
		if (ivaCameraActiveKerbal != null)
		{
			ivaCameraActiveKerbal.IVADisable();
		}
		if (FlightGlobals.ActiveVessel != null)
		{
			FlightGlobals.ActiveVessel.RecallReferenceTransform();
		}
		ScaledCamera.Instance.SetFoV(60f);
		FlightCamera.fetch.DisableCamera(disableAudioListener: false);
		FlightCamera.fetch.gameObject.SetActive(value: false);
		CrewHatchController.fetch.DisableInterface();
		if (currentCameraMode == CameraMode.Flight)
		{
			storeFlightCameraIVAOverlayState = KerbalPortraitGallery.isIVAOverlayVisible;
		}
		previousCameraMode = currentCameraMode;
		currentCameraMode = CameraMode.Map;
		activeInternalPart = null;
		GameEvents.OnCameraChange.Fire(CameraMode.Map);
	}

	public bool SetCameraIVA()
	{
		if (!MissionSystem.AllowCameraSwitch(CameraMode.const_3))
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8003104"), 5f, ScreenMessageStyle.UPPER_CENTER);
			return false;
		}
		if (currentCameraMode == CameraMode.Flight)
		{
			existingFlightFoV = FlightCamera.fetch.FieldOfView;
			storeFlightCameraIVAOverlayState = KerbalPortraitGallery.isIVAOverlayVisible;
		}
		if (ivaCameraActiveKerbal != null && ivaCameraActiveKerbal.InVessel != null && ivaCameraActiveKerbal.InVessel == FlightGlobals.fetch.activeVessel && ivaCameraActiveKerbal.state == Kerbal.States.ALIVE)
		{
			return SetCameraIVA(ivaCameraActiveKerbal, resetCamera: false);
		}
		List<ProtoCrewMember> vesselCrew = FlightGlobals.fetch.activeVessel.GetVesselCrew();
		int num = 0;
		int count = vesselCrew.Count;
		ProtoCrewMember protoCrewMember;
		while (true)
		{
			if (num < count)
			{
				protoCrewMember = vesselCrew[num];
				if (protoCrewMember.KerbalRef != null && protoCrewMember.KerbalRef.state == Kerbal.States.ALIVE)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return SetCameraIVA(protoCrewMember.KerbalRef, resetCamera: true);
	}

	public bool SetCameraIVA(Kerbal kerbal, bool resetCamera)
	{
		if (!MissionSystem.AllowCameraSwitch(CameraMode.const_3))
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_8003104"), 5f, ScreenMessageStyle.UPPER_CENTER);
			return false;
		}
		if (currentCameraMode == CameraMode.Flight)
		{
			existingFlightFoV = FlightCamera.fetch.FieldOfView;
			storeFlightCameraIVAOverlayState = KerbalPortraitGallery.isIVAOverlayVisible;
		}
		if (ivaCameraActiveKerbal == kerbal && currentCameraMode == CameraMode.const_3)
		{
			SetCameraFlight();
			return true;
		}
		ICameras_DeactivateAll();
		if (ivaCameraActiveKerbal != null && ivaCameraActiveKerbal != kerbal)
		{
			ICameras_ResetAll();
			ivaCameraActiveKerbal.IVADisable();
		}
		FlightCamera.fetch.EnableCamera();
		FlightCamera.fetch.DeactivateUpdate();
		FlightCamera.fetch.gameObject.SetActive(value: true);
		CrewHatchController.fetch.DisableInterface();
		if (!GameSettings.IVA_RETAIN_CONTROL_POINT)
		{
			kerbal.InVessel.SetReferenceTransform(kerbal.InPart, storeRecall: false);
		}
		if (kerbal.protoCrewMember.seat != null)
		{
			Vector3 kerbalEyeOffset = kerbal.protoCrewMember.seat.kerbalEyeOffset;
			if (kerbal.protoCrewMember.gender == ProtoCrewMember.Gender.Female)
			{
				kerbalEyeOffset *= GameSettings.FEMALE_EYE_OFFSET_SCALE;
				kerbalEyeOffset.x += GameSettings.FEMALE_EYE_OFFSET_X;
				kerbalEyeOffset.y += GameSettings.FEMALE_EYE_OFFSET_Y;
				kerbalEyeOffset.z += GameSettings.FEMALE_EYE_OFFSET_Z;
			}
			kerbal.eyeTransform.localPosition = kerbal.eyeInitialPos + kerbalEyeOffset;
		}
		else
		{
			kerbal.eyeTransform.localPosition = kerbal.eyeInitialPos;
		}
		InternalCamera.Instance.SetTransform(kerbal.eyeTransform, resetCamera);
		InternalCamera.Instance.EnableCamera();
		ivaCameraActiveKerbal = kerbal;
		ivaCameraActiveKerbal.IVAEnable();
		ivaCameraActiveKerbalIndex = FlightGlobals.fetch.activeVessel.GetVesselCrew().IndexOf(ivaCameraActiveKerbal.protoCrewMember);
		activeInternalPart = ivaCameraActiveKerbal.InPart;
		FlightGlobals.ActiveVessel.SetActiveInternalSpace(activeInternalPart);
		if (currentCameraMode != CameraMode.const_3)
		{
			previousCameraMode = currentCameraMode;
			currentCameraMode = CameraMode.const_3;
			GameEvents.OnCameraChange.Fire(CameraMode.const_3);
		}
		return true;
	}

	public void NextCameraIVA()
	{
		List<ProtoCrewMember> vesselCrew = FlightGlobals.fetch.activeVessel.GetVesselCrew();
		if (vesselCrew.Count != 0)
		{
			if (ivaCameraActiveKerbal == null || ivaCameraActiveKerbalIndex == -1)
			{
				SetCameraIVA(vesselCrew[0].KerbalRef, resetCamera: true);
			}
			SetCameraIVA(vesselCrew[GetNextIVA(vesselCrew)].KerbalRef, resetCamera: true);
			GameEvents.OnIVACameraKerbalChange.Fire(vesselCrew[GetNextIVA(vesselCrew)].KerbalRef);
		}
	}

	public int GetNextIVA(List<ProtoCrewMember> ptc)
	{
		int num = ivaCameraActiveKerbalIndex;
		int i;
		for (i = num + 1; i != num; i++)
		{
			if (i >= ptc.Count)
			{
				i = 0;
			}
			if (ptc[i].KerbalRef != null)
			{
				break;
			}
		}
		return i;
	}

	public void SetCameraInternal(InternalModel internalModel, Transform target)
	{
		if (currentCameraMode == CameraMode.Flight)
		{
			existingFlightFoV = FlightCamera.fetch.FieldOfView;
			storeFlightCameraIVAOverlayState = KerbalPortraitGallery.isIVAOverlayVisible;
		}
		if (ivaCameraActiveKerbal != null)
		{
			ivaCameraActiveKerbal.IVADisable();
		}
		ICameras_ResetAll();
		FlightCamera.fetch.EnableCamera();
		FlightCamera.fetch.DeactivateUpdate();
		CrewHatchController.fetch.DisableInterface();
		InternalCamera.Instance.SetTransform(target, resetCamera: true);
		InternalCamera.Instance.EnableCamera();
		activeInternalPart = internalModel.part;
		FlightGlobals.ActiveVessel.SetActiveInternalSpace(activeInternalPart);
		if (currentCameraMode != CameraMode.Internal)
		{
			previousCameraMode = currentCameraMode;
		}
		currentCameraMode = CameraMode.Internal;
		GameEvents.OnCameraChange.Fire(CameraMode.Internal);
	}

	public static void ICameras_DeactivateAll()
	{
		IGameCamera[] array = (IGameCamera[])Object.FindObjectsOfType(typeof(IGameCamera));
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			array[i].DisableCamera();
		}
	}

	public static void ICameras_ResetAll()
	{
		IGameCamera[] array = (IGameCamera[])Object.FindObjectsOfType(typeof(IGameCamera));
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			array[i].ResetCamera();
		}
	}

	public void OnVesselModified(Vessel v)
	{
		if (activeInternalPart != null)
		{
			if (activeInternalPart.vessel != FlightGlobals.ActiveVessel)
			{
				Debug.Log("[CameraManager]: Active IVA Space " + activeInternalPart.name + " is no longer part of active vessel " + FlightGlobals.ActiveVessel.GetDisplayName(), activeInternalPart.gameObject);
				SetCameraFlight();
			}
			else if (activeInternalPart.State == PartStates.DEAD)
			{
				Debug.Log("[CameraManager]: Active IVA Space " + activeInternalPart.name + " is destroyed.", activeInternalPart.gameObject);
				SetCameraFlight();
			}
			else
			{
				FlightGlobals.ActiveVessel.SetActiveInternalSpace(activeInternalPart);
			}
		}
	}

	public static Camera GetCurrentCamera()
	{
		if (HighLogic.LoadedSceneIsEditor && (bool)EditorLogic.fetch)
		{
			return EditorLogic.fetch.editorCamera;
		}
		if (MapView.MapIsEnabled && (bool)PlanetariumCamera.fetch)
		{
			return PlanetariumCamera.Camera;
		}
		if ((bool)FlightCamera.fetch && FlightCamera.fetch.mainCamera != null)
		{
			return FlightCamera.fetch.mainCamera;
		}
		return Camera.main;
	}
}

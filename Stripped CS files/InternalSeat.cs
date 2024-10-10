using System;
using ns9;
using UnityEngine;

public class InternalSeat : InternalModule
{
	[KSPField]
	public string seatTransformName = "";

	[KSPField]
	public string displayseatName = "";

	[KSPField]
	public string displayseatIndex = "";

	public Transform seatTransform;

	[KSPField]
	public bool allowCrewHelmet = true;

	[KSPField]
	public Vector3 kerbalEyeOffset = Vector3.zero;

	[KSPField]
	public Vector3 kerbalScale = Vector3.one;

	[KSPField]
	public Vector3 kerbalOffset = Vector3.zero;

	[KSPField]
	public string portraitCameraName = "";

	public Camera portraitCamera;

	public Vector3 portraitCameraInitialPosition;

	public Quaternion portraitCameraInitialRotation;

	public bool capturedInitial;

	[KSPField]
	public Vector3 portraitOffset = Vector3.zero;

	[NonSerialized]
	public ProtoCrewMember crew;

	public bool taken;

	public Kerbal kerbalRef;

	public void SpawnCrew()
	{
		if (crew != null)
		{
			Kerbal kerbal = ProtoCrewMember.Spawn(crew);
			kerbal.transform.parent = seatTransform;
			kerbal.transform.localPosition = kerbalOffset;
			kerbal.transform.localScale = Vector3.Scale(kerbal.transform.localScale, kerbalScale);
			kerbal.transform.localRotation = Quaternion.identity;
			kerbal.InPart = internalProp.internalModel.part;
			kerbal.ShowHelmet(allowCrewHelmet);
			kerbalRef = kerbal;
		}
	}

	public void DespawnCrew()
	{
		if ((bool)kerbalRef)
		{
			UnityEngine.Object.Destroy(kerbalRef.gameObject);
		}
	}

	public override void OnAwake()
	{
		if (seatTransformName != null)
		{
			seatTransform = internalProp.FindModelTransform(seatTransformName);
			if (seatTransform == null)
			{
				Debug.Log("InternalSeat: Cannot find seatTransform of name '" + seatTransformName + "'");
			}
		}
		if (displayseatName == null && !(displayseatIndex != string.Empty))
		{
			displayseatName = seatTransformName;
		}
		else if (displayseatIndex == null && !(displayseatIndex != string.Empty))
		{
			displayseatName = Localizer.Format(displayseatName);
		}
		else
		{
			displayseatName = Localizer.Format(displayseatName, displayseatIndex);
		}
		if (portraitCamera == null && !string.IsNullOrEmpty(portraitCameraName))
		{
			portraitCamera = internalProp.FindModelComponent<Camera>(portraitCameraName);
			if (portraitCamera == null)
			{
				Debug.Log("InternalSeat: Cannot find portraitCamera of name '" + portraitCameraName + "'");
			}
		}
		if (!capturedInitial && portraitCamera != null)
		{
			capturedInitial = true;
			portraitCameraInitialPosition = portraitCamera.transform.localPosition;
			portraitCameraInitialRotation = portraitCamera.transform.localRotation;
		}
	}

	public void UpdatePortraitOffset()
	{
		if (!capturedInitial)
		{
			capturedInitial = true;
			portraitCameraInitialPosition = portraitCamera.transform.localPosition;
			portraitCameraInitialRotation = portraitCamera.transform.localRotation;
		}
		portraitCamera.transform.localPosition = portraitCameraInitialPosition + portraitOffset;
	}
}

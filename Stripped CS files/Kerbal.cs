using System;
using System.Collections;
using System.Reflection;
using ns36;
using UnityEngine;

public class Kerbal : MonoBehaviour
{
	public enum States
	{
		BAILED_OUT,
		DEAD,
		NO_SIGNAL,
		ALIVE
	}

	public string crewMemberName;

	public float courage;

	public float stupidity;

	public bool isBadass;

	public bool veteran;

	public ProtoCrewMember.RosterStatus rosterStatus;

	public Part InPart;

	public ProtoCrewMember protoCrewMember;

	public float noiseSeed;

	public float camBobAmount = 10f;

	public Renderer[] textureTargets;

	public Texture2D textureStandard;

	public Texture2D textureVeteran;

	public Transform helmetTransform;

	public Transform headTransform;

	public States state;

	public float staticOverlayDuration = 3f;

	public float updateInterval;

	public bool running;

	public WaitForSeconds updIntervalYield;

	public Coroutine updateCoroutine;

	public bool EVAisUnlocked;

	public Camera kerbalCam;

	public Transform camPivot;

	public KerbalPortrait portrait;

	public RenderTexture avatarTexture;

	public bool visibleInPortrait;

	[HideInInspector]
	public bool showHelmet;

	public bool ivaMode;

	public IVACamera ivaCamera;

	public Transform eyeTransform;

	public bool randomizeOnStartup = true;

	public Vector3 eyeInitialPos;

	public bool capturedInitial;

	public Renderer[] headRenderers;

	public Animator[] animators;

	public FieldInfo canvasWillRenderCanvasesFieldInfo;

	public Vector2 screenPos;

	public Vessel InVessel
	{
		get
		{
			if (!(InPart != null))
			{
				return null;
			}
			return InPart.vessel;
		}
	}

	public Renderer[] HeadRenderers => headRenderers;

	public Animator[] Animators => animators;

	public virtual void Awake()
	{
		kerbalCam.enabled = false;
		running = false;
		GameEvents.onVesselSituationChange.Add(onVesselSitChange);
	}

	public void DestroyAvatarTexture()
	{
		if (kerbalCam != null)
		{
			kerbalCam.targetTexture = null;
		}
		if (protoCrewMember != null && protoCrewMember.seat != null && protoCrewMember.seat.portraitCamera != null)
		{
			protoCrewMember.seat.portraitCamera.targetTexture = null;
		}
		UnityEngine.Object.Destroy(avatarTexture);
		avatarTexture = null;
	}

	public virtual void OnDestroy()
	{
		DestroyAvatarTexture();
		if (KerbalPortraitGallery.Instance != null)
		{
			KerbalPortraitGallery.Instance.UnregisterActiveCrew(this);
		}
		GameEvents.onVesselSituationChange.Remove(onVesselSitChange);
		headRenderers = null;
		animators = null;
	}

	public virtual void Start()
	{
		canvasWillRenderCanvasesFieldInfo = typeof(Canvas).GetField("willRenderCanvases", BindingFlags.Static | BindingFlags.NonPublic);
		running = true;
		state = States.NO_SIGNAL;
		if (avatarTexture != null)
		{
			DestroyAvatarTexture();
		}
		avatarTexture = new RenderTexture(256, 256, 24);
		if (KerbalPortraitGallery.Instance != null)
		{
			portrait = KerbalPortraitGallery.Instance.RegisterActiveCrew(this);
		}
		kerbalCam.targetTexture = avatarTexture;
		kerbalCam.clearFlags = CameraClearFlags.Color;
		kerbalCam.backgroundColor = Color.black;
		SetupKerbalModel();
		SetupCache();
		if (randomizeOnStartup)
		{
			UnityEngine.Random.InitState(crewMemberName.GetHashCode_Net35());
			updateInterval = UnityEngine.Random.Range(0.1f, 0.15f);
			noiseSeed = UnityEngine.Random.Range(0, 20);
		}
		updIntervalYield = new WaitForSeconds(updateInterval);
		if (rosterStatus == ProtoCrewMember.RosterStatus.Dead)
		{
			state = States.DEAD;
		}
		if (visibleInPortrait)
		{
			updateCoroutine = StartCoroutine(kerbalAvatarUpdateCycle());
		}
		StartCoroutine(CallbackUtil.DelayedCallback(staticOverlayDuration, delegate
		{
			if (running)
			{
				state = States.ALIVE;
			}
		}));
	}

	public virtual void SetupKerbalModel()
	{
		int num = textureTargets.Length;
		while (num-- > 0)
		{
			textureTargets[num].material.mainTexture = (protoCrewMember.veteran ? textureVeteran : textureStandard);
		}
		if (!capturedInitial)
		{
			capturedInitial = true;
			eyeInitialPos = eyeTransform.localPosition;
		}
	}

	public virtual void SetupCache()
	{
		headRenderers = headTransform.GetComponentsInChildren<Renderer>();
		animators = base.gameObject.GetComponentsInChildren<Animator>();
	}

	public virtual void ShowHelmet(bool show)
	{
		if (show)
		{
			showHelmet = true;
			return;
		}
		showHelmet = false;
		if (helmetTransform != null)
		{
			UnityEngine.Object.Destroy(helmetTransform.gameObject);
		}
	}

	public virtual IEnumerator kerbalAvatarUpdateCycle()
	{
		while (running)
		{
			if (protoCrewMember != null && protoCrewMember.seat != null && protoCrewMember.seat.portraitCamera != null)
			{
				kerbalSeatCamUpdate(protoCrewMember.seat.portraitCamera);
			}
			else
			{
				kerbalAvatarUpdate();
			}
			yield return updIntervalYield;
		}
	}

	public virtual void kerbalAvatarUpdate()
	{
		kerbalCam.targetTexture = avatarTexture;
		if (ivaMode)
		{
			IVADisable(setIVAMode: false);
		}
		try
		{
			object value = canvasWillRenderCanvasesFieldInfo.GetValue(null);
			canvasWillRenderCanvasesFieldInfo.SetValue(null, null);
			kerbalCam.Render();
			canvasWillRenderCanvasesFieldInfo.SetValue(null, value);
		}
		catch (Exception ex)
		{
			Debug.LogWarning(ex.Message);
		}
		if (ivaMode)
		{
			IVAEnable(setIVAMode: false);
		}
	}

	public virtual void kerbalSeatCamUpdate(Camera seatCamera)
	{
		protoCrewMember.seat.UpdatePortraitOffset();
		seatCamera.targetTexture = avatarTexture;
		seatCamera.clearFlags = CameraClearFlags.Color;
		seatCamera.backgroundColor = Color.black;
		seatCamera.cullingMask = kerbalCam.cullingMask;
		float num = Mathf.PerlinNoise(Time.time, noiseSeed) - 0.5f;
		float num2 = Mathf.PerlinNoise(Time.time, noiseSeed + 1f) - 0.5f;
		float num3 = Mathf.PerlinNoise(Time.time, noiseSeed + 2f) - 0.5f;
		if (FlightGlobals.ActiveVessel != null)
		{
			float x = num * camBobAmount * Mathf.Clamp((float)FlightGlobals.ship_acceleration.magnitude * 5f, 0f, 50f) * 0.4f;
			float y = num2 * camBobAmount * Mathf.Clamp((float)FlightGlobals.ship_acceleration.magnitude * 5f, 0f, 50f) * 0.4f;
			float z = num3 * camBobAmount * Mathf.Clamp((float)FlightGlobals.ship_angularVelocity.magnitude, 0f, 100f) * 0.5f;
			seatCamera.transform.localRotation = protoCrewMember.seat.portraitCameraInitialRotation * Quaternion.Euler(x, y, z);
		}
		if (ivaMode)
		{
			IVADisable(setIVAMode: false);
		}
		try
		{
			object value = canvasWillRenderCanvasesFieldInfo.GetValue(null);
			canvasWillRenderCanvasesFieldInfo.SetValue(null, null);
			seatCamera.RenderDontRestore();
			canvasWillRenderCanvasesFieldInfo.SetValue(null, value);
		}
		catch (Exception ex)
		{
			Debug.LogWarning(ex.Message);
		}
		if (ivaMode)
		{
			IVAEnable(setIVAMode: false);
		}
	}

	public virtual void CameraOverlayUpdate()
	{
		if (portrait != null)
		{
			portrait.OverlayUpdate(state != States.ALIVE, (float)state);
		}
	}

	public virtual void die()
	{
		if (portrait != null)
		{
			portrait.OnCrewDie();
		}
		state = States.DEAD;
		rosterStatus = ProtoCrewMember.RosterStatus.Dead;
	}

	public virtual void onVesselSitChange(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> evt)
	{
		if (evt.host == FlightGlobals.ActiveVessel)
		{
			CheckEVAUnlocked();
			protoCrewMember.UpdateExperience();
		}
	}

	public virtual void CheckEVAUnlocked()
	{
		EVAisUnlocked = GameVariables.Instance.UnlockedEVA(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.AstronautComplex));
		EVAisUnlocked = GameVariables.Instance.EVAIsPossible(EVAisUnlocked, FlightGlobals.ActiveVessel);
	}

	public virtual void IVAEnable(bool setIVAMode = true)
	{
		int num = headRenderers.Length;
		while (num-- > 0)
		{
			headRenderers[num].enabled = false;
		}
		if (setIVAMode)
		{
			ivaMode = true;
		}
	}

	public virtual void IVADisable(bool setIVAMode = true)
	{
		int num = headRenderers.Length;
		while (num-- > 0)
		{
			headRenderers[num].enabled = true;
		}
		if (setIVAMode)
		{
			ivaMode = false;
		}
	}

	public virtual void SetVisibleInPortrait(bool visible)
	{
		if (visible)
		{
			if (!visibleInPortrait)
			{
				visibleInPortrait = true;
				if (updateCoroutine == null)
				{
					updateCoroutine = StartCoroutine(kerbalAvatarUpdateCycle());
				}
			}
		}
		else if (visibleInPortrait)
		{
			visibleInPortrait = false;
			if (updateCoroutine != null)
			{
				StopCoroutine(updateCoroutine);
				updateCoroutine = null;
			}
		}
	}
}

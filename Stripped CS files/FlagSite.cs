using ns37;
using ns9;
using UnityEngine;

public class FlagSite : PartModule
{
	public ConfigurableJoint joint;

	public AnimationClip flagDeployAnimation;

	public Animation animationRoot;

	public Collider[] colliders;

	public GameObject[] visibilityNodes;

	[KSPField]
	public float deployVisibilityDelay = 1f;

	[KSPField]
	public float deployFailRevertThreshold = 3f;

	[KSPField(isPersistant = true)]
	public string placedBy = "Unknown";

	[KSPField]
	public float unbreakablePeriodLength = 3f;

	[KSPField]
	public float breakForce = 180f;

	public KerbalFSM fsm;

	public KFSMState st_placing;

	public KFSMState st_placed;

	public KFSMState st_retracting;

	public KFSMState st_toppled;

	public KFSMEvent onPlacementFailed;

	public KFSMEvent onRetractStart;

	public KFSMEvent onTopple;

	public KFSMTimedEvent onPlaceComplete;

	public KFSMTimedEvent onRetractComplete;

	public string stateName;

	public Transform groundPivot;

	public PopupDialog SiteRenameDialog;

	public string siteName;

	public string newPlaqueText;

	[KSPField(isPersistant = true)]
	public string PlaqueText = "";

	public FlagPlaqueDialog FlagPlaqueDialog;

	public void Start()
	{
		isEnabled = true;
		base.Events["TakeDown"].active = false;
		base.Events["PickUp"].active = false;
		if ((bool)animationRoot)
		{
			SetupFSM();
		}
		GameEvents.onGameSceneLoadRequested.Add(onGameSceneLoadRequested);
	}

	public void OnDestroy()
	{
		GameEvents.onGameSceneLoadRequested.Remove(onGameSceneLoadRequested);
	}

	public void onGameSceneLoadRequested(GameScenes scene)
	{
		if (SiteRenameDialog != null)
		{
			DismissSiteRename();
		}
		if (FlagPlaqueDialog != null)
		{
			FlagPlaqueDialog.Terminate();
			onFlagPlaqueDialogDismiss();
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("state"))
		{
			stateName = node.GetValue("state");
		}
	}

	public void Update()
	{
		if (fsm.Started)
		{
			fsm.UpdateFSM();
		}
	}

	public void FixedUpdate()
	{
		if (fsm.Started)
		{
			fsm.FixedUpdateFSM();
		}
	}

	public void LateUpdate()
	{
		if (fsm.Started)
		{
			fsm.LateUpdateFSM();
		}
	}

	public override void OnSave(ConfigNode node)
	{
		if (fsm != null)
		{
			node.AddValue("state", fsm.currentStateName);
		}
	}

	public void SetupFSM()
	{
		fsm = new KerbalFSM();
		st_placing = new KFSMState("Placing");
		st_placing.OnEnter = delegate
		{
			animationRoot.Play(flagDeployAnimation.name);
			int num3 = colliders.Length;
			while (num3-- > 0)
			{
				colliders[num3].enabled = false;
			}
			MakeInvisible();
			Invoke("MakeVisible", deployVisibilityDelay);
			if (!base.part.packed)
			{
				SetJoint();
			}
			InputLockManager.SetControlLock(ControlTypes.PAUSE | ControlTypes.TIMEWARP | ControlTypes.QUICKSAVE | ControlTypes.QUICKLOAD | ControlTypes.VESSEL_SWITCHING, "Flag_NoInterruptWhileDeploying");
		};
		st_placing.OnLeave = delegate
		{
			MakeVisible();
			if (fsm.LastEvent != onPlaceComplete)
			{
				UnsetJoint();
			}
			InputLockManager.RemoveControlLock("Flag_NoInterruptWhileDeploying");
		};
		fsm.AddState(st_placing);
		st_placed = new KFSMState("Placed");
		st_placed.OnEnter = delegate
		{
			animationRoot.Stop();
			animationRoot[flagDeployAnimation.name].normalizedTime = 1f;
			animationRoot[flagDeployAnimation.name].weight = 1f;
			animationRoot[flagDeployAnimation.name].enabled = true;
			int num2 = colliders.Length;
			while (num2-- > 0)
			{
				colliders[num2].enabled = true;
			}
			base.Events["TakeDown"].active = true;
			if (!joint && !base.part.packed)
			{
				SetJoint();
			}
			base.part.PermanentGroundContact = true;
			if (base.vessel != null)
			{
				base.vessel.permanentGroundContact = true;
			}
		};
		st_placed.OnLeave = delegate
		{
			base.Events["TakeDown"].active = false;
			if ((bool)joint)
			{
				UnsetJoint();
			}
			base.part.PermanentGroundContact = false;
			if (base.vessel != null)
			{
				base.vessel.permanentGroundContact = false;
			}
		};
		fsm.AddState(st_placed);
		st_retracting = new KFSMState("Retracting");
		fsm.AddState(st_retracting);
		st_toppled = new KFSMState("Toppled");
		st_toppled.OnEnter = delegate
		{
			animationRoot.Stop();
			animationRoot[flagDeployAnimation.name].normalizedTime = 1f;
			animationRoot[flagDeployAnimation.name].weight = 1f;
			animationRoot[flagDeployAnimation.name].enabled = true;
			base.Events["PickUp"].active = true;
			int i = 0;
			for (int num = colliders.Length; i < num; i++)
			{
				colliders[i].enabled = true;
			}
		};
		st_toppled.OnLeave = delegate
		{
			base.Events["PickUp"].active = false;
		};
		fsm.AddState(st_toppled);
		onPlaceComplete = new KFSMTimedEvent("Place Complete", flagDeployAnimation.length);
		onPlaceComplete.GoToStateOnEvent = st_placed;
		fsm.AddEvent(onPlaceComplete, st_placing);
		onPlacementFailed = new KFSMEvent("Place Failed");
		onPlacementFailed.GoToStateOnEvent = st_toppled;
		onPlacementFailed.OnEvent = delegate
		{
			if (fsm.TimeAtCurrentState < (double)deployFailRevertThreshold)
			{
				PickUp();
			}
		};
		fsm.AddEvent(onPlacementFailed, st_placing);
		onTopple = new KFSMEvent("Toppled");
		onTopple.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		onTopple.GoToStateOnEvent = st_toppled;
		fsm.AddEventExcluding(onTopple, st_toppled);
		if (stateName != null)
		{
			fsm.StartFSM(stateName);
		}
		else
		{
			fsm.StartFSM(st_placing);
		}
	}

	public void MakeVisible()
	{
		int i = 0;
		for (int num = visibilityNodes.Length; i < num; i++)
		{
			visibilityNodes[i].SetActive(value: true);
		}
	}

	public void MakeInvisible()
	{
		int i = 0;
		for (int num = visibilityNodes.Length; i < num; i++)
		{
			visibilityNodes[i].SetActive(value: false);
		}
	}

	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, unfocusedRange = 2f, guiName = "#autoLOC_6001833")]
	public void TakeDown()
	{
		PickUp();
	}

	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, unfocusedRange = 2f, guiName = "#autoLOC_6001834")]
	public void PickUp()
	{
		KerbalEVA component = FlightGlobals.ActiveVessel.GetComponent<KerbalEVA>();
		if (component != null && !component.isRagdoll)
		{
			component.AddFlag();
			base.vessel.Die();
		}
	}

	public void SetJoint()
	{
		Collider[] array = Physics.OverlapSphere(groundPivot.position, 0.1f, 32769, QueryTriggerInteraction.Ignore);
		Collider collider = null;
		int num = array.Length;
		while (num-- > 0)
		{
			Collider collider2 = array[num];
			if (!findRigidbodyInVessel(collider2.attachedRigidbody, base.vessel))
			{
				collider = collider2;
				break;
			}
		}
		joint = base.gameObject.AddComponent<ConfigurableJoint>();
		joint.angularXMotion = ConfigurableJointMotion.Locked;
		joint.angularYMotion = ConfigurableJointMotion.Locked;
		joint.angularZMotion = ConfigurableJointMotion.Locked;
		joint.xMotion = ConfigurableJointMotion.Locked;
		joint.yMotion = ConfigurableJointMotion.Locked;
		joint.zMotion = ConfigurableJointMotion.Locked;
		joint.projectionMode = JointProjectionMode.PositionAndRotation;
		joint.projectionDistance = 0f;
		joint.projectionAngle = 0f;
		joint.configuredInWorldSpace = true;
		StartCoroutine(CallbackUtil.DelayedCallback(unbreakablePeriodLength, delegate
		{
			if (joint != null)
			{
				joint.breakForce = breakForce;
				joint.breakTorque = breakForce;
			}
		}));
		if ((bool)collider)
		{
			if ((bool)collider.attachedRigidbody)
			{
				joint.connectedBody = collider.attachedRigidbody;
			}
		}
		else
		{
			Debug.LogWarning("[Flag Module]: WARNING - No Colliders detected in range. Flag may be floating", base.gameObject);
		}
	}

	public bool findRigidbodyInVessel(Rigidbody rb, Vessel v)
	{
		if (rb == null)
		{
			return false;
		}
		int num = 0;
		while (true)
		{
			if (num < v.parts.Count)
			{
				Part part = v.parts[num];
				if (rb == part.rb)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public void UnsetJoint()
	{
		Object.Destroy(joint);
	}

	public void OnVesselPack()
	{
		if (base.vessel != null)
		{
			base.vessel.BackupVessel();
		}
		if ((bool)joint)
		{
			UnsetJoint();
		}
	}

	public void OnVesselUnpack()
	{
		if (!joint && fsm.CurrentState == st_placed)
		{
			SetJoint();
		}
	}

	public void OnPlacementFail()
	{
		fsm.RunEvent(onPlacementFailed);
	}

	public void OnPlacementComplete()
	{
		int num = colliders.Length;
		while (num-- > 0)
		{
			colliders[num].enabled = true;
		}
		RenameSite(delegate
		{
			GameEvents.afterFlagPlanted.Fire(this);
		});
	}

	public void OnJointBreak(float breakForce)
	{
		Debug.LogFormat("[FlagSite]: Flag {0} broke from ground. Force: {1}", (base.vessel != null) ? base.vessel.vesselName : base.part.persistentId.ToString(), breakForce);
		fsm.RunEvent(onTopple);
	}

	public static FlagSite CreateFlag(Vector3 position, Quaternion rotation, Part spawner)
	{
		Part partPrefab = PartLoader.getPartInfoByName("flag").partPrefab;
		if (partPrefab == null)
		{
			return null;
		}
		partPrefab.gameObject.SetActive(value: true);
		FlagSite component = Object.Instantiate(partPrefab).GetComponent<FlagSite>();
		partPrefab.gameObject.SetActive(value: false);
		component.gameObject.SetActive(value: true);
		component.transform.rotation = rotation;
		component.transform.position = position - rotation * component.groundPivot.localPosition;
		component.vessel.orbit.referenceBody = FlightGlobals.getMainBody(component.transform.position);
		component.vessel.Initialize();
		component.vessel.vesselType = VesselType.Flag;
		component.part.flagURL = spawner.flagURL;
		component.part.flightID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.Updated().flightState);
		component.part.missionID = spawner.missionID;
		component.part.launchID = spawner.launchID;
		component.placedBy = spawner.vessel.vesselName;
		component.GetComponent<Rigidbody>().inertiaTensor = Vector3.one * 0.001f;
		GameEvents.onFlagPlant.Fire(component.vessel);
		return component;
	}

	public static Vector3 ScanSurroundingTerrain(Vector3 refPos, Vector3 fwd, float distance, int samples = 9)
	{
		Vector3 vector = FlightGlobals.getUpAxis(FlightGlobals.currentMainBody, refPos);
		float num = float.PositiveInfinity;
		Vector3 vector2 = Vector3.zero;
		float num2 = 360f / (float)samples;
		Vector3 vector3 = vector * 10f;
		Physics.Raycast(refPos + vector3, -vector, out var hitInfo, 20f, 32768, QueryTriggerInteraction.Ignore);
		refPos = hitInfo.point;
		for (int i = 0; i < samples; i++)
		{
			Vector3 vector4 = Quaternion.AngleAxis(num2 * (float)i, vector) * (fwd * distance);
			Physics.Raycast(refPos + vector4 + vector3, -vector, out hitInfo, 20f, 32768, QueryTriggerInteraction.Ignore);
			float num3 = Mathf.Abs(Vector3.Dot(hitInfo.point - refPos, vector));
			if (num3 < num)
			{
				num = num3;
				vector2 = hitInfo.point;
			}
			Debug.DrawRay(refPos + vector4 + vector3, -vector * 20f, Color.green, 2f);
		}
		Debug.DrawLine(refPos, vector2, Color.green, 2f);
		return vector2;
	}

	public void RenameSite()
	{
		RenameSite(null);
	}

	public void RenameSite(Callback afterDialog)
	{
		InputLockManager.SetControlLock("siteRenameDialog");
		siteName = "";
		newPlaqueText = PlaqueText;
		SiteRenameDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("SiteRename", Localizer.Format("#autoLOC_226963"), Localizer.Format("#autoLOC_6001958"), HighLogic.UISkin, 250f, new DialogGUITextInput(siteName, multiline: false, 64, delegate(string n)
		{
			siteName = n;
			return siteName;
		}, 28f), new DialogGUILabel(Localizer.Format("#autoLOC_226969")), new DialogGUITextInput(newPlaqueText, multiline: true, 1000, delegate(string n)
		{
			newPlaqueText = n;
			return newPlaqueText;
		}, 100f), new DialogGUIButton(Localizer.Format("#autoLOC_226975"), delegate
		{
			AcceptSiteRename();
			if (afterDialog != null)
			{
				afterDialog();
			}
		}, () => Vessel.IsValidVesselName(siteName), dismissOnSelect: true), new DialogGUIButton(Localizer.Format("#autoLOC_226976"), delegate
		{
			DismissSiteRename();
			if (afterDialog != null)
			{
				afterDialog();
			}
		}, dismissOnSelect: true)), persistAcrossScenes: false, null);
		SiteRenameDialog.OnDismiss = DismissSiteRename;
	}

	public void AcceptSiteRename()
	{
		if (Vessel.IsValidVesselName(siteName))
		{
			base.vessel.vesselName = siteName;
			PlaqueText = newPlaqueText;
			DismissSiteRename();
		}
	}

	public void CancelSiteRename()
	{
		base.vessel.vesselName = Localizer.Format("#autoLoc_6002179");
		PlaqueText = "";
		DismissSiteRename();
	}

	public void DismissSiteRename()
	{
		SiteRenameDialog.Dismiss();
		SiteRenameDialog = null;
		InputLockManager.RemoveControlLock("siteRenameDialog");
	}

	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, unfocusedRange = 2f, guiName = "#autoLOC_6001835")]
	public void ReadPlaque()
	{
		InputLockManager.SetControlLock(~(ControlTypes.flag_2 | ControlTypes.CAMERACONTROLS), "FlagPlaqueDialog");
		FlagPlaqueDialog = FlagPlaqueDialog.Spawn(base.vessel.vesselName, PlaqueText, onFlagPlaqueDialogDismiss);
	}

	public void onFlagPlaqueDialogDismiss()
	{
		FlagPlaqueDialog = null;
		InputLockManager.RemoveControlLock("FlagPlaqueDialog");
	}
}

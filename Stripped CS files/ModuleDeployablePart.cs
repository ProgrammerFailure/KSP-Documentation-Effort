using System;
using System.Collections.Generic;
using Expansions.Missions.Adjusters;
using ns9;
using UnityEngine;

public class ModuleDeployablePart : PartModule, IMultipleDragCube, IScalarModule, IConstruction
{
	public enum DeployState
	{
		RETRACTED,
		EXTENDED,
		RETRACTING,
		EXTENDING,
		BROKEN
	}

	public enum TrackingMode
	{
		const_0,
		HOME,
		CURRENT,
		VESSEL,
		NONE
	}

	public enum PanelAlignType
	{
		PIVOT,
		const_1,
		const_2,
		const_3
	}

	[KSPField(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001352")]
	public string status = Localizer.Format("#autoLOC_234861");

	[KSPField]
	public bool showStatus = true;

	[KSPField]
	public Quaternion originalRotation;

	[KSPField(isPersistant = true)]
	public Quaternion currentRotation;

	[KSPField]
	public bool runOnce = true;

	[KSPField(isPersistant = true)]
	public float storedAnimationTime;

	[KSPField(isPersistant = true)]
	public float storedAnimationSpeed;

	[KSPField]
	public bool isTracking = true;

	[KSPField]
	public bool applyShielding = true;

	[KSPField]
	public bool applyShieldingExtend = true;

	[KSPField]
	public float TrackingAlignmentOffset;

	[KSPField]
	public bool retractable = true;

	[KSPField]
	public bool isBreakable = true;

	[KSPField]
	public float windResistance = 3f;

	[KSPField]
	public double gResistance = double.PositiveInfinity;

	[KSPField]
	public float impactResistance = 2f;

	[KSPField]
	public float impactResistanceRetracted = 5f;

	[KSPField]
	public float subPartMass = 0.01f;

	[KSPField]
	public float trackingSpeed = 0.25f;

	[KSPField]
	public string pivotName = "sunPivot";

	[KSPField]
	public string breakName = string.Empty;

	[KSPField]
	public PanelAlignType alignType;

	[KSPField]
	public string secondaryTransformName = string.Empty;

	[KSPField]
	public string animationName;

	[KSPField]
	public float editorAnimationSpeedMult = 10f;

	[KSPField]
	public bool useAnimation = true;

	public bool bypassSetupAnimation;

	[KSPField]
	public float panelDrag = 0.4f;

	[KSPField]
	public bool useCurve;

	[KSPField(isPersistant = true)]
	public DeployState deployState;

	[KSPField]
	public TrackingMode trackingMode;

	[KSPField]
	public bool eventsInSymmwtryAlways;

	[KSPField]
	public bool eventsInSymmwtryEditor = true;

	[KSPField]
	public string extendActionName = "#autoLOC_6001801";

	[KSPField]
	public string retractActionName = "#autoLOC_6001802";

	[KSPField]
	public string extendpanelsActionName = "#autoLOC_6001800";

	[KSPField]
	public string subPartName = "#autoLOC_235328";

	[KSPField]
	public string partType = "#autoLOC_235329";

	public Transform panelRotationTransform;

	public Transform panelBreakTransform;

	public bool hasPivot;

	public CelestialBody trackingBody;

	public Vessel trackingVessel;

	public string vesselID;

	public Transform trackingTransformLocal;

	public Transform trackingTransformScaled;

	public Transform secondaryTransform;

	public Animation anim;

	public bool stopAnimation;

	public List<GameObject> breakObjects;

	public EventData<float, float> onMove;

	public EventData<float> onStop;

	public bool playAnimationOnStart;

	[SerializeField]
	public int repairKitsNecessary = 1;

	public BaseEvent eventRepairExternal;

	[KSPField(guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6003063")]
	[UI_Label(scene = UI_Scene.Flight)]
	public string brokenStatusWarning = "";

	public UI_Label brokenStatusWarningField;

	public List<string> destroyOnBreakObjects;

	public bool trackingLOS;

	public string blockingObject;

	[KSPField]
	public string moduleID = "deployablePart";

	public bool overrideUIWriteState = true;

	public List<AdjusterDeployablePartBase> adjusterCache = new List<AdjusterDeployablePartBase>();

	public static string cacheAutoLOC_234828;

	public static string cacheAutoLOC_234841;

	public static string cacheAutoLOC_234856;

	public static string cacheAutoLOC_234861;

	public static string cacheAutoLOC_234868;

	public static string cacheAutoLOC_6001415;

	public static string cacheAutoLOC_6001017;

	public static string cacheAutoLOC_6005093;

	public virtual float MinAoAForQCheck => 0.1875f;

	public string ScalarModuleID => moduleID;

	public virtual float GetScalar
	{
		get
		{
			if (base.part.ShieldedFromAirstream && applyShielding)
			{
				return 0f;
			}
			if (useAnimation)
			{
				switch (deployState)
				{
				case DeployState.EXTENDED:
					return 1f;
				default:
					return anim[animationName].normalizedTime;
				case DeployState.RETRACTED:
				case DeployState.BROKEN:
					return 0f;
				}
			}
			if (deployState == DeployState.BROKEN)
			{
				return 0f;
			}
			return 1f;
		}
	}

	public virtual bool CanMove
	{
		get
		{
			if (deployState != DeployState.BROKEN && !IsDeployablePartStuck())
			{
				if (useAnimation)
				{
					if (!HighLogic.LoadedSceneIsEditor && base.part.ShieldedFromAirstream)
					{
						if (!applyShielding)
						{
							return !applyShieldingExtend;
						}
						return false;
					}
					return true;
				}
				return true;
			}
			return false;
		}
	}

	public EventData<float, float> OnMoving => onMove;

	public EventData<float> OnStop => onStop;

	public bool IsMultipleCubesActive
	{
		get
		{
			if (!useAnimation)
			{
				return isTracking;
			}
			return true;
		}
	}

	[KSPAction("#autoLOC_6001800")]
	public void ExtendPanelsAction(KSPActionParam param)
	{
		if (param.type != 0 && (param.type != KSPActionType.Toggle || deployState != 0))
		{
			Retract();
		}
		else
		{
			Extend();
		}
	}

	public void ControlPanelsWithoutUsingSymmetry(KSPActionType action)
	{
		if (action == KSPActionType.Activate)
		{
			DoExtend();
		}
		else
		{
			DoRetract();
		}
	}

	[KSPAction("#autoLOC_6001801")]
	public void ExtendAction(KSPActionParam param)
	{
		Extend();
	}

	[KSPAction("#autoLOC_6001802")]
	public void RetractAction(KSPActionParam param)
	{
		Retract();
	}

	[KSPEvent(unfocusedRange = 4f, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001803")]
	public virtual void Extend()
	{
		DoExtend();
		if (!eventsInSymmwtryAlways && (!HighLogic.LoadedSceneIsEditor || !eventsInSymmwtryEditor))
		{
			return;
		}
		int count = base.part.symmetryCounterparts.Count;
		while (count-- > 0)
		{
			ModuleDeployablePart moduleDeployablePart = base.part.symmetryCounterparts[count].Modules[base.part.Modules.IndexOf(this)] as ModuleDeployablePart;
			if (moduleDeployablePart != null)
			{
				moduleDeployablePart.DoExtend();
			}
		}
	}

	public virtual void DoExtend()
	{
		if (deployState != 0)
		{
			return;
		}
		if (!HighLogic.LoadedSceneIsEditor && base.part.ShieldedFromAirstream && applyShieldingExtend)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_234422", partType), 5f, ScreenMessageStyle.UPPER_LEFT);
			return;
		}
		if (useAnimation)
		{
			anim[animationName].speed = (HighLogic.LoadedSceneIsEditor ? editorAnimationSpeedMult : 1f);
			anim[animationName].normalizedTime = 0f;
			anim[animationName].enabled = true;
			anim.Play(animationName);
		}
		deployState = DeployState.EXTENDING;
		base.Events["Extend"].active = false;
		onMove.Fire(0f, 1f);
	}

	[KSPEvent(unfocusedRange = 4f, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001804")]
	public virtual void Retract()
	{
		DoRetract();
		if (!eventsInSymmwtryAlways && (!HighLogic.LoadedSceneIsEditor || !eventsInSymmwtryEditor))
		{
			return;
		}
		int count = base.part.symmetryCounterparts.Count;
		while (count-- > 0)
		{
			ModuleDeployablePart moduleDeployablePart = base.part.symmetryCounterparts[count].Modules[base.part.Modules.IndexOf(this)] as ModuleDeployablePart;
			if (moduleDeployablePart != null)
			{
				moduleDeployablePart.DoRetract();
			}
		}
	}

	public virtual void DoRetract()
	{
		if ((retractable || HighLogic.LoadedSceneIsEditor) && deployState == DeployState.EXTENDED)
		{
			if (useAnimation)
			{
				anim[animationName].speed = (HighLogic.LoadedSceneIsEditor ? (0f - editorAnimationSpeedMult) : (-1f));
				anim[animationName].normalizedTime = 1f;
				anim[animationName].enabled = true;
				anim.Play(animationName);
			}
			deployState = DeployState.RETRACTING;
			base.Events["Retract"].active = false;
			onMove.Fire(1f, 0f);
		}
	}

	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = false, unfocusedRange = 4f, guiName = "#autoLOC_8003453")]
	public void EventRepairExternal()
	{
		if (HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().KerbalExperienceEnabled(HighLogic.CurrentGame.Mode) && FlightGlobals.ActiveVessel.VesselValues.RepairSkill.value < 1)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_246904", 1.ToString()));
		}
		else
		{
			if (!FlightGlobals.ActiveVessel.isEVA || !(FlightGlobals.ActiveVessel.evaController.ModuleInventoryPartReference != null))
			{
				return;
			}
			if (FlightGlobals.ActiveVessel.VesselValues.RepairSkill.value > 0)
			{
				if (FlightGlobals.ActiveVessel.evaController.ModuleInventoryPartReference.TotalAmountOfPartStored("evaRepairKit") >= repairKitsNecessary)
				{
					FlightGlobals.ActiveVessel.evaController.ModuleInventoryPartReference.RemoveNPartsFromInventory("evaRepairKit", repairKitsNecessary, playSound: true);
					if (deployState == DeployState.BROKEN)
					{
						DoRepair();
					}
					return;
				}
				AvailablePart partInfoByName = PartLoader.getPartInfoByName("evaRepairKit");
				if (partInfoByName != null)
				{
					ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6006097", repairKitsNecessary.ToString(), partInfoByName.title));
				}
			}
			else
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6006098"));
			}
		}
	}

	public void CheatRepair()
	{
		DoRepair();
	}

	public virtual bool DoRepair()
	{
		bool flag = false;
		if (base.part.partInfo != null && base.part.partInfo.partPrefab != null)
		{
			Part part = UnityEngine.Object.Instantiate(base.part.partInfo.partPrefab);
			Transform transform = part.FindModelTransform(breakName);
			Transform transform2 = null;
			Transform transform3 = part.FindModelTransform(pivotName);
			if (transform == null)
			{
				transform = transform3;
			}
			if (transform != null)
			{
				if (panelBreakTransform != null)
				{
					transform2 = panelBreakTransform.parent;
					panelBreakTransform.gameObject.DestroyGameObjectImmediate();
				}
				else
				{
					transform2 = base.part.FindModelTransform(transform.parent.name);
				}
				if (transform2 != null)
				{
					panelRotationTransform = part.FindModelTransform(pivotName);
					panelBreakTransform = part.FindModelTransform(breakName);
					Vector3 localPosition = transform.localPosition;
					Quaternion localRotation = transform.localRotation;
					Vector3 localScale = transform.localScale;
					transform.SetParent(transform2);
					transform.localPosition = localPosition;
					transform.localRotation = localRotation;
					transform.localScale = localScale;
					flag = true;
				}
			}
			part.gameObject.DestroyGameObjectImmediate();
		}
		if (flag)
		{
			breakObjects = new List<GameObject>();
			if (panelRotationTransform != null)
			{
				originalRotation = panelRotationTransform.localRotation;
				currentRotation = originalRotation;
				hasPivot = true;
			}
			else
			{
				hasPivot = false;
			}
			FindAnimations();
			if (!string.IsNullOrEmpty(secondaryTransformName))
			{
				secondaryTransform = base.part.FindModelTransform(secondaryTransformName);
				if (secondaryTransform == null)
				{
					Debug.LogError("Couldn't access secondaryTransform");
				}
			}
			deployState = DeployState.RETRACTED;
			eventRepairExternal.active = false;
			GameEvents.onPartRepaired.Fire(base.part);
			startFSM();
		}
		return flag;
	}

	public void onVesselFocusChange(Vessel v)
	{
		if (v.isEVA)
		{
			eventRepairExternal.guiName = Localizer.Format("#autoLOC_6005092", repairKitsNecessary.ToString());
			if (brokenStatusWarningField != null)
			{
				brokenStatusWarningField.SetSceneVisibility(UI_Scene.Flight, state: false);
			}
			if (!retractable)
			{
				base.Events["Retract"].guiActive = false;
			}
		}
		else
		{
			eventRepairExternal.guiName = cacheAutoLOC_6005093;
			if (brokenStatusWarningField != null)
			{
				brokenStatusWarningField.SetSceneVisibility(UI_Scene.Flight, deployState == DeployState.BROKEN);
			}
		}
	}

	public override void OnAwake()
	{
		base.OnAwake();
		onMove = new EventData<float, float>(base.part.partName + "_" + base.part.flightID + "_" + base.part.Modules.IndexOf(this) + "_onMove");
		onStop = new EventData<float>(base.part.partName + "_" + base.part.flightID + "_" + base.part.Modules.IndexOf(this) + "_onStop");
		panelRotationTransform = base.part.FindModelTransform(pivotName);
		breakObjects = new List<GameObject>();
		if (panelRotationTransform != null)
		{
			originalRotation = panelRotationTransform.localRotation;
			currentRotation = originalRotation;
			hasPivot = true;
		}
		else
		{
			hasPivot = false;
		}
	}

	public void OnDestroy()
	{
		GameEvents.onVesselChange.Remove(onVesselFocusChange);
	}

	public override void OnStart(StartState state)
	{
		FindAnimations();
		if (HighLogic.LoadedSceneIsMissionBuilder)
		{
			anim[animationName].normalizedTime = 0f;
			anim[animationName].enabled = true;
			anim[animationName].weight = 1f;
			anim.Stop(animationName);
			return;
		}
		BaseEvent baseEvent = base.Events["Extend"];
		string text2 = (base.Actions["ExtendAction"].guiName = Localizer.Format(extendActionName, partType));
		baseEvent.guiName = text2;
		BaseEvent baseEvent2 = base.Events["Retract"];
		text2 = (base.Actions["RetractAction"].guiName = Localizer.Format(retractActionName, partType));
		baseEvent2.guiName = text2;
		base.Events["Retract"].externalToEVAOnly = retractable;
		base.Actions["ExtendPanelsAction"].guiName = Localizer.Format(extendpanelsActionName, partType);
		repairKitsNecessary = Math.Min(Math.Max((int)(base.part.mass / GameSettings.PART_REPAIR_MASS_PER_KIT), 1), GameSettings.PART_REPAIR_MAX_KIT_AMOUNT);
		eventRepairExternal = base.Events["EventRepairExternal"];
		eventRepairExternal.active = deployState == DeployState.BROKEN;
		if (HighLogic.LoadedSceneIsFlight)
		{
			if (FlightGlobals.ActiveVessel.isEVA)
			{
				eventRepairExternal.guiName = Localizer.Format("#autoLOC_6005092", repairKitsNecessary.ToString());
				if (!retractable)
				{
					base.Events["Retract"].guiActive = false;
				}
			}
			else
			{
				eventRepairExternal.guiName = cacheAutoLOC_6005093;
			}
			brokenStatusWarning = cacheAutoLOC_6005093;
			if (base.Fields["brokenStatusWarning"] != null)
			{
				base.Fields.TryGetFieldUIControl<UI_Label>("brokenStatusWarning", out brokenStatusWarningField);
				if (brokenStatusWarningField != null)
				{
					brokenStatusWarningField.SetSceneVisibility(UI_Scene.Flight, deployState == DeployState.BROKEN);
				}
			}
			GameEvents.onVesselChange.Add(onVesselFocusChange);
		}
		if (!retractable)
		{
			base.Events["Retract"].guiActive = false;
			base.Actions["RetractAction"].active = false;
			base.Actions["ExtendPanelsAction"].active = false;
			BaseEvent baseEvent3 = base.Events["Retract"];
			baseEvent3.guiName = baseEvent3.guiName ?? "";
		}
		if (state == StartState.None)
		{
			return;
		}
		if ((bool)Planetarium.fetch)
		{
			switch (trackingMode)
			{
			default:
				trackingBody = Planetarium.fetch.Sun;
				break;
			case TrackingMode.HOME:
				trackingBody = Planetarium.fetch.Home;
				break;
			case TrackingMode.CURRENT:
				trackingBody = Planetarium.fetch.CurrentMainBody;
				break;
			case TrackingMode.VESSEL:
				trackingVessel = FlightGlobals.FindVessel(new Guid(vesselID));
				break;
			case TrackingMode.NONE:
				trackingBody = null;
				trackingVessel = null;
				break;
			}
		}
		GetTrackingBodyTransforms();
		if (!string.IsNullOrEmpty(secondaryTransformName))
		{
			secondaryTransform = base.part.FindModelTransform(secondaryTransformName);
			if (secondaryTransform == null)
			{
				Debug.LogError("Couldn't access secondaryTransform");
			}
		}
		panelRotationTransform = base.part.FindModelTransform(pivotName);
		if (string.IsNullOrEmpty(breakName))
		{
			breakName = pivotName;
		}
		panelBreakTransform = base.part.FindModelTransform(breakName);
		if (panelRotationTransform == null)
		{
			if (alignType == PanelAlignType.PIVOT)
			{
				Debug.LogError("Could not find the pivotName, " + pivotName + ", in heirarchy");
			}
			hasPivot = false;
		}
		else
		{
			hasPivot = true;
		}
		BaseField baseField = base.Fields["status"];
		bool guiActive = (base.Fields["status"].guiActiveEditor = showStatus);
		baseField.guiActive = guiActive;
		startFSM();
		base.part.ScheduleSetCollisionIgnores();
	}

	public void FindAnimations()
	{
		Animation[] componentsInChildren = GetComponentsInChildren<Animation>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].GetClip(animationName) != null)
			{
				anim = componentsInChildren[i];
				break;
			}
		}
		if (componentsInChildren.Length != 0 && anim == null)
		{
			anim = componentsInChildren[0];
		}
		if (!(anim == null) && useAnimation)
		{
			useAnimation = true;
		}
		else
		{
			useAnimation = false;
		}
		if (useAnimation && !anim[animationName])
		{
			useAnimation = false;
		}
	}

	public override string GetInfo()
	{
		string text = "";
		if (isTracking)
		{
			text += Localizer.Format("#autoLOC_234569", trackingSpeed.ToString("0.0###"));
		}
		if (isBreakable)
		{
			text += Localizer.Format("#autoLOC_234572", windResistance.ToString("0.0###"));
			if (gResistance < double.PositiveInfinity)
			{
				text = text + ", " + gResistance.ToString("F0") + "G";
			}
		}
		if (!retractable)
		{
			text += Localizer.Format("#autoLOC_234576", partType);
		}
		return text;
	}

	public override void OnLoad(ConfigNode node)
	{
		subPartName = Localizer.Format(subPartName);
		partType = Localizer.Format(partType);
		if (node.HasValue("stateString"))
		{
			deployState = (DeployState)Enum.Parse(typeof(DeployState), node.GetValue("stateString"));
		}
		if (node.HasValue("sunTracking"))
		{
			if (bool.Parse(node.GetValue("sunTracking")))
			{
				isTracking = true;
				trackingMode = TrackingMode.const_0;
			}
			else
			{
				isTracking = false;
			}
		}
		if (node.HasValue("vesselID"))
		{
			vesselID = node.GetValue("vesselID");
		}
		destroyOnBreakObjects = node.GetValuesList("destroyOnBreakObject");
	}

	public override void OnSave(ConfigNode node)
	{
		if (trackingVessel != null)
		{
			node.AddValue("vesselID", trackingVessel.id.ToString("N"));
		}
		for (int i = 0; i < destroyOnBreakObjects.Count; i++)
		{
			node.AddValue("destroyOnBreakObject", destroyOnBreakObjects[i]);
		}
	}

	public override void OnInventoryModeDisable()
	{
		bypassSetupAnimation = true;
		FindAnimations();
		startFSM();
		if (anim != null && anim[animationName] != null)
		{
			float normalizedTime = 0f;
			if (deployState == DeployState.EXTENDED)
			{
				normalizedTime = 1f;
			}
			anim[animationName].enabled = true;
			anim[animationName].weight = 1f;
			anim[animationName].speed = 0f;
			if (HighLogic.LoadedSceneIsFlight)
			{
				anim[animationName].normalizedTime = normalizedTime;
			}
		}
		if (base.part.protoPartSnapshot != null)
		{
			ProtoPartModuleSnapshot protoPartModuleSnapshot = base.part.protoPartSnapshot.FindModule("ModuleDeployablePart");
			if (protoPartModuleSnapshot != null && protoPartModuleSnapshot.moduleValues != null)
			{
				protoPartModuleSnapshot.moduleValues.SetValue("deployState", deployState.ToString());
			}
		}
	}

	public override void OnStoredInInventory(ModuleInventoryPart moduleInventoryPart)
	{
		base.OnStoredInInventory(moduleInventoryPart);
		if (HighLogic.LoadedSceneIsFlight)
		{
			deployState = DeployState.RETRACTED;
		}
	}

	public virtual void FixedUpdate()
	{
		if (HighLogic.LoadedSceneIsFlight && base.vessel != null)
		{
			if (trackingMode == TrackingMode.CURRENT)
			{
				if (trackingBody != base.vessel.mainBody)
				{
					trackingBody = base.vessel.mainBody;
					GetTrackingBodyTransforms();
				}
			}
			else if (trackingMode == TrackingMode.VESSEL && trackingTransformLocal == null)
			{
				if (trackingVessel == null)
				{
					trackingVessel = base.vessel;
				}
				trackingTransformLocal = trackingVessel.vesselTransform;
			}
			updateFSM();
		}
		else if (HighLogic.LoadedSceneIsEditor)
		{
			updateFSM();
		}
	}

	public virtual void LateUpdate()
	{
		if (!HighLogic.LoadedSceneIsMissionBuilder)
		{
			if (deployState == DeployState.BROKEN)
			{
				eventRepairExternal.active = true;
			}
			else if (useAnimation && stopAnimation)
			{
				anim.Stop(animationName);
				stopAnimation = false;
			}
		}
	}

	public virtual void OnCollisionEnter(Collision collision)
	{
		float num = impactResistance;
		if (isTracking && deployState == DeployState.RETRACTED)
		{
			num = impactResistanceRetracted;
		}
		if (collision.relativeVelocity.magnitude > num)
		{
			breakPanels();
		}
	}

	public virtual void startFSM()
	{
		if (useAnimation)
		{
			anim[animationName].wrapMode = WrapMode.ClampForever;
			switch (deployState)
			{
			case DeployState.RETRACTED:
				anim[animationName].normalizedTime = 0f;
				anim[animationName].enabled = true;
				anim[animationName].weight = 1f;
				anim.Stop(animationName);
				bypassSetupAnimation = true;
				base.Events["Retract"].active = false;
				base.Events["Extend"].active = true;
				break;
			case DeployState.EXTENDED:
				anim[animationName].normalizedTime = 1f;
				anim[animationName].enabled = true;
				anim[animationName].speed = 0f;
				anim[animationName].weight = 1f;
				base.Events["Extend"].active = false;
				base.Events["Retract"].active = retractable || HighLogic.LoadedSceneIsEditor;
				if (hasPivot)
				{
					panelRotationTransform.localRotation = currentRotation;
				}
				break;
			case DeployState.RETRACTING:
				base.Events["Retract"].active = false;
				base.Events["Extend"].active = false;
				break;
			case DeployState.EXTENDING:
				base.Events["Retract"].active = false;
				base.Events["Extend"].active = false;
				break;
			}
			if (deployState == DeployState.RETRACTING || deployState == DeployState.EXTENDING || deployState == DeployState.BROKEN)
			{
				anim[animationName].normalizedTime = storedAnimationTime;
				anim[animationName].speed = storedAnimationSpeed;
			}
			if (!bypassSetupAnimation)
			{
				anim.Play(animationName);
			}
			if (!playAnimationOnStart && deployState != DeployState.EXTENDING && deployState != DeployState.RETRACTING)
			{
				stopAnimation = true;
			}
		}
		else
		{
			if (hasPivot)
			{
				panelRotationTransform.localRotation = originalRotation;
			}
			if (deployState != DeployState.BROKEN)
			{
				deployState = DeployState.EXTENDED;
			}
			base.Events["Retract"].active = false;
			base.Events["Extend"].active = false;
			base.Actions["ExtendPanelsAction"].active = false;
			base.Actions["ExtendAction"].active = false;
			base.Actions["RetractAction"].active = false;
			base.Fields["status"].guiActiveEditor = false;
		}
		if (deployState == DeployState.BROKEN)
		{
			base.Events["Retract"].active = false;
			base.Events["Extend"].active = false;
			if ((bool)panelBreakTransform)
			{
				panelBreakTransform.gameObject.SetActive(value: false);
			}
		}
	}

	public virtual void recurse(Transform t)
	{
		for (int i = 0; i < t.childCount; i++)
		{
			Transform child = t.GetChild(i);
			if ((bool)child.gameObject.GetComponent<Renderer>())
			{
				if (!child.gameObject.GetComponent<Collider>())
				{
					child.gameObject.AddComponent<BoxCollider>();
				}
				breakObjects.Add(child.gameObject);
			}
			recurse(child);
		}
	}

	public virtual void breakPanels()
	{
		if ((!hasPivot && panelBreakTransform == null) || base.part.packed || deployState == DeployState.BROKEN || CheatOptions.NoCrashDamage)
		{
			return;
		}
		if (useAnimation && anim.IsPlaying(animationName))
		{
			anim.Stop(animationName);
		}
		recurse((panelBreakTransform != null) ? panelBreakTransform : panelRotationTransform);
		int count = breakObjects.Count;
		while (count-- > 0)
		{
			if (destroyOnBreakObjects.Contains(breakObjects[count].name))
			{
				UnityEngine.Object.Destroy(breakObjects[count]);
				continue;
			}
			GameObject gameObject = breakObjects[count];
			physicalObject obj = physicalObject.ConvertToPhysicalObject(base.part, gameObject);
			Rigidbody rb = obj.rb;
			rb.maxAngularVelocity = PhysicsGlobals.MaxAngularVelocity;
			Vector3 vector = new Vector3(UnityEngine.Random.Range(0, 2), UnityEngine.Random.Range(0, 2), UnityEngine.Random.Range(0, 2));
			Vector3 vector2 = new Vector3(UnityEngine.Random.Range(-3, 3), UnityEngine.Random.Range(-3, 3), UnityEngine.Random.Range(-3, 3));
			rb.angularVelocity = base.part.Rigidbody.angularVelocity + vector2;
			Vector3 lhs = base.vessel.CurrentCoM - base.part.Rigidbody.worldCenterOfMass;
			rb.velocity = base.part.Rigidbody.velocity + vector + Vector3.Cross(lhs, rb.angularVelocity);
			rb.mass = subPartMass;
			rb.useGravity = false;
			gameObject.transform.parent = null;
			obj.origDrag = panelDrag;
		}
		base.Events["Retract"].active = false;
		base.Events["Extend"].active = false;
		deployState = DeployState.BROKEN;
		if (base.part != null)
		{
			base.part.RefreshHighlighter();
			base.part.ResetCollisions();
			GameEvents.onVesselWasModified.Fire(base.part.vessel);
		}
	}

	public virtual void breakNotifications(string partTitle, string breakMessage)
	{
		if (base.vessel == FlightGlobals.ActiveVessel)
		{
			ScreenMessages.PostScreenMessage("<color=orange>[" + partTitle + "]: " + breakMessage + "</color>", 6f, ScreenMessageStyle.UPPER_CENTER);
		}
		FlightLogger.fetch.LogEvent(partTitle + ": " + breakMessage);
		Debug.Log(partTitle + ": " + breakMessage);
	}

	public virtual void PostFSMUpdate()
	{
	}

	public virtual void updateFSM()
	{
		bool flag = (trackingTransformLocal != null && (hasPivot || alignType != 0)) || (trackingTransformLocal != null && HighLogic.LoadedSceneIsEditor);
		switch (deployState)
		{
		case DeployState.RETRACTED:
			status = cacheAutoLOC_234861;
			SetDragCubes(1f, 0f);
			break;
		case DeployState.EXTENDED:
			status = cacheAutoLOC_234828;
			if (flag)
			{
				CalculateTracking();
			}
			else
			{
				SetDragCubes(0f, 0f);
			}
			break;
		case DeployState.RETRACTING:
			if (flag)
			{
				panelRotationTransform.localRotation = Quaternion.Lerp(panelRotationTransform.localRotation, originalRotation, (1f - anim[animationName].normalizedTime) * TimeWarp.deltaTime);
			}
			if (anim[animationName].normalizedTime <= 0f)
			{
				anim.Stop(animationName);
				if (flag)
				{
					panelRotationTransform.localRotation = originalRotation;
				}
				deployState = DeployState.RETRACTED;
				base.part.ScheduleSetCollisionIgnores();
				base.Events["Extend"].active = true;
				onStop.Fire(0f);
			}
			status = cacheAutoLOC_234856;
			SetDragCubes(1f - anim[animationName].normalizedTime, 0f);
			break;
		case DeployState.EXTENDING:
			if (anim[animationName].normalizedTime >= 1f)
			{
				anim.Stop(animationName);
				deployState = DeployState.EXTENDED;
				base.part.ScheduleSetCollisionIgnores();
				base.Events["Retract"].active = retractable || HighLogic.LoadedSceneIsEditor;
				onStop.Fire(1f);
			}
			status = cacheAutoLOC_234841;
			SetDragCubes(1f - anim[animationName].normalizedTime, 0f);
			break;
		case DeployState.BROKEN:
			if (flag && panelRotationTransform != null)
			{
				panelRotationTransform.gameObject.SetActive(value: false);
			}
			status = cacheAutoLOC_234868;
			SetDragCubes(1f, 0f);
			break;
		}
		if (HighLogic.LoadedSceneIsFlight && deployState != 0 && deployState != DeployState.BROKEN && isBreakable && !base.part.vessel.HoldPhysics)
		{
			string title = cacheAutoLOC_6001017;
			if (base.part.partInfo != null)
			{
				title = base.part.partInfo.title;
			}
			if (ShouldBreakFromPressure())
			{
				breakNotifications(title, Localizer.Format("#autoLOC_6001018", partType));
				breakPanels();
			}
			else if (ShouldBreakFromG())
			{
				breakNotifications(title, Localizer.Format("#autoLOC_6001019", partType));
				breakPanels();
			}
		}
		if (hasPivot && panelRotationTransform != null)
		{
			currentRotation = panelRotationTransform.localRotation;
		}
		if (useAnimation && deployState != DeployState.BROKEN)
		{
			storedAnimationTime = anim[animationName].normalizedTime;
			storedAnimationSpeed = anim[animationName].speed;
		}
		PostFSMUpdate();
	}

	public virtual bool ShouldBreakFromPressure()
	{
		if (!base.part.ShieldedFromAirstream && !(base.vessel == null))
		{
			float num = (float)(base.part.dynamicPressurekPa + base.part.submergedDynamicPressurekPa);
			if (num < windResistance)
			{
				return false;
			}
			if ((!hasPivot || !(panelRotationTransform != null)) && !(panelBreakTransform != null))
			{
				return false;
			}
			float num2 = Mathf.Abs(Vector3.Dot(base.vessel.velocityD.normalized, hasPivot ? panelRotationTransform.forward.normalized : panelBreakTransform.forward.normalized));
			float minAoAForQCheck = MinAoAForQCheck;
			if (num2 < minAoAForQCheck)
			{
				num2 = minAoAForQCheck;
			}
			return num2 * num > windResistance;
		}
		return false;
	}

	public virtual bool ShouldBreakFromG()
	{
		return base.vessel.geeForce > gResistance;
	}

	public virtual void PostCalculateTracking(bool trackingLOS, Vector3 trackingDirection)
	{
	}

	public virtual bool CalculateTrackingLOS(Vector3 trackingDirection, ref string blocker)
	{
		if (base.part.ShieldedFromAirstream && applyShielding)
		{
			blocker = "aero shielding";
			return false;
		}
		return true;
	}

	public virtual void CalculateTracking()
	{
		if (!HighLogic.LoadedSceneIsFlight)
		{
			return;
		}
		if (hasPivot)
		{
			_ = panelRotationTransform.position;
		}
		else
		{
			_ = base.part.partTransform.position;
		}
		Vector3 normalized = (trackingTransformLocal.position - panelRotationTransform.position).normalized;
		trackingLOS = CalculateTrackingLOS(normalized, ref blockingObject);
		if (isTracking)
		{
			Vector3 vector = panelRotationTransform.InverseTransformPoint(trackingTransformLocal.position);
			float y = TrackingAlignmentOffset + Mathf.Atan2(vector.x, vector.z) * 57.29578f;
			Quaternion b = panelRotationTransform.rotation * Quaternion.Euler(0f, y, 0f);
			panelRotationTransform.rotation = Quaternion.Lerp(panelRotationTransform.rotation, b, TimeWarp.deltaTime * trackingSpeed);
		}
		if (UIPartActionController.Instance != null && UIPartActionController.Instance.ItemListContains(base.part, includeSymmetryCounterparts: false))
		{
			if (trackingLOS)
			{
				if (isTracking)
				{
					status = Localizer.Format("#autoLOC_234989", trackingBody.displayName);
				}
			}
			else
			{
				CelestialBody bodyByName = FlightGlobals.GetBodyByName(blockingObject);
				status = Localizer.Format("#autoLOC_234994", (bodyByName != null) ? bodyByName.displayName : blockingObject);
			}
		}
		if (isTracking || useAnimation)
		{
			SetDragCubes(0f, (panelRotationTransform != null) ? Mathf.Abs(Mathf.Sin((float)Math.PI / 180f * panelRotationTransform.rotation.eulerAngles.y)) : 0f);
		}
		PostCalculateTracking(trackingLOS, normalized);
	}

	public virtual void GetTrackingBodyTransforms()
	{
		if (trackingBody != null)
		{
			trackingTransformLocal = trackingBody.transform;
			if ((bool)trackingBody.scaledBody)
			{
				trackingTransformScaled = trackingBody.scaledBody.transform;
			}
		}
		else if (trackingVessel != null)
		{
			trackingTransformLocal = trackingVessel.vesselTransform;
		}
	}

	public virtual void SetScalar(float t)
	{
		if (useAnimation && deployState != DeployState.BROKEN && (HighLogic.LoadedSceneIsEditor || !base.part.ShieldedFromAirstream || (!applyShielding && !applyShieldingExtend)))
		{
			if (t > 0f)
			{
				Extend();
			}
			else
			{
				Retract();
			}
		}
	}

	public virtual void SetUIRead(bool state)
	{
		if (!showStatus)
		{
			base.Fields["status"].guiActive = state;
		}
	}

	public virtual void SetUIWrite(bool state)
	{
		overrideUIWriteState = state;
		base.Events["Extend"].active = useAnimation && state && deployState == DeployState.RETRACTED;
		base.Events["Retract"].active = useAnimation && state && deployState == DeployState.EXTENDED && retractable;
		base.Actions["ExtendPanelsAction"].active = useAnimation && state;
		base.Actions["ExtendAction"].active = useAnimation && state;
		base.Actions["RetractAction"].active = useAnimation && state;
	}

	public virtual bool IsMoving()
	{
		if (useAnimation)
		{
			if (deployState != DeployState.EXTENDING)
			{
				return deployState == DeployState.RETRACTING;
			}
			return true;
		}
		return false;
	}

	public string[] GetDragCubeNames()
	{
		anim = GetComponentInChildren<Animation>();
		if (!(anim == null) && useAnimation)
		{
			if (!isTracking)
			{
				return new string[2] { "RETRACTED", "EXTENDED_A" };
			}
			return new string[3] { "RETRACTED", "EXTENDED_A", "EXTENDED_B" };
		}
		if (isTracking)
		{
			return new string[2] { "EXTENDED_A", "EXTENDED_B" };
		}
		return null;
	}

	public void AssumeDragCubePosition(string name)
	{
		switch (name)
		{
		case "EXTENDED_B":
			deployState = DeployState.EXTENDED;
			if (useAnimation)
			{
				anim[animationName].normalizedTime = 1f;
				anim[animationName].normalizedSpeed = 0f;
				anim[animationName].enabled = true;
				anim[animationName].speed = 0f;
				anim[animationName].weight = 1f;
				anim.Play(animationName);
			}
			if (panelRotationTransform != null)
			{
				panelRotationTransform.rotation *= Quaternion.Euler(0f, 90f, 0f);
			}
			break;
		case "EXTENDED_A":
			deployState = DeployState.EXTENDED;
			if (useAnimation)
			{
				anim[animationName].normalizedTime = 1f;
				anim[animationName].normalizedSpeed = 0f;
				anim[animationName].enabled = true;
				anim[animationName].speed = 0f;
				anim[animationName].weight = 1f;
				anim.Play(animationName);
			}
			if (panelRotationTransform != null)
			{
				panelRotationTransform.rotation *= Quaternion.Euler(0f, 0f, 0f);
			}
			break;
		case "RETRACTED":
			deployState = DeployState.RETRACTED;
			if (useAnimation)
			{
				anim[animationName].normalizedTime = 0f;
				anim[animationName].normalizedSpeed = 0f;
				anim[animationName].enabled = true;
				anim[animationName].weight = 1f;
				anim.Play(animationName);
			}
			break;
		}
	}

	public bool UsesProceduralDragCubes()
	{
		return false;
	}

	public void SetDragCubes(float retracted, float angle)
	{
		base.part.DragCubes.SetCubeWeight("RETRACTED", retracted);
		if (isTracking)
		{
			base.part.DragCubes.SetCubeWeight("EXTENDED_A", (1f - retracted) * angle);
			base.part.DragCubes.SetCubeWeight("EXTENDED_B", (1f - retracted) * (1f - angle));
		}
		else
		{
			base.part.DragCubes.SetCubeWeight("EXTENDED_A", 1f - retracted);
		}
	}

	public override void OnModuleAdjusterAdded(AdjusterPartModuleBase adjuster)
	{
		if (adjuster is AdjusterDeployablePartBase item)
		{
			adjusterCache.Add(item);
		}
		base.OnModuleAdjusterAdded(adjuster);
	}

	public override void OnModuleAdjusterRemoved(AdjusterPartModuleBase adjuster)
	{
		AdjusterDeployablePartBase item = adjuster as AdjusterDeployablePartBase;
		adjusterCache.Remove(item);
		base.OnModuleAdjusterRemoved(adjuster);
	}

	public bool IsDeployablePartStuck()
	{
		int num = 0;
		while (true)
		{
			if (num < adjusterCache.Count)
			{
				if (adjusterCache[num].IsDeployablePartStuck())
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

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLoc_6003036");
	}

	public bool CanBeDetached()
	{
		return AllowConstructionDeployState();
	}

	public bool CanBeOffset()
	{
		return AllowConstructionDeployState();
	}

	public bool CanBeRotated()
	{
		return AllowConstructionDeployState();
	}

	public virtual bool AllowConstructionDeployState()
	{
		if (deployState != DeployState.BROKEN && deployState != DeployState.EXTENDING && deployState != DeployState.RETRACTING)
		{
			return true;
		}
		return false;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_234828 = Localizer.Format("#autoLOC_234828");
		cacheAutoLOC_234841 = Localizer.Format("#autoLOC_234841");
		cacheAutoLOC_234856 = Localizer.Format("#autoLOC_234856");
		cacheAutoLOC_234861 = Localizer.Format("#autoLOC_234861");
		cacheAutoLOC_234868 = Localizer.Format("#autoLOC_234868");
		cacheAutoLOC_6001017 = Localizer.Format("#autoLOC_6001017");
		cacheAutoLOC_6005093 = Localizer.Format("#autoLOC_6005093");
	}
}

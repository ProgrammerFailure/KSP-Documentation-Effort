using ns9;
using UnityEngine;

public class ModuleAnimateGeneric : PartModule, IScalarModule, IMultipleDragCube
{
	public enum animationStates
	{
		LOCKED,
		MOVING,
		CLAMPED,
		FIXED
	}

	[KSPField]
	public bool instantAnimInEditor = true;

	[KSPField]
	public string animationName = "default";

	[KSPField]
	public int CrewCapacity;

	[KSPField]
	public int layer = 1;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001352")]
	public string status = Localizer.Format("#autoLOC_215362");

	[KSPField]
	public bool showStatus = true;

	[KSPField(isPersistant = true)]
	public animationStates aniState;

	[KSPField(isPersistant = true)]
	public bool animSwitch = true;

	[KSPField(isPersistant = true)]
	public float animTime;

	[KSPField(isPersistant = true)]
	public float animSpeed = 1f;

	public float revertAnimSpeedOnSave = 1f;

	[KSPField]
	public bool isOneShot;

	[KSPField]
	public bool actionAvailable = true;

	[KSPField]
	public bool eventAvailableEditor = true;

	[KSPField]
	public bool eventAvailableFlight = true;

	[KSPField]
	public bool eventAvailableEVA = true;

	[KSPField]
	public float evaDistance = 5f;

	[KSPField]
	public string startEventGUIName = "#autoLOC_6001354";

	[KSPField]
	public string endEventGUIName = "#autoLOC_6001354";

	[KSPField]
	public string actionGUIName = "#autoLOC_6001354";

	[KSPField]
	public KSPActionGroup defaultActionGroup;

	[KSPField]
	public bool allowManualControl = true;

	public bool originalAllowManualControl = true;

	[KSPField]
	public bool allowAnimationWhileShielded = true;

	[UI_FloatRange(minValue = 0f, stepIncrement = 1f, maxValue = 100f)]
	[KSPAxisField(minValue = 0f, incrementalSpeed = 20f, isPersistant = true, axisMode = KSPAxisMode.Incremental, maxValue = 100f, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_6001353")]
	public float deployPercent = 100f;

	public float revertDeployPercentOnSave = -1f;

	public bool originalDeployPercentGUIActive = true;

	[KSPField]
	public bool revClampDirection;

	[KSPField]
	public bool revClampSpeed;

	[KSPField]
	public bool revClampPercent;

	[KSPField]
	public bool allowDeployLimit;

	[KSPField]
	public string restrictedNode = "";

	[KSPField]
	public bool disableAfterPlaying;

	[KSPField(isPersistant = true)]
	public bool animationIsDisabled;

	[KSPField]
	public bool useMultipleDragCubes = true;

	public bool animationIsDisabledByVariant;

	public bool stopAnimation;

	public bool isManuallyMoving;

	public Animation anim;

	public bool needsColliderFlagsReset;

	public float scalarLerpMaxDelta;

	public EventData<float, float> onMove;

	public EventData<float> onStop;

	public BaseEvent toggleEvt;

	public BaseAction toggleAct;

	public float lastClamp;

	[KSPField]
	public string moduleID = "genericAnim";

	public static string cacheAutoLOC_215506;

	public static string cacheAutoLOC_215639;

	public static string cacheAutoLOC_215714;

	public static string cacheAutoLOC_215719;

	public static string cacheAutoLOC_215760;

	public float Progress => animTime;

	public bool AnimationIsDisabledByVariant => animationIsDisabledByVariant;

	public string ScalarModuleID => moduleID;

	public float GetScalar => animTime;

	public bool CanMove
	{
		get
		{
			if (!HighLogic.LoadedSceneIsEditor && !allowAnimationWhileShielded)
			{
				return !base.part.ShieldedFromAirstream;
			}
			return true;
		}
	}

	public EventData<float, float> OnMoving => onMove;

	public EventData<float> OnStop => onStop;

	public bool IsMultipleCubesActive => useMultipleDragCubes;

	[KSPAction("#autoLOC_6001329", KSPActionGroup.REPLACEWITHDEFAULT)]
	public void ToggleAction(KSPActionParam param)
	{
		if (allowManualControl)
		{
			Toggle();
		}
	}

	public AnimationState GetState()
	{
		return anim[animationName];
	}

	public Animation GetAnimation()
	{
		return anim;
	}

	[KSPEvent(unfocusedRange = 5f, guiActiveUnfocused = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001329")]
	public void Toggle()
	{
		if (allowManualControl && (HighLogic.LoadedSceneIsEditor || !base.part.ShieldedFromAirstream || allowAnimationWhileShielded))
		{
			animSwitch = !animSwitch;
			UpdateAnimSwitch();
			if (HighLogic.LoadedSceneIsFlight && !animationIsDisabled && disableAfterPlaying && !animSwitch)
			{
				animationIsDisabled = true;
			}
		}
	}

	public void UpdateAnimSwitch()
	{
		if (anim == null)
		{
			FindAnimation();
		}
		if (animSwitch)
		{
			anim[animationName].speed = ((!instantAnimInEditor || !HighLogic.LoadedSceneIsEditor) ? (-1f) : (-10f * anim[animationName].length));
			anim.Play(animationName);
			toggleEvt.guiName = startEventGUIName;
			onMove.Fire(1f, 0f);
		}
		else
		{
			anim[animationName].speed = ((!instantAnimInEditor || !HighLogic.LoadedSceneIsEditor) ? 1f : (10f * anim[animationName].length));
			anim.Play(animationName);
			toggleEvt.guiName = endEventGUIName;
			onMove.Fire(0f, 1f);
		}
		aniState = animationStates.MOVING;
		status = cacheAutoLOC_215506;
	}

	public override void OnAwake()
	{
		toggleEvt = base.Events["Toggle"];
		toggleAct = base.Actions["ToggleAction"];
		if (toggleAct.actionGroup == KSPActionGroup.REPLACEWITHDEFAULT)
		{
			toggleAct.actionGroup = defaultActionGroup;
		}
		if (toggleAct.defaultActionGroup == KSPActionGroup.REPLACEWITHDEFAULT)
		{
			toggleAct.defaultActionGroup = defaultActionGroup;
		}
		onMove = new EventData<float, float>(base.part.partName + "_" + base.part.flightID + "_" + base.part.Modules.IndexOf(this) + "_onMove");
		onStop = new EventData<float>(base.part.partName + "_" + base.part.flightID + "_" + base.part.Modules.IndexOf(this) + "_onStop");
	}

	public override void OnStart(StartState state)
	{
		if (anim == null)
		{
			FindAnimation();
		}
		if (!base.enabled)
		{
			return;
		}
		toggleAct.guiName = actionGUIName;
		if (animSwitch)
		{
			toggleEvt.guiName = startEventGUIName;
		}
		else
		{
			toggleEvt.guiName = endEventGUIName;
		}
		anim[animationName].speed = 0f;
		anim[animationName].enabled = true;
		anim[animationName].weight = 1f;
		anim[animationName].layer = layer;
		if (isOneShot && aniState == animationStates.FIXED)
		{
			anim[animationName].normalizedTime = 1f;
			SetDragState(1f);
		}
		else
		{
			anim[animationName].normalizedTime = animTime;
			SetDragState(animTime);
			if (aniState == animationStates.MOVING)
			{
				anim[animationName].speed = animSpeed;
				anim.Play(animationName);
				onMove.Fire(animTime, (animSpeed > 0f) ? 1f : 0f);
			}
		}
		base.part.ScheduleSetCollisionIgnores();
		scalarLerpMaxDelta = 1f / anim[animationName].length;
		toggleAct.active = actionAvailable;
		toggleEvt.guiActive = eventAvailableFlight;
		toggleEvt.guiActiveEditor = eventAvailableEditor;
		toggleEvt.guiActiveUnfocused = eventAvailableEVA;
		toggleEvt.unfocusedRange = evaDistance;
		base.Fields["deployPercent"].guiActiveEditor = allowDeployLimit && eventAvailableEditor;
		base.Fields["deployPercent"].guiActive = allowDeployLimit && eventAvailableFlight;
		lastClamp = deployPercent;
		base.Fields["status"].guiActive = showStatus;
		status = Localizer.Format(status);
		if (!allowManualControl)
		{
			toggleAct.active = false;
			toggleEvt.guiActive = false;
		}
		CheckCrewState();
	}

	public void FindAnimation()
	{
		Animation[] array = base.part.FindModelAnimators(animationName);
		if (array.Length != 0)
		{
			anim = array[0];
		}
		if (anim == null)
		{
			Debug.Log("ModuleAnimateGeneric: Could not find animation component '" + animationName + "' - on part " + base.part.name + " failed to load! Check the model file");
			base.enabled = false;
		}
		else if (anim[animationName] == null)
		{
			Debug.Log("ModuleAnimateGeneric: Could not find animation " + animationName + " in part's animation component. Check the animationName and model file");
			base.enabled = false;
		}
	}

	public override void OnInventoryModeDisable()
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			lastClamp = deployPercent;
		}
		if (anim == null)
		{
			FindAnimation();
		}
		if (anim != null && anim[animationName] != null)
		{
			anim[animationName].normalizedTime = animTime;
			anim[animationName].speed = 0f;
		}
		if (base.part.protoPartSnapshot != null)
		{
			ProtoPartModuleSnapshot protoPartModuleSnapshot = base.part.protoPartSnapshot.FindModule("ModuleAnimateGeneric");
			if (protoPartModuleSnapshot != null && protoPartModuleSnapshot.moduleValues != null)
			{
				protoPartModuleSnapshot.moduleValues.SetValue("animTime", animTime.ToString());
				protoPartModuleSnapshot.moduleValues.SetValue("animSwitch", animSwitch.ToString());
			}
		}
		if (toggleEvt != null)
		{
			toggleEvt.guiName = startEventGUIName;
		}
	}

	public override void OnStoredInInventory(ModuleInventoryPart moduleInventoryPart)
	{
		base.OnStoredInInventory(moduleInventoryPart);
		animSwitch = true;
		animTime = 0f;
	}

	public void CheckForRestrictions()
	{
		bool flag = false;
		bool flag2 = false;
		if (!string.IsNullOrEmpty(restrictedNode))
		{
			flag = base.part.FindAttachNode(restrictedNode).attachedPart != null;
		}
		if (CrewCapacity > 0 && HighLogic.LoadedSceneIsFlight && base.part.protoModuleCrew.Count > 0)
		{
			flag2 = true;
		}
		toggleEvt.active = !animationIsDisabled && !animationIsDisabledByVariant && aniState != animationStates.MOVING && !flag && !flag2;
		toggleAct.active = !animationIsDisabled && !animationIsDisabledByVariant && aniState != animationStates.MOVING && !flag2;
	}

	public void FixedUpdate()
	{
		if (anim == null || anim[animationName] == null)
		{
			return;
		}
		CheckForRestrictions();
		float num = deployPercent * 0.01f;
		if (num > 0.995f)
		{
			num = 1f;
		}
		if (revClampPercent)
		{
			num = 1f - num;
		}
		if (Mathf.Abs(lastClamp - deployPercent) > 0.1f)
		{
			lastClamp = deployPercent;
			float num3 = (anim[animationName].normalizedTime = num);
			animTime = num3;
			anim[animationName].speed = 0f;
			animSpeed = anim[animationName].speed;
			anim.Play(animationName);
			aniState = animationStates.CLAMPED;
			status = cacheAutoLOC_215639;
			SetDragState(animTime);
			if (revClampDirection)
			{
				if (deployPercent > 0f)
				{
					animSwitch = false;
					toggleEvt.guiName = endEventGUIName;
				}
				else
				{
					animSwitch = true;
					toggleEvt.guiName = startEventGUIName;
				}
				onStop.Fire(1f - deployPercent);
			}
			else
			{
				if (deployPercent == 0f)
				{
					animSwitch = false;
					toggleEvt.guiName = endEventGUIName;
				}
				else
				{
					animSwitch = true;
					toggleEvt.guiName = startEventGUIName;
				}
				onStop.Fire(deployPercent);
			}
			return;
		}
		if (allowDeployLimit)
		{
			bool flag = false;
			bool flag2 = false;
			if (animSpeed == 0f && anim.IsPlaying(animationName))
			{
				anim[animationName].normalizedTime = num;
				animTime = anim[animationName].normalizedTime;
			}
			if (revClampDirection && animTime >= num)
			{
				flag = true;
			}
			if (!revClampDirection && animTime <= num)
			{
				flag = true;
			}
			if (revClampSpeed && anim[animationName].speed < 0f)
			{
				flag2 = true;
			}
			if (!revClampSpeed && anim[animationName].speed > 0f)
			{
				flag2 = true;
			}
			if (flag && flag2)
			{
				anim[animationName].speed = 0f;
			}
		}
		AnimationHandler();
	}

	public void AnimationHandler()
	{
		if (aniState == animationStates.CLAMPED)
		{
			float num = deployPercent / 100f;
			if (revClampPercent)
			{
				num = 1f - num;
			}
			if (anim[animationName].normalizedTime != num)
			{
				aniState = animationStates.MOVING;
			}
			status = cacheAutoLOC_215506;
		}
		if (aniState == animationStates.MOVING)
		{
			if (!anim.IsPlaying(animationName))
			{
				if (isOneShot)
				{
					aniState = animationStates.FIXED;
					status = cacheAutoLOC_215714;
				}
				else
				{
					aniState = animationStates.LOCKED;
					status = cacheAutoLOC_215719;
				}
				if (!animSwitch)
				{
					anim[animationName].normalizedTime = 1f;
					animTime = 1f;
					SetDragState(1f);
				}
				else
				{
					anim[animationName].normalizedTime = 0f;
					animTime = 0f;
					SetDragState(0f);
				}
				base.part.SetCollisionIgnores();
				onStop.Fire(animTime);
			}
			else
			{
				animSpeed = anim[animationName].speed;
				animTime = anim[animationName].normalizedTime;
				SetDragState(animTime);
			}
		}
		if (stopAnimation)
		{
			anim.Stop(animationName);
			base.part.ScheduleSetCollisionIgnores();
			stopAnimation = false;
			onStop.Fire(anim[animationName].normalizedTime);
		}
		if (aniState != 0 && animSpeed == 0f && anim.IsPlaying(animationName) && !isManuallyMoving)
		{
			status = cacheAutoLOC_215760;
			aniState = animationStates.CLAMPED;
		}
		CheckCrewState();
	}

	public void SetScalar(float t)
	{
		if (isOneShot && aniState == animationStates.FIXED)
		{
			animTime = t;
			return;
		}
		if (anim.IsPlaying(animationName))
		{
			anim.Stop(animationName);
			base.part.ScheduleSetCollisionIgnores();
			onStop.Fire(anim[animationName].normalizedTime);
		}
		anim[animationName].speed = 0f;
		anim[animationName].enabled = true;
		anim[animationName].weight = 1f;
		base.part.ScheduleSetCollisionIgnores();
		if (!HighLogic.LoadedSceneIsEditor && base.part.ShieldedFromAirstream && !allowAnimationWhileShielded)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_215802", base.part.partInfo.title), 5f, ScreenMessageStyle.UPPER_LEFT);
			return;
		}
		t = Mathf.MoveTowards(animTime, t, scalarLerpMaxDelta * Time.deltaTime);
		anim[animationName].normalizedTime = t;
		animTime = t;
		if (Mathf.Abs(t - 1f) < 0.01f)
		{
			isManuallyMoving = false;
			if (isOneShot)
			{
				aniState = animationStates.FIXED;
				status = cacheAutoLOC_215714;
				return;
			}
			animSwitch = false;
			aniState = animationStates.LOCKED;
			status = cacheAutoLOC_215719;
			toggleEvt.guiName = endEventGUIName;
		}
		else if (Mathf.Abs(t) < 0.01f)
		{
			isManuallyMoving = false;
			animSwitch = true;
			aniState = animationStates.LOCKED;
			status = cacheAutoLOC_215719;
			toggleEvt.guiName = startEventGUIName;
			toggleEvt.active = true;
		}
		else
		{
			aniState = animationStates.MOVING;
			isManuallyMoving = true;
			status = cacheAutoLOC_215506;
			animSwitch = t < animTime;
			toggleEvt.active = false;
		}
	}

	public void SetUIRead(bool state)
	{
		base.Fields["status"].guiActive = state && showStatus;
	}

	public void SetUIWrite(bool state)
	{
		toggleEvt.guiActive = state && allowManualControl;
		toggleEvt.guiActiveUnfocused = state && allowManualControl;
	}

	public bool IsMoving()
	{
		if (anim.isPlaying)
		{
			return Mathf.Abs(anim[animationName].speed) != 0f;
		}
		return false;
	}

	public void SetDragState(float b)
	{
		base.part.DragCubes.SetCubeWeight("A", b);
		base.part.DragCubes.SetCubeWeight("B", 1f - b);
		if (base.part.DragCubes.Procedural)
		{
			base.part.DragCubes.ForceUpdate(weights: true, occlusion: true);
		}
	}

	public string[] GetDragCubeNames()
	{
		return new string[2] { "A", "B" };
	}

	public void AssumeDragCubePosition(string name)
	{
		if (base.part.FindModelAnimators(animationName).Length != 0)
		{
			anim = base.part.FindModelAnimators(animationName)[0];
		}
		if (anim == null)
		{
			Debug.Log("ModuleAnimateGeneric: Could not find animation component - on part " + base.part.name + " failed to load! Check the model file");
			base.enabled = false;
			return;
		}
		if (anim[animationName] == null)
		{
			Debug.Log("ModuleAnimateGeneric: Could not find animation " + animationName + " in part's animation component. Check the animationName and model file");
			base.enabled = false;
			return;
		}
		anim[animationName].speed = 0f;
		anim[animationName].enabled = true;
		anim[animationName].weight = 1f;
		if (!(name == "A"))
		{
			if (name == "B")
			{
				anim[animationName].normalizedTime = 0f;
			}
		}
		else
		{
			anim[animationName].normalizedTime = 1f;
		}
	}

	public bool UsesProceduralDragCubes()
	{
		return false;
	}

	public override void OnSave(ConfigNode node)
	{
		base.OnSave(node);
		if (node.HasValue("deployPercent") && revertDeployPercentOnSave >= 0f)
		{
			node.SetValue("deployPercent", revertDeployPercentOnSave);
			node.SetValue("animSpeed", revertAnimSpeedOnSave);
			aniState = animationStates.MOVING;
			node.SetValue("aniState", aniState.ToString());
		}
	}

	public void CheckCrewState()
	{
		if (CrewCapacity > 0)
		{
			int crewCapacity = base.part.CrewCapacity;
			bool flag;
			if ((flag = Mathf.Approximately(anim[animationName].normalizedTime, 1f)) && base.part.CrewCapacity == 0)
			{
				base.part.CrewCapacity = CrewCapacity;
			}
			else if (!flag && base.part.CrewCapacity > 0)
			{
				base.part.CrewCapacity = 0;
			}
			if (crewCapacity != base.part.CrewCapacity)
			{
				base.part.CheckTransferDialog();
				MonoUtilities.RefreshPartContextWindow(base.part);
			}
		}
	}

	public void VariantToggleAnimationDisabled(bool disabled)
	{
		animationIsDisabledByVariant = disabled;
	}

	public void FailAnimationForCargoBay(float newDeployPercent, bool freezeMidMovement)
	{
		if (freezeMidMovement)
		{
			revertDeployPercentOnSave = newDeployPercent;
			revertAnimSpeedOnSave = animSpeed;
		}
		originalAllowManualControl = allowManualControl;
		allowManualControl = false;
		originalDeployPercentGUIActive = base.Fields["deployPercent"].guiActive;
		base.Fields["deployPercent"].guiActive = false;
	}

	public void RestartFailedAnimationForCargoBay()
	{
		if (revertDeployPercentOnSave > 0f)
		{
			if (animSwitch && revertAnimSpeedOnSave > 0f && deployPercent > 0f)
			{
				animSwitch = false;
				toggleEvt.guiName = endEventGUIName;
			}
			else if (!revClampPercent && !animSwitch && revertAnimSpeedOnSave < 0f && deployPercent == 0f)
			{
				animSwitch = true;
				toggleEvt.guiName = startEventGUIName;
			}
			deployPercent = revertDeployPercentOnSave;
			lastClamp = deployPercent;
			animSpeed = revertAnimSpeedOnSave;
			anim[animationName].speed = animSpeed;
			aniState = animationStates.MOVING;
			revertDeployPercentOnSave = -1f;
		}
		allowManualControl = originalAllowManualControl;
		base.Fields["deployPercent"].guiActive = originalDeployPercentGUIActive;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_215506 = Localizer.Format("#autoLOC_215506");
		cacheAutoLOC_215639 = Localizer.Format("#autoLOC_215639");
		cacheAutoLOC_215714 = Localizer.Format("#autoLOC_215714");
		cacheAutoLOC_215719 = Localizer.Format("#autoLOC_215719");
		cacheAutoLOC_215760 = Localizer.Format("#autoLOC_215760");
	}
}

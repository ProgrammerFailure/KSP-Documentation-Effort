using System;
using System.Collections.Generic;
using ns9;
using UnityEngine;

namespace ModuleWheels;

public class ModuleWheelDeployment : ModuleWheelSubmodule, IMultipleDragCube
{
	[UI_Toggle(disabledText = "#autoLOC_6001071", invertButton = true, enabledText = "#autoLOC_6001072", affectSymCounterparts = UI_Scene.All)]
	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001460")]
	public bool shieldedCanDeploy;

	[KSPField(isPersistant = true, guiActive = true, guiName = "#autoLOC_6001372")]
	public string stateDisplayString = "";

	[KSPField(isPersistant = true)]
	public string stateString = "";

	[KSPField]
	public string animationTrfName = "";

	[KSPField]
	public string animationStateName = "";

	[KSPField]
	public float deployedPosition;

	public float retractedPosition;

	[KSPField]
	public float extendDurationFactor = 1f;

	[KSPField]
	public float retractDurationFactor = 1f;

	[KSPField]
	public float TsubSys = 0.3f;

	[KSPField]
	public string deployTargetTransformName = "";

	[KSPField]
	public string retractTransformName = "";

	[KSPField]
	public string fxDeploy = string.Empty;

	[KSPField]
	public string fxDeployed = string.Empty;

	[KSPField]
	public string fxRetract = string.Empty;

	[KSPField]
	public string fxRetracted = string.Empty;

	public bool retractStarted;

	public BaseField fieldState;

	public BaseEvent eventToggle;

	public BaseEvent eventInitialStateToggle;

	[NonSerialized]
	public Animation anim;

	[NonSerialized]
	public AnimationState animationState;

	[NonSerialized]
	public Transform deployTgt;

	[NonSerialized]
	public Transform retractTransform;

	public KerbalFSM fsm;

	public float position;

	public float animLength = 1f;

	[NonSerialized]
	public List<IScalarModule> slaveModules;

	public int[] slaveModuleIndices;

	public WheelSubsystem deploymentSubsystems;

	public bool subsystemsDisabled;

	[SerializeField]
	public SphereCollider standInCollider;

	[KSPField]
	public bool useStandInCollider;

	public ModuleLight lightModule;

	public KFSMState st_deployed;

	public KFSMState st_retracted;

	public KFSMState st_deploying;

	public KFSMState st_retracting;

	public KFSMState st_inoperable;

	public KFSMEvent on_deploy;

	public KFSMEvent on_deployed;

	public KFSMEvent on_retract;

	public KFSMEvent on_retracted;

	public KFSMEvent on_inoperable;

	public static string cacheAutoLOC_6002269;

	public static string cacheAutoLOC_6002270;

	public static string cacheAutoLOC_234856;

	public static string cacheAutoLOC_234861;

	public static string cacheAutoLOC_7003226;

	public static string cacheAutoLOC_247601;

	public static string cacheAutoLOC_6002271;

	public static string cacheAutoLOC_7003224;

	public static string cacheAutoLOC_7003225;

	public float Position
	{
		get
		{
			if (deployedPosition != 0f)
			{
				return position;
			}
			return 1f - position;
		}
	}

	public bool IsMultipleCubesActive => true;

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("position"))
		{
			position = float.Parse(node.GetValue("position"));
		}
		lightModule = base.part.FindModuleImplementing<ModuleLight>();
		if (node.HasValue("slaveModules"))
		{
			slaveModuleIndices = KSPUtil.ParseArray(node.GetValue("slaveModules"), int.Parse);
		}
	}

	public override void OnSave(ConfigNode node)
	{
		if (animationState != null)
		{
			position = Mathf.Clamp01(animationState.normalizedTime);
			node.AddValue("position", position);
		}
	}

	public override void OnStart(StartState state)
	{
		base.OnStart(state);
		fieldState = base.Fields["stateDisplayString"];
		fieldState.guiActive = HighLogic.LoadedSceneIsFlight;
		eventToggle = base.Events["EventToggle"];
		eventToggle.guiName = GetToggleEventName();
		fsm = new KerbalFSM();
		if (string.IsNullOrEmpty(animationTrfName))
		{
			Debug.LogError("[ModuleWheelDeployment]: Animation transform name is undefined", base.gameObject);
			return;
		}
		if (string.IsNullOrEmpty(animationStateName))
		{
			Debug.LogError("[ModuleWheelDeployment]: Animation state name is undefined", base.gameObject);
			return;
		}
		anim = base.part.FindModelAnimator(animationTrfName, animationStateName);
		if (anim == null)
		{
			Debug.LogError("[ModuleWheelDeployment]: No animation found with clip" + animationStateName + " on transform " + animationTrfName + " at " + base.part.partName + ".", base.gameObject);
			return;
		}
		if (!string.IsNullOrEmpty(deployTargetTransformName))
		{
			deployTgt = base.part.FindModelTransform(deployTargetTransformName);
			if (deployTgt == null)
			{
				Debug.LogError("[ModuleWheelDeployment]: No transform found with id " + deployTargetTransformName + " on " + base.part.partName + ".", base.gameObject);
				return;
			}
		}
		if (!string.IsNullOrEmpty(retractTransformName))
		{
			retractTransform = base.part.FindModelTransform(retractTransformName);
			if (retractTransform == null)
			{
				Debug.LogError("[ModuleWheelDeployment]: No transform found with id " + retractTransformName + " on " + base.part.partName + ".", base.gameObject);
				return;
			}
		}
		if (slaveModuleIndices != null)
		{
			if (slaveModules == null)
			{
				slaveModules = new List<IScalarModule>();
			}
			else
			{
				slaveModules.Clear();
			}
			int count = base.part.Modules.Count;
			while (count-- > 0)
			{
				if (!(base.part.Modules[count] is IScalarModule item))
				{
					continue;
				}
				int num = slaveModuleIndices.Length;
				while (num-- > 0)
				{
					if (slaveModuleIndices[num] == count)
					{
						slaveModules.Add(item);
						break;
					}
				}
			}
		}
		deploymentSubsystems = new WheelSubsystem("Wheel Retracted", WheelSubsystem.SystemTypes.All, this);
		anim.animatePhysics = true;
		anim.Stop();
		animationState = anim[animationStateName];
		animLength = (HighLogic.LoadedSceneIsFlight ? animationState.length : 0f);
		animationState.wrapMode = WrapMode.ClampForever;
		animationState.enabled = true;
		animationState.weight = 1f;
		animationState.speed = 0f;
		position = Mathf.Clamp01(position);
		retractedPosition = 1f - deployedPosition;
		if (standInCollider != null)
		{
			UnityEngine.Object.Destroy(standInCollider.gameObject);
			standInCollider = null;
		}
		if (stateString != string.Empty)
		{
			animationState.normalizedTime = position;
			FSMSetup(stateString);
		}
		else
		{
			animationState.normalizedTime = deployedPosition;
			FSMSetup("Deployed");
		}
		if (HighLogic.LoadedScene == GameScenes.FLIGHT && base.vessel.situation == Vessel.Situations.PRELAUNCH && position == deployedPosition)
		{
			base.vessel.ActionGroups[KSPActionGroup.Gear] = true;
		}
		lightModule = base.part.FindModuleImplementing<ModuleLight>();
	}

	public override void OnInventoryModeDisable()
	{
		if (anim == null)
		{
			anim = base.part.FindModelAnimator(animationTrfName, animationStateName);
		}
		if (!anim)
		{
			return;
		}
		animationState = anim[animationStateName];
		if (!animationState)
		{
			return;
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			float normalizedTime = 0f;
			string text = stateString;
			if (!(text == "Retracted"))
			{
				if (text == "Deployed")
				{
					normalizedTime = deployedPosition;
				}
			}
			else
			{
				normalizedTime = 1f - deployedPosition;
			}
			animationState.enabled = true;
			animationState.weight = 1f;
			animationState.speed = 0f;
			animationState.normalizedTime = normalizedTime;
			if (base.part.protoPartSnapshot != null)
			{
				ProtoPartModuleSnapshot protoPartModuleSnapshot = base.part.protoPartSnapshot.FindModule("ModuleWheelDeployment");
				if (protoPartModuleSnapshot != null && protoPartModuleSnapshot.moduleValues != null)
				{
					protoPartModuleSnapshot.moduleValues.SetValue("stateString", stateString);
				}
			}
		}
		if (HighLogic.LoadedSceneIsEditor)
		{
			anim.Play();
			animationState.speed = float.MaxValue;
		}
	}

	public override void OnStoredInInventory(ModuleInventoryPart moduleInventoryPart)
	{
		base.OnStoredInInventory(moduleInventoryPart);
		if (base.part.protoPartSnapshot != null)
		{
			stateString = "Retracted";
			ProtoPartModuleSnapshot protoPartModuleSnapshot = base.part.protoPartSnapshot.FindModule("ModuleWheelDeployment");
			if (protoPartModuleSnapshot != null && protoPartModuleSnapshot.moduleValues != null)
			{
				protoPartModuleSnapshot.moduleValues.SetValue("stateString", stateString);
			}
		}
	}

	public override void OnWheelSetup()
	{
		SetInopSubsystems(Position < TsubSys);
		if (deployTgt != null)
		{
			wheel.SetWheelTransform(deployTgt.position, deployTgt.rotation);
			if (standInCollider != null)
			{
				standInCollider.transform.position = deployTgt.position;
			}
		}
	}

	public void SetInopSubsystems(bool disable)
	{
		if (disable && !subsystemsDisabled)
		{
			if (slaveModules != null)
			{
				int count = slaveModules.Count;
				while (count-- > 0)
				{
					slaveModules[count].SetScalar(0f);
					slaveModules[count].SetUIWrite(state: false);
				}
			}
			wheelBase.InopSystems.AddSubsystem(deploymentSubsystems);
			if (HighLogic.LoadedSceneIsFlight && useStandInCollider && standInCollider == null)
			{
				standInCollider = CreateStandInCollider(wheel);
			}
		}
		if (!disable && subsystemsDisabled)
		{
			if (slaveModules != null)
			{
				int count2 = slaveModules.Count;
				while (count2-- > 0)
				{
					slaveModules[count2].SetUIWrite(state: true);
				}
			}
			wheelBase.InopSystems.RemoveSubsystem(deploymentSubsystems);
			if (standInCollider != null)
			{
				UnityEngine.Object.Destroy(standInCollider.gameObject);
				standInCollider = null;
			}
		}
		subsystemsDisabled = disable;
	}

	public SphereCollider CreateStandInCollider(KSPWheelController w)
	{
		GameObject obj = new GameObject("StandIn Collider");
		obj.transform.SetParent(base.part.partTransform.Find("model"), worldPositionStays: false);
		obj.transform.position = deployTgt.position;
		SphereCollider sphereCollider = obj.AddComponent<SphereCollider>();
		sphereCollider.radius = w.wheelCollider.radius;
		base.part.ResetCollisionIgnores();
		return sphereCollider;
	}

	public void FSMSetup(string startStateName)
	{
		st_deployed = new KFSMState("Deployed");
		st_deployed.OnEnter = delegate
		{
			position = deployedPosition;
			animationState.speed = 0f;
			animationState.normalizedTime = deployedPosition;
			eventToggle.guiName = GetToggleEventName();
			eventToggle.active = true;
			if (deployTgt != null && wheel != null)
			{
				wheel.SetWheelTransform(deployTgt.position, deployTgt.rotation);
				if (standInCollider != null)
				{
					standInCollider.transform.position = deployTgt.position;
				}
			}
			base.part.DragCubes.SetCubeWeight("Retracted", 0f);
			base.part.DragCubes.SetCubeWeight("Deployed", 1f);
		};
		st_deployed.OnUpdate = delegate
		{
		};
		st_deployed.OnLeave = delegate
		{
		};
		fsm.AddState(st_deployed);
		st_retracting = new KFSMState("Retracting...");
		st_retracting.OnEnter = delegate
		{
			eventToggle.guiName = GetToggleEventName();
			eventToggle.active = true;
			retractStarted = true;
		};
		st_retracting.OnFixedUpdate = delegate
		{
			if (retractStarted)
			{
				retractStarted = false;
				if (deployedPosition == 0f)
				{
					animationState.speed = retractDurationFactor;
				}
				else
				{
					animationState.speed = 0f - retractDurationFactor;
				}
				if (retractTransform != null)
				{
					animationState.normalizedTime = FindTransformAnimationTime(retractTransform);
				}
			}
			position = Mathf.Clamp01(animationState.normalizedTime);
			if (deployTgt != null && wheel != null)
			{
				wheel.SetWheelTransform(deployTgt.position, deployTgt.rotation);
				if (standInCollider != null)
				{
					standInCollider.transform.position = deployTgt.position;
				}
			}
			base.part.DragCubes.SetCubeWeight("Retracted", 1f - Position);
			base.part.DragCubes.SetCubeWeight("Deployed", Position);
			if (baseSetup)
			{
				SetInopSubsystems(Position < TsubSys);
			}
		};
		st_retracting.OnLeave = delegate
		{
		};
		fsm.AddState(st_retracting);
		st_retracted = new KFSMState("Retracted");
		st_retracted.OnEnter = delegate
		{
			position = retractedPosition;
			animationState.speed = 0f;
			animationState.normalizedTime = retractedPosition;
			eventToggle.guiName = GetToggleEventName();
			eventToggle.active = true;
			if (deployTgt != null && wheel != null)
			{
				wheel.SetWheelTransform(deployTgt.position, deployTgt.rotation);
				if (standInCollider != null)
				{
					standInCollider.transform.position = deployTgt.position;
				}
			}
			if (lightModule != null && lightModule.isOn)
			{
				lightModule.SetFlareColor(Color.black);
			}
			base.part.DragCubes.SetCubeWeight("Retracted", 1f);
			base.part.DragCubes.SetCubeWeight("Deployed", 0f);
		};
		st_retracted.OnUpdate = delegate
		{
		};
		st_retracted.OnLeave = delegate
		{
		};
		fsm.AddState(st_retracted);
		st_deploying = new KFSMState("Deploying...");
		st_deploying.OnEnter = delegate
		{
			if (deployedPosition == 0f)
			{
				animationState.speed = 0f - extendDurationFactor;
			}
			else
			{
				animationState.speed = extendDurationFactor;
			}
			if (lightModule != null && lightModule.isOn)
			{
				lightModule.SetFlareColor(Color.white);
			}
			eventToggle.guiName = GetToggleEventName();
			eventToggle.active = true;
		};
		st_deploying.OnUpdate = delegate
		{
			position = Mathf.Clamp01(animationState.normalizedTime);
			if (deployTgt != null && wheel != null)
			{
				wheel.SetWheelTransform(deployTgt.position, deployTgt.rotation);
				if (standInCollider != null)
				{
					standInCollider.transform.position = deployTgt.position;
				}
			}
			base.part.DragCubes.SetCubeWeight("Retracted", 1f - Position);
			base.part.DragCubes.SetCubeWeight("Deployed", Position);
			if (baseSetup)
			{
				SetInopSubsystems(Position < TsubSys);
			}
		};
		st_deploying.OnLeave = delegate
		{
		};
		fsm.AddState(st_deploying);
		st_inoperable = new KFSMState("Inoperable");
		st_inoperable.OnEnter = delegate
		{
			animationState.speed = 0f;
			eventToggle.guiName = GetToggleEventName();
			eventToggle.active = false;
		};
		st_inoperable.OnUpdate = delegate
		{
		};
		st_inoperable.OnLeave = delegate
		{
		};
		fsm.AddState(st_inoperable);
		on_deploy = new KFSMEvent("on_deploy");
		on_deploy.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_deploy.GoToStateOnEvent = st_deploying;
		on_deploy.OnEvent = delegate
		{
			if (!string.IsNullOrEmpty(fxDeploy))
			{
				base.part.Effect(fxDeploy);
			}
			if (!string.IsNullOrEmpty(fxRetract))
			{
				base.part.Effect(fxRetract, 0f);
			}
		};
		fsm.AddEvent(on_deploy, st_retracted, st_retracting);
		on_deployed = new KFSMEvent("on_deployed");
		on_deployed.updateMode = KFSMUpdateMode.LATEUPDATE;
		on_deployed.GoToStateOnEvent = st_deployed;
		on_deployed.OnCheckCondition = (KFSMState st) => HighLogic.LoadedSceneIsEditor || Mathf.Abs(Mathf.Clamp01(animationState.normalizedTime) - deployedPosition) < 0.01f;
		on_deployed.OnEvent = delegate
		{
			if (baseSetup)
			{
				SetInopSubsystems(disable: false);
			}
			if (!string.IsNullOrEmpty(fxDeployed))
			{
				base.part.Effect(fxDeployed);
			}
			if (!string.IsNullOrEmpty(fxRetract))
			{
				base.part.Effect(fxRetract, 0f);
			}
			if (!string.IsNullOrEmpty(fxDeploy))
			{
				base.part.Effect(fxDeploy, 0f);
			}
		};
		fsm.AddEvent(on_deployed, st_deploying);
		on_retract = new KFSMEvent("on_retract");
		on_retract.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_retract.GoToStateOnEvent = st_retracting;
		on_retract.OnEvent = delegate
		{
			if (!string.IsNullOrEmpty(fxRetract))
			{
				base.part.Effect(fxRetract);
			}
			if (!string.IsNullOrEmpty(fxDeploy))
			{
				base.part.Effect(fxDeploy, 0f);
			}
		};
		fsm.AddEvent(on_retract, st_deployed, st_deploying);
		on_retracted = new KFSMEvent("on_retracted");
		on_retracted.updateMode = KFSMUpdateMode.LATEUPDATE;
		on_retracted.GoToStateOnEvent = st_retracted;
		on_retracted.OnCheckCondition = (KFSMState st) => HighLogic.LoadedSceneIsEditor || Mathf.Abs(Mathf.Clamp01(animationState.normalizedTime) - retractedPosition) < 0.01f;
		on_retracted.OnEvent = delegate
		{
			if (baseSetup)
			{
				SetInopSubsystems(disable: true);
			}
			if (!string.IsNullOrEmpty(fxRetracted))
			{
				base.part.Effect(fxRetracted);
			}
			if (!string.IsNullOrEmpty(fxRetract))
			{
				base.part.Effect(fxRetract, 0f);
			}
			if (!string.IsNullOrEmpty(fxDeploy))
			{
				base.part.Effect(fxDeploy, 0f);
			}
		};
		fsm.AddEvent(on_retracted, st_retracting);
		on_inoperable = new KFSMEvent("on_inoperable");
		on_inoperable.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_inoperable.GoToStateOnEvent = st_inoperable;
		on_inoperable.OnCheckCondition = (KFSMState currentState) => false;
		on_inoperable.OnEvent = delegate
		{
			if (!string.IsNullOrEmpty(fxRetract))
			{
				base.part.Effect(fxRetract, 0f);
			}
			if (!string.IsNullOrEmpty(fxDeploy))
			{
				base.part.Effect(fxDeploy, 0f);
			}
		};
		fsm.AddEventExcluding(on_inoperable, st_inoperable);
		KerbalFSM kerbalFSM = fsm;
		kerbalFSM.OnStateChange = (Callback<KFSMState, KFSMState, KFSMEvent>)Delegate.Combine(kerbalFSM.OnStateChange, new Callback<KFSMState, KFSMState, KFSMEvent>(OnFSMStateChange));
		fsm.StartFSM(startStateName);
		OnFSMStateChange(st_retracted, st_deployed, on_deploy);
	}

	public void OnFSMStateChange(KFSMState from, KFSMState to, KFSMEvent evt)
	{
		stateString = fsm.currentStateName;
		switch (stateString)
		{
		case "Inoperable":
			stateDisplayString = cacheAutoLOC_7003226;
			break;
		case "Retracted":
			stateDisplayString = cacheAutoLOC_234861;
			break;
		case "Deploying...":
			stateDisplayString = cacheAutoLOC_6002270;
			break;
		case "Retracting...":
			stateDisplayString = cacheAutoLOC_234856;
			break;
		case "Deployed":
			stateDisplayString = cacheAutoLOC_6002269;
			break;
		}
	}

	public void FixedUpdate()
	{
		if (fsm.Started)
		{
			fsm.FixedUpdateFSM();
		}
	}

	public void Update()
	{
		if (fsm.Started)
		{
			fsm.UpdateFSM();
		}
	}

	public void LateUpdate()
	{
		if (fsm != null && fsm.Started)
		{
			fsm.LateUpdateFSM();
		}
	}

	[KSPEvent(active = true, guiActiveUnfocused = false, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001329")]
	public void EventToggle()
	{
		ToggleDeployment(affectSymCParts: true);
	}

	public void ToggleDeployment(bool affectSymCParts)
	{
		if (fsm.CurrentState != st_deployed && fsm.CurrentState != st_deploying)
		{
			if (fsm.CurrentState == st_retracted || fsm.CurrentState == st_retracting)
			{
				if (base.part.ShieldedFromAirstream && !shieldedCanDeploy && HighLogic.LoadedSceneIsFlight)
				{
					ScreenMessages.PostScreenMessage("<color=orange><b>[" + wheelBase.GetModuleDisplayName() + "]:</b> " + Localizer.Format("#autoLOC_7003227") + "</color>", 5f, ScreenMessageStyle.UPPER_LEFT);
					return;
				}
				fsm.RunEvent(on_deploy);
			}
		}
		else
		{
			fsm.RunEvent(on_retract);
		}
		if (affectSymCParts)
		{
			int i = 0;
			for (int count = base.part.symmetryCounterparts.Count; i < count; i++)
			{
				(base.part.symmetryCounterparts[i].Modules[base.part.Modules.IndexOf(this)] as ModuleWheelDeployment).ToggleDeployment(affectSymCParts: false);
			}
		}
	}

	public string GetToggleEventName()
	{
		if (HighLogic.LoadedSceneIsEditor)
		{
			if (position == deployedPosition)
			{
				return cacheAutoLOC_247601;
			}
			return cacheAutoLOC_6002271;
		}
		if (stateString != "Inoperable")
		{
			if (position != deployedPosition)
			{
				return cacheAutoLOC_7003225;
			}
			return cacheAutoLOC_7003224;
		}
		return cacheAutoLOC_7003226;
	}

	[KSPAction("#autoLOC_6001461", KSPActionGroup.Gear)]
	public void ActionToggle(KSPActionParam kAct)
	{
		KSPActionType kSPActionType = kAct.type;
		if (fsm != null && kSPActionType == KSPActionType.Toggle)
		{
			kSPActionType = ((fsm.CurrentState != st_retracted && fsm.CurrentState != st_retracting) ? KSPActionType.Deactivate : KSPActionType.Activate);
		}
		ActionToggle(kSPActionType);
	}

	public void ActionToggle(KSPActionType action)
	{
		switch (action)
		{
		case KSPActionType.Deactivate:
			if (fsm.CurrentState == st_deployed || fsm.CurrentState == st_deploying)
			{
				fsm.RunEvent(on_retract);
			}
			break;
		case KSPActionType.Activate:
			if (fsm.CurrentState == st_retracted || fsm.CurrentState == st_retracting)
			{
				if (base.part.ShieldedFromAirstream && !shieldedCanDeploy && HighLogic.LoadedSceneIsFlight)
				{
					ScreenMessages.PostScreenMessage("<color=orange><b>[" + wheelBase.moduleName + "]:</b> " + Localizer.Format("#autoLOC_7003227") + "</color>", 5f, ScreenMessageStyle.UPPER_LEFT);
				}
				else
				{
					fsm.RunEvent(on_deploy);
				}
			}
			break;
		}
	}

	public override string OnGatherInfo()
	{
		return Localizer.Format("#autoLOC_247645");
	}

	public string[] GetDragCubeNames()
	{
		anim = Part.FindModelAnimator(base.transform, animationTrfName, animationStateName);
		if (anim == null)
		{
			Debug.LogError(("[ModuleWheelDeployment]: Could not generate Drag Cubes for " + base.name + "! no animation " + animationStateName + " exists on object " + animationTrfName) ?? "", base.gameObject);
			Debug.Break();
			return null;
		}
		animationState = anim[animationStateName];
		animationState.normalizedSpeed = 0f;
		anim.Play(animationStateName);
		return new string[2] { "Deployed", "Retracted" };
	}

	public void AssumeDragCubePosition(string name)
	{
		if (!(name == "Deployed"))
		{
			if (name == "Retracted")
			{
				animationState.normalizedTime = 1f - deployedPosition;
			}
		}
		else
		{
			animationState.normalizedTime = deployedPosition;
		}
	}

	public bool UsesProceduralDragCubes()
	{
		return false;
	}

	public float FindTransformAnimationTime(Transform t)
	{
		float normalizedTime = animationState.normalizedTime;
		Vector3 b = t.position;
		float num = float.MaxValue;
		float result = float.MaxValue;
		if (deployedPosition == 0f)
		{
			for (float num2 = 0f; num2 < 1f; num2 += 0.025f)
			{
				animationState.normalizedTime = num2;
				anim.Sample();
				float num3 = Vector3.Distance(t.position, b);
				if (num3 < num)
				{
					num = num3;
					result = num2;
				}
			}
		}
		else
		{
			for (float num4 = 1f; num4 > 0f; num4 -= 0.025f)
			{
				animationState.normalizedTime = num4;
				anim.Sample();
				float num5 = Vector3.Distance(t.position, b);
				if (num5 < num)
				{
					num = num5;
					result = num4;
				}
			}
		}
		animationState.normalizedTime = normalizedTime;
		anim.Sample();
		return result;
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLOC_8004215");
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_6002269 = Localizer.Format("#autoLOC_6002269");
		cacheAutoLOC_6002270 = Localizer.Format("#autoLOC_6002270");
		cacheAutoLOC_234856 = Localizer.Format("#autoLOC_234856");
		cacheAutoLOC_234861 = Localizer.Format("#autoLOC_234861");
		cacheAutoLOC_7003226 = Localizer.Format("#autoLOC_7003226");
		cacheAutoLOC_247601 = Localizer.Format("#autoLOC_247601");
		cacheAutoLOC_6002271 = Localizer.Format("#autoLOC_6002271");
		cacheAutoLOC_7003224 = Localizer.Format("#autoLOC_7003224");
		cacheAutoLOC_7003225 = Localizer.Format("#autoLOC_7003225");
	}
}

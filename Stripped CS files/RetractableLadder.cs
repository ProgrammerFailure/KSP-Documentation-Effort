using UnityEngine;

public class RetractableLadder : PartModule, IMultipleDragCube
{
	[KSPField(isPersistant = true)]
	public string StateName = "Retracted";

	[KSPField]
	public string ladderAnimationRootName;

	[KSPField]
	public string ladderColliderName;

	[KSPField]
	public string ladderRetractAnimationName;

	[KSPField]
	public float externalActivationRange = 5f;

	public Animation anim;

	public Collider ladder;

	public KerbalFSM ladderFSM;

	public KFSMState st_retracted;

	public KFSMState st_extended;

	public KFSMState st_retracting;

	public KFSMState st_extending;

	public KFSMEvent On_retractStart;

	public KFSMEvent On_retractEnd;

	public KFSMEvent On_extendStart;

	public KFSMEvent On_extendEnd;

	public KFSMEvent On_retractEditor;

	public KFSMEvent On_extendEditor;

	public bool IsMultipleCubesActive => true;

	[KSPAction("#autoLOC_6001410")]
	public void ToggleAction(KSPActionParam param)
	{
		if (param.type != 0 && (param.type != KSPActionType.Toggle || ladderFSM == null || ladderFSM.CurrentState != st_retracted))
		{
			Retract();
		}
		else
		{
			Extend();
		}
	}

	[KSPAction("#autoLOC_6001411")]
	public void ExtendAction(KSPActionParam param)
	{
		Extend();
	}

	[KSPAction("#autoLOC_6001412")]
	public void RetractAction(KSPActionParam param)
	{
		Retract();
	}

	[KSPEvent(guiActiveEditor = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 4f, guiName = "#autoLOC_6001411")]
	public void Extend()
	{
		if (ladderFSM == null || !ladderFSM.Started || ladderFSM.CurrentState != st_retracted)
		{
			return;
		}
		if (HighLogic.LoadedSceneIsEditor)
		{
			if (!anim.isPlaying)
			{
				anim.Play();
			}
			ladderFSM.RunEvent(On_extendEditor);
			anim.Sample();
			base.Events["Extend"].active = false;
			base.Events["Retract"].active = true;
		}
		else
		{
			ladderFSM.RunEvent(On_extendStart);
		}
	}

	[KSPEvent(guiActiveEditor = true, guiActiveUnfocused = true, guiActive = true, unfocusedRange = 4f, guiName = "#autoLOC_6001412")]
	public void Retract()
	{
		if (ladderFSM == null || !ladderFSM.Started || ladderFSM.CurrentState != st_extended)
		{
			return;
		}
		if (HighLogic.LoadedSceneIsEditor)
		{
			if (!anim.isPlaying)
			{
				anim.Play();
			}
			ladderFSM.RunEvent(On_retractEditor);
			anim.Sample();
			base.Events["Extend"].active = true;
			base.Events["Retract"].active = false;
		}
		else
		{
			ladderFSM.RunEvent(On_retractStart);
		}
	}

	public override void OnStart(StartState state)
	{
		if (HighLogic.LoadedSceneIsMissionBuilder)
		{
			Transform transform = base.part.FindModelTransform(ladderAnimationRootName);
			if (transform != null)
			{
				anim = transform.GetComponent<Animation>();
				if ((bool)anim && (bool)anim[ladderRetractAnimationName])
				{
					anim[ladderRetractAnimationName].enabled = true;
					anim[ladderRetractAnimationName].speed = float.MaxValue;
					anim.Play(ladderRetractAnimationName);
				}
			}
		}
		else
		{
			if (state == StartState.None)
			{
				return;
			}
			base.Events["Extend"].active = false;
			base.Events["Retract"].active = false;
			Transform transform = base.part.FindModelTransform(ladderAnimationRootName);
			if (transform == null)
			{
				Debug.LogError("RetractableLadder: Cannot find ladder animation root '" + ladderAnimationRootName + "'");
				return;
			}
			anim = transform.GetComponent<Animation>();
			if (!anim)
			{
				Debug.LogError("RetractableLadder: No animation component found for ladder module.", base.gameObject);
				return;
			}
			if (!anim[ladderRetractAnimationName])
			{
				Debug.LogError("RetractableLadder: Retract animation clip name not found on ladder module animation.", base.gameObject);
				return;
			}
			Transform transform2 = base.part.FindModelTransform(ladderColliderName);
			if (transform == null)
			{
				Debug.LogError("RetractableLadder: Cannot find ladder collider '" + ladderColliderName + "'");
				return;
			}
			ladder = transform2.GetComponent<Collider>();
			if (!ladder)
			{
				Debug.LogError("RetractableLadder: No ladder collider found for ladder module.", base.gameObject);
				return;
			}
			anim[ladderRetractAnimationName].wrapMode = WrapMode.ClampForever;
			anim[ladderRetractAnimationName].enabled = true;
			anim[ladderRetractAnimationName].weight = 1f;
			ladderFSM = new KerbalFSM();
			st_retracted = new KFSMState("Retracted");
			st_retracted.OnEnter = delegate
			{
				anim[ladderRetractAnimationName].speed = 0f;
				anim[ladderRetractAnimationName].normalizedTime = 1f;
				SetDragState(1f);
				ladder.enabled = false;
				base.Events["Extend"].active = true;
				StateName = "Retracted";
			};
			ladderFSM.AddState(st_retracted);
			st_extended = new KFSMState("Extended");
			st_extended.OnEnter = delegate
			{
				anim[ladderRetractAnimationName].speed = 0f;
				anim[ladderRetractAnimationName].normalizedTime = 0f;
				ladder.enabled = true;
				SetDragState(0f);
				base.Events["Retract"].active = true;
				StateName = "Extended";
			};
			ladderFSM.AddState(st_extended);
			st_extending = new KFSMState("Extending");
			st_extending.OnEnter = delegate
			{
				anim[ladderRetractAnimationName].speed = -1f;
				anim.Play(ladderRetractAnimationName);
				ladder.enabled = false;
				base.Events["Extend"].active = false;
			};
			ladderFSM.AddState(st_extending);
			st_retracting = new KFSMState("Retracting");
			st_retracting.OnEnter = delegate
			{
				anim[ladderRetractAnimationName].speed = 1f;
				anim.Play(ladderRetractAnimationName);
				ladder.enabled = false;
				base.Events["Retract"].active = false;
			};
			ladderFSM.AddState(st_retracting);
			On_extendEditor = new KFSMEvent("Extend Editor");
			On_extendEditor.GoToStateOnEvent = st_extended;
			ladderFSM.AddEvent(On_extendEditor, st_retracted);
			On_retractEditor = new KFSMEvent("Retract Editor");
			On_retractEditor.GoToStateOnEvent = st_retracted;
			ladderFSM.AddEvent(On_retractEditor, st_extended);
			On_extendStart = new KFSMEvent("Extend Start");
			On_extendStart.GoToStateOnEvent = st_extending;
			On_extendStart.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
			ladderFSM.AddEvent(On_extendStart, st_retracted);
			On_extendEnd = new KFSMTimedEvent("Extend End", anim[ladderRetractAnimationName].length);
			On_extendEnd.GoToStateOnEvent = st_extended;
			ladderFSM.AddEvent(On_extendEnd, st_extending);
			On_retractStart = new KFSMEvent("Retract Start");
			On_retractStart.GoToStateOnEvent = st_retracting;
			On_retractStart.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
			ladderFSM.AddEvent(On_retractStart, st_extended);
			On_retractEnd = new KFSMTimedEvent("Retract End", anim[ladderRetractAnimationName].length);
			On_retractEnd.GoToStateOnEvent = st_retracted;
			ladderFSM.AddEvent(On_retractEnd, st_retracting);
			ladderFSM.StartFSM(StateName);
		}
	}

	public override void OnInventoryModeDisable()
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			ProtoPartModuleSnapshot protoPartModuleSnapshot = base.part.protoPartSnapshot.FindModule("RetractableLadder");
			if (protoPartModuleSnapshot != null && protoPartModuleSnapshot.moduleValues != null)
			{
				protoPartModuleSnapshot.moduleValues.SetValue("StateName", StateName);
			}
			if (string.Equals(StateName, "Retracted"))
			{
				SetAnimationToRetracted();
			}
		}
	}

	public override void OnStoredInInventory(ModuleInventoryPart moduleInventoryPart)
	{
		base.OnStoredInInventory(moduleInventoryPart);
		if (base.part.protoPartSnapshot == null)
		{
			return;
		}
		ProtoPartModuleSnapshot protoPartModuleSnapshot = base.part.protoPartSnapshot.FindModule("RetractableLadder");
		StateName = "Retracted";
		if (protoPartModuleSnapshot != null && protoPartModuleSnapshot.moduleValues != null)
		{
			string value = "";
			if (protoPartModuleSnapshot.moduleValues.TryGetValue("StateName", ref value))
			{
				value = "Retracted";
				protoPartModuleSnapshot.moduleValues.SetValue("StateName", value);
			}
		}
	}

	public override void OnPartCreatedFomInventory(ModuleInventoryPart moduleInventoryPart)
	{
		SetAnimationToRetracted();
	}

	public override void DemoteToPhysicslessPart()
	{
		base.DemoteToPhysicslessPart();
	}

	public void Update()
	{
		if (ladderFSM != null)
		{
			ladderFSM.UpdateFSM();
		}
	}

	public void FixedUpdate()
	{
		if (ladderFSM != null)
		{
			ladderFSM.FixedUpdateFSM();
			SetDragState(anim[ladderRetractAnimationName].normalizedTime);
		}
	}

	public void SetDragState(float b)
	{
		base.part.DragCubes.SetCubeWeight("A", b);
		base.part.DragCubes.SetCubeWeight("B", 1f - b);
	}

	public string[] GetDragCubeNames()
	{
		return new string[2] { "A", "B" };
	}

	public void AssumeDragCubePosition(string name)
	{
		anim = base.part.FindModelTransform(ladderAnimationRootName).GetComponent<Animation>();
		if (!anim)
		{
			Debug.LogError("RetractableLadder: No animation component found for ladder module.", base.gameObject);
			return;
		}
		if (!anim[ladderRetractAnimationName])
		{
			Debug.LogError("RetractableLadder: Retract animation clip name not found on ladder module animation.", base.gameObject);
			return;
		}
		anim[ladderRetractAnimationName].speed = 0f;
		anim[ladderRetractAnimationName].enabled = true;
		anim[ladderRetractAnimationName].weight = 1f;
		if (!(name == "A"))
		{
			if (name == "B")
			{
				anim[ladderRetractAnimationName].normalizedTime = 0f;
			}
		}
		else
		{
			anim[ladderRetractAnimationName].normalizedTime = 1f;
		}
	}

	public bool UsesProceduralDragCubes()
	{
		return false;
	}

	public void SetAnimationToRetracted()
	{
		Transform transform = base.part.FindModelTransform(ladderAnimationRootName);
		if (transform != null)
		{
			anim = transform.GetComponent<Animation>();
			if ((bool)anim && anim[ladderRetractAnimationName] != null)
			{
				anim[ladderRetractAnimationName].enabled = true;
				anim[ladderRetractAnimationName].weight = 1f;
				anim[ladderRetractAnimationName].speed = 0f;
				anim[ladderRetractAnimationName].normalizedTime = 1f;
			}
		}
	}
}

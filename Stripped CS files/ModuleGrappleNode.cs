using System;
using System.Collections;
using System.Collections.Generic;
using Expansions.Missions.Adjusters;
using ns9;
using UnityEngine;

public class ModuleGrappleNode : PartModule, IActiveJointHost, IJointLockState, IContractObjectiveModule
{
	[KSPField]
	public string nodeTransformName = "grappleNode";

	[KSPField]
	public string controlTransformName = "";

	[KSPField]
	public float undockEjectionForce = 5f;

	[KSPField]
	public float minDistanceToReEngage = 1f;

	[KSPField]
	public float captureRange = 0.06f;

	[KSPField]
	public float captureMinFwdDot = 0.998f;

	[KSPField]
	public float captureMaxRvel = 0.3f;

	[KSPField]
	public float pivotRange = 30f;

	[KSPField]
	public int deployAnimationController = -1;

	[KSPField]
	public float deployAnimationTarget = 1f;

	public DockedVesselInfo vesselInfo;

	public DockedVesselInfo otherVesselInfo;

	public string state = "Ready";

	public Transform nodeTransform;

	public Transform controlTransform;

	public uint dockedPartUId;

	public Part otherPart;

	public ModuleAnimateGeneric deployAnimator;

	public bool originalDeployAnimatorAllowManualControl = true;

	public AttachNode grappleNode;

	public RaycastHit hit;

	public Vector3 rGrabPoint;

	public Vector3 initNodeForward;

	public Vector3 initNodeUp;

	public Vector3 grabPos;

	public Vector3 grabOrt;

	public Vector3 grabOrt2;

	public ActiveJointPivot pivotJoint;

	public PartJoint GrappleJoint;

	public bool grappledSameVessel;

	public KerbalFSM fsm;

	public KFSMState st_ready;

	public KFSMState st_grappled;

	public KFSMState st_grappled_sameVessel;

	public KFSMState st_disengage;

	public KFSMState st_disabled;

	public KFSMEvent on_nodeDistance;

	public KFSMEvent on_nodeLost;

	public KFSMEvent on_contact;

	public KFSMEvent on_undock;

	public KFSMEvent on_sameVessel_disconnect;

	public KFSMEvent on_disable;

	public KFSMEvent on_enable;

	public KFSMEvent on_decouple;

	public List<AdjusterGrappleNodeBase> adjusterCache = new List<AdjusterGrappleNodeBase>();

	public bool IsDisabled
	{
		get
		{
			if (fsm != null && fsm.Started)
			{
				return fsm.CurrentState == st_disabled;
			}
			return false;
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("state"))
		{
			state = node.GetValue("state");
		}
		if (node.HasValue("dockUId"))
		{
			dockedPartUId = uint.Parse(node.GetValue("dockUId"));
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			grappleNode = new AttachNode();
			grappleNode.size = 1;
			grappleNode.id = "grapple";
		}
		if (node.HasValue("grapplePos"))
		{
			grabPos = KSPUtil.ParseVector3(node.GetValue("grapplePos"));
		}
		if (node.HasValue("grappleOrt"))
		{
			grabOrt = KSPUtil.ParseVector3(node.GetValue("grappleOrt"));
		}
		if (node.HasValue("grappleOrt2"))
		{
			grabOrt2 = KSPUtil.ParseVector3(node.GetValue("grappleOrt2"));
		}
		node.TryGetValue("grappledSameVessel", ref grappledSameVessel);
		if (node.HasNode("DOCKEDVESSEL"))
		{
			vesselInfo = new DockedVesselInfo();
			vesselInfo.Load(node.GetNode("DOCKEDVESSEL"));
		}
		if (node.HasNode("DOCKEDVESSEL_Other"))
		{
			otherVesselInfo = new DockedVesselInfo();
			otherVesselInfo.Load(node.GetNode("DOCKEDVESSEL_Other"));
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("state", (fsm == null || !fsm.Started) ? "Ready" : fsm.currentStateName);
		node.AddValue("dockUId", dockedPartUId);
		if (grappleNode != null)
		{
			node.AddValue("grappledSameVessel", grappledSameVessel);
			node.AddValue("grapplePos", KSPUtil.WriteVector(grappleNode.position));
			node.AddValue("grappleOrt", KSPUtil.WriteVector(grappleNode.orientation));
			node.AddValue("grappleOrt2", KSPUtil.WriteVector(grappleNode.secondaryAxis));
		}
		if (vesselInfo != null)
		{
			vesselInfo.Save(node.AddNode("DOCKEDVESSEL"));
		}
		if (otherVesselInfo != null)
		{
			otherVesselInfo.Save(node.AddNode("DOCKEDVESSEL_Other"));
		}
	}

	public override void OnStart(StartState st)
	{
		base.Events["Release"].active = false;
		base.Events["ReleaseSameVessel"].active = false;
		base.Events["SetLoose"].active = false;
		base.Events["LockPivot"].active = false;
		nodeTransform = base.part.FindModelTransform(nodeTransformName);
		if (!nodeTransform)
		{
			Debug.LogWarning("[Docking Node Module]: WARNING - No node transform found with name " + nodeTransformName, base.part.gameObject);
			return;
		}
		if (controlTransformName == string.Empty)
		{
			controlTransform = base.part.transform;
		}
		else
		{
			controlTransform = base.part.FindModelTransform(controlTransformName);
			if (!controlTransform)
			{
				Debug.LogWarning("[Docking Node Module]: WARNING - No control transform found with name " + nodeTransformName, base.part.gameObject);
				controlTransform = base.part.transform;
			}
		}
		if (base.part.physicalSignificance != 0)
		{
			Debug.LogWarning("[Docking Node Module]: WARNING - The part for a docking node module cannot be physicsless!", base.part.gameObject);
			base.part.physicalSignificance = Part.PhysicalSignificance.FULL;
		}
		if (deployAnimationController != -1)
		{
			deployAnimator = base.part.Modules.GetModule(deployAnimationController) as ModuleAnimateGeneric;
		}
		if (HighLogic.LoadedSceneIsFlight && state.Contains("Grappled"))
		{
			otherPart = base.vessel[dockedPartUId];
			if (otherPart == base.part.parent)
			{
				grappleNode.attachedPart = otherPart;
				grappleNode.owner = base.part;
				base.part.attachNodes.Add(grappleNode);
			}
			else
			{
				grappleNode.attachedPart = base.part;
				grappleNode.owner = otherPart;
				otherPart.attachNodes.Add(grappleNode);
			}
			grappleNode.position = grabPos;
			grappleNode.orientation = grabOrt;
			grappleNode.secondaryAxis = grabOrt2;
			grappleNode.originalPosition = grappleNode.position;
			grappleNode.originalOrientation = grappleNode.orientation;
			grappleNode.originalSecondaryAxis = grappleNode.secondaryAxis;
			pivotJoint = ActiveJointPivot.Create(this, grappleNode);
			pivotJoint.SetPivotAngleLimit(pivotRange);
		}
		if (!HighLogic.LoadedSceneIsMissionBuilder)
		{
			StartCoroutine(lateFSMStart(st));
		}
	}

	public IEnumerator lateFSMStart(StartState st)
	{
		yield return null;
		SetupFSM();
		if ((st & StartState.Editor) == 0)
		{
			fsm.StartFSM(state);
		}
	}

	public void SetupFSM()
	{
		fsm = new KerbalFSM();
		st_ready = new KFSMState("Ready");
		st_ready.OnEnter = delegate
		{
			if ((bool)deployAnimator)
			{
				deployAnimator.Events["Toggle"].active = true;
			}
		};
		st_ready.OnFixedUpdate = delegate
		{
			otherPart = FindContactParts();
		};
		st_ready.OnLeave = delegate
		{
			if ((bool)deployAnimator)
			{
				deployAnimator.Events["Toggle"].active = false;
			}
		};
		fsm.AddState(st_ready);
		st_grappled = new KFSMState("Grappled");
		st_grappled.OnEnter = delegate
		{
			rGrabPoint = otherPart.partTransform.InverseTransformPoint(nodeTransform.position);
			base.Events["Release"].active = true;
		};
		st_grappled.OnUpdate = delegate
		{
			bool flag2 = CountAdjustersBlockingGrappleRelease() > 0;
			base.Events["Release"].active = !flag2;
			if (flag2 && deployAnimator != null)
			{
				deployAnimator.Events["Toggle"].active = false;
				deployAnimator.allowManualControl = false;
			}
		};
		KFSMState kFSMState = st_grappled;
		kFSMState.OnLateUpdate = (KFSMCallback)Delegate.Combine(kFSMState.OnLateUpdate, new KFSMCallback(updateGrappleTransform));
		st_grappled.OnLeave = delegate
		{
			base.Events["Release"].active = false;
			if (CountAdjustersBlockingGrappleRelease() > 0 && deployAnimator != null)
			{
				deployAnimator.allowManualControl = originalDeployAnimatorAllowManualControl;
			}
		};
		fsm.AddState(st_grappled);
		st_grappled_sameVessel = new KFSMState("Grappled (same vessel)");
		st_grappled_sameVessel.OnEnter = delegate
		{
			base.Events["ReleaseSameVessel"].active = true;
			if (!base.vessel.packed)
			{
				GrappleSameVessel(otherPart);
			}
		};
		KFSMState kFSMState2 = st_grappled_sameVessel;
		kFSMState2.OnLateUpdate = (KFSMCallback)Delegate.Combine(kFSMState2.OnLateUpdate, new KFSMCallback(updateGrappleTransform));
		st_grappled_sameVessel.OnUpdate = delegate
		{
			if (GrappleJoint == null && !base.vessel.packed)
			{
				GrappleSameVessel(otherPart);
			}
			bool flag = CountAdjustersBlockingGrappleRelease() > 0;
			base.Events["ReleaseSameVessel"].active = !flag;
			if (flag && deployAnimator != null)
			{
				deployAnimator.Events["Toggle"].active = false;
				deployAnimator.allowManualControl = false;
			}
		};
		st_grappled_sameVessel.OnLeave = delegate
		{
			base.Events["ReleaseSameVessel"].active = false;
			if (CountAdjustersBlockingGrappleRelease() > 0 && deployAnimator != null)
			{
				deployAnimator.allowManualControl = originalDeployAnimatorAllowManualControl;
			}
			DestroySameVesselJoint();
			if (fsm.LastEvent == on_sameVessel_disconnect)
			{
				otherPart = null;
				vesselInfo = null;
				otherVesselInfo = null;
			}
		};
		fsm.AddState(st_grappled_sameVessel);
		st_disengage = new KFSMState("Disengage");
		st_disengage.OnEnter = delegate
		{
			if ((bool)otherPart)
			{
				rGrabPoint = otherPart.partTransform.InverseTransformPoint(nodeTransform.position);
			}
			if ((bool)deployAnimator)
			{
				deployAnimator.Events["Toggle"].active = true;
			}
		};
		st_disengage.OnLeave = delegate
		{
			if ((bool)deployAnimator)
			{
				deployAnimator.Events["Toggle"].active = false;
			}
		};
		fsm.AddState(st_disengage);
		st_disabled = new KFSMState("Disabled");
		st_disabled.OnEnter = delegate
		{
			if ((bool)deployAnimator)
			{
				deployAnimator.Events["Toggle"].active = true;
			}
		};
		st_disabled.OnLeave = delegate
		{
			if ((bool)deployAnimator)
			{
				deployAnimator.Events["Toggle"].active = false;
			}
		};
		fsm.AddState(st_disabled);
		on_contact = new KFSMEvent("Contact");
		on_contact.updateMode = KFSMUpdateMode.FIXEDUPDATE;
		on_contact.OnCheckCondition = delegate
		{
			if (otherPart == null)
			{
				return false;
			}
			if (IsAdjusterBlockingGrappleGrab())
			{
				return false;
			}
			return (otherPart.vessel != base.vessel) ? (CheckGrappleContact(nodeTransform, hit, captureRange, captureMinFwdDot) && (base.part.rb.velocity - otherPart.Rigidbody.velocity).sqrMagnitude < captureMaxRvel * captureMaxRvel) : CheckGrappleContact(nodeTransform, hit, captureRange, captureMinFwdDot);
		};
		on_contact.OnEvent = delegate
		{
			dockedPartUId = otherPart.flightID;
			if (otherPart.vessel != base.vessel)
			{
				on_contact.GoToStateOnEvent = st_grappled;
				if (otherPart.vessel.isEVA)
				{
					otherPart.GetComponent<KerbalEVA>().OnGrapple();
					FlightGlobals.ForceSetActiveVessel(base.vessel);
					FlightInputHandler.ResumeVesselCtrlState(base.vessel);
				}
				if (!(Vessel.GetDominantVessel(base.vessel, otherPart.vessel) == base.vessel) && !otherPart.vessel.isEVA)
				{
					Grapple(otherPart, base.part);
				}
				else
				{
					Grapple(otherPart, otherPart);
				}
			}
			else
			{
				on_contact.GoToStateOnEvent = st_grappled_sameVessel;
			}
		};
		fsm.AddEvent(on_contact, st_ready);
		on_nodeLost = new KFSMEvent("Node Lost");
		on_nodeLost.updateMode = KFSMUpdateMode.LATEUPDATE;
		on_nodeLost.GoToStateOnEvent = st_ready;
		on_nodeLost.OnCheckCondition = (KFSMState st) => OtherPartIsLost() && fsm.FramesInCurrentState > 2;
		on_nodeLost.OnEvent = delegate
		{
			otherPart = null;
			vesselInfo = null;
			otherVesselInfo = null;
		};
		fsm.AddEvent(on_nodeLost, st_grappled);
		on_nodeDistance = new KFSMEvent("Node Distanced");
		on_nodeDistance.updateMode = KFSMUpdateMode.LATEUPDATE;
		on_nodeDistance.GoToStateOnEvent = st_ready;
		on_nodeDistance.OnCheckCondition = (KFSMState st) => (OtherPartIsLost() || NodeIsTooFar()) && fsm.FramesInCurrentState > 2;
		on_nodeDistance.OnEvent = delegate
		{
			otherPart = null;
			vesselInfo = null;
			otherVesselInfo = null;
		};
		fsm.AddEvent(on_nodeDistance, st_disengage);
		on_undock = new KFSMEvent("Undock");
		on_undock.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_undock.GoToStateOnEvent = st_disengage;
		on_undock.OnEvent = delegate
		{
			on_undock.GoToStateOnEvent = (otherPart ? st_disengage : st_ready);
		};
		fsm.AddEvent(on_undock, st_grappled, st_grappled_sameVessel);
		on_sameVessel_disconnect = new KFSMEvent("Same Vessel Disconnect");
		on_sameVessel_disconnect.updateMode = KFSMUpdateMode.FIXEDUPDATE;
		on_sameVessel_disconnect.GoToStateOnEvent = st_ready;
		on_sameVessel_disconnect.OnCheckCondition = (KFSMState st) => otherPart == null || otherPart.vessel != base.vessel;
		on_sameVessel_disconnect.OnEvent = delegate
		{
		};
		fsm.AddEvent(on_sameVessel_disconnect, st_grappled_sameVessel);
		on_disable = new KFSMEvent("Disable");
		on_disable.updateMode = KFSMUpdateMode.UPDATE;
		on_disable.OnCheckCondition = (KFSMState st) => (bool)deployAnimator && deployAnimator.Progress != 1f;
		on_disable.OnEvent = delegate
		{
			if (grappleNode.attachedPart != null && !grappledSameVessel)
			{
				Release();
			}
		};
		on_disable.GoToStateOnEvent = st_disabled;
		fsm.AddEvent(on_disable, st_ready, st_disengage, st_grappled, st_grappled_sameVessel);
		on_enable = new KFSMEvent("Enable");
		on_enable.updateMode = KFSMUpdateMode.UPDATE;
		on_enable.OnCheckCondition = (KFSMState st) => (bool)deployAnimator && deployAnimator.Progress == 1f;
		on_enable.GoToStateOnEvent = st_ready;
		fsm.AddEvent(on_enable, st_disabled);
	}

	public void Update()
	{
		if (fsm != null && fsm.Started)
		{
			fsm.UpdateFSM();
			state = fsm.currentStateName;
		}
	}

	public void FixedUpdate()
	{
		if (fsm != null && fsm.Started)
		{
			fsm.FixedUpdateFSM();
		}
	}

	public void LateUpdate()
	{
		if (fsm != null && fsm.Started)
		{
			fsm.LateUpdateFSM();
		}
		if (pivotJoint != null && pivotJoint.isValid)
		{
			pivotJoint.DrawDebug();
		}
	}

	public Part FindContactParts()
	{
		if (base.part.packed)
		{
			return null;
		}
		if (Physics.Raycast(nodeTransform.position, nodeTransform.forward * captureRange, out hit, captureRange, LayerUtil.DefaultEquivalent))
		{
			Debug.DrawRay(nodeTransform.position, nodeTransform.forward * captureRange, Color.green);
			return FlightGlobals.GetPartUpwardsCached(hit.transform.gameObject);
		}
		Debug.DrawRay(nodeTransform.position, nodeTransform.forward * captureRange, Color.yellow);
		return null;
	}

	public bool CheckGrappleContact(Transform nodeT, RaycastHit hit, float minDist, float minFwdDot)
	{
		if ((nodeT.position - hit.point).sqrMagnitude < minDist * minDist && Vector3.Dot(nodeT.forward, -hit.normal) > minFwdDot)
		{
			return true;
		}
		return false;
	}

	public void Grapple(Part other, Part dockerSide)
	{
		Debug.Log("[Grapple Module] Grabbing on to " + Localizer.Format(other.partInfo.title) + " on " + Localizer.Format(other.vessel.vesselName), base.gameObject);
		vesselInfo = new DockedVesselInfo();
		vesselInfo.name = base.vessel.vesselName;
		vesselInfo.vesselType = base.vessel.vesselType;
		vesselInfo.rootPartUId = base.vessel.rootPart.flightID;
		otherVesselInfo = new DockedVesselInfo();
		otherVesselInfo.name = other.vessel.vesselName;
		otherVesselInfo.vesselType = other.vessel.vesselType;
		otherVesselInfo.rootPartUId = other.vessel.rootPart.flightID;
		Vector3 position = other.partTransform.InverseTransformPoint(hit.point);
		UntrackedObjectClass untrackedObjectSize = (UntrackedObjectClass)Enum.Parse(typeof(UntrackedObjectClass), other.vessel.DiscoveryInfo.size.Value);
		Dictionary<Part, Vector3> dictionary = new Dictionary<Part, Vector3>();
		Dictionary<Part, Quaternion> dictionary2 = new Dictionary<Part, Quaternion>();
		for (int i = 0; i < base.vessel.parts.Count; i++)
		{
			Part part = base.vessel.parts[i];
			if (part.physicalSignificance == Part.PhysicalSignificance.FULL)
			{
				dictionary.Add(part, part.partTransform.position);
				dictionary2.Add(part, part.partTransform.rotation);
			}
		}
		for (int i = 0; i < otherPart.vessel.parts.Count; i++)
		{
			Part part = otherPart.vessel.parts[i];
			if (part.physicalSignificance == Part.PhysicalSignificance.FULL)
			{
				dictionary.Add(part, part.partTransform.position);
				dictionary2.Add(part, part.partTransform.rotation);
			}
		}
		GameEvents.onActiveJointNeedUpdate.Fire(other.vessel);
		GameEvents.onActiveJointNeedUpdate.Fire(base.vessel);
		other.vessel.SetRotation(other.vessel.transform.rotation);
		base.vessel.SetRotation(base.vessel.transform.rotation);
		base.vessel.SetPosition(base.vessel.transform.position - (nodeTransform.position - other.partTransform.TransformPoint(position)), usePristineCoords: true);
		base.vessel.IgnoreGForces(10);
		grappleNode.position = dockerSide.partTransform.InverseTransformPoint(other.partTransform.TransformPoint(position));
		grappleNode.orientation = dockerSide.partTransform.InverseTransformDirection(base.part.partTransform.TransformDirection(Vector3.up));
		grappleNode.secondaryAxis = dockerSide.partTransform.InverseTransformDirection(base.part.partTransform.TransformDirection(Vector3.back));
		grappleNode.originalPosition = grappleNode.position;
		grappleNode.originalOrientation = grappleNode.orientation;
		grappleNode.originalSecondaryAxis = grappleNode.secondaryAxis;
		if (FlightGlobals.fetch.VesselTarget != null && FlightGlobals.fetch.VesselTarget.GetVessel() == other.vessel)
		{
			FlightGlobals.fetch.SetVesselTarget(null);
		}
		if (base.vessel.targetObject != null && base.vessel.targetObject.GetVessel() == other.vessel)
		{
			base.vessel.targetObject = null;
		}
		if (other.vessel.targetObject != null && other.vessel.targetObject.GetVessel() == base.part.vessel)
		{
			other.vessel.targetObject = null;
		}
		if (dockerSide == base.part)
		{
			grappleNode.attachedPart = other;
			grappleNode.owner = base.part;
			base.part.attachNodes.Add(grappleNode);
			if (FlightGlobals.ActiveVessel == base.vessel)
			{
				FlightGlobals.ForceSetActiveVessel(other.vessel);
				FlightInputHandler.SetNeutralControls();
			}
			base.part.Couple(other);
			GrappleJoint = base.part.attachJoint;
		}
		else
		{
			grappleNode.attachedPart = base.part;
			grappleNode.owner = other;
			other.attachNodes.Add(grappleNode);
			if (FlightGlobals.ActiveVessel == other.vessel)
			{
				FlightGlobals.ForceSetActiveVessel(base.vessel);
				FlightInputHandler.SetNeutralControls();
			}
			else if (FlightGlobals.ActiveVessel == base.vessel)
			{
				base.vessel.MakeActive();
				FlightInputHandler.SetNeutralControls();
			}
			other.Couple(base.part);
			GrappleJoint = other.attachJoint;
		}
		if (pivotJoint == null || !pivotJoint.isValid)
		{
			pivotJoint = ActiveJointPivot.Create(this, grappleNode);
			pivotJoint.SetPivotAngleLimit(pivotRange);
		}
		for (int i = 0; i < base.vessel.parts.Count; i++)
		{
			Part part = base.vessel.parts[i];
			if (part.physicalSignificance == Part.PhysicalSignificance.FULL)
			{
				part.partTransform.position = dictionary[part];
				part.partTransform.rotation = dictionary2[part];
			}
		}
		ModuleAsteroid moduleAsteroid = other.FindModuleImplementing<ModuleAsteroid>();
		if (moduleAsteroid != null)
		{
			AnalyticsUtil.LogAsteroidEvent(moduleAsteroid, AnalyticsUtil.SpaceObjectEventTypes.grappled, HighLogic.CurrentGame, base.part.vessel);
			base.vessel.DiscoveryInfo.SetUntrackedObjectSize(untrackedObjectSize);
		}
		ModuleComet moduleComet = other.FindModuleImplementing<ModuleComet>();
		if (moduleComet != null)
		{
			AnalyticsUtil.LogCometEvent(moduleComet, AnalyticsUtil.SpaceObjectEventTypes.grappled, HighLogic.CurrentGame, base.part.vessel);
			base.vessel.DiscoveryInfo.SetUntrackedObjectSize(untrackedObjectSize);
		}
		StartCoroutine(WaitAndSwitchFocus());
		GameEvents.onVesselWasModified.Fire(base.vessel);
	}

	public void updateGrappleTransform()
	{
		if (pivotJoint.driveMode != 0)
		{
			nodeTransform.rotation = base.part.partTransform.rotation * pivotJoint.GetLocalPivotRotation();
		}
	}

	[KSPAction("#autoLOC_6001450", activeEditor = false)]
	public void ReleaseAction(KSPActionParam param)
	{
		if (base.Events["Release"].active)
		{
			Release();
		}
	}

	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 2f, guiName = "#autoLOC_6001451")]
	public void Release()
	{
		Part parent = base.part.parent;
		Part part = otherPart;
		Vessel vessel = base.vessel;
		uint referenceTransformId = base.vessel.referenceTransformId;
		UntrackedObjectClass untrackedObjectSize = (UntrackedObjectClass)Enum.Parse(typeof(UntrackedObjectClass), base.vessel.DiscoveryInfo.size.Value);
		GrappleJoint = null;
		if (parent != otherPart)
		{
			otherPart.Undock(otherVesselInfo);
			otherPart.attachNodes.Remove(grappleNode);
		}
		else
		{
			base.part.Undock(vesselInfo);
			base.part.attachNodes.Remove(grappleNode);
		}
		grappleNode.attachedPart = null;
		grappleNode.owner = null;
		base.part.AddForce(nodeTransform.forward * ((0f - undockEjectionForce) * 0.5f));
		part.AddForce(nodeTransform.forward * (undockEjectionForce * 0.5f));
		if (vessel == FlightGlobals.ActiveVessel && vessel[referenceTransformId] == null)
		{
			StartCoroutine(WaitAndSwitchFocus());
		}
		ModuleAsteroid moduleAsteroid = otherPart.FindModuleImplementing<ModuleAsteroid>();
		if (moduleAsteroid != null)
		{
			AnalyticsUtil.LogAsteroidEvent(moduleAsteroid, AnalyticsUtil.SpaceObjectEventTypes.released, HighLogic.CurrentGame, base.part.vessel);
			otherPart.vessel.DiscoveryInfo.SetUntrackedObjectSize(untrackedObjectSize);
		}
		ModuleComet moduleComet = otherPart.FindModuleImplementing<ModuleComet>();
		if (moduleComet != null)
		{
			AnalyticsUtil.LogCometEvent(moduleComet, AnalyticsUtil.SpaceObjectEventTypes.released, HighLogic.CurrentGame, base.part.vessel);
			otherPart.vessel.DiscoveryInfo.SetUntrackedObjectSize(untrackedObjectSize);
		}
		fsm.RunEvent(on_undock);
	}

	public IEnumerator WaitAndSwitchFocus()
	{
		yield return null;
		FlightGlobals.ForceSetActiveVessel(base.vessel);
		FlightInputHandler.SetNeutralControls();
		base.vessel.ResumeStaging();
		FlightCamera.MinDistanceBackaway();
	}

	public void GrappleSameVessel(Part other)
	{
		Vector3 position = other.partTransform.InverseTransformPoint(hit.point);
		grappleNode.position = base.part.partTransform.InverseTransformPoint(other.partTransform.TransformPoint(position));
		grappleNode.orientation = base.part.partTransform.InverseTransformDirection(base.part.partTransform.TransformDirection(Vector3.up));
		grappleNode.secondaryAxis = base.part.partTransform.InverseTransformDirection(base.part.partTransform.TransformDirection(Vector3.back));
		grappleNode.originalPosition = grappleNode.position;
		grappleNode.originalOrientation = grappleNode.orientation;
		grappleNode.originalSecondaryAxis = grappleNode.secondaryAxis;
		grappleNode.attachedPart = other;
		grappleNode.owner = base.part;
		base.part.attachNodes.Add(grappleNode);
		GrappleJoint = PartJoint.Create(base.part, other, grappleNode, null, AttachModes.SRF_ATTACH);
		if (pivotJoint == null || !pivotJoint.isValid)
		{
			pivotJoint = ActiveJointPivot.Create(this, grappleNode);
			pivotJoint.SetPivotAngleLimit(pivotRange);
		}
		grappledSameVessel = true;
	}

	public void DestroySameVesselJoint()
	{
		pivotJoint.Terminate();
		GrappleJoint.DestroyJoint();
		base.Events["SetLoose"].active = false;
		base.Events["LockPivot"].active = false;
		base.part.attachNodes.Remove(grappleNode);
		grappleNode.owner = null;
		grappleNode.attachedPart = null;
		pivotJoint = null;
		grappledSameVessel = false;
	}

	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 2f, guiName = "#autoLOC_6001451")]
	public void ReleaseSameVessel()
	{
		fsm.RunEvent(on_undock);
	}

	public void OnOtherNodeSameVesselDisconnect()
	{
		fsm.RunEvent(on_nodeDistance);
	}

	public bool OtherPartIsLost()
	{
		if (otherPart == null)
		{
			Debug.LogWarning("[GrappleNode]: other node is null!", base.gameObject);
			return true;
		}
		if (otherPart.partTransform == null)
		{
			Debug.LogWarning("[GrappleNode]: other node transform is null!", base.gameObject);
			return true;
		}
		return false;
	}

	public bool NodeIsTooFar()
	{
		return (nodeTransform.position - otherPart.partTransform.TransformPoint(rGrabPoint)).sqrMagnitude > minDistanceToReEngage * minDistanceToReEngage;
	}

	[KSPAction("#autoLOC_6001452", activeEditor = false)]
	public void DecoupleAction(KSPActionParam param)
	{
		if (base.Events["Decouple"].active)
		{
			Decouple();
		}
	}

	[KSPEvent(active = false, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, unfocusedRange = 2f, guiName = "#autoLOC_6001452")]
	public void Decouple()
	{
		Part attachedPart = grappleNode.attachedPart;
		UntrackedObjectClass untrackedObjectSize = (UntrackedObjectClass)Enum.Parse(typeof(UntrackedObjectClass), base.vessel.DiscoveryInfo.size.Value);
		if (grappleNode.attachedPart == base.part.parent)
		{
			base.part.decouple();
		}
		else
		{
			grappleNode.attachedPart.decouple();
		}
		ModuleAsteroid moduleAsteroid = attachedPart.FindModuleImplementing<ModuleAsteroid>();
		if (moduleAsteroid != null)
		{
			AnalyticsUtil.LogAsteroidEvent(moduleAsteroid, AnalyticsUtil.SpaceObjectEventTypes.released, HighLogic.CurrentGame, base.part.vessel);
			attachedPart.vessel.DiscoveryInfo.SetUntrackedObjectSize(untrackedObjectSize);
		}
		ModuleComet moduleComet = attachedPart.FindModuleImplementing<ModuleComet>();
		if (moduleComet != null)
		{
			AnalyticsUtil.LogCometEvent(moduleComet, AnalyticsUtil.SpaceObjectEventTypes.released, HighLogic.CurrentGame, base.part.vessel);
			attachedPart.vessel.DiscoveryInfo.SetUntrackedObjectSize(untrackedObjectSize);
		}
		base.part.AddForce(nodeTransform.forward * ((0f - undockEjectionForce) * 0.5f));
		attachedPart.AddForce(nodeTransform.forward * (undockEjectionForce * 0.5f));
		fsm.RunEvent(on_undock);
	}

	[KSPAction("#autoLOC_6001447")]
	public void MakeReferenceToggle(KSPActionParam act)
	{
		MakeReferenceTransform();
	}

	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001447")]
	public void MakeReferenceTransform()
	{
		base.part.SetReferenceTransform(controlTransform);
		base.vessel.SetReferenceTransform(base.part);
	}

	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001453")]
	public void SetLoose()
	{
		pivotJoint.SetDriveMode(ActiveJoint.DriveMode.Neutral);
		base.Events["SetLoose"].active = false;
		base.Events["LockPivot"].active = true;
		base.vessel.CycleAllAutoStrut();
	}

	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001454")]
	public void LockPivot()
	{
		base.Events["LockPivot"].active = false;
		base.Events["SetLoose"].active = true;
		pivotJoint.SetDriveMode(ActiveJoint.DriveMode.Park);
		base.vessel.CycleAllAutoStrut();
	}

	public bool IsLoose()
	{
		if (pivotJoint != null)
		{
			return pivotJoint.driveMode == ActiveJoint.DriveMode.Neutral;
		}
		return true;
	}

	public Part GetHostPart()
	{
		return base.part;
	}

	public void OnJointInit(ActiveJoint joint)
	{
		base.Events["SetLoose"].active = joint != null;
		base.Events["LockPivot"].active = false;
		if (joint != null)
		{
			Debug.Log("[Grapple Joint]: joint ready", base.gameObject);
		}
		else
		{
			Debug.Log("[Grapple Joint]: joint lost", base.gameObject);
		}
	}

	public void OnDriveModeChanged(ActiveJoint.DriveMode mode)
	{
		if (mode != 0)
		{
			base.Events["SetLoose"].active = mode != ActiveJoint.DriveMode.Neutral;
			base.Events["LockPivot"].active = mode == ActiveJoint.DriveMode.Neutral;
		}
		else
		{
			base.Events["SetLoose"].active = false;
			base.Events["LockPivot"].active = false;
		}
	}

	public Transform GetLocalTransform()
	{
		return base.part.partTransform;
	}

	public override string GetInfo()
	{
		return "" + Localizer.Format("#autoLOC_242893", captureRange.ToString("0.0###"));
	}

	public bool IsJointUnlocked()
	{
		if (pivotJoint != null)
		{
			return pivotJoint.driveMode != ActiveJoint.DriveMode.Park;
		}
		return false;
	}

	public string GetContractObjectiveType()
	{
		return "Grapple";
	}

	public bool CheckContractObjectiveValidity()
	{
		return true;
	}

	public override void OnModuleAdjusterAdded(AdjusterPartModuleBase adjuster)
	{
		if (adjuster is AdjusterGrappleNodeBase item)
		{
			adjusterCache.Add(item);
		}
		base.OnModuleAdjusterAdded(adjuster);
	}

	public override void OnModuleAdjusterRemoved(AdjusterPartModuleBase adjuster)
	{
		AdjusterGrappleNodeBase item = adjuster as AdjusterGrappleNodeBase;
		adjusterCache.Remove(item);
		base.OnModuleAdjusterRemoved(adjuster);
	}

	public void SetUpAdjusterBlockingGrappleNodeAbilityToRelease()
	{
		if (CountAdjustersBlockingGrappleRelease() == 0 && deployAnimator != null)
		{
			originalDeployAnimatorAllowManualControl = deployAnimator.allowManualControl;
		}
	}

	public void RemoveAdjusterBlockingGrappleNodeAbilityToRelease()
	{
		if (CountAdjustersBlockingGrappleRelease() == 1 && deployAnimator != null)
		{
			deployAnimator.allowManualControl = originalDeployAnimatorAllowManualControl;
		}
	}

	public int CountAdjustersBlockingGrappleRelease()
	{
		int num = 0;
		for (int i = 0; i < adjusterCache.Count; i++)
		{
			if (adjusterCache[i].IsBlockingGrappleRelease())
			{
				num++;
			}
		}
		return num;
	}

	public bool IsAdjusterBlockingGrappleGrab()
	{
		int num = 0;
		while (true)
		{
			if (num < adjusterCache.Count)
			{
				if (adjusterCache[num].IsBlockingGrappleGrab())
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
		return Localizer.Format("#autoLoc_6003044");
	}
}

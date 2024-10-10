using System;
using System.Collections;
using System.Collections.Generic;
using Expansions.Missions.Adjusters;
using ns9;
using UnityEngine;

public class ModuleCargoBay : PartModule, IAirstreamShield
{
	[Serializable]
	public class PartCollider
	{
		public Collider collider;

		public Part owner;

		public PartCollider(Part p, Collider c)
		{
			collider = c;
			owner = p;
		}
	}

	[KSPField]
	public int DeployModuleIndex;

	public IScalarModule deployModule;

	[KSPField]
	public bool useBayContainer;

	[KSPField]
	public string bayContainerName = "";

	public Collider bayContainer;

	[KSPField]
	public float closedPosition;

	[KSPField]
	public float lookupRadius = 2f;

	[KSPField]
	public Vector3 lookupCenter = Vector3.zero;

	public List<Part> partsInCargoPrev;

	[SerializeField]
	public List<Part> partsInCargo;

	public List<Callback<IAirstreamShield>> onShdModifiedCallbacks;

	public List<PartCollider> ownColliders;

	[NonSerialized]
	public AttachNode NodeOuterFore;

	[KSPField]
	public string nodeOuterForeID;

	[NonSerialized]
	public AttachNode NodeOuterAft;

	[KSPField]
	public string nodeOuterAftID;

	[NonSerialized]
	public AttachNode NodeInnerFore;

	[KSPField]
	public string nodeInnerForeID;

	[NonSerialized]
	public AttachNode NodeInnerAft;

	[KSPField]
	public string nodeInnerAftID;

	[KSPField]
	public string partTypeName = "";

	public ModuleAnimateGeneric bayAnimator;

	public ModuleProceduralFairing fairing;

	public ModuleServiceModule serviceModule;

	public bool originalDeployFairingEventActive;

	public bool originalDeployFairingActionActive;

	public bool colliderResetCoroutineComplete;

	public bool shouldEnableShieldedVolume;

	public List<Part> connectingParts;

	public List<ModuleCargoBay> connectedCargoBays;

	public int layerMask;

	public float colliderResetSchedule;

	public bool enableShieldedVolumeAfterColliderReset;

	public List<AdjusterCargoBayBase> adjusterCache = new List<AdjusterCargoBayBase>();

	public override void OnAwake()
	{
		partsInCargo = new List<Part>();
		ownColliders = new List<PartCollider>();
		connectingParts = new List<Part>();
		connectedCargoBays = new List<ModuleCargoBay>();
		layerMask = LayerUtil.DefaultEquivalent | (1 << LayerMask.NameToLayer("WheelCollidersIgnore")) | (1 << LayerMask.NameToLayer("WheelColliders")) | (1 << LayerMask.NameToLayer("EVA"));
	}

	public override void OnStart(StartState state)
	{
		if (HighLogic.LoadedSceneIsEditor || HighLogic.LoadedSceneIsFlight)
		{
			deployModule = findModule(DeployModuleIndex);
			Transform transform = base.part.FindModelTransform(bayContainerName);
			if (transform != null)
			{
				bayContainer = transform.GetComponent<Collider>();
			}
			serviceModule = base.part.Modules.GetModule<ModuleServiceModule>();
			FindAttachNodes();
			scheduleColliderReset();
			GameEvents.onEditorShipModified.Add(onEditorVesselModified);
			GameEvents.onVesselWasModified.Add(onVesselModified);
			GameEvents.onPartDie.Add(OnPartDestroyed);
			GameEvents.onCommandSeatInteraction.Add(onCommandSeatInteraction);
			if (deployModule != null)
			{
				deployModule.OnMoving.Add(onCargoBayDoorsMoving);
				deployModule.OnStop.Add(onCargoBayDoorsStopped);
			}
		}
	}

	public override void OnStartFinished(StartState state)
	{
		if (!colliderResetCoroutineComplete)
		{
			enableShieldedVolumeAfterColliderReset = true;
			return;
		}
		ColliderListCleanUp();
		if (HighLogic.LoadedSceneIsMissionBuilder)
		{
			return;
		}
		if (serviceModule != null)
		{
			if (serviceModule.IsShrouded)
			{
				EnableShieldedVolume();
			}
			else
			{
				DisableShieldedVolume();
			}
		}
		else
		{
			EnableShieldedVolume();
		}
	}

	public void FindAttachNodes()
	{
		if (!string.IsNullOrEmpty(nodeOuterForeID))
		{
			NodeOuterFore = base.part.FindAttachNode(nodeOuterForeID);
			if (NodeOuterFore == null)
			{
				Debug.LogError("[ModuleCargoBay]: Could not find an attach node with id " + nodeOuterForeID + " for NodeOuterFore.", base.gameObject);
			}
		}
		if (!string.IsNullOrEmpty(nodeOuterAftID))
		{
			NodeOuterAft = base.part.FindAttachNode(nodeOuterAftID);
			if (NodeOuterAft == null)
			{
				Debug.LogError("[ModuleCargoBay]: Could not find an attach node with id " + nodeOuterAftID + " for NodeOuterAft.", base.gameObject);
			}
		}
		if (!string.IsNullOrEmpty(nodeInnerForeID))
		{
			NodeInnerFore = base.part.FindAttachNode(nodeInnerForeID);
			if (NodeInnerFore == null)
			{
				Debug.LogError("[ModuleCargoBay]: Could not find an attach node with id " + nodeInnerForeID + " for NodeInnerFore.", base.gameObject);
			}
		}
		if (!string.IsNullOrEmpty(nodeInnerAftID))
		{
			NodeInnerAft = base.part.FindAttachNode(nodeInnerAftID);
			if (NodeInnerAft == null)
			{
				Debug.LogError("[ModuleCargoBay]: Could not find an attach node with id " + nodeInnerAftID + " for NodeInnerAft.", base.gameObject);
			}
		}
	}

	public void onEditorVesselModified(ShipConstruct ship)
	{
		scheduleColliderReset();
	}

	public void scheduleColliderReset()
	{
		if (serviceModule == null)
		{
			shouldEnableShieldedVolume = HighLogic.LoadedSceneIsEditor || (HighLogic.LoadedSceneIsFlight && !base.vessel.packed);
		}
		else
		{
			shouldEnableShieldedVolume = (HighLogic.LoadedSceneIsEditor && serviceModule.IsShrouded) || (HighLogic.LoadedSceneIsFlight && !base.vessel.packed && serviceModule.IsShrouded);
		}
		StartCoroutine(scheduledColliderResetCoroutine(0.5f, shouldEnableShieldedVolume));
	}

	public IEnumerator scheduledColliderResetCoroutine(float delay, bool enableShieldedVolume)
	{
		enableShieldedVolumeAfterColliderReset = enableShieldedVolume;
		if (colliderResetSchedule > Time.realtimeSinceStartup)
		{
			colliderResetSchedule = Time.realtimeSinceStartup + delay;
			yield break;
		}
		colliderResetSchedule = Time.realtimeSinceStartup + delay;
		while (colliderResetSchedule > Time.realtimeSinceStartup)
		{
			yield return null;
		}
		ColliderReset(enableShieldedVolumeAfterColliderReset);
		colliderResetCoroutineComplete = true;
	}

	public void ColliderReset(bool enableShieldedVolume)
	{
		ownColliders.Clear();
		connectedCargoBays.Clear();
		FindBayColliders(base.part, this, ownColliders, connectedCargoBays);
		if (!HighLogic.LoadedSceneIsMissionBuilder && enableShieldedVolume)
		{
			EnableShieldedVolume();
		}
	}

	public void onVesselModified(Vessel v)
	{
		if (v == base.vessel || (v.vesselTransform.position - base.part.partTransform.TransformPoint(lookupCenter)).sqrMagnitude < lookupRadius * lookupRadius)
		{
			ColliderListCleanUp();
			EnableShieldedVolume();
		}
	}

	public void onCommandSeatInteraction(KerbalEVA eva, bool entered)
	{
		EnableShieldedVolume();
	}

	public void onVesselUnpack(Vessel v)
	{
		if (v == base.vessel)
		{
			ColliderListCleanUp();
			EnableShieldedVolume();
		}
	}

	public void onVesselPack(Vessel v)
	{
		if (v == base.vessel)
		{
			DisableShieldedVolume();
		}
	}

	public void onCargoBayDoorsMoving(float from, float to)
	{
		if (from != to || from != closedPosition)
		{
			NotifyShieldModified(notifyConnected: true);
			Debug.Log("Cargo Doors moving... (from " + from + ", to " + to + ")");
		}
	}

	public void onCargoBayDoorsStopped(float at)
	{
		EnableShieldedVolume();
		int count = connectedCargoBays.Count;
		while (count-- > 0)
		{
			connectedCargoBays[count].EnableShieldedVolume();
		}
		if (Math.Abs(at - closedPosition) < 0.01f)
		{
			Debug.Log("Cargo Doors Closed. (" + at + ")");
		}
		else
		{
			Debug.Log("Cargo Doors Stopped, Not Closed (" + at + ")");
		}
		NotifyShieldModified(notifyConnected: true);
	}

	public void OnPartDestroyed(Part p)
	{
		if (p == base.part)
		{
			UnShieldEnclosedParts(partsInCargo);
		}
	}

	public void OnDestroy()
	{
		GameEvents.onEditorShipModified.Remove(onEditorVesselModified);
		GameEvents.onVesselWasModified.Remove(onVesselModified);
		GameEvents.onPartDie.Remove(OnPartDestroyed);
		GameEvents.onCommandSeatInteraction.Remove(onCommandSeatInteraction);
		if (deployModule != null)
		{
			deployModule.OnMoving.Remove(onCargoBayDoorsMoving);
			deployModule.OnStop.Remove(onCargoBayDoorsStopped);
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			OnPartDestroyed(base.part);
		}
	}

	public void ColliderListCleanUp()
	{
		int count = ownColliders.Count;
		while (count-- > 0)
		{
			if (ownColliders[count] == null || ownColliders[count].collider == null || ownColliders[count].owner.vessel != base.vessel)
			{
				ownColliders.RemoveAt(count);
			}
		}
	}

	public IScalarModule findModule(int index)
	{
		if (base.part.Modules[index] is IScalarModule)
		{
			return base.part.Modules[index] as IScalarModule;
		}
		Debug.LogError("[ModuleCargoBay]: Module " + base.part.Modules[index].moduleName + " is not an IScalarModule", base.gameObject);
		return null;
	}

	public List<Part> FindEnclosedParts(List<Part> parts, List<PartCollider> ownColliders)
	{
		List<Part> list = new List<Part>();
		int count = parts.Count;
		for (int i = 0; i < count; i++)
		{
			Part part = parts[i];
			if (part != base.part && PartWithinBounds(part, ownColliders))
			{
				list.Add(part);
			}
		}
		return list;
	}

	public bool PartWithinBounds(Part p, List<PartCollider> ownColliders)
	{
		if (lookupRadius == 0f)
		{
			return false;
		}
		Bounds[] rendererBounds = p.GetRendererBounds();
		Bounds bounds = PartGeometryUtil.MergeBounds(base.part.GetRendererBounds(), base.part.partTransform);
		if (PartGeometryUtil.MergeBounds(rendererBounds, p.partTransform).size.sqrMagnitude > bounds.size.sqrMagnitude)
		{
			return false;
		}
		Vector3 vector = p.partTransform.TransformPoint(PartGeometryUtil.FindBoundsCentroid(rendererBounds, p.partTransform));
		if (!p.boundsCentroidOffset.IsZero())
		{
			vector += p.partTransform.rotation * p.boundsCentroidOffset;
		}
		if (!bounds.Contains(vector))
		{
			return false;
		}
		Vector3 vector2 = base.part.partTransform.TransformPoint(lookupCenter);
		float magnitude = (vector2 - vector).magnitude;
		Vector3 vector3 = (vector2 - vector) / magnitude;
		if (magnitude > lookupRadius)
		{
			return false;
		}
		if (magnitude > 0f)
		{
			if (useBayContainer)
			{
				if (bayContainer.Raycast(new Ray(vector, vector3), out var _, magnitude))
				{
					DebugDrawUtil.DrawCrosshairs(vector, 0.3f, Color.red, 5f);
					return false;
				}
			}
			else
			{
				int count = ownColliders.Count;
				while (count-- > 0)
				{
					PartCollider partCollider = ownColliders[count];
					if (!(p == partCollider.owner))
					{
						Debug.DrawRay(vector, vector3 * magnitude, Color.gray, 5f);
						if (partCollider.collider.Raycast(new Ray(vector, vector3), out var _, magnitude))
						{
							DebugDrawUtil.DrawCrosshairs(vector, 0.3f, Color.red, 5f);
							return false;
						}
						continue;
					}
					return false;
				}
			}
		}
		DebugDrawUtil.DrawCrosshairs(vector, 0.5f, Color.green, 5f);
		DebugDrawUtil.DrawCrosshairs(vector2, 0.5f, Color.yellow, 5f);
		return true;
	}

	public List<Part> FindNearbyParts(float radius, Vector3 center)
	{
		List<Part> list = new List<Part>();
		if (radius == 0f)
		{
			return list;
		}
		Collider[] array = Physics.OverlapSphere(base.part.partTransform.TransformPoint(center), radius, layerMask);
		int num = array.Length;
		while (num-- > 0)
		{
			Part partUpwardsCached = FlightGlobals.GetPartUpwardsCached(array[num].gameObject);
			if (partUpwardsCached != null && partUpwardsCached != base.part)
			{
				list.AddUnique(partUpwardsCached);
			}
		}
		return list;
	}

	public void EnableShieldedVolume()
	{
		if (partsInCargo != null && partsInCargo.Count > 0)
		{
			partsInCargoPrev = new List<Part>(partsInCargo);
		}
		DisableShieldedVolume();
		SetupDynamicCargoOccluders(testActive: true);
		partsInCargo = FindEnclosedParts(FindNearbyParts(lookupRadius, lookupCenter), ownColliders);
		SetupDynamicCargoOccluders(testActive: false);
		onShdModifiedCallbacks = ShieldEnclosedParts(partsInCargo);
		if (partsInCargoPrev == null || partsInCargoPrev.Count <= 0)
		{
			return;
		}
		int count = partsInCargoPrev.Count;
		while (count-- > 0)
		{
			Part part = partsInCargoPrev[count];
			if (part != null && !partsInCargo.Contains(part))
			{
				part.RemoveShield(this);
			}
		}
	}

	public void DisableShieldedVolume()
	{
		if (partsInCargo != null && partsInCargo.Count > 0)
		{
			UnShieldEnclosedParts(partsInCargo);
			partsInCargo.Clear();
			onShdModifiedCallbacks.Clear();
		}
	}

	public void SetupDynamicCargoOccluders(bool testActive)
	{
		List<Part> list = null;
		if (HighLogic.LoadedSceneIsFlight)
		{
			list = base.vessel.parts;
		}
		else if (HighLogic.LoadedSceneIsEditor)
		{
			list = EditorLogic.fetch.ship.parts;
		}
		if (list == null)
		{
			return;
		}
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			Part part = list[i];
			for (int j = 0; j < part.Modules.Count; j++)
			{
				if (part.Modules[j] is IDynamicCargoOccluder dynamicCargoOccluder)
				{
					dynamicCargoOccluder.SetupOcclusionTest(testActive);
				}
			}
		}
	}

	public List<Callback<IAirstreamShield>> ShieldEnclosedParts(List<Part> enclosedParts)
	{
		List<Callback<IAirstreamShield>> list = new List<Callback<IAirstreamShield>>();
		int count = enclosedParts.Count;
		while (count-- > 0)
		{
			list.AddUnique(enclosedParts[count].AddShield(this));
		}
		return list;
	}

	public void UnShieldEnclosedParts(List<Part> enclosedParts)
	{
		if (enclosedParts == null)
		{
			return;
		}
		int count = enclosedParts.Count;
		while (count-- > 0)
		{
			onShdModifiedCallbacks.RemoveAt(count);
			if (enclosedParts[count] != null)
			{
				enclosedParts[count].RemoveShield(this);
			}
			enclosedParts.RemoveAt(count);
		}
	}

	public void ModifyShieldInEnclosedParts(bool shieldStatus)
	{
		if (shieldStatus)
		{
			ShieldEnclosedParts(partsInCargo);
		}
		else
		{
			UnShieldEnclosedParts(partsInCargo);
		}
	}

	public void NotifyShieldModified(bool notifyConnected)
	{
		if (onShdModifiedCallbacks == null)
		{
			return;
		}
		int count = onShdModifiedCallbacks.Count;
		while (count-- > 0)
		{
			if (partsInCargo[count] != null)
			{
				onShdModifiedCallbacks[count](this);
				continue;
			}
			partsInCargo.RemoveAt(count);
			onShdModifiedCallbacks.RemoveAt(count);
		}
		if (notifyConnected)
		{
			int count2 = connectedCargoBays.Count;
			while (count2-- > 0)
			{
				connectedCargoBays[count2].NotifyShieldModified(notifyConnected: false);
			}
		}
	}

	public static List<PartCollider> FindPartColliders(Part p)
	{
		List<PartCollider> list = new List<PartCollider>();
		List<Collider> list2 = p.FindModelComponents<Collider>();
		int count = list2.Count;
		for (int i = 0; i < count; i++)
		{
			Collider collider = list2[i];
			bool num;
			if (!HighLogic.LoadedSceneIsFlight)
			{
				if (collider.gameObject.CompareTag("Airlock"))
				{
					continue;
				}
				num = !collider.gameObject.CompareTag("Ladder");
			}
			else
			{
				num = !collider.isTrigger;
			}
			if (num && (collider.gameObject.layer == 0 || collider.gameObject.layer == 19))
			{
				list.Add(new PartCollider(p, collider));
			}
		}
		return list;
	}

	public void FindBayColliders(Part originalPart, ModuleCargoBay origin, List<PartCollider> cList, List<ModuleCargoBay> cBays)
	{
		cList.AddUniqueRange(FindPartColliders(base.part));
		int i = 0;
		for (int count = connectingParts.Count; i < count; i++)
		{
			Part part = connectingParts[i];
			if (!(part == originalPart))
			{
				ModuleCargoBay moduleCargoBay = part.FindModuleImplementing<ModuleCargoBay>();
				if (moduleCargoBay != null)
				{
					cBays.AddUnique(moduleCargoBay);
					moduleCargoBay.FindBayColliders(originalPart, this, cList, cBays);
				}
				else
				{
					cList.AddUniqueRange(FindPartColliders(part));
				}
			}
		}
		List<AttachNode> list = FindConnectingNodes(origin.part);
		int j = 0;
		for (int count2 = list.Count; j < count2; j++)
		{
			AttachNode attachNode = list[j];
			if (attachNode.attachedPart != null && !connectingParts.Contains(attachNode.attachedPart))
			{
				ModuleCargoBay moduleCargoBay2 = attachNode.attachedPart.FindModuleImplementing<ModuleCargoBay>();
				if (moduleCargoBay2 != null)
				{
					cBays.AddUnique(moduleCargoBay2);
					moduleCargoBay2.FindBayColliders(originalPart, this, cList, cBays);
				}
				else
				{
					cList.AddUniqueRange(FindPartColliders(attachNode.attachedPart));
				}
			}
		}
	}

	public List<AttachNode> FindConnectingNodes(Part origin)
	{
		List<AttachNode> list = new List<AttachNode>();
		if (origin != base.part)
		{
			AttachNode opposingNode = GetOpposingNode(origin);
			if (opposingNode != null)
			{
				list.AddUnique(opposingNode);
			}
		}
		else
		{
			if (NodeOuterAft != null)
			{
				list.AddUnique(NodeOuterAft);
			}
			if (NodeOuterFore != null)
			{
				list.AddUnique(NodeOuterFore);
			}
		}
		return list;
	}

	public AttachNode GetOpposingNode(Part origin)
	{
		AttachNode attachNode = base.part.FindAttachNodeByPart(origin);
		if (attachNode != null)
		{
			if (attachNode == NodeOuterAft || attachNode == NodeInnerFore)
			{
				return NodeOuterFore;
			}
			if (attachNode == NodeInnerAft || attachNode == NodeOuterFore)
			{
				return NodeOuterAft;
			}
		}
		return null;
	}

	public void AddConnectingPart(Part p)
	{
		if (!connectingParts.Contains(p))
		{
			connectingParts.Add(p);
			scheduleColliderResetForAllBays();
		}
	}

	public void RemoveConnectingPart(Part p)
	{
		if (connectingParts.Contains(p))
		{
			connectingParts.Remove(p);
			scheduleColliderResetForAllBays();
		}
	}

	public void ClearConnectingParts()
	{
		if (connectingParts.Count > 0)
		{
			connectingParts.Clear();
			scheduleColliderResetForAllBays();
		}
	}

	public void scheduleColliderResetForAllBays()
	{
		scheduleColliderReset();
		int count = connectedCargoBays.Count;
		while (count-- > 0)
		{
			connectedCargoBays[count].scheduleColliderReset();
		}
	}

	public void SetLookupRadius(float radius)
	{
		lookupRadius = radius;
	}

	public void SetLookupCenter(Vector3 p)
	{
		lookupCenter = p;
	}

	public bool ClosedAndLocked()
	{
		if (!SelfClosedAndLocked())
		{
			return false;
		}
		int count = connectedCargoBays.Count;
		do
		{
			if (count-- <= 0)
			{
				return true;
			}
		}
		while (connectedCargoBays[count].SelfClosedAndLocked());
		return false;
	}

	public bool SelfClosedAndLocked()
	{
		if (deployModule != null && EndCapped() && deployModule.GetScalar == closedPosition && !deployModule.IsMoving())
		{
			return true;
		}
		return false;
	}

	public bool EndCapped()
	{
		if (NodeOuterAft != null)
		{
			if (NodeOuterAft.attachedPart == null)
			{
				return false;
			}
			if (!TestAttachmentFit(NodeOuterAft))
			{
				return false;
			}
		}
		if (NodeOuterFore != null)
		{
			if (NodeOuterFore.attachedPart == null)
			{
				return false;
			}
			if (!TestAttachmentFit(NodeOuterFore))
			{
				return false;
			}
		}
		return true;
	}

	public bool TestAttachmentFit(AttachNode n)
	{
		AttachNode attachNode = n.FindOpposingNode();
		if (attachNode != null)
		{
			return n.size <= attachNode.size;
		}
		return false;
	}

	public Vessel GetVessel()
	{
		return base.vessel;
	}

	public Part GetPart()
	{
		return base.part;
	}

	public override void OnModuleAdjusterAdded(AdjusterPartModuleBase adjuster)
	{
		if (adjuster is AdjusterCargoBayBase item)
		{
			adjusterCache.Add(item);
		}
		base.OnModuleAdjusterAdded(adjuster);
	}

	public override void OnModuleAdjusterRemoved(AdjusterPartModuleBase adjuster)
	{
		AdjusterCargoBayBase item = adjuster as AdjusterCargoBayBase;
		adjusterCache.Remove(item);
		base.OnModuleAdjusterRemoved(adjuster);
	}

	public void ForceCargoBayStuck(AdjusterPartModuleBase.FailureOpenState stuckState)
	{
		if (bayAnimator == null)
		{
			bayAnimator = base.part.Modules.GetModule(DeployModuleIndex) as ModuleAnimateGeneric;
		}
		if (bayAnimator != null)
		{
			float deployPercent = bayAnimator.deployPercent;
			bool freezeMidMovement = false;
			if (stuckState == AdjusterPartModuleBase.FailureOpenState.CurrentState)
			{
				if (bayAnimator.IsMoving())
				{
					if (bayAnimator.revClampPercent)
					{
						bayAnimator.deployPercent = (1f - bayAnimator.animTime) * 100f;
					}
					else
					{
						bayAnimator.deployPercent = bayAnimator.animTime * 100f;
					}
					freezeMidMovement = true;
				}
			}
			else
			{
				float num = closedPosition;
				float num2 = 1f - closedPosition;
				if (bayAnimator.allowDeployLimit)
				{
					num2 = 1f - bayAnimator.deployPercent / 100f;
				}
				if (stuckState == AdjusterPartModuleBase.FailureOpenState.Closed)
				{
					num = num2;
					num2 = closedPosition;
				}
				if ((Mathf.Abs(num - bayAnimator.animTime) < Mathf.Abs(num2 - bayAnimator.animTime) && !bayAnimator.IsMoving()) || (bayAnimator.IsMoving() && Mathf.Abs(num - (bayAnimator.animSpeed / 100f + bayAnimator.animTime)) < Mathf.Abs(num - bayAnimator.animTime)))
				{
					bayAnimator.Toggle();
				}
			}
			if (CountAdjustersForcingCargoBayStuck() == 0)
			{
				bayAnimator.FailAnimationForCargoBay(deployPercent, freezeMidMovement);
			}
			return;
		}
		fairing = base.part.Modules.GetModule<ModuleProceduralFairing>();
		if (fairing != null)
		{
			originalDeployFairingEventActive = fairing.Events["DeployFairing"].active;
			originalDeployFairingActionActive = fairing.Actions["DeployFairingAction"].active;
			fairing.Events["DeployFairing"].active = false;
			fairing.Actions["DeployFairingAction"].active = false;
			if (stuckState == AdjusterPartModuleBase.FailureOpenState.Open)
			{
				fairing.DeployFairing();
				originalDeployFairingEventActive = false;
				originalDeployFairingActionActive = false;
			}
		}
		else
		{
			Debug.LogWarning("No cargo bay and no fairing found for failure to act on!");
		}
	}

	public void RemoveAdjusterForcingCargoBayStuck()
	{
		if (CountAdjustersForcingCargoBayStuck() == 1)
		{
			if (bayAnimator != null)
			{
				bayAnimator.RestartFailedAnimationForCargoBay();
			}
			else if (fairing != null)
			{
				fairing.Events["DeployFairing"].active = originalDeployFairingEventActive;
				fairing.Actions["DeployFairingAction"].active = originalDeployFairingActionActive;
			}
		}
	}

	public int CountAdjustersForcingCargoBayStuck()
	{
		int num = 0;
		for (int i = 0; i < adjusterCache.Count; i++)
		{
			if (adjusterCache[i].IsCargoBayStuck())
			{
				num++;
			}
		}
		return num;
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLOC_8004213");
	}
}

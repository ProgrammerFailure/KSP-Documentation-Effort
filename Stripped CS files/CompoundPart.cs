using System;
using CompoundParts;
using UnityEngine;

public class CompoundPart : Part
{
	public enum AttachState
	{
		Detached,
		Attaching,
		Attached
	}

	public Vector3 direction;

	public Vector3 targetPosition;

	public Quaternion targetRotation;

	public Part target;

	public string targetMeshColName;

	public float maxLength = 10f;

	public AttachState attachState;

	public RaycastHit hit;

	public RaycastHit[] hits;

	public bool hasSaveData;

	public uint tgtId;

	public uint targetPersistentId;

	public bool needsDirectionFlip;

	public CompoundPart original;

	public CompoundPartModule[] cmpModules;

	[KSPField]
	public string disconnectedEffectName = "Disconnect";

	public Vector3 wTgtPos;

	public Quaternion wTgtRot;

	public bool tweakStarted;

	public bool tweakEnded = true;

	public ICMTweakTarget tweakTargetModule;

	[KSPField(isPersistant = true)]
	public bool disconnectAction;

	public bool isTweakingTarget
	{
		get
		{
			if (tweakTargetModule == null)
			{
				return false;
			}
			return tweakTargetModule.TweakingTarget;
		}
		set
		{
			if (tweakTargetModule != null)
			{
				tweakTargetModule.TweakingTarget = value;
			}
		}
	}

	[KSPEvent(guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6006112")]
	public void DisconnectEvent()
	{
		DisconnectCompoundPart();
	}

	[KSPAction("#autoLOC_6006112")]
	public void DisconnectAction(KSPActionParam param)
	{
		DisconnectCompoundPart();
	}

	public void DisconnectCompoundPart()
	{
		if (attachState != 0)
		{
			Effect(disconnectedEffectName, 1f);
		}
		DumpTarget();
		attachState = AttachState.Detached;
		int count = symmetryCounterparts.Count;
		while (count-- > 0)
		{
			if (symmetryCounterparts[count].persistentId != persistentId && symmetryCounterparts[count].isCompund)
			{
				CompoundPart compoundPart = symmetryCounterparts[count] as CompoundPart;
				if (compoundPart != null && compoundPart.attachState != 0)
				{
					compoundPart.DumpTarget();
					compoundPart.attachState = AttachState.Detached;
				}
			}
		}
	}

	public override void onCopy(Part original, bool asSymCPart)
	{
		hasSaveData = false;
		if (symmetryCounterparts.Contains(EditorLogic.SelectedPart))
		{
			direction = Vector3.zero;
		}
		else if (asSymCPart && EditorLogic.fetch.symmetryMethod == SymmetryMethod.Mirror)
		{
			this.original = (CompoundPart)original;
			needsDirectionFlip = true;
		}
	}

	public override void onPartAwake()
	{
		hasSaveData = false;
		cmpModules = GetComponents<CompoundPartModule>();
		ICMTweakTarget iCMTweakTarget = null;
		int count = base.Modules.Count;
		for (int i = 0; i < count; i++)
		{
			if (base.Modules[i] is ICMTweakTarget iCMTweakTarget2)
			{
				tweakTargetModule = iCMTweakTarget2;
			}
		}
		GameEvents.onEditorPartEvent.Add(OnEditorEvent);
		compund = true;
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("tgt", tgtId);
		node.AddValue("tpersID", targetPersistentId);
		node.AddValue("pos", KSPUtil.WriteVector(targetPosition));
		node.AddValue("rot", KSPUtil.WriteQuaternion(targetRotation));
		node.AddValue("dir", KSPUtil.WriteVector(direction));
		if (!string.IsNullOrEmpty(targetMeshColName))
		{
			node.AddValue("col", targetMeshColName);
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		hasSaveData = node.HasData;
		node.TryGetValue("tgt", ref tgtId);
		node.TryGetValue("tpersID", ref targetPersistentId);
		node.TryGetValue("pos", ref targetPosition);
		node.TryGetValue("dir", ref direction);
		node.TryGetValue("rot", ref targetRotation);
		node.TryGetValue("col", ref targetMeshColName);
	}

	public override void onStartComplete()
	{
		if (customPartData != string.Empty)
		{
			OnLoad(ParseCustomPartData(customPartData));
			customPartData = "";
		}
		attachState = AttachState.Detached;
		target = null;
		Part part = null;
		if (hasSaveData)
		{
			if (HighLogic.LoadedSceneIsFlight)
			{
				for (int i = 0; i < vessel.parts.Count; i++)
				{
					if (vessel.parts[i].craftID != tgtId || vessel.parts[i].missionID != missionID)
					{
						if (targetPersistentId != 0 && vessel.parts[i].persistentId == targetPersistentId)
						{
							part = vessel.parts[i];
							break;
						}
						continue;
					}
					part = vessel.parts[i];
					break;
				}
			}
			if (HighLogic.LoadedSceneIsEditor)
			{
				part = EditorLogic.fetch.ship.parts.Find((Part p) => p.craftID == tgtId);
			}
			if ((bool)part)
			{
				bool flag = false;
				if (EVAConstructionModeController.Instance != null && EVAConstructionModeController.Instance.IsOpen && EVAConstructionModeController.Instance.evaEditor != null && EVAConstructionModeController.Instance.evaEditor.EVAConstructionMode == ConstructionMode.Place)
				{
					flag = true;
				}
				if (!flag)
				{
					target = part;
					SetTarget(part, targetMeshColName);
					GameEvents.onEditorCompoundPartLinked.Fire(this);
				}
			}
			else if (HighLogic.LoadedSceneIsEditor)
			{
				if (direction != Vector3.zero)
				{
					Debug.LogWarning("[CompoundPart]: No target found with craftID " + craftID + ". Attempting to find it at direction [" + KSPUtil.WriteVector(direction) + "].", base.gameObject);
					StartCoroutine(CallbackUtil.DelayedCallback(1, schedule_raycast));
				}
			}
			else
			{
				Debug.Log("[CompoundPart]: Part: " + partName + " craftID: " + craftID + " No target found with craftID: " + tgtId);
			}
		}
		base.Events["DisconnectEvent"].guiActive = disconnectAction;
		base.Actions["DisconnectAction"].active = disconnectAction;
		base.Actions["DisconnectAction"].activeEditor = disconnectAction;
	}

	public override void onPartAttach(Part parent)
	{
		if (HighLogic.LoadedSceneIsEditor)
		{
			if (EditorLogic.SelectedPart == this)
			{
				lockEditor();
				attachState = AttachState.Attaching;
			}
			else
			{
				attachState = AttachState.Detached;
				if (direction != Vector3.zero)
				{
					if (needsDirectionFlip)
					{
						Vector3 vector = original.transform.TransformDirection(original.direction);
						vector = new Vector3(0f - vector.x, vector.y, vector.z);
						direction = base.transform.InverseTransformDirection(vector);
						needsDirectionFlip = false;
					}
					StartCoroutine(CallbackUtil.DelayedCallback(1, schedule_raycast));
				}
			}
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			LockEVAEditor();
		}
	}

	public override void onPartDetach()
	{
		if ((HighLogic.LoadedSceneIsEditor && EditorLogic.SelectedPart == this) || ((bool)target && target.localRoot != base.localRoot))
		{
			DumpTarget();
			attachState = AttachState.Detached;
			if (EditorLogic.SelectedPart == this)
			{
				direction = Vector3.zero;
			}
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			DumpTarget();
			InputLockManager.RemoveControlLock("EVACompoundPart_Placement");
		}
	}

	public override void onPartDestroy()
	{
		DumpTarget();
		unlockEditor();
		GameEvents.onEditorPartEvent.Remove(OnEditorEvent);
	}

	public override void onEditorStartTweak()
	{
		if (tweakTargetModule == null)
		{
			return;
		}
		if (!tweakStarted)
		{
			if (tweakTargetModule != null)
			{
				tweakTargetModule.SelectTweakTarget(Input.mousePosition);
			}
			tweakStarted = true;
		}
		if (tweakEnded)
		{
			tweakEnded = false;
			ToggleSymmetryCounterpartsTweak(isTweakingTarget);
		}
	}

	public override void onEditorEndTweak()
	{
		isTweakingTarget = false;
		ToggleSymmetryCounterpartsTweak(toggleValue: false);
		tweakEnded = true;
		tweakStarted = false;
	}

	public void ToggleSymmetryCounterpartsTweak(bool toggleValue)
	{
		for (int i = 0; i < symmetryCounterparts.Count; i++)
		{
			if (symmetryCounterparts[i] != null)
			{
				CompoundPart compoundPart = symmetryCounterparts[i] as CompoundPart;
				if (compoundPart != null)
				{
					compoundPart.isTweakingTarget = toggleValue;
				}
			}
		}
	}

	public override Transform GetReferenceTransform()
	{
		if (tweakTargetModule != null)
		{
			return tweakTargetModule.GetReferenceTransform();
		}
		return partTransform;
	}

	public override Part GetReferenceParent()
	{
		if (!isTweakingTarget)
		{
			return parent;
		}
		return target;
	}

	public override void SetSymmetryValues(Vector3 newPosition, Quaternion newRotation)
	{
		if (tweakTargetModule == null || !tweakTargetModule.SetSymmetryValues(newPosition, newRotation))
		{
			base.transform.position = newPosition;
			base.transform.rotation = newRotation;
		}
	}

	public void ToggleTweakTarget(bool tweakTargetValue)
	{
		isTweakingTarget = tweakTargetValue;
	}

	public override Collider[] GetPartColliders()
	{
		if (HighLogic.LoadedSceneIsEditor && tweakTargetModule != null)
		{
			return tweakTargetModule.GetSelectedColliders();
		}
		return partTransform.Find("model").GetComponentsInChildren<Collider>();
	}

	public void onTargetDetach()
	{
		if ((bool)this)
		{
			UnsetLink();
			attachState = AttachState.Detached;
		}
	}

	public void onTargetDestroy()
	{
		if ((bool)this)
		{
			UnsetLink();
			attachState = AttachState.Detached;
		}
	}

	public void onTargetReattach()
	{
		if (!this)
		{
			return;
		}
		StartCoroutine(CallbackUtil.DelayedCallback(1, schedule_raycast));
		if (EditorLogic.fetch.symmetryMethod == SymmetryMethod.Radial)
		{
			for (int i = 0; i < symmetryCounterparts.Count; i++)
			{
				CompoundPart compoundPart = (CompoundPart)symmetryCounterparts[i];
				compoundPart.direction = direction;
				compoundPart.StartCoroutine(CallbackUtil.DelayedCallback(1, compoundPart.schedule_raycast));
			}
		}
		else if (EditorLogic.fetch.symmetryMethod == SymmetryMethod.Mirror)
		{
			Vector3 vector = base.transform.TransformDirection(direction);
			vector = new Vector3(0f - vector.x, vector.y, vector.z);
			for (int j = 0; j < symmetryCounterparts.Count; j++)
			{
				CompoundPart compoundPart2 = (CompoundPart)symmetryCounterparts[j];
				compoundPart2.direction = compoundPart2.transform.InverseTransformDirection(vector);
				compoundPart2.StartCoroutine(CallbackUtil.DelayedCallback(1, compoundPart2.schedule_raycast));
			}
		}
	}

	public override void onEditorUpdate()
	{
		AttachState attachState = this.attachState;
		if (attachState == AttachState.Attaching)
		{
			onAttachUpdate();
		}
	}

	public override void OnConstructionModeUpdate()
	{
		base.OnConstructionModeUpdate();
		AttachState attachState = this.attachState;
		if (attachState == AttachState.Attaching)
		{
			onAttachUpdate();
		}
	}

	public override void OnInventoryModeDisable()
	{
		base.OnInventoryModeDisable();
		attachState = AttachState.Detached;
		DumpTarget();
	}

	public override void OnInventoryModeEnable()
	{
		base.OnInventoryModeEnable();
		DumpTarget();
	}

	public override void OnPartCreatedFomInventory(ModuleInventoryPart moduleInventoryPart)
	{
		ModulesOnStart();
		if (moduleInventoryPart != null)
		{
			missionID = moduleInventoryPart.part.missionID;
		}
		else
		{
			missionID = protoPartSnapshot.missionID;
		}
		protoPartSnapshot.missionID = missionID;
		direction = Vector3.zero;
		attachState = AttachState.Detached;
	}

	public override void LateUpdate()
	{
		if (!HighLogic.LoadedSceneIsFlight)
		{
			return;
		}
		switch (attachState)
		{
		case AttachState.Attached:
		{
			int num = cmpModules.Length;
			while (num-- > 0)
			{
				cmpModules[num].OnTargetUpdate();
			}
			break;
		}
		case AttachState.Attaching:
			onAttachUpdate();
			break;
		}
	}

	public override void onPartFixedUpdate()
	{
		if (attachState == AttachState.Attached && (target == null || target.vessel != vessel))
		{
			DumpTarget();
		}
	}

	public void onAttachUpdate()
	{
		if (direction != Vector3.zero)
		{
			targetPosition = base.transform.InverseTransformPoint(hit.point);
			targetRotation = Quaternion.FromToRotation(Vector3.right, base.transform.InverseTransformDirection(hit.normal));
			PreviewAttachment(direction, targetPosition, targetRotation);
			if (Input.GetMouseButtonUp(0) && Vector3.Distance(base.transform.position, hit.point) <= maxLength)
			{
				EndPreview();
				raycastTarget(direction);
				if (HighLogic.LoadedSceneIsEditor)
				{
					if (symMethod == SymmetryMethod.Radial)
					{
						for (int i = 0; i < symmetryCounterparts.Count; i++)
						{
							((CompoundPart)symmetryCounterparts[i]).raycastTarget(direction);
						}
					}
					else if (symMethod == SymmetryMethod.Mirror)
					{
						Vector3 vector = base.transform.TransformDirection(direction);
						vector = new Vector3(0f - vector.x, vector.y, vector.z);
						for (int j = 0; j < symmetryCounterparts.Count; j++)
						{
							CompoundPart obj = (CompoundPart)symmetryCounterparts[j];
							obj.raycastTarget(obj.transform.InverseTransformDirection(vector));
						}
					}
				}
				if ((bool)target)
				{
					if (HighLogic.LoadedSceneIsEditor)
					{
						EditorLogic.fetch.ResetBackup();
						EditorLogic.fetch.GetComponent<AudioSource>().PlayOneShot(EditorLogic.fetch.attachClip);
						StartCoroutine(CallbackUtil.DelayedCallback(1, unlockEditor));
					}
					if (HighLogic.LoadedSceneIsFlight)
					{
						if (EVAConstructionModeController.Instance != null && EVAConstructionModeController.Instance.evaEditor != null)
						{
							EVAConstructionModeController.Instance.evaEditor.PlayAudioClip(EVAConstructionModeController.Instance.evaEditor.attachClip);
						}
						StartCoroutine(CallbackUtil.DelayedCallback(1, UnLockEVAEditor));
					}
					GameEvents.onEditorCompoundPartLinked.Fire(this);
					return;
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.Delete))
		{
			DumpTarget();
			for (int k = 0; k < symmetryCounterparts.Count; k++)
			{
				((CompoundPart)symmetryCounterparts[k]).DumpTarget();
			}
			if (HighLogic.LoadedSceneIsEditor)
			{
				unlockEditor();
			}
			if (HighLogic.LoadedSceneIsFlight)
			{
				UnLockEVAEditor();
			}
		}
		direction = findTargetDirection();
	}

	public Vector3 findTargetDirection()
	{
		hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 1000f, LayerUtil.DefaultEquivalent);
		if (hits.Length != 0)
		{
			Array.Sort(hits, (RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance));
			for (int i = 0; i < hits.Length; i++)
			{
				hit = hits[i];
				Part partUpwardsCached = FlightGlobals.GetPartUpwardsCached(hit.collider.gameObject);
				if (partUpwardsCached != null && partUpwardsCached.persistentId != persistentId)
				{
					return base.transform.InverseTransformPoint(hit.point).normalized;
				}
			}
		}
		return Vector3.zero;
	}

	public void lockEditor()
	{
		InputLockManager.SetControlLock(ControlTypes.EDITOR_SOFT_LOCK | ControlTypes.EDITOR_EDIT_STAGES, "CompoundPart_Placement");
	}

	public void LockEVAEditor()
	{
		InputLockManager.SetControlLock(ControlTypes.EDITOR_SOFT_LOCK | ControlTypes.EDITOR_EDIT_STAGES, "EVACompoundPart_Placement");
	}

	public void unlockEditor()
	{
		InputLockManager.RemoveControlLock("CompoundPart_Placement");
	}

	public void UnLockEVAEditor()
	{
		InputLockManager.RemoveControlLock("EVACompoundPart_Placement");
	}

	public void schedule_raycast()
	{
		raycastTarget(direction);
	}

	public bool raycastTarget(Vector3 dir)
	{
		direction = dir;
		bool result = false;
		Debug.DrawRay(base.transform.position, base.transform.TransformDirection(dir), Color.yellow, 3f);
		int layer = base.gameObject.layer;
		base.gameObject.SetLayerRecursive(2);
		if (Physics.Raycast(base.transform.position, base.transform.TransformDirection(dir), out hit, maxLength, EditorLogic.LayerMask))
		{
			Part partUpwardsCached = FlightGlobals.GetPartUpwardsCached(hit.collider.gameObject);
			string tgtColName = hit.collider.name;
			if (partUpwardsCached != null && !partUpwardsCached.frozen)
			{
				targetPosition = base.transform.InverseTransformPoint(hit.point);
				targetRotation = Quaternion.FromToRotation(Vector3.right, base.transform.InverseTransformDirection(hit.normal));
				result = true;
				SetTarget(partUpwardsCached, tgtColName);
			}
		}
		base.gameObject.SetLayerRecursive(layer);
		return result;
	}

	public bool SetTarget(Part tgt, string tgtColName)
	{
		if (target != null)
		{
			DumpTarget();
		}
		target = tgt;
		targetMeshColName = tgtColName;
		if (target != null)
		{
			if (target.frozen)
			{
				target = null;
				return false;
			}
			if (HighLogic.LoadedSceneIsEditor)
			{
				Part part = target;
				part.OnEditorDetach = (Callback)Delegate.Combine(part.OnEditorDetach, new Callback(onTargetDetach));
				Part part2 = target;
				part2.OnEditorDestroy = (Callback)Delegate.Combine(part2.OnEditorDestroy, new Callback(onTargetDestroy));
				Part part3 = target;
				part3.OnEditorAttach = (Callback)Delegate.Combine(part3.OnEditorAttach, new Callback(onTargetReattach));
			}
			tgtId = target.craftID;
			targetPersistentId = target.persistentId;
			UpdateWorldValues();
			SetLink();
			attachState = AttachState.Attached;
			if (HighLogic.LoadedSceneIsFlight && target.persistentId != persistentId)
			{
				GameEvents.OnFlightCompoundPartLinked.Fire(this);
			}
			return true;
		}
		return false;
	}

	public void UpdateWorldValues()
	{
		wTgtPos = target.transform.InverseTransformPoint(base.transform.TransformPoint(targetPosition));
		wTgtRot = Quaternion.Inverse(target.transform.rotation) * base.transform.rotation * targetRotation;
	}

	public void DumpTarget()
	{
		if (target != null)
		{
			Part part = target;
			part.OnEditorDetach = (Callback)Delegate.Remove(part.OnEditorDetach, new Callback(onTargetDetach));
			Part part2 = target;
			part2.OnEditorDestroy = (Callback)Delegate.Remove(part2.OnEditorDestroy, new Callback(onTargetDestroy));
			Part part3 = target;
			part3.OnEditorAttach = (Callback)Delegate.Remove(part3.OnEditorAttach, new Callback(onTargetReattach));
			if (HighLogic.LoadedSceneIsFlight)
			{
				GameEvents.OnFlightCompoundPartDetached.Fire(this);
			}
		}
		onEditorEndTweak();
		wTgtPos = Vector3.zero;
		wTgtRot = Quaternion.identity;
		UnsetLink();
		target = null;
		tgtId = 0u;
		targetPersistentId = 0u;
		attachState = AttachState.Detached;
	}

	public void SetLink()
	{
		int num = cmpModules.Length;
		while (num-- > 0)
		{
			cmpModules[num].OnTargetSet(target);
		}
	}

	public void UnsetLink()
	{
		int num = cmpModules.Length;
		while (num-- > 0)
		{
			cmpModules[num].OnTargetLost();
		}
	}

	public void PreviewAttachment(Vector3 rDir, Vector3 rPos, Quaternion rRot)
	{
		int num = cmpModules.Length;
		while (num-- > 0)
		{
			cmpModules[num].OnPreviewAttachment(rDir, rPos, rRot);
		}
		if (symMethod == SymmetryMethod.Radial)
		{
			for (int i = 0; i < symmetryCounterparts.Count; i++)
			{
				CompoundPart compoundPart = (CompoundPart)symmetryCounterparts[i];
				int num2 = compoundPart.cmpModules.Length;
				while (num2-- > 0)
				{
					compoundPart.cmpModules[num2].OnPreviewAttachment(rDir, rPos, rRot);
				}
			}
		}
		if (symMethod != SymmetryMethod.Mirror)
		{
			return;
		}
		Vector3 vector = base.transform.TransformDirection(direction);
		vector = new Vector3(0f - vector.x, vector.y, vector.z);
		for (int j = 0; j < symmetryCounterparts.Count; j++)
		{
			CompoundPart compoundPart2 = (CompoundPart)symmetryCounterparts[j];
			Vector3 vector2 = compoundPart2.transform.InverseTransformDirection(vector);
			Vector3 rPos2 = vector2 * rPos.magnitude;
			Quaternion anchorRot = compoundPart2.getAnchorRot(vector2, compoundPart2.targetRotation);
			int num3 = compoundPart2.cmpModules.Length;
			while (num3-- > 0)
			{
				compoundPart2.cmpModules[num3].OnPreviewAttachment(vector2, rPos2, anchorRot);
			}
		}
	}

	public void EndPreview()
	{
		int num = cmpModules.Length;
		while (num-- > 0)
		{
			cmpModules[num].OnPreviewEnd();
		}
		for (int i = 0; i < symmetryCounterparts.Count; i++)
		{
			CompoundPart compoundPart = (CompoundPart)symmetryCounterparts[i];
			int num2 = compoundPart.cmpModules.Length;
			while (num2-- > 0)
			{
				compoundPart.cmpModules[num2].OnPreviewEnd();
			}
		}
	}

	public Quaternion getAnchorRot(Vector3 rDir, Quaternion defaultRot)
	{
		int layer = base.gameObject.layer;
		base.gameObject.SetLayerRecursive(2);
		Quaternion result = defaultRot;
		if (Physics.Raycast(base.transform.position, base.transform.TransformDirection(rDir), out hit, maxLength, EditorLogic.LayerMask))
		{
			Part partUpwardsCached = FlightGlobals.GetPartUpwardsCached(hit.collider.gameObject);
			if (partUpwardsCached != null && !partUpwardsCached.frozen)
			{
				result = Quaternion.FromToRotation(Vector3.right, base.transform.InverseTransformDirection(hit.normal));
			}
		}
		base.gameObject.SetLayerRecursive(layer);
		return result;
	}

	public void OnEditorEvent(ConstructionEventType evt, Part selPart)
	{
		if ((uint)(evt - 10) <= 3u)
		{
			UpdateTargetCoords();
		}
	}

	public void UpdateTargetCoords()
	{
		if (!(target != null))
		{
			return;
		}
		targetPosition = base.transform.InverseTransformPoint(target.transform.TransformPoint(wTgtPos));
		targetRotation = Quaternion.Inverse(base.transform.rotation) * target.transform.rotation * wTgtRot;
		direction = targetPosition.normalized;
		if (attachState == AttachState.Attached)
		{
			int num = cmpModules.Length;
			while (num-- > 0)
			{
				cmpModules[num].OnTargetUpdate();
			}
		}
	}

	public ConfigNode ParseCustomPartData(string customPartData)
	{
		ConfigNode configNode = new ConfigNode();
		if (customPartData != string.Empty)
		{
			Debug.LogWarning("[CompoundPart]: Deprecated 'customPartData' field found. Upgrading to new format...");
			string[] array = customPartData.Split(';');
			foreach (string text in array)
			{
				if (!text.Contains(":"))
				{
					continue;
				}
				string text2 = text.Split(':')[0].Trim();
				string text3 = text.Split(':')[1].Trim();
				switch (text2)
				{
				case "rot":
					configNode.AddValue("rot", text3);
					break;
				case "pos":
					configNode.AddValue("pos", text3);
					break;
				case "dir":
					configNode.AddValue("dir", text3);
					break;
				case "tgt":
					if (text3.Contains("_"))
					{
						int index = int.Parse(text3.Split('_')[1].Trim());
						uint value = 0u;
						if (HighLogic.LoadedSceneIsEditor)
						{
							value = EditorLogic.SortedShipList[index].craftID;
						}
						else if (HighLogic.LoadedSceneIsFlight)
						{
							value = vessel.parts[index].craftID;
						}
						configNode.AddValue("tgt", value);
					}
					break;
				}
			}
		}
		return configNode;
	}
}

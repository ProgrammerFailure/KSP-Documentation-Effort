using System.Collections.Generic;
using ns9;
using UnityEngine;

public class ModuleJettison : PartModule, IActivateOnDecouple, IScalarModule, IMultipleDragCube, IPartMassModifier
{
	[SerializeField]
	public Transform _jettisonTransform;

	[KSPField]
	public string bottomNodeName = "bottom";

	[KSPField]
	public bool checkBottomNode;

	[KSPField]
	public bool decoupleEnabled = true;

	[KSPField]
	public bool isFairing = true;

	[KSPField(isPersistant = true)]
	public string activejettisonName = "";

	[KSPField]
	public string jettisonName = "Fairing";

	[KSPField]
	public string menuName = "";

	[KSPField]
	public string actionSuffix = "";

	[KSPField]
	public bool useMultipleDragCubes = true;

	[KSPField]
	public bool useProceduralDragCubes;

	[KSPField(isPersistant = true)]
	public bool isJettisoned;

	[UI_Toggle(disabledText = "#autoLOC_6001072", enabledText = "#autoLOC_6001071")]
	[KSPField(isPersistant = true, guiActiveEditor = true, guiName = "#autoLOC_6001455")]
	public bool shroudHideOverride;

	[KSPField]
	public bool hideJettisonMenu;

	[KSPField]
	public bool allowShroudToggle = true;

	[KSPField]
	public bool useCalculatedMass;

	[KSPField]
	public bool modifyJettisonedMass;

	[KSPField]
	public float jettisonedObjectMass = 0.1f;

	[KSPField]
	public float jettisonForce = 5f;

	[KSPField]
	public bool ignoreNodes;

	[KSPField]
	public Vector3 jettisonDirection = new Vector3(0f, 0f, 1f);

	[KSPField]
	public bool manualJettison;

	public float forceScalar = 10f;

	public bool inEditor;

	public Vector3 partVolume;

	public float partDensity;

	public MaterialColorUpdater jettisonTemperatureRenderer;

	public bool hasVariantDragCubes;

	public bool setupComplete;

	public EventData<float, float> OnJettisonStart = new EventData<float, float>("JettisonStart");

	public EventData<float> OnJettisonEnd = new EventData<float>("JettisonEnd");

	[KSPField]
	public string moduleID = "jettison";

	public Transform jettisonTransform
	{
		get
		{
			return _jettisonTransform;
		}
		set
		{
			_jettisonTransform = value;
		}
	}

	public bool IsMultipleCubesActive => useMultipleDragCubes;

	public string ScalarModuleID => moduleID;

	public float GetScalar
	{
		get
		{
			if (!isJettisoned)
			{
				return 0f;
			}
			return 1f;
		}
	}

	public bool CanMove => !isJettisoned;

	public EventData<float, float> OnMoving => OnJettisonStart;

	public EventData<float> OnStop => OnJettisonEnd;

	[KSPAction("#autoLOC_6001456", activeEditor = false)]
	public void JettisonAction(KSPActionParam param)
	{
		Jettison();
	}

	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001456")]
	public void Jettison()
	{
		if (checkBottomNode)
		{
			AttachNode attachNode = base.part.FindAttachNode(bottomNodeName);
			if (attachNode != null && attachNode.attachedPart != null)
			{
				Debug.Log("Checked bottom node and found an attached part. Unable to Jettison");
				return;
			}
		}
		if (!isJettisoned && jettisonTransform != null)
		{
			base.part.partRendererBoundsIgnore.Add(jettisonTransform.name);
			bool flag = false;
			if (jettisonTransform.parent != null && jettisonTransform.parent.gameObject.GetComponent<ModuleDockingNode>() != null)
			{
				jettisonTransform.SetParent(null);
				Debug.Log("Jettisoned object does not have a collider, but is attached to a docking node. Creating a Convex Mesh Collider for it");
				MeshCollider meshCollider = jettisonTransform.gameObject.AddComponent<MeshCollider>();
				meshCollider.convex = true;
				meshCollider.enabled = false;
				flag = true;
			}
			if (!isFairing || flag)
			{
				OnMoving.Fire(0f, 1f);
				List<Collider> list = new List<Collider>();
				if (jettisonTransform != null)
				{
					jettisonTransform.GetComponentsInChildren(list);
				}
				List<Collider> list2 = base.part.FindModelComponents<Collider>();
				int count = list2.Count;
				while (count-- > 0)
				{
					int count2 = list.Count;
					while (count2-- > 0)
					{
						if (list[count2] == list2[count])
						{
							list2.RemoveAt(count);
							break;
						}
					}
				}
				if (list.Count > 0)
				{
					int count3 = list.Count;
					while (count3-- > 0)
					{
						list[count3].enabled = true;
					}
					SetIgnoreCollisionFlags(list, list2);
					AttachNode attachNode2 = base.part.FindAttachNode(bottomNodeName);
					if (attachNode2 != null && attachNode2.attachedPart != null)
					{
						List<Collider> set = attachNode2.attachedPart.FindModelComponents<Collider>();
						SetIgnoreCollisionFlags(list, set);
					}
				}
				if (jettisonTransform != null)
				{
					Rigidbody rigidbody = base.part.Rigidbody;
					if (rigidbody != null)
					{
						Rigidbody rb = physicalObject.ConvertToPhysicalObject(base.part, jettisonTransform.gameObject).rb;
						base.part.ResetCollisions();
						rb.maxAngularVelocity = PhysicsGlobals.MaxAngularVelocity;
						rb.angularVelocity = rigidbody.angularVelocity;
						Vector3 lhs = base.vessel.CurrentCoM - rigidbody.worldCenterOfMass;
						rb.velocity = rigidbody.velocity + Vector3.Cross(lhs, rb.angularVelocity);
						if (useCalculatedMass)
						{
							rb.SetDensity(partDensity);
						}
						else
						{
							rb.mass = jettisonedObjectMass;
						}
						rb.useGravity = false;
						if (!isFairing)
						{
							rb.AddForceAtPosition(jettisonDirection * jettisonForce * 0.5f, base.transform.position, ForceMode.Force);
							base.part.AddForceAtPosition(-jettisonDirection * jettisonForce * 0.5f, base.transform.position);
						}
						jettisonTemperatureRenderer = null;
					}
				}
			}
		}
		isJettisoned = true;
		SetFairingDragCube(fairingState: false);
		OnStop.Fire(1f);
		base.Events["Jettison"].active = false;
	}

	public void onVariantApplied(Part eventPart, PartVariant variant)
	{
		CheckAttachNode();
	}

	public override void OnLoad(ConfigNode node)
	{
		base.OnLoad(node);
		base.Fields["shroudHideOverride"].guiActiveEditor = allowShroudToggle;
	}

	public override void OnInventoryModeDisable()
	{
		if (HighLogic.LoadedSceneIsFlight && UIPartActionControllerInventory.Instance != null)
		{
			UIPartActionControllerInventory.Instance.StartCoroutine(CallbackUtil.DelayedCallback(1, delegate
			{
				SetVariants();
				SetupJettisonState(isEditor: false);
			}));
		}
		else
		{
			SetVariants();
			SetupJettisonState(isEditor: false);
		}
	}

	public Transform FetchActiveMeshTransform(string nameList)
	{
		string[] array = nameList.Split(',');
		int num = array.Length;
		int num2 = 0;
		Transform transform;
		while (true)
		{
			if (num2 < num)
			{
				transform = base.part.FindModelTransform(array[num2]);
				if (transform != null && transform.gameObject.activeSelf)
				{
					break;
				}
				num2++;
				continue;
			}
			return jettisonTransform;
		}
		activejettisonName = array[num2];
		return transform;
	}

	public void LoadJettisonMeshState()
	{
		if (string.IsNullOrEmpty(activejettisonName))
		{
			jettisonTransform = FetchActiveMeshTransform(jettisonName);
		}
		else
		{
			jettisonTransform = FetchActiveMeshTransform(activejettisonName);
		}
	}

	public override void OnStart(StartState state)
	{
		SetVariants();
		GameEvents.onVariantApplied.Add(onVariantApplied);
		if (menuName != string.Empty)
		{
			base.Events["Jettison"].guiName = menuName;
			base.Actions["JettisonAction"].guiName = menuName;
		}
		base.Fields["shroudHideOverride"].guiActiveEditor = allowShroudToggle;
		isJettisoned = isJettisoned || shroudHideOverride;
		if (state == StartState.Editor)
		{
			if (jettisonTransform != null && !ignoreNodes)
			{
				jettisonTransform.gameObject.SetActive(value: false);
			}
			inEditor = true;
		}
		else
		{
			inEditor = false;
		}
		base.Events["Jettison"].active = false;
		partVolume = base.part.collider.bounds.size;
		partDensity = partVolume.x * partVolume.y * partVolume.z / base.part.mass;
		if (!string.IsNullOrEmpty(actionSuffix))
		{
			SetActionsSuffix();
		}
	}

	public void SetVariants()
	{
		if (base.part.variants != null && base.part.variants.IsMultipleCubesActive)
		{
			hasVariantDragCubes = true;
		}
	}

	public void SetActionsSuffix()
	{
		base.Actions["JettisonAction"].guiName = base.Actions["JettisonAction"].guiName + " (" + actionSuffix + ")";
		base.Events["Jettison"].guiName = base.Events["Jettison"].guiName + " (" + actionSuffix + ")";
		base.Fields["shroudHideOverride"].guiName = base.Fields["shroudHideOverride"].guiName + " (" + actionSuffix + ")";
	}

	public void SetupJettisonState(bool isEditor)
	{
		LoadJettisonMeshState();
		if (!isEditor && !ignoreNodes)
		{
			if ((base.part.FindAttachNode(bottomNodeName).attachedPart == null && !ignoreNodes) || isJettisoned)
			{
				if (jettisonTransform != null && !ignoreNodes)
				{
					jettisonTransform.gameObject.SetActive(value: false);
				}
				SetFairingDragCube(fairingState: false);
			}
			else if (!isJettisoned)
			{
				if (jettisonTransform != null)
				{
					jettisonTransform.gameObject.SetActive(value: true);
				}
				SetFairingDragCube(fairingState: true);
			}
		}
		if (!isJettisoned && !isEditor && isFairing)
		{
			AttachNode attachNode = base.part.FindAttachNode(bottomNodeName);
			if (attachNode != null && jettisonTransform != null && attachNode.attachedPart != null)
			{
				Collider[] componentsInChildren = jettisonTransform.GetComponentsInChildren<Collider>();
				if (componentsInChildren != null)
				{
					int i = 0;
					for (int num = componentsInChildren.Length; i < num; i++)
					{
						Object.Destroy(componentsInChildren[i]);
					}
				}
				if (decoupleEnabled)
				{
					jettisonTransform.parent = attachNode.attachedPart.gameObject.transform;
				}
			}
		}
		if (jettisonTransform != null && isJettisoned)
		{
			jettisonTransform.gameObject.SetActive(value: false);
		}
		if (jettisonTransform != null && !isJettisoned && !isEditor && !isFairing)
		{
			List<Collider> list = new List<Collider>();
			jettisonTransform.GetComponentsInChildren(list);
			List<Collider> list2 = base.part.FindModelComponents<Collider>();
			int count = list2.Count;
			while (count-- > 0)
			{
				int count2 = list.Count;
				while (count2-- > 0)
				{
					if (list[count2] == list2[count])
					{
						list2.RemoveAt(count);
						break;
					}
				}
			}
			if (list.Count > 0)
			{
				SetIgnoreCollisionFlags(list, list2);
				AttachNode attachNode2 = base.part.FindAttachNode(bottomNodeName);
				if (attachNode2 != null && attachNode2.attachedPart != null)
				{
					List<Collider> set = attachNode2.attachedPart.FindModelComponents<Collider>();
					SetIgnoreCollisionFlags(list, set);
				}
				int count3 = list.Count;
				while (count3-- > 0)
				{
					list[count3].enabled = false;
				}
			}
			else
			{
				Debug.Log("Jettisoned object does not have a collider, creating a Convex Mesh Collider for it");
				MeshCollider meshCollider = jettisonTransform.gameObject.AddComponent<MeshCollider>();
				meshCollider.convex = true;
				SetIgnoreCollisionFlags(new List<Collider> { meshCollider }, list2);
				meshCollider.enabled = false;
			}
		}
		if (_jettisonTransform != null)
		{
			jettisonTemperatureRenderer = new MaterialColorUpdater(_jettisonTransform, PhysicsGlobals.TemperaturePropertyID, base.part);
		}
	}

	public void SetIgnoreCollisionFlags(List<Collider> set1, List<Collider> set2)
	{
		int i = 0;
		for (int count = set1.Count; i < count; i++)
		{
			int j = 0;
			for (int count2 = set2.Count; j < count2; j++)
			{
				Physics.IgnoreCollision(set1[i], set2[j]);
			}
		}
	}

	public override void OnActive()
	{
		if (!manualJettison && !isJettisoned && stagingEnabled)
		{
			Jettison();
		}
	}

	public void Update()
	{
		if (jettisonTemperatureRenderer != null && !isJettisoned)
		{
			if (HighLogic.LoadedSceneIsFlight)
			{
				jettisonTemperatureRenderer.Update(PhysicsGlobals.GetBlackBodyRadiation((float)base.part.skinTemperature, base.part));
			}
			else
			{
				jettisonTemperatureRenderer.Update(Color.clear);
			}
		}
	}

	public void LateUpdate()
	{
		if (!setupComplete)
		{
			SetupJettisonState(inEditor);
			setupComplete = true;
		}
		if (inEditor)
		{
			CheckAttachNode();
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			if (!isJettisoned && !shroudHideOverride && !hideJettisonMenu && !isFairing)
			{
				base.Events["Jettison"].active = true;
			}
			else
			{
				base.Events["Jettison"].active = false;
			}
		}
	}

	public void CheckAttachNode()
	{
		jettisonTransform = FetchActiveMeshTransform(jettisonName);
		AttachNode attachNode = base.part.FindAttachNode(bottomNodeName);
		if ((attachNode != null && attachNode.attachedPart == null && !ignoreNodes) || shroudHideOverride)
		{
			isJettisoned = true;
			if (jettisonTransform != null && !ignoreNodes)
			{
				jettisonTransform.gameObject.SetActive(value: false);
				SetFairingDragCube(fairingState: false);
			}
		}
		else if (jettisonTransform != null)
		{
			isJettisoned = false;
			jettisonTransform.gameObject.SetActive(value: true);
			SetFairingDragCube(fairingState: true);
		}
	}

	public void OnDestroy()
	{
		GameEvents.onVariantApplied.Remove(onVariantApplied);
	}

	public void SetFairingDragCube(bool fairingState)
	{
		if (!IsMultipleCubesActive)
		{
			return;
		}
		if (fairingState)
		{
			if (hasVariantDragCubes)
			{
				base.part.variants.SetCubes(state: false);
			}
			else
			{
				base.part.DragCubes.SetCubeWeight("Clean", 0f);
			}
			base.part.DragCubes.SetCubeWeight("Fairing", 1f);
		}
		else if (hasVariantDragCubes)
		{
			base.part.variants.SetCubes(state: true);
		}
		else
		{
			base.part.DragCubes.SetCubeWeight("Fairing", 0f);
			base.part.DragCubes.SetCubeWeight("Clean", 1f);
		}
		bool resetProcTiming = true;
		if (HighLogic.LoadedSceneIsEditor)
		{
			resetProcTiming = !PhysicsGlobals.AeroDataDisplay;
		}
		base.part.DragCubes.ForceUpdate(weights: true, occlusion: true, resetProcTiming);
	}

	public string[] GetDragCubeNames()
	{
		return new string[2] { "Fairing", "Clean" };
	}

	public void AssumeDragCubePosition(string name)
	{
		string text = activejettisonName;
		if (string.IsNullOrEmpty(text))
		{
			text = jettisonName;
		}
		Transform transform = FetchActiveMeshTransform(text);
		if (transform == null)
		{
			return;
		}
		if (!(name == "Fairing"))
		{
			if (name == "Clean")
			{
				transform.gameObject.SetActive(value: false);
			}
		}
		else
		{
			transform.gameObject.SetActive(value: true);
		}
	}

	public bool UsesProceduralDragCubes()
	{
		return useProceduralDragCubes;
	}

	public void DecoupleAction(string nodeName, bool weDecouple)
	{
		if (!isJettisoned && !shroudHideOverride && decoupleEnabled && nodeName == bottomNodeName && !ignoreNodes)
		{
			Jettison();
		}
	}

	public float GetModuleMass(float defaultMass, ModifierStagingSituation sit)
	{
		if (!modifyJettisonedMass)
		{
			return 0f;
		}
		if (isJettisoned)
		{
			if (useCalculatedMass)
			{
				return 0f - partDensity;
			}
			return 0f - jettisonedObjectMass;
		}
		return 0f;
	}

	public ModifierChangeWhen GetModuleMassChangeWhen()
	{
		return ModifierChangeWhen.STAGED;
	}

	public void SetScalar(float t)
	{
	}

	public void SetUIRead(bool state)
	{
	}

	public void SetUIWrite(bool state)
	{
	}

	public bool IsMoving()
	{
		return false;
	}

	public override bool IsStageable()
	{
		if (!stagingEnabled && !stagingToggleEnabledEditor)
		{
			return stagingToggleEnabledFlight;
		}
		return true;
	}

	public override string GetStagingEnableText()
	{
		if (!string.IsNullOrEmpty(stagingEnableText))
		{
			return stagingEnableText;
		}
		return Localizer.Format("#autoLOC_243358");
	}

	public override string GetStagingDisableText()
	{
		if (!string.IsNullOrEmpty(stagingDisableText))
		{
			return stagingDisableText;
		}
		return Localizer.Format("#autoLOC_243359");
	}
}

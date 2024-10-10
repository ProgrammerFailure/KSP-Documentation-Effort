using System;
using System.Collections;
using System.Collections.Generic;
using ns9;
using ProceduralFairings;
using UnityEngine;

public class ModuleProceduralFairing : PartModule, IPartMassModifier, IPartCostModifier, IScalarModule, IModuleInfo, IDynamicCargoOccluder, IActivateOnDecouple
{
	[KSPField(isPersistant = true)]
	public uint interstageCraftID;

	[KSPField]
	public string fairingNode = "top";

	[KSPField]
	public int nSides = 48;

	[UI_FloatRange(scene = UI_Scene.Editor, stepIncrement = 1f, maxValue = 6f, minValue = 2f)]
	[KSPField(isPersistant = true, guiActiveEditor = true, guiName = "#autoLOC_6001394")]
	public float nArcs = 3f;

	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6006090")]
	[UI_Label(scene = UI_Scene.Editor)]
	public string editingBlocked;

	[KSPField]
	public int nCollidersPerXSection = 12;

	[KSPField]
	public int panelGrouping = 3;

	[KSPField]
	public Vector3 pivot = Vector3.zero;

	[KSPField]
	public Vector3 axis = Vector3.up;

	[KSPField]
	public float baseRadius = 1.25f;

	[KSPField]
	public float maxRadius = 6f;

	[KSPField]
	public float capRadius = 0.375f;

	[KSPField]
	public float snapThreshold = 0.25f;

	[KSPField]
	public float minHeightRadiusRatio = 0.07f;

	[KSPField]
	public float snapThresholdFineAdjust = 0.01f;

	[KSPField]
	public float xSectionHeightMin = 0.3f;

	[KSPField]
	public float xSectionHeightMax = 3f;

	[KSPField]
	public float xSectionHeightMinFineAdjust = 0.03f;

	[KSPField]
	public float xSectionHeightMaxFineAdjust = 4f;

	[KSPField]
	public float aberrantNormalLimit = 45f;

	[KSPField]
	public float edgeSlide = 0.1f;

	[KSPField]
	public float edgeWarp = 0.02f;

	[KSPField]
	public float noseTip = 0.5f;

	[KSPField]
	public string TextureURL = "";

	[KSPField]
	public string CapTextureURL = "";

	[KSPField]
	public string TextureNormalURL = "";

	[KSPField]
	public string CapTextureNormalURL = "";

	[KSPField]
	public string BaseModelTransformName = "Base";

	[KSPField]
	public string DefaultBaseTextureURL = "Squad/Parts/Aero/fairings/FairingBase";

	[KSPField]
	public string DefaultBaseNormalsURL = "Squad/Parts/Aero/fairings/FairingBaseNormals";

	[KSPField]
	public float UnitAreaMass = 0.04f;

	[KSPField]
	public float UnitAreaCost = 10f;

	[KSPField(isPersistant = true, guiActiveEditor = true, guiName = "#autoLOC_6001395")]
	[UI_FloatRange(scene = UI_Scene.Editor, stepIncrement = 5f, maxValue = 1000f, minValue = 0f)]
	public float ejectionForce = 100f;

	[KSPField]
	public float interstageOcclusionFudge = 1.03f;

	[KSPField]
	public int coneSweepRays = 120;

	[KSPField]
	public float coneSweepPrecision = 10f;

	[KSPField(isPersistant = true, guiActiveEditor = true, guiName = "#autoLOC_6001396")]
	[UI_Toggle(disabledText = "#autoLOC_6001073", enabledText = "#autoLOC_6001074")]
	public bool useClamshell;

	[UI_Toggle(disabledText = "#autoLOC_6006082", enabledText = "#autoLOC_6006083")]
	[KSPField(guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6006081")]
	public bool isFadeLocked;

	[KSPField(isPersistant = true)]
	public bool isCapped = true;

	public List<FairingXSection> xSections;

	public int nCollidersPerArc;

	[NonSerialized]
	public FairingXSection currentFairing;

	[NonSerialized]
	public FairingXSection prevFairing;

	public bool isTube;

	public float r;

	public float h;

	public float panelMass;

	public Material FairingMaterial;

	public Material FairingConeMaterial;

	public Material FairingFlightMaterial;

	public Material FairingFlightConeMaterial;

	public List<FairingPanel> Panels;

	public List<MeshCollider> ClosedColliders;

	public Transform modelTransform;

	public Renderer baseRenderer;

	public PartVariant cachedVariant;

	public bool applyVariantRequired;

	public float coneSweepOffset;

	public float srfLevel;

	public float srfHeight;

	public float closeRadius;

	public float reconnectSchedule;

	public float lastR;

	public bool coneSweepHit;

	public bool coneSweepHitLast;

	public bool coneSweepClear;

	public RaycastHit hit;

	public Part interstage;

	[SerializeField]
	public KerbalFSM fsm;

	public KFSMState st_editor_idle;

	public KFSMState st_editor_place;

	public KFSMEvent on_PartAttached;

	public KFSMEvent on_PartDetached;

	public KFSMEvent on_XSectionPlace;

	public KFSMEvent on_SymCPartUpdate;

	public KFSMEvent on_PlaceCap;

	public KFSMEvent on_Unplace;

	public KFSMEvent on_PlaceCancel;

	public KFSMEvent on_Edit;

	public KFSMEvent on_Delete;

	public KFSMEvent on_ReconnectInterstage;

	public KFSMEvent on_OpenCap;

	public KFSMEvent on_EndEdit;

	public KFSMState st_flight_idle;

	public KFSMState st_flight_deployed;

	public KFSMEvent on_Deploy;

	public KFSMEvent on_Breakoff;

	public KFSMEvent on_Breakoff_NoJettison;

	public string fsmStateName;

	public BaseEvent evtDeleteFairing;

	public BaseEvent evtEditFairing;

	public BaseEvent evtBuildFairing;

	public BaseEvent evtDeployFairing;

	public Vector3 prevMousePosition = Vector3.zero;

	public bool variantsCached;

	[NonSerialized]
	public ModuleCargoBay cargoModule;

	public bool partFlagSrfAttach;

	public Ray rayForFlag;

	public RaycastHit rayHitForFlag;

	public bool fairingsAreLocked;

	public Dictionary<Vector3, List<FairingPanel>> panelDict;

	public AttachNode payloadAttachNode;

	public PartJoint payloadJoint;

	public EventData<float, float> OnDeployStart = new EventData<float, float>("FairingDeployStart");

	public EventData<float> OnDeployEnd = new EventData<float>("FairingDeployEnd");

	[KSPField]
	public string moduleID = "fairing";

	public Vector3 originalCoP = Vector3.zero;

	public Vector3 originalCoM = Vector3.zero;

	public Vector3 originalCoL = Vector3.zero;

	public float dragCoPOffset;

	public static string cacheAutoLOC_6001005;

	public static string cacheAutoLOC_6001006;

	public static string cacheAutoLOC_6001007;

	public static string cacheAutoLOC_6001008;

	public static string cacheAutoLOC_6005081;

	public static string cacheAutoLOC_225016;

	public static string cacheAutoLOC_6005085 = "";

	public string ScalarModuleID => moduleID;

	public float GetScalar
	{
		get
		{
			if (fsm.CurrentState != st_flight_idle && (fsm.CurrentState != st_editor_idle || xSections.Count <= 0))
			{
				return 1f;
			}
			return 0f;
		}
	}

	public bool CanMove => fsm.CurrentState == st_flight_idle;

	public EventData<float, float> OnMoving => OnDeployStart;

	public EventData<float> OnStop => OnDeployEnd;

	public override void OnAwake()
	{
		fsmStateName = "";
		fsm = new KerbalFSM();
		interstage = null;
		if (Panels != null)
		{
			WipeMesh();
		}
		xSections = new List<FairingXSection>();
		Panels = new List<FairingPanel>();
		originalCoP = base.part.CoPOffset;
		originalCoM = base.part.CoMOffset;
		originalCoL = base.part.CoLOffset;
		GameEvents.onVariantsAdded.Add(onVariantsAdded);
	}

	public override void OnStart(StartState state)
	{
		if (HighLogic.LoadedSceneIsEditor)
		{
			GameEvents.onEditorPartEvent.Add(onEditorPartEvent);
			Part obj = base.part;
			obj.OnEditorAttach = (Callback)Delegate.Combine(obj.OnEditorAttach, new Callback(onSelfReattach));
			Part obj2 = base.part;
			obj2.OnEditorDetach = (Callback)Delegate.Combine(obj2.OnEditorDetach, new Callback(onSelfDetach));
		}
		FairingMaterial = new Material(Resources.Load("Procedural/FairingsMat") as Material);
		FairingConeMaterial = new Material(Resources.Load("Procedural/FairingsMat") as Material);
		FairingFlightMaterial = new Material(Resources.Load("Procedural/FairingsFlightMat") as Material);
		FairingFlightConeMaterial = new Material(Resources.Load("Procedural/FairingsFlightMat") as Material);
		modelTransform = base.part.FindModelTransform("model");
		GameObject child = base.gameObject.GetChild(BaseModelTransformName);
		if (child != null)
		{
			baseRenderer = child.GetComponent<Renderer>();
		}
		base.Fields["nArcs"].OnValueModified += OnFairingSidesChanged;
		base.Fields["nArcs"].OnValueModified += OnFairingSidesChanged;
		variantsCached = true;
		if (TextureURL != string.Empty)
		{
			Texture2D texture = GameDatabase.Instance.GetTexture(TextureURL, asNormalMap: false);
			FairingMaterial.mainTexture = texture;
			FairingFlightMaterial.mainTexture = texture;
			if (TextureNormalURL != string.Empty)
			{
				Texture2D texture2 = GameDatabase.Instance.GetTexture(TextureNormalURL, asNormalMap: true);
				FairingMaterial.SetTexture("_BumpMap", texture2);
				FairingMaterial.shader = Shader.Find("KSP/Bumped Specular (Cutoff)");
				FairingFlightMaterial.SetTexture("_BumpMap", texture2);
				FairingFlightMaterial.shader = Shader.Find("KSP/Bumped Specular Opaque (Cutoff)");
			}
			else
			{
				FairingMaterial.shader = Shader.Find("KSP/Specular (Cutoff)");
				FairingFlightMaterial.shader = Shader.Find("KSP/Specular Opaque (Cutoff)");
			}
		}
		if (CapTextureURL != string.Empty)
		{
			Texture2D texture3 = GameDatabase.Instance.GetTexture(CapTextureURL, asNormalMap: false);
			FairingConeMaterial.mainTexture = texture3;
			FairingFlightConeMaterial.mainTexture = texture3;
			if (CapTextureNormalURL != string.Empty)
			{
				Texture2D texture4 = GameDatabase.Instance.GetTexture(CapTextureNormalURL, asNormalMap: true);
				FairingConeMaterial.SetTexture("_BumpMap", texture4);
				FairingConeMaterial.shader = Shader.Find("KSP/Bumped Specular (Cutoff)");
				FairingFlightConeMaterial.SetTexture("_BumpMap", texture4);
				FairingFlightConeMaterial.shader = Shader.Find("KSP/Bumped Specular Opaque (Cutoff)");
			}
			else
			{
				FairingConeMaterial.shader = Shader.Find("KSP/Specular (Cutoff)");
				FairingFlightConeMaterial.shader = Shader.Find("KSP/Specular Opaque (Cutoff)");
			}
		}
		base.part.baseVariant.TryCopyMaterial(FairingMaterial);
		base.part.baseVariant.TryCopyMaterial(FairingFlightMaterial);
		base.part.findFxGroup("activate")?.setActive(value: false);
		evtDeleteFairing = base.Events["DeleteFairing"];
		evtEditFairing = base.Events["EditFairing"];
		evtBuildFairing = base.Events["BuildFairing"];
		evtDeployFairing = base.Events["DeployFairing"];
		evtDeleteFairing.active = false;
		evtEditFairing.active = false;
		evtBuildFairing.active = false;
		evtDeployFairing.active = false;
		if (base.part.stagingIcon == string.Empty && overrideStagingIconIfBlank)
		{
			base.part.stagingIcon = DefaultIcons.FUEL_TANK.ToString();
		}
		cargoModule = base.part.FindModuleImplementing<ModuleCargoBay>();
		if (xSections.Count > 0)
		{
			SpawnMeshes(addColliders: true);
		}
		if (HighLogic.LoadedSceneIsEditor)
		{
			SetupEditorFSM();
			fsm.StartFSM((!string.IsNullOrEmpty(fsmStateName)) ? fsmStateName : "st_idle");
		}
		else if (HighLogic.LoadedSceneIsFlight)
		{
			GameEvents.onVesselWasModified.Add(onVesselModified);
			GameEvents.onPartDie.Add(onPartDestroyed);
			SetupFlightFSM();
			fsm.StartFSM((!string.IsNullOrEmpty(fsmStateName)) ? fsmStateName : "st_idle");
		}
		if (interstageCraftID != 0)
		{
			if (HighLogic.LoadedSceneIsEditor)
			{
				SetInterstage(ShipConstruction.FindPartWithCraftID(interstageCraftID));
			}
			else if (!HighLogic.LoadedSceneIsMissionBuilder)
			{
				SetInterstage(base.vessel.parts.Find((Part p) => p.craftID == interstageCraftID && p.missionID == base.part.missionID));
			}
		}
		if (HighLogic.LoadedSceneIsFlight && xSections.Count > 0)
		{
			StartCoroutine(CallbackUtil.WaitUntil(() => base.part.started, PayloadStrutsSetup));
		}
		if (base.part.variants != null)
		{
			GameEvents.onVariantApplied.Add(onVariantApplied);
			onVariantApplied(base.part, base.part.variants.SelectedVariant);
		}
		panelMass = GetFairingArea() * UnitAreaMass;
		if (HighLogic.LoadedSceneIsEditor)
		{
			GameEvents.onEditorShipModified.Fire(EditorLogic.fetch.ship);
		}
		ParentChildFlagsParts();
		CheckForFairingEdit();
	}

	public void OnFairingSidesChanged(object arg1)
	{
		WipeMesh();
		SpawnMeshes(addColliders: true);
	}

	public void onEditorPartEvent(ConstructionEventType evt, Part p)
	{
		switch (evt)
		{
		case ConstructionEventType.PartDetached:
			if (p == base.part && interstage != null)
			{
				DumpInterstage();
				WipeMesh();
			}
			break;
		case ConstructionEventType.PartAttached:
			if (p == base.part)
			{
				if (xSections.Count == 0)
				{
					fsm.RunEvent(on_PartAttached);
				}
			}
			else if (base.part.symmetryCounterparts != null && base.part.symmetryCounterparts.Count > 0)
			{
				ModuleProceduralFairing moduleProceduralFairing = base.part.symmetryCounterparts[0].FindModuleImplementing<ModuleProceduralFairing>();
				if (moduleProceduralFairing != null && moduleProceduralFairing.xSections != null && moduleProceduralFairing.xSections.Count > 0)
				{
					UpdatePanelsFromCPart(moduleProceduralFairing.xSections);
				}
			}
			break;
		}
	}

	public void onVariantsAdded(AvailablePart aP)
	{
		if (!variantsCached && aP == base.part.partInfo)
		{
			for (int i = 0; i < aP.Variants.Count; i++)
			{
				onVariantApplied(base.part, aP.Variants[i]);
			}
			onVariantApplied(base.part, aP.Variants[0]);
			variantsCached = true;
		}
	}

	public void onVariantApplied(Part part, PartVariant variant)
	{
		if (part == null || part != base.part)
		{
			return;
		}
		cachedVariant = variant;
		string text = variant.GetExtraInfoValue("shaderName");
		if (string.IsNullOrEmpty(text))
		{
			text = "KSP/Bumped Specular (Cutoff)";
		}
		if (Panels.Count > 0)
		{
			for (int i = 0; i < Panels.Count; i++)
			{
				ApplyVariantToPanel(Panels[i], variant);
			}
			if (!text.Equals("KSP/Bumped Specular (Cutoff)"))
			{
				FairingFlightMaterial = Panels[0].go.GetComponent<Renderer>().material;
			}
			applyVariantRequired = false;
		}
		else
		{
			applyVariantRequired = true;
		}
		string extraInfoValue = variant.GetExtraInfoValue("BaseMaterialName");
		string extraInfoValue2 = variant.GetExtraInfoValue("BaseTextureName");
		string extraInfoValue3 = variant.GetExtraInfoValue("BaseNormalsName");
		Texture2D texture2D = ((baseRenderer != null) ? (baseRenderer.material.mainTexture as Texture2D) : null);
		Texture2D texture2D2 = null;
		bool flag = baseRenderer != null && baseRenderer.material.HasProperty("_BumpMap");
		if (baseRenderer != null && flag)
		{
			texture2D2 = baseRenderer.material.GetTexture("_BumpMap") as Texture2D;
		}
		if (text.Equals("KSP/Bumped Specular (Cutoff)"))
		{
			text = "KSP/Bumped Specular";
		}
		if (!string.IsNullOrEmpty(extraInfoValue2))
		{
			texture2D = GameDatabase.Instance.GetTexture(extraInfoValue2, asNormalMap: false);
			if (texture2D == null)
			{
				texture2D = ((baseRenderer != null) ? (baseRenderer.material.mainTexture as Texture2D) : GameDatabase.Instance.GetTexture(DefaultBaseTextureURL, asNormalMap: false));
			}
		}
		if (!string.IsNullOrEmpty(extraInfoValue3) && flag)
		{
			texture2D2 = GameDatabase.Instance.GetTexture(extraInfoValue3, asNormalMap: true);
			if (texture2D2 == null)
			{
				texture2D2 = ((baseRenderer != null) ? (baseRenderer.material.GetTexture("_BumpMap") as Texture2D) : GameDatabase.Instance.GetTexture(DefaultBaseNormalsURL, asNormalMap: false));
			}
		}
		int num = 0;
		while (true)
		{
			if (num < variant.Materials.Count)
			{
				if (variant.Materials[num].name.StartsWith(extraInfoValue))
				{
					break;
				}
				if (HighLogic.LoadedSceneIsEditor && variant.Materials[num].name.Contains("IconShell") && !string.IsNullOrEmpty(variant.GetExtraInfoValue("FairingsTextureURL")))
				{
					variant.Materials[num].SetTexture("_MainTex", GameDatabase.Instance.GetTexture(variant.GetExtraInfoValue("FairingsTextureURL"), asNormalMap: false));
				}
				num++;
				continue;
			}
			return;
		}
		variant.Materials[num].shader = Shader.Find(text);
		variant.Materials[num].mainTexture = texture2D;
		if (variant.Materials[num].HasProperty("_BumpMap"))
		{
			variant.Materials[num].SetTexture("_BumpMap", texture2D2);
		}
		variant.UpdateMaterialFromExtraInfo(variant.Materials[num]);
		if (baseRenderer != null)
		{
			baseRenderer.material = variant.Materials[num];
		}
	}

	public void ApplyVariantToPanel(FairingPanel panel, PartVariant variant)
	{
		string value = variant.GetExtraInfoValue("shaderName");
		string text = variant.GetExtraInfoValue("FairingsTextureURL");
		string url = variant.GetExtraInfoValue("FairingsNormalURL");
		if (string.IsNullOrEmpty(value))
		{
			value = ((!HighLogic.LoadedSceneIsFlight) ? "KSP/Bumped Specular (Cutoff)" : "KSP/Bumped Specular Opaque (Cutoff)");
		}
		if (string.IsNullOrEmpty(text))
		{
			text = TextureURL;
			url = TextureNormalURL;
		}
		Texture texture = GameDatabase.Instance.GetTexture(text, asNormalMap: false);
		Texture texture2 = GameDatabase.Instance.GetTexture(url, asNormalMap: false);
		FairingMaterial.mainTexture = texture;
		FairingMaterial.SetTexture("_BumpMap", texture2);
		if (!panel.isCap)
		{
			Material material = panel.go.GetComponent<Renderer>().material;
			material.shader = Shader.Find(value);
			material.mainTexture = texture;
			material.SetTexture("_BumpMap", texture2);
			variant.UpdateMaterialFromExtraInfo(material);
		}
	}

	public void ParentChildFlagsParts()
	{
		FlagDecalBackground flagDecalBackground = null;
		for (int i = 0; i < base.part.children.Count; i++)
		{
			flagDecalBackground = base.part.children[i].GetComponent<FlagDecalBackground>();
			if (!(flagDecalBackground != null))
			{
				continue;
			}
			for (int j = 0; j < xSections.Count; j++)
			{
				int panelIndexFromFlag = xSections[j].GetPanelIndexFromFlag(base.part.children[i].craftID, (uint)flagDecalBackground.placementID);
				if (panelIndexFromFlag >= 0 && panelIndexFromFlag < Panels.Count && Panels[panelIndexFromFlag] != null)
				{
					Panels[panelIndexFromFlag].AddFlag(base.part.children[i]);
				}
			}
		}
	}

	public void SetInterstage(Part iStg)
	{
		if (!(iStg != interstage))
		{
			return;
		}
		if (iStg == null)
		{
			DumpInterstage();
			return;
		}
		if (interstage != null && HighLogic.LoadedSceneIsEditor)
		{
			Part obj = interstage;
			obj.OnEditorAttach = (Callback)Delegate.Remove(obj.OnEditorAttach, new Callback(onInterstageReattach));
			Part obj2 = interstage;
			obj2.OnEditorDetach = (Callback)Delegate.Remove(obj2.OnEditorDetach, new Callback(onInterstageDetach));
			Part obj3 = interstage;
			obj3.OnEditorDestroy = (Callback)Delegate.Remove(obj3.OnEditorDestroy, new Callback(onInterstageDestroy));
		}
		interstage = iStg;
		interstageCraftID = interstage.craftID;
		if (HighLogic.LoadedSceneIsEditor)
		{
			Part obj4 = interstage;
			obj4.OnEditorAttach = (Callback)Delegate.Combine(obj4.OnEditorAttach, new Callback(onInterstageReattach));
			Part obj5 = interstage;
			obj5.OnEditorDetach = (Callback)Delegate.Combine(obj5.OnEditorDetach, new Callback(onInterstageDetach));
			Part obj6 = interstage;
			obj6.OnEditorDestroy = (Callback)Delegate.Combine(obj6.OnEditorDestroy, new Callback(onInterstageDestroy));
		}
	}

	public void DumpInterstage()
	{
		if (interstage != null && HighLogic.LoadedSceneIsEditor)
		{
			Part obj = interstage;
			obj.OnEditorAttach = (Callback)Delegate.Remove(obj.OnEditorAttach, new Callback(onInterstageReattach));
			Part obj2 = interstage;
			obj2.OnEditorDetach = (Callback)Delegate.Remove(obj2.OnEditorDetach, new Callback(onInterstageDetach));
			Part obj3 = interstage;
			obj3.OnEditorDestroy = (Callback)Delegate.Remove(obj3.OnEditorDestroy, new Callback(onInterstageDestroy));
		}
		interstage = null;
		interstageCraftID = 0u;
		base.part.FindAttachNode(fairingNode).overrideDragArea = -1f;
	}

	public void onInterstageReattach()
	{
		if (interstage != null && interstage.localRoot == base.part.localRoot)
		{
			fsm.RunEvent(on_ReconnectInterstage);
		}
	}

	public void onInterstageDetach()
	{
		if (interstage != null && interstage.localRoot != base.part.localRoot)
		{
			WipeMesh();
		}
	}

	public void onInterstageDestroy()
	{
		if (interstage != null && HighLogic.LoadedSceneIsEditor)
		{
			Part obj = interstage;
			obj.OnEditorAttach = (Callback)Delegate.Remove(obj.OnEditorAttach, new Callback(onInterstageReattach));
			Part obj2 = interstage;
			obj2.OnEditorDetach = (Callback)Delegate.Remove(obj2.OnEditorDetach, new Callback(onInterstageDetach));
			Part obj3 = interstage;
			obj3.OnEditorDestroy = (Callback)Delegate.Remove(obj3.OnEditorDestroy, new Callback(onInterstageDestroy));
		}
		WipeMesh();
		interstage = null;
		interstageCraftID = 0u;
	}

	public void onSelfReattach()
	{
		if (fsm.CurrentState == st_editor_idle && interstage != null && interstage.localRoot == base.part.localRoot)
		{
			fsm.RunEvent(on_ReconnectInterstage);
		}
	}

	public void onSelfDetach()
	{
		if (fsm.CurrentState == st_editor_idle && interstage != null && interstage.localRoot != base.part.localRoot)
		{
			WipeMesh();
		}
	}

	public void onVesselModified(Vessel v)
	{
		if (interstage != null)
		{
			if ((v == base.vessel || v == interstage.vessel) && base.vessel != interstage.vessel && base.vessel != FlightGlobals.ActiveVessel)
			{
				fsm.RunEvent(on_Breakoff_NoJettison);
			}
		}
		else
		{
			interstageCraftID = 0u;
		}
	}

	public void onPartDestroyed(Part p)
	{
		if (p == interstage && interstage != null)
		{
			fsm.RunEvent(on_Breakoff);
		}
	}

	public void LockEditor(bool lockState)
	{
		if (lockState)
		{
			InputLockManager.SetControlLock(ControlTypes.EDITOR_SOFT_LOCK_ALLOW_SNAP, "proceduralFairingLock");
		}
		else
		{
			InputLockManager.RemoveControlLock("proceduralFairingLock");
		}
	}

	public void OpenFairing(object field)
	{
		fsm.RunEvent(on_OpenCap);
	}

	public void OnDestroy()
	{
		Part obj = base.part;
		obj.OnEditorDetach = (Callback)Delegate.Remove(obj.OnEditorDetach, new Callback(onSelfDetach));
		Part obj2 = base.part;
		obj2.OnEditorAttach = (Callback)Delegate.Remove(obj2.OnEditorAttach, new Callback(onSelfReattach));
		base.Fields["nArcs"].OnValueModified -= OnFairingSidesChanged;
		if (interstage != null)
		{
			DumpInterstage();
		}
		InputLockManager.RemoveControlLock("proceduralFairingLock");
		GameEvents.onVesselWasModified.Remove(onVesselModified);
		GameEvents.onPartDie.Remove(onPartDestroyed);
		GameEvents.onEditorPartEvent.Remove(onEditorPartEvent);
		if (base.part.variants != null)
		{
			GameEvents.onVariantApplied.Remove(onVariantApplied);
		}
	}

	public void OnDrawGizmos()
	{
		GizmoDisplay();
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
		if (fsm.Started)
		{
			fsm.LateUpdateFSM();
		}
	}

	public void SetupEditorFSM()
	{
		if (cacheAutoLOC_6005085 == "")
		{
			cacheAutoLOC_6005085 = Localizer.Format("#autoLOC_6005085", GameSettings.MODIFIER_KEY.name);
		}
		st_editor_idle = new KFSMState("st_idle");
		st_editor_idle.OnEnter = delegate
		{
			evtDeleteFairing.active = (evtEditFairing.active = xSections.Count > 0);
			evtBuildFairing.active = !evtDeleteFairing.active;
			panelMass = GetFairingArea() * UnitAreaMass;
		};
		st_editor_idle.OnLateUpdate = delegate
		{
			if (xSections.Count > 0 && fsm.TimeAtCurrentState > 1.0 && prevMousePosition != Input.mousePosition)
			{
				partFlagSrfAttach = (GameSettings.MODIFIER_KEY.GetKey() || isFadeLocked) && EditorLogic.SelectedPart != null && (bool)EditorLogic.SelectedPart.GetComponent<FlagDecalBackground>();
				MouseFadeUpdate();
				prevMousePosition = Input.mousePosition;
			}
		};
		st_editor_idle.OnLeave = delegate
		{
			evtBuildFairing.active = false;
			evtDeleteFairing.active = false;
			evtEditFairing.active = false;
		};
		fsm.AddState(st_editor_idle);
		st_editor_place = new KFSMState("st_place");
		st_editor_place.OnEnter = delegate
		{
			LockEditor(lockState: true);
			coneSweepClear = false;
			coneSweepHit = false;
			coneSweepHitLast = false;
			coneSweepOffset = 0f;
			panelMass = GetFairingArea() * UnitAreaMass;
			currentFairing = xSections[xSections.Count - 1];
			prevFairing = xSections[xSections.Count - 2];
		};
		st_editor_place.OnUpdate = delegate
		{
			Vector3 wAxis = base.transform.rotation * axis;
			Vector3 wPivot = base.transform.TransformPoint(pivot);
			Vector3 wFwd = base.transform.rotation * Vector3.forward;
			closeRadius = capRadius;
			UpdateXSectionPlacement(currentFairing, prevFairing, wAxis, wPivot);
			bool flag = true;
			if (UpdateXSectionConeCast(currentFairing, prevFairing, wAxis, wPivot, wFwd, out hit, resetOnMouseMove: true) && hit.collider != null && hit.collider.gameObject != null)
			{
				SetInterstage(FlightGlobals.GetPartUpwardsCached(hit.collider.gameObject));
				flag = false;
			}
			if (flag)
			{
				DumpInterstage();
			}
			if (prevMousePosition != Input.mousePosition)
			{
				MeshPreviewUpdate(currentFairing, prevFairing);
			}
			panelMass = GetFairingArea() * UnitAreaMass;
		};
		st_editor_place.OnLateUpdate = delegate
		{
			prevMousePosition = Input.mousePosition;
			string text = XKCDColors.ColorTranslator.ToHex(currentFairing.color);
			string text2 = "";
			isTube = GameSettings.MODIFIER_KEY.GetKey();
			text2 = ((!willCapOnPlace(currentFairing)) ? cacheAutoLOC_6001006 : cacheAutoLOC_6001005);
			if (xSections.Count > 1)
			{
				text2 = text2 + "\n" + cacheAutoLOC_6005085;
			}
			string text3 = "";
			text3 = ((xSections.Count <= 2) ? cacheAutoLOC_6001008 : cacheAutoLOC_6001007);
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6001009", text, text2) + Localizer.Format("#autoLOC_6001010", text3) + Localizer.Format("#autoLOC_6001011", GetTotalCost().ToString("N2"), GetTotalMass().ToString("N2")) + "\n" + Localizer.Format("#autoLOC_8004190", "[" + GameSettings.Editor_toggleAngleSnap.name + "]", cacheAutoLOC_6005081), 0f, ScreenMessageStyle.LOWER_CENTER);
			GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartTweaked, base.part);
		};
		st_editor_place.OnLeave = delegate
		{
			LockEditor(lockState: false);
			panelMass = GetFairingArea() * UnitAreaMass;
		};
		fsm.AddState(st_editor_place);
		on_PartAttached = new KFSMEvent("onPartAttached");
		on_PartAttached.GoToStateOnEvent = st_editor_place;
		on_PartAttached.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_PartAttached.OnEvent = delegate
		{
			xSections.Clear();
			prevFairing = PlaceNewXSection();
			prevFairing.h = 0f;
			prevFairing.r = baseRadius;
			currentFairing = PlaceNewXSection();
			RefreshPanels();
			panelMass = GetFairingArea() * UnitAreaMass;
			GameEvents.onEditorShipModified.Fire(EditorLogic.fetch.ship);
		};
		fsm.AddEvent(on_PartAttached, st_editor_idle);
		on_XSectionPlace = new KFSMEvent("on_XSectionPlace");
		on_XSectionPlace.GoToStateOnEvent = st_editor_place;
		on_XSectionPlace.updateMode = KFSMUpdateMode.LATEUPDATE;
		on_XSectionPlace.OnCheckCondition = (KFSMState st) => fsm.FramesInCurrentState > 1 && Mouse.Left.GetButtonUp() && currentFairing.isValid && !willCapOnPlace(currentFairing) && !isTube;
		on_XSectionPlace.OnEvent = delegate
		{
			currentFairing.color = Color.white;
			prevFairing = currentFairing;
			currentFairing = PlaceNewXSection();
			RefreshPanels();
			panelMass = GetFairingArea() * UnitAreaMass;
			isCapped = false;
			GameEvents.onEditorShipModified.Fire(EditorLogic.fetch.ship);
		};
		fsm.AddEvent(on_XSectionPlace, st_editor_place);
		on_PlaceCap = new KFSMEvent("on_PlaceCap");
		on_PlaceCap.GoToStateOnEvent = st_editor_idle;
		on_PlaceCap.updateMode = KFSMUpdateMode.LATEUPDATE;
		on_PlaceCap.OnCheckCondition = (KFSMState st) => fsm.FramesInCurrentState > 1 && Mouse.Left.GetButtonUp() && currentFairing.isValid && willCapOnPlace(currentFairing) && !isTube;
		on_PlaceCap.OnEvent = delegate
		{
			currentFairing.color = Color.white;
			currentFairing.isLast = true;
			currentFairing = null;
			prevFairing = null;
			WipeMesh();
			SpawnMeshes(addColliders: true);
			if (base.part.symmetryCounterparts.Count > 0)
			{
				UpdateSymCPartPanels(xSections);
			}
			isCapped = true;
			panelMass = GetFairingArea() * UnitAreaMass;
			GameEvents.onEditorShipModified.Fire(EditorLogic.fetch.ship);
		};
		fsm.AddEvent(on_PlaceCap, st_editor_place);
		on_EndEdit = new KFSMEvent("onEndEdit");
		on_EndEdit.GoToStateOnEvent = st_editor_idle;
		on_EndEdit.updateMode = KFSMUpdateMode.LATEUPDATE;
		on_EndEdit.OnCheckCondition = (KFSMState st) => fsm.FramesInCurrentState > 1 && Mouse.Left.GetButtonUp() && currentFairing.isValid && isTube;
		on_EndEdit.OnEvent = delegate
		{
			currentFairing.color = Color.white;
			prevFairing = currentFairing;
			currentFairing = null;
			WipeMesh();
			SpawnMeshes(addColliders: true);
			isCapped = false;
			panelMass = GetFairingArea() * UnitAreaMass;
			GameEvents.onEditorShipModified.Fire(EditorLogic.fetch.ship);
		};
		fsm.AddEvent(on_EndEdit, st_editor_place);
		on_Unplace = new KFSMEvent("on_Unplace");
		on_Unplace.GoToStateOnEvent = st_editor_place;
		on_Unplace.updateMode = KFSMUpdateMode.UPDATE;
		on_Unplace.OnCheckCondition = (KFSMState st) => fsm.FramesInCurrentState > 1 && Mouse.Right.GetClick() && !Mouse.Right.WasDragging() && xSections.Count > 2;
		on_Unplace.OnEvent = delegate
		{
			xSections.RemoveAt(xSections.Count - 1);
			currentFairing = xSections[xSections.Count - 1];
			prevFairing = xSections[xSections.Count - 2];
			RefreshPanels();
			panelMass = GetFairingArea() * UnitAreaMass;
			isCapped = false;
			GameEvents.onEditorShipModified.Fire(EditorLogic.fetch.ship);
		};
		fsm.AddEvent(on_Unplace, st_editor_place);
		on_PlaceCancel = new KFSMEvent("on_PlaceCancel");
		on_PlaceCancel.GoToStateOnEvent = st_editor_idle;
		on_PlaceCancel.updateMode = KFSMUpdateMode.UPDATE;
		on_PlaceCancel.OnCheckCondition = (KFSMState st) => fsm.FramesInCurrentState > 1 && Mouse.Right.GetClick() && !Mouse.Right.WasDragging() && xSections.Count <= 2;
		on_PlaceCancel.OnEvent = delegate
		{
			WipeMesh();
			xSections.Clear();
			currentFairing = null;
			prevFairing = null;
			applyVariantRequired = true;
			AssumeDeployedDragCube();
			if (base.part.symmetryCounterparts.Count > 0)
			{
				UpdateSymCPartPanels(null);
			}
			panelMass = GetFairingArea() * UnitAreaMass;
			isCapped = false;
			GameEvents.onEditorShipModified.Fire(EditorLogic.fetch.ship);
		};
		fsm.AddEvent(on_PlaceCancel, st_editor_place);
		on_Edit = new KFSMEvent("on_Edit");
		on_Edit.GoToStateOnEvent = st_editor_place;
		on_Edit.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_Edit.OnEvent = delegate
		{
			currentFairing = xSections[xSections.Count - 1];
			currentFairing.isLast = false;
			prevFairing = xSections[xSections.Count - 2];
			RefreshPanels();
			if (base.part.symmetryCounterparts.Count > 0)
			{
				UpdateSymCPartPanels(null);
			}
			panelMass = GetFairingArea() * UnitAreaMass;
			GameEvents.onEditorShipModified.Fire(EditorLogic.fetch.ship);
		};
		fsm.AddEvent(on_Edit, st_editor_idle);
		on_Delete = new KFSMEvent("on_Delete");
		on_Delete.GoToStateOnEvent = st_editor_idle;
		on_Delete.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_Delete.OnEvent = on_PlaceCancel.OnEvent;
		fsm.AddEvent(on_Delete, st_editor_idle);
		on_SymCPartUpdate = new KFSMEvent("on_SymCPartUpdate");
		on_SymCPartUpdate.GoToStateOnEvent = st_editor_idle;
		on_SymCPartUpdate.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_SymCPartUpdate.OnEvent = delegate
		{
		};
		fsm.AddEvent(on_SymCPartUpdate, st_editor_idle, st_editor_place);
		on_ReconnectInterstage = new KFSMEvent("on_ReconnectInterstage");
		on_ReconnectInterstage.GoToStateOnEvent = st_editor_idle;
		on_ReconnectInterstage.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_ReconnectInterstage.OnEvent = delegate
		{
			if (xSections.Count >= 2)
			{
				scheduleReconnect();
			}
		};
		fsm.AddEvent(on_ReconnectInterstage, st_editor_idle);
		on_OpenCap = new KFSMEvent("onOpenCap");
		on_OpenCap.GoToStateOnEvent = st_editor_idle;
		on_OpenCap.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_OpenCap.OnEvent = delegate
		{
			panelMass = GetFairingArea() * UnitAreaMass;
			isCapped = false;
			GameEvents.onEditorShipModified.Fire(EditorLogic.fetch.ship);
		};
		fsm.AddEvent(on_OpenCap, st_editor_idle);
	}

	public void SetupFlightFSM()
	{
		st_flight_idle = new KFSMState("st_idle");
		st_flight_idle.OnEnter = delegate
		{
			evtDeployFairing.active = xSections.Count > 0;
			panelMass = GetFairingArea() * UnitAreaMass;
			if (xSections.Count == 0)
			{
				base.part.stackIcon.SetIconColor(XKCDColors.SlateGrey);
			}
		};
		st_flight_idle.OnUpdate = delegate
		{
			if (payloadJoint != null && payloadJoint.Child != null && payloadJoint.Parent != null && payloadJoint.Child.vessel != payloadJoint.Parent.vessel)
			{
				ReleasePayload();
			}
		};
		st_flight_idle.OnLeave = delegate
		{
			GameEvents.onVesselWasModified.Remove(onVesselModified);
			evtDeployFairing.active = false;
		};
		fsm.AddState(st_flight_idle);
		st_flight_deployed = new KFSMState("st_flight_deployed");
		st_flight_deployed.OnEnter = delegate
		{
			base.part.stackIcon.SetIconColor(XKCDColors.SlateGrey);
			OnStop.Fire(1f);
			AssumeDeployedDragCube();
		};
		st_flight_deployed.OnLeave = delegate
		{
		};
		fsm.AddState(st_flight_deployed);
		on_Deploy = new KFSMEvent("on_Deploy");
		on_Deploy.GoToStateOnEvent = st_flight_deployed;
		on_Deploy.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_Deploy.OnEvent = delegate
		{
			ReleasePayload();
			OnMoving.Fire(0f, 1f);
			JettisonPanels(Panels, ejectionForce);
			GameEvents.onFairingsDeployed.Fire(base.part);
		};
		fsm.AddEvent(on_Deploy, st_flight_idle);
		on_Breakoff = new KFSMEvent("on_Breakoff");
		on_Breakoff.GoToStateOnEvent = st_flight_deployed;
		on_Breakoff.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_Breakoff.OnEvent = delegate
		{
			ReleasePayload();
			SwitchToConvexColliders();
			OnMoving.Fire(0f, 1f);
			JettisonPanels(Panels, 0f);
		};
		fsm.AddEvent(on_Breakoff, st_flight_idle);
		on_Breakoff_NoJettison = new KFSMEvent("on_Breakoff_NoJettison");
		on_Breakoff_NoJettison.GoToStateOnEvent = st_flight_idle;
		on_Breakoff_NoJettison.updateMode = KFSMUpdateMode.MANUAL_TRIGGER;
		on_Breakoff_NoJettison.OnEvent = delegate
		{
			ReleasePayload();
			SwitchToConvexColliders();
			OnMoving.Fire(0f, 1f);
			panelMass = GetFairingArea() * UnitAreaMass;
			GameEvents.onVesselWasModified.Fire(base.vessel);
		};
		fsm.AddEvent(on_Breakoff_NoJettison, st_flight_idle);
	}

	public FairingXSection PlaceNewXSection()
	{
		FairingXSection fairingXSection = new FairingXSection();
		xSections.Add(fairingXSection);
		return fairingXSection;
	}

	public void MeshPreviewUpdate(FairingXSection current, FairingXSection previous)
	{
		int num = Mathf.RoundToInt(nArcs);
		int i = 0;
		for (int count = Panels.Count; i < count; i++)
		{
			FairingPanel fairingPanel = Panels[i];
			if (fairingPanel.ContainsSection(current) || fairingPanel.ContainsSection(previous))
			{
				fairingPanel.BuildMesh(triangulate: false, (i >= num) ? Panels[i - num] : null);
			}
		}
	}

	public bool willCapOnPlace(FairingXSection current)
	{
		return current.r <= closeRadius;
	}

	public void UpdateXSectionPlacement(FairingXSection currentXsc, FairingXSection previous, Vector3 wAxis, Vector3 wPivot)
	{
		Vector3 mouseXplane = GetMouseXPlane(wPivot) - wPivot;
		h = GetMouseHeight(mouseXplane, wAxis);
		r = GetMouseLatOffset(mouseXplane, wAxis);
		h *= 1f / base.part.rescaleFactor;
		r *= 1f / base.part.rescaleFactor;
		h = ClampH(h, currentXsc);
		r = ClampR(Mathf.Abs(r));
		currentXsc.r = SnapR(r);
		currentXsc.h = SnapH(h, r);
	}

	public bool UpdateXSectionConeCast(FairingXSection currentXsc, FairingXSection previous, Vector3 wAxis, Vector3 wPivot, Vector3 wFwd, out RaycastHit hit, bool resetOnMouseMove)
	{
		float num = Mathf.Atan(1f / coneSweepPrecision / currentXsc.r) * 57.29578f;
		coneSweepOffset = (coneSweepOffset + num) % (360f / (float)coneSweepRays);
		float num2 = currentXsc.h - previous.h;
		if (FairingXSection.ConeCast(currentXsc, previous, wAxis, wPivot, wFwd, -0.02f, coneSweepRays, LayerUtil.DefaultEquivalent, out var hitLengthScalar, coneSweepOffset))
		{
			coneSweepHit = true;
			coneSweepHitLast = true;
			coneSweepClear = false;
			coneSweepOffset = 0f;
		}
		else if ((int)coneSweepOffset == 0)
		{
			if (!coneSweepHitLast)
			{
				coneSweepHit = false;
			}
			coneSweepHitLast = false;
			coneSweepClear = !coneSweepHit;
		}
		if (resetOnMouseMove && lastR != currentXsc.r)
		{
			coneSweepClear = false;
			coneSweepHit = false;
			coneSweepHitLast = false;
		}
		lastR = currentXsc.r;
		if (coneSweepClear)
		{
			currentXsc.isValid = true;
			currentXsc.color = XKCDColors.ElectricLime;
		}
		else
		{
			currentXsc.isValid = false;
			currentXsc.color = (coneSweepHit ? XKCDColors.KSPNotSoGoodOrange : XKCDColors.KSPNeutralUIGrey);
		}
		Debug.Log("[ModuleProceduralFairings] Current radius: " + currentXsc.r.ToString("F2") + " Current height: " + currentXsc.h.ToString("F2") + " ratio: " + (currentXsc.h / currentXsc.r).ToString("F2"));
		srfHeight = FairingXSection.CircleCast(currentXsc, wAxis, wPivot, wFwd, nSides, currentXsc.r + 0.1f, LayerUtil.DefaultEquivalent, out srfLevel, out hit);
		if (hitLengthScalar > (num2 - 0.1f) / num2 && srfLevel < 0.1f && previous.r >= srfHeight - 0.1f && ((Mathf.Abs(srfHeight - currentXsc.r) < snapThreshold && GameSettings.VAB_USE_ANGLE_SNAP) || (Mathf.Abs(srfHeight - currentXsc.r) < snapThresholdFineAdjust && !GameSettings.VAB_USE_ANGLE_SNAP)) && currentXsc.h / currentXsc.r >= minHeightRadiusRatio)
		{
			closeRadius = srfHeight;
			currentXsc.r = srfHeight;
			currentXsc.color = XKCDColors.KSPUnnamedCyan;
			currentXsc.isValid = true;
			return true;
		}
		return false;
	}

	public void scheduleReconnect()
	{
		StartCoroutine(scheduledReconnect(0.2f));
	}

	public IEnumerator scheduledReconnect(float delay)
	{
		if (reconnectSchedule > Time.realtimeSinceStartup)
		{
			reconnectSchedule = Time.realtimeSinceStartup + delay;
			yield break;
		}
		reconnectSchedule = Time.realtimeSinceStartup + delay;
		while (reconnectSchedule < Time.realtimeSinceStartup)
		{
			yield return null;
		}
		WipeMesh();
		coneSweepClear = false;
		coneSweepHit = false;
		coneSweepOffset = 0f;
		Vector3 wAxis = base.transform.rotation * axis;
		Vector3 wPivot = base.transform.TransformPoint(pivot);
		Vector3 wFwd = base.transform.rotation * Vector3.forward;
		Part newIStg = null;
		InputLockManager.SetControlLock(ControlTypes.EDITOR_SOFT_LOCK_ALLOW_SNAP, "FairingReconnect_" + base.part.craftID);
		int sweepsPerFrame = 1;
		while (!coneSweepClear && !coneSweepHit)
		{
			for (int i = 0; i < sweepsPerFrame; i++)
			{
				closeRadius = capRadius;
				if (UpdateXSectionConeCast(xSections[xSections.Count - 1], xSections[xSections.Count - 2], wAxis, wPivot, wFwd, out hit, resetOnMouseMove: false))
				{
					newIStg = FlightGlobals.GetPartUpwardsCached(hit.collider.gameObject);
					break;
				}
			}
			yield return null;
		}
		if (newIStg != null)
		{
			SetInterstage(newIStg);
			SpawnMeshes(addColliders: true);
		}
		else
		{
			DumpInterstage();
			fsm.RunEvent(on_Delete);
		}
		reconnectSchedule = 0f;
		panelMass = GetFairingArea() * UnitAreaMass;
		InputLockManager.RemoveControlLock("FairingReconnect_" + base.part.craftID);
	}

	public float GetMouseHeight(Vector3 mouseXplane, Vector3 planeNorm)
	{
		return Vector3.Dot(mouseXplane, planeNorm);
	}

	public float GetMouseLatOffset(Vector3 mouseXplane, Vector3 planeNorm)
	{
		return Vector3.Dot(mouseXplane, Vector3.Cross(-Camera.main.ScreenPointToRay(Input.mousePosition).direction, planeNorm).normalized);
	}

	public Vector3 GetMouseXPlane(Vector3 planePt)
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Plane plane = new Plane(-Camera.main.transform.forward, planePt);
		Vector3 result = Vector3.zero;
		if (plane.Raycast(ray, out var enter))
		{
			result = ray.origin + ray.direction * enter;
		}
		return result;
	}

	public float ClampH(float h, FairingXSection current)
	{
		h = (GameSettings.VAB_USE_ANGLE_SNAP ? Mathf.Max(h, xSectionHeightMin) : Mathf.Max(h, xSectionHeightMinFineAdjust));
		bool flag = false;
		FairingXSection fairingXSection = null;
		int count = xSections.Count;
		while (count-- > 0)
		{
			FairingXSection fairingXSection2 = xSections[count];
			if (fairingXSection2 == currentFairing)
			{
				flag = true;
				if (count != 0)
				{
					fairingXSection = xSections[count - 1];
				}
			}
			else
			{
				h = ((!flag) ? (GameSettings.VAB_USE_ANGLE_SNAP ? Mathf.Min(h, fairingXSection2.h - xSectionHeightMin) : Mathf.Min(h, fairingXSection2.h - xSectionHeightMinFineAdjust)) : (GameSettings.VAB_USE_ANGLE_SNAP ? Mathf.Max(h, fairingXSection2.h + xSectionHeightMin) : Mathf.Max(h, fairingXSection2.h + xSectionHeightMinFineAdjust)));
			}
		}
		h = ((fairingXSection == null) ? (GameSettings.VAB_USE_ANGLE_SNAP ? Mathf.Min(h, xSectionHeightMax) : Mathf.Min(h, xSectionHeightMaxFineAdjust)) : (GameSettings.VAB_USE_ANGLE_SNAP ? Mathf.Min(h, fairingXSection.h + xSectionHeightMax) : Mathf.Min(h, fairingXSection.h + xSectionHeightMaxFineAdjust)));
		return h;
	}

	public float ClampR(float r)
	{
		return Mathf.Clamp(r, closeRadius, maxRadius);
	}

	public float SnapR(float r)
	{
		float num = baseRadius;
		int num2 = xSections.IndexOf(currentFairing);
		if (num2 > 0)
		{
			num = xSections[num2 - 1].r;
		}
		if ((Mathf.Abs(r - num) < snapThreshold && GameSettings.VAB_USE_ANGLE_SNAP) || (Mathf.Abs(r - num) < snapThresholdFineAdjust && !GameSettings.VAB_USE_ANGLE_SNAP))
		{
			r = num;
		}
		if ((Mathf.Abs(r - closeRadius) < snapThreshold && GameSettings.VAB_USE_ANGLE_SNAP) || (Mathf.Abs(r - closeRadius) < snapThresholdFineAdjust && !GameSettings.VAB_USE_ANGLE_SNAP))
		{
			r = closeRadius;
		}
		return r;
	}

	public float SnapH(float h, float r)
	{
		if (h / r <= minHeightRadiusRatio)
		{
			h = r * minHeightRadiusRatio;
		}
		return h;
	}

	public void SpawnMeshes(bool addColliders)
	{
		nCollidersPerArc = Mathf.Max(1, Mathf.RoundToInt((float)nCollidersPerXSection / nArcs));
		Panels = FairingPanel.SetupPanelArray(Mathf.RoundToInt(nArcs), HighLogic.LoadedSceneIsFlight ? FairingFlightMaterial : FairingMaterial, HighLogic.LoadedSceneIsFlight ? FairingFlightConeMaterial : FairingConeMaterial, xSections, this, capRadius);
		int num = Mathf.RoundToInt(nArcs);
		int i = 0;
		for (int count = Panels.Count; i < count; i++)
		{
			FairingPanel fairingPanel = Panels[i];
			fairingPanel.Spawn(modelTransform);
			fairingPanel.BuildMesh(triangulate: true, (i >= num) ? Panels[i - num] : null);
			if (applyVariantRequired && cachedVariant != null)
			{
				ApplyVariantToPanel(fairingPanel, cachedVariant);
			}
			if (addColliders)
			{
				AddPanelColliders(fairingPanel);
			}
		}
		applyVariantRequired = false;
		if (HighLogic.LoadedSceneIsFlight)
		{
			if (interstageCraftID == 0 && isCapped)
			{
				ClosedColliders = new List<MeshCollider>();
				int j = 0;
				for (int num2 = xSections.Count - 1; j < num2; j++)
				{
					ClosedColliders.Add(ColliderSection.Create(modelTransform, xSections[j], xSections[j + 1], 12));
				}
			}
			else
			{
				SwitchToConvexColliders();
			}
		}
		if (cargoModule != null)
		{
			float num3 = xSections[xSections.Count - 1].h * 0.5f;
			cargoModule.SetLookupCenter(axis * num3);
			cargoModule.SetLookupRadius(num3);
			if (interstage != null && addColliders)
			{
				cargoModule.AddConnectingPart(interstage);
			}
		}
		if (addColliders)
		{
			AssumeClosedDragCube();
		}
		int count2 = Panels.Count;
		while (count2-- > 0)
		{
			Panels[count2].CleanUp();
		}
		base.part.ResetModelMeshRenderersCache();
		base.part.ResetModelRenderersCache();
		base.part.ResetModelSkinnedMeshRenderersCache();
	}

	public void WipeMesh()
	{
		for (int i = 0; i < base.part.children.Count; i++)
		{
			if (base.part.children[i] != null && (bool)base.part.children[i].GetComponent<FlagDecalBackground>())
			{
				if (EditorLogic.fetch != null && EditorLogic.fetch.ship != null && EditorLogic.fetch.ship.Contains(base.part.children[i]))
				{
					EditorLogic.fetch.ship.Remove(base.part.children[i]);
				}
				base.part.children[i].OnDelete();
				UnityEngine.Object.Destroy(base.part.children[i].gameObject);
				base.part.children.RemoveAt(i);
				i--;
			}
		}
		base.part.FindAttachNode(fairingNode).overrideDragArea = -1f;
		int count = Panels.Count;
		while (count-- > 0)
		{
			Panels[count].Despawn();
		}
		Panels.Clear();
		if (cargoModule != null)
		{
			cargoModule.SetLookupRadius(0f);
			cargoModule.ClearConnectingParts();
		}
		base.part.ResetModelMeshRenderersCache();
		base.part.ResetModelRenderersCache();
		base.part.ResetModelSkinnedMeshRenderersCache();
		applyVariantRequired = true;
		CheckForFairingEdit();
	}

	public void RefreshPanels()
	{
		WipeMesh();
		SpawnMeshes(addColliders: false);
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasNode("XSECTION"))
		{
			xSections.Clear();
			ConfigNode[] nodes = node.GetNodes("XSECTION");
			int num = nodes.Length;
			for (int i = 0; i < num; i++)
			{
				ConfigNode node2 = nodes[i];
				FairingXSection fairingXSection = new FairingXSection();
				fairingXSection.Load(node2);
				xSections.Add(fairingXSection);
			}
			xSections.Sort();
			if (xSections.Count >= 2)
			{
				xSections[xSections.Count - 1].isLast = true;
			}
		}
		if (node.HasValue("fsm"))
		{
			fsmStateName = node.GetValue("fsm");
		}
	}

	public override void OnSave(ConfigNode node)
	{
		if (fsm.Started)
		{
			node.AddValue("fsm", fsm.currentStateName);
		}
		int i = 0;
		for (int count = xSections.Count; i < count; i++)
		{
			xSections[i].Save(node.AddNode("XSECTION"));
		}
	}

	public Color getXSectionColor()
	{
		if (currentFairing != null)
		{
			if (willCapOnPlace(currentFairing))
			{
				return XKCDColors.ElectricLime;
			}
			return XKCDColors.LightAqua;
		}
		return XKCDColors.Cyan;
	}

	public void GizmoDisplay()
	{
		Gizmos.color = getXSectionColor();
		Vector3 vector = base.transform.rotation * axis;
		Vector3 vector2 = base.transform.TransformPoint(pivot);
		int count = xSections.Count;
		for (int i = 0; i < count; i++)
		{
			FairingXSection fairingXSection = xSections[i];
			GizmoDrawUtil.DrawArc(vector2 + vector * fairingXSection.h, vector, base.transform.forward, 0f, 360f, fairingXSection.r, nSides);
		}
	}

	[KSPEvent(active = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001397")]
	public void DeleteFairing()
	{
		fsm.RunEvent(on_Delete);
	}

	[KSPEvent(active = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001398")]
	public void EditFairing()
	{
		fsm.RunEvent(on_Edit);
	}

	[KSPEvent(active = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001399")]
	public void BuildFairing()
	{
		fsm.RunEvent(on_PartAttached);
	}

	[KSPEvent(active = true, guiActiveUnfocused = false, guiActive = true, guiName = "#autoLOC_6002396")]
	public void DeployFairing()
	{
		fsm.RunEvent(on_Deploy);
	}

	[KSPAction("#autoLOC_6002396", KSPActionGroup.None, activeEditor = false)]
	public void DeployFairingAction(KSPActionParam p)
	{
		if (p.type != KSPActionType.Deactivate)
		{
			if (fsm.CurrentState != st_flight_deployed)
			{
				fsm.RunEvent(on_Deploy);
			}
			else
			{
				JettisonPanels(Panels, ejectionForce);
			}
		}
	}

	public override void OnActive()
	{
		if (stagingEnabled)
		{
			fsm.RunEvent(on_Deploy);
		}
	}

	public float GetFairingArea()
	{
		float num = 0f;
		int count = Panels.Count;
		while (count-- > 0)
		{
			num += Panels[count].GetArea();
		}
		return num;
	}

	public void MouseFadeUpdate()
	{
		int count = Panels.Count;
		while (count-- > 0)
		{
			if (!isFadeLocked && !partFlagSrfAttach && (!InputLockManager.IsLocked(ControlTypes.EDITOR_SOFT_LOCK) || !(EditorLogic.SelectedPart != null)))
			{
				if (fairingsAreLocked)
				{
					StartCoroutine(CallbackUtil.DelayedCallback(1, delegate
					{
						fairingsAreLocked = false;
					}));
					continue;
				}
				float cursorProximity = Panels[count].GetCursorProximity(Input.mousePosition, baseRadius * 4f, Camera.main);
				cursorProximity = Mathf.Pow(Mathf.Clamp01(cursorProximity), 1.4f);
				if (cursorProximity == 0f)
				{
					Panels[count].SetTgtOpacity(1f);
					Panels[count].SetTgtExplodedView(0f);
					if (!Panels[count].ColliderContainer.activeSelf)
					{
						Panels[count].ColliderContainer.SetActive(value: true);
					}
					continue;
				}
				if (base.part.gameObject.layer == 1)
				{
					Panels[count].SetTgtOpacity(Mathf.Max(1f - cursorProximity, 0.2f) * 0.5f);
				}
				else
				{
					Panels[count].SetTgtOpacity(Mathf.Max(1f - cursorProximity, 0.2f));
				}
				if (!isFadeLocked && !partFlagSrfAttach)
				{
					cursorProximity = Panels[count].GetCursorProximity(Input.mousePosition, baseRadius * 2.2f, Camera.main);
					cursorProximity = Mathf.Pow(Mathf.Clamp01(cursorProximity), 1.6f);
				}
				else
				{
					cursorProximity = 0f;
				}
				Panels[count].SetTgtExplodedView(cursorProximity);
				if (cursorProximity > 0.5f)
				{
					if (Panels[count].ColliderContainer.activeSelf)
					{
						Panels[count].ColliderContainer.SetActive(value: false);
					}
				}
				else if (!Panels[count].ColliderContainer.activeSelf)
				{
					Panels[count].ColliderContainer.SetActive(value: true);
				}
			}
			else
			{
				fairingsAreLocked = true;
				float cursorProximity = 0f;
				Panels[count].SetTgtOpacity(1f);
				Panels[count].SetCollapsedViewInstantly();
				if (!Panels[count].ColliderContainer.activeSelf)
				{
					Panels[count].ColliderContainer.SetActive(value: true);
				}
			}
		}
	}

	public float GetModuleMass(float defaultMass, ModifierStagingSituation sit)
	{
		if (sit == ModifierStagingSituation.STAGED)
		{
			return 0f;
		}
		return Mathf.Clamp(panelMass, Mathf.Epsilon, Mathf.Abs(panelMass));
	}

	public ModifierChangeWhen GetModuleMassChangeWhen()
	{
		return ModifierChangeWhen.STAGED;
	}

	public float GetTotalMass()
	{
		return base.part.partInfo.partPrefab.mass + base.part.GetModuleMass(base.part.partInfo.partPrefab.mass);
	}

	public float GetModuleCost(float defaultCost, ModifierStagingSituation sit)
	{
		if (sit == ModifierStagingSituation.STAGED)
		{
			return 0f;
		}
		return GetFairingArea() * UnitAreaCost;
	}

	public ModifierChangeWhen GetModuleCostChangeWhen()
	{
		return ModifierChangeWhen.STAGED;
	}

	public float GetTotalCost()
	{
		return base.part.partInfo.cost + base.part.GetModuleCosts(base.part.partInfo.cost);
	}

	public Vector3 GetModuleSize(Vector3 defaultSize, ModifierStagingSituation sit)
	{
		return Vector3.zero;
	}

	public ModifierChangeWhen GetModuleSizeChangeWhen()
	{
		return ModifierChangeWhen.STAGED;
	}

	public void SetupOcclusionTest(bool testActive)
	{
		if (Panels.Count == 0)
		{
			return;
		}
		int i = 0;
		for (int count = ClosedColliders.Count; i < count; i++)
		{
			if (ClosedColliders[i] != null)
			{
				ClosedColliders[i].enabled = !testActive;
			}
		}
		int count2 = Panels.Count;
		while (count2-- > 0)
		{
			if (Panels[count2].ColliderContainer != null)
			{
				if (testActive)
				{
					Panels[count2].ColliderContainer.SetActive(testActive);
				}
				else
				{
					Panels[count2].ColliderContainer.SetActive(interstageCraftID != 0 || !isCapped);
				}
			}
		}
	}

	public void SwitchToConvexColliders()
	{
		int i = 0;
		for (int count = ClosedColliders.Count; i < count; i++)
		{
			if (ClosedColliders[i] != null)
			{
				ClosedColliders[i].enabled = false;
				UnityEngine.Object.Destroy(ClosedColliders[i].gameObject);
			}
		}
		ClosedColliders.Clear();
		int count2 = Panels.Count;
		while (count2-- > 0)
		{
			if (Panels[count2].ColliderContainer != null && !Panels[count2].ColliderContainer.activeSelf)
			{
				Panels[count2].ColliderContainer.SetActive(value: true);
			}
		}
		GameEvents.OnCollisionIgnoreUpdate.Fire();
	}

	public void JettisonPanels(List<FairingPanel> panels, float ejectionForce)
	{
		base.part.HighlightRecursive(active: false);
		base.part.RefreshHighlighter();
		if (panels.Count == 0)
		{
			return;
		}
		SwitchToConvexColliders();
		float angularDrag = base.part.rb.angularDrag;
		float drag = base.part.rb.drag;
		Vector3 angularVelocity = base.part.rb.angularVelocity;
		Vector3 velocity = base.part.rb.velocity;
		Vector3 currentCoM = base.vessel.CurrentCoM;
		Vector3 vector = Vector3.Cross(base.part.rb.worldCenterOfMass - currentCoM, base.vessel.angularVelocity);
		Vector3 planeNormal = base.part.partTransform.rotation * axis;
		bool flag = false;
		if (useClamshell)
		{
			panelDict = new Dictionary<Vector3, List<FairingPanel>>();
			int count = panels.Count;
			while (count-- > 0)
			{
				if (panels[count].ReadyFlagsForJettison())
				{
					flag = true;
				}
				FairingPanel fairingPanel = panels[count];
				Vector3 normalized = Vector3.ProjectOnPlane(base.transform.InverseTransformPoint(fairingPanel.go.transform.position) - axis, axis).normalized;
				List<FairingPanel> list = null;
				Dictionary<Vector3, List<FairingPanel>>.Enumerator enumerator = panelDict.GetEnumerator();
				while (enumerator.MoveNext())
				{
					KeyValuePair<Vector3, List<FairingPanel>> current = enumerator.Current;
					if (!(Vector3.Dot(current.Key, normalized) <= 0.99f))
					{
						list = current.Value;
						break;
					}
				}
				if (list == null)
				{
					list = new List<FairingPanel>();
					panelDict.Add(normalized, list);
				}
				list.Add(fairingPanel);
			}
			Dictionary<Vector3, List<FairingPanel>>.ValueCollection.Enumerator enumerator2 = panelDict.Values.GetEnumerator();
			while (enumerator2.MoveNext())
			{
				List<FairingPanel> current2 = enumerator2.Current;
				FairingPanel fairingPanel2 = current2[0];
				Rigidbody rb = physicalObject.ConvertToPhysicalObject(base.part, fairingPanel2.go).rb;
				rb.useGravity = false;
				rb.drag = drag;
				rb.angularDrag = angularDrag;
				rb.angularVelocity = angularVelocity;
				rb.velocity = velocity + vector;
				rb.maxAngularVelocity = PhysicsGlobals.MaxAngularVelocity;
				rb.mass = fairingPanel2.GetArea() * UnitAreaMass;
				Vector3d vector3d = (double)rb.mass * (Vector3d)rb.transform.position;
				int i = 1;
				for (int count2 = current2.Count; i < count2; i++)
				{
					float num = current2[i].GetArea() * UnitAreaMass;
					rb.mass += num;
					vector3d += (double)num * (Vector3d)current2[i].go.transform.position;
					current2[i].go.transform.parent = fairingPanel2.go.transform;
				}
				vector3d /= (double)rb.mass;
				Vector3 vector2 = vector3d;
				rb.centerOfMass = rb.transform.InverseTransformPoint(vector2);
				Vector3 force = Vector3.ProjectOnPlane(vector2 - base.part.partTransform.position, planeNormal).normalized * ejectionForce * rb.mass;
				rb.AddForceAtPosition(force, vector2, ForceMode.Force);
			}
		}
		else
		{
			int count3 = panels.Count;
			while (count3-- > 0)
			{
				if (panels[count3].ReadyFlagsForJettison())
				{
					flag = true;
				}
				Vector3 force = Vector3.ProjectOnPlane(panels[count3].go.transform.position - base.part.partTransform.position, planeNormal).normalized * ejectionForce;
				MakePanelPhysical(panels[count3], velocity + vector, angularVelocity, force, UnitAreaMass, drag, angularDrag);
			}
		}
		base.part.ResetModelMeshRenderersCache();
		base.part.ResetModelRenderersCache();
		base.part.ResetModelSkinnedMeshRenderersCache();
		base.part.ResetCollisions();
		if (flag)
		{
			GameEvents.onVesselPartCountChanged.Fire(base.vessel);
		}
		Panels.Clear();
		xSections.Clear();
		panelMass = GetFairingArea() * UnitAreaMass;
		GameEvents.onVesselWasModified.Fire(base.vessel);
	}

	public physicalObject MakePanelPhysical(FairingPanel panel, Vector3 velocity, Vector3 angularVelocity, Vector3 push, float unitMass, float drag, float angDrag)
	{
		physicalObject obj = physicalObject.ConvertToPhysicalObject(base.part, panel.go);
		Rigidbody rb = obj.rb;
		rb.useGravity = false;
		rb.mass = panel.GetArea() * unitMass;
		rb.drag = drag;
		rb.angularDrag = angDrag;
		rb.angularVelocity = angularVelocity;
		rb.velocity = velocity;
		rb.maxAngularVelocity = PhysicsGlobals.MaxAngularVelocity;
		rb.AddForceAtPosition(push, panel.go.transform.position, ForceMode.Force);
		return obj;
	}

	public void AddPanelColliders(FairingPanel panel)
	{
		panel.ColliderContainer = new GameObject("Colliders");
		panel.ColliderContainer.transform.NestToParent(panel.go.transform);
		if (HighLogic.LoadedSceneIsFlight)
		{
			panel.ColliderContainer.layer = LayerMask.NameToLayer("PhysicalObjects");
		}
		panel.ColliderContainer.SetActive(value: false);
		panel.GeneratePanelColliders(panel.ColliderContainer, panel.ColliderContainer.layer, HighLogic.LoadedSceneIsEditor);
	}

	public void UpdateSymCPartPanels(List<FairingXSection> newXSections)
	{
		int count = base.part.symmetryCounterparts.Count;
		while (count-- > 0)
		{
			base.part.symmetryCounterparts[count].FindModuleImplementing<ModuleProceduralFairing>().UpdatePanelsFromCPart(newXSections);
		}
	}

	public void UpdatePanelsFromCPart(List<FairingXSection> newXSections)
	{
		WipeMesh();
		xSections.Clear();
		if (newXSections != null)
		{
			int i = 0;
			for (int count = newXSections.Count; i < count; i++)
			{
				xSections.Add(new FairingXSection(newXSections[i]));
			}
			SpawnMeshes(addColliders: true);
		}
		panelMass = GetFairingArea() * UnitAreaMass;
		fsm.RunEvent(on_SymCPartUpdate);
	}

	public void PayloadStrutsSetup()
	{
		float fHeight = xSections[xSections.Count - 1].h;
		Vector3 wAxis = base.part.partTransform.rotation * axis;
		Vector3 position = base.part.partTransform.position;
		SetupDynamicCargoOccluders(testActive: true);
		if (interstage != null)
		{
			SecurePayload(interstage, xSections[xSections.Count - 1].r, position, wAxis, fHeight);
		}
		else
		{
			Part part = FindPayloadFromTop(position, wAxis, fHeight);
			if (part != null && part != base.part)
			{
				SecurePayload(part, baseRadius, position, wAxis, fHeight);
			}
		}
		SetupDynamicCargoOccluders(testActive: false);
	}

	public void SetupDynamicCargoOccluders(bool testActive)
	{
		List<Part> list = null;
		list = ((!HighLogic.LoadedSceneIsFlight) ? EditorLogic.fetch.ship.parts : base.vessel.parts);
		if (list == null)
		{
			return;
		}
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			Part part = list[i];
			int j = 0;
			for (int count2 = part.Modules.Count; j < count2; j++)
			{
				if (part.Modules[j] is IDynamicCargoOccluder dynamicCargoOccluder)
				{
					dynamicCargoOccluder.SetupOcclusionTest(testActive);
				}
			}
		}
	}

	public Part FindPayloadFromTop(Vector3 wBase, Vector3 wAxis, float fHeight)
	{
		if (xSections.Count == 0)
		{
			return null;
		}
		if (Physics.Raycast(wBase + wAxis * fHeight, -wAxis, out var hitInfo, fHeight, LayerUtil.DefaultEquivalent))
		{
			Part partUpwardsCached = FlightGlobals.GetPartUpwardsCached(hitInfo.collider.gameObject);
			if (partUpwardsCached != null && partUpwardsCached != base.part)
			{
				return partUpwardsCached.Rigidbody.GetComponent<Part>();
			}
		}
		return null;
	}

	public void SecurePayload(Part payloadPart, float attachRadius, Vector3 wBase, Vector3 wAxis, float fHeight)
	{
		payloadAttachNode = new AttachNode();
		payloadAttachNode.id = "Payload";
		payloadAttachNode.attachedPart = payloadPart;
		payloadAttachNode.attachMethod = AttachNodeMethod.FIXED_JOINT;
		payloadAttachNode.breakingForce = float.MaxValue;
		payloadAttachNode.breakingTorque = float.MaxValue;
		Vector3 position = wBase + wAxis * fHeight;
		payloadAttachNode.position = payloadPart.partTransform.InverseTransformPoint(position);
		payloadAttachNode.orientation = payloadPart.partTransform.InverseTransformDirection(wAxis);
		payloadAttachNode.ResourceXFeed = false;
		payloadAttachNode.owner = base.part;
		payloadAttachNode.size = (int)attachRadius;
		payloadJoint = PartJoint.Create(base.part, payloadPart, payloadAttachNode, null, AttachModes.SRF_ATTACH);
	}

	public void ReleasePayload()
	{
		base.part.FindAttachNode(fairingNode).overrideDragArea = -1f;
		if (payloadJoint != null)
		{
			payloadJoint.DestroyJoint();
		}
		if (payloadAttachNode != null && payloadAttachNode.attachedPart != null && payloadAttachNode.attachedPart.attachJoint != null && payloadAttachNode.attachedPart.attachJoint.Parent != null && payloadAttachNode.attachedPart.attachJoint.Parent.persistentId == base.part.persistentId && interstageCraftID != 0)
		{
			payloadAttachNode.attachedPart.attachJoint.DestroyJoint();
		}
		payloadAttachNode = null;
		DumpInterstage();
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

	public void DecoupleAction(string nodeName, bool weDecouple)
	{
		if (payloadAttachNode != null && payloadJoint != null)
		{
			AttachNode attachNode = base.part.FindAttachNode(nodeName);
			if (attachNode != null && attachNode.attachedPart != null && attachNode.attachedPart.persistentId == payloadAttachNode.attachedPart.persistentId)
			{
				fsm.RunEvent(on_Breakoff);
			}
		}
	}

	public void AssumeClosedDragCube()
	{
		if (interstage != null || interstageCraftID != 0)
		{
			float num = xSections[xSections.Count - 1].r * interstageOcclusionFudge;
			num *= num;
			base.part.FindAttachNode(fairingNode).overrideDragArea = num * (float)Math.PI;
		}
		DragCube dragCube = DragCubeSystem.Instance.RenderProceduralDragCube(base.part);
		SetPartDragCube(dragCube);
		dragCoPOffset = dragCube.Center.y + dragCube.Size.y * 0.5f - dragCube.Depth[2];
		dragCoPOffset = (dragCube.Center.y + dragCoPOffset) * 0.5f;
		base.part.CoPOffset = originalCoP;
		base.part.CoPOffset.y += dragCoPOffset;
		base.part.CoMOffset = originalCoM;
		base.part.CoMOffset.y += dragCoPOffset * 0.5f;
	}

	public void AssumeDeployedDragCube()
	{
		base.part.FindAttachNode(fairingNode).overrideDragArea = -1f;
		DragCube partDragCube = base.part.partInfo.partPrefab.DragCubes.Cubes[0];
		SetPartDragCube(partDragCube);
		base.part.CoMOffset = originalCoM;
		base.part.CoPOffset = originalCoP;
		base.part.CoLOffset = originalCoL;
	}

	public void SetPartDragCube(DragCube cube)
	{
		bool procedural = base.part.DragCubes.Procedural;
		base.part.DragCubes.ClearCubes();
		base.part.DragCubes.Cubes.Add(cube);
		base.part.DragCubes.Procedural = procedural;
		base.part.DragCubes.ResetCubeWeights();
		base.part.DragCubes.ForceUpdate(weights: true, occlusion: true);
	}

	public void OnChildAdd(Part child)
	{
		if (!HighLogic.LoadedSceneIsEditor || !(EditorLogic.fetch.FairingHitCollider != null) || !(EditorLogic.fetch.FairingHitCollider.transform.parent != null))
		{
			return;
		}
		StartCoroutine(CallbackUtil.DelayedCallback(1, delegate
		{
			if (EditorLogic.fetch.FairingHitCollider != null)
			{
				AddFlagToFairing(EditorLogic.fetch.FairingHitCollider.transform.parent, child);
			}
			else
			{
				if (child.isClone)
				{
					child.transform.SetLayerRecursive(2);
					rayForFlag = default(Ray);
					rayForFlag.origin = child.transform.position;
					rayForFlag.direction = (base.part.transform.position - child.transform.position).normalized;
					rayHitForFlag = default(RaycastHit);
					Debug.DrawRay(rayForFlag.origin, rayForFlag.direction, Color.red, 5f);
					Debug.DrawRay(rayForFlag.origin, child.attRotation0.eulerAngles * -1f, Color.yellow, 5f);
					if (Physics.Raycast(rayForFlag, out rayHitForFlag, 2f, LayerUtil.DefaultEquivalent))
					{
						if (rayHitForFlag.collider.transform != null && rayHitForFlag.collider.transform.parent != null)
						{
							AddFlagToFairing(rayHitForFlag.collider.transform.parent, child);
						}
						else
						{
							Debug.LogError("Could not attach Part " + base.part.name);
						}
					}
					child.transform.SetLayerRecursive(0);
				}
				CheckForFairingEdit();
			}
		}));
	}

	public void OnChildRemove(Part child)
	{
		if (HighLogic.LoadedSceneIsEditor)
		{
			uint flagID = 0u;
			FlagDecalBackground component = child.GetComponent<FlagDecalBackground>();
			if (component != null)
			{
				flagID = ((component.placementID == 0) ? child.craftID : ((uint)component.placementID));
			}
			for (int i = 0; i < xSections.Count; i++)
			{
				xSections[i].RemoveAttachedFlag(flagID);
			}
			for (int j = 0; j < Panels.Count; j++)
			{
				Panels[j].RemoveFlag(flagID);
			}
		}
		CheckForFairingEdit();
	}

	public void AddFlagToFairing(Transform panel_T, Part flagPart)
	{
		for (int i = 0; i < Panels.Count; i++)
		{
			if (panel_T.gameObject.GetInstanceID() == Panels[i].ColliderContainer.GetInstanceID())
			{
				Panels[i].AddFlag(flagPart);
			}
		}
		CheckForFairingEdit();
		EditorLogic.fetch.FairingHitCollider = null;
	}

	public void CheckForFairingEdit()
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			base.Fields["editingBlocked"].guiActive = false;
			return;
		}
		int num = 0;
		while (true)
		{
			if (num < Panels.Count)
			{
				if (Panels[num].attachedFlagParts != null && Panels[num].attachedFlagParts.Count > 0)
				{
					break;
				}
				num++;
				continue;
			}
			base.Events["EditFairing"].guiActiveEditor = true;
			base.Fields["nArcs"].guiActiveEditor = true;
			base.Fields["editingBlocked"].guiActiveEditor = false;
			return;
		}
		base.Events["EditFairing"].guiActiveEditor = false;
		base.Fields["nArcs"].guiActiveEditor = false;
		base.Fields["editingBlocked"].guiActiveEditor = true;
	}

	public string GetModuleTitle()
	{
		return cacheAutoLOC_225016;
	}

	public Callback<Rect> GetDrawModulePanelCallback()
	{
		return null;
	}

	public string GetPrimaryField()
	{
		return Localizer.Format("#autoLOC_6001012", maxRadius.ToString("N2"));
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_6001012", maxRadius.ToString("N2")) + Localizer.Format("#autoLOC_6001013", UnitAreaMass.ToString("N3")) + Localizer.Format("#autoLOC_6001014", UnitAreaCost.ToString("N2"));
	}

	public override string GetModuleDisplayName()
	{
		return cacheAutoLOC_225016;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_6001005 = Localizer.Format("#autoLOC_6001005");
		cacheAutoLOC_6001006 = Localizer.Format("#autoLOC_6001006");
		cacheAutoLOC_6001007 = Localizer.Format("#autoLOC_6001007");
		cacheAutoLOC_6001008 = Localizer.Format("#autoLOC_6001008");
		cacheAutoLOC_225016 = Localizer.Format("#autoLOC_225016");
		cacheAutoLOC_6005081 = Localizer.Format("#autoLOC_6005081");
	}
}

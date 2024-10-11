using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using ProceduralFairings;
using UnityEngine;

public class ModuleProceduralFairing : PartModule, IPartMassModifier, IPartCostModifier, IScalarModule, IModuleInfo, IDynamicCargoOccluder, IActivateOnDecouple
{
	[CompilerGenerated]
	private sealed class _003CscheduledReconnect_003Ed__133 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ModuleProceduralFairing _003C_003E4__this;

		public float delay;

		private Vector3 _003CwAxis_003E5__2;

		private Vector3 _003CwPivot_003E5__3;

		private Vector3 _003CwFwd_003E5__4;

		private Part _003CnewIStg_003E5__5;

		private int _003CsweepsPerFrame_003E5__6;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CscheduledReconnect_003Ed__133(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	[KSPField(isPersistant = true)]
	public uint interstageCraftID;

	[KSPField]
	public string fairingNode;

	[KSPField]
	public int nSides;

	[UI_FloatRange(scene = UI_Scene.Editor, stepIncrement = 1f, maxValue = 6f, minValue = 2f)]
	[KSPField(isPersistant = true, guiActiveEditor = true, guiName = "#autoLOC_6001394")]
	public float nArcs;

	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6006090")]
	[UI_Label(scene = UI_Scene.Editor)]
	public string editingBlocked;

	[KSPField]
	public int nCollidersPerXSection;

	[KSPField]
	public int panelGrouping;

	[KSPField]
	public Vector3 pivot;

	[KSPField]
	public Vector3 axis;

	[KSPField]
	public float baseRadius;

	[KSPField]
	public float maxRadius;

	[KSPField]
	public float capRadius;

	[KSPField]
	public float snapThreshold;

	[KSPField]
	public float minHeightRadiusRatio;

	[KSPField]
	public float snapThresholdFineAdjust;

	[KSPField]
	public float xSectionHeightMin;

	[KSPField]
	public float xSectionHeightMax;

	[KSPField]
	public float xSectionHeightMinFineAdjust;

	[KSPField]
	public float xSectionHeightMaxFineAdjust;

	[KSPField]
	public float aberrantNormalLimit;

	[KSPField]
	public float edgeSlide;

	[KSPField]
	public float edgeWarp;

	[KSPField]
	public float noseTip;

	[KSPField]
	public string TextureURL;

	[KSPField]
	public string CapTextureURL;

	[KSPField]
	public string TextureNormalURL;

	[KSPField]
	public string CapTextureNormalURL;

	[KSPField]
	public string BaseModelTransformName;

	[KSPField]
	public string DefaultBaseTextureURL;

	[KSPField]
	public string DefaultBaseNormalsURL;

	[KSPField]
	public float UnitAreaMass;

	[KSPField]
	public float UnitAreaCost;

	[KSPField(isPersistant = true, guiActiveEditor = true, guiName = "#autoLOC_6001395")]
	[UI_FloatRange(scene = UI_Scene.Editor, stepIncrement = 5f, maxValue = 1000f, minValue = 0f)]
	public float ejectionForce;

	[KSPField]
	public float interstageOcclusionFudge;

	[KSPField]
	public int coneSweepRays;

	[KSPField]
	public float coneSweepPrecision;

	[KSPField(isPersistant = true, guiActiveEditor = true, guiName = "#autoLOC_6001396")]
	[UI_Toggle(disabledText = "#autoLOC_6001073", enabledText = "#autoLOC_6001074")]
	public bool useClamshell;

	[UI_Toggle(disabledText = "#autoLOC_6006082", enabledText = "#autoLOC_6006083")]
	[KSPField(guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6006081")]
	public bool isFadeLocked;

	[KSPField(isPersistant = true)]
	public bool isCapped;

	public List<FairingXSection> xSections;

	public int nCollidersPerArc;

	[NonSerialized]
	private FairingXSection currentFairing;

	[NonSerialized]
	private FairingXSection prevFairing;

	private bool isTube;

	private float r;

	private float h;

	private float panelMass;

	public Material FairingMaterial;

	public Material FairingConeMaterial;

	public Material FairingFlightMaterial;

	public Material FairingFlightConeMaterial;

	public List<FairingPanel> Panels;

	public List<MeshCollider> ClosedColliders;

	private Transform modelTransform;

	private Renderer baseRenderer;

	private PartVariant cachedVariant;

	private bool applyVariantRequired;

	private float coneSweepOffset;

	private float srfLevel;

	private float srfHeight;

	private float closeRadius;

	private float reconnectSchedule;

	private float lastR;

	private bool coneSweepHit;

	private bool coneSweepHitLast;

	private bool coneSweepClear;

	private RaycastHit hit;

	private Part interstage;

	[SerializeField]
	private KerbalFSM fsm;

	private KFSMState st_editor_idle;

	private KFSMState st_editor_place;

	private KFSMEvent on_PartAttached;

	private KFSMEvent on_PartDetached;

	private KFSMEvent on_XSectionPlace;

	private KFSMEvent on_SymCPartUpdate;

	private KFSMEvent on_PlaceCap;

	private KFSMEvent on_Unplace;

	private KFSMEvent on_PlaceCancel;

	private KFSMEvent on_Edit;

	private KFSMEvent on_Delete;

	private KFSMEvent on_ReconnectInterstage;

	private KFSMEvent on_OpenCap;

	private KFSMEvent on_EndEdit;

	private KFSMState st_flight_idle;

	private KFSMState st_flight_deployed;

	private KFSMEvent on_Deploy;

	private KFSMEvent on_Breakoff;

	private KFSMEvent on_Breakoff_NoJettison;

	private string fsmStateName;

	private BaseEvent evtDeleteFairing;

	private BaseEvent evtEditFairing;

	private BaseEvent evtBuildFairing;

	private BaseEvent evtDeployFairing;

	private Vector3 prevMousePosition;

	private bool variantsCached;

	[NonSerialized]
	private ModuleCargoBay cargoModule;

	private bool partFlagSrfAttach;

	private Ray rayForFlag;

	private RaycastHit rayHitForFlag;

	private bool fairingsAreLocked;

	private Dictionary<Vector3, List<FairingPanel>> panelDict;

	private AttachNode payloadAttachNode;

	private PartJoint payloadJoint;

	private EventData<float, float> OnDeployStart;

	private EventData<float> OnDeployEnd;

	[KSPField]
	public string moduleID;

	private Vector3 originalCoP;

	private Vector3 originalCoM;

	private Vector3 originalCoL;

	private float dragCoPOffset;

	private static string cacheAutoLOC_6001005;

	private static string cacheAutoLOC_6001006;

	private static string cacheAutoLOC_6001007;

	private static string cacheAutoLOC_6001008;

	private static string cacheAutoLOC_6005081;

	private static string cacheAutoLOC_225016;

	private static string cacheAutoLOC_6005085;

	public string ScalarModuleID
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float GetScalar
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool CanMove
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public EventData<float, float> OnMoving
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public EventData<float> OnStop
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleProceduralFairing()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ModuleProceduralFairing()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFairingSidesChanged(object arg1)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onEditorPartEvent(ConstructionEventType evt, Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVariantsAdded(AvailablePart aP)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVariantApplied(Part part, PartVariant variant)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ApplyVariantToPanel(FairingPanel panel, PartVariant variant)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ParentChildFlagsParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetInterstage(Part iStg)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DumpInterstage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onInterstageReattach()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onInterstageDetach()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onInterstageDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSelfReattach()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSelfDetach()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselModified(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onPartDestroyed(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LockEditor(bool lockState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OpenFairing(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDrawGizmos()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupEditorFSM()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupFlightFSM()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private FairingXSection PlaceNewXSection()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MeshPreviewUpdate(FairingXSection current, FairingXSection previous)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool willCapOnPlace(FairingXSection current)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateXSectionPlacement(FairingXSection currentXsc, FairingXSection previous, Vector3 wAxis, Vector3 wPivot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool UpdateXSectionConeCast(FairingXSection currentXsc, FairingXSection previous, Vector3 wAxis, Vector3 wPivot, Vector3 wFwd, out RaycastHit hit, bool resetOnMouseMove)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void scheduleReconnect()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CscheduledReconnect_003Ed__133))]
	private IEnumerator scheduledReconnect(float delay)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetMouseHeight(Vector3 mouseXplane, Vector3 planeNorm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetMouseLatOffset(Vector3 mouseXplane, Vector3 planeNorm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3 GetMouseXPlane(Vector3 planePt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float ClampH(float h, FairingXSection current)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float ClampR(float r)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float SnapR(float r)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float SnapH(float h, float r)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SpawnMeshes(bool addColliders)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void WipeMesh()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshPanels()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Color getXSectionColor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GizmoDisplay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001397")]
	public void DeleteFairing()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001398")]
	public void EditFairing()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001399")]
	public void BuildFairing()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(active = true, guiActiveUnfocused = false, guiActive = true, guiName = "#autoLOC_6002396")]
	public void DeployFairing()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6002396", KSPActionGroup.None, activeEditor = false)]
	public void DeployFairingAction(KSPActionParam p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetFairingArea()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseFadeUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetModuleMass(float defaultMass, ModifierStagingSituation sit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModifierChangeWhen GetModuleMassChangeWhen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetTotalMass()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetModuleCost(float defaultCost, ModifierStagingSituation sit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModifierChangeWhen GetModuleCostChangeWhen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetTotalCost()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetModuleSize(Vector3 defaultSize, ModifierStagingSituation sit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModifierChangeWhen GetModuleSizeChangeWhen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupOcclusionTest(bool testActive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SwitchToConvexColliders()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void JettisonPanels(List<FairingPanel> panels, float ejectionForce)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private physicalObject MakePanelPhysical(FairingPanel panel, Vector3 velocity, Vector3 angularVelocity, Vector3 push, float unitMass, float drag, float angDrag)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddPanelColliders(FairingPanel panel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateSymCPartPanels(List<FairingXSection> newXSections)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdatePanelsFromCPart(List<FairingXSection> newXSections)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void PayloadStrutsSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupDynamicCargoOccluders(bool testActive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Part FindPayloadFromTop(Vector3 wBase, Vector3 wAxis, float fHeight)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SecurePayload(Part payloadPart, float attachRadius, Vector3 wBase, Vector3 wAxis, float fHeight)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ReleasePayload()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetScalar(float t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetUIRead(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetUIWrite(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsMoving()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DecoupleAction(string nodeName, bool weDecouple)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AssumeClosedDragCube()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AssumeDeployedDragCube()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetPartDragCube(DragCube cube)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnChildAdd(Part child)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnChildRemove(Part child)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddFlagToFairing(Transform panel_T, Part flagPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckForFairingEdit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetModuleTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Callback<Rect> GetDrawModulePanelCallback()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetPrimaryField()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using EditorGizmos;
using Expansions.Missions;
using KSP.UI;
using KSP.UI.Screens;
using PreFlightTests;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditorLogic : EditorLogicBase
{
	public enum EditorModes
	{
		SIMPLE,
		ADVANCED
	}

	[CompilerGenerated]
	private sealed class _003CStart_003Ed__132 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public EditorLogic _003C_003E4__this;

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
		public _003CStart_003Ed__132(int _003C_003E1__state)
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

	[CompilerGenerated]
	private sealed class _003CDelayedAbort_003Ed__315 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public EditorLogic _003C_003E4__this;

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
		public _003CDelayedAbort_003Ed__315(int _003C_003E1__state)
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

	[CompilerGenerated]
	private sealed class _003CDelayedUnlock_003Ed__326 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

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
		public _003CDelayedUnlock_003Ed__326(int _003C_003E1__state)
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

	public EditorToolsUI toolsUI;

	public Camera editorCamera;

	public EditorCamera editorCam;

	public Camera editorCargoCamera;

	private List<string> availableCargoParts;

	private static string autoshipname;

	public string startPodId;

	public Quaternion vesselRotation;

	public float dragSharpness;

	public float srfAttachAngleSnap;

	public float srfAttachAngleSnapFine;

	public bool disallowSave;

	public int sceneToLoad;

	private Part rootPart;

	public static EditorPartIcon iconRequestedVariantChange;

	public AudioClip attachClip;

	public AudioClip deletePartClip;

	public AudioClip partGrabClip;

	public AudioClip partReleaseClip;

	public AudioClip cannotPlaceClip;

	public AudioClip tweakGrabClip;

	public AudioClip tweakReleaseClip;

	public AudioClip reRootClip;

	public string launchSiteName;

	private PSystemSetup.SpaceCenterFacility selectedLaunchFacility;

	private LaunchSite selectedLaunchSite;

	private Ray ray;

	private RaycastHit hit;

	internal Collider FairingHitCollider;

	private int undoLevel;

	private int layerMask;

	private int undoLimit;

	public ShipConstruct ship;

	private ConstructionMode constructionMode;

	private Attachment attachment;

	private Attachment[] cPartAttachments;

	public static EditorLogic fetch;

	public Button partPanelBtn;

	public Button actionPanelBtn;

	public Button crewPanelBtn;

	public Button switchEditorBtn;

	public Button cargoPanelBtn;

	public UIStateToggleButton editorBtnStateToggle;

	public RectTransform crewBtnOrigPos;

	public RectTransform cargoBtnOrigPos;

	public RectTransform editorBtnOrigPos;

	public Button saveBtn;

	public Button launchBtn;

	public Button exitBtn;

	public Button loadBtn;

	public Button newBtn;

	public Button steamBtn;

	[SerializeField]
	private GameObject launchSiteSelector;

	public Button coordSpaceBtn;

	public TextMeshProUGUI coordSpaceText;

	public Button radialSymmetryBtn;

	public TextMeshProUGUI radialSymmetryText;

	public UIOnClick symmetryButton;

	public UIStateImage symmetrySprite;

	public UIStateImage mirrorSprite;

	public UIOnClick angleSnapButton;

	public UIStateImage angleSnapSprite;

	public TMP_InputField shipNameField;

	public TMP_InputField shipDescriptionField;

	public FlagBrowserButton flagBrowserButton;

	public Collider[] modalAreas;

	public bool allowSrfAttachment;

	public bool allowNodeAttachment;

	public GUISkin shipBrowserSkin;

	public Texture2D shipFileImage;

	private List<MissionRecoveryDialog> missionDialogs;

	public SymmetryMethod symmetryMethod;

	private SymmetryMethod symmetryMethodTmp;

	public int symmetryMode;

	public int symmetryModeTmp;

	private int symmetryModeBeforeNodeAttachment;

	private bool tmpSymMethodInUse;

	private EditorPartListFilter<AvailablePart> rootPartsOnlyFilter;

	private EditorPartListFilter<AvailablePart> inaccessiblePartsFilter;

	private Quaternion gizmoAttRotate;

	private Quaternion gizmoAttRotate0;

	private GizmoRotate gizmoRotate;

	private GizmoOffset gizmoOffset;

	private Space symmetryCoordSpace;

	private AudioSource audioSource;

	private bool setSteamExportItemPublic;

	private string setSteamExportItemChangeLog;

	private string setSteamModsText;

	private string steamExportTempPath;

	private VesselType setSteamVesselType;

	private bool setSteamRobiticsTag;

	private List<AppId_t> appDependenciesToRemove;

	private List<string> steamTags;

	private string steamThumbURL;

	private ERemoteStoragePublishedFileVisibility steamVisibility;

	private float steamTotalCost;

	private int steamCrewCapacity;

	private double timeSteamCommsStarted;

	private PopupDialog steamCommsDialog;

	private bool steamUploadingNewItem;

	public static EditorModes Mode;

	private bool skipPartAttach;

	private CraftBrowserDialog craftBrowserDialog;

	public static string FlagURL;

	private KFSMState st_podSelect;

	private KFSMState st_idle;

	private KFSMState st_place;

	private KFSMState st_offset_select;

	private KFSMState st_offset_tweak;

	private KFSMState st_rotate_select;

	private KFSMState st_rotate_tweak;

	private KFSMState st_root_unselected;

	private KFSMState st_root_select;

	private KFSMEvent on_podSelect;

	private KFSMEvent on_partCreated;

	private KFSMEvent on_partPicked;

	private KFSMEvent on_partCopied;

	private KFSMEvent on_partReveal;

	private KFSMEvent on_partDropped;

	private KFSMEvent on_partAttached;

	private KFSMEvent on_partDeleted;

	private KFSMEvent on_partLost;

	private KFSMEvent on_podDeleted;

	private KFSMEvent on_partOverInventoryPAW;

	private KFSMEvent on_goToModeOffset;

	private KFSMEvent on_offsetSelect;

	private KFSMEvent on_offsetDeselect;

	private KFSMEvent on_offsetReset;

	private KFSMEvent on_goToModeRotate;

	private KFSMEvent on_rotateSelect;

	private KFSMEvent on_rotateDeselect;

	private KFSMEvent on_rotateReset;

	private KFSMEvent on_goToModeRoot;

	private KFSMEvent on_rootPickSet;

	private KFSMEvent on_rootDeselect;

	private KFSMEvent on_rootSelect;

	private KFSMEvent on_rootSelectFail;

	private KFSMEvent on_goToModePlace;

	private KFSMEvent on_undoRedo;

	private KFSMEvent on_newShip;

	private KFSMEvent on_shipLoaded;

	private Quaternion attRot0;

	private int symUpdateMode;

	private Part symUpdateParent;

	private AttachNode symUpdateAttachNode;

	private Vector3 gizmoPivot;

	private Quaternion refRot;

	private Vector3 offsetGap;

	private float threshold;

	private AttachNode childToParent;

	private AttachNode parentToChild;

	private Vector3 diff;

	private List<PartSelector> rootCandidates;

	private int undoIndexAtLastSave;

	private string vesselNameAtLastSave;

	private string vesselNameAtLastSave_Sanitized;

	private string savedCraftPath;

	private LaunchSiteClear launchSiteClearTest;

	private bool panelButtonsLocked;

	private double startTime;

	private double modeStartTime;

	private Dictionary<string, double> timeInMode;

	private EditorScreen currentMode;

	public bool FSMStarted
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string currentStateName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string lastEventName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static string autoShipName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3 initialPodPosition
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Quaternion VesselRotation
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	private bool AllowSave
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Bounds editorBounds
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Part SelectedPart
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Part RootPart
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static int LayerMask
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ConstructionMode EditorConstructionMode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Texture2D ShipFileImage
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static List<Part> SortedShipList
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EditorLogic()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static EditorLogic()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CStart_003Ed__132))]
	private IEnumerator Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void StartEditor(bool isRestart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FlagSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMissionFlagSelect(FlagBrowser.FlagEntry selected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SelectPanelParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SelectPanelCargo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SelectPanelParts(bool isReset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SelectPanelActions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SwitchEditor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SelectPanelCrew()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void symButton(PointerEventData evtData = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void nextSymMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void prevSymMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetSymState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void snapButton(PointerEventData evtData = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void symInputUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void snapInputUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSymMethodChange(SymmetryMethod method)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void editorSwitchInput(bool keyOverride = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupFSM()
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
	private void OnPartPurchased(AvailablePart part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onConstructionModeChanged(ConstructionMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onRotateGizmoUpdate(Quaternion dRot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onRotateGizmoUpdated(Quaternion dRot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void partRotationResetUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3 getPivotOffset(Vector3 pivot0, Vector3 pivot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onOffsetGizmoUpdate(Vector3 dPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onOffsetGizmoUpdated(Vector3 dPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onOffsetGizmoBoundsUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void partOffsetInputUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnNewRootSelect(Part newRoot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Part pickPart(int layerMask, bool pickRoot, bool pickRootIfFrozen)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CenterDragPlane(Vector3 pos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool dragOverPlane(out Vector3 position)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetBackup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetBackup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UndoRedoInputUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RestoreState(int offset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnSubassemblyDialogDismiss(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPartListIconTap(AvailablePart p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPartListIconTap(ShipTemplate st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPartListBackgroundTap()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPodSpawn(AvailablePart pod)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SpawnPart(AvailablePart partInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetIconAsPart(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Please use UIPartActionControllerInventory.Instance.CreatePartFromInventory")]
	public Part CreatePartForInventoryUse(AvailablePart partInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ReleasePartToIcon()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SpawnTemplate(ShipTemplate st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SpawnConstruct(ShipConstruct construct)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Part DuplicatePart(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void DeletePart(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void deletePart(Part part, bool symmetry = false, bool printConfirmation = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DestroySelectedPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void deleteInputUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RestoreSymmetryState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RestoreSymmetryModeBeforeNodeAttachment()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void createSymmetry(int mode, Attachment attach)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdatePartAndChildren(Part SourcePart, Part UpdatePart, SymmetryMethod SymMethod)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateSymmetry(Part selPart, int symMode, Part partParent, AttachNode selPartNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateChildMirrorSymmetry(Part part, Part mirrorRoot, int symIdx)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void cleanSymmetry()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckCopiedSymmetryPersistentId(Part selectedPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void wipeSymmetry(Part selPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void deleteSymmetryParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void deletePartAndSymmetryParts(Part part, bool symmetry = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Part FirstNonSymmetricalParentFrom(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Attachment checkAttach(Part selPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool checkEditorCollision(Attachment attach)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Attachment[] CheckSymPartsAttach(Attachment oAttach)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void attachPart(Part part, Attachment attach)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void detachPart(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CopyActionGroups(Part sPart, Part cPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool attachSymParts(Attachment[] cAttaches)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void addToShip(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void removeFromShip(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void highlightSelected(bool attach, Part part, bool reset = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetLastSanitizedSaveName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void NewShip()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnNewShipConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnNewShipDialogDismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onShipNameFieldSubmit(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onShipNameFieldValueChanged(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void steamExport()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSteamExportConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSteamExportAfterSave()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onQuerySteamComplete(SteamUGCQueryCompleted_t qryResults, bool bIOFailure)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onNewItemCreated(CreateItemResult_t qryResults, bool bIOFailure)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void preupdateSteamItem(PublishedFileId_t fileId, bool newItem)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onGetAppDependency(GetAppDependenciesResult_t result, bool bIOFailure)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onAppDependencyRemoved(RemoveAppDependencyResult_t result, bool bIOFailure)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void updateSteamItem(PublishedFileId_t fileId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onItemUpdated(SubmitItemUpdateResult_t updateResult, bool bIOFailure)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void saveShip()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool saveShip(Callback afterSave)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool MissionCraftIsCreators(string shipName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool MissionCraftInOtherFolder(string shipName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSaveConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSaveConfirmDismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void exitEditor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onExitContinue()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onExitConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onExitConfirmDismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void loadShip()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void changeCoordSpace()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void changeRadialSymmetrySpace()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onLoadConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onLoadConfirmDismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void LoadShipFromFile(string path)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ShipToLoadSelected(string path, CraftBrowserDialog.LoadType loadType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ShipToLoadSelected(ConfigNode node, CraftBrowserDialog.LoadType loadType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CraftBrowseCancelled()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartVesselNamingChanged(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselNamingShipModified(ShipConstruct ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void launchVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void launchVessel(string siteName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static bool MissionCheckLaunchClamps(ShipConstruct ship, Part rootPart, VesselSituation vesselSituation, out ShipConstruct shipOut, bool overrideSituationCheck = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private PreFlightCheck GetStockPreFlightCheck(string siteName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private PreFlightCheck GetMissionPreFlightCheck(string siteName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void processMissionAppChecks()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void proceedWithMissionLaunch()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void proceedWithVesselLaunch()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onMissionDialogUp(MissionRecoveryDialog dialog)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onMissionDialogDismiss(MissionRecoveryDialog dialog)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void goForLaunch()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void abortLaunch()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CDelayedAbort_003Ed__315))]
	private IEnumerator DelayedAbort()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ResetCrewAssignment(ConfigNode craftNode, bool allowAutoHire)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshCrewAssignment(ConfigNode craftNode, Func<PartCrewManifest, bool> persistFilter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void RefreshCrewDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SpawningAC()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool partIsAttached(PartCrewManifest pcm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Func<PartCrewManifest, bool> GetPartExistsFilter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool partExists(PartCrewManifest pcm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateCrewAssignment()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void UpdateCrewManifest()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CDelayedUnlock_003Ed__326))]
	private IEnumerator DelayedUnlock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnInputLocksModified(GameEvents.FromToAction<ControlTypes, ControlTypes> ct)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Lock(bool lockLoad, bool lockExit, bool lockSave, string lockID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Unlock(string lockID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool NameOrDescriptionFocused()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AnyTextFieldHasFocus()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool mouseOverModalArea(Collider[] areas)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void WipeTooltips()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void partSearchUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<Part> getSortedShipList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void recurseShipList(Part part, List<Part> shipList)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDrawGizmosSelected()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AreAllPartsConnected()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int CountAllSceneParts(bool includeSelected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected bool ShipIsValid(ShipConstruct ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool HeldPartIsStacked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitAnalytics()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FinalizeAnalytics()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorScreenChange(EditorScreen screen)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnModeChange(EditorScreen mode)
	{
		throw null;
	}
}

using System.Runtime.CompilerServices;
using KSP.UI.Screens.Flight.Dialogs;
using UnityEngine;

public class FlagSite : PartModule
{
	private ConfigurableJoint joint;

	public AnimationClip flagDeployAnimation;

	public Animation animationRoot;

	public Collider[] colliders;

	public GameObject[] visibilityNodes;

	[KSPField]
	public float deployVisibilityDelay;

	[KSPField]
	public float deployFailRevertThreshold;

	[KSPField(isPersistant = true)]
	public string placedBy;

	[KSPField]
	public float unbreakablePeriodLength;

	[KSPField]
	public float breakForce;

	private KerbalFSM fsm;

	private KFSMState st_placing;

	private KFSMState st_placed;

	private KFSMState st_retracting;

	private KFSMState st_toppled;

	private KFSMEvent onPlacementFailed;

	private KFSMEvent onRetractStart;

	private KFSMEvent onTopple;

	private KFSMTimedEvent onPlaceComplete;

	private KFSMTimedEvent onRetractComplete;

	private string stateName;

	public Transform groundPivot;

	private PopupDialog SiteRenameDialog;

	private string siteName;

	private string newPlaqueText;

	[KSPField(isPersistant = true)]
	public string PlaqueText;

	private FlagPlaqueDialog FlagPlaqueDialog;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FlagSite()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onGameSceneLoadRequested(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupFSM()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MakeVisible()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MakeInvisible()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, unfocusedRange = 2f, guiName = "#autoLOC_6001833")]
	public void TakeDown()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, unfocusedRange = 2f, guiName = "#autoLOC_6001834")]
	public void PickUp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetJoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool findRigidbodyInVessel(Rigidbody rb, Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UnsetJoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnVesselPack()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnVesselUnpack()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPlacementFail()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPlacementComplete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnJointBreak(float breakForce)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static FlagSite CreateFlag(Vector3 position, Quaternion rotation, Part spawner)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 ScanSurroundingTerrain(Vector3 refPos, Vector3 fwd, float distance, int samples = 9)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RenameSite()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RenameSite(Callback afterDialog)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AcceptSiteRename()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CancelSiteRename()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DismissSiteRename()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, unfocusedRange = 2f, guiName = "#autoLOC_6001835")]
	public void ReadPlaque()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onFlagPlaqueDialogDismiss()
	{
		throw null;
	}
}

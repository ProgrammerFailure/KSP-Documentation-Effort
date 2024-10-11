using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class KSCVesselMarker : AnchoredDialog
{
	public enum DismissAction
	{
		None,
		Fly,
		Recover
	}

	private Vessel v;

	private string vesselName;

	private VesselType vesselType;

	private string situationString;

	private string crewHeaderString;

	private string crewNames;

	private string partString;

	[SerializeField]
	private Button Marker;

	[SerializeField]
	private Image Panel;

	[SerializeField]
	private Button FlyButton;

	[SerializeField]
	private Button RecoverButton;

	[SerializeField]
	private TextMeshProUGUI situationText;

	[SerializeField]
	private TextMeshProUGUI crewHeader;

	[SerializeField]
	private TextMeshProUGUI crewText;

	[SerializeField]
	private TextMeshProUGUI partsText;

	[SerializeField]
	private TextMeshProUGUI typeText;

	[SerializeField]
	private Image vesselIconPanel;

	[SerializeField]
	private Image vesselIconMarker;

	[SerializeField]
	private TextMeshProUGUI MarkerCaptionText;

	[SerializeField]
	private Sprite[] vesselIcons;

	public Callback<Vessel, DismissAction> OnDismiss;

	private bool expanded;

	private bool locked;

	private Coroutine ctrlCleanup;

	private XSelectable[] markerCtrls;

	private XSelectable[] panelCtrls;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSCVesselMarker()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static KSCVesselMarker Create(Vessel v, Callback<Vessel, DismissAction> onDismiss)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnPanelSetupComplete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string makeShipname(string originalName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Expand()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Collapse()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void CreateWindowContent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClearCtrls(XSelectable[] ctrls)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMarkerButtonInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFlyButtonInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRecoverButtonInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onInputLocksModified(GameEvents.FromToAction<ControlTypes, ControlTypes> inputLocks)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override string GetWindowTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnClickOut()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Dismiss(DismissAction dma)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnLateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void StartThis()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnDestroyThis()
	{
		throw null;
	}
}

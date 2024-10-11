using System.Runtime.CompilerServices;
using KSP.UI.Screens.Flight.Dialogs;
using KSP.UI.TooltipTypes;
using UnityEngine;

public class CrewHatchController : MonoBehaviour
{
	[SerializeField]
	private Tooltip_Text crewHatchTooltip;

	public static CrewHatchController fetch;

	private bool interfaceEnabled;

	private RaycastHit rayHit;

	private Part hoveredPart;

	private CrewHatchDialog crewDialog;

	private CrewTransfer crewTransfer;

	public Vector2 anchorOffset;

	public bool overrideTransfer;

	private Tooltip_Text CrewHatchTooltip
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public CrewHatchDialog CrewDialog
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public bool Active
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CrewHatchController()
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
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ShowTooltip()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void TooltipUpdate(Part hp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void HideTooltip()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Vector2 GetMouseUiPos()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SpawnCrewDialog(Part part, bool showEVA, bool showTransfer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnEVABtn(ProtoCrewMember crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnTransferBtn(ProtoCrewMember crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void DismissDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnCrewDialogDismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void DespawnUIs()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnVesselSwitch(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselSitChange(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> evt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnCrewTransferDone(PartItemTransfer.DismissAction dma, Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void EnableInterface()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DisableInterface()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ShowHatchTooltip(bool show)
	{
		throw null;
	}
}

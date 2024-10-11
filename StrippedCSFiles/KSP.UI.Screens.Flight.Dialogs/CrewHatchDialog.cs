using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace KSP.UI.Screens.Flight.Dialogs;

public class CrewHatchDialog : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI textHeader;

	[SerializeField]
	private TextMeshProUGUI textModuleCrew;

	[SerializeField]
	private GameObject widgetPrefab;

	[SerializeField]
	private GameObject emptyModuleObject;

	[SerializeField]
	private TextMeshProUGUI emptyModuleText;

	[SerializeField]
	private Transform listContainer;

	[SerializeField]
	private XSelectable hoverArea;

	private bool showTransfer;

	private bool showEVA;

	private Callback<ProtoCrewMember> onEVAEvtHandler;

	private Callback<ProtoCrewMember> onTransferEvtHandler;

	private Callback onDestroyEvtHandler;

	private Part part;

	private List<CrewHatchDialogWidget> widgets;

	private bool hover
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Part Part
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CrewHatchDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static CrewHatchDialog Spawn(Part p, bool showEVA, bool showTransfer, Callback<ProtoCrewMember> onEVAEvtHandler, Callback<ProtoCrewMember> onTransferEvtHandler, Callback onDismiss)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Terminate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnCameraChange(CameraManager.CameraMode data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnFlightUIModeChanged(FlightUIMode data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onVesselWasModified(Vessel data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onVesselChange(Vessel data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void CreatePanelContent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ClearList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void CreateList(List<ProtoCrewMember> crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnBtnEVA(ProtoCrewMember crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnBtnTransfer(ProtoCrewMember crew)
	{
		throw null;
	}
}

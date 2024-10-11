using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace KSP.UI.Screens;

public class AstronautComplex : MonoBehaviour
{
	[SerializeField]
	private CrewListItem widgetApplicants;

	[SerializeField]
	private UIList scrollListApplicants;

	[SerializeField]
	private CrewListItem widgetEnlisted;

	[SerializeField]
	private UIList scrollListAvailable;

	[SerializeField]
	private UIList scrollListAssigned;

	[SerializeField]
	private UIList scrollListKia;

	[SerializeField]
	private TextMeshProUGUI activeCrewsCount;

	[SerializeField]
	private TextMeshProUGUI nextHireCostField;

	[SerializeField]
	private TextMeshProUGUI availableCrewsTabText;

	[SerializeField]
	private TextMeshProUGUI assignedCrewsTabText;

	[SerializeField]
	private TextMeshProUGUI lostCrewsTabText;

	private int activeCrews;

	private int crewLimit;

	private VesselCrewManifest assignmentDialogManifest;

	public UIList ScrollListApplicants
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public UIList ScrollListAvailable
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public UIList ScrollListAssigned
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public UIList ScrollListKia
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AstronautComplex()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitiateGUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateApplicantList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateAvailableList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateKiaList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateAssignedList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddItem_Applicants(ProtoCrewMember crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddItem_Available(ProtoCrewMember crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddItem_Assigned(string name, float courage, float stupidity, CrewListItem.KerbalTypes type, string label, ProtoCrewMember crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddItem_Kia(ProtoCrewMember crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private CrewListItem AddItem(UIList list, CrewListItem widget)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Vbutton(CrewListItem.ButtonTypes type, CrewListItem clickItem)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Xbutton_AvailableCrew(CrewListItem.ButtonTypes type, CrewListItem clickItem)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Xbutton_MIA(CrewListItem.ButtonTypes type, CrewListItem clickItem)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void HireRecruit(UIList fromlist, UIList tolist, UIListItem listItem)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateCrewCounts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateCrewCosts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetApplicantsListUnlocked(bool unlocked, string lockReasonTitle = "", string lockReasonCaption = "")
	{
		throw null;
	}
}

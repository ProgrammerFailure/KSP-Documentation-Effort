using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class CrewCreationDialog : MonoBehaviour
{
	public delegate void CreateCallback(ProtoCrewMember kerbal);

	public delegate void CancelledCallback();

	public TMP_InputField kerbalName;

	public Toggle maleToggle;

	public Toggle femaleToggle;

	public TMP_Dropdown kerbalRole;

	public Slider kerbalExperience;

	public Slider kerbalCourage;

	public Slider kerbalStupidity;

	public Toggle veteranToggle;

	public Toggle badassToggle;

	public CreateCallback OnCrewCreate;

	public CancelledCallback OnDialogCancelled;

	[SerializeField]
	private Button btnCreate;

	[SerializeField]
	private Button btnRandom;

	private ProtoCrewMember crewToEdit;

	private List<ProtoCrewMember> tempCrewList;

	private KerbalRoster currentCrewRoster;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CrewCreationDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(CreateCallback onCrewCreate, CancelledCallback onDialogCancelled, List<ProtoCrewMember> tempCrewList = null, KerbalRoster currentCrewRoster = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRoleChanged(int selected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnButtonCreate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnButtonCancel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnButtonRandom()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Show()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Show(ProtoCrewMember crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Hide()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Dismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetValues(ProtoCrewMember crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ResetValues()
	{
		throw null;
	}
}

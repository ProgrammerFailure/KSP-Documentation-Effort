using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MissionValidationDialog : MonoBehaviour
{
	private static MissionValidationDialog Instance;

	[SerializeField]
	private MissionValidationEntry validationEntryPrefab;

	[SerializeField]
	private Button btnValidate;

	[SerializeField]
	private ToggleGroup entryGroup;

	[SerializeField]
	private Button btnOK;

	[SerializeField]
	private TMP_Dropdown modeDropdown;

	private static Mission currentMission;

	private Callback afterOKCallback;

	private Callback afterCancelCallback;

	private string[] modeNames;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionValidationDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MissionValidationDialog Display(Mission currentMission, Callback afterOKCallback = null, Callback afterCancelCallback = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildDropDown()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCancel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnOK()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DestroyDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnButtonValidate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PrintValidationResults(List<MissionValidationTestResult> results, bool includePasses = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClearValidationResults()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}
}

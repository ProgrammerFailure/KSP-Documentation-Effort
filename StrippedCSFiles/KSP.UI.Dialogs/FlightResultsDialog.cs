using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Dialogs;

public class FlightResultsDialog : MonoBehaviour
{
	private static FlightResultsDialog Instance;

	private bool display;

	public static bool showExitControls;

	public static bool allowClosingDialog;

	[SerializeField]
	private Button Btn_revLaunch;

	[SerializeField]
	private Button Btn_revEditor;

	[SerializeField]
	private Button Btn_returnEditor;

	[SerializeField]
	private Button Btn_TS;

	[SerializeField]
	private Button Btn_KSC;

	[SerializeField]
	private Button Btn_Menu;

	[SerializeField]
	private Button Btn_Close;

	[SerializeField]
	private TextMeshProUGUI txt_Outcome;

	[SerializeField]
	private TextMeshProUGUI txt_Results;

	[SerializeField]
	private TextMeshProUGUI txt_Achievements;

	private string titleMsg;

	public static bool isDisplaying
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	private string lastLogEntries
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FlightResultsDialog()
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
	public static void SetMissionOutcome(string msg)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static FlightResultsDialog Display(string outcomeMsg)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Close()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupGUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onLeavingFlight(GameScenes destination, EditorFacility facility)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onLeavingFlightProceed(GameScenes scn, EditorFacility facility)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onLeavingFlightDismiss()
	{
		throw null;
	}
}

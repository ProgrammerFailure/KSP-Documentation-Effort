using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Runtime;

public class MissionEndDialog : MonoBehaviour
{
	private static MissionEndDialog Instance;

	private bool display;

	public static bool showExitControls;

	public static bool allowClosingDialog;

	[SerializeField]
	private AwardWidget awardWidgetPrefab;

	[SerializeField]
	private Button Btn_SaveAndQuit;

	[SerializeField]
	private Button Btn_Restart;

	[SerializeField]
	private Button Btn_Close;

	[SerializeField]
	private Button Btn_Revert;

	[SerializeField]
	private Button Tab_Score;

	[SerializeField]
	private Button Tab_Details;

	[SerializeField]
	private RawImage image_Result;

	[SerializeField]
	private TextMeshProUGUI txt_EndMessage;

	[SerializeField]
	private TextMeshProUGUI scoreDetailsText;

	[SerializeField]
	private TextMeshProUGUI totalScoreText;

	[SerializeField]
	private TextMeshProUGUI statusMessageText;

	[SerializeField]
	private GameObject content_Score;

	[SerializeField]
	private GameObject content_Details;

	[SerializeField]
	private Transform awardsContent;

	private Mission finishedMission;

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
	public MissionEndDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MissionEndDialog Display(Mission finishedMission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Close()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Restart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Revert()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static DialogGUIBase[] drawRevertOptions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void DisplayScore()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void DisplayDetails()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupGUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SaveAndQuit()
	{
		throw null;
	}
}

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI.Screens.SpaceCenter.MissionSummaryDialog;
using KSP.UI.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class MissionRecoveryDialog : MonoBehaviour
{
	public enum Mode
	{
		None,
		Science,
		Parts,
		Crew
	}

	[SerializeField]
	private TextMeshProUGUI header;

	[SerializeField]
	private ToggleGroup tabGroup;

	[SerializeField]
	private Toggle toggleScience;

	[SerializeField]
	private Toggle toggleParts;

	[SerializeField]
	private Toggle toggleCrew;

	[SerializeField]
	private TextMeshProUGUI tabHeaderField;

	[SerializeField]
	private Button ButtonProceed;

	[SerializeField]
	private TextMeshProUGUI ButtonProceedCaption;

	[SerializeField]
	private RectTransform wContainerScience;

	[SerializeField]
	private RectTransform wContainerParts;

	[SerializeField]
	private RectTransform wContainerSpacer;

	[SerializeField]
	private RectTransform wContainerResources;

	[SerializeField]
	private RectTransform wContainerCrew;

	public Sprite dataIcon;

	public Sprite scienceIcon;

	public Sprite fundsIconGreen;

	public Sprite fundsIconRed;

	public Sprite reputationIconGreen;

	public Sprite reputationIconRed;

	private string windowTitle;

	private List<ScienceSubjectWidget> scienceWidgets;

	private List<PartWidget> partWidgets;

	private List<ResourceWidget> resourceWidgets;

	private List<CrewWidget> crewWidgets;

	[SerializeField]
	private ImgText fieldMissionEarned;

	[SerializeField]
	private ImgText fieldTotal;

	[SerializeField]
	private DragPanel dragPanel;

	public float beforeMissionScience;

	public float scienceEarned;

	public float totalScience;

	public double beforeMissionFunds;

	public double fundsEarned;

	public double totalFunds;

	public string FundsModifier;

	public string ScienceModifier;

	public string RepModifier;

	public string recoveryLocation;

	public string recoveryFactor;

	public float beforeMissionReputation;

	public float reputationEarned;

	public float totalReputation;

	public bool displayReputation;

	private bool MissionIsDebris;

	public bool RnDOperational;

	public Mode mode;

	public bool ScienceModeAvailable;

	public bool PartsModeAvailable;

	public bool CrewModeAvailable;

	private int partCount;

	private int rscCount;

	private string partCaption;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionRecoveryDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MissionRecoveryDialog CreateScienceDialog(ProtoVessel pv, float beforeMissionScience, float currentScience)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MissionRecoveryDialog CreateFullDialog(ProtoVessel pv)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGameSceneLoadRequested(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnSciTabPress(bool toggle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnPartsTabPress(bool toggle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnCrewTabPress(bool toggle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool MissionIsUninteresting()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddDataWidget(ScienceSubjectWidget widget)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddPartWidget(PartWidget widget)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddResourceWidget(ResourceWidget widget)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddCrewWidget(CrewWidget widget)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void dismissDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnProceedButtonClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected string GetProceedButtonCaption(Mode m)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetMode(Mode m)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void updateScienceWindowContent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void updatePartsWindowContent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void updateCrewWindowContent()
	{
		throw null;
	}
}

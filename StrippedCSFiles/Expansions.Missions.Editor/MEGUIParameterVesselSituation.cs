using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[MEGUI_VesselSituation]
public class MEGUIParameterVesselSituation : MEGUICompoundParameter
{
	public delegate void DelegateCallback(MEGUIParameterVesselSituation parameterReference);

	public RectTransform containerHeader;

	public RectTransform containerDropDowns;

	public RectTransform containerButtons;

	public Image imageHeader;

	public Image imageBackGround;

	public TMP_InputField inputVesselName;

	public Toggle togglePlayerBuilt;

	public Toggle toggleCustomDefined;

	public TMP_Dropdown dropDownVessels;

	public Button buttonSetCrew;

	public Button buttonReset;

	public Button buttonRemove;

	public Button buttonCreateVessel;

	public ToggleGroup toggleGroupVesselType;

	public Button buttonExpand;

	[SerializeField]
	private GameObject CollapsibleGroup;

	[SerializeField]
	private GameObject CollapsibleButtonIcon;

	private VesselSituation vesselSituation;

	private MissionSituation missionSituation;

	private MEGUIParameterCheckbox autoPopulateCrew;

	private MEGUIParameterTextArea vesselDescription;

	private MEGUIParameterCheckbox focusOnSpawn;

	private MEGUIParameterVesselLocation vesselGUILocation;

	private MEGUIParameterDynamicModuleList vesselRestrictionsListGUI;

	private MEGUIParameterSwitchCompound vesselpartFilter;

	private int dropdownVesselsIndex;

	private List<string> dropdownCraftLookup;

	private TextMeshProUGUI btnCreateVesselText;

	private bool btnCreateVesseltoEdit;

	private bool goToVAB;

	public VesselSituation FieldValue
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameterVesselSituation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Setup(string name, object value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private new void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Display()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void setupVessels()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void InputTextAction_SetVesselName(string newTitle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleAction_PlayerBuilt(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DropDownAction_Vessels(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ButtonAction_SetCrew()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ButtonAction_ResetVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void toggleExpand()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ButtonAction_CreateVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateVessel_SaveCompleted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onCreateVesselMissionSetupSuccess()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleAction_AutoPopulateCrew(bool selected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override ConfigNode GetState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void onHistorysetVesselName(ConfigNode data, HistoryType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void onHistoryDropDownVessels(ConfigNode data, HistoryType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void onHistoryResetVessel(ConfigNode data, HistoryType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void onHistoryTogglePlayerBuilt(ConfigNode data, HistoryType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnHistorySetCrew(ConfigNode data, HistoryType type)
	{
		throw null;
	}
}

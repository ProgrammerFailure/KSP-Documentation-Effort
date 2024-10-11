using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI;

public class ManeuverToolUIFrame : GenericAppFrame
{
	public delegate void OnActionDelegate(bool state);

	public delegate void OnErrorStateDelegate(bool state);

	public AppUIMemberDropdown transferTypeParent;

	public TMP_Dropdown transferTypeDropDown;

	public TMP_Dropdown positionDropDown;

	[SerializeField]
	private ToggleGroup timeReadoutGroup;

	[SerializeField]
	private ToggleGroup timeDisplayGroup;

	[SerializeField]
	private Toggle timeDisplayUT;

	[SerializeField]
	private Toggle timeDisplayOffsetTime;

	[SerializeField]
	private Toggle timeDisplayLong;

	[SerializeField]
	private Toggle timeDisplaySecs;

	[SerializeField]
	private TextMeshProUGUI transferTypeHeader;

	public AppUIInputPanel inputPanel;

	[SerializeField]
	private TextMeshProUGUI errorText;

	[SerializeField]
	private GameObject bottomSection;

	[SerializeField]
	private Button createManeuverBtn;

	[SerializeField]
	private Toggle createAlarmToggle;

	[SerializeField]
	private TextMeshProUGUI timeDisplay;

	[SerializeField]
	private TextMeshProUGUI dvDisplay;

	public ManeuverTool maneuverApp;

	[SerializeField]
	private TextMeshProUGUI helpText;

	private bool errorState;

	[SerializeField]
	private GameObject topPanel;

	[SerializeField]
	private Button btnTopPanel;

	[SerializeField]
	private UIPanelTransition topPanelTransition;

	public AppUIInputPanel topPanelData;

	[SerializeField]
	private TextMeshProUGUI topPanelHeader;

	[SerializeField]
	private TextMeshProUGUI topPanelCol1Header;

	[SerializeField]
	private TextMeshProUGUI topPanelCol2Header;

	[SerializeField]
	private TextMeshProUGUI topPanelCol3Header;

	private bool topPanelActive;

	private bool topPanelExtended;

	[SerializeField]
	private GameObject visualPanel;

	[SerializeField]
	private Button btnVisualPanel;

	[SerializeField]
	private UIPanelTransition visualPanelTransition;

	private bool visualPanelActive;

	private bool visualPanelExtended;

	[SerializeField]
	private ManeuverToolCBContainer planetContainerPrefab;

	[SerializeField]
	private List<ManeuverToolCBContainer> planetContainers;

	[SerializeField]
	private Transform sidePanelParentTransform;

	public Vector2 sourceBodyLocalPos;

	public Vector2 targetBodyLocalPos;

	public Vector2 parentBodyLocalPos;

	private OnActionDelegate onSidePanelClicked;

	private OnErrorStateDelegate onErrorStateChanged;

	public bool ErrorState
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool TopPanelTransitioning
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool TopPanelActive
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool TopPanelExtended
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool VisualPanelTransitioning
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool VisualPanelActive
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool VisualPanelExtended
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ManeuverToolUIFrame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddOnSidePanelClicked(OnActionDelegate method)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveOnSidePanelClicked(OnActionDelegate method)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddOnErrorStateChanged(OnErrorStateDelegate method)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveOnErrorStateChanged(OnErrorStateDelegate method)
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
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreatePlanets()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ManeuverToolCBContainer AddPlanetRecursively(PSystemBody body, float offset_pos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetTopPanelState(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void TransitionTopPanelImmediate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnBtnTopPanelClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnBtnSidePanelClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetVisualPanelState(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void TransitionVisualPanelImmediate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTimeDisplayUTToggle(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTimeDisplaySecToggle(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCreateManeuverBtn()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateTimeText(string text)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateDVText(string text)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetHelpString(string text)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetErrorState(bool state, string text, AppUI_Data data, bool toggleInputPanel, Callback onDataChanged)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetHeaderText(string text)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetCalculationState(TransferDataBase.ManeuverCalculationState state, int percentage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetCreateManeuverButton(bool interactable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetTopPanelHeader(string topHeader, string col1Header, string col2Header, string col3Header)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ManeuverToolCBContainer ActivatePlanet(string bodyName, Vector2 position, float scale)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DeactivatePlanet(string bodyName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DeactivatePlanet(ManeuverToolCBContainer planet)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ManeuverToolCBContainer FindPlanet(string bodyName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GameObject AddObjectToVisualizerWindow(GameObject prefab)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GameObject AddObjectToVisualizerWindow(GameObject prefab, Vector2 localPosition, Vector3 localScale)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RemoveObjectFromVisualizerWindow(string name)
	{
		throw null;
	}
}

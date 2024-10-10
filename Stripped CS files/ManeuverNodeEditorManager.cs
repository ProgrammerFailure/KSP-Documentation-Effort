using System.Collections;
using System.Collections.Generic;
using ns12;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ManeuverNodeEditorManager : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public class UsageStats
	{
		public int leftTabChange;

		public int rightTabChange;

		public int nodeSelector;

		public int warpTo;

		public int deleteNode;

		public int vectorHandle;

		public int vectorText;

		public int utHandle;

		public int utText;

		public int precisionSlider;

		public int orbitSelection;

		public bool UsageFound
		{
			get
			{
				if (leftTabChange <= 0 && rightTabChange <= 0 && nodeSelector <= 0 && vectorHandle <= 0 && vectorText <= 0 && utHandle <= 0 && utText <= 0 && precisionSlider <= 0 && warpTo <= 0 && deleteNode <= 0)
				{
					return orbitSelection > 0;
				}
				return true;
			}
		}

		public void Reset()
		{
			leftTabChange = 0;
			rightTabChange = 0;
			nodeSelector = 0;
			vectorHandle = 0;
			vectorText = 0;
			utHandle = 0;
			utText = 0;
			precisionSlider = 0;
			warpTo = 0;
			deleteNode = 0;
			orbitSelection = 0;
		}
	}

	public static ManeuverNodeEditorManager Instance;

	public List<ManeuverNodeEditorTab> maneuverNodeEditorTabs;

	public Button deleteNodeButton;

	public Button nextNodeButton;

	public Button prevNodeButton;

	public Button warpToNextNodeButton;

	public TextMeshProUGUI nodeName;

	public ManeuverNode _selectedManeuverNode;

	public int _selectedManNodeIndex = -1;

	public Vector3d newDeltaV;

	public bool _mouseWithinTool;

	public bool ready;

	[SerializeField]
	public ManeuverNodeEditorTabButton tabButtonPrefab;

	[SerializeField]
	public RectTransform leftTabButtonsParent;

	[SerializeField]
	public RectTransform leftTabsParent;

	[SerializeField]
	public RectTransform rightTabButtonsParent;

	[SerializeField]
	public RectTransform rightTabsParent;

	public List<ManeuverNodeEditorTab> leftTabs;

	public List<ManeuverNodeEditorTab> rightTabs;

	public List<ManeuverNodeEditorTabButton> leftTabButtons;

	public List<ManeuverNodeEditorTabButton> rightTabButtons;

	public bool justSelected;

	public WaitForSeconds selectionCooldown = new WaitForSeconds(1f);

	public ManeuverNode previousManNode;

	public bool isActive;

	public double tempWarpToUT;

	public static string cacheAutoLOC_7001217;

	public static string cacheAutoLOC_198614;

	public UsageStats usage;

	public ManeuverNode SelectedManeuverNode => _selectedManeuverNode;

	public int SelectedManNodeIndex => _selectedManNodeIndex;

	public bool MouseWithinTool => _mouseWithinTool;

	public bool IsReady => ready;

	public List<ManeuverNodeEditorTab> LeftTabs => leftTabs;

	public List<ManeuverNodeEditorTab> RightTabs => rightTabs;

	public bool JustSelectedNode => justSelected;

	public bool IsActive => isActive;

	public void Awake()
	{
		if ((bool)Instance)
		{
			Object.Destroy(this);
			return;
		}
		Instance = this;
		usage = new UsageStats();
	}

	public void Start()
	{
		leftTabs = new List<ManeuverNodeEditorTab>();
		rightTabs = new List<ManeuverNodeEditorTab>();
		leftTabButtons = new List<ManeuverNodeEditorTabButton>();
		rightTabButtons = new List<ManeuverNodeEditorTabButton>();
		deleteNodeButton.onClick.AddListener(DeleteManNode);
		nextNodeButton.onClick.AddListener(NextManNode);
		prevNodeButton.onClick.AddListener(PrevManNode);
		warpToNextNodeButton.onClick.AddListener(WarpToNextManNode);
		GameEvents.OnFlightUIModeChanged.Add(onUIModeFinishedChanging);
		GameEvents.onManeuverNodeDeselected.Add(OnManNodeDeselected);
		GameEvents.onManeuverNodeSelected.Add(OnManNodeSelected);
		GameEvents.OnTargetObjectChanged.Add(OnTargetChanged);
		GameEvents.onGameSceneSwitchRequested.Add(OnSceneSwitch);
		GameEvents.OnMapEntered.Add(OnMapEntered);
		GameEvents.OnMapExited.Add(OnMapExited);
		GameEvents.onGamePause.Add(SetPauseButtonState);
		GameEvents.onGameUnpause.Add(SetPauseButtonState);
		setupAllTabs();
		if (rightTabs.Count > 0)
		{
			SetCurrentTab(0, ManeuverNodeEditorTabPosition.RIGHT);
		}
		if (leftTabs.Count > 0)
		{
			SetCurrentTab(0, ManeuverNodeEditorTabPosition.LEFT);
		}
		UpdateButtonAvailability();
		if (FlightGlobals.ActiveVessel.targetObject != null)
		{
			ManeuverNodeEditorTabButton tabToggle = GetTabToggle(0, ManeuverNodeEditorTabPosition.LEFT);
			for (int i = 0; i < maneuverNodeEditorTabs.Count; i++)
			{
				if (maneuverNodeEditorTabs[i].tabName.Equals("Intercept"))
				{
					tabToggle = GetTabToggle(i, ManeuverNodeEditorTabPosition.LEFT);
					break;
				}
			}
			ChangeTooltipText(tabToggle.tooltip, Localizer.Format("#autoLOC_6005040", FlightGlobals.ActiveVessel.targetObject.GetDisplayName()));
		}
		ready = true;
	}

	public void Update()
	{
		UpdateTabButtons(ref leftTabButtons, ref leftTabs);
		UpdateTabButtons(ref rightTabButtons, ref rightTabs);
	}

	public void UpdateTabButtons(ref List<ManeuverNodeEditorTabButton> tabButtonList, ref List<ManeuverNodeEditorTab> tabList)
	{
		for (int i = 0; i < tabList.Count; i++)
		{
			tabButtonList[i].toggle.interactable = tabList[i].IsTabInteractable();
			if (!tabList[i].tabManagesCaption)
			{
				if (tabButtonList[i].toggle.interactable)
				{
					tabButtonList[i].tooltip.textString = tabList[i].tabTooltipCaptionActive;
				}
				else
				{
					tabButtonList[i].tooltip.textString = tabList[i].tabTooltipCaptionInactive;
				}
			}
		}
	}

	public static void addTab(ManeuverNodeEditorTab newItem)
	{
		if (!Instance.maneuverNodeEditorTabs.Contains(newItem))
		{
			Instance.maneuverNodeEditorTabs.Add(newItem);
		}
		Instance.setupAllTabs();
	}

	public ManeuverNodeEditorTabButton GetTabToggle(int index, ManeuverNodeEditorTabPosition type)
	{
		ManeuverNodeEditorTabButton result = null;
		if (type != 0 && type == ManeuverNodeEditorTabPosition.RIGHT)
		{
			if (index < rightTabButtons.Count)
			{
				result = rightTabButtons[index];
			}
		}
		else if (index < leftTabButtons.Count)
		{
			result = leftTabButtons[index];
		}
		return result;
	}

	public void ModifyBurnVector(NavBallVector axis, double amount)
	{
		switch (axis)
		{
		case NavBallVector.PROGRADE:
			newDeltaV.z = amount;
			UpdateDeltaVInNodeComponents();
			break;
		case NavBallVector.NORMAL:
			newDeltaV.y = amount;
			UpdateDeltaVInNodeComponents();
			break;
		case NavBallVector.RADIAL:
			newDeltaV.x = amount;
			UpdateDeltaVInNodeComponents();
			break;
		case NavBallVector.const_3:
			SelectedManeuverNode.double_0 = amount;
			SelectedManeuverNode.attachedGizmo.double_0 = amount;
			break;
		}
		FlightGlobals.ActiveVessel.patchedConicSolver.UpdateFlightPlan();
	}

	public void SetManeuverNodeInitialValues()
	{
		AssignSelectedManeuverNode();
		UpdateButtonAvailability();
		if (_selectedManeuverNode != null)
		{
			nodeName.text = Localizer.Format("#autoLOC_6100053", cacheAutoLOC_198614, (_selectedManNodeIndex + 1).ToString()).ToUpper();
		}
	}

	public void AssignSelectedManeuverNode()
	{
		_selectedManeuverNode = null;
		int num = FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes.Count - 1;
		while (num >= 0)
		{
			if (!(FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes[num].attachedGizmo != null))
			{
				num--;
				continue;
			}
			_selectedManeuverNode = FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes[num];
			_selectedManNodeIndex = num;
			nodeName.text = Localizer.Format("#autoLOC_6100053", cacheAutoLOC_198614, (_selectedManNodeIndex + 1).ToString()).ToUpper();
			newDeltaV = _selectedManeuverNode.DeltaV;
			break;
		}
		if (_selectedManeuverNode != null || _selectedManNodeIndex <= -1 || FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes.Count <= 0)
		{
			return;
		}
		_selectedManeuverNode = FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes[_selectedManNodeIndex];
		_selectedManeuverNode.AttachGizmo(MapView.ManeuverNodePrefab, FlightGlobals.ActiveVessel.patchedConicRenderer);
		for (int num2 = FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes.Count - 1; num2 >= 0; num2--)
		{
			if (num2 != _selectedManNodeIndex)
			{
				FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes[num2].DetachGizmo();
			}
		}
	}

	public void OnManNodeDeselected()
	{
		if (!justSelected)
		{
			_selectedManeuverNode = null;
			_selectedManNodeIndex = -1;
			FlightUIModeController.Instance.SetMode(FlightUIMode.MANEUVER_INFO);
			UpdateButtonAvailability();
			nodeName.text = cacheAutoLOC_7001217.ToUpper();
		}
	}

	public void OnManNodeSelected()
	{
		justSelected = true;
		InputLockManager.SetControlLock("mannodeSelected");
		StartCoroutine("ManNodeCreateCooldown");
	}

	public void ChangeTooltipText(TooltipController_Text toolTip, string text)
	{
		toolTip.textString = text;
	}

	public void OnTargetChanged(MapObject target)
	{
		ManeuverNodeEditorTabButton tabToggle = GetTabToggle(0, ManeuverNodeEditorTabPosition.LEFT);
		for (int i = 0; i < maneuverNodeEditorTabs.Count; i++)
		{
			if (maneuverNodeEditorTabs[i].tabName.Equals("Intercept"))
			{
				tabToggle = GetTabToggle(i, ManeuverNodeEditorTabPosition.LEFT);
				break;
			}
		}
		if (target == null)
		{
			ChangeTooltipText(tabToggle.tooltip, Localizer.Format("#autoLOC_6005041"));
			SetCurrentTab(0, ManeuverNodeEditorTabPosition.LEFT);
		}
		else
		{
			ChangeTooltipText(tabToggle.tooltip, Localizer.Format("#autoLOC_6005040", target.GetDisplayName()));
		}
	}

	public IEnumerator ManNodeCreateCooldown()
	{
		yield return selectionCooldown;
		justSelected = false;
		AssignSelectedManeuverNode();
		for (int i = 0; i < maneuverNodeEditorTabs.Count; i++)
		{
			if (SelectedManeuverNode != null)
			{
				maneuverNodeEditorTabs[i].SetInitialValues();
			}
			else
			{
				OnManNodeDeselected();
			}
		}
		InputLockManager.RemoveControlLock("mannodeSelected");
	}

	public void UpdateDeltaVInNodeComponents()
	{
		if (_selectedManeuverNode != null && _selectedManeuverNode.attachedGizmo != null)
		{
			_selectedManeuverNode.DeltaV = newDeltaV;
			_selectedManeuverNode.attachedGizmo.DeltaV = newDeltaV;
		}
	}

	public void setupAllTabs()
	{
		leftTabs.Clear();
		rightTabs.Clear();
		leftTabButtons.Clear();
		rightTabButtons.Clear();
		for (int i = 0; i < maneuverNodeEditorTabs.Count; i++)
		{
			ManeuverNodeEditorTab maneuverNodeEditorTab = maneuverNodeEditorTabs[i];
			ManeuverNodeEditorTab component = Object.Instantiate(maneuverNodeEditorTab.gameObject).GetComponent<ManeuverNodeEditorTab>();
			if (maneuverNodeEditorTab.tabPosition == ManeuverNodeEditorTabPosition.LEFT)
			{
				component.Setup(leftTabsParent.transform);
				leftTabs.Add(component);
				ManeuverNodeEditorTabButton maneuverNodeEditorTabButton = Object.Instantiate(tabButtonPrefab);
				maneuverNodeEditorTabButton.Setup(leftTabButtonsParent, maneuverNodeEditorTab, OnTabButtonPressed);
				leftTabButtons.Add(maneuverNodeEditorTabButton);
			}
			else
			{
				component.Setup(rightTabsParent.transform);
				rightTabs.Add(component);
				ManeuverNodeEditorTabButton maneuverNodeEditorTabButton = Object.Instantiate(tabButtonPrefab);
				maneuverNodeEditorTabButton.Setup(rightTabButtonsParent, maneuverNodeEditorTab, OnTabButtonPressed);
				rightTabButtons.Add(maneuverNodeEditorTabButton);
			}
		}
	}

	public void OnTabButtonPressed(bool pressedToggle)
	{
		for (int i = 0; i < leftTabs.Count; i++)
		{
			if (leftTabButtons[i].toggle.isOn)
			{
				SetCurrentTab(i, ManeuverNodeEditorTabPosition.LEFT);
			}
		}
		for (int j = 0; j < rightTabs.Count; j++)
		{
			if (rightTabButtons[j].toggle.isOn)
			{
				SetCurrentTab(j, ManeuverNodeEditorTabPosition.RIGHT);
			}
		}
	}

	public void SetCurrentTab(int selected, ManeuverNodeEditorTabPosition type)
	{
		if (type != 0 && type == ManeuverNodeEditorTabPosition.RIGHT)
		{
			for (int i = 0; i < rightTabs.Count; i++)
			{
				if (i == selected)
				{
					if (IsReady && !rightTabs[i].gameObject.activeSelf)
					{
						usage.rightTabChange++;
					}
					rightTabs[i].gameObject.SetActive(value: true);
					rightTabButtons[i].toggle.isOn = true;
				}
				else
				{
					rightTabs[i].gameObject.SetActive(value: false);
					rightTabButtons[i].toggle.isOn = false;
				}
			}
			return;
		}
		for (int j = 0; j < leftTabs.Count; j++)
		{
			if (j == selected)
			{
				if (IsReady && !leftTabs[j].gameObject.activeSelf)
				{
					usage.leftTabChange++;
				}
				leftTabs[j].gameObject.SetActive(value: true);
				leftTabButtons[j].toggle.isOn = true;
			}
			else
			{
				leftTabs[j].gameObject.SetActive(value: false);
				leftTabButtons[j].toggle.isOn = false;
			}
		}
	}

	public void UpdateButtonAvailability()
	{
		if (MapView.MapIsEnabled && FlightGlobals.ActiveVessel.patchedConicSolver != null)
		{
			deleteNodeButton.interactable = _selectedManeuverNode != null && _selectedManNodeIndex >= 0;
			warpToNextNodeButton.interactable = _selectedManeuverNode != null && _selectedManNodeIndex == 0;
			prevNodeButton.interactable = FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes.Count > 0;
			nextNodeButton.interactable = FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes.Count > 0;
		}
		else
		{
			deleteNodeButton.interactable = false;
			warpToNextNodeButton.interactable = false;
			prevNodeButton.interactable = false;
			nextNodeButton.interactable = false;
		}
	}

	public void onUIModeFinishedChanging(FlightUIMode data)
	{
		UpdateTabButtons(ref leftTabButtons, ref leftTabs);
		UpdateTabButtons(ref rightTabButtons, ref rightTabs);
		UpdateButtonAvailability();
		if (data != FlightUIMode.MANEUVER_INFO && data != FlightUIMode.MANEUVER_EDIT)
		{
			isActive = false;
		}
		else
		{
			isActive = true;
		}
	}

	public void DeleteManNode()
	{
		if (FlightGlobals.ActiveVessel.patchedConicSolver != null)
		{
			if (_selectedManeuverNode != null)
			{
				previousManNode = _selectedManeuverNode;
				previousManNode.DetachGizmo();
				previousManNode.RemoveSelf();
				_selectedManeuverNode = null;
				previousManNode = null;
				_selectedManNodeIndex--;
				for (int num = FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes.Count - 1; num >= 0; num--)
				{
					if (FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes[num].attachedGizmo != null)
					{
						FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes[num].DetachGizmo();
					}
				}
				FlightUIModeController.Instance.SetMode(FlightUIMode.MANEUVER_INFO);
				UpdateButtonAvailability();
				nodeName.text = cacheAutoLOC_7001217.ToUpper();
				usage.deleteNode++;
			}
			else
			{
				UpdateButtonAvailability();
			}
		}
		else
		{
			UpdateButtonAvailability();
		}
	}

	public void NextManNode()
	{
		if (FlightGlobals.ActiveVessel.patchedConicSolver != null)
		{
			justSelected = true;
			if (_selectedManNodeIndex < -1)
			{
				_selectedManNodeIndex = -1;
			}
			else if (_selectedManNodeIndex == -1)
			{
				if (FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes.Count > 0)
				{
					FlightUIModeController.Instance.SetMode(FlightUIMode.MANEUVER_EDIT);
					_selectedManNodeIndex++;
					_selectedManeuverNode = FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes[_selectedManNodeIndex];
					_selectedManeuverNode.AttachGizmo(MapView.ManeuverNodePrefab, FlightGlobals.ActiveVessel.patchedConicRenderer);
					UpdateButtonAvailability();
					nodeName.text = Localizer.Format("#autoLOC_6100053", cacheAutoLOC_198614, (_selectedManNodeIndex + 1).ToString()).ToUpper();
				}
				else
				{
					UpdateButtonAvailability();
				}
				TriggerManNodeChange();
			}
			else if (_selectedManNodeIndex == FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes.Count - 1)
			{
				FlightUIModeController.Instance.SetMode(FlightUIMode.MANEUVER_INFO);
				_selectedManNodeIndex = -1;
				UpdateButtonAvailability();
				nodeName.text = cacheAutoLOC_7001217.ToUpper();
				_selectedManeuverNode.DetachGizmo();
				_selectedManeuverNode = null;
			}
			else
			{
				FlightUIModeController.Instance.SetMode(FlightUIMode.MANEUVER_EDIT);
				previousManNode = _selectedManeuverNode;
				_selectedManNodeIndex++;
				_selectedManeuverNode = FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes[_selectedManNodeIndex];
				_selectedManeuverNode.AttachGizmo(MapView.ManeuverNodePrefab, FlightGlobals.ActiveVessel.patchedConicRenderer);
				UpdateButtonAvailability();
				nodeName.text = Localizer.Format("#autoLOC_6100053", cacheAutoLOC_198614, (_selectedManNodeIndex + 1).ToString()).ToUpper();
				if (previousManNode != null && previousManNode.attachedGizmo != null)
				{
					previousManNode.DetachGizmo();
					previousManNode = null;
				}
				TriggerManNodeChange();
			}
			usage.nodeSelector++;
		}
		else
		{
			UpdateButtonAvailability();
		}
	}

	public void PrevManNode()
	{
		if (FlightGlobals.ActiveVessel.patchedConicSolver != null)
		{
			previousManNode = _selectedManeuverNode;
			justSelected = true;
			if (_selectedManNodeIndex <= -1)
			{
				FlightUIModeController.Instance.SetMode(FlightUIMode.MANEUVER_EDIT);
				_selectedManNodeIndex = FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes.Count - 1;
				_selectedManeuverNode = ((_selectedManNodeIndex == -1) ? null : FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes[_selectedManNodeIndex]);
				if (_selectedManeuverNode != null)
				{
					_selectedManeuverNode.AttachGizmo(MapView.ManeuverNodePrefab, FlightGlobals.ActiveVessel.patchedConicRenderer);
				}
				UpdateButtonAvailability();
				nodeName.text = Localizer.Format("#autoLOC_6100053", "#autoLOC_198614", (_selectedManNodeIndex + 1).ToString()).ToUpper();
				if (previousManNode != null && previousManNode.attachedGizmo != null)
				{
					previousManNode.DetachGizmo();
					previousManNode = null;
				}
				TriggerManNodeChange();
			}
			else if (_selectedManNodeIndex == 0)
			{
				FlightUIModeController.Instance.SetMode(FlightUIMode.MANEUVER_INFO);
				_selectedManNodeIndex--;
				UpdateButtonAvailability();
				nodeName.text = cacheAutoLOC_7001217.ToUpper();
				_selectedManeuverNode.DetachGizmo();
				_selectedManeuverNode = null;
			}
			else
			{
				FlightUIModeController.Instance.SetMode(FlightUIMode.MANEUVER_EDIT);
				previousManNode = _selectedManeuverNode;
				_selectedManNodeIndex--;
				_selectedManeuverNode = ((_selectedManNodeIndex == -1) ? null : FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes[_selectedManNodeIndex]);
				if (_selectedManeuverNode != null)
				{
					_selectedManeuverNode.AttachGizmo(MapView.ManeuverNodePrefab, FlightGlobals.ActiveVessel.patchedConicRenderer);
				}
				UpdateButtonAvailability();
				nodeName.text = Localizer.Format("#autoLOC_6100053", "#autoLOC_198614", (_selectedManNodeIndex + 1).ToString()).ToUpper();
				if (previousManNode != null && previousManNode.attachedGizmo != null)
				{
					previousManNode.DetachGizmo();
					previousManNode = null;
				}
				TriggerManNodeChange();
			}
			usage.nodeSelector++;
		}
		else
		{
			UpdateButtonAvailability();
		}
	}

	public void TriggerManNodeChange()
	{
		for (int i = 0; i < RightTabs.Count; i++)
		{
			RightTabs[i].SetInitialValues();
		}
		GameEvents.onManeuverNodeSelected.Fire();
	}

	public void WarpToNextManNode()
	{
		if (FlightGlobals.ActiveVessel.patchedConicSolver != null)
		{
			if (_selectedManeuverNode != null)
			{
				tempWarpToUT = ((_selectedManeuverNode.startBurnIn > 0.0) ? (_selectedManeuverNode.startBurnIn + Planetarium.GetUniversalTime() - (double)GameSettings.WARP_TO_MANNODE_MARGIN) : (_selectedManeuverNode.double_0 - (double)GameSettings.WARP_TO_MANNODE_MARGIN));
				TimeWarp.fetch.WarpTo(tempWarpToUT);
				usage.warpTo++;
			}
			else
			{
				UpdateButtonAvailability();
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		SetMouseOverGizmo(state: true);
		_mouseWithinTool = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		SetMouseOverGizmo(state: false);
		_mouseWithinTool = false;
	}

	public void SetMouseOverGizmo(bool state)
	{
		if (!MapView.MapIsEnabled || !(FlightGlobals.ActiveVessel.patchedConicSolver != null))
		{
			return;
		}
		for (int i = 0; i < FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes.Count; i++)
		{
			ManeuverGizmo attachedGizmo = FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes[i].attachedGizmo;
			if (attachedGizmo != null)
			{
				attachedGizmo.SetMouseOverGizmo(state);
			}
		}
	}

	public void OnDestroy()
	{
		deleteNodeButton.onClick.RemoveListener(DeleteManNode);
		nextNodeButton.onClick.RemoveListener(NextManNode);
		prevNodeButton.onClick.RemoveListener(PrevManNode);
		warpToNextNodeButton.onClick.RemoveListener(WarpToNextManNode);
		GameEvents.OnFlightUIModeChanged.Remove(onUIModeFinishedChanging);
		GameEvents.onManeuverNodeDeselected.Remove(OnManNodeDeselected);
		GameEvents.onManeuverNodeSelected.Remove(OnManNodeSelected);
		GameEvents.OnTargetObjectChanged.Remove(OnTargetChanged);
		GameEvents.onGameSceneSwitchRequested.Remove(OnSceneSwitch);
		GameEvents.OnMapEntered.Remove(OnMapEntered);
		GameEvents.OnMapExited.Remove(OnMapExited);
		GameEvents.onGamePause.Remove(SetPauseButtonState);
		GameEvents.onGameUnpause.Remove(SetPauseButtonState);
		leftTabs.Clear();
		leftTabs = null;
		rightTabs.Clear();
		rightTabs = null;
		leftTabButtons.Clear();
		leftTabButtons = null;
		rightTabButtons.Clear();
		rightTabButtons = null;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_7001217 = Localizer.Format("#autoLOC_7001217");
		cacheAutoLOC_198614 = Localizer.Format("#autoLOC_198614");
	}

	public void OnSceneSwitch(GameEvents.FromToAction<GameScenes, GameScenes> scn)
	{
		if (!MapView.MapIsEnabled && usage.UsageFound)
		{
			AnalyticsUtil.ManeuverModeUsage(HighLogic.CurrentGame, MapView.MapIsEnabled, usage);
			usage.Reset();
		}
	}

	public void SetPauseButtonState()
	{
		Selectable[] componentsInChildren = GetComponentsInChildren<Selectable>();
		if (FlightDriver.Pause)
		{
			int num = componentsInChildren.Length;
			while (num-- > 0)
			{
				componentsInChildren[num].interactable = false;
			}
			for (int i = 0; i < leftTabButtons.Count; i++)
			{
				leftTabButtons[i].toggle.interactable = false;
			}
			for (int j = 0; j < rightTabButtons.Count; j++)
			{
				rightTabButtons[j].toggle.interactable = false;
			}
		}
		else
		{
			int num2 = componentsInChildren.Length;
			while (num2-- > 0)
			{
				componentsInChildren[num2].interactable = true;
			}
			for (int k = 0; k < leftTabButtons.Count; k++)
			{
				leftTabButtons[k].toggle.interactable = true;
			}
			for (int l = 0; l < rightTabButtons.Count; l++)
			{
				rightTabButtons[l].toggle.interactable = true;
			}
		}
	}

	public void OnMapEntered()
	{
		if (usage.UsageFound)
		{
			AnalyticsUtil.ManeuverModeUsage(HighLogic.CurrentGame, !MapView.MapIsEnabled, usage);
			usage.Reset();
		}
	}

	public void OnMapExited()
	{
		if (usage.UsageFound)
		{
			AnalyticsUtil.ManeuverModeUsage(HighLogic.CurrentGame, !MapView.MapIsEnabled, usage);
			usage.Reset();
		}
	}
}

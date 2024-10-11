using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using KSP.UI;
using KSP.UI.TooltipTypes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[Serializable]
public class MEGUINode : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IMEHistoryTarget
{
	[Serializable]
	internal enum ColoredConnectorOptions
	{
		None,
		InputOnly,
		OutputOnly,
		Both
	}

	[CompilerGenerated]
	private sealed class _003CDelayedUpdateConnectors_003Ed__93 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MEGUINode _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CDelayedUpdateConnectors_003Ed__93(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	public TextMeshProUGUI title;

	public Button inputConnectorButton;

	public Button outputConnectorButton;

	public RectTransform InputOutputGroup;

	public Transform ParameterListTransform;

	public Button toggleParametersButton;

	public RectTransform dockedNodesParentTransform;

	[SerializeField]
	public MEGUINode dockedParentNode;

	public MEGUINodeBodyParameter nodeBodyParameterPrefab;

	[SerializeField]
	private List<MEGUINodeBodyParameter> parameterList;

	private bool parameterListIsMinimised;

	public int numberOfParametersToDisplayWhenMinimised;

	private MissionEditorLogic meLogic;

	protected UIDragPanel dragPanel;

	private Vector3 nodeDragOffset;

	public bool isSelected;

	public Image headerImage;

	[SerializeField]
	private Image validityIndicator;

	[SerializeField]
	private TooltipController_TitleAndText validityTooltip;

	[SerializeField]
	private GameObject DockedJoiner;

	private Image DockedJoinerImage;

	[SerializeField]
	private GameObject DockableIndicator;

	private Image DockableIndicatorImage;

	[SerializeField]
	private float DockableIndicatorOpacity;

	[SerializeField]
	internal ColoredConnectorOptions ColoredConnectorPins;

	[SerializeField]
	private Image highlighter;

	[SerializeField]
	private Image inputHolderHighLighter;

	[SerializeField]
	private Image outputHolderHighLighter;

	[SerializeField]
	private RawImage nodeImage;

	[SerializeField]
	private Image selectedHighlighter;

	[SerializeField]
	private Color selectedHighlighterColor;

	private bool isLogicNode;

	private bool isVesselNode;

	private bool isLaunchSiteNode;

	private MEGUINode dockCandidate;

	private bool nodeParametersDirty;

	private UIStateImage minMaxImage;

	private UIStateImage inputConnectorImage;

	private UIStateImage outputConnectorImage;

	[SerializeField]
	private MEBasicNode basicNode;

	[SerializeField]
	private MEGUINodeIcon nodeIcon;

	[SerializeField]
	public MENode Node
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public RectTransform rectTransform
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public List<MEGUIConnector> InputConnectors
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public List<MEGUIConnector> OutputConnectors
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public MEGUINode DockCandidate
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public MEBasicNode BasicNode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public event Action<MEGUINode> DockStatusChange
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		add
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		remove
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUINode()
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
	protected virtual void Setup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Destroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CleanUp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetNode(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetEndNode(bool setNode, bool initialize = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetTitleText(string title)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetNodeColor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetNodeColor(Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ShowInputConnectorButton(bool show)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ShowOutputConnectorButton(bool show)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void InputButtonDragged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void InputButtonDropped()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void OutputButtonDragged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void OutputButtonDropped()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void CancelDrag()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveAllConnections()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RemoveInputConnections()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RemoveOutputConnections()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasInputConnections()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasOutputConnections()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool DoesConnectionExist(MEGUINode otherNode, MENodeConnectionType otherNodeConnectionType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddNewLine(MEGUIConnector line, MENodeConnectionType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveLine(MEGUIConnector line, MENodeConnectionType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector2 GetInputButtonPosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector2 GetOutputButtonPosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateConnectors()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CDelayedUpdateConnectors_003Ed__93))]
	protected IEnumerator DelayedUpdateConnectors()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateInputConnectorImage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateOutputConnectorImage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupDockingData()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanDock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClearDockableIndicator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetDockableIndicator(MEGUINode draggedNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool WillDock(MEGUINode dragging, MEGUINode target)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CheckNodeDockingPosition(Vector3 nodePositionCheckOverride)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CheckDockStatus()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool WillUndock()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DockNode(MEGUINode parentNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DockNode(MEGUINode parentNode, int dockIndex, bool fromHistory)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateDockedIndex()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UndockNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetDepthOfDockedNode(MEGUINode dockedNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleParametersButtonClicked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DisplayNodeBodyParameters()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void DisplayModuleSection(IMENodeDisplay module, string parameterName, int parameterPosition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void DisplayParameter(string displayString, int parameterPosition, bool allowEmpty)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void DisplayParameter(string displayString, int parameterPosition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CleanNodeBodyParameters()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PlaceNodeAtCanvas(Vector3 position)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PlaceNodeAtCanvas(Vector3 position, MEGUINodeIcon baseNodeIcon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PushUndoActionOnPasteNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Select(bool deselectOtherNodes = false, bool bypassNodeSelection = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Deselect()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerClick(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnNodeBeginDrag(PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void OnDragSetDockingIndicators()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnNodeDrag(PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void NodeGroupDrag()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnNodeEndDrag(PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void OnDragEndClearDockingIndicators()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearValidityIndicators()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetValidityIndicators(MissionValidationTestResult result)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetHighlighter(Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearHighlighter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleHighlighter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleHighlighter(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleInputHolderHighlighter(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleOutputHolderHighlighter(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Color GetHighlighterColor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleSelectionHighlighter(bool show)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnHistoryDock(ConfigNode data, HistoryType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnHistoryUnDock(ConfigNode data, HistoryType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnHistoryMove(ConfigNode data, HistoryType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnHistoryCreate(ConfigNode data, HistoryType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnHistoryDestroy(ConfigNode data, HistoryType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode GetState()
	{
		throw null;
	}
}

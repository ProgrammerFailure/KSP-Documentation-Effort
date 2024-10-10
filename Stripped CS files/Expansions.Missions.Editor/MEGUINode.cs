using System;
using System.Collections;
using System.Collections.Generic;
using ns12;
using ns2;
using ns5;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[Serializable]
public class MEGUINode : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IMEHistoryTarget
{
	[Serializable]
	public enum ColoredConnectorOptions
	{
		None,
		InputOnly,
		OutputOnly,
		Both
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
	public List<MEGUINodeBodyParameter> parameterList;

	public bool parameterListIsMinimised;

	public int numberOfParametersToDisplayWhenMinimised = 1;

	public MissionEditorLogic meLogic;

	public UIDragPanel dragPanel;

	public Vector3 nodeDragOffset;

	public bool isSelected;

	public Image headerImage;

	[SerializeField]
	public Image validityIndicator;

	[SerializeField]
	public TooltipController_TitleAndText validityTooltip;

	[SerializeField]
	public GameObject DockedJoiner;

	public Image DockedJoinerImage;

	[SerializeField]
	public GameObject DockableIndicator;

	public Image DockableIndicatorImage;

	[SerializeField]
	public float DockableIndicatorOpacity = 0.5f;

	[SerializeField]
	public ColoredConnectorOptions ColoredConnectorPins;

	[SerializeField]
	public Image highlighter;

	[SerializeField]
	public Image inputHolderHighLighter;

	[SerializeField]
	public Image outputHolderHighLighter;

	[SerializeField]
	public RawImage nodeImage;

	[SerializeField]
	public Image selectedHighlighter;

	[SerializeField]
	public Color selectedHighlighterColor = new Color(0.21f, 0.58f, 0.68f, 1f);

	public bool isLogicNode;

	public bool isVesselNode;

	public bool isLaunchSiteNode;

	public MEGUINode dockCandidate;

	public bool nodeParametersDirty;

	public UIStateImage minMaxImage;

	public UIStateImage inputConnectorImage;

	public UIStateImage outputConnectorImage;

	[SerializeField]
	public MEBasicNode basicNode;

	[SerializeField]
	public MEGUINodeIcon nodeIcon;

	[SerializeField]
	public MENode Node { get; set; }

	public RectTransform rectTransform { get; set; }

	public List<MEGUIConnector> InputConnectors { get; set; }

	public List<MEGUIConnector> OutputConnectors { get; set; }

	public MEGUINode DockCandidate => dockCandidate;

	public MEBasicNode BasicNode => basicNode;

	public event Action<MEGUINode> DockStatusChange = delegate
	{
	};

	public void Awake()
	{
		meLogic = MissionEditorLogic.Instance;
		rectTransform = base.transform as RectTransform;
		OutputConnectors = new List<MEGUIConnector>();
		InputConnectors = new List<MEGUIConnector>();
		dragPanel = GetComponent<UIDragPanel>();
		parameterList = new List<MEGUINodeBodyParameter>();
		minMaxImage = toggleParametersButton.GetComponent<UIStateImage>();
		inputConnectorImage = inputConnectorButton.GetComponent<UIStateImage>();
		outputConnectorImage = outputConnectorButton.GetComponent<UIStateImage>();
		DockedJoinerImage = DockedJoiner.GetComponentInChildren<Image>();
		DockableIndicatorImage = DockableIndicator.GetComponentInChildren<Image>();
		DockableIndicatorImage.color = headerImage.color.smethod_0(DockableIndicatorOpacity);
		if (selectedHighlighter != null)
		{
			selectedHighlighter.color = selectedHighlighterColor;
			selectedHighlighter.gameObject.SetActive(value: false);
		}
	}

	public void Start()
	{
		Setup();
	}

	public virtual void Setup()
	{
		title.text = ((Node != null) ? Node.Title : base.name);
		dragPanel.onBeginDrag.AddListener(OnNodeBeginDrag);
		dragPanel.onDrag.AddListener(OnNodeDrag);
		dragPanel.onEndDrag.AddListener(OnNodeEndDrag);
		toggleParametersButton.onClick.AddListener(ToggleParametersButtonClicked);
		ColoredConnectorPins = meLogic.ColoredConnectorPins;
		SetNodeColor();
		SetupDockingData();
		if (nodeIcon == null)
		{
			nodeIcon = MENodeCategorizer.Instance.AddCanvasNodeButton(basicNode, this);
		}
		else
		{
			nodeIcon.gameObject.SetActive(value: true);
		}
		base.gameObject.SetActive(value: true);
		UpdateDockedIndex();
	}

	public void Destroy()
	{
		MissionEditorHistory.PushUndoAction(this, OnHistoryDestroy);
		meLogic.RemoveNode(this);
		for (int i = 0; i < MissionEditorLogic.Instance.selectedNodes.Count; i++)
		{
			if (MissionEditorLogic.Instance.selectedNodes.Contains(this))
			{
				MissionEditorLogic.Instance.selectedNodes.Remove(this);
			}
		}
		dragPanel.onBeginDrag.RemoveListener(OnNodeBeginDrag);
		dragPanel.onDrag.RemoveListener(OnNodeDrag);
		dragPanel.onEndDrag.RemoveListener(OnNodeEndDrag);
		toggleParametersButton.onClick.RemoveListener(ToggleParametersButtonClicked);
		RemoveAllConnections();
		int count = Node.dockedNodes.Count;
		while (count-- > 0)
		{
			Node.dockedNodes[count].guiNode.UndockNode();
		}
		if (nodeIcon != null)
		{
			MENodeCategorizer.Instance.RemoveCanvasNodeButton(nodeIcon);
		}
		base.gameObject.SetActive(value: false);
	}

	public void CleanUp()
	{
		CleanNodeBodyParameters();
		InputConnectors.Clear();
		OutputConnectors.Clear();
		if (nodeIcon != null)
		{
			MENodeCategorizer.Instance.RemoveCanvasNodeButton(nodeIcon);
			nodeIcon.gameObject.DestroyGameObject();
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void SetNode(MENode node)
	{
		if (node.id == Guid.Empty)
		{
			return;
		}
		Node = node;
		Node.guiNode = this;
		int count = MissionEditorLogic.Instance.basicNodes.Count;
		for (int i = 0; i < count; i++)
		{
			if (node.basicNodeSource == MissionEditorLogic.Instance.basicNodes[i].name)
			{
				basicNode = MissionEditorLogic.Instance.basicNodes[i];
				break;
			}
		}
		base.gameObject.name = "MEGUINode " + Localizer.Format(node.Title);
		title.text = node.Title;
		if (nodeIcon != null)
		{
			nodeIcon.nodeText.text = node.Title;
		}
		else if (MENodeCategorizer.Instance != null && basicNode != null)
		{
			nodeIcon = MENodeCategorizer.Instance.AddCanvasNodeButton(basicNode, this);
		}
		headerImage.color = node.nodeColor;
		if (!Node.isStartNode && !node.IsLaunchPadNode)
		{
			UpdateInputConnectorImage();
		}
		else
		{
			ShowInputConnectorButton(show: false);
		}
		if (node.IsLaunchPadNode)
		{
			ShowOutputConnectorButton(show: false);
		}
		else
		{
			UpdateOutputConnectorImage();
		}
		SetEndNode(Node.isEndNode, initialize: true);
		if (node.mission != null)
		{
			DisplayNodeBodyParameters();
		}
		if (node.isStartNode)
		{
			node.SetCatchAllNode(newCatchAll: false);
		}
		if (Node.nodeBodyParameters.Count <= 1)
		{
			toggleParametersButton.gameObject.SetActive(value: false);
		}
		if (!(nodeImage != null))
		{
			return;
		}
		if (!string.IsNullOrEmpty(node.basicNodeIconURL) && MENodeCategorizer.Instance != null && MENodeCategorizer.Instance.nodeIconLoader != null)
		{
			Icon icon = MENodeCategorizer.Instance.nodeIconLoader.GetIcon(node.basicNodeIconURL);
			if (icon != null)
			{
				nodeImage.texture = icon.iconNormal;
			}
		}
		else
		{
			nodeImage.gameObject.SetActive(value: false);
		}
	}

	public void SetEndNode(bool setNode, bool initialize = false)
	{
		if (isSelected || initialize)
		{
			if (setNode)
			{
				RemoveOutputConnections();
			}
			if (!Node.IsLaunchPadNode)
			{
				ShowOutputConnectorButton(!setNode);
			}
		}
	}

	public void SetTitleText(string title)
	{
		this.title.text = title;
		if (nodeIcon != null)
		{
			nodeIcon.nodeText.text = title;
		}
		else
		{
			nodeIcon = MENodeCategorizer.Instance.AddCanvasNodeButton(basicNode, this);
		}
	}

	public void SetNodeColor()
	{
		SetNodeColor(headerImage.color);
	}

	public void SetNodeColor(Color color)
	{
		headerImage.color = color;
		switch (ColoredConnectorPins)
		{
		case ColoredConnectorOptions.InputOnly:
			inputConnectorImage.image.color = color;
			break;
		case ColoredConnectorOptions.OutputOnly:
			outputConnectorImage.image.color = color;
			break;
		case ColoredConnectorOptions.Both:
			inputConnectorImage.image.color = color;
			outputConnectorImage.image.color = color;
			break;
		}
		if (DockableIndicatorImage != null)
		{
			DockableIndicatorImage.color = color.smethod_0(DockableIndicatorOpacity);
		}
		for (int i = 0; i < Node.dockedNodes.Count; i++)
		{
			MENode mENode = Node.dockedNodes[i];
			if (mENode.guiNode != null && mENode.guiNode.DockedJoinerImage != null)
			{
				mENode.guiNode.DockedJoinerImage.color = color;
			}
		}
	}

	public void ShowInputConnectorButton(bool show)
	{
		inputConnectorButton.gameObject.SetActive(show);
	}

	public void ShowOutputConnectorButton(bool show)
	{
		outputConnectorButton.gameObject.SetActive(show);
	}

	public void InputButtonDragged()
	{
		if (!Node.isStartNode)
		{
			meLogic.ConnectorButtonDragged(this, MENodeConnectionType.Input);
		}
	}

	public void InputButtonDropped()
	{
		if (!Node.isEndNode)
		{
			meLogic.ConnectorButtonDropped(this, MENodeConnectionType.Output);
		}
	}

	public void OutputButtonDragged()
	{
		if (!Node.isEndNode)
		{
			meLogic.ConnectorButtonDragged(this, MENodeConnectionType.Output);
		}
	}

	public void OutputButtonDropped()
	{
		if (!Node.isStartNode)
		{
			meLogic.ConnectorButtonDropped(this, MENodeConnectionType.Input);
		}
	}

	public void CancelDrag()
	{
		meLogic.DestroyCurrentConnector();
	}

	public void RemoveAllConnections()
	{
		RemoveInputConnections();
		RemoveOutputConnections();
	}

	public void RemoveInputConnections()
	{
		int count = InputConnectors.Count;
		while (count-- > 0)
		{
			InputConnectors[count].Destroy();
		}
	}

	public void RemoveOutputConnections()
	{
		int count = OutputConnectors.Count;
		while (count-- > 0)
		{
			OutputConnectors[count].Destroy();
		}
	}

	public bool HasInputConnections()
	{
		return InputConnectors.Count > 0;
	}

	public bool HasOutputConnections()
	{
		return OutputConnectors.Count > 0;
	}

	public bool DoesConnectionExist(MEGUINode otherNode, MENodeConnectionType otherNodeConnectionType)
	{
		if (otherNodeConnectionType == MENodeConnectionType.Output)
		{
			for (int i = 0; i < InputConnectors.Count; i++)
			{
				if (InputConnectors[i].fromNode == otherNode)
				{
					return true;
				}
			}
		}
		if (otherNodeConnectionType == MENodeConnectionType.Input)
		{
			for (int j = 0; j < OutputConnectors.Count; j++)
			{
				if (OutputConnectors[j].toNode == otherNode)
				{
					return true;
				}
			}
		}
		return false;
	}

	public void AddNewLine(MEGUIConnector line, MENodeConnectionType type)
	{
		switch (type)
		{
		case MENodeConnectionType.Input:
			InputConnectors.AddUnique(line);
			UpdateInputConnectorImage();
			break;
		case MENodeConnectionType.Output:
			OutputConnectors.AddUnique(line);
			MissionEditorLogic.Instance.actionPane.RemoveParameterGroup("MissionEnd");
			MissionEditorLogic.Instance.actionPane.SAPRefreshNodeParameters();
			UpdateOutputConnectorImage();
			break;
		}
		line.UpdateLine();
	}

	public void RemoveLine(MEGUIConnector line, MENodeConnectionType type)
	{
		switch (type)
		{
		case MENodeConnectionType.Input:
			InputConnectors.Remove(line);
			UpdateInputConnectorImage();
			break;
		case MENodeConnectionType.Output:
			OutputConnectors.Remove(line);
			UpdateOutputConnectorImage();
			break;
		}
	}

	public Vector2 GetInputButtonPosition()
	{
		if (dockCandidate == null)
		{
			Vector2 anchoredPosition = rectTransform.anchoredPosition;
			anchoredPosition.x -= rectTransform.rect.width / 2f;
			anchoredPosition.y += InputOutputGroup.anchoredPosition.y - InputOutputGroup.rect.height / 2f;
			return anchoredPosition;
		}
		Vector2 anchoredPosition2 = dockCandidate.rectTransform.anchoredPosition;
		anchoredPosition2.x -= dockCandidate.rectTransform.rect.width / 2f;
		anchoredPosition2.y -= dockCandidate.rectTransform.rect.height - dockCandidate.dockedNodesParentTransform.rect.height;
		anchoredPosition2.y += rectTransform.anchoredPosition.y + InputOutputGroup.anchoredPosition.y - InputOutputGroup.rect.height / 2f;
		return anchoredPosition2;
	}

	public Vector2 GetOutputButtonPosition()
	{
		if (dockCandidate == null)
		{
			Vector2 anchoredPosition = rectTransform.anchoredPosition;
			anchoredPosition.x += rectTransform.rect.width / 2f;
			anchoredPosition.y += InputOutputGroup.anchoredPosition.y - InputOutputGroup.rect.height / 2f;
			return anchoredPosition;
		}
		Vector2 anchoredPosition2 = dockCandidate.rectTransform.anchoredPosition;
		anchoredPosition2.x += dockCandidate.rectTransform.rect.width / 2f;
		anchoredPosition2.y -= dockCandidate.rectTransform.rect.height - dockCandidate.dockedNodesParentTransform.rect.height;
		anchoredPosition2.y += rectTransform.anchoredPosition.y + InputOutputGroup.anchoredPosition.y - InputOutputGroup.rect.height / 2f;
		return anchoredPosition2;
	}

	public void UpdateConnectors()
	{
		int count = InputConnectors.Count;
		while (count-- > 0)
		{
			if (base.gameObject.activeInHierarchy)
			{
				StartCoroutine(InputConnectors[count].DelayedUpdateLine());
			}
			else
			{
				InputConnectors[count].UpdateLine();
			}
		}
		int count2 = OutputConnectors.Count;
		while (count2-- > 0)
		{
			if (base.gameObject.activeInHierarchy)
			{
				StartCoroutine(OutputConnectors[count2].DelayedUpdateLine());
			}
			else
			{
				OutputConnectors[count2].UpdateLine();
			}
		}
		for (int i = 0; i < Node.dockedNodes.Count; i++)
		{
			if (Node.dockedNodes[i].guiNode != null)
			{
				Node.dockedNodes[i].guiNode.UpdateConnectors();
			}
		}
	}

	public IEnumerator DelayedUpdateConnectors()
	{
		yield return null;
		UpdateConnectors();
	}

	public void UpdateInputConnectorImage()
	{
		if (inputConnectorImage != null)
		{
			if (Node.IsOrphanNode)
			{
				inputConnectorImage.SetState("Orphan");
			}
			else if (Node.fromNodes.Count > 0)
			{
				inputConnectorImage.SetState("Connected");
			}
			else
			{
				inputConnectorImage.SetState("Disconnected");
			}
		}
	}

	public void UpdateOutputConnectorImage()
	{
		if (outputConnectorImage != null)
		{
			if (Node.HasConnectedOutput)
			{
				outputConnectorImage.SetState("Connected");
			}
			else
			{
				outputConnectorImage.SetState("Disconnected");
			}
		}
	}

	public void Update()
	{
		if (nodeParametersDirty && parameterList.Count > 0 && (parameterList[0].transform as RectTransform).rect.height > 0f)
		{
			InputOutputGroup.sizeDelta = new Vector2(240f, Mathf.Max((parameterList[0].transform as RectTransform).rect.height, 34f));
			nodeParametersDirty = false;
		}
	}

	public void SetupDockingData()
	{
		isLogicNode = Node != null && Node.IsLogicNode;
		isVesselNode = Node != null && Node.IsVesselNode;
		isLaunchSiteNode = Node != null && Node.IsLaunchPadNode;
	}

	public bool CanDock()
	{
		if (!isLogicNode && !isVesselNode)
		{
			return isLaunchSiteNode;
		}
		return true;
	}

	public void ClearDockableIndicator()
	{
		DockableIndicator.SetActive(value: false);
	}

	public void SetDockableIndicator(MEGUINode draggedNode)
	{
		DockableIndicator.SetActive(WillDock(draggedNode, this));
	}

	public static bool WillDock(MEGUINode dragging, MEGUINode target)
	{
		if (MissionEditorLogic.Instance.EditorMission.IsTutorialMission)
		{
			return false;
		}
		if (!(dragging == target) && !target.Node.IsDocked)
		{
			if (dragging.isLogicNode && !target.Node.isStartNode && !target.Node.IsScoringNode)
			{
				return true;
			}
			if (dragging.isVesselNode && target.Node.isStartNode)
			{
				return true;
			}
			if (dragging.isLaunchSiteNode && target.Node.isStartNode)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public void CheckNodeDockingPosition(Vector3 nodePositionCheckOverride)
	{
		if (Node.dockedNodes.Count != 0 || meLogic.selectedNodes.Count >= 2 || !CanDock())
		{
			return;
		}
		MEGUINode nodeToDockTo = null;
		if (MissionEditorLogic.Instance.IsNodeDockable(this, ref nodeToDockTo, nodePositionCheckOverride))
		{
			if (WillDock(this, nodeToDockTo))
			{
				dockCandidate = nodeToDockTo;
				base.transform.SetParent(nodeToDockTo.dockedNodesParentTransform);
			}
		}
		else if (dockCandidate != null)
		{
			base.transform.SetParent(MissionEditorLogic.Instance.NodeCanvas.NodeRoot.transform);
			dockCandidate.DockableIndicator.SetActive(value: true);
			DockedJoiner.SetActive(value: false);
			dockCandidate = null;
		}
		if (dockCandidate != null)
		{
			DockedJoinerImage.color = dockCandidate.headerImage.color;
			dockCandidate.DockableIndicator.SetActive(value: false);
			DockedJoiner.SetActive(value: true);
			LayoutRebuilder.ForceRebuildLayoutImmediate(dockCandidate.GetComponent<RectTransform>());
		}
	}

	public void CheckDockStatus()
	{
		if (dockCandidate != null)
		{
			DockNode(dockCandidate);
		}
		else if (WillUndock())
		{
			UndockNode();
		}
	}

	public bool WillUndock()
	{
		if (MissionEditorLogic.Instance.EditorMission.IsTutorialMission)
		{
			return false;
		}
		return dockedParentNode != null;
	}

	public void DockNode(MEGUINode parentNode)
	{
		DockNode(parentNode, 0, fromHistory: false);
	}

	public void DockNode(MEGUINode parentNode, int dockIndex, bool fromHistory)
	{
		MissionEditorHistory.PushUndoAction(this, OnHistoryDock);
		base.transform.SetParent(parentNode.dockedNodesParentTransform);
		if (Node.IsOrphanNode)
		{
			Node.mission.UpdateOrphanNodeState(Node, makeOrphan: false);
		}
		if (dockCandidate == null)
		{
			dockCandidate = parentNode;
		}
		dockedParentNode = parentNode;
		if (fromHistory)
		{
			base.transform.SetSiblingIndex(dockIndex);
			dockedParentNode.Node.dockedNodes.Insert(dockIndex, Node);
		}
		else
		{
			dockedParentNode.Node.dockedNodes.AddUnique(Node);
		}
		Node.dockParentNode = dockedParentNode.Node;
		RemoveInputConnections();
		if (Node.IsDockedToStartNode || Node.IsScoringNode)
		{
			RemoveOutputConnections();
			ShowOutputConnectorButton(show: false);
		}
		ShowInputConnectorButton(show: false);
		DockedJoiner.SetActive(value: true);
		DockedJoinerImage.color = parentNode.headerImage.color;
		MissionEditorValidator.RunValidationOnParamChange();
		this.DockStatusChange(parentNode);
		if (MissionEditorLogic.Instance.CurrentSelectedNode == this)
		{
			MissionEditorLogic.Instance.actionPane.SAPRefreshNodeParameters();
		}
		if (parentNode != null)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(parentNode.GetComponent<RectTransform>());
		}
		if (fromHistory)
		{
			for (int i = 0; i < dockedParentNode.Node.dockedNodes.Count; i++)
			{
				dockedParentNode.Node.dockedNodes[i].guiNode.UpdateDockedIndex();
			}
		}
		else
		{
			UpdateDockedIndex();
		}
	}

	public void UpdateDockedIndex()
	{
		if (!(dockedParentNode != null))
		{
			return;
		}
		int num = 0;
		while (true)
		{
			if (num < dockedParentNode.Node.dockedNodes.Count)
			{
				if (dockedParentNode.Node.dockedNodes[num] == Node)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		Node.dockedIndex = num;
	}

	public void UndockNode()
	{
		MissionEditorHistory.PushUndoAction(this, OnHistoryUnDock);
		base.transform.SetParent(MissionEditorLogic.Instance.NodeCanvas.NodeRoot.transform);
		if (dockedParentNode != null)
		{
			if (dockedParentNode.gameObject.activeInHierarchy)
			{
				dockedParentNode.StartCoroutine(dockedParentNode.DelayedUpdateConnectors());
			}
			dockedParentNode.Node.dockedNodes.Remove(Node);
			if (dockedParentNode.Node.dockedNodes.Count > 0)
			{
				for (int i = 0; i < dockedParentNode.Node.dockedNodes.Count; i++)
				{
					dockedParentNode.Node.dockedNodes[i].guiNode.UpdateDockedIndex();
				}
			}
		}
		dockedParentNode = null;
		Node.dockParentNode = null;
		Node.dockedIndex = 0;
		dockCandidate = null;
		rectTransform.anchoredPosition = Node.editorPosition;
		if (!Node.isEndNode && !Node.IsLaunchPadNode)
		{
			ShowOutputConnectorButton(show: true);
		}
		if (!Node.IsLaunchPadNode)
		{
			ShowInputConnectorButton(show: true);
		}
		DockedJoiner.SetActive(value: false);
		UpdateConnectors();
		this.DockStatusChange(null);
		if (Node.IsOrphanNode)
		{
			Node.mission.UpdateOrphanNodeState(Node, makeOrphan: true);
		}
		MissionEditorValidator.RunValidationOnParamChange();
		if (MissionEditorLogic.Instance.CurrentSelectedNode == this)
		{
			MissionEditorLogic.Instance.actionPane.SAPRefreshNodeParameters();
		}
	}

	public float GetDepthOfDockedNode(MEGUINode dockedNode)
	{
		float num = 0f;
		int num2 = 0;
		while (true)
		{
			if (num2 < Node.dockedNodes.Count)
			{
				if (dockedNode == Node.dockedNodes[num2].guiNode)
				{
					break;
				}
				num += Node.dockedNodes[num2].guiNode.rectTransform.rect.height;
				num2++;
				continue;
			}
			return num;
		}
		return num;
	}

	public void ToggleParametersButtonClicked()
	{
		parameterListIsMinimised = !parameterListIsMinimised;
		if (parameterListIsMinimised)
		{
			minMaxImage.SetState("Collapsed");
		}
		else
		{
			minMaxImage.SetState("Expanded");
		}
		DisplayNodeBodyParameters();
	}

	public void DisplayNodeBodyParameters()
	{
		CleanNodeBodyParameters();
		if (Node == null || (parameterListIsMinimised && numberOfParametersToDisplayWhenMinimised <= 0))
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		while (true)
		{
			if (num2 < Node.nodeBodyParameters.Count)
			{
				int i = 0;
				for (int count = Node.testGroups.Count; i < count; i++)
				{
					int j = 0;
					for (int count2 = Node.testGroups[i].testModules.Count; j < count2; j++)
					{
						if (Node.testGroups[i].testModules[j].GetName() == Node.nodeBodyParameters[num2].module)
						{
							DisplayModuleSection(Node.testGroups[i].testModules[j], Node.nodeBodyParameters[num2].parameter, num);
							num++;
							if (parameterListIsMinimised && num == numberOfParametersToDisplayWhenMinimised)
							{
								UpdateConnectors();
								return;
							}
						}
						List<IMENodeDisplay> internalParametersToDisplay = Node.testGroups[i].testModules[j].GetInternalParametersToDisplay();
						int k = 0;
						for (int count3 = internalParametersToDisplay.Count; k < count3; k++)
						{
							if (internalParametersToDisplay[k].GetName() == Node.nodeBodyParameters[num2].module)
							{
								DisplayModuleSection(internalParametersToDisplay[k], Node.nodeBodyParameters[num2].parameter, num);
								num++;
								if (parameterListIsMinimised && num == numberOfParametersToDisplayWhenMinimised)
								{
									UpdateConnectors();
									return;
								}
							}
						}
					}
				}
				int l = 0;
				for (int count4 = Node.actionModules.Count; l < count4; l++)
				{
					if (Node.actionModules[l].GetName() == Node.nodeBodyParameters[num2].module)
					{
						DisplayModuleSection(Node.actionModules[l], Node.nodeBodyParameters[num2].parameter, num);
						num++;
						if (parameterListIsMinimised && num == numberOfParametersToDisplayWhenMinimised)
						{
							UpdateConnectors();
							return;
						}
					}
					List<IMENodeDisplay> internalParametersToDisplay2 = Node.actionModules[l].GetInternalParametersToDisplay();
					int m = 0;
					for (int count5 = internalParametersToDisplay2.Count; m < count5; m++)
					{
						if (internalParametersToDisplay2[m].GetName() == Node.nodeBodyParameters[num2].module)
						{
							DisplayModuleSection(internalParametersToDisplay2[m], Node.nodeBodyParameters[num2].parameter, num);
							num++;
							if (parameterListIsMinimised && num == numberOfParametersToDisplayWhenMinimised)
							{
								UpdateConnectors();
								return;
							}
						}
					}
				}
				if (Node.isStartNode)
				{
					if (Node.mission.situation.GetName() == Node.nodeBodyParameters[num2].module)
					{
						DisplayModuleSection(Node.mission.situation, Node.nodeBodyParameters[num2].parameter, num);
						num++;
						if (parameterListIsMinimised && num == numberOfParametersToDisplayWhenMinimised)
						{
							break;
						}
					}
					List<IMENodeDisplay> internalParametersToDisplay3 = Node.mission.situation.GetInternalParametersToDisplay();
					int n = 0;
					for (int count6 = internalParametersToDisplay3.Count; n < count6; n++)
					{
						if (internalParametersToDisplay3[n].GetName() == Node.nodeBodyParameters[num2].module)
						{
							DisplayModuleSection(internalParametersToDisplay3[n], Node.nodeBodyParameters[num2].parameter, num);
							num++;
							if (parameterListIsMinimised && num == numberOfParametersToDisplayWhenMinimised)
							{
								UpdateConnectors();
								return;
							}
						}
					}
				}
				num2++;
				continue;
			}
			if (num < 1)
			{
				DisplayParameter("", 0, allowEmpty: true);
			}
			nodeParametersDirty = true;
			if (Node.nodeBodyParameters.Count <= 1)
			{
				toggleParametersButton.gameObject.SetActive(value: false);
			}
			else
			{
				toggleParametersButton.gameObject.SetActive(value: true);
			}
			UpdateConnectors();
			return;
		}
		UpdateConnectors();
	}

	public void DisplayModuleSection(IMENodeDisplay module, string parameterName, int parameterPosition)
	{
		BaseAPFieldList baseAPFieldList = new BaseAPFieldList(module);
		int i = 0;
		for (int count = baseAPFieldList.Count; i < count; i++)
		{
			if (parameterName == baseAPFieldList[i].name)
			{
				DisplayParameter(module.GetNodeBodyParameterString(baseAPFieldList[i]), parameterPosition);
			}
		}
	}

	public void DisplayParameter(string displayString, int parameterPosition, bool allowEmpty)
	{
		if (allowEmpty || !string.IsNullOrEmpty(displayString))
		{
			MEGUINodeBodyParameter mEGUINodeBodyParameter = UnityEngine.Object.Instantiate(nodeBodyParameterPrefab, ParameterListTransform);
			mEGUINodeBodyParameter.Setup(this, displayString, parameterPosition);
			mEGUINodeBodyParameter.transform.localScale = rectTransform.localScale;
			parameterList.Add(mEGUINodeBodyParameter);
		}
	}

	public void DisplayParameter(string displayString, int parameterPosition)
	{
		DisplayParameter(displayString, parameterPosition, allowEmpty: false);
	}

	public void CleanNodeBodyParameters()
	{
		int count = parameterList.Count;
		while (count-- > 0)
		{
			if (parameterList[count].gameObject != null)
			{
				UnityEngine.Object.Destroy(parameterList[count].gameObject);
			}
		}
		parameterList.Clear();
	}

	public void PlaceNodeAtCanvas(Vector3 position)
	{
		PlaceNodeAtCanvas(position, null);
	}

	public void PlaceNodeAtCanvas(Vector3 position, MEGUINodeIcon baseNodeIcon)
	{
		MissionEditorHistory.PushUndoAction(this, OnHistoryCreate, true);
		rectTransform.localPosition = position;
		if (Node != null)
		{
			Node.editorPosition = rectTransform.anchoredPosition;
			Select(deselectOtherNodes: true);
			MENodeCanvas.Instance.CalculateBorders();
		}
		if (baseNodeIcon != null)
		{
			basicNode = baseNodeIcon.basicNode;
		}
	}

	public void PushUndoActionOnPasteNode()
	{
		MissionEditorHistory.PushUndoAction(this, OnHistoryCreate, true);
	}

	public void Select(bool deselectOtherNodes = false, bool bypassNodeSelection = false)
	{
		if (deselectOtherNodes)
		{
			meLogic.ClearSelectedNodesList();
		}
		meLogic.ClearConnectorGroupSelection();
		isSelected = true;
		if ((meLogic.selectedNodes.Count < 1 || deselectOtherNodes) && !bypassNodeSelection)
		{
			meLogic.NodeSelectionChange(this);
		}
		meLogic.AddNodeToSelectedList(this);
		ToggleSelectionHighlighter(show: true);
	}

	public void Deselect()
	{
		isSelected = false;
		meLogic.RemoveNodeFromSelectedList(this);
		ToggleSelectionHighlighter(show: false);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!eventData.dragging)
		{
			if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
			{
				Select(deselectOtherNodes: true);
			}
			else if (isSelected)
			{
				Deselect();
			}
			else
			{
				Select();
			}
		}
	}

	public void OnNodeBeginDrag(PointerEventData data)
	{
		if (!isSelected)
		{
			Select(!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift));
		}
		OnDragSetDockingIndicators();
		nodeDragOffset = base.transform.position - meLogic.GetGridMousePosition();
		if (meLogic.selectedNodes.Count > 1)
		{
			meLogic.lastFramePos.x = rectTransform.anchoredPosition.x;
			meLogic.lastFramePos.y = rectTransform.anchoredPosition.y;
		}
	}

	public void OnDragSetDockingIndicators()
	{
		if (Node.dockedNodes.Count == 0 && meLogic.selectedNodes.Count < 2)
		{
			for (int i = 0; i < MissionEditorLogic.Instance.editorNodeList.Count; i++)
			{
				MissionEditorLogic.Instance.editorNodeList[i].SetDockableIndicator(this);
			}
		}
	}

	public void OnNodeDrag(PointerEventData data)
	{
		Vector3 vector = meLogic.GetGridMousePosition() + nodeDragOffset;
		base.transform.position = MENodeCanvas.CheckSnap(vector);
		CheckNodeDockingPosition(vector);
		UpdateConnectors();
		if (meLogic.selectedNodes.Count > 1)
		{
			NodeGroupDrag();
		}
		MENodeCanvas.Instance.TryCanvasMovement(data, this);
		MENodeCanvas.Instance.ToggleScrollLock(newValue: true);
	}

	public void NodeGroupDrag()
	{
		meLogic.nodeDragPos.x = meLogic.lastFramePos.x - rectTransform.anchoredPosition.x;
		meLogic.nodeDragPos.y = meLogic.lastFramePos.y - rectTransform.anchoredPosition.y;
		meLogic.UpdateSelectedNodesPosition(this, meLogic.nodeDragPos);
		meLogic.lastFramePos.x = rectTransform.anchoredPosition.x;
		meLogic.lastFramePos.y = rectTransform.anchoredPosition.y;
	}

	public void OnNodeEndDrag(PointerEventData data)
	{
		meLogic.lastFramePos = Vector2.zero;
		UpdateConnectors();
		MissionEditorHistory.PushUndoAction(this, OnHistoryMove);
		if (Node != null)
		{
			Node.editorPosition = rectTransform.anchoredPosition;
		}
		for (int i = 0; i < meLogic.selectedNodes.Count; i++)
		{
			if (meLogic.selectedNodes[i] != this)
			{
				meLogic.selectedNodes[i].Node.editorPosition = meLogic.selectedNodes[i].rectTransform.anchoredPosition;
			}
		}
		CheckDockStatus();
		OnDragEndClearDockingIndicators();
		if (dockedParentNode != null)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(dockedParentNode.GetComponent<RectTransform>());
		}
		MENodeCanvas.Instance.StopCanvasMovement();
		MENodeCanvas.Instance.ToggleScrollLock(newValue: false);
		MENodeCanvas.Instance.CalculateBorders();
	}

	public void OnDragEndClearDockingIndicators()
	{
		for (int i = 0; i < MissionEditorLogic.Instance.editorNodeList.Count; i++)
		{
			MissionEditorLogic.Instance.editorNodeList[i].ClearDockableIndicator();
		}
	}

	public void ClearValidityIndicators()
	{
		if (validityIndicator != null)
		{
			validityIndicator.gameObject.SetActive(value: false);
		}
		if (validityTooltip != null)
		{
			validityTooltip.gameObject.SetActive(value: false);
			validityTooltip.textString = "";
		}
	}

	public void SetValidityIndicators(MissionValidationTestResult result)
	{
		if (validityIndicator != null)
		{
			validityIndicator.color = MissionEditorValidator.GetValidationColor(result.status);
			validityIndicator.gameObject.SetActive(value: true);
		}
		if (validityTooltip != null)
		{
			validityTooltip.gameObject.SetActive(value: true);
			if (validityTooltip.textString != "")
			{
				validityTooltip.textString += "\n";
			}
			validityTooltip.textString += result.message;
		}
	}

	public void SetHighlighter(Color color)
	{
		if (highlighter != null)
		{
			highlighter.color = color;
			highlighter.gameObject.SetActive(value: true);
		}
	}

	public void ClearHighlighter()
	{
		if (highlighter != null)
		{
			highlighter.gameObject.SetActive(value: false);
		}
	}

	public void ToggleHighlighter()
	{
		if (highlighter != null)
		{
			ToggleHighlighter(!highlighter.gameObject.activeSelf);
		}
	}

	public void ToggleHighlighter(bool state)
	{
		if (highlighter != null)
		{
			highlighter.gameObject.SetActive(state);
		}
	}

	public void ToggleInputHolderHighlighter(bool state)
	{
		if (inputHolderHighLighter != null)
		{
			inputHolderHighLighter.gameObject.SetActive(state);
		}
	}

	public void ToggleOutputHolderHighlighter(bool state)
	{
		if (outputHolderHighLighter != null)
		{
			outputHolderHighLighter.gameObject.SetActive(state);
		}
	}

	public Color GetHighlighterColor()
	{
		return highlighter.color;
	}

	public void ToggleSelectionHighlighter(bool show)
	{
		if (selectedHighlighter != null)
		{
			selectedHighlighter.gameObject.SetActive(show);
		}
	}

	public void OnHistoryDock(ConfigNode data, HistoryType type)
	{
		if (type == HistoryType.Undo)
		{
			UndockNode();
			return;
		}
		Guid value = default(Guid);
		int value2 = 0;
		data.TryGetValue("dockedIndex", ref value2);
		if (data.TryGetValue("dockParent", ref value))
		{
			DockNode(meLogic.EditorMission.nodes[value].guiNode, value2, fromHistory: true);
		}
	}

	public void OnHistoryUnDock(ConfigNode data, HistoryType type)
	{
		OnHistoryDock(data, (type != HistoryType.Undo) ? HistoryType.Undo : HistoryType.Redo);
	}

	public void OnHistoryMove(ConfigNode data, HistoryType type)
	{
		if (data.TryGetValue("position", ref Node.editorPosition))
		{
			rectTransform.anchoredPosition = Node.editorPosition;
		}
		UpdateConnectors();
	}

	public void OnHistoryCreate(ConfigNode data, HistoryType type)
	{
		if (type == HistoryType.Undo)
		{
			if (nodeIcon != null)
			{
				MENodeCategorizer.Instance.RemoveCanvasNodeButton(nodeIcon);
			}
			Destroy();
			return;
		}
		Setup();
		if (data.TryGetValue("position", ref Node.editorPosition))
		{
			rectTransform.anchoredPosition = Node.editorPosition;
		}
		meLogic.AddNode(this);
		MENodeCanvas.Instance.CalculateBorders();
	}

	public void OnHistoryDestroy(ConfigNode data, HistoryType type)
	{
		if (type == HistoryType.Undo)
		{
			Setup();
			if (data.TryGetValue("position", ref Node.editorPosition))
			{
				rectTransform.anchoredPosition = Node.editorPosition;
			}
			meLogic.AddNode(this);
			meLogic.NodeSelectionChange(this);
			MENodeCanvas.Instance.CalculateBorders();
			Guid value = default(Guid);
			if (data.TryGetValue("dockParent", ref value))
			{
				int value2 = 0;
				data.TryGetValue("dockedIndex", ref value2);
				DockNode(meLogic.EditorMission.nodes[value].guiNode, value2, fromHistory: true);
			}
		}
		else
		{
			Destroy();
		}
	}

	public ConfigNode GetState()
	{
		ConfigNode configNode = new ConfigNode();
		configNode.AddValue("position", Node.editorPosition);
		configNode.AddValue("dockedIndex", Node.dockedIndex);
		if (dockedParentNode != null)
		{
			configNode.AddValue("dockParent", dockedParentNode.Node.id);
		}
		return configNode;
	}
}

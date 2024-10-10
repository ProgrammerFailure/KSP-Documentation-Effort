using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

namespace Expansions.Missions.Editor;

public class MEGUIConnector : MonoBehaviour, IMEHistoryTarget
{
	public MissionEditorLogic meLogic;

	public MEGUINode fromNode;

	public MEGUINode toNode;

	public VectorLine connectionLine;

	public float lineExtendFromConnectorDistance = 25f;

	public float normalLineWidth = 4f;

	public float selectedLineWidth = 8f;

	public int curveSegments = 5;

	public float curveArcSize = 15f;

	public Color lineColour = Color.white;

	public bool isSelected;

	[MEGUI_ColorPicker(resetValue = "#FFFFFF", guiName = "#autoLOC_8006115")]
	public Color LineColour
	{
		get
		{
			return lineColour;
		}
		set
		{
			lineColour = value;
			connectionLine.color = value;
			UpdateLine();
		}
	}

	public void SetUpConnector(Canvas screenCanvas, RectTransform contentTransform, MissionEditorLogic missionEditorLogic)
	{
		Vector2[] collection = new Vector2[6 + curveSegments * 4];
		connectionLine = new VectorLine("Line", new List<Vector2>(collection), normalLineWidth, LineType.Continuous, Joins.Weld);
		connectionLine.rectTransform.SetParent(contentTransform, worldPositionStays: false);
		connectionLine.rectTransform.SetSiblingIndex(0);
		connectionLine.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
		connectionLine.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
		connectionLine.rectTransform.localPosition = Vector3.zero;
		connectionLine.rectTransform.anchoredPosition = Vector2.zero;
		connectionLine.rectTransform.localScale = Vector3.one;
		connectionLine.SetCanvas(screenCanvas);
		connectionLine.color = LineColour;
		connectionLine.SetWidth(normalLineWidth);
		meLogic = missionEditorLogic;
		StartCoroutine(FirstUpdate());
	}

	public IEnumerator FirstUpdate()
	{
		yield return null;
		UpdateLine();
	}

	public IEnumerator DelayedUpdateLine()
	{
		yield return null;
		UpdateLine();
	}

	public void UpdateLine()
	{
		Vector2 zero = Vector2.zero;
		Vector2 vector = Vector2.zero;
		bool flag = false;
		float num = lineExtendFromConnectorDistance;
		float num2 = lineExtendFromConnectorDistance;
		if ((bool)fromNode)
		{
			zero = fromNode.GetOutputButtonPosition();
		}
		else
		{
			zero = meLogic.GetGridMousePosition();
			num = 0f;
			flag = true;
		}
		if ((bool)toNode)
		{
			vector = toNode.GetInputButtonPosition();
		}
		else if (!flag)
		{
			num2 = 0f;
			vector = meLogic.GetGridMousePosition();
		}
		if (!(zero != Vector2.zero) || !(vector != Vector2.zero))
		{
			return;
		}
		connectionLine.points2[0] = zero;
		if (zero.y != vector.y)
		{
			if (zero.x < vector.x - (num + num2))
			{
				Vector2 point = new Vector2((zero.x + vector.x) / 2f, zero.y);
				Vector2 point2 = new Vector2((zero.x + vector.x) / 2f, vector.y);
				int index = 1;
				float deltaY = zero.y - vector.y;
				MakeCurveType1(point, curveArcSize, deltaY, ref index);
				MakeCurveType2(point2, curveArcSize, deltaY, ref index);
				connectionLine.drawEnd = index;
			}
			else
			{
				Vector3 vector2 = new Vector2(zero.x + num, zero.y);
				Vector3 vector3 = new Vector2(zero.x + num, (zero.y + vector.y) / 2f);
				Vector3 vector4 = new Vector2(vector.x - num2, (zero.y + vector.y) / 2f);
				Vector3 vector5 = new Vector2(vector.x - num2, vector.y);
				int index2 = 1;
				MakeCurveType1(vector2, curveArcSize, vector2.y - vector3.y, ref index2);
				MakeCurveType2(vector3, vector4.x - vector3.x, vector2.y - vector3.y, ref index2);
				MakeCurveType1(vector4, vector4.x - vector3.x, vector4.y - vector5.y, ref index2);
				MakeCurveType2(vector5, curveArcSize, vector4.y - vector5.y, ref index2);
				connectionLine.drawEnd = index2;
			}
		}
		else if (zero.x < vector.x)
		{
			connectionLine.drawEnd = 1;
		}
		else
		{
			connectionLine.points2[1] = new Vector2(zero.x + num, zero.y);
			connectionLine.points2[2] = new Vector2(vector.x - num2, vector.y);
			connectionLine.drawEnd = 3;
		}
		connectionLine.points2[connectionLine.drawEnd] = vector;
		connectionLine.Draw();
	}

	public void MakeCurveType1(Vector2 point, float deltaX, float deltaY, ref int index)
	{
		if (Mathf.Abs(deltaX) > curveArcSize)
		{
			deltaX = curveArcSize * Mathf.Sign(deltaX);
		}
		if (Mathf.Abs(deltaY) > curveArcSize)
		{
			deltaY = curveArcSize * Mathf.Sign(deltaY);
		}
		connectionLine.MakeCurve(new Vector2(point.x - deltaX, point.y), point, new Vector2(point.x, point.y - deltaY), point, curveSegments, index);
		index += curveSegments + 1;
	}

	public void MakeCurveType2(Vector2 point, float deltaX, float deltaY, ref int index)
	{
		if (Mathf.Abs(deltaX) > curveArcSize)
		{
			deltaX = curveArcSize * Mathf.Sign(deltaX);
		}
		if (Mathf.Abs(deltaY) > curveArcSize)
		{
			deltaY = curveArcSize * Mathf.Sign(deltaY);
		}
		connectionLine.MakeCurve(new Vector2(point.x, point.y + deltaY), point, new Vector2(point.x + deltaX, point.y), point, curveSegments, index);
		index += curveSegments + 1;
	}

	public void HideLine()
	{
		connectionLine.color = Color.clear;
	}

	public void AddNewEnd(MEGUINode node, MENodeConnectionType type)
	{
		switch (type)
		{
		case MENodeConnectionType.Input:
			toNode = node;
			break;
		case MENodeConnectionType.Output:
			fromNode = node;
			break;
		}
		if (toNode != null && fromNode != null)
		{
			MissionEditorHistory.PushUndoAction(this, OnHistoryCreate, true);
		}
	}

	public bool IsConnectedToNode(MEGUINode node)
	{
		if (node != null)
		{
			if (!(fromNode == node))
			{
				return toNode == node;
			}
			return true;
		}
		return false;
	}

	public bool IsNewConnectionValid(MENodeConnectionType newType)
	{
		if (newType == MENodeConnectionType.Input && toNode == null)
		{
			return true;
		}
		if (newType == MENodeConnectionType.Output)
		{
			return fromNode == null;
		}
		return false;
	}

	public MEGUINode GetConnectedNode()
	{
		if (fromNode != null)
		{
			return fromNode;
		}
		return toNode;
	}

	public void ConnectLogic()
	{
		if (toNode != null && toNode.Node != null && fromNode != null && fromNode.Node != null)
		{
			if (toNode.Node.IsOrphanNode)
			{
				toNode.Node.mission.UpdateOrphanNodeState(toNode.Node, makeOrphan: false);
			}
			toNode.Node.fromNodeIDs.AddUnique(fromNode.Node.id);
			toNode.Node.fromNodes.AddUnique(fromNode.Node);
			fromNode.Node.toNodeIDs.AddUnique(toNode.Node.id);
			fromNode.Node.toNodes.AddUnique(toNode.Node);
			toNode.Node.SetCatchAllNode(newCatchAll: false);
			toNode.UpdateInputConnectorImage();
			fromNode.UpdateOutputConnectorImage();
			if (MissionEditorLogic.Instance.CurrentSelectedNode == fromNode || MissionEditorLogic.Instance.CurrentSelectedNode == toNode)
			{
				MissionEditorLogic.Instance.actionPane.SAPRefreshNodeParameters();
			}
			GameEvents.Mission.onBuilderNodeConnection.Fire(new GameEvents.FromToAction<MENode, MENode>(fromNode.Node, toNode.Node));
		}
	}

	public void DisconnectLogic()
	{
		if (toNode != null && toNode.Node != null && fromNode != null && fromNode.Node != null)
		{
			toNode.Node.fromNodeIDs.Remove(fromNode.Node.id);
			toNode.Node.fromNodes.Remove(fromNode.Node);
			fromNode.Node.toNodeIDs.Remove(toNode.Node.id);
			fromNode.Node.toNodes.Remove(toNode.Node);
			toNode.UpdateInputConnectorImage();
			fromNode.UpdateOutputConnectorImage();
			if (!fromNode.HasOutputConnections())
			{
				MissionEditorLogic.Instance.actionPane.SAPRefreshNodeParameters();
			}
			GameEvents.Mission.onBuilderNodeDisconnection.Fire(new GameEvents.FromToAction<MENode, MENode>(fromNode.Node, toNode.Node));
		}
	}

	public void Destroy()
	{
		if (fromNode != null && toNode != null)
		{
			MissionEditorHistory.PushUndoAction(this, OnHistoryDestroy);
		}
		if (fromNode != null)
		{
			fromNode.RemoveLine(this, MENodeConnectionType.Output);
		}
		if (toNode != null)
		{
			toNode.RemoveLine(this, MENodeConnectionType.Input);
		}
		DisconnectLogic();
		meLogic.ClearConnector(this);
		connectionLine.rectTransform.gameObject.SetActive(value: false);
	}

	public void CleanUp()
	{
		if (connectionLine != null)
		{
			VectorLine.Destroy(ref connectionLine);
			connectionLine = null;
		}
		Object.Destroy(base.gameObject);
	}

	public void Activate()
	{
		if (fromNode != null)
		{
			fromNode.AddNewLine(this, MENodeConnectionType.Output);
		}
		if (toNode != null)
		{
			toNode.AddNewLine(this, MENodeConnectionType.Input);
		}
		ConnectLogic();
		meLogic.AddConnectorToConnectorList(this);
		connectionLine.rectTransform.gameObject.SetActive(value: true);
	}

	public bool WasMouseClickWithinTolerance(Vector2 clickPosition, float acceptableDistanceForLineCheck)
	{
		int num = connectionLine.drawEnd + 1;
		if (num == connectionLine.points2.Count)
		{
			if ((connectionLine.points2[curveSegments + 1].x - acceptableDistanceForLineCheck > clickPosition.x && connectionLine.points2[num - curveSegments].x - acceptableDistanceForLineCheck > clickPosition.x) || (connectionLine.points2[curveSegments + 1].x + acceptableDistanceForLineCheck < clickPosition.x && connectionLine.points2[num - curveSegments].x + acceptableDistanceForLineCheck < clickPosition.x) || (connectionLine.points2[0].y - acceptableDistanceForLineCheck > clickPosition.y && connectionLine.points2[num - 1].y - acceptableDistanceForLineCheck > clickPosition.y) || (connectionLine.points2[0].y + acceptableDistanceForLineCheck < clickPosition.y && connectionLine.points2[num - 1].y + acceptableDistanceForLineCheck < clickPosition.y))
			{
				return false;
			}
		}
		else if ((connectionLine.points2[0].x - acceptableDistanceForLineCheck > clickPosition.x && connectionLine.points2[num - 1].x - acceptableDistanceForLineCheck > clickPosition.x) || (connectionLine.points2[0].x + acceptableDistanceForLineCheck < clickPosition.x && connectionLine.points2[num - 1].x + acceptableDistanceForLineCheck < clickPosition.x) || (connectionLine.points2[0].y - acceptableDistanceForLineCheck > clickPosition.y && connectionLine.points2[num - 1].y - acceptableDistanceForLineCheck > clickPosition.y) || (connectionLine.points2[0].y + acceptableDistanceForLineCheck < clickPosition.y && connectionLine.points2[num - 1].y + acceptableDistanceForLineCheck < clickPosition.y))
		{
			return false;
		}
		int num2 = 0;
		while (true)
		{
			if (num2 < num - 1)
			{
				if ((!(connectionLine.points2[num2].x - acceptableDistanceForLineCheck > clickPosition.x) || !(connectionLine.points2[num2 + 1].x - acceptableDistanceForLineCheck > clickPosition.x)) && (!(connectionLine.points2[num2].x + acceptableDistanceForLineCheck < clickPosition.x) || !(connectionLine.points2[num2 + 1].x + acceptableDistanceForLineCheck < clickPosition.x)) && (!(connectionLine.points2[num2].y - acceptableDistanceForLineCheck > clickPosition.y) || !(connectionLine.points2[num2 + 1].y - acceptableDistanceForLineCheck > clickPosition.y)) && (!(connectionLine.points2[num2].y + acceptableDistanceForLineCheck < clickPosition.y) || !(connectionLine.points2[num2 + 1].y + acceptableDistanceForLineCheck < clickPosition.y)) && !(Vector3.Cross((connectionLine.points2[num2 + 1] - connectionLine.points2[num2]).normalized, clickPosition - connectionLine.points2[num2]).magnitude >= acceptableDistanceForLineCheck))
				{
					break;
				}
				num2++;
				continue;
			}
			return false;
		}
		return true;
	}

	public void ResetLineWidth(bool zoomed)
	{
		if (isSelected)
		{
			connectionLine.SetWidth(selectedLineWidth / meLogic.NodeCanvas.zoomDisplayPercentage);
		}
		else
		{
			connectionLine.SetWidth(normalLineWidth / meLogic.NodeCanvas.zoomDisplayPercentage);
		}
		connectionLine.Draw();
	}

	public void Select()
	{
		isSelected = true;
		connectionLine.SetWidth(selectedLineWidth);
		UpdateLine();
	}

	public void Deselect()
	{
		isSelected = false;
		connectionLine.SetWidth(normalLineWidth);
		UpdateLine();
	}

	public void OnHistoryCreate(ConfigNode data, HistoryType type)
	{
		if (type == HistoryType.Undo)
		{
			Destroy();
		}
		else
		{
			Activate();
		}
	}

	public void OnHistoryDestroy(ConfigNode data, HistoryType type)
	{
		if (type == HistoryType.Undo)
		{
			Activate();
			if (meLogic.CurrentSelectedNode == null)
			{
				meLogic.ConnectorSelectionChange(this);
			}
		}
		else
		{
			Destroy();
		}
	}

	public ConfigNode GetState()
	{
		return null;
	}
}

using UnityEngine;
using UnityEngine.EventSystems;

namespace Expansions.Missions.Editor;

public class MEGUIConnectorDrag : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	[SerializeField]
	public MEGUINode meGUIParent;

	[SerializeField]
	public MENodeConnectionType connectorType;

	public MEGUINode GetMeGUIParent()
	{
		return meGUIParent;
	}

	public bool IsValidDrop(MENodeConnectionType startConnectorType)
	{
		switch (startConnectorType)
		{
		case MENodeConnectionType.Input:
			if (connectorType == MENodeConnectionType.Output)
			{
				return true;
			}
			return false;
		case MENodeConnectionType.Output:
			if (connectorType == MENodeConnectionType.Input)
			{
				return true;
			}
			return false;
		default:
			return false;
		}
	}

	void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (connectorType == MENodeConnectionType.Input)
			{
				meGUIParent.InputButtonDragged();
			}
			else if (connectorType == MENodeConnectionType.Output)
			{
				meGUIParent.OutputButtonDragged();
			}
		}
	}

	void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
	{
		if (eventData.button != 0)
		{
			return;
		}
		MEGUIConnectorDrag mEGUIConnectorDrag = null;
		foreach (GameObject item in eventData.hovered)
		{
			mEGUIConnectorDrag = item.GetComponent<MEGUIConnectorDrag>();
			if (mEGUIConnectorDrag != null)
			{
				if (mEGUIConnectorDrag.IsValidDrop(connectorType))
				{
					break;
				}
				mEGUIConnectorDrag = null;
			}
		}
		if (mEGUIConnectorDrag == null)
		{
			meGUIParent.CancelDrag();
		}
		else if (connectorType == MENodeConnectionType.Input)
		{
			mEGUIConnectorDrag.GetMeGUIParent().InputButtonDropped();
		}
		else if (connectorType == MENodeConnectionType.Output)
		{
			mEGUIConnectorDrag.GetMeGUIParent().OutputButtonDropped();
		}
	}
}

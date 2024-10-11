using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Expansions.Missions.Editor;

public class MEGUIConnectorDrag : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	[SerializeField]
	private MEGUINode meGUIParent;

	[SerializeField]
	private MENodeConnectionType connectorType;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIConnectorDrag()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUINode GetMeGUIParent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsValidDrop(MENodeConnectionType startConnectorType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
	{
		throw null;
	}
}

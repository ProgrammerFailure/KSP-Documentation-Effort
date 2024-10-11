using System.Runtime.CompilerServices;
using Cursors;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Expansions.Missions.Editor;

public class MEGUIPanelSeparator : MonoBehaviour, IDragHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler
{
	public MEGUIPanel Panel;

	public string CursorID;

	public TextureCursor DefaultCursor;

	public TextureCursor LeftClickCursor;

	public TextureCursor RightClickCursor;

	public Vector2 PanelOffset;

	protected static bool isDragging;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIPanelSeparator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerEnter(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerExit(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnBeginDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnEndDrag(PointerEventData eventData)
	{
		throw null;
	}
}

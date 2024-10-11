using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class ActionPaneDisplay : Selectable, IPointerClickHandler, IEventSystemHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	[Serializable]
	public class RaycastEvent : UnityEvent<RaycastHit?>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public RaycastEvent()
		{
			throw null;
		}
	}

	[Serializable]
	public class DragEvent : UnityEvent<PointerEventData.InputButton, Vector2>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public DragEvent()
		{
			throw null;
		}
	}

	[Serializable]
	public class MouseOverEvent : UnityEvent<Vector2>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public MouseOverEvent()
		{
			throw null;
		}
	}

	[HideInInspector]
	public RectTransform rectTransform;

	[HideInInspector]
	public RawImage displayImage;

	[SerializeField]
	protected Camera displayCamera;

	protected RenderTexture displayTexture;

	protected float hitDistance;

	private Ray ray;

	private RaycastHit hit;

	protected int layerMask;

	private Vector2 cameraPoint;

	private float aspect;

	protected bool isSelected;

	protected bool isDragging;

	protected bool isMouseOver;

	private RectMask2D displayMask;

	protected DictionaryValueList<string, MonoBehaviour> toolbarControls;

	public RaycastEvent DisplayClick;

	public RaycastEvent DisplayClickUp;

	public DragEvent DisplayDrag;

	public RaycastEvent DisplayDragEnd;

	public MouseOverEvent MouseOver;

	public RenderTexture DisplayTexture
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ActionPaneDisplay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Setup(Camera displayCamera, int layerMask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateDisplayArea()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Clean()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Destroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool GetMousePointOnCamera(Vector2 mousePosition, Camera canvasCamera, ref Vector2 point)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Raycast(Vector3 cameraPoint, out RaycastHit hit, int layerMask = -1)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Button AddToolbarButton(string id, string icon, string toolTip)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Toggle AddToolbarToggle(string id, string icon, string toolTip, bool startState = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Button GetToolbarButton(string id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ClearToolbarEvents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearToolbar()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private T AddObject<T>(string id, string prefab, string icon, string toolTip, DictionaryValueList<string, MonoBehaviour> controls, Transform parent) where T : MonoBehaviour
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnPointerUp(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnPointerClick(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnBeginDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnEndDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnDrag(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSelect(BaseEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnDeselect(BaseEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnPointerEnter(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnPointerExit(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnDisplayDrag(PointerEventData.InputButton button, Vector2 delta)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnDisplayDragEnd(RaycastHit? hit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnDisplayClick(RaycastHit? hit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnDisplayClickUp(RaycastHit? hit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnMouseOver(Vector2 position)
	{
		throw null;
	}
}

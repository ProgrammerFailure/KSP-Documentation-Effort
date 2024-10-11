using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class PopupDialog : MonoBehaviour
{
	private RectTransform rTrf;

	public MultiOptionDialog dialogToDisplay;

	public string defaultSkinName;

	public GameObject popupWindow;

	private DragPanel dragPanel;

	public Callback OnDismiss;

	private bool hover;

	private static List<PopupDialog> instantiatedPopUps;

	public bool modal;

	private List<GameObject> childItems;

	private Vector2 anchoredPosition;

	private Vector2 prevSize;

	private CanvasGroup canvasGroup;

	public UnityEvent onDestroy;

	public bool Hover
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public RectTransform RTrf
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsDraggable
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PopupDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static PopupDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ClearPopUps()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void DismissPopup(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static PopupDialog SpawnPopupDialog(Vector2 anchorMin, Vector2 anchorMax, string dialogName, string title, string message, string buttonMessage, bool persistAcrossScenes, UISkinDef skin, bool isModal = true, string titleExtra = "")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static PopupDialog SpawnPopupDialog(Vector2 anchorMin, Vector2 anchorMax, MultiOptionDialog dialog, bool persistAcrossScenes, UISkinDef skin, bool isModal = true, string titleExtra = "")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static PopupDialog SpawnPopupDialog(MultiOptionDialog dialog, bool persistAcrossScenes, UISkinDef skin, bool isModal = true, string titleExtra = "")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetPopupData(MultiOptionDialog dialog, UISkinDef skin)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool CheckForOpenDialogs()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnRectTransformDimensionsChange()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Dismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Dismiss(bool KeepMouseState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetDraggable(bool draggable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool Contains(Vector3 point)
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
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRenderObject()
	{
		throw null;
	}
}

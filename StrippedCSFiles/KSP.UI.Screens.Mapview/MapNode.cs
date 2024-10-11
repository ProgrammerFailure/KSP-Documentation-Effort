using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI.Screens.Mapview;

public class MapNode : Selectable
{
	public enum PatchTransitionNodeType
	{
		Encounter,
		EncounterNextPatch,
		Escape,
		EscapeNextPatch,
		Impact
	}

	public enum ApproachNodeType
	{
		CloseApproachOwn,
		CloseApproachOther,
		IntersectOwn,
		IntersectOther
	}

	public enum SiteType
	{
		LaunchSite,
		Runway,
		KSC
	}

	public class CaptionData
	{
		public string Header;

		public string captionLine1;

		public string captionLine2;

		public string captionLine3;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public CaptionData(string header)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string FormatCaption(float headerSize, float textSize)
		{
			throw null;
		}
	}

	public class IconData
	{
		public bool visible;

		public bool iconEnabled;

		public Color color;

		public int pixelSize;

		public Vector3 offset;

		public Color bgColor;

		public Vector2 bgSize;

		public Vector3 bgOffset;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public IconData(bool visibleByDefault, Color defaultColor, int defaultSize)
		{
			throw null;
		}
	}

	public class TypeData
	{
		public MapObject.ObjectType oType;

		public VesselType vType;

		public PatchTransitionNodeType pType;

		public ApproachNodeType aType;

		public SiteType sType;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public TypeData(MapObject.ObjectType type)
		{
			throw null;
		}
	}

	private bool inputBlocking;

	[SerializeField]
	private CanvasGroup canvasGroup;

	private Color pinnedTextColor;

	private Color unpinnedTextColor;

	private Vector3d scaledSpacePos;

	private Vector3 screenPos;

	[SerializeField]
	protected Image img;

	[SerializeField]
	protected TextMeshProUGUI textCaption;

	[SerializeField]
	public MapObject mapObject;

	[SerializeField]
	protected SpriteRenderer bgImg;

	[SerializeField]
	public TMP_FontAsset labelFont;

	[SerializeField]
	protected Sprite[] iconSprites;

	[SerializeField]
	private Canvas textCanvas;

	public static float captionHeaderHeight;

	public static float captionTextHeight;

	private static List<MapNode> allNodes;

	private Vector3 captionPos0;

	private Bounds textBounds;

	protected Color color;

	protected int pixelSize;

	[CompilerGenerated]
	private Func<MapNode, Vector3d, Vector3> OnUpdatePositionToUI;

	private bool hoverOnLastPress;

	private CaptionData cData;

	private IconData vData;

	internal TypeData tData;

	private int iconIndex;

	private int iconIndexLast;

	private bool hover;

	private bool pinnable;

	private bool pinned;

	private bool prevPinned;

	private RectTransform trf;

	private Vector2 preferedDir;

	[SerializeField]
	private AlarmMapNodeButton addAlarmButton;

	internal bool addAlarmHover;

	private bool showAddAlarmButton;

	public static float zSpaceEasing;

	public static float zSpaceMidpoint;

	public static float zSpaceLength;

	public static float zSpaceUIStart;

	private float fadeEnd;

	private bool fading;

	[SerializeField]
	private float behindBodyOpacity;

	[SerializeField]
	private float behindBodyFadeSpeed;

	private RaycastHit nodeUpdateHitInfo;

	private static string cacheAutoLOC_6003113;

	private static string cacheAutoLOC_6003114;

	private static string cacheAutoLOC_6003115;

	private static string cacheAutoLOC_6003116;

	public static List<MapNode> AllMapNodes
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public IconData VisualIconData
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool Hover
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool Pinnable
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool Pinned
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool HoverOrPinned
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool Interactable
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public bool InputBlocking
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	protected float Opacity
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public event Callback<MapNode, IconData> OnUpdateVisible
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

	public event Func<MapNode, Vector3d> OnUpdatePosition
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

	internal event Func<MapNode, Vector3d, Vector3> _001E
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

	public event Callback<MapNode, CaptionData> OnUpdateCaption
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

	public event Callback<MapNode, TypeData> OnUpdateType
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

	public event Callback<bool> OnPress
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

	public event Callback<bool> OnRelease
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

	public event Callback<MapNode, Mouse.Buttons> OnClick
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
	public MapNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static MapNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MapNode Create(MapObject mObj, Color color, int pixelSize, bool hoverable, bool pinnable, bool blocksInput, Transform parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MapNode Create(MapObject mObj, Color color, int pixelSize, bool hoverable, bool pinnable, bool blocksInput)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MapNode Create(string name, Color color, int pixelSize, bool hoverable, bool pinnable, bool blocksInput)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Terminate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Init()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void CheckAndEnablePinnedCaption()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMapEntered()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnEnable()
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
	internal void OnAddAlarmPointerExit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void HideAddAlarm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnPointerDown(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnPointerUp(PointerEventData eventData)
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
	public void NodeUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CaptionUpdate(CaptionData cData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void UpdateCaptionText(CaptionData cData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void MoveAwayFromEachOthers()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void UpdateCursorInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void UpdateHoverCaption(bool st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateType()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetIconIndex(TypeData tData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetIcon(int iconIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AttachLabel(Vector2 offset, string textMessage, int fontSize)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetIcon(Sprite sprite, Material mat = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetBackground(Sprite sprite, Material mat = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetType(TypeData typeData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}

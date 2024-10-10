using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns23;

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
		const_2
	}

	public class CaptionData
	{
		public string Header;

		public string captionLine1;

		public string captionLine2;

		public string captionLine3;

		public CaptionData(string header)
		{
			Header = header;
			captionLine1 = null;
			captionLine2 = null;
			captionLine3 = null;
		}

		public string FormatCaption(float headerSize, float textSize)
		{
			string text = string.Empty;
			if (!string.IsNullOrEmpty(Header))
			{
				text = text + "<b>" + Header + "</b>";
			}
			if (!string.IsNullOrEmpty(captionLine1))
			{
				text = text + "\n" + captionLine1;
			}
			if (!string.IsNullOrEmpty(captionLine2))
			{
				text = text + "\n" + captionLine2;
			}
			if (!string.IsNullOrEmpty(captionLine3))
			{
				text = text + "\n" + captionLine3;
			}
			return text;
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

		public IconData(bool visibleByDefault, Color defaultColor, int defaultSize)
		{
			iconEnabled = true;
			visible = visibleByDefault;
			color = defaultColor;
			pixelSize = defaultSize;
		}
	}

	public class TypeData
	{
		public MapObject.ObjectType oType;

		public VesselType vType;

		public PatchTransitionNodeType pType;

		public ApproachNodeType aType;

		public SiteType sType;

		public TypeData(MapObject.ObjectType type)
		{
			oType = type;
			vType = VesselType.Unknown;
			aType = ApproachNodeType.CloseApproachOwn;
			pType = PatchTransitionNodeType.Impact;
			sType = SiteType.LaunchSite;
		}
	}

	public bool inputBlocking;

	[SerializeField]
	public CanvasGroup canvasGroup;

	public Color pinnedTextColor;

	public Color unpinnedTextColor;

	public Vector3d scaledSpacePos;

	public Vector3 screenPos;

	[SerializeField]
	public Image img;

	[SerializeField]
	public TextMeshProUGUI textCaption;

	[SerializeField]
	public MapObject mapObject;

	[SerializeField]
	public SpriteRenderer bgImg;

	[SerializeField]
	public TMP_FontAsset labelFont;

	[SerializeField]
	public Sprite[] iconSprites;

	[SerializeField]
	public Canvas textCanvas;

	public static float captionHeaderHeight = 22f;

	public static float captionTextHeight = 20f;

	public static List<MapNode> allNodes = new List<MapNode>();

	public Vector3 captionPos0;

	public Bounds textBounds;

	public Color color;

	public int pixelSize;

	[CompilerGenerated]
	private Func<MapNode, Vector3d, Vector3> OnUpdatePositionToUI = (MapNode m, Vector3d v) => MapViewCanvasUtil.ScaledToUISpacePos(v, ref m.vData.visible, zSpaceEasing, zSpaceMidpoint, zSpaceUIStart, zSpaceLength);

	public bool hoverOnLastPress;

	public CaptionData cData;

	public IconData vData;

	public TypeData tData;

	public int iconIndex = int.MinValue;

	public int iconIndexLast = int.MinValue;

	public bool hover;

	public bool pinnable;

	public bool pinned;

	public bool prevPinned;

	public RectTransform trf;

	public Vector2 preferedDir;

	[SerializeField]
	public AlarmMapNodeButton addAlarmButton;

	public bool addAlarmHover;

	public bool showAddAlarmButton;

	public static float zSpaceEasing = 0.8f;

	public static float zSpaceMidpoint = 500f;

	public static float zSpaceLength = 100f;

	public static float zSpaceUIStart = 110f;

	public float fadeEnd;

	public bool fading;

	[SerializeField]
	public float behindBodyOpacity = 0.5f;

	[SerializeField]
	public float behindBodyFadeSpeed = 1.5f;

	public RaycastHit nodeUpdateHitInfo;

	public static string cacheAutoLOC_6003113;

	public static string cacheAutoLOC_6003114;

	public static string cacheAutoLOC_6003115;

	public static string cacheAutoLOC_6003116;

	public static List<MapNode> AllMapNodes => allNodes;

	public IconData VisualIconData => vData;

	public bool Hover => hover;

	public bool Pinnable => pinnable;

	public bool Pinned => pinned;

	public bool HoverOrPinned
	{
		get
		{
			if (!hover)
			{
				return pinned;
			}
			return true;
		}
	}

	public bool Interactable
	{
		get
		{
			return canvasGroup.interactable;
		}
		set
		{
			canvasGroup.interactable = value;
		}
	}

	public bool InputBlocking
	{
		get
		{
			return inputBlocking;
		}
		set
		{
			inputBlocking = value;
			canvasGroup.blocksRaycasts = value;
		}
	}

	public float Opacity
	{
		get
		{
			return canvasGroup.alpha;
		}
		set
		{
			canvasGroup.alpha = value;
		}
	}

	public event Callback<MapNode, IconData> OnUpdateVisible = delegate
	{
	};

	public event Func<MapNode, Vector3d> OnUpdatePosition = (MapNode m) => Vector3d.zero;

	internal event Func<MapNode, Vector3d, Vector3> Event_0
	{
		[CompilerGenerated]
		add
		{
			Func<MapNode, Vector3d, Vector3> func = OnUpdatePositionToUI;
			Func<MapNode, Vector3d, Vector3> func2;
			do
			{
				func2 = func;
				Func<MapNode, Vector3d, Vector3> value2 = (Func<MapNode, Vector3d, Vector3>)Delegate.Combine(func2, value);
				func = Interlocked.CompareExchange(ref OnUpdatePositionToUI, value2, func2);
			}
			while ((object)func != func2);
		}
		[CompilerGenerated]
		remove
		{
			Func<MapNode, Vector3d, Vector3> func = OnUpdatePositionToUI;
			Func<MapNode, Vector3d, Vector3> func2;
			do
			{
				func2 = func;
				Func<MapNode, Vector3d, Vector3> value2 = (Func<MapNode, Vector3d, Vector3>)Delegate.Remove(func2, value);
				func = Interlocked.CompareExchange(ref OnUpdatePositionToUI, value2, func2);
			}
			while ((object)func != func2);
		}
	}

	public event Callback<MapNode, CaptionData> OnUpdateCaption = delegate
	{
	};

	public event Callback<MapNode, TypeData> OnUpdateType = delegate
	{
	};

	public event Callback<bool> OnPress = delegate
	{
	};

	public event Callback<bool> OnRelease = delegate
	{
	};

	public event Callback<MapNode, Mouse.Buttons> OnClick = delegate
	{
	};

	public static MapNode Create(MapObject mObj, Color color, int pixelSize, bool hoverable, bool pinnable, bool blocksInput, Transform parent)
	{
		MapNode mapNode = UnityEngine.Object.Instantiate(MapView.UINodePrefab);
		mapNode.name = ((mObj != null) ? (mObj.name + " Map Node") : "Unnamed Map Node");
		mapNode.trf.SetParent(parent);
		mapNode.mapObject = mObj;
		mapNode.color = color;
		mapNode.pixelSize = pixelSize;
		mapNode.Interactable = hoverable;
		mapNode.pinnable = pinnable;
		mapNode.InputBlocking = blocksInput;
		mObj.uiNode = mapNode;
		mapNode.Init();
		return mapNode;
	}

	public static MapNode Create(MapObject mObj, Color color, int pixelSize, bool hoverable, bool pinnable, bool blocksInput)
	{
		return Create(mObj, color, pixelSize, hoverable, pinnable, blocksInput, MapViewCanvasUtil.NodeContainer);
	}

	public static MapNode Create(string name, Color color, int pixelSize, bool hoverable, bool pinnable, bool blocksInput)
	{
		MapNode mapNode = UnityEngine.Object.Instantiate(MapView.UINodePrefab);
		mapNode.name = name;
		mapNode.trf.SetParent(MapViewCanvasUtil.NodeContainer);
		mapNode.color = color;
		mapNode.pixelSize = pixelSize;
		mapNode.Interactable = hoverable;
		mapNode.pinnable = pinnable;
		mapNode.InputBlocking = blocksInput;
		mapNode.Init();
		return mapNode;
	}

	public void Terminate()
	{
		allNodes.Remove(this);
		if ((bool)base.gameObject)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public override void Awake()
	{
		base.Awake();
		trf = base.gameObject.GetComponent<RectTransform>();
		hover = false;
		pinned = false;
		prevPinned = false;
		behindBodyOpacity = GameSettings.MAPNODE_BEHINDBODY_OPACITY;
		GameEvents.OnMapEntered.Add(OnMapEntered);
	}

	public void Init()
	{
		cData = new CaptionData(string.Empty);
		vData = new IconData(visibleByDefault: true, color, pixelSize);
		tData = new TypeData((!(mapObject != null)) ? MapObject.ObjectType.Generic : mapObject.type);
		unpinnedTextColor = textCaption.color;
		pinnedTextColor = textCaption.color.smethod_0(0.6f);
		captionPos0 = textCaption.rectTransform.localPosition;
		textCaption.gameObject.SetActive(value: false);
		textCanvas.overrideSorting = true;
		preferedDir = UnityEngine.Random.insideUnitCircle.normalized;
		addAlarmButton.mapObject = mapObject;
		addAlarmButton.gameObject.SetActive(value: false);
		if (AlarmClockScenario.MapNodeDefined(tData.oType) && mapObject.vesselRef != null)
		{
			showAddAlarmButton = true;
			Interactable = true;
			InputBlocking = true;
		}
		allNodes.Add(this);
	}

	public void CheckAndEnablePinnedCaption()
	{
		pinned = prevPinned;
		if (pinned)
		{
			UpdateHoverCaption(st: true);
		}
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		GameEvents.OnMapEntered.Remove(OnMapEntered);
	}

	public void OnMapEntered()
	{
		CheckAndEnablePinnedCaption();
	}

	public override void OnEnable()
	{
		CheckAndEnablePinnedCaption();
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		if (eventData != null)
		{
			base.OnPointerEnter(eventData);
		}
		hover = true;
		if (showAddAlarmButton && AlarmClockScenario.ShowAlarmMapButton(mapObject))
		{
			addAlarmButton.gameObject.SetActive(value: true);
		}
		UpdateHoverCaption(hover);
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		if (eventData != null)
		{
			base.OnPointerExit(eventData);
		}
		hover = false;
		if (!addAlarmHover)
		{
			addAlarmButton.gameObject.SetActive(value: false);
		}
		UpdateHoverCaption(hover || addAlarmHover);
	}

	public void OnAddAlarmPointerExit()
	{
		if (!hover)
		{
			addAlarmButton.gameObject.SetActive(value: false);
		}
		UpdateHoverCaption(hover || addAlarmHover);
	}

	public void HideAddAlarm()
	{
		addAlarmButton.OnPointerExit();
		addAlarmButton.gameObject.SetActive(value: false);
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		if (InputLockManager.IsUnlocked(ControlTypes.MAP_UI))
		{
			if (eventData != null)
			{
				base.OnPointerDown(eventData);
			}
			hoverOnLastPress = hover;
			this.OnPress(hover);
		}
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		if (!InputLockManager.IsUnlocked(ControlTypes.MAP_UI))
		{
			return;
		}
		if (eventData != null)
		{
			base.OnPointerUp(eventData);
		}
		this.OnRelease(hover);
		if (hoverOnLastPress)
		{
			if ((Mouse.Right.GetButtonUp() || (eventData != null && eventData.button == PointerEventData.InputButton.Right)) && pinnable)
			{
				pinned = !pinned;
				prevPinned = pinned;
				UpdateHoverCaption(hover);
			}
			this.OnClick(this, Mouse.GetAllMouseButtonsUp());
		}
	}

	public override void OnSelect(BaseEventData eventData)
	{
		if (InputLockManager.IsUnlocked(ControlTypes.MAP_UI))
		{
			base.OnSelect(eventData);
			hover = true;
			UpdateHoverCaption(hover);
		}
	}

	public override void OnDeselect(BaseEventData eventData)
	{
		if (InputLockManager.IsUnlocked(ControlTypes.MAP_UI))
		{
			base.OnDeselect(eventData);
			hover = false;
			UpdateHoverCaption(hover);
		}
	}

	public void NodeUpdate()
	{
		this.OnUpdateVisible(this, vData);
		if (vData.visible && vData.iconEnabled)
		{
			scaledSpacePos = this.OnUpdatePosition(this);
			screenPos = OnUpdatePositionToUI(this, scaledSpacePos);
			trf.localPosition = screenPos;
			Vector3 direction = (scaledSpacePos - PlanetariumCamera.fetch.transform.position) * 0.8999999761581421;
			if (Physics.Raycast(PlanetariumCamera.fetch.transform.position, direction, out nodeUpdateHitInfo, direction.magnitude, 1 << LayerMask.NameToLayer("Scaled Scenery")))
			{
				if (nodeUpdateHitInfo.collider != null && nodeUpdateHitInfo.collider.GetComponent<ScaledMovement>() != null && Opacity != behindBodyOpacity)
				{
					fadeEnd = behindBodyOpacity;
					fading = true;
				}
			}
			else if (Opacity != 1f)
			{
				fading = true;
				fadeEnd = 1f;
			}
			if (fading)
			{
				if (Opacity != fadeEnd)
				{
					Opacity = Mathf.MoveTowards(Opacity, fadeEnd, behindBodyFadeSpeed * Time.deltaTime);
				}
				else
				{
					fading = false;
				}
			}
			if (HoverOrPinned)
			{
				Opacity = 1f;
			}
			if (vData.visible)
			{
				if (!base.gameObject.activeSelf)
				{
					base.gameObject.SetActive(value: true);
				}
				UpdateType();
				img.transform.localPosition = vData.offset;
				img.rectTransform.sizeDelta = Vector3.one * vData.pixelSize;
				base.targetGraphic.rectTransform.sizeDelta = Vector3.one * vData.pixelSize;
				base.targetGraphic.transform.localPosition = new Vector3(vData.bgOffset.x, vData.bgOffset.y, 0.0001f);
				img.color = vData.color;
				if (bgImg != null && bgImg.enabled)
				{
					bgImg.transform.localPosition = new Vector3(vData.bgOffset.x, vData.bgOffset.y, 0.0001f);
					bgImg.transform.localScale = vData.bgSize;
					bgImg.color = vData.bgColor;
				}
				if (InputLockManager.IsUnlocked(ControlTypes.MAP_UI) && Interactable && !InputBlocking)
				{
					UpdateCursorInput();
				}
				if (hover || pinned)
				{
					this.OnUpdateCaption(this, cData);
					CaptionUpdate(cData);
				}
			}
		}
		if (!vData.visible || !vData.iconEnabled)
		{
			if (hover || pinned)
			{
				pinned = false;
				hover = false;
				UpdateHoverCaption(st: false);
			}
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(value: false);
			}
		}
	}

	public void CaptionUpdate(CaptionData cData)
	{
		UpdateCaptionText(cData);
	}

	public void UpdateCaptionText(CaptionData cData)
	{
		if (hover || pinned)
		{
			string text = cData.FormatCaption(captionHeaderHeight, captionTextHeight);
			textCaption.rectTransform.localPosition = captionPos0;
			textCaption.text = text;
			textBounds = textCaption.textBounds;
		}
	}

	public static void MoveAwayFromEachOthers()
	{
		int num = 0;
		bool flag;
		do
		{
			flag = true;
			int i = 0;
			for (int count = allNodes.Count; i < count; i++)
			{
				MapNode mapNode = allNodes[i];
				if ((!mapNode.hover && !mapNode.pinned) || string.IsNullOrEmpty(mapNode.textCaption.text))
				{
					continue;
				}
				Bounds bounds = mapNode.textBounds;
				bounds.center = (Vector2)bounds.center + (Vector2)mapNode.textCaption.rectTransform.position;
				Vector2 vector = bounds.center;
				Vector2 zero = Vector2.zero;
				int j = 0;
				for (int count2 = allNodes.Count; j < count2; j++)
				{
					if (i == j)
					{
						continue;
					}
					MapNode mapNode2 = allNodes[j];
					if ((!mapNode2.hover && !mapNode2.pinned) || string.IsNullOrEmpty(mapNode2.textCaption.text))
					{
						continue;
					}
					Bounds bounds2 = mapNode2.textBounds;
					bounds2.center = (Vector2)bounds2.center + (Vector2)mapNode2.textCaption.rectTransform.position;
					if (bounds.Intersects(bounds2))
					{
						Vector2 vector2 = bounds2.center;
						Vector2 vector3 = vector - vector2;
						float sqrMagnitude = vector3.sqrMagnitude;
						if (sqrMagnitude > 4f)
						{
							float num2 = 1f / sqrMagnitude;
							Vector2 normalized = vector3.normalized;
							zero += normalized * num2;
						}
						else
						{
							zero += mapNode.preferedDir;
						}
					}
				}
				if (zero.sqrMagnitude > 0f)
				{
					flag = false;
					zero = zero.normalized * 10f;
					Vector3 localPosition = mapNode.textCaption.rectTransform.localPosition + (Vector3)zero;
					if ((zero.x > 0f) ^ (localPosition.x < 0f))
					{
						localPosition.x = Mathf.Sign(localPosition.x) * Mathf.Ceil(Mathf.Abs(localPosition.x) / 5f) * 5f;
					}
					else
					{
						localPosition.x = Mathf.Sign(localPosition.x) * Mathf.Floor(Mathf.Abs(localPosition.x) / 5f) * 5f;
					}
					if ((zero.y > 0f) ^ (localPosition.y < 0f))
					{
						localPosition.y = Mathf.Sign(localPosition.y) * Mathf.Ceil(Mathf.Abs(localPosition.y) / 5f) * 5f;
					}
					else
					{
						localPosition.y = Mathf.Sign(localPosition.y) * Mathf.Floor(Mathf.Abs(localPosition.y) / 5f) * 5f;
					}
					mapNode.textCaption.rectTransform.localPosition = localPosition;
				}
			}
			num++;
		}
		while (!flag && num < 40);
	}

	public void UpdateCursorInput()
	{
		if (!RectTransformUtility.RectangleContainsScreenPoint(base.targetGraphic.rectTransform, Input.mousePosition, UIMasterController.Instance.uiCamera) && (!pinned || !RectTransformUtility.RectangleContainsScreenPoint(textCaption.rectTransform, Input.mousePosition, UIMasterController.Instance.uiCamera)))
		{
			if (hover)
			{
				OnPointerExit(null);
			}
			return;
		}
		if (!hover)
		{
			OnPointerEnter(null);
		}
		if (Mouse.Right.GetButtonDown())
		{
			OnPointerDown(null);
		}
		if (Mouse.Right.GetButtonUp())
		{
			OnPointerUp(null);
		}
	}

	public void UpdateHoverCaption(bool st)
	{
		if (st)
		{
			textCaption.gameObject.SetActive(value: true);
			if (pinned)
			{
				textCaption.color = pinnedTextColor;
			}
			else
			{
				textCaption.color = unpinnedTextColor;
			}
		}
		else if (!pinned)
		{
			textCaption.gameObject.SetActive(value: false);
		}
	}

	public void UpdateType()
	{
		this.OnUpdateType(this, tData);
		iconIndex = GetIconIndex(tData);
		if (iconIndex != iconIndexLast)
		{
			SetIcon(iconIndex);
			iconIndexLast = iconIndex;
		}
	}

	public int GetIconIndex(TypeData tData)
	{
		switch (tData.oType)
		{
		default:
			return 22;
		case MapObject.ObjectType.CelestialBody:
			return 6;
		case MapObject.ObjectType.Vessel:
			return tData.vType switch
			{
				VesselType.Debris => 7, 
				VesselType.SpaceObject => 21, 
				VesselType.Probe => 18, 
				VesselType.Relay => 24, 
				VesselType.Rover => 19, 
				VesselType.Lander => 14, 
				VesselType.Ship => 20, 
				VesselType.Plane => 23, 
				VesselType.Station => 0, 
				VesselType.Base => 5, 
				VesselType.const_11 => 13, 
				VesselType.Flag => 11, 
				VesselType.DeployedScienceController => 28, 
				VesselType.DeployedGroundPart => 29, 
				_ => 22, 
			};
		case MapObject.ObjectType.ManeuverNode:
			return 15;
		case MapObject.ObjectType.Periapsis:
			return 17;
		case MapObject.ObjectType.Apoapsis:
			return 2;
		case MapObject.ObjectType.AscendingNode:
			return 1;
		case MapObject.ObjectType.DescendingNode:
			return 8;
		case MapObject.ObjectType.ApproachIntersect:
			switch (tData.aType)
			{
			default:
				return 22;
			case ApproachNodeType.CloseApproachOwn:
			case ApproachNodeType.IntersectOwn:
				return 3;
			case ApproachNodeType.CloseApproachOther:
			case ApproachNodeType.IntersectOther:
				return 4;
			}
		case MapObject.ObjectType.CelestialBodyAtUT:
			return 16;
		case MapObject.ObjectType.PatchTransition:
			switch (tData.pType)
			{
			default:
				return 22;
			case PatchTransitionNodeType.Encounter:
			case PatchTransitionNodeType.EncounterNextPatch:
				return 9;
			case PatchTransitionNodeType.Escape:
			case PatchTransitionNodeType.EscapeNextPatch:
				return 10;
			case PatchTransitionNodeType.Impact:
				return 12;
			}
		case MapObject.ObjectType.Generic:
		case MapObject.ObjectType.MENode:
			return 16;
		case MapObject.ObjectType.Site:
			return tData.sType switch
			{
				SiteType.LaunchSite => 25, 
				SiteType.Runway => 26, 
				SiteType.const_2 => 27, 
				_ => 25, 
			};
		}
	}

	public void SetIcon(int iconIndex)
	{
		img.sprite = iconSprites[iconIndex];
		switch (iconIndex)
		{
		case 2:
			AttachLabel(new Vector2(0f, 10f), cacheAutoLOC_6003115, 11);
			break;
		case 1:
			AttachLabel(new Vector2(10f, 0f), cacheAutoLOC_6003113, 11);
			break;
		case 17:
			AttachLabel(new Vector2(0f, 10f), cacheAutoLOC_6003116, 11);
			break;
		case 8:
			AttachLabel(new Vector2(-10f, 0f), cacheAutoLOC_6003114, 11);
			break;
		}
	}

	public void AttachLabel(Vector2 offset, string textMessage, int fontSize)
	{
		TextMeshProUGUI textMeshProUGUI = UnityEngine.Object.Instantiate(new GameObject("iconLabel"), base.transform, worldPositionStays: false).AddComponent<TextMeshProUGUI>();
		textMeshProUGUI.rectTransform.anchorMin = new Vector2(0f, 0f);
		textMeshProUGUI.rectTransform.anchorMax = new Vector2(1f, 1f);
		textMeshProUGUI.rectTransform.pivot = new Vector2(0.5f, 0.5f);
		textMeshProUGUI.fontSize = fontSize;
		textMeshProUGUI.text = textMessage;
		textMeshProUGUI.rectTransform.sizeDelta = new Vector2(5f, 5f);
		textMeshProUGUI.font = labelFont;
		textMeshProUGUI.alignment = TextAlignmentOptions.Center;
		textMeshProUGUI.rectTransform.localPosition = new Vector3(offset.x, offset.y, -1f);
		textMeshProUGUI.color = Color.Lerp(vData.color, Color.white, 0.6f);
	}

	public void SetIcon(Sprite sprite, Material mat = null)
	{
		if (!(img == null))
		{
			UpdateType();
			img.sprite = sprite;
			if (mat != null)
			{
				img.material = mat;
			}
		}
	}

	public void SetBackground(Sprite sprite, Material mat = null)
	{
		if (!(bgImg == null))
		{
			bgImg.gameObject.SetActive(sprite != null);
			bgImg.sprite = sprite;
			if (mat != null)
			{
				bgImg.material = mat;
			}
		}
	}

	public void SetType(TypeData typeData)
	{
		tData = typeData;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_6003113 = Localizer.Format("#autoLOC_6003113");
		cacheAutoLOC_6003114 = Localizer.Format("#autoLOC_6003114");
		cacheAutoLOC_6003115 = Localizer.Format("#autoLOC_6003115");
		cacheAutoLOC_6003116 = Localizer.Format("#autoLOC_6003116");
	}
}

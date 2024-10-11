using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI.Screens.Flight;

public class KerbalPortraitGallery : MonoBehaviour
{
	public enum GalleryMode
	{
		IVA,
		EVA
	}

	public class ActiveCrewItem
	{
		public Kerbal kerbal;

		public KerbalEVA kerbalEVA;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ActiveCrewItem(Kerbal kerbal)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ActiveCrewItem(KerbalEVA kerbalEVA)
		{
			throw null;
		}
	}

	protected bool areaHover;

	public KerbalPortrait portraitPrefab;

	private bool toggleIVAOverlayOnWhenIVA;

	[SerializeField]
	private GalleryMode _portraitGalleryMode;

	protected List<ActiveCrewItem> activeCrew;

	[SerializeField]
	protected List<KerbalPortrait> portraits;

	[SerializeField]
	protected Toggle IVAOverlayButton;

	protected InternalSpaceOverlay ivaOverlay;

	public static int GalleryCapacity;

	public static int GalleryMaxSize;

	[SerializeField]
	protected float PortraitWidth;

	[SerializeField]
	protected float PortraitHeight;

	[SerializeField]
	protected float leftScreenEdge;

	[SerializeField]
	protected float leftEdgePadding;

	[SerializeField]
	protected Button btnRight;

	[SerializeField]
	protected Button btnLeft;

	[SerializeField]
	protected LayoutElement portraitViewport;

	[SerializeField]
	protected RectTransform portraitLayoutParent;

	[SerializeField]
	protected UIPanelTweener portraitContainer;

	[SerializeField]
	protected Button btnAddSlot;

	[SerializeField]
	protected TextMeshProUGUI btnAddText;

	[SerializeField]
	protected Button btnRemSlot;

	[SerializeField]
	protected TextMeshProUGUI btnRemText;

	[SerializeField]
	protected XSelectable hoverArea;

	protected int firstPortrait;

	protected Coroutine resetCoroutine;

	protected Coroutine refreshCoroutine;

	protected float usableSpace;

	protected bool dirty;

	protected CameraManager.CameraMode lastMode;

	public static KerbalPortraitGallery Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	protected GalleryMode portraitGalleryMode
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

	public GalleryMode PortraitGalleryMode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[Obsolete]
	public List<Kerbal> ActiveCrew
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

	public List<ActiveCrewItem> ActiveCrewItems
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

	public List<KerbalPortrait> Portraits
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

	internal int countPortraits
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	protected int lastPortrait
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool ContainerTransitioning
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool isIVAOverlayVisible
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerbalPortraitGallery()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static KerbalPortraitGallery()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCommandSeatInteraction(KerbalEVA kerbal, bool entering)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void StartReset(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void StartRefresh(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void onVesselWasModified(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void onCrewTransferred(GameEvents.HostedFromToAction<ProtoCrewMember, Part> data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnCameraChange(CameraManager.CameraMode m)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnKerbalLevelUp(ProtoCrewMember pcm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerbalPortrait RegisterActiveCrew(Kerbal crewMember)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerbalPortrait RegisterActiveCrew(KerbalEVA crewMember)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerbalPortrait RegisterActiveCrew(KerbalEVA crewMember, bool spawnPortrait)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UnregisterActiveCrew(Kerbal crewMember)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UnregisterActiveCrew(KerbalEVA crewMember)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerbalPortrait SpawnPortrait(Kerbal crewMember)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerbalPortrait SpawnPortrait(KerbalEVA crewMember)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DespawnPortrait(Kerbal crewMember)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DespawnPortrait(KerbalEVA crewMember)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DespawnPortrait(KerbalPortrait portrait)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DespawnInactivePortraits()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckInvalidActiveCrew()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetActivePortraitsForVessel(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdatePortrait(Kerbal kerbal)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdatePortrait(KerbalEVA kerbal)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerbalPortrait GetPortrait(Kerbal crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerbalPortrait GetPortrait(KerbalEVA crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearPortraits()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HoverArea_onPointerExit(XSelectable arg1, PointerEventData arg2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void HoverArea_onPointerEnter(XSelectable arg1, PointerEventData arg2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void onBtnRight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void onBtnLeft()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void onBtnAddSlot()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void onBtnRemSlot()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void onIVAOverlayPress(bool st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ToggleIVAOverlay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void onIVAOverlayDismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UIControlsUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdatePortraitScrolling(int pIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetMaxGalleryCapacity(float leftEdge, float padding, float portraitWidth)
	{
		throw null;
	}
}

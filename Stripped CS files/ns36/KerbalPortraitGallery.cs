using System;
using System.Collections.Generic;
using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns36;

public class KerbalPortraitGallery : MonoBehaviour
{
	public enum GalleryMode
	{
		const_0,
		const_1
	}

	public class ActiveCrewItem
	{
		public Kerbal kerbal;

		public KerbalEVA kerbalEVA;

		public ActiveCrewItem(Kerbal kerbal)
		{
			this.kerbal = kerbal;
		}

		public ActiveCrewItem(KerbalEVA kerbalEVA)
		{
			this.kerbalEVA = kerbalEVA;
		}
	}

	public bool areaHover;

	public KerbalPortrait portraitPrefab;

	public bool toggleIVAOverlayOnWhenIVA;

	[SerializeField]
	public GalleryMode _portraitGalleryMode;

	public List<ActiveCrewItem> activeCrew = new List<ActiveCrewItem>();

	[SerializeField]
	public List<KerbalPortrait> portraits = new List<KerbalPortrait>();

	[SerializeField]
	public Toggle IVAOverlayButton;

	public InternalSpaceOverlay ivaOverlay;

	public static int GalleryCapacity = 3;

	public static int GalleryMaxSize = 3;

	[SerializeField]
	public float PortraitWidth = 128f;

	[SerializeField]
	public float PortraitHeight = 156f;

	[SerializeField]
	public float leftScreenEdge = 0.5f;

	[SerializeField]
	public float leftEdgePadding = 200f;

	[SerializeField]
	public Button btnRight;

	[SerializeField]
	public Button btnLeft;

	[SerializeField]
	public LayoutElement portraitViewport;

	[SerializeField]
	public RectTransform portraitLayoutParent;

	[SerializeField]
	public UIPanelTweener portraitContainer;

	[SerializeField]
	public Button btnAddSlot;

	[SerializeField]
	public TextMeshProUGUI btnAddText;

	[SerializeField]
	public Button btnRemSlot;

	[SerializeField]
	public TextMeshProUGUI btnRemText;

	[SerializeField]
	public XSelectable hoverArea;

	public int firstPortrait;

	public Coroutine resetCoroutine;

	public Coroutine refreshCoroutine;

	public float usableSpace;

	public bool dirty;

	public CameraManager.CameraMode lastMode;

	public static KerbalPortraitGallery Instance { get; set; }

	public GalleryMode portraitGalleryMode
	{
		get
		{
			return _portraitGalleryMode;
		}
		set
		{
			if (value == GalleryMode.const_0)
			{
				if (IVAOverlayButton != null)
				{
					if (toggleIVAOverlayOnWhenIVA)
					{
						IVAOverlayButton.isOn = true;
						toggleIVAOverlayOnWhenIVA = false;
					}
					IVAOverlayButton.gameObject.SetActive(value: true);
				}
				btnRemText.text = Localizer.Format("#autoLOC_7003304");
				btnAddText.text = Localizer.Format("#autoLOC_900612");
			}
			else
			{
				if (IVAOverlayButton != null)
				{
					if (IVAOverlayButton.isOn)
					{
						toggleIVAOverlayOnWhenIVA = true;
						IVAOverlayButton.isOn = false;
					}
					IVAOverlayButton.gameObject.SetActive(value: false);
				}
				btnRemText.text = Localizer.Format("#autoLOC_8002335");
				btnAddText.text = Localizer.Format("#autoLOC_8002336");
			}
			_portraitGalleryMode = value;
		}
	}

	public GalleryMode PortraitGalleryMode => portraitGalleryMode;

	[Obsolete]
	public List<Kerbal> ActiveCrew
	{
		get
		{
			List<Kerbal> list = new List<Kerbal>();
			for (int i = 0; i < activeCrew.Count; i++)
			{
				if (activeCrew[i].kerbal != null)
				{
					list.Add(activeCrew[i].kerbal);
				}
			}
			return list;
		}
		set
		{
			int count = activeCrew.Count;
			while (count-- > 0)
			{
				if (activeCrew[count].kerbal != null)
				{
					activeCrew.RemoveAt(count);
				}
			}
			for (int i = 0; i < value.Count; i++)
			{
				ActiveCrewItem item = new ActiveCrewItem(value[i]);
				activeCrew.Add(item);
			}
		}
	}

	public List<ActiveCrewItem> ActiveCrewItems
	{
		get
		{
			return activeCrew;
		}
		set
		{
			activeCrew = value;
		}
	}

	public List<KerbalPortrait> Portraits
	{
		get
		{
			return portraits;
		}
		set
		{
			portraits = value;
			int num = Math.Max(portraits.Count - 1, 0);
			if (firstPortrait > num)
			{
				firstPortrait = num;
			}
			else if (firstPortrait < 0)
			{
				firstPortrait = 0;
			}
		}
	}

	public int countPortraits => GalleryCapacity;

	public int lastPortrait => Mathf.Max(0, portraits.Count - GalleryCapacity);

	public bool ContainerTransitioning => portraitContainer.Transitioning;

	public static bool isIVAOverlayVisible
	{
		get
		{
			if (Instance != null)
			{
				return Instance.ivaOverlay != null;
			}
			return false;
		}
	}

	public void Awake()
	{
		Instance = this;
		btnRight.onClick.AddListener(onBtnRight);
		btnLeft.onClick.AddListener(onBtnLeft);
		btnAddSlot.onClick.AddListener(onBtnAddSlot);
		btnRemSlot.onClick.AddListener(onBtnRemSlot);
		if (IVAOverlayButton != null)
		{
			IVAOverlayButton.onValueChanged.AddListener(onIVAOverlayPress);
		}
		GameEvents.onVesselChange.Add(StartReset);
		GameEvents.onCommandSeatInteraction.Add(OnCommandSeatInteraction);
		GameEvents.onCrewTransferred.Add(onCrewTransferred);
		GameEvents.OnCameraChange.Add(OnCameraChange);
		GameEvents.onKerbalLevelUp.Add(OnKerbalLevelUp);
	}

	public void Start()
	{
		hoverArea.onPointerEnter += HoverArea_onPointerEnter;
		hoverArea.onPointerExit += HoverArea_onPointerExit;
	}

	public void Update()
	{
		if (dirty)
		{
			if (this != null)
			{
				UIControlsUpdate();
			}
			dirty = false;
		}
	}

	public void OnDestroy()
	{
		dirty = false;
		GameEvents.onVesselChange.Remove(StartReset);
		GameEvents.onCrewTransferred.Remove(onCrewTransferred);
		GameEvents.OnCameraChange.Remove(OnCameraChange);
		GameEvents.onKerbalLevelUp.Remove(OnKerbalLevelUp);
		GameEvents.onCommandSeatInteraction.Remove(OnCommandSeatInteraction);
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void OnDisable()
	{
		GameEvents.onVesselWasModified.Remove(onVesselWasModified);
	}

	public void OnEnable()
	{
		GameEvents.onVesselWasModified.Add(onVesselWasModified);
	}

	public void OnCommandSeatInteraction(KerbalEVA kerbal, bool entering)
	{
		if (FlightGlobals.ActiveVessel != null && kerbal.vessel.id == FlightGlobals.ActiveVessel.id)
		{
			StartRefresh(kerbal.vessel);
		}
	}

	public void StartReset(Vessel v)
	{
		if (!(v == null))
		{
			ClearPortraits();
			if (resetCoroutine != null)
			{
				StopCoroutine(resetCoroutine);
			}
			resetCoroutine = StartCoroutine(CallbackUtil.DelayedCallback(3, delegate
			{
				SetActivePortraitsForVessel(v);
				resetCoroutine = null;
			}));
		}
	}

	public void StartRefresh(Vessel v)
	{
		if (!(v == null))
		{
			if (refreshCoroutine != null)
			{
				StopCoroutine(refreshCoroutine);
			}
			refreshCoroutine = StartCoroutine(CallbackUtil.DelayedCallback(3, delegate
			{
				CheckInvalidActiveCrew();
				DespawnInactivePortraits();
				SetActivePortraitsForVessel(v);
				refreshCoroutine = null;
			}));
		}
	}

	public void onVesselWasModified(Vessel v)
	{
		if (v == FlightGlobals.ActiveVessel)
		{
			StartRefresh(FlightGlobals.ActiveVessel);
		}
	}

	public void onCrewTransferred(GameEvents.HostedFromToAction<ProtoCrewMember, Part> data)
	{
		StartReset(data.to.vessel);
	}

	public void OnCameraChange(CameraManager.CameraMode m)
	{
		if (m == CameraManager.CameraMode.Flight && lastMode != m)
		{
			StartRefresh(FlightGlobals.ActiveVessel);
		}
		lastMode = m;
	}

	public void OnKerbalLevelUp(ProtoCrewMember pcm)
	{
		int num = 0;
		while (true)
		{
			if (num < portraits.Count)
			{
				if (portraits[num].crewPcm != null && portraits[num].crewPcm.name == pcm.name)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		if (portraits[num].PortraitMode == GalleryMode.const_0)
		{
			UpdatePortrait(portraits[num].crewMember);
		}
		else
		{
			UpdatePortrait(portraits[num].crewEVAMember);
		}
	}

	public KerbalPortrait RegisterActiveCrew(Kerbal crewMember)
	{
		ActiveCrewItem item = new ActiveCrewItem(crewMember);
		activeCrew.Add(item);
		return SpawnPortrait(crewMember);
	}

	public KerbalPortrait RegisterActiveCrew(KerbalEVA crewMember)
	{
		return RegisterActiveCrew(crewMember, spawnPortrait: true);
	}

	public KerbalPortrait RegisterActiveCrew(KerbalEVA crewMember, bool spawnPortrait)
	{
		ActiveCrewItem item = new ActiveCrewItem(crewMember);
		activeCrew.Add(item);
		if (spawnPortrait)
		{
			return SpawnPortrait(crewMember);
		}
		return null;
	}

	public void UnregisterActiveCrew(Kerbal crewMember)
	{
		if (crewMember.state != Kerbal.States.DEAD)
		{
			DespawnPortrait(crewMember);
		}
		int count = activeCrew.Count;
		do
		{
			if (count-- <= 0)
			{
				return;
			}
		}
		while (!(activeCrew[count].kerbal != null) || !(activeCrew[count].kerbal == crewMember));
		activeCrew.RemoveAt(count);
	}

	public void UnregisterActiveCrew(KerbalEVA crewMember)
	{
		if (crewMember.part != null && crewMember.part.protoModuleCrew.Count > 0 && crewMember.part.protoModuleCrew[0].rosterStatus != ProtoCrewMember.RosterStatus.Dead)
		{
			DespawnPortrait(crewMember);
		}
	}

	public KerbalPortrait SpawnPortrait(Kerbal crewMember)
	{
		KerbalPortrait kerbalPortrait = UnityEngine.Object.Instantiate(portraitPrefab);
		kerbalPortrait.Setup(crewMember, portraitViewport.transform as RectTransform);
		kerbalPortrait.transform.SetParent(portraitLayoutParent, worldPositionStays: false);
		portraits.Add(kerbalPortrait);
		return kerbalPortrait;
	}

	public KerbalPortrait SpawnPortrait(KerbalEVA crewMember)
	{
		KerbalPortrait kerbalPortrait = UnityEngine.Object.Instantiate(portraitPrefab);
		kerbalPortrait.Setup(null, crewMember, portraitViewport.transform as RectTransform);
		kerbalPortrait.transform.SetParent(portraitLayoutParent, worldPositionStays: false);
		portraits.Add(kerbalPortrait);
		return kerbalPortrait;
	}

	public void DespawnPortrait(Kerbal crewMember)
	{
		int count = portraits.Count;
		KerbalPortrait kerbalPortrait;
		while (true)
		{
			if (count-- > 0)
			{
				kerbalPortrait = portraits[count];
				if (kerbalPortrait == null)
				{
					portraits.RemoveAt(count);
					dirty = true;
				}
				else if (kerbalPortrait.gameObject == null)
				{
					portraits.RemoveAt(count);
					dirty = true;
				}
				else if (kerbalPortrait.crewMember == crewMember)
				{
					break;
				}
				continue;
			}
			return;
		}
		DespawnPortrait(kerbalPortrait);
		dirty = true;
	}

	public void DespawnPortrait(KerbalEVA crewMember)
	{
		int count = portraits.Count;
		KerbalPortrait kerbalPortrait;
		while (true)
		{
			if (count-- > 0)
			{
				kerbalPortrait = portraits[count];
				if (kerbalPortrait == null)
				{
					portraits.RemoveAt(count);
					dirty = true;
				}
				else if (kerbalPortrait.gameObject == null)
				{
					portraits.RemoveAt(count);
					dirty = true;
				}
				else if (kerbalPortrait.crewEVAMember == crewMember)
				{
					break;
				}
				continue;
			}
			return;
		}
		DespawnPortrait(kerbalPortrait);
		dirty = true;
	}

	public void DespawnPortrait(KerbalPortrait portrait)
	{
		UnityEngine.Object.Destroy(portrait.gameObject);
		portraits.Remove(portrait);
		int num = Math.Max(portraits.Count - 1, 0);
		if (firstPortrait > num)
		{
			firstPortrait = num;
		}
		else if (firstPortrait < 0)
		{
			firstPortrait = 0;
		}
	}

	public void DespawnInactivePortraits()
	{
		int count = portraits.Count;
		while (count-- > 0)
		{
			KerbalPortrait kerbalPortrait = portraits[count];
			if (kerbalPortrait != null && (kerbalPortrait.crewMember == null || kerbalPortrait.crewPcm == null))
			{
				DespawnPortrait(kerbalPortrait);
			}
		}
		UIControlsUpdate();
	}

	public void CheckInvalidActiveCrew()
	{
		int count = activeCrew.Count;
		while (count-- > 0)
		{
			if (activeCrew[count].kerbal == null && activeCrew[count].kerbalEVA == null)
			{
				activeCrew.RemoveAt(count);
			}
		}
	}

	public void SetActivePortraitsForVessel(Vessel v)
	{
		if (v == null)
		{
			return;
		}
		if (v.isEVA)
		{
			portraitGalleryMode = GalleryMode.const_1;
		}
		else
		{
			portraitGalleryMode = GalleryMode.const_0;
		}
		if (portraitGalleryMode == GalleryMode.const_0)
		{
			if (activeCrew.Count < GalleryCapacity)
			{
				firstPortrait = 0;
			}
			else
			{
				firstPortrait = Mathf.Min(firstPortrait + GalleryCapacity, activeCrew.Count) - GalleryCapacity;
			}
			int count = activeCrew.Count;
			HashSet<Part> hashSet = new HashSet<Part>();
			for (int i = 0; i < count; i++)
			{
				if (activeCrew[i].kerbal != null)
				{
					hashSet.Add(activeCrew[i].kerbal.InPart);
				}
			}
			v.SetActiveInternalSpaces(hashSet);
			for (int j = 0; j < count; j++)
			{
				bool flag;
				KerbalPortrait kerbalPortrait = ((flag = activeCrew[j].kerbal != null) ? GetPortrait(activeCrew[j].kerbal) : GetPortrait(activeCrew[j].kerbalEVA));
				if (kerbalPortrait == null)
				{
					if (flag)
					{
						SpawnPortrait(activeCrew[j].kerbal);
					}
					else if (activeCrew[j].kerbalEVA.vessel.id == v.id)
					{
						SpawnPortrait(activeCrew[j].kerbalEVA);
					}
				}
				else if (flag)
				{
					kerbalPortrait.Setup(activeCrew[j].kerbal, portraitViewport.transform as RectTransform);
				}
				else if (activeCrew[j].kerbalEVA.vessel.id == v.id && activeCrew[j].kerbalEVA != null)
				{
					kerbalPortrait.Setup(null, activeCrew[j].kerbalEVA, portraitViewport.transform as RectTransform);
				}
				else
				{
					DespawnPortrait(kerbalPortrait);
				}
			}
		}
		else
		{
			firstPortrait = 0;
			KerbalEVA kerbalEVA = null;
			if (v.parts.Count > 0)
			{
				kerbalEVA = v.parts[0].FindModuleImplementing<KerbalEVA>();
			}
			if (kerbalEVA != null)
			{
				KerbalPortrait portrait = GetPortrait(kerbalEVA);
				if (portrait == null)
				{
					SpawnPortrait(kerbalEVA);
				}
				else
				{
					portrait.Setup(null, kerbalEVA, portraitViewport.transform as RectTransform);
				}
			}
		}
		UpdatePortraitScrolling(firstPortrait);
		UIControlsUpdate();
	}

	public void UpdatePortrait(Kerbal kerbal)
	{
		KerbalPortrait portrait = GetPortrait(kerbal);
		if (portrait != null)
		{
			portrait.roleLevelImage.sprite = portrait.lvlSprites[kerbal.protoCrewMember.experienceLevel];
		}
	}

	public void UpdatePortrait(KerbalEVA kerbal)
	{
		KerbalPortrait portrait = GetPortrait(kerbal);
		if (portrait != null)
		{
			portrait.roleLevelImage.sprite = portrait.lvlSprites[kerbal.part.protoModuleCrew[0].experienceLevel];
		}
	}

	public KerbalPortrait GetPortrait(Kerbal crew)
	{
		int count = portraits.Count;
		do
		{
			if (count-- <= 0)
			{
				return null;
			}
		}
		while (!(portraits[count].crewMember == crew));
		return portraits[count];
	}

	public KerbalPortrait GetPortrait(KerbalEVA crew)
	{
		int count = portraits.Count;
		do
		{
			if (count-- <= 0)
			{
				return null;
			}
		}
		while (!(portraits[count].crewEVAMember == crew));
		return portraits[count];
	}

	public void ClearPortraits()
	{
		int count = portraits.Count;
		while (count-- > 0)
		{
			DespawnPortrait(portraits[count]);
		}
		portraits.Clear();
		firstPortrait = 0;
		UIControlsUpdate();
	}

	public void HoverArea_onPointerExit(XSelectable arg1, PointerEventData arg2)
	{
		areaHover = false;
		UIControlsUpdate();
	}

	public void HoverArea_onPointerEnter(XSelectable arg1, PointerEventData arg2)
	{
		areaHover = true;
		UIControlsUpdate();
	}

	public void onBtnRight()
	{
		firstPortrait = Mathf.Min(firstPortrait + 1, Mathf.Max(0, portraits.Count - GalleryCapacity));
		UpdatePortraitScrolling(firstPortrait);
	}

	public void onBtnLeft()
	{
		firstPortrait = Mathf.Max(firstPortrait - 1, 0);
		UpdatePortraitScrolling(firstPortrait);
	}

	public void onBtnAddSlot()
	{
		if (portraitGalleryMode == GalleryMode.const_0)
		{
			GalleryCapacity = Mathf.Min(Mathf.Min(GalleryCapacity + 1, portraits.Count), GalleryMaxSize);
			firstPortrait = Mathf.Min(firstPortrait, Mathf.Max(0, portraits.Count - GalleryCapacity));
		}
		else
		{
			GalleryCapacity = 1;
			firstPortrait = 0;
		}
		UIControlsUpdate();
		UpdatePortraitScrolling(firstPortrait);
	}

	public void onBtnRemSlot()
	{
		if (portraitGalleryMode == GalleryMode.const_0)
		{
			GalleryCapacity = Mathf.Max(Mathf.Min(GalleryCapacity, portraits.Count) - 1, 0);
			firstPortrait = Mathf.Min(firstPortrait, Mathf.Max(0, portraits.Count - GalleryCapacity));
		}
		else
		{
			GalleryCapacity = 0;
			firstPortrait = 0;
		}
		UIControlsUpdate();
		UpdatePortraitScrolling(firstPortrait);
	}

	public void onIVAOverlayPress(bool st)
	{
		if (portraitGalleryMode == GalleryMode.const_0)
		{
			if (st && ivaOverlay == null)
			{
				ivaOverlay = InternalSpaceOverlay.Create(FlightGlobals.ActiveVessel, onIVAOverlayDismiss);
			}
			else if (ivaOverlay != null)
			{
				ivaOverlay.Dismiss();
			}
		}
	}

	public static void ToggleIVAOverlay()
	{
		if (!(Instance == null) && Instance.portraitGalleryMode == GalleryMode.const_0)
		{
			bool isOn = !Instance.IVAOverlayButton.isOn;
			Instance.IVAOverlayButton.isOn = isOn;
		}
	}

	public void onIVAOverlayDismiss()
	{
		if (portraitGalleryMode != 0)
		{
			return;
		}
		IVAOverlayButton.isOn = false;
		if (!(this == null))
		{
			StartCoroutine(CallbackUtil.DelayedCallback(2, delegate
			{
				SetActivePortraitsForVessel((FlightGlobals.fetch == null) ? null : FlightGlobals.fetch.activeVessel);
			}));
		}
	}

	public void UIControlsUpdate()
	{
		dirty = false;
		GalleryMaxSize = GetMaxGalleryCapacity(leftScreenEdge, leftEdgePadding, PortraitWidth);
		GalleryCapacity = Mathf.Min(GalleryCapacity, GalleryMaxSize);
		if (portraitGalleryMode == GalleryMode.const_0)
		{
			if (portraits.Count > GalleryCapacity && GalleryCapacity > 0)
			{
				btnLeft.gameObject.SetActive(value: true);
				btnRight.gameObject.SetActive(value: true);
			}
			else
			{
				btnLeft.gameObject.SetActive(value: false);
				btnRight.gameObject.SetActive(value: false);
			}
			btnLeft.interactable = firstPortrait > 0;
			btnRight.interactable = firstPortrait < lastPortrait;
			float preferredWidth = (float)Mathf.Min(portraits.Count, GalleryCapacity) * PortraitWidth;
			portraitViewport.preferredWidth = preferredWidth;
			portraitViewport.preferredHeight = PortraitHeight;
		}
		else
		{
			btnLeft.gameObject.SetActive(value: false);
			btnRight.gameObject.SetActive(value: false);
			if (GalleryCapacity == 0)
			{
				portraitViewport.preferredWidth = 0f;
				portraitViewport.preferredHeight = PortraitHeight;
				for (int i = 0; i < portraits.Count; i++)
				{
					portraits[i].portrait.enabled = false;
				}
			}
			else
			{
				float preferredWidth2 = (float)Mathf.Min(portraits.Count, GalleryCapacity) * PortraitWidth;
				portraitViewport.preferredWidth = preferredWidth2;
				portraitViewport.preferredHeight = PortraitHeight;
				for (int j = 0; j < portraits.Count; j++)
				{
					portraits[j].portrait.enabled = GameSettings.EVA_SHOW_PORTRAIT;
				}
			}
		}
		if (areaHover)
		{
			bool flag = portraits.Count > 0;
			bool flag2 = activeCrew.Count > GalleryCapacity;
			bool flag3 = activeCrew.Count > 0 && GalleryCapacity > 0;
			if (portraitGalleryMode == GalleryMode.const_0)
			{
				IVAOverlayButton.gameObject.SetActive(flag);
				btnAddSlot.gameObject.SetActive(flag && flag2);
				btnRemSlot.gameObject.SetActive(flag && flag3);
				IVAOverlayButton.interactable = true;
				btnAddSlot.interactable = GalleryCapacity < GalleryMaxSize;
				btnRemSlot.interactable = GalleryCapacity > 0;
			}
			else
			{
				IVAOverlayButton.gameObject.SetActive(value: false);
				btnAddSlot.gameObject.SetActive(GalleryCapacity == 0);
				btnRemSlot.gameObject.SetActive(GalleryCapacity > 0);
				btnAddSlot.interactable = true;
				btnRemSlot.interactable = true;
			}
		}
		else
		{
			IVAOverlayButton.gameObject.SetActive(value: false);
			if (portraitGalleryMode == GalleryMode.const_0)
			{
				btnAddSlot.gameObject.SetActive(GalleryCapacity == 0 && activeCrew.Count > 0);
				btnRemSlot.gameObject.SetActive(value: false);
			}
			else
			{
				btnAddSlot.gameObject.SetActive(GalleryCapacity == 0);
				btnRemSlot.gameObject.SetActive(GalleryCapacity > 0);
			}
		}
	}

	public void UpdatePortraitScrolling(int pIndex)
	{
		portraitContainer.Transition(Vector2.left * (PortraitWidth * (float)pIndex), inputLockAfterTransition: false);
		btnLeft.interactable = pIndex > 0;
		btnRight.interactable = pIndex < lastPortrait;
	}

	public int GetMaxGalleryCapacity(float leftEdge, float padding, float portraitWidth)
	{
		usableSpace = (float)Screen.width - (float)Screen.width * leftEdge - padding;
		return Mathf.FloorToInt(usableSpace / portraitWidth);
	}
}

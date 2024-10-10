using System.Collections;
using ns12;
using ns16;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns36;

public class KerbalPortrait : MonoBehaviour
{
	[SerializeField]
	public KerbalPortraitGallery.GalleryMode portraitMode;

	public RawImage portrait;

	public Material portraitMaterial;

	public XSelectable hoverArea;

	public RectContainmentDetector rectContainment;

	public TextMeshProUGUI nameField;

	public GameObject hoverObjectsContainer;

	public GameObject roleObjectsContainer;

	public GameObject geeMeterContainer;

	public Button ivaButton;

	public Button evaButton;

	public TextMeshProUGUI roleText;

	public Image roleLevelImage;

	public Sprite[] lvlSprites;

	public TooltipController_Text ivaTooltip;

	public TooltipController_Text evaTooltip;

	public TooltipController_Text geeMeterTooltip;

	public Texture2D overlayImg;

	public bool overlayUp;

	public float overlayFrame;

	public Vector2 overlayUVOffset;

	[SerializeField]
	public bool visible;

	public Coroutine overlayUpdateCoroutine;

	public WaitForSeconds overlayUpdateYield;

	public float overlayUpdateInterval = 0.1f;

	public bool eventSetupDone;

	public Image geeMeterImage;

	public TooltipController_CrewAC tooltip;

	public static string cacheAutoLOC_459483;

	public static string cacheAutoLOC_459494;

	public KerbalPortraitGallery.GalleryMode PortraitMode => portraitMode;

	public Kerbal crewMember { get; set; }

	public KerbalEVA crewEVAMember { get; set; }

	public ProtoCrewMember crewPcm
	{
		get
		{
			if (portraitMode == KerbalPortraitGallery.GalleryMode.const_0)
			{
				if (crewMember != null)
				{
					return crewMember.protoCrewMember;
				}
			}
			else if (crewEVAMember != null && crewEVAMember.part != null && crewEVAMember.part.protoModuleCrew.Count > 0)
			{
				return crewEVAMember.part.protoModuleCrew[0];
			}
			return null;
		}
	}

	public string crewMemberName { get; set; }

	public void Setup(Kerbal kerbal, RectTransform containerViewport)
	{
		Setup(kerbal, null, containerViewport);
	}

	public void Setup(Kerbal kerbal, KerbalEVA kerbalEVA, RectTransform containerViewport)
	{
		crewMember = kerbal;
		crewEVAMember = kerbalEVA;
		if (portrait != null)
		{
			portraitMaterial = portrait.material;
		}
		if (crewMember == null)
		{
			portraitMode = KerbalPortraitGallery.GalleryMode.const_1;
		}
		else
		{
			portraitMode = KerbalPortraitGallery.GalleryMode.const_0;
			if (portrait != null)
			{
				portrait.material = null;
			}
		}
		if (crewPcm == null)
		{
			return;
		}
		crewMemberName = crewPcm.name;
		nameField.text = crewMemberName;
		roleText.text = crewPcm.experienceTrait.TypeName;
		roleLevelImage.sprite = lvlSprites[crewPcm.experienceLevel];
		portrait.texture = overlayImg;
		OverlayUpdate(displayOverlay: true, 2f);
		if (!eventSetupDone)
		{
			if (portraitMode == KerbalPortraitGallery.GalleryMode.const_0)
			{
				evaButton.onClick.AddListener(ClickEVA);
				ivaButton.onClick.AddListener(ClickIVA);
				rectContainment.OnContainmentChanged += RectContainment_OnContainmentChanged;
				evaTooltip.continuousUpdate = true;
				evaTooltip.RequireInteractable = false;
				ivaTooltip.continuousUpdate = true;
				overlayUpdateInterval = kerbal.updateInterval;
			}
			else
			{
				evaButton.gameObject.SetActive(value: false);
				ivaButton.gameObject.SetActive(value: false);
				rectContainment.OnContainmentChanged += RectContainment_OnContainmentChanged;
				overlayUpdateInterval = kerbalEVA.KerbalAvatarUpdateInterval;
			}
			eventSetupDone = true;
		}
		hoverObjectsContainer.SetActive(value: false);
		geeMeterContainer.SetActive(HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().GKerbalLimits);
		rectContainment.container = containerViewport;
		overlayUpdateYield = new WaitForSeconds(overlayUpdateInterval);
		if (portraitMode == KerbalPortraitGallery.GalleryMode.const_1)
		{
			overlayUpdateCoroutine = StartCoroutine(OnOverlayUpdate());
			if (!GameSettings.EVA_SHOW_PORTRAIT)
			{
				portrait.enabled = false;
			}
		}
		if (portraitMode == KerbalPortraitGallery.GalleryMode.const_0 && kerbal.state != Kerbal.States.ALIVE)
		{
			overlayUpdateCoroutine = StartCoroutine(OnOverlayUpdate(kerbal));
		}
		tooltip.SetTooltip(crewPcm);
	}

	public void ClickEVA()
	{
		if (crewMember != null && crewMember.state != 0)
		{
			CameraManager.Instance.SetCameraFlight();
			FlightEVA.SpawnEVA(crewMember);
			Mouse.Left.ClearMouseState();
		}
	}

	public void ClickIVA()
	{
		if (crewMember != null)
		{
			CameraManager.Instance.SetCameraIVA(crewMember, resetCamera: true);
		}
	}

	public void Update()
	{
		bool gKerbalLimits;
		if ((gKerbalLimits = HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().GKerbalLimits) != geeMeterContainer.activeSelf)
		{
			geeMeterContainer.SetActive(gKerbalLimits);
		}
		if (geeMeterContainer.activeSelf && crewPcm != null)
		{
			double gExperiencedNormalized = crewPcm.GExperiencedNormalized;
			geeMeterImage.fillAmount = Mathf.Clamp((float)gExperiencedNormalized, 0f, 1f);
			geeMeterTooltip.textString = Localizer.Format("#autoLOC_7003284", gExperiencedNormalized.ToString("P1"));
		}
		if (portraitMode == KerbalPortraitGallery.GalleryMode.const_1 && !GameSettings.EVA_SHOW_PORTRAIT)
		{
			if (!hoverObjectsContainer.activeSelf)
			{
				hoverObjectsContainer.SetActive(value: true);
			}
			portrait.enabled = false;
		}
		else
		{
			if (!hoverArea.Hover)
			{
				if (hoverObjectsContainer.activeSelf)
				{
					hoverObjectsContainer.SetActive(value: false);
				}
				return;
			}
			if (!hoverObjectsContainer.activeSelf)
			{
				hoverObjectsContainer.SetActive(value: true);
			}
		}
		if (portraitMode == KerbalPortraitGallery.GalleryMode.const_0)
		{
			if (CanIVA())
			{
				ivaTooltip.textString = cacheAutoLOC_459483;
				ivaButton.interactable = true;
			}
			else
			{
				ivaTooltip.textString = string.Empty;
				ivaButton.interactable = false;
			}
			if (CanEVA())
			{
				evaTooltip.textString = cacheAutoLOC_459494;
				evaButton.interactable = true;
			}
			else
			{
				evaTooltip.textString = GameVariables.Instance.GetEVALockedReason(FlightGlobals.ActiveVessel, crewMember.protoCrewMember);
				evaButton.interactable = false;
			}
		}
	}

	public void OnDisable()
	{
		OnBecameInvisible();
	}

	public void OnEnable()
	{
		if (eventSetupDone)
		{
			OnBecameVisible();
		}
	}

	public bool CanIVA()
	{
		if (hoverArea.Hover && crewMember.protoCrewMember.seat != null && crewMember.protoCrewMember.seat.internalModel != null)
		{
			return HighLogic.CurrentGame.Parameters.Flight.CanIVA;
		}
		return false;
	}

	public bool CanEVA()
	{
		bool evaUnlocked = GameVariables.Instance.UnlockedEVA(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.AstronautComplex));
		evaUnlocked = GameVariables.Instance.EVAIsPossible(evaUnlocked, FlightGlobals.ActiveVessel);
		return hoverArea.Hover && crewMember.state == Kerbal.States.ALIVE && !crewMember.InPart.packed && HighLogic.CurrentGame.Parameters.Flight.CanEVA && !crewMember.InPart.NoAutoEVA && !crewMember.protoCrewMember.inactive && crewMember.protoCrewMember.type != ProtoCrewMember.KerbalType.Tourist && evaUnlocked;
	}

	public void RectContainment_OnContainmentChanged(RectUtil.ContainmentLevel cLvl)
	{
		if (portraitMode == KerbalPortraitGallery.GalleryMode.const_1)
		{
			if (cLvl == RectUtil.ContainmentLevel.Partial)
			{
				if (visible)
				{
					portrait.material = null;
				}
			}
			else if (cLvl >= RectUtil.ContainmentLevel.Full)
			{
				OnBecameVisible();
			}
			else if (cLvl == RectUtil.ContainmentLevel.None)
			{
				OnBecameInvisible();
			}
		}
		else if (cLvl >= RectUtil.ContainmentLevel.Partial)
		{
			OnBecameVisible();
		}
		else
		{
			OnBecameInvisible();
		}
	}

	public void OnBecameVisible()
	{
		if (!visible)
		{
			visible = true;
			if (crewMember != null)
			{
				crewMember.SetVisibleInPortrait(visible: true);
			}
			if (crewEVAMember != null)
			{
				crewEVAMember.SetVisibleInPortrait(visible: true);
			}
			if (portraitMode == KerbalPortraitGallery.GalleryMode.const_1)
			{
				portrait.material = portraitMaterial;
			}
			if (overlayUpdateCoroutine == null)
			{
				overlayUpdateCoroutine = ((portraitMode == KerbalPortraitGallery.GalleryMode.const_0) ? StartCoroutine(OnOverlayUpdate(crewMember)) : StartCoroutine(OnOverlayUpdate()));
			}
		}
	}

	public void OnBecameInvisible()
	{
		if (visible)
		{
			visible = false;
			if (crewMember != null)
			{
				crewMember.SetVisibleInPortrait(visible: false);
			}
			if (crewEVAMember != null)
			{
				crewEVAMember.SetVisibleInPortrait(visible: false);
			}
			if (portraitMode == KerbalPortraitGallery.GalleryMode.const_1)
			{
				portrait.material = null;
			}
			if (overlayUpdateCoroutine != null)
			{
				StopCoroutine(overlayUpdateCoroutine);
				overlayUpdateCoroutine = null;
			}
		}
	}

	public IEnumerator OnOverlayUpdate()
	{
		while (true)
		{
			if (visible)
			{
				if (crewPcm != null && !crewPcm.inactive)
				{
					OverlayUpdate(displayOverlay: false, 3f);
				}
				else
				{
					OverlayUpdate(displayOverlay: true, 2f);
				}
			}
			yield return overlayUpdateYield;
		}
	}

	public IEnumerator OnOverlayUpdate(Kerbal kerbal)
	{
		while (true)
		{
			if (visible)
			{
				if (kerbal != null && !kerbal.protoCrewMember.inactive)
				{
					OverlayUpdate(kerbal.state != Kerbal.States.ALIVE, (float)kerbal.state);
				}
				else
				{
					OverlayUpdate(displayOverlay: true, 2f);
				}
			}
			yield return overlayUpdateYield;
		}
	}

	public void OverlayUpdate(bool displayOverlay, float state)
	{
		if (displayOverlay)
		{
			if (!overlayUp)
			{
				portrait.texture = overlayImg;
				overlayUp = true;
			}
			overlayFrame = (overlayFrame + 1f) % 3f;
			overlayUVOffset = new Vector2(overlayFrame / 3f, state / 3f);
			portrait.uvRect = new Rect(overlayUVOffset.x, overlayUVOffset.y, 0.3334f, 0.3334f);
		}
		else if (overlayUp)
		{
			if (portraitMode == KerbalPortraitGallery.GalleryMode.const_0)
			{
				portrait.texture = crewMember.avatarTexture;
			}
			else if (crewEVAMember != null)
			{
				portrait.texture = crewEVAMember.AvatarTexture;
			}
			portrait.uvRect = new Rect(0f, 0f, 1f, 1f);
			overlayUp = false;
		}
	}

	public void OnCrewDie()
	{
		if (KerbalPortraitGallery.Instance != null)
		{
			if (crewMember != null)
			{
				KerbalPortraitGallery.Instance.UnregisterActiveCrew(crewMember);
			}
			if (crewEVAMember != null)
			{
				KerbalPortraitGallery.Instance.UnregisterActiveCrew(crewEVAMember);
			}
		}
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_459483 = Localizer.Format("#autoLOC_459483");
		cacheAutoLOC_459494 = Localizer.Format("#autoLOC_459494");
	}
}

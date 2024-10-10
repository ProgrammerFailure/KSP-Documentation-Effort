using System.Collections;
using System.Collections.Generic;
using System.IO;
using Expansions.Missions.Runtime;
using ns13;
using ns2;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns11;

[RequireComponent(typeof(Button), typeof(PointerEnterExitHandler))]
public class EditorPartIcon : MonoBehaviour
{
	public Color experimentalPartColor = new Color32(128, 128, byte.MaxValue, byte.MaxValue);

	public Button btnSpawnPart;

	public Button btnRemove;

	public Button btnAdd;

	public Button btnSwapTexture;

	public Button btnPlacePart;

	public TextMeshProUGUI stackAmountText;

	public RawImage inventoryItemThumbnail;

	public Slider stackSlider;

	[SerializeField]
	public float placeOffsetWhenStack = 7f;

	public bool inInventory;

	public int inventorySlotIndex = -1;

	public PointerEnterExitHandler hoverHandler;

	public EditorPartList partList;

	public AvailablePart availablePart;

	public float iconSize;

	public float iconOverScale;

	public float iconOverSpin;

	public int variantIndex;

	public bool checkedExperimental;

	[SerializeField]
	public bool isCargoPart;

	[SerializeField]
	public bool isDeployablePart;

	public bool isFlag;

	public FlagDecalBackground flagDecalBackground;

	public Material[] materials;

	public Color missionRequiredPartColor = new Color32(128, 128, byte.MaxValue, byte.MaxValue);

	public bool checkedMissionRequired;

	public Callback<EditorPartIcon> PlacePartCallback;

	public PartIcon partIcon;

	public SpriteState buttonState;

	public bool isPart = true;

	public bool isEmptySlot;

	public PartUpgradeHandler.Upgrade upgrade;

	public UIPartActionInventorySlot UIPAIS;

	public WaitForEndOfFrame wfoef;

	public string thumbnailPath;

	public Image highlightImage;

	public RectTransform btnPlacePartRect;

	public Vector2 placeButtonPosition;

	public float placePositionXInitial;

	public bool changingDefaultVariant;

	public bool mouseOver;

	public bool stillFocused;

	public Vector3 partScale;

	public float partRotation;

	public Quaternion startRot;

	public AvailablePart AvailPart => availablePart;

	public bool IsCargoPart => isCargoPart;

	public bool IsDeployablePart => isDeployablePart;

	public AvailablePart partInfo => availablePart;

	public Transform PartIcon
	{
		get
		{
			if (partIcon != null && partIcon.Icon != null)
			{
				return partIcon.Icon.transform;
			}
			return null;
		}
	}

	public bool HasIconOrThumbnail
	{
		get
		{
			if (partIcon != null && partIcon.Icon != null)
			{
				return true;
			}
			if (inventoryItemThumbnail != null && inventoryItemThumbnail.texture != null)
			{
				return true;
			}
			return false;
		}
	}

	public bool MouseOver => mouseOver;

	public bool StillFocused => stillFocused;

	public bool Focused
	{
		get
		{
			if (!mouseOver)
			{
				return stillFocused;
			}
			return true;
		}
	}

	public bool isGrey { get; set; }

	public string greyoutToolTipMessage { get; set; }

	public void Awake()
	{
		partIcon = new PartIcon(null);
		hoverHandler = GetComponent<PointerEnterExitHandler>();
		hoverHandler.onPointerEnter.AddListener(MouseInput_PointerEnter);
		hoverHandler.onPointerExit.AddListener(MouseInput_PointerExit);
		if (btnSpawnPart != null)
		{
			buttonState = btnSpawnPart.spriteState;
			btnSpawnPart.onClick.AddListener(MouseInput_SpawnPart);
		}
		if (btnRemove != null)
		{
			btnRemove.gameObject.SetActive(value: false);
			btnRemove.onClick.AddListener(MouseInput_Delete);
		}
		if (btnAdd != null)
		{
			btnAdd.gameObject.SetActive(value: false);
			btnAdd.onClick.AddListener(MouseInput_Add);
		}
		if (btnPlacePart != null)
		{
			btnPlacePart.gameObject.SetActive(value: false);
			btnPlacePart.onClick.AddListener(MouseInput_PlacePart);
			btnPlacePartRect = btnPlacePart.gameObject.transform as RectTransform;
			placeButtonPosition = btnPlacePartRect.anchoredPosition;
			placePositionXInitial = placeButtonPosition.x;
		}
		if (stackAmountText != null)
		{
			stackAmountText.gameObject.SetActive(value: false);
		}
		if (stackSlider != null)
		{
			stackSlider.gameObject.SetActive(value: false);
		}
		if (inventoryItemThumbnail != null)
		{
			inventoryItemThumbnail.texture = null;
			inventoryItemThumbnail.gameObject.SetActive(value: false);
		}
		if (highlightImage != null && highlightImage.gameObject != null)
		{
			highlightImage.gameObject.SetActive(value: false);
		}
		greyoutToolTipMessage = "";
		isGrey = false;
		checkedExperimental = false;
		variantIndex = 0;
		wfoef = new WaitForEndOfFrame();
		GameEvents.onEditorDefaultVariantChanged.Add(OnEditorDefaultVariantChanged);
		GameEvents.onEditorPartEvent.Add(OnEditorPartEvent);
		GameEvents.OnInventoryPartOnMouseChanged.Add(OnInventoryPartOnMouseChanged);
	}

	public void OnEditorPartEvent(ConstructionEventType evt, Part p)
	{
		if (!(UIPAIS == null))
		{
			string partVariantName = ((p.variants != null) ? p.variants.SelectedVariant.Name : "");
			switch (evt)
			{
			case ConstructionEventType.PartDropped:
			case ConstructionEventType.PartAttached:
				highlightImage.gameObject.SetActive(value: false);
				break;
			case ConstructionEventType.PartCreated:
			case ConstructionEventType.PartPicked:
			case ConstructionEventType.PartDetached:
			case ConstructionEventType.PartCopied:
			case ConstructionEventType.PartOverInventoryGrid:
			{
				bool active = UIPAIS.CanStackInSlot(p.partInfo, partVariantName);
				highlightImage.gameObject.SetActive(active);
				break;
			}
			}
		}
	}

	public void OnInventoryPartOnMouseChanged(Part p)
	{
		if (!(UIPAIS == null))
		{
			if (p == null)
			{
				highlightImage.gameObject.SetActive(value: false);
				return;
			}
			string partVariantName = ((p.variants != null) ? p.variants.SelectedVariant.Name : "");
			bool active = UIPAIS.CanStackInSlot(p.partInfo, partVariantName);
			highlightImage.gameObject.SetActive(active);
		}
	}

	public void Start()
	{
		if (btnSpawnPart != null)
		{
			btnSpawnPart.interactable = !isGrey;
		}
	}

	public void OnDestroy()
	{
		if (partIcon != null && partIcon.Icon != null)
		{
			CleanUpMaterials(partIcon.Icon);
		}
		GameEvents.onEditorDefaultVariantChanged.Remove(OnEditorDefaultVariantChanged);
		GameEvents.onEditorPartEvent.Remove(OnEditorPartEvent);
		GameEvents.OnInventoryPartOnMouseChanged.Remove(OnInventoryPartOnMouseChanged);
		GameEvents.onFlagSelect.Remove(SetFlagBrowserTexture);
	}

	public void Create(EditorPartList partList, AvailablePart part, float iconSize, float iconOverScale, float iconOverSpin)
	{
		Create(partList, part, null, iconSize, iconOverScale, iconOverSpin, null, btnPlacePartActive: false, skipVariants: false, null, useImageThumbnail: false, inInventory: false);
	}

	public void Create(EditorPartList partList, AvailablePart part, float iconSize, float iconOverScale, float iconOverSpin, Callback<EditorPartIcon> placePartCallback, bool btnPlacePartActive)
	{
		Create(partList, part, null, iconSize, iconOverScale, iconOverSpin, placePartCallback, btnPlacePartActive, skipVariants: false, null, useImageThumbnail: false, inInventory: false);
	}

	public void Create(EditorPartList partList, AvailablePart part, StoredPart sPart, float iconSize, float iconOverScale, float iconOverSpin, Callback<EditorPartIcon> placePartCallback, bool btnPlacePartActive, bool skipVariants, PartVariant variant, bool useImageThumbnail, bool inInventory)
	{
		this.iconSize = iconSize;
		this.iconOverScale = iconOverScale;
		this.iconOverSpin = iconOverSpin;
		this.inInventory = inInventory;
		if (inInventory)
		{
			PartListTooltipController component = GetComponent<PartListTooltipController>();
			if ((bool)component)
			{
				component.enabled = false;
			}
		}
		if (placePartCallback != null)
		{
			PlacePartCallback = placePartCallback;
			if (HighLogic.LoadedSceneIsFlight && btnPlacePart != null && part != null && part.partPrefab != null)
			{
				isCargoPart = part.partPrefab.FindModuleImplementing<ModuleCargoPart>() != null;
				isDeployablePart = part.partPrefab.FindModuleImplementing<ModuleGroundPart>() != null;
				if (isDeployablePart && (!EVAConstructionModeController.Instance || !EVAConstructionModeController.Instance.IsOpen))
				{
					btnPlacePart.gameObject.SetActive(btnPlacePartActive);
				}
				else
				{
					btnPlacePart.gameObject.SetActive(value: false);
				}
			}
		}
		isEmptySlot = false;
		this.partList = partList;
		availablePart = part;
		if (!useImageThumbnail)
		{
			InstantiatePartIcon();
		}
		if (part != null && part.partPrefab != null)
		{
			flagDecalBackground = part.partPrefab.FindModuleImplementing<FlagDecalBackground>();
			if (flagDecalBackground != null)
			{
				isFlag = true;
			}
		}
		if (part != null && part.Variants != null && part.Variants.Count > 0)
		{
			if (btnSwapTexture != null && HighLogic.LoadedSceneIsEditor)
			{
				btnSwapTexture.gameObject.SetActive(value: true);
				btnSwapTexture.onClick.AddListener(ToggleVariant);
			}
			else if (HighLogic.LoadedSceneIsFlight && btnSwapTexture != null)
			{
				btnSwapTexture.gameObject.SetActive(value: false);
			}
			if (!skipVariants)
			{
				if (variant == null && part.variant == null && part.partPrefab != null)
				{
					part.variant = part.partPrefab.baseVariant;
					variantIndex = -1;
				}
				UIPartActionInventorySlot component2 = GetComponent<UIPartActionInventorySlot>();
				PartVariant partVariant = null;
				if (component2 != null)
				{
					variantIndex = part.partPrefab.variants.GetVariantIndex((variant != null) ? variant.Name : part.variant.Name);
					if (variantIndex > -1)
					{
						partVariant = part.Variants[variantIndex];
					}
					else
					{
						variantIndex = part.partPrefab.variants.GetVariantIndex(part.partPrefab.baseVariant.Name);
						partVariant = part.Variants[variantIndex];
					}
					inventorySlotIndex = component2.slotIndex;
				}
				else
				{
					variantIndex = part.partPrefab.variants.GetVariantIndex((variant != null) ? variant.Name : part.variant.Name);
					partVariant = part.Variants[variantIndex];
				}
				if (!useImageThumbnail)
				{
					ModulePartVariants.ApplyVariant(null, partIcon.Icon.transform, partVariant, materials, skipShader: true, variantIndex);
				}
			}
		}
		else if (btnSwapTexture != null)
		{
			btnSwapTexture.gameObject.SetActive(value: false);
			variantIndex = -1;
		}
		if (isFlag && flagDecalBackground != null)
		{
			if (partIcon != null && partIcon.Icon != null)
			{
				flagDecalBackground.ToolboxSetFlagTexture(partIcon.Icon);
			}
			else
			{
				LoadPartThumbnail(variantIndex, sPart);
			}
			GameEvents.onFlagSelect.Add(SetFlagBrowserTexture);
		}
		else if (useImageThumbnail)
		{
			LoadPartThumbnail(variantIndex);
		}
	}

	public void SetEmptySlot()
	{
		partList = null;
		availablePart = null;
		isEmptySlot = true;
		if (partIcon != null)
		{
			partIcon.DestroyIcon();
		}
		if (btnSwapTexture != null)
		{
			btnSwapTexture.gameObject.SetActive(value: false);
			btnSwapTexture.onClick.RemoveListener(ToggleVariant);
		}
		if (btnPlacePart != null)
		{
			btnPlacePart.gameObject.SetActive(value: false);
		}
		if (stackAmountText != null)
		{
			stackAmountText.gameObject.SetActive(value: false);
		}
		if (stackSlider != null)
		{
			stackSlider.gameObject.SetActive(value: false);
		}
		if (inventoryItemThumbnail != null)
		{
			inventoryItemThumbnail.texture = null;
			inventoryItemThumbnail.gameObject.SetActive(value: false);
		}
	}

	public void UpdateStackUI(bool isStackable, int currentStackedAmount, int maxStackedAmount)
	{
		stackAmountText.gameObject.SetActive(isStackable);
		stackAmountText.text = currentStackedAmount.ToString();
		stackSlider.gameObject.SetActive(isStackable);
		stackSlider.maxValue = maxStackedAmount;
		stackSlider.value = currentStackedAmount;
		if (btnPlacePartRect != null)
		{
			if (isStackable)
			{
				placeButtonPosition.x = placePositionXInitial - placeOffsetWhenStack;
			}
			else
			{
				placeButtonPosition.x = placePositionXInitial;
			}
			btnPlacePartRect.anchoredPosition = placeButtonPosition;
		}
	}

	public void MouseInput_SpawnPart()
	{
		if (partList != null && InputLockManager.IsUnlocked(ControlTypes.EDITOR_ICON_PICK) && !isGrey)
		{
			if (isFlag && flagDecalBackground != null)
			{
				flagDecalBackground.flagSize = 0;
			}
			partList.TapIcon(availablePart);
		}
	}

	public void EnableDeleteButton()
	{
		btnRemove.gameObject.SetActive(value: true);
	}

	public void DisableDeleteButton()
	{
		btnRemove.gameObject.SetActive(value: false);
	}

	public void MouseInput_Delete()
	{
		if (EditorLogic.SelectedPart == null)
		{
			PartCategorizer.Instance.RemovePartFromCategory(availablePart);
		}
	}

	public void EnableAddButton()
	{
		btnAdd.gameObject.SetActive(value: true);
	}

	public void DisableAddButton()
	{
		btnAdd.gameObject.SetActive(value: false);
	}

	public void MouseInput_Add()
	{
		if (EditorLogic.SelectedPart == null)
		{
			PartCategorizer.Instance.AddPartToCustomCategoryViaPopup(availablePart);
		}
	}

	public void MouseInput_PlacePart()
	{
		if (PlacePartCallback != null)
		{
			PlacePartCallback(this);
		}
	}

	public void ToggleVariant()
	{
		EditorLogic.iconRequestedVariantChange = this;
		List<PartVariant> variants = availablePart.Variants;
		if (variants.Count > 0)
		{
			variantIndex = (variantIndex + 1) % variants.Count;
			PartVariant partVariant = variants[variantIndex];
			if (!inInventory)
			{
				availablePart.variant = partVariant;
			}
			ModulePartVariants.ApplyVariant(null, (partIcon == null) ? null : ((partIcon.Icon != null) ? partIcon.Icon.transform : null), partVariant, materials, skipShader: true, variantIndex);
			if (isFlag && flagDecalBackground != null)
			{
				flagDecalBackground.ToolboxSetFlagTexture((partIcon != null) ? partIcon.Icon : null);
			}
			if (isGrey)
			{
				SetPartColor((partIcon != null) ? partIcon.Renderers : null, new Color(0.25f, 0.25f, 0.25f, 1f), processUnlit: true, isFlag, availablePart);
			}
			changingDefaultVariant = true;
			if (!inInventory)
			{
				GameEvents.onEditorDefaultVariantChanged.Fire(availablePart, partVariant);
			}
			changingDefaultVariant = false;
			if (PartIcon != null)
			{
				materials = CreateMaterialArray(PartIcon.gameObject, includeInactiveRenderers: true);
				PartListTooltipController.SetScreenSpaceMaskMaterials(materials);
			}
			else
			{
				LoadPartThumbnail(variantIndex);
			}
		}
		EditorLogic.iconRequestedVariantChange = null;
	}

	public void OnEditorDefaultVariantChanged(AvailablePart ap, PartVariant variant)
	{
		if (!changingDefaultVariant && ap == availablePart && EditorLogic.iconRequestedVariantChange == this)
		{
			variantIndex = availablePart.Variants.IndexOf(ap.variant);
			ModulePartVariants.ApplyVariant(null, (partIcon == null) ? null : ((partIcon.Icon != null) ? partIcon.Icon.transform : null), variant, materials, skipShader: true, variantIndex);
			if (isFlag && flagDecalBackground != null)
			{
				flagDecalBackground.ToolboxSetFlagTexture((partIcon != null) ? partIcon.Icon : null);
			}
			if (isGrey)
			{
				SetPartColor((partIcon != null) ? partIcon.Renderers : null, new Color(0.25f, 0.25f, 0.25f, 1f), processUnlit: true, isFlag, availablePart);
			}
		}
	}

	public void SetFlagBrowserTexture(string flagURL)
	{
		if (!inInventory && isFlag && flagDecalBackground != null)
		{
			if (partIcon != null && partIcon.Icon != null)
			{
				flagDecalBackground.ToolboxSetFlagTexture(partIcon.Icon, flagURL);
			}
			else
			{
				LoadPartThumbnail(variantIndex);
			}
		}
	}

	public bool VariantsAvailable()
	{
		if (availablePart.Variants != null)
		{
			return availablePart.Variants.Count > 0;
		}
		return false;
	}

	public PartVariant GetCurrentVariant()
	{
		return availablePart.variant;
	}

	public void InstantiatePartIcon()
	{
		GameObject gameObject = Object.Instantiate(availablePart.iconPrefab);
		gameObject.SetActive(value: true);
		partIcon = new PartIcon(gameObject);
		partIcon.Icon.transform.parent = base.transform;
		partIcon.Icon.transform.localPosition = new Vector3(0f, 0f, 0f - iconSize / 2f);
		partIcon.Icon.transform.localScale = Vector3.one * (iconSize / 2f);
		partScale = partIcon.Icon.transform.localScale;
		partIcon.Icon.transform.rotation = Quaternion.Euler(-15f, 0f, 0f);
		partIcon.Icon.transform.Rotate(0f, -30f, 0f);
		startRot = partIcon.Icon.transform.rotation;
		U5Util.SetLayerRecursive(gameObject, LayerMask.NameToLayer("UIAdditional"));
		materials = CreateMaterialArray(gameObject, includeInactiveRenderers: true);
	}

	public static Material[] CreateMaterialArray(GameObject gameObject)
	{
		return CreateMaterialArray(gameObject, includeInactiveRenderers: false);
	}

	public static Material[] CreateMaterialArray(GameObject gameObject, bool includeInactiveRenderers)
	{
		List<Material> list = new List<Material>();
		Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>(includeInactiveRenderers);
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			Material[] array = componentsInChildren[i].materials;
			int j = 0;
			for (int num2 = array.Length; j < num2; j++)
			{
				Material material = array[j];
				if (material.HasProperty(PropertyIDs._MinX))
				{
					list.Add(material);
				}
			}
		}
		return list.ToArray();
	}

	public static void CleanUpMaterials(GameObject gameObject)
	{
		Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			if (!(componentsInChildren[i] == null))
			{
				Material[] array = componentsInChildren[i].materials;
				int j = 0;
				for (int num2 = array.Length; j < num2; j++)
				{
					Object.Destroy(array[j]);
				}
			}
		}
	}

	public void LoadPartThumbnail(int variantIdx, StoredPart sPart = null)
	{
		if (!isEmptySlot)
		{
			string text = ((variantIdx > -1) ? variantIdx.ToString() : "");
			Path.GetFileNameWithoutExtension(availablePart.configFileFullName);
			int num = availablePart.partUrl.LastIndexOf("Parts/");
			thumbnailPath = "";
			if (num > 0)
			{
				thumbnailPath = availablePart.partUrl.Substring(0, availablePart.partUrl.LastIndexOf("Parts/") + 6) + "@thumbs/" + availablePart.name + "_icon" + text;
			}
			else
			{
				thumbnailPath = KSPUtil.ApplicationRootPath + "@thumbs/Parts/" + availablePart.name + "_icon" + text;
			}
			IThumbnailSetup thumbNailSetupIface = CraftThumbnail.GetThumbNailSetupIface(availablePart);
			if (thumbNailSetupIface != null && sPart != null)
			{
				thumbnailPath += thumbNailSetupIface.ThumbSuffix(sPart.snapshot);
			}
			Texture2D texture2D = null;
			texture2D = GameDatabase.Instance.GetTexture(thumbnailPath, asNormalMap: false);
			if (texture2D == null)
			{
				StartCoroutine(WaitAndTakePartSnapshot(thumbnailPath, variantIdx, sPart));
			}
			else
			{
				inventoryItemThumbnail.texture = texture2D;
				inventoryItemThumbnail.gameObject.SetActive(value: true);
			}
			UIPAIS = GetComponent<UIPartActionInventorySlot>();
			if (UIPAIS != null)
			{
				UIPAIS.UpdateStackUI();
			}
		}
	}

	public IEnumerator WaitAndTakePartSnapshot(string thumbnailPath, int variantIdx, StoredPart sPart)
	{
		UIPAIS = GetComponent<UIPartActionInventorySlot>();
		if (UIPAIS != null)
		{
			int frames = 0;
			while (frames < UIPAIS.slotIndex + 2)
			{
				yield return wfoef;
				int num = frames + 1;
				frames = num;
			}
		}
		else
		{
			yield return wfoef;
		}
		string fullFileName = "";
		inventoryItemThumbnail.texture = CraftThumbnail.TakePartSnapshot(availablePart.name, sPart, null, 256, thumbnailPath.Substring(0, thumbnailPath.LastIndexOf(availablePart.name)), out fullFileName, 15f, 25f, 15f, 25f, 18f, variantIdx);
		if (!string.IsNullOrEmpty(fullFileName))
		{
			Texture2D tex2 = Object.Instantiate(AssetBase.GetTexture("defaultPartIcon"));
			yield return StartCoroutine(ShipConstruction.LoadThumbnail(tex2, fullFileName + ".png"));
			FileInfo info = new FileInfo(fullFileName + ".png");
			UrlDir.UrlFile file = new UrlDir.UrlFile(GameDatabase.Instance.root, info);
			tex2.name = thumbnailPath;
			if (tex2.width % 4 == 0 && tex2.height % 4 == 0)
			{
				tex2.Compress(highQuality: false);
			}
			tex2.Apply();
			GameDatabase.TextureInfo textureInfo = new GameDatabase.TextureInfo(file, tex2, isNormalMap: false, isReadable: true, isCompressed: true);
			textureInfo.name = thumbnailPath;
			GameDatabase.Instance.databaseTexture.Add(textureInfo);
			inventoryItemThumbnail.texture = textureInfo.texture;
			inventoryItemThumbnail.gameObject.SetActive(value: true);
		}
	}

	public void MouseInput_PointerEnter(PointerEventData data)
	{
		if (mouseOver || !InputLockManager.IsUnlocked(ControlTypes.EDITOR_ICON_HOVER))
		{
			return;
		}
		if (!isGrey && !stillFocused)
		{
			partRotation = 0f;
			if (partIcon != null && partIcon.Icon != null)
			{
				partIcon.Icon.transform.localScale = partScale * iconOverScale;
			}
		}
		mouseOver = true;
	}

	public void MouseInput_PointerExit(PointerEventData data)
	{
		if (!mouseOver)
		{
			return;
		}
		if (InputLockManager.IsUnlocked(ControlTypes.EDITOR_ICON_HOVER))
		{
			if (!isGrey && partIcon != null && partIcon.Icon != null)
			{
				partIcon.Icon.transform.rotation = startRot;
				partIcon.Icon.transform.localScale = partScale;
			}
		}
		else
		{
			stillFocused = true;
		}
		mouseOver = false;
	}

	public void Update()
	{
		CheckExperimental();
		CheckMissionRequired();
		if (mouseOver)
		{
			if (!isGrey)
			{
				partRotation += iconOverSpin * Time.deltaTime;
				if (partIcon != null && partIcon.Icon != null)
				{
					partIcon.Icon.transform.localRotation = startRot * Quaternion.AngleAxis(partRotation, Vector3.up);
				}
			}
		}
		else if (stillFocused)
		{
			stillFocused = false;
			MouseInput_PointerExit(null);
		}
	}

	public void Highlight()
	{
		MouseInput_PointerEnter(null);
	}

	public void Unhighlight()
	{
		MouseInput_PointerExit(null);
	}

	public void SetGrey(string why)
	{
		if (!isGrey)
		{
			btnSpawnPart.spriteState = buttonState;
			greyoutToolTipMessage = why;
			btnSpawnPart.interactable = false;
			SetPartColor((partIcon != null) ? partIcon.Renderers : null, new Color(0.25f, 0.25f, 0.25f, 1f), processUnlit: true, isFlag, availablePart);
			isGrey = true;
		}
	}

	public void UnsetGrey()
	{
		if (isGrey)
		{
			btnSpawnPart.spriteState = buttonState;
			greyoutToolTipMessage = "";
			btnSpawnPart.interactable = true;
			SetPartColor((partIcon != null) ? partIcon.Renderers : null, new Color(1f, 1f, 1f, 1f), processUnlit: true, isFlag, availablePart);
			isGrey = false;
		}
	}

	public static void SetPartColor(GameObject partIcon, Color color, AvailablePart part = null)
	{
		SetPartColor(partIcon, color, isFlag: false, part);
	}

	public static void SetPartColor(GameObject partIcon, Color color, bool isFlag, AvailablePart part = null)
	{
		SetPartColor(partIcon, color, processUnlit: true, isFlag, part);
	}

	public static void SetPartColor(GameObject partIcon, Color color, bool processUnlit, bool isFlag, AvailablePart part = null)
	{
		SetPartColor(partIcon.GetComponentsInChildren<Renderer>(), color, processUnlit, isFlag, part);
	}

	public static void SetPartColor(Renderer[] renderers, Color color, bool processUnlit, bool isFlag, AvailablePart part = null)
	{
		if (renderers == null)
		{
			return;
		}
		bool flag = color == Color.white;
		int num = renderers.Length;
		while (num-- > 0)
		{
			Material[] array = renderers[num].materials;
			int num2 = array.Length;
			while (num2-- > 0)
			{
				bool flag2 = true;
				if (flag && part != null)
				{
					flag2 = SetMatchingIconPrefabMaterial(part, array[num2]);
				}
				if (flag2)
				{
					if (isFlag)
					{
						color = new Color(color.r, color.g, color.b, array[num2].color.a);
					}
					if (!flag && processUnlit && array[num2].shader.name == "KSP/ScreenSpaceMaskUnlit")
					{
						SetMatchingIconPrefabMaterial(part, array[num2]);
						Color color2 = new Color(Mathf.Max(0f, array[num2].color.r - color.r * 2f), Mathf.Max(0f, array[num2].color.g - color.g * 2f), Mathf.Max(0f, array[num2].color.b - color.b * 2f), color.r);
						array[num2].color = color2;
					}
					else
					{
						array[num2].color = color;
					}
				}
			}
			renderers[num].materials = array;
		}
	}

	public static bool SetMatchingIconPrefabMaterial(AvailablePart part, Material material)
	{
		bool flag = true;
		Renderer[] iconRenderers = part.iconRenderers;
		string value = material.name;
		int num = material.name.IndexOf("(Instance");
		if (num > -1)
		{
			value = material.name.Substring(0, num).Trim();
		}
		for (int i = 0; i < iconRenderers.Length; i++)
		{
			Material[] array = iconRenderers[i].materials;
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j].name.Contains(value))
				{
					material.color = array[j].color;
					flag = false;
					break;
				}
			}
			if (!flag)
			{
				break;
			}
		}
		return flag;
	}

	public void CheckExperimental()
	{
		if (checkedExperimental || HighLogic.CurrentGame == null)
		{
			return;
		}
		if (HighLogic.CurrentGame.Mode != Game.Modes.CAREER)
		{
			checkedExperimental = true;
		}
		else
		{
			if (ResearchAndDevelopment.Instance == null)
			{
				return;
			}
			checkedExperimental = true;
			if (ResearchAndDevelopment.IsExperimentalPart(availablePart))
			{
				Image component = base.gameObject.GetComponent<Image>();
				if (component != null)
				{
					component.color = experimentalPartColor;
				}
			}
		}
	}

	public void CheckMissionRequired()
	{
		if (checkedMissionRequired || HighLogic.CurrentGame == null)
		{
			return;
		}
		if (HighLogic.CurrentGame.Mode != Game.Modes.MISSION)
		{
			checkedMissionRequired = true;
		}
		else
		{
			if (MissionSystem.Instance == null || MissionsApp.Instance == null || MissionsApp.Instance.CurrentVessel == null)
			{
				return;
			}
			checkedMissionRequired = true;
			if (availablePart != null && MissionsApp.Instance.CurrentVessel.vesselSituation.requiredParts.Contains(availablePart.name))
			{
				Image component = base.gameObject.GetComponent<Image>();
				if (component != null)
				{
					component.color = missionRequiredPartColor;
				}
			}
		}
	}
}

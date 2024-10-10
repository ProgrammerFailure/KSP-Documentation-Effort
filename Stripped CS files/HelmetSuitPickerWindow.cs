using System;
using System.Collections.Generic;
using Expansions;
using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HelmetSuitPickerWindow : MonoBehaviour
{
	[SerializeField]
	public TextMeshProUGUI title;

	[SerializeField]
	public TextMeshProUGUI kerbalName;

	[SerializeField]
	public Button closeButton;

	[SerializeField]
	public SuitButton suitButtonPrefab;

	public CrewListItem crewItem;

	public ProtoCrewMember crew;

	public List<GameObject> previewCamList;

	public int thumbnailCameraMask;

	[SerializeField]
	public float thumbnailCameraSize = 0.5f;

	[NonSerialized]
	public Camera thumbnailCamera;

	[NonSerialized]
	public RenderTexture thumbnailRenderTexture;

	public List<ProtoCrewMember.KerbalSuit> availableSuitTypes;

	public SuitCombos suitCombos;

	public List<SuitButton> suitButtons;

	public List<PreviewType> previewTypes;

	[SerializeField]
	public GameObject prefabComboButton;

	public CrewListItem.KerbalTypes kerbalType;

	public List<GameObject> lightPreviewList;

	public bool isAC;

	public SuitCombo initialSuit;

	public string initialSuitId;

	public void Start()
	{
		closeButton.onClick.AddListener(OnCloseClick);
		GameEvents.onGameSceneSwitchRequested.Add(OnSceneSwitch);
		GameEvents.onSuitComboSelection.Add(OnComboSelected);
		GameEvents.onClickHelmetNeckringButton.Add(OnHelmetNeckringSelection);
		GameEvents.onClickSuitLightButton.Add(OpenSuitLightPicker);
		GameEvents.onGUIAstronautComplexDespawn.Add(OnCloseClick);
		GameEvents.onGUIAstronautComplexSpawn.Add(OnACSpawn);
		GameEvents.onGUIAstronautComplexDespawn.Add(OnACDespawn);
	}

	public void OnDestroy()
	{
		closeButton.onClick.RemoveListener(OnCloseClick);
		GameEvents.onGameSceneSwitchRequested.Remove(OnSceneSwitch);
		GameEvents.onSuitComboSelection.Remove(OnComboSelected);
		GameEvents.onClickHelmetNeckringButton.Remove(OnHelmetNeckringSelection);
		GameEvents.onClickSuitLightButton.Remove(OpenSuitLightPicker);
		GameEvents.onGUIAstronautComplexDespawn.Remove(OnCloseClick);
		GameEvents.onGUIAstronautComplexSpawn.Remove(OnACSpawn);
		GameEvents.onGUIAstronautComplexDespawn.Remove(OnACDespawn);
	}

	public void OnCloseClick()
	{
		CloseWindow();
	}

	public void CloseWindow()
	{
		CleanWindow();
		for (int i = 0; i < lightPreviewList.Count; i++)
		{
			UnityEngine.Object.Destroy(lightPreviewList[i]);
		}
		lightPreviewList.Clear();
		base.gameObject.SetActive(value: false);
		if (crew.ComboId != initialSuitId)
		{
			AnalyticsUtil.LogSuitWindowUsage(HighLogic.CurrentGame, crew, initialSuit, suitCombos.GetCombo(crew.ComboId), initialSuitId, crew.ComboId);
		}
		initialSuitId = "";
		initialSuit = null;
	}

	public void SetupSuitTypeButtons(CrewListItem crewListItem, ProtoCrewMember crew, CrewListItem.KerbalTypes kerbalType)
	{
		if (this.crew != null && this.crew.name != crew.name && this.crew.ComboId != initialSuitId)
		{
			AnalyticsUtil.LogSuitWindowUsage(HighLogic.CurrentGame, this.crew, initialSuit, suitCombos.GetCombo(this.crew.ComboId), initialSuitId, this.crew.ComboId);
		}
		crewItem = crewListItem;
		this.crew = crew;
		this.kerbalType = kerbalType;
		title.text = Localizer.Format("#autoLOC_8012008");
		kerbalName.text = crewItem.GetName();
		CleanWindow();
		GetAvailableSuits();
		for (int i = 0; i < availableSuitTypes.Count; i++)
		{
			SuitButton suitButton = UnityEngine.Object.Instantiate(suitButtonPrefab);
			suitButton.SuitLightColorPicker.crew = crew;
			suitButton.transform.SetParent(base.transform);
			suitButton.transform.localPosition = new Vector3(0f, 0f, 1f);
			suitButton.transform.localScale = Vector3.one;
			suitButton.KerbalSuit = availableSuitTypes[i];
			suitButton.buttonIndex = i;
			suitButton.GenerateButtonName(crewItem.GetName(), availableSuitTypes[i].ToString());
			SetupStockCombos(suitButton);
			SetupExtraCombos(suitButton);
			SetupPreview(suitButton);
			SetupHelmetNeckringButton(suitButton);
			suitButtons.Add(suitButton);
		}
		InitSelection();
		initialSuitId = crew.ComboId;
		initialSuit = suitCombos.GetCombo(crew.ComboId);
	}

	public void InitSelection()
	{
		if (!crew.completedFirstEVA)
		{
			for (int i = 0; i < suitButtons.Count; i++)
			{
				suitButtons[i].comboSelector.comboList[0].Select(0);
			}
			suitButtons[0].Select();
			SetDefaultCombo(suitButtons[0].comboSelector, suitButtons[0].comboSelector.comboList[0].comboId);
		}
		else
		{
			InitFromProtoCrewData();
		}
	}

	public void InitFromProtoCrewData()
	{
		for (int i = 0; i < suitButtons.Count; i++)
		{
			suitButtons[i].comboSelector.comboList[0].Select(0);
		}
		if (crew.ComboId != null)
		{
			for (int j = 0; j < suitButtons.Count; j++)
			{
				for (int k = 0; k < suitButtons[j].comboSelector.comboList.Count; k++)
				{
					if (crew.ComboId == suitButtons[j].comboSelector.comboList[k].comboId)
					{
						OnComboSelected(suitButtons[j].comboSelector, crew.ComboId, k);
						break;
					}
				}
			}
		}
		else
		{
			for (int l = 0; l < suitButtons.Count; l++)
			{
				suitButtons[l].comboSelector.comboList[0].Select(0);
			}
			suitButtons[0].Select();
			SetDefaultCombo(suitButtons[0].comboSelector, suitButtons[0].comboSelector.comboList[0].comboId);
		}
	}

	public void CleanWindow()
	{
		if (previewCamList != null)
		{
			for (int i = 0; i < previewCamList.Count; i++)
			{
				UnityEngine.Object.Destroy(previewCamList[i]);
			}
			previewCamList.Clear();
		}
		else
		{
			previewCamList = new List<GameObject>();
		}
		if (suitButtons != null)
		{
			for (int j = 0; j < suitButtons.Count; j++)
			{
				if (suitButtons[j].KerbalPreview != null)
				{
					UnityEngine.Object.Destroy(suitButtons[j].KerbalPreview.gameObject);
				}
				UnityEngine.Object.Destroy(suitButtons[j].gameObject);
			}
			suitButtons.Clear();
		}
		else
		{
			suitButtons = new List<SuitButton>();
		}
		if (lightPreviewList != null)
		{
			for (int k = 0; k < lightPreviewList.Count; k++)
			{
				UnityEngine.Object.Destroy(lightPreviewList[k]);
			}
			lightPreviewList.Clear();
		}
		else
		{
			lightPreviewList = new List<GameObject>();
		}
	}

	public void GetAvailableSuits()
	{
		availableSuitTypes = new List<ProtoCrewMember.KerbalSuit>();
		int num = Enum.GetNames(typeof(ProtoCrewMember.KerbalSuit)).Length;
		for (int i = 0; i < num; i++)
		{
			if (ExpansionsLoader.IsExpansionKerbalSuitInstalled((ProtoCrewMember.KerbalSuit)i))
			{
				availableSuitTypes.Add((ProtoCrewMember.KerbalSuit)i);
			}
		}
	}

	public void SetupHelmetNeckringButton(SuitButton suitButton)
	{
		bool eVA_DEFAULT_HELMET_ON;
		if (!crew.completedFirstEVA)
		{
			if (eVA_DEFAULT_HELMET_ON = GameSettings.EVA_DEFAULT_HELMET_ON)
			{
				if (eVA_DEFAULT_HELMET_ON && (eVA_DEFAULT_HELMET_ON = GameSettings.EVA_DEFAULT_NECKRING_ON))
				{
					suitButton.HelmetNeckRingButton.image.sprite = suitButton.HelmetNeckRingState[0];
					suitButton.helmetNeckRingIndex = 0;
					crew.hasHelmetOn = true;
					crew.hasNeckRingOn = true;
				}
			}
			else if (eVA_DEFAULT_HELMET_ON = GameSettings.EVA_DEFAULT_NECKRING_ON)
			{
				if (eVA_DEFAULT_HELMET_ON)
				{
					if (suitButton.KerbalSuit == ProtoCrewMember.KerbalSuit.Vintage)
					{
						suitButton.HelmetNeckRingButton.image.sprite = suitButton.HelmetNeckRingState[2];
						suitButton.helmetNeckRingIndex = 2;
						crew.hasHelmetOn = false;
						crew.hasNeckRingOn = false;
					}
					else
					{
						suitButton.HelmetNeckRingButton.image.sprite = suitButton.HelmetNeckRingState[1];
						suitButton.helmetNeckRingIndex = 1;
						crew.hasHelmetOn = false;
						crew.hasNeckRingOn = true;
					}
				}
			}
			else
			{
				suitButton.HelmetNeckRingButton.image.sprite = suitButton.HelmetNeckRingState[2];
				suitButton.helmetNeckRingIndex = 2;
				crew.hasHelmetOn = false;
				crew.hasNeckRingOn = false;
			}
		}
		else if (eVA_DEFAULT_HELMET_ON = crew.hasHelmetOn)
		{
			if (eVA_DEFAULT_HELMET_ON && (eVA_DEFAULT_HELMET_ON = crew.hasNeckRingOn))
			{
				suitButton.HelmetNeckRingButton.image.sprite = suitButton.HelmetNeckRingState[0];
				suitButton.helmetNeckRingIndex = 0;
			}
		}
		else if (eVA_DEFAULT_HELMET_ON = crew.hasNeckRingOn)
		{
			if (eVA_DEFAULT_HELMET_ON)
			{
				if (suitButton.KerbalSuit == ProtoCrewMember.KerbalSuit.Vintage)
				{
					suitButton.HelmetNeckRingButton.image.sprite = suitButton.HelmetNeckRingState[2];
					suitButton.helmetNeckRingIndex = 2;
				}
				else
				{
					suitButton.HelmetNeckRingButton.image.sprite = suitButton.HelmetNeckRingState[1];
					suitButton.helmetNeckRingIndex = 1;
				}
			}
		}
		else
		{
			suitButton.HelmetNeckRingButton.image.sprite = suitButton.HelmetNeckRingState[2];
			suitButton.helmetNeckRingIndex = 2;
		}
		suitButton.KerbalPreview.PreviewHelmetSelection(crew);
	}

	public void SetupStockCombos(SuitButton suitButton)
	{
		for (int i = 0; i < suitCombos.StockCombos.Count; i++)
		{
			if (suitCombos.StockCombos[i].gender == crew.gender.ToString() && suitCombos.StockCombos[i].suitType == suitButton.KerbalSuit.ToString())
			{
				SetupComboSelector(suitButton.comboSelector, suitCombos.StockCombos[i]);
			}
		}
	}

	public void SetupExtraCombos(SuitButton suitButton)
	{
		for (int i = 0; i < suitCombos.ExtraCombos.Count; i++)
		{
			if (suitCombos.ExtraCombos[i].gender == crew.gender.ToString() && suitCombos.ExtraCombos[i].suitType == suitButton.KerbalSuit.ToString())
			{
				SetupComboSelector(suitButton.comboSelector, suitCombos.ExtraCombos[i]);
			}
		}
	}

	public void SetupPreview(SuitButton suitButton)
	{
		float num = 0f;
		thumbnailCameraMask = LayerMask.GetMask("UI");
		for (int i = 0; i < previewTypes.Count; i++)
		{
			if (crew.gender == previewTypes[i].gender && suitButton.KerbalSuit == previewTypes[i].suitType)
			{
				if (suitButton.KerbalPreview == null)
				{
					suitButton.KerbalPreview = UnityEngine.Object.Instantiate(previewTypes[i].prefabPreview).GetComponent<KerbalPreview>();
					suitButton.KerbalPreview.transform.localPosition = new Vector3(999f + num, 999f, 999f);
				}
				CreateThumbnailCamera(out thumbnailCamera, thumbnailCameraSize, thumbnailCameraMask, out thumbnailRenderTexture, 256, 24, suitButton.KerbalPreview.transform);
				suitButton.thumbImg.texture = thumbnailRenderTexture;
				suitButton.comboSelector.previewBodyMaterial = suitButton.KerbalPreview.bodyMaterial;
				suitButton.comboSelector.previewHelmetMaterial = suitButton.KerbalPreview.helmetMaterial;
				if (suitButton.KerbalPreview.neckringMaterial != null)
				{
					suitButton.comboSelector.previewNeckringMaterial = suitButton.KerbalPreview.neckringMaterial;
				}
				SetupGlowColor(suitButton);
			}
			num += 4f;
		}
		GameObject gameObject = new GameObject();
		gameObject.name = suitButton.KerbalPreview.name + " LightPreview";
		gameObject.transform.SetParent(suitButton.KerbalPreview.transform);
		gameObject.transform.localPosition = new Vector3(0f, 0.5f, 1f);
		gameObject.transform.SetParent(UIMasterController.Instance.dialogCanvas.transform);
		Light light = gameObject.AddComponent<Light>();
		light.type = LightType.Point;
		light.range = 15f;
		light.cullingMask = thumbnailCameraMask;
		if (HighLogic.LoadedScene == GameScenes.EDITOR)
		{
			light.intensity = 0.5f;
		}
		else if (HighLogic.LoadedScene == GameScenes.MISSIONBUILDER)
		{
			light.intensity = 0.6f;
		}
		else if (isAC || HighLogic.LoadedScene == GameScenes.SPACECENTER)
		{
			light.intensity = 0.9f;
		}
		lightPreviewList.Add(gameObject);
	}

	public void CreateThumbnailCamera(out Camera camRef, float camSize, LayerMask layerMask, out RenderTexture rtRef, int rtSize, int rtDepth, Transform kerbalPreview)
	{
		camRef = new GameObject("SuitPreviewCamera").AddComponent<Camera>();
		camRef.orthographic = true;
		camRef.orthographicSize = camSize;
		camRef.cullingMask = layerMask;
		camRef.farClipPlane = 295f;
		camRef.clearFlags = CameraClearFlags.Color;
		camRef.allowHDR = true;
		camRef.transform.SetParent(kerbalPreview);
		camRef.transform.localPosition = new Vector3(0f, 0.22f, 3f);
		camRef.transform.eulerAngles = new Vector3(0f, 180f, 0f);
		camRef.transform.SetParent(null);
		rtRef = new RenderTexture(rtSize, rtSize, rtDepth);
		camRef.targetTexture = rtRef;
		previewCamList.Add(camRef.gameObject);
	}

	public void SetupComboSelector(ComboSelector comboSelector, SuitCombo suitCombo)
	{
		ComboButton component = UnityEngine.Object.Instantiate(prefabComboButton, comboSelector.ScrollMain.content).GetComponent<ComboButton>();
		SetupComboButton(comboSelector, component, suitCombo);
		comboSelector.comboList.Add(component);
		comboSelector.comboEntryIndex++;
	}

	public void SetupComboButton(ComboSelector comboSelector, ComboButton comboButton, SuitCombo suitCombo)
	{
		comboButton.comboSelector = comboSelector;
		comboButton.comboId = suitCombo.name;
		comboButton.comboIndex = comboSelector.comboEntryIndex;
		Color color = Color.white;
		if (suitCombo.primaryColor == "")
		{
			suitCombo.primaryColor = "#ffffff";
			Debug.LogWarning("Suit combo primary color wrong, assigning default color #ffffff.");
		}
		if (suitCombo.secondaryColor == "")
		{
			suitCombo.secondaryColor = "#000000";
			Debug.LogWarning("Suit combo secondary color wrong, assigning default color #000000.");
		}
		ColorUtility.TryParseHtmlString(suitCombo.primaryColor, out color);
		Color color2;
		Color color3 = ((!ColorUtility.TryParseHtmlString(suitCombo.secondaryColor, out color2)) ? color : color2);
		comboButton.ImagePrimaryColor.color = color;
		comboButton.ImageSecondaryColor.color = color3;
	}

	public void OpenSuitLightPicker(SuitLightColorPicker suitLightColorPicker)
	{
		Color currentColor = new Color(crew.lightR, crew.lightG, crew.lightB);
		suitLightColorPicker.colorPicker.CurrentColor = currentColor;
		suitLightColorPicker.gameObject.SetActive(!suitLightColorPicker.gameObject.activeSelf);
	}

	public void OnHelmetNeckringSelection(SuitButton suitButton)
	{
		if (suitButton.KerbalSuit != ProtoCrewMember.KerbalSuit.Vintage && suitButton.KerbalSuit != ProtoCrewMember.KerbalSuit.Slim)
		{
			if (suitButton.helmetNeckRingIndex < suitButton.HelmetStates - 1)
			{
				suitButton.helmetNeckRingIndex++;
			}
			else
			{
				suitButton.helmetNeckRingIndex = 0;
			}
		}
		else if (suitButton.helmetNeckRingIndex < suitButton.HelmetStates - 1)
		{
			suitButton.helmetNeckRingIndex++;
			if (suitButton.helmetNeckRingIndex == 1)
			{
				suitButton.helmetNeckRingIndex = 2;
			}
		}
		else
		{
			suitButton.helmetNeckRingIndex = 0;
		}
		suitButton.evaHelmetNeckRingState = (SuitButton.EVAHelmetNeckRingState)suitButton.helmetNeckRingIndex;
		if (suitButton.HelmetNeckRingState != null)
		{
			suitButton.HelmetNeckRingButton.image.sprite = suitButton.HelmetNeckRingState[suitButton.helmetNeckRingIndex];
		}
		switch (suitButton.evaHelmetNeckRingState)
		{
		case SuitButton.EVAHelmetNeckRingState.HelmetNeckRingOn:
			crew.hasHelmetOn = true;
			crew.hasNeckRingOn = true;
			break;
		case SuitButton.EVAHelmetNeckRingState.OnlyNeckRingOn:
			crew.hasHelmetOn = false;
			crew.hasNeckRingOn = true;
			break;
		case SuitButton.EVAHelmetNeckRingState.HelmetNeckRingOff:
			crew.hasHelmetOn = false;
			crew.hasNeckRingOn = false;
			break;
		}
		suitButton.KerbalPreview.PreviewHelmetSelection(crew);
	}

	public void OnComboSelected(ComboSelector comboSelector, string comboId, int comboIndex)
	{
		crew.completedFirstEVA = true;
		for (int i = 0; i < suitButtons.Count; i++)
		{
			for (int j = 0; j < suitButtons[i].comboSelector.comboList.Count; j++)
			{
				if (comboId == suitButtons[i].comboSelector.comboList[j].comboId)
				{
					suitButtons[i].comboSelector.comboList[j].Select(suitButtons[i].comboSelector.comboList[j].comboIndex);
				}
				else if (comboId != suitButtons[i].comboSelector.comboList[j].comboId && comboSelector.suitButton.buttonIndex == suitButtons[i].buttonIndex)
				{
					suitButtons[i].comboSelector.comboList[j].Reset();
				}
			}
			if (comboSelector.suitButton.buttonIndex == suitButtons[i].buttonIndex)
			{
				suitButtons[i].Select();
			}
			else
			{
				suitButtons[i].Reset();
			}
		}
		if (CheckStock(comboId))
		{
			SetDefaultCombo(comboSelector, comboId);
		}
		else
		{
			SetExtraCombo(comboSelector, comboId);
		}
		SetupGlowColor(comboSelector.suitButton);
	}

	public bool CheckStock(string comboId)
	{
		int num = 0;
		while (true)
		{
			if (num < suitCombos.StockCombos.Count)
			{
				if (comboId == suitCombos.StockCombos[num].name)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public void SetDefaultCombo(ComboSelector comboSelector, string comboId)
	{
		Vector2 scaleUpdate = new Vector2(1f, 1f);
		Material helmetMaterial = comboSelector.suitButton.KerbalPreview.helmetMaterial;
		Material bodyMaterial = comboSelector.suitButton.KerbalPreview.bodyMaterial;
		Material neckringMaterial = comboSelector.suitButton.KerbalPreview.neckringMaterial;
		suitCombos.UpdateTextureScale(helmetMaterial, scaleUpdate, SuitCombo.MaterialProperty.All);
		suitCombos.UpdateTextureScale(bodyMaterial, scaleUpdate, SuitCombo.MaterialProperty.All);
		if (neckringMaterial != null)
		{
			suitCombos.UpdateTextureScale(neckringMaterial, scaleUpdate, SuitCombo.MaterialProperty.All);
		}
		for (int i = 0; i < suitCombos.StockCombos.Count; i++)
		{
			if (comboId == suitCombos.StockCombos[i].name && comboSelector.suitButton.KerbalSuit.ToString() == suitCombos.StockCombos[i].suitType)
			{
				crew.suit = comboSelector.suitButton.KerbalSuit;
				crew.SuitTexturePath = null;
				crew.NormalTexturePath = null;
				crew.SpritePath = null;
				crew.ComboId = comboId;
				helmetMaterial.mainTexture = suitCombos.StockCombos[i].defaultSuitTexture;
				helmetMaterial.SetTexture("_BumpMap", suitCombos.StockCombos[i].defaultNormalTexture);
				bodyMaterial.mainTexture = suitCombos.StockCombos[i].defaultSuitTexture;
				bodyMaterial.SetTexture("_BumpMap", suitCombos.StockCombos[i].defaultNormalTexture);
				if (neckringMaterial != null)
				{
					neckringMaterial.mainTexture = suitCombos.StockCombos[i].defaultSuitTexture;
					neckringMaterial.SetTexture("_BumpMap", suitCombos.StockCombos[i].defaultNormalTexture);
				}
				UpdateDisplayName(suitCombos.StockCombos[i].displayName, comboSelector.suitButton.KerbalSuit);
				break;
			}
		}
		crewItem.SetKerbal(crew, kerbalType);
	}

	public void SetExtraCombo(ComboSelector comboSelector, string comboId)
	{
		Vector2 scaleUpdate = new Vector2(1f, -1f);
		Material helmetMaterial = comboSelector.suitButton.KerbalPreview.helmetMaterial;
		Material bodyMaterial = comboSelector.suitButton.KerbalPreview.bodyMaterial;
		Material neckringMaterial = comboSelector.suitButton.KerbalPreview.neckringMaterial;
		suitCombos.UpdateTextureScale(helmetMaterial, scaleUpdate, SuitCombo.MaterialProperty.All);
		suitCombos.UpdateTextureScale(bodyMaterial, scaleUpdate, SuitCombo.MaterialProperty.All);
		if (neckringMaterial != null)
		{
			suitCombos.UpdateTextureScale(neckringMaterial, scaleUpdate, SuitCombo.MaterialProperty.All);
		}
		int num = 0;
		while (true)
		{
			if (num < suitCombos.ExtraCombos.Count)
			{
				if (comboId == suitCombos.ExtraCombos[num].name && comboSelector.suitButton.KerbalSuit.ToString() == suitCombos.ExtraCombos[num].suitType)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		crew.suit = comboSelector.suitButton.KerbalSuit;
		crew.SuitTexturePath = suitCombos.ExtraCombos[num].suitTexture;
		crew.NormalTexturePath = suitCombos.ExtraCombos[num].normalTexture;
		crew.SpritePath = suitCombos.ExtraCombos[num].sprite;
		crew.ComboId = comboId;
		Texture texture = GameDatabase.Instance.GetTexture(crew.SuitTexturePath, asNormalMap: false);
		Texture texture2 = GameDatabase.Instance.GetTexture(crew.NormalTexturePath, asNormalMap: true);
		helmetMaterial.mainTexture = ((texture == null) ? suitCombos.GetDefaultTexture(crew, SuitCombo.TextureTarget.Helmet, helmetMaterial, SuitCombo.MaterialProperty.MainTex) : texture);
		helmetMaterial.SetTexture("_BumpMap", (texture2 == null) ? suitCombos.GetDefaultTexture(crew, SuitCombo.TextureTarget.Normal, helmetMaterial, SuitCombo.MaterialProperty.BumpMap) : texture2);
		bodyMaterial.mainTexture = ((texture == null) ? suitCombos.GetDefaultTexture(crew, SuitCombo.TextureTarget.Body, bodyMaterial, SuitCombo.MaterialProperty.MainTex) : texture);
		bodyMaterial.SetTexture("_BumpMap", (texture2 == null) ? suitCombos.GetDefaultTexture(crew, SuitCombo.TextureTarget.Normal, bodyMaterial, SuitCombo.MaterialProperty.BumpMap) : texture2);
		if (neckringMaterial != null)
		{
			neckringMaterial.mainTexture = ((texture == null) ? suitCombos.GetDefaultTexture(crew, SuitCombo.TextureTarget.Body, neckringMaterial, SuitCombo.MaterialProperty.MainTex) : texture);
			neckringMaterial.SetTexture("_BumpMap", (texture2 == null) ? suitCombos.GetDefaultTexture(crew, SuitCombo.TextureTarget.Normal, neckringMaterial, SuitCombo.MaterialProperty.BumpMap) : texture2);
		}
		UpdateDisplayName(suitCombos.ExtraCombos[num].displayName, comboSelector.suitButton.KerbalSuit);
		crewItem.SetKerbal(crew, kerbalType);
	}

	public void SetupGlowColor(SuitButton suitButton)
	{
		if (suitButton.KerbalSuit == ProtoCrewMember.KerbalSuit.Future)
		{
			Color value = new Color(crew.lightR, crew.lightG, crew.lightB);
			suitButton.comboSelector.previewBodyMaterial.SetColor("_EmissiveColor", value);
			suitButton.comboSelector.previewHelmetMaterial.SetColor("_EmissiveColor", value);
		}
	}

	public void UpdateDisplayName(string displayName, ProtoCrewMember.KerbalSuit suitType)
	{
		int num = 0;
		while (true)
		{
			if (num < suitButtons.Count)
			{
				if (suitButtons[num].ComboName != null)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		string text = LocalizedSuitText(suitType);
		string text2 = Localizer.Format(displayName);
		suitButtons[num].ComboName.text = Localizer.Format("#autoLOC_8003407", text2, text);
	}

	public string LocalizedSuitText(ProtoCrewMember.KerbalSuit suitType)
	{
		return suitType switch
		{
			ProtoCrewMember.KerbalSuit.Default => Localizer.Format("#autoLOC_8012021"), 
			ProtoCrewMember.KerbalSuit.Vintage => Localizer.Format("#autoLOC_8012022"), 
			ProtoCrewMember.KerbalSuit.Future => Localizer.Format("#autoLOC_8012023"), 
			ProtoCrewMember.KerbalSuit.Slim => Localizer.Format("#autoLOC_6011176"), 
			_ => " ", 
		};
	}

	public void OnSceneSwitch(GameEvents.FromToAction<GameScenes, GameScenes> data)
	{
		CleanWindow();
		for (int i = 0; i < lightPreviewList.Count; i++)
		{
			UnityEngine.Object.Destroy(lightPreviewList[i]);
		}
		lightPreviewList.Clear();
		base.gameObject.SetActive(value: false);
	}

	public void SetupWindowTransform(RectTransform suitPickerWindowRT)
	{
		suitPickerWindowRT.localPosition = new Vector3((float)Screen.width * -0.05f * GameSettings.UI_SCALE, (float)(-Screen.height) * 0.1f * GameSettings.UI_SCALE, 80f);
		suitPickerWindowRT.SetAsLastSibling();
	}

	public void OnACSpawn()
	{
		isAC = true;
	}

	public void OnACDespawn()
	{
		isAC = false;
	}
}

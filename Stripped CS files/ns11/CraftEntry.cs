using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Expansions;
using ns9;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ns11;

public class CraftEntry : MonoBehaviour
{
	public string craftName;

	public string fullFilePath;

	public Callback<CraftEntry> OnSelected;

	public const string squadExpansionFolder = "SquadExpansion";

	public const string shipsFolder = "Ships";

	public const string thumbnailFolder = "thumbs";

	public const string thumbnailFolderPrefix = "@";

	public const string folderSeparator = "/";

	public float defaultLayoutHeight;

	public static ScrollRect scrollView;

	public int partCount;

	public int stageCount;

	public bool isStock;

	public bool isValid;

	public bool steamItem;

	public ShipTemplate _template;

	public VersionCompareResult compatibility;

	public CraftProfileInfo craftProfileInfo;

	public Texture2D thumbnail;

	public string thumbURL;

	[SerializeField]
	public Toggle tgtCtrl;

	[SerializeField]
	public TextMeshProUGUI header1;

	[SerializeField]
	public TextMeshProUGUI header2;

	[SerializeField]
	public TextMeshProUGUI fieldStats;

	[SerializeField]
	public TextMeshProUGUI fieldCost;

	[SerializeField]
	public TextMeshProUGUI fieldPath;

	[SerializeField]
	public TextMeshProUGUI fieldMsg;

	[SerializeField]
	public RawImage craftThumbImg;

	[SerializeField]
	public GameObject imageSteam;

	public DirectoryController directoryController;

	public ConfigNode _configNode;

	public SteamCraftInfo steamCraftInfo;

	public ShipTemplate template
	{
		get
		{
			if (_template == null)
			{
				_template = new ShipTemplate();
				_template.LoadShip(configNode);
			}
			return _template;
		}
	}

	public Toggle Toggle => tgtCtrl;

	public ConfigNode configNode
	{
		get
		{
			if (_configNode == null)
			{
				_configNode = ConfigNode.Load(fullFilePath);
			}
			return _configNode;
		}
	}

	public void Awake()
	{
		LayoutElement component = GetComponent<LayoutElement>();
		if (component == null)
		{
			defaultLayoutHeight = 53f;
		}
		else
		{
			defaultLayoutHeight = component.minHeight;
		}
		AttachListener(base.gameObject, EventTriggerType.PointerEnter, OnPointerEnter);
		AttachListener(base.gameObject, EventTriggerType.PointerExit, OnPointerExit);
		AttachListener(base.gameObject, EventTriggerType.Scroll, OnScroll);
	}

	public static CraftEntry Create(FileInfo fInfo, bool stock, Callback<CraftEntry> OnSelected)
	{
		return Create(fInfo, stock, OnSelected, steamItem: false, null);
	}

	public static CraftEntry Create(FileInfo fInfo, bool stock, Callback<CraftEntry> OnSelected, bool steamItem, SteamCraftInfo steamCraftInfo)
	{
		CraftEntry component = UnityEngine.Object.Instantiate(AssetBase.GetPrefab("CraftBrowserWidget")).GetComponent<CraftEntry>();
		if (steamItem && fInfo != null)
		{
			component.imageSteam.SetActive(value: true);
		}
		component.OnSelected = OnSelected;
		component.Init(fInfo, stock, steamItem, steamCraftInfo);
		component.name = component.craftName + (stock ? Localizer.Format("#autoLOC_482705") : string.Empty);
		return component;
	}

	public void Init(FileInfo fInfo, bool stock)
	{
		Init(fInfo, stock, steamItem: false, null);
	}

	public void Init(FileInfo fInfo, bool stock, bool steamItem, SteamCraftInfo steamCraftInfo)
	{
		this.steamItem = steamItem;
		this.steamCraftInfo = steamCraftInfo;
		isStock = stock;
		bool canBeUsed = true;
		if (steamItem && steamCraftInfo != null)
		{
			craftProfileInfo = new CraftProfileInfo();
			craftName = (craftProfileInfo.shipName = steamCraftInfo.itemDetails.m_rgchTitle + " (" + Localizer.Format("#autoLOC_8002139") + ")");
			craftProfileInfo.steamPublishedFileId = steamCraftInfo.itemDetails.m_nPublishedFileId.m_PublishedFileId;
			craftProfileInfo.totalCost = steamCraftInfo.cost;
			craftProfileInfo.shipPartsExperimental = false;
			thumbURL = steamCraftInfo.previewURL;
			fullFilePath = steamCraftInfo.itemDetails.m_rgchURL;
			craftProfileInfo.partCount = steamCraftInfo.partCount;
			StartCoroutine(LoadSteamItemPreviewURL());
			craftProfileInfo.stageCount = steamCraftInfo.stageCount;
			craftProfileInfo.compatibility = KSPUtil.CheckVersion(steamCraftInfo.KSPversion, ShipConstruct.lastCompatibleMajor, ShipConstruct.lastCompatibleMinor, ShipConstruct.lastCompatibleRev);
			steamCraftInfo.UpdateSteamState();
			canBeUsed = steamCraftInfo.canBeUsed;
		}
		else
		{
			craftName = fInfo.Name.Replace(fInfo.Extension, "");
			fullFilePath = fInfo.FullName;
			string text = fInfo.FullName.Replace(fInfo.Extension, ".loadmeta");
			if (steamItem)
			{
				text = KSPSteamUtils.GetSteamCacheLocation(text);
			}
			craftProfileInfo = CraftProfileInfo.GetSaveData(fInfo.FullName, text);
			thumbURL = GetThumbURL(fInfo);
			thumbnail = GetThumbnail(thumbURL, fInfo);
			craftThumbImg.texture = thumbnail;
			if (steamItem)
			{
				string stateText = "";
				bool subscribed = false;
				SteamManager.Instance.GetItemState(new PublishedFileId_t(GetSteamFileId()), out stateText, out canBeUsed, out subscribed);
			}
		}
		isValid = craftProfileInfo.shipPartsUnlocked && craftProfileInfo.shipPartModulesAvailable;
		partCount = craftProfileInfo.partCount;
		stageCount = craftProfileInfo.stageCount;
		compatibility = craftProfileInfo.compatibility;
		tgtCtrl.interactable = canBeUsed;
		tgtCtrl.onValueChanged.AddListener(onValueChanged);
		ShowPath(show: false);
		UIUpdate(steamCraftInfo);
	}

	public void SetDirectoryController(DirectoryController directoryController)
	{
		this.directoryController = directoryController;
	}

	public Texture2D GetThumbnail(string thumbURL, FileInfo fileInfo)
	{
		if (steamItem)
		{
			return ShipConstruction.GetThumbnail(thumbURL, fullPath: true, addFileExt: true);
		}
		if (IsExpansion(fileInfo))
		{
			return ShipConstruction.GetThumbnail(thumbURL, fullPath: true, addFileExt: true, fileInfo);
		}
		return ShipConstruction.GetThumbnail(thumbURL, fullPath: false, addFileExt: true, fileInfo);
	}

	public string GetThumbURL(FileInfo fileInfo)
	{
		try
		{
			if (isStock)
			{
				string editorFolder = new FileInfo(fileInfo.FullName).Directory.Name;
				return GetStockThumbnailPath(fileInfo, editorFolder);
			}
			if (steamItem)
			{
				return fileInfo.DirectoryName + "/" + Path.GetFileNameWithoutExtension(fullFilePath);
			}
			if (HighLogic.CurrentGame.IsMissionMode)
			{
				string craftFileName = fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length);
				string playerCraftThumbnailName = ShipConstruction.GetPlayerCraftThumbnailName(fileInfo.Directory.FullName, craftFileName);
				string empty = string.Empty;
				if (HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER)
				{
					empty = "test_missions";
				}
				else
				{
					empty = "missions";
					if (!new FileInfo(string.Concat(KSPUtil.ApplicationRootPath + "thumbs/" + empty + "/", playerCraftThumbnailName, ".png")).Exists)
					{
						empty = "test_missions";
					}
				}
				return string.Concat("thumbs/" + empty + "/", playerCraftThumbnailName);
			}
			string craftFileName2 = fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length);
			return "thumbs/" + ShipConstruction.GetPlayerCraftThumbnailName(fileInfo.Directory.FullName, craftFileName2);
		}
		catch (Exception)
		{
			return string.Empty;
		}
	}

	public bool IsExpansion(FileInfo fileInfo)
	{
		return fileInfo.Directory.ToString().Contains("SquadExpansion");
	}

	public string GetStockThumbnailPath(FileInfo fileInfo, string editorFolder)
	{
		string text = "Ships/@thumbs/" + editorFolder + "/" + KSPUtil.SanitizeFilename(Path.GetFileNameWithoutExtension(fullFilePath));
		if (IsExpansion(fileInfo))
		{
			List<ExpansionsLoader.ExpansionInfo> installedExpansions = ExpansionsLoader.GetInstalledExpansions();
			for (int i = 0; i < installedExpansions.Count; i++)
			{
				if (fullFilePath.Contains(installedExpansions[i].FolderName))
				{
					text = string.Concat(KSPExpansionsUtils.ExpansionsGameDataPath + installedExpansions[i].FolderName + "/", text);
				}
			}
		}
		return text;
	}

	public IEnumerator LoadSteamItemPreviewURL()
	{
		using UnityWebRequest www = UnityWebRequestTexture.GetTexture(steamCraftInfo.previewURL);
		yield return www.SendWebRequest();
		while (!www.isDone)
		{
			yield return null;
		}
		if (!www.isNetworkError && !www.isHttpError && string.IsNullOrEmpty(www.error))
		{
			thumbnail = DownloadHandlerTexture.GetContent(www);
			craftThumbImg.texture = thumbnail;
			yield break;
		}
		Debug.LogWarning("Texture load error in " + steamCraftInfo.previewURL + "': " + www.error);
	}

	public void UIUpdate()
	{
		UIUpdate(null);
	}

	public void UIUpdate(SteamCraftInfo steamCraftInfo)
	{
		if (craftProfileInfo.shipName != craftName)
		{
			TextMeshProUGUI textMeshProUGUI = header1;
			string text2 = (header2.text = Localizer.Format(craftProfileInfo.shipName) + (isStock ? Localizer.Format("#autoLOC_482705") : string.Empty) + " [" + craftName + "]");
			textMeshProUGUI.text = text2;
		}
		else
		{
			TextMeshProUGUI textMeshProUGUI2 = header1;
			string text2 = (header2.text = Localizer.Format(craftProfileInfo.shipName) + (isStock ? Localizer.Format("#autoLOC_482705") : string.Empty));
			textMeshProUGUI2.text = text2;
		}
		fieldStats.text = Localizer.Format("#autoLOC_452442", partCount, stageCount);
		fieldCost.text = GetCostTextColorTag(craftProfileInfo.totalCost) + Localizer.Format("#autoLOC_6003099", craftProfileInfo.totalCost.ToString("N2")) + "</color>";
		fieldMsg.text = string.Empty;
		if (compatibility == VersionCompareResult.COMPATIBLE)
		{
			if (isValid)
			{
				if (craftProfileInfo.shipPartsExperimental)
				{
					fieldMsg.text = "<color=#8dffec>  *** " + Localizer.Format("#autoLOC_6003094") + " ***</color>";
				}
				else
				{
					fieldMsg.text = string.Empty;
				}
			}
			else if (!craftProfileInfo.shipPartModulesAvailable && craftProfileInfo.shipPartsUnlocked)
			{
				fieldMsg.text = Localizer.Format("#autoLOC_8004266");
			}
			else
			{
				fieldMsg.text = "<color=#db6227>  *** " + Localizer.Format("#autoLOC_6003097") + " ***</color>";
			}
		}
		else
		{
			fieldMsg.text = "<color=#db6227>  *** " + Localizer.Format("#autoLOC_8004246") + " ***</color>";
		}
		if (steamItem)
		{
			UIUpdateSteamField(steamCraftInfo);
		}
	}

	public void UIUpdateSteamField(SteamCraftInfo steamCraftInfo)
	{
		if (string.IsNullOrEmpty(fieldMsg.text))
		{
			string stateText = "";
			bool canBeUsed = false;
			bool subscribed = false;
			if (steamCraftInfo != null)
			{
				stateText = steamCraftInfo.steamStateText;
				canBeUsed = steamCraftInfo.canBeUsed;
				subscribed = steamCraftInfo.subscribed;
			}
			else
			{
				SteamManager.Instance.GetItemState(new PublishedFileId_t(GetSteamFileId()), out stateText, out canBeUsed, out subscribed);
			}
			if (subscribed)
			{
				fieldMsg.text = Localizer.Format("#autoLOC_8002130");
			}
			else
			{
				fieldMsg.text = Localizer.Format("#autoLOC_8002131");
			}
			if (!canBeUsed)
			{
				fieldMsg.text = stateText;
			}
		}
	}

	public string GetCostTextColorTag(float cost)
	{
		if (Funding.CanAfford(cost))
		{
			return "<color=" + XKCDColors.HexFormat.KSPBadassGreen + ">";
		}
		return "<color=" + XKCDColors.HexFormat.KSPNotSoGoodOrange + ">";
	}

	public void onValueChanged(bool st)
	{
		if (st)
		{
			OnSelected(this);
		}
	}

	public void Terminate()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public ulong GetSteamFileId()
	{
		if (steamCraftInfo != null)
		{
			return steamCraftInfo.itemDetails.m_nPublishedFileId.m_PublishedFileId;
		}
		if (craftProfileInfo != null)
		{
			return craftProfileInfo.steamPublishedFileId;
		}
		ulong value = 0uL;
		if (configNode != null)
		{
			configNode.TryGetValue("steamPublishedFileId", ref value);
			if (value == 0L)
			{
				value = KSPSteamUtils.GetSteamIDFromSteamFolder(fullFilePath);
			}
		}
		return value;
	}

	public void ShowPath(bool show)
	{
		if (show)
		{
			ShowPath();
		}
		else
		{
			HidePath();
		}
		UpdateWidgetSize();
	}

	public void ShowPath()
	{
		if (string.IsNullOrWhiteSpace(fieldPath.text))
		{
			string displayPathFrom = GetDisplayPathFrom(fullFilePath);
			fieldPath.text = displayPathFrom.Replace("\\", "/");
		}
		fieldPath.gameObject.SetActive(value: true);
	}

	public string GetDisplayPathFrom(string path)
	{
		try
		{
			path = Path.GetDirectoryName(path);
			DirectoryInfo directoryInfo = new DirectoryInfo(ShipConstruction.GetRootCraftSavePath());
			DirectoryInfo directoryInfo2 = new DirectoryInfo(path);
			DirectoryInfo directoryInfo3 = directoryInfo2;
			DirectoryInfo directoryInfo4 = directoryInfo3;
			DirectoryInfo directoryInfo5 = directoryInfo4;
			while (directoryInfo3.Parent.FullName != directoryInfo.FullName)
			{
				directoryInfo5 = directoryInfo4;
				directoryInfo4 = directoryInfo3;
				directoryInfo3 = directoryInfo3.Parent;
			}
			string text = directoryInfo3.Name;
			string text2 = directoryInfo2.FullName.Substring(directoryInfo5.FullName.Length);
			return text + text2;
		}
		catch
		{
			return string.Empty;
		}
	}

	public void HidePath()
	{
		fieldPath.gameObject.SetActive(value: false);
	}

	public void UpdateWidgetSize()
	{
		LayoutElement component = GetComponent<LayoutElement>();
		if (!(component == null) && !(fieldPath == null) && !(fieldMsg == null))
		{
			bool flag = !string.IsNullOrEmpty(fieldMsg.text);
			bool activeInHierarchy = fieldPath.gameObject.activeInHierarchy;
			float num = defaultLayoutHeight;
			float num2 = fieldPath.fontSize * fieldPath.transform.localScale.y;
			float num3 = num + num2;
			component.minHeight = ((flag && activeInHierarchy) ? num3 : num);
		}
	}

	public void AttachListener(GameObject target, EventTriggerType triggerType, Func<PointerEventData, bool> callback)
	{
		EventTrigger component = target.GetComponent<EventTrigger>();
		if (!(component == null))
		{
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = triggerType;
			entry.callback.AddListener(delegate(BaseEventData data)
			{
				callback((PointerEventData)data);
			});
			component.triggers.Add(entry);
		}
	}

	public bool OnPointerEnter(BaseEventData data)
	{
		if (directoryController == null)
		{
			return false;
		}
		directoryController.OverrideDisplay(fullFilePath);
		return true;
	}

	public bool OnPointerExit(BaseEventData data)
	{
		if (directoryController == null)
		{
			return false;
		}
		directoryController.RestoreDisplay();
		return true;
	}

	public bool OnScroll(PointerEventData data)
	{
		if (CraftBrowserDialog.ScrollView != null)
		{
			CraftBrowserDialog.ScrollView.OnScroll(data);
			return true;
		}
		RectTransform rectTransform = base.transform as RectTransform;
		while (scrollView == null && rectTransform.parent != null)
		{
			rectTransform = rectTransform.parent as RectTransform;
			scrollView = rectTransform.GetComponent<ScrollRect>();
		}
		if (scrollView != null)
		{
			scrollView.OnScroll(data);
			return true;
		}
		return false;
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using ns11;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[UI_Group]
public class DirectoryActionGroup : MonoBehaviour
{
	public const string craftFiles = "*.craft";

	[Header("Inspector assigned")]
	[SerializeField]
	[Tooltip("Object with an EventTrigger, which collapses / expands the directory.")]
	public EventTrigger collapseTrigger;

	[Tooltip("Object with an EventTrigger, which enables the user to select a directory for viewing.")]
	[SerializeField]
	public EventTrigger directorySelectTrigger;

	[SerializeField]
	[Tooltip("Object with the TMP_Text component that will show the directory name")]
	public TextMeshProUGUI directoryHeader;

	[SerializeField]
	[Tooltip("Content object")]
	public RectTransform content;

	[Tooltip("Content object")]
	[SerializeField]
	public CanvasGroup contentCanvasGroup;

	[Tooltip("Content object")]
	[SerializeField]
	public LayoutElement contentLayout;

	[Tooltip("Object that shows in the folder is collapsed or expanded")]
	[SerializeField]
	public GameObject collapseStateObject;

	[SerializeField]
	[Tooltip("Object with an Image component for holding the collapse / expand sprites")]
	public Image collapseStateImage;

	[SerializeField]
	public Sprite collapseSprite;

	[SerializeField]
	public Sprite expandSprite;

	[SerializeField]
	[Tooltip("Object with the image or effect that appears to indicate a directory is selected")]
	public GameObject selectedEffect;

	public bool IsCurrentGame;

	public const string fileDisplayTemplate = "{0} ({1})";

	public bool isContentCollapsed;

	public bool initialized;

	public DirectoryController directoryController;

	public bool previousCollapseState;

	public bool previousHighlightState;

	public LayoutElement directoryGroupLayout;

	public float stockFolderSpacing = 4f;

	public List<Transform> ContentItems { get; set; }

	public string Path { get; set; }

	public DirectoryActionGroup Parent { get; set; }

	public bool IsTopLevelDirectory => Parent == null;

	public bool IsStockDirectory { get; set; }

	public bool IsSteamDirectory { get; set; }

	public bool IsExpansionIconVisible
	{
		set
		{
			if (!(collapseStateObject == null))
			{
				collapseStateObject.SetActive(value);
			}
		}
	}

	public DirectoryActionGroup GetSelected
	{
		get
		{
			if (selectedEffect.activeInHierarchy)
			{
				return this;
			}
			if (ContentItems.Count > 0)
			{
				int i = 0;
				for (int count = ContentItems.Count; i < count; i++)
				{
					Transform transform = ContentItems[i];
					if (transform == null)
					{
						continue;
					}
					DirectoryActionGroup component = transform.GetComponent<DirectoryActionGroup>();
					if (!(component == null))
					{
						component = component.GetSelected;
						if (!(component == null))
						{
							return component;
						}
					}
				}
			}
			return null;
		}
	}

	public void Awake()
	{
		ContentItems = new List<Transform>();
	}

	public void Start()
	{
		GameEvents.onUIScaleChange.Add(RescaleContentItems);
	}

	public void OnDestroy()
	{
		GameEvents.onUIScaleChange.Remove(RescaleContentItems);
		collapseTrigger.triggers.RemoveAll((EventTrigger.Entry x) => true);
		directorySelectTrigger.triggers.RemoveAll((EventTrigger.Entry x) => true);
	}

	public void Initialize(string directoryName, string path, bool startCollapsed, DirectoryController controller, bool isStockDirectory = false, bool isSteamDirectory = false)
	{
		directoryHeader.text = directoryName;
		IsStockDirectory = isStockDirectory;
		IsSteamDirectory = isSteamDirectory;
		base.gameObject.name = directoryName;
		Path = path;
		isContentCollapsed = startCollapsed;
		directoryController = controller;
		UpdateFileCount();
		AttachListener(collapseTrigger.gameObject, EventTriggerType.PointerClick, OnExpandToggleClick);
		AttachListener(collapseTrigger.gameObject, EventTriggerType.Scroll, OnScroll);
		AttachListener(directorySelectTrigger.gameObject, EventTriggerType.PointerClick, OnDirectoryClicked);
		AttachListener(directorySelectTrigger.gameObject, EventTriggerType.Scroll, OnScroll);
		if (IsStockDirectory || isSteamDirectory)
		{
			AddExtraSpacing();
		}
		SetUIState();
		Deselect();
		initialized = true;
	}

	public void SetName(string name)
	{
		base.gameObject.name = name;
		UpdateFileCount();
	}

	public void UpdateFileCount()
	{
		if (IsSteamDirectory)
		{
			UpdateSteamFileCount();
		}
		else if (IsStockDirectory)
		{
			UpdateStockFileCount();
		}
		else
		{
			UpdateCraftFileCount();
		}
	}

	public void UpdateCraftFileCount()
	{
		string arg = base.gameObject.name;
		int fileCount = GetFileCount(Path, "*.craft");
		string text = $"{arg} ({fileCount})";
		directoryHeader.text = text;
	}

	public void UpdateSteamFileCount()
	{
		string arg = base.gameObject.name;
		int num = 0;
		List<FileInfo> list = KSPSteamUtils.GatherCraftFilesFileInfo();
		if (list != null)
		{
			num = list.Count;
		}
		string text = $"{arg} ({num})";
		directoryHeader.text = text;
	}

	public void UpdateStockFileCount()
	{
		int num = GetFileCount(Path, "*.craft");
		List<string> list = directoryController.ExpansionDirectories();
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(list[i] + ShipConstruction.GetShipsSubfolderFor(directoryController.DisplayedFacility));
			if (directoryInfo.Exists)
			{
				FileInfo[] files = directoryInfo.GetFiles("*.craft");
				num += files.Length;
			}
		}
		string text = $"{base.name} ({num})";
		directoryHeader.text = text;
	}

	public int GetFileCount(string fullPath, string searchPattern)
	{
		try
		{
			return new DirectoryInfo(fullPath).GetFiles(searchPattern).Length;
		}
		catch (Exception)
		{
			return 0;
		}
	}

	public void SetLabelColor(Color color)
	{
		directoryHeader.faceColor = color;
	}

	public void Select()
	{
		selectedEffect.SetActive(value: true);
	}

	public void Deselect()
	{
		selectedEffect.SetActive(value: false);
	}

	public void Collapse(bool collapseSubdirectories = false, bool ignoreCanvasUpdate = false)
	{
		if (!initialized)
		{
			return;
		}
		isContentCollapsed = true;
		SetUIState();
		if (collapseSubdirectories)
		{
			int i = 0;
			for (int childCount = content.childCount; i < childCount; i++)
			{
				DirectoryActionGroup component = content.GetChild(i).GetComponent<DirectoryActionGroup>();
				if (!(component == null))
				{
					component.Collapse(collapseSubdirectories: true, ignoreCanvasUpdate: true);
				}
			}
		}
		if (!ignoreCanvasUpdate)
		{
			directoryController.UpdateDirectoryLayout(base.gameObject);
		}
	}

	public void Expand(bool expandSubdirectories = false, bool ignoreCanvasUpdate = false)
	{
		if (!initialized)
		{
			return;
		}
		isContentCollapsed = false;
		SetUIState();
		if (expandSubdirectories)
		{
			int i = 0;
			for (int childCount = content.childCount; i < childCount; i++)
			{
				DirectoryActionGroup component = content.GetChild(i).GetComponent<DirectoryActionGroup>();
				if (!(component == null))
				{
					component.Expand(expandSubdirectories: true, ignoreCanvasUpdate: true);
				}
			}
		}
		if (!ignoreCanvasUpdate)
		{
			directoryController.UpdateDirectoryLayout(base.gameObject);
		}
	}

	public void AddItemToContent(Transform t)
	{
		DirectoryActionGroup component = t.GetComponent<DirectoryActionGroup>();
		if (t != null)
		{
			component.Parent = this;
		}
		ContentItems.Add(t);
		t.SetParent(content.transform);
		UpdateContentSize();
	}

	public void RemoveItemFromContent(Transform t)
	{
		DirectoryActionGroup component = t.GetComponent<DirectoryActionGroup>();
		if (t != null)
		{
			component.Parent = null;
		}
		ContentItems.Remove(t);
		t.SetParent(null);
	}

	public void UpdateContentSize()
	{
		for (int i = 0; i < ContentItems.Count; i++)
		{
			if (ContentItems[i] == null)
			{
				ContentItems.RemoveAt(i);
			}
		}
		if (ContentItems.Count <= 0)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		RescaleContentItems();
		Canvas.ForceUpdateCanvases();
		SetUIState();
	}

	public void UpdateExpansionIcon()
	{
		if (!(collapseStateObject == null))
		{
			collapseStateObject.SetActive(ContentItems.Count > 0);
		}
	}

	public void SetUIState()
	{
		contentLayout.ignoreLayout = isContentCollapsed;
		contentCanvasGroup.alpha = ((!isContentCollapsed) ? 1 : 0);
		contentCanvasGroup.interactable = !isContentCollapsed;
		contentCanvasGroup.blocksRaycasts = !isContentCollapsed;
		collapseStateImage.sprite = (isContentCollapsed ? expandSprite : collapseSprite);
	}

	public bool OnExpandToggleClick(PointerEventData data)
	{
		if (isContentCollapsed)
		{
			Expand();
		}
		else
		{
			Collapse();
		}
		return true;
	}

	public bool OnDirectoryClicked(PointerEventData data)
	{
		directoryController.SelectDirectory(this);
		return true;
	}

	public bool OnScroll(PointerEventData data)
	{
		directoryController.OnScroll(data);
		return true;
	}

	public void RescaleContentItems()
	{
		for (int i = 0; i < ContentItems.Count; i++)
		{
			if (ContentItems[i] != null)
			{
				ContentItems[i].localScale = Vector3.one;
				RectTransform rectTransform = ContentItems[i] as RectTransform;
				if (rectTransform != null)
				{
					rectTransform.anchoredPosition3D = Vector3.zero;
				}
			}
			else
			{
				ContentItems.RemoveAt(i);
			}
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

	public void ExpandOverride(bool overrideOn)
	{
		if (overrideOn)
		{
			previousCollapseState = isContentCollapsed;
			previousHighlightState = selectedEffect.activeInHierarchy;
			Expand(expandSubdirectories: false, ignoreCanvasUpdate: true);
			selectedEffect.SetActive(value: true);
		}
		else
		{
			if (previousCollapseState)
			{
				Collapse(collapseSubdirectories: false, ignoreCanvasUpdate: true);
			}
			else
			{
				Expand(expandSubdirectories: false, ignoreCanvasUpdate: true);
			}
			selectedEffect.SetActive(previousHighlightState);
		}
		Canvas.ForceUpdateCanvases();
	}

	public void AddExtraSpacing()
	{
		if (directoryGroupLayout == null)
		{
			directoryGroupLayout = GetComponent<LayoutElement>();
		}
		if (directoryGroupLayout != null)
		{
			directoryGroupLayout.preferredHeight += stockFolderSpacing;
		}
	}
}

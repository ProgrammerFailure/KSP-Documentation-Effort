using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[UI_Group]
public class DirectoryActionGroup : MonoBehaviour
{
	private const string craftFiles = "*.craft";

	[Header("Inspector assigned")]
	[SerializeField]
	[Tooltip("Object with an EventTrigger, which collapses / expands the directory.")]
	private EventTrigger collapseTrigger;

	[Tooltip("Object with an EventTrigger, which enables the user to select a directory for viewing.")]
	[SerializeField]
	private EventTrigger directorySelectTrigger;

	[SerializeField]
	[Tooltip("Object with the TMP_Text component that will show the directory name")]
	private TextMeshProUGUI directoryHeader;

	[SerializeField]
	[Tooltip("Content object")]
	private RectTransform content;

	[Tooltip("Content object")]
	[SerializeField]
	private CanvasGroup contentCanvasGroup;

	[Tooltip("Content object")]
	[SerializeField]
	private LayoutElement contentLayout;

	[Tooltip("Object that shows in the folder is collapsed or expanded")]
	[SerializeField]
	private GameObject collapseStateObject;

	[SerializeField]
	[Tooltip("Object with an Image component for holding the collapse / expand sprites")]
	private Image collapseStateImage;

	[SerializeField]
	private Sprite collapseSprite;

	[SerializeField]
	private Sprite expandSprite;

	[SerializeField]
	[Tooltip("Object with the image or effect that appears to indicate a directory is selected")]
	private GameObject selectedEffect;

	public bool IsCurrentGame;

	private const string fileDisplayTemplate = "{0} ({1})";

	private bool isContentCollapsed;

	private bool initialized;

	private DirectoryController directoryController;

	private bool previousCollapseState;

	private bool previousHighlightState;

	private LayoutElement directoryGroupLayout;

	private float stockFolderSpacing;

	public List<Transform> ContentItems
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	public string Path
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	public DirectoryActionGroup Parent
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public bool IsTopLevelDirectory
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsStockDirectory
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public bool IsSteamDirectory
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public bool IsExpansionIconVisible
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public DirectoryActionGroup GetSelected
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DirectoryActionGroup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
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
	public void Initialize(string directoryName, string path, bool startCollapsed, DirectoryController controller, bool isStockDirectory = false, bool isSteamDirectory = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetName(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateFileCount()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateCraftFileCount()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateSteamFileCount()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateStockFileCount()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int GetFileCount(string fullPath, string searchPattern)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLabelColor(Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Select()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Deselect()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Collapse(bool collapseSubdirectories = false, bool ignoreCanvasUpdate = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Expand(bool expandSubdirectories = false, bool ignoreCanvasUpdate = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddItemToContent(Transform t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveItemFromContent(Transform t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateContentSize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateExpansionIcon()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetUIState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool OnExpandToggleClick(PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool OnDirectoryClicked(PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool OnScroll(PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RescaleContentItems()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AttachListener(GameObject target, EventTriggerType triggerType, Func<PointerEventData, bool> callback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ExpandOverride(bool overrideOn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void AddExtraSpacing()
	{
		throw null;
	}
}

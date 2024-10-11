using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI.Screens;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI;

public class UICraftSaveFlyoutController : UIHoverSlidePanel
{
	[SerializeField]
	[Tooltip("The object that holds the tab to open the flyout")]
	[Header("Inspector Assigned")]
	private XSelectable dropDownTag;

	[SerializeField]
	[Tooltip("Um?")]
	private XSelectable saveFolderSelectable;

	[SerializeField]
	[Tooltip("The prefab for the line item")]
	private EditorSaveFolderItem saveFolderPrefab;

	[SerializeField]
	[Tooltip("The object that contains the radio buttons for settting the default save location")]
	private ToggleGroup selectedToggleGroup;

	[Tooltip("The transform object that the save-folder items will parent to")]
	public RectTransform saveFolderContainer;

	private bool groupSet;

	private List<EditorSaveFolderItem> saveFolderItems;

	private GameObject craftSaveFolderBrowser;

	private DirectoryController craftSaveDirectoryController;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UICraftSaveFlyoutController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected new void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
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
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupSaveFolderChoices()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool UpdateSaveLocationForMissions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<string> GetSubdirectoryList(string parentPath, int depth = 0)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetItems()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateSelectionRadioButtons()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private EditorSaveFolderItem CreateFolderSaveItem(string path, int leftTrimCount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupToggleGroup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SaveFolderSelectable_onPointerExit(XSelectable arg1, PointerEventData arg2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SaveFolderSelectable_onPointerEnter(XSelectable arg1, PointerEventData arg2)
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
	public void CloseFlyout()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OpenCraftFolderBrowser()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateFolderDisplay(string path)
	{
		throw null;
	}
}

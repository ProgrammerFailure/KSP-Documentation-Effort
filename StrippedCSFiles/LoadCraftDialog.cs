using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LoadCraftDialog : MonoBehaviour
{
	public delegate void SelectFileCallback(string fullPath, LoadType t);

	public delegate void CancelCallback();

	public enum LoadType
	{
		Normal,
		Merge
	}

	public class CraftEntry
	{
		public string name;

		public string fullFilePath;

		public int partCount;

		public int stageCount;

		public bool isStock;

		public bool isValid;

		public ShipTemplate template;

		public VersionCompareResult compatibility;

		public Texture2D thumbnail;

		public string thumbURL;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public CraftEntry(FileInfo fInfo, bool stock)
		{
			throw null;
		}
	}

	public string windowTitle;

	public Texture2D FileIcon;

	public string profileName;

	public SelectFileCallback OnFileSelected;

	public CancelCallback OnBrowseCancelled;

	private EditorFacility facility;

	private List<CraftEntry> craftList;

	private bool promptFileCleanup;

	private UISkinDef skin;

	private PopupDialog window;

	private Rect windowRect;

	private CraftEntry selectedEntry;

	private float previousSelectionTime;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LoadCraftDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static LoadCraftDialog Create(EditorFacility facility, string profile, SelectFileCallback onFileSelected, CancelCallback onCancel, bool showMergeOption)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ShowWindowBrowser()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private DialogGUIBase CreateBrowserDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private DialogGUIToggleButton[] CreateBrowserItems()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SelectItem(int craftIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildCraftList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ShowWindowCleanupPrompt()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private DialogGUIBase CreateWindowCleanupPrompt()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DeleteInvalidSaves()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ShowDeleteFileConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private DialogGUIBase CreateDeleteFileConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDeleteConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}
}

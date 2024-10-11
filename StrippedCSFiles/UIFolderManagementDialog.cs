using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class UIFolderManagementDialog : UIConfirmDialog
{
	public enum FolderAction
	{
		Create,
		Delete
	}

	public enum FolderType
	{
		Craft,
		GameSave
	}

	[Tooltip("Text color for the \"Create Folder\" button")]
	[SerializeField]
	[Header("UI Folder Management")]
	private Color ConfirmCreateColor;

	[SerializeField]
	[Tooltip("Text color for the \"Delete Folder\" button")]
	private Color ConfirmDeleteColor;

	[SerializeField]
	private TMP_InputField folderToCreateInput;

	private Callback<string> onSuccessCallback;

	private const string directorySeparator = "/";

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIFolderManagementDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static UIFolderManagementDialog Spawn(FolderAction folderAction, string folderPath, Callback<string> onCompleteCallback, FolderType folderType = FolderType.Craft)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitCreateDialog(string parentFolderPath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitDeleteDialog(string folderName, FolderType itemType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int CountItemsInDirectory(string folderPath, FolderType foldertype, bool includeSubfolders = false)
	{
		throw null;
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using ns9;
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
	public Color ConfirmCreateColor;

	[SerializeField]
	[Tooltip("Text color for the \"Delete Folder\" button")]
	public Color ConfirmDeleteColor;

	[SerializeField]
	public TMP_InputField folderToCreateInput;

	public Callback<string> onSuccessCallback;

	public const string directorySeparator = "/";

	public static UIFolderManagementDialog Spawn(FolderAction folderAction, string folderPath, Callback<string> onCompleteCallback, FolderType folderType = FolderType.Craft)
	{
		GameObject prefab = AssetBase.GetPrefab("UIFolderManagementDialog");
		if (prefab == null)
		{
			return null;
		}
		UIFolderManagementDialog component = UnityEngine.Object.Instantiate(prefab).GetComponent<UIFolderManagementDialog>();
		if (component == null)
		{
			return null;
		}
		component.transform.SetParent(PopupDialogController.PopupDialogCanvas.transform, worldPositionStays: true);
		component.transform.localScale = Vector3.one;
		component.transform.localPosition = Vector3.zero;
		component.onSuccessCallback = onCompleteCallback;
		if (folderAction == FolderAction.Create)
		{
			component.InitCreateDialog(folderPath);
		}
		else
		{
			component.InitDeleteDialog(folderPath, folderType);
		}
		return component;
	}

	public void InitCreateDialog(string parentFolderPath)
	{
		folderToCreateInput.gameObject.SetActive(value: true);
		textHeader.text = Localizer.Format("#autoLOC_6009009");
		textDescription.text = Localizer.Format("#autoLOC_6009016");
		textConfirmation.text = Localizer.Format("#autoLOC_6009017");
		textCancel.text = Localizer.Format("#autoLOC_6009013");
		textConfirmation.color = ConfirmCreateColor;
		onOk = delegate
		{
			string text = folderToCreateInput.text;
			if (string.IsNullOrWhiteSpace(text))
			{
				return;
			}
			try
			{
				new DirectoryInfo(parentFolderPath).CreateSubdirectory(text);
				onSuccessCallback(parentFolderPath + "/" + text);
			}
			catch (Exception)
			{
			}
		};
	}

	public void InitDeleteDialog(string folderName, FolderType itemType)
	{
		folderToCreateInput.gameObject.SetActive(value: false);
		textHeader.text = Localizer.Format("#autoLOC_6009011");
		textConfirmation.text = Localizer.Format("#autoLOC_6009012");
		textCancel.text = Localizer.Format("#autoLOC_6009013");
		textConfirmation.color = ConfirmDeleteColor;
		int num = CountItemsInDirectory(folderName, itemType, includeSubfolders: true);
		string text = ((itemType == FolderType.Craft) ? Localizer.Format("#autoLOC_6009014") : Localizer.Format("#autoLOC_6009015"));
		bool flag = false;
		try
		{
			flag = new DirectoryInfo(folderName).GetDirectories().Length != 0;
		}
		catch
		{
		}
		if (flag)
		{
			textDescription.text = Localizer.Format("#autoLOC_6009019", num, text);
		}
		else
		{
			textDescription.text = Localizer.Format("#autoLOC_6009018", num, text);
		}
		onOk = delegate
		{
			if (string.IsNullOrWhiteSpace(folderName))
			{
				return;
			}
			try
			{
				new DirectoryInfo(folderName).Delete(recursive: true);
				onSuccessCallback(folderName);
			}
			catch (Exception)
			{
			}
		};
	}

	public int CountItemsInDirectory(string folderPath, FolderType foldertype, bool includeSubfolders = false)
	{
		string searchPattern = ((foldertype == FolderType.Craft) ? "*.craft" : "*.sfs");
		int num = 0;
		List<string> list = new List<string>();
		list.Add(folderPath);
		while (list.Count > 0)
		{
			string path = list[0];
			list.RemoveAt(0);
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(path);
				if (includeSubfolders)
				{
					DirectoryInfo[] directories = directoryInfo.GetDirectories();
					int i = 0;
					for (int num2 = directories.Length; i < num2; i++)
					{
						list.Add(directories[i].FullName);
					}
				}
				num += directoryInfo.GetFiles(searchPattern, SearchOption.TopDirectoryOnly).Length;
			}
			catch (Exception)
			{
			}
		}
		return num;
	}
}

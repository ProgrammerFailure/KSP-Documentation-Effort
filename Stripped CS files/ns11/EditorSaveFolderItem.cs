using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

public class EditorSaveFolderItem : MonoBehaviour
{
	public Toggle toggleSetDefault;

	public string saveFolderPath;

	public TextMeshProUGUI textName;

	public Button buttonSave;

	public UICraftSaveFlyoutController controller;

	public bool IsOpenFileBrowserRequest;

	public void Awake()
	{
		buttonSave.onClick.AddListener(MouseInput_OnClick);
		toggleSetDefault.onValueChanged.AddListener(MouseInput_SelectDefault);
	}

	public void OnDestroy()
	{
		buttonSave.onClick.RemoveAllListeners();
		toggleSetDefault.onValueChanged.RemoveAllListeners();
	}

	public void MouseInput_OnClick()
	{
		if (IsOpenFileBrowserRequest)
		{
			controller.OpenCraftFolderBrowser();
		}
		else
		{
			SaveCraft();
		}
		controller.CloseFlyout();
	}

	public void MouseInput_SelectDefault(bool value)
	{
		if (!IsOpenFileBrowserRequest && value && !EditorDriver.SetDefaultSaveFolder(saveFolderPath))
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6009024"), 5f);
		}
	}

	public void Create(UICraftSaveFlyoutController controller, string saveFolderPath, string displayName)
	{
		this.controller = controller;
		this.saveFolderPath = saveFolderPath;
		SetPathDisplay(displayName);
	}

	public bool IsPath(string path)
	{
		return saveFolderPath == path;
	}

	public void SetPathDisplay(string displayName)
	{
		textName.text = displayName.Replace("\\", "/");
	}

	public void SetPath(string fullPath)
	{
		saveFolderPath = fullPath;
	}

	public void SaveCraft()
	{
		ShipConstruction.SaveShipToPath(EditorLogic.fetch.shipNameField.text, saveFolderPath);
		controller.UpdateFolderDisplay(saveFolderPath);
	}
}

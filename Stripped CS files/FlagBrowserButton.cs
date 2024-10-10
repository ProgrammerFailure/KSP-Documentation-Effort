using UnityEngine;
using UnityEngine.UI;

public class FlagBrowserButton : MonoBehaviour
{
	public FlagBrowser flagBrowserPrefab;

	public RawImage flagRawImage;

	public Button button;

	public FlagBrowser browser;

	public FlagBrowser.FlagSelectedCallback OnFlagSelected;

	public Callback OnFlagCancelled;

	public Callback<string> OnFlagSelectedURL;

	public bool isFlagDecalBrowsing;

	public void Start()
	{
		button.onClick.AddListener(SpawnBrowser);
	}

	public void Setup(Texture texture, Callback onBrowserOpen, FlagBrowser.FlagSelectedCallback onFlagSelect, Callback onFlagCancel)
	{
		SetFlag(texture);
		OnFlagSelected = onFlagSelect;
		OnFlagCancelled = onFlagCancel;
	}

	public void SetFlag(Texture texture)
	{
		flagRawImage.texture = texture;
	}

	public void SpawnBrowser()
	{
		browser = Object.Instantiate(flagBrowserPrefab);
		browser.OnDismiss = OnFlagCancel;
		browser.OnFlagSelected = OnFlagSelect;
		InputLockManager.SetControlLock(ControlTypes.EDITOR_SOFT_LOCK, "FlagBrowser");
		button.interactable = false;
	}

	public void OnFlagSelect(FlagBrowser.FlagEntry selected)
	{
		if (isFlagDecalBrowsing)
		{
			isFlagDecalBrowsing = false;
			if (OnFlagSelectedURL != null)
			{
				OnFlagSelectedURL(selected.textureInfo.name);
			}
			InputLockManager.RemoveControlLock("FlagBrowser");
			button.interactable = true;
			if (browser != null && browser.OnDismiss != null)
			{
				browser.OnDismiss();
			}
		}
		else
		{
			if (HighLogic.CurrentGame != null)
			{
				HighLogic.CurrentGame.flagURL = selected.textureInfo.name;
				GameEvents.onFlagSelect.Fire(HighLogic.CurrentGame.flagURL);
			}
			GameEvents.onFlagSelect.Fire(selected.textureInfo.name);
			InputLockManager.RemoveControlLock("FlagBrowser");
			button.interactable = true;
			SetFlag(selected.textureInfo.texture);
			OnFlagSelected(selected);
		}
	}

	public void OnFlagCancel()
	{
		InputLockManager.RemoveControlLock("FlagBrowser");
		button.interactable = true;
		OnFlagCancelled();
	}
}

using ns9;
using UnityEngine;

public class FlagPoleFacility : SpaceCenterBuilding
{
	public GameObject FlagBrowserPrefab;

	public FlagBrowser browser;

	public override void OnClicked()
	{
		if (HighLogic.CurrentGame.Parameters.SpaceCenter.CanSelectFlag)
		{
			SpawnBrowser();
			return;
		}
		InputLockManager.SetControlLock(ControlTypes.KSC_ALL, "flagPoleFacility");
		PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("FacilityLocked", Localizer.Format("#autoLOC_7003201"), Localizer.Format("#autoLOC_6002147"), HighLogic.UISkin, new DialogGUIButton(Localizer.Format("#autoLOC_253299"), OnPopupWarningDismiss)), persistAcrossScenes: false, HighLogic.UISkin).OnDismiss = OnPopupWarningDismiss;
	}

	public void OnPopupWarningDismiss()
	{
		InputLockManager.RemoveControlLock("flagPoleFacility");
	}

	public void SpawnBrowser()
	{
		browser = Object.Instantiate(FlagBrowserPrefab).GetComponent<FlagBrowser>();
		browser.OnDismiss = OnFlagCancel;
		browser.OnFlagSelected = OnFlagSelect;
		InputLockManager.SetControlLock(ControlTypes.KSC_ALL, "flagPoleFacility");
	}

	public void OnFlagSelect(FlagBrowser.FlagEntry selected)
	{
		if (HighLogic.CurrentGame != null)
		{
			HighLogic.CurrentGame.flagURL = selected.textureInfo.name;
			GameEvents.onFlagSelect.Fire(HighLogic.CurrentGame.flagURL);
		}
		GameEvents.onFlagSelect.Fire(selected.textureInfo.name);
		InputLockManager.RemoveControlLock("flagPoleFacility");
	}

	public void OnFlagCancel()
	{
		InputLockManager.RemoveControlLock("flagPoleFacility");
	}
}

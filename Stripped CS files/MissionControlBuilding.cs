using Contracts;
using ns9;
using UnityEngine;

public class MissionControlBuilding : SpaceCenterBuilding
{
	public bool dialogIsUp;

	public override bool IsOpen()
	{
		return ContractSystem.Instance != null;
	}

	public override void OnClicked()
	{
		InputLockManager.SetControlLock(ControlTypes.KSC_ALL, "missionControlFacility");
		if (HighLogic.CurrentGame.Parameters.SpaceCenter.CanGoToMissionControl)
		{
			if (ContractSystem.Instance != null)
			{
				GameEvents.onGUIMissionControlSpawn.Fire();
				GameEvents.onGUIMissionControlDespawn.Add(onDialogClose);
				dialogIsUp = true;
				HighLightBuilding(mouseOverIcon: false);
			}
			else
			{
				PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("FacilityClosed", Localizer.Format("#autoLOC_7003200"), Localizer.Format("#autoLOC_6001645"), HighLogic.UISkin, new DialogGUIButton(Localizer.Format("#autoLOC_253299"), OnPopupWarningDismiss)), persistAcrossScenes: false, HighLogic.UISkin).OnDismiss = OnPopupWarningDismiss;
			}
		}
		else
		{
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("FacilityLocked", Localizer.Format("#autoLOC_7003201"), Localizer.Format("#autoLOC_6001645"), HighLogic.UISkin, new DialogGUIButton(Localizer.Format("#autoLOC_253299"), OnPopupWarningDismiss)), persistAcrossScenes: false, HighLogic.UISkin).OnDismiss = OnPopupWarningDismiss;
		}
	}

	public void OnPopupWarningDismiss()
	{
		InputLockManager.RemoveControlLock("missionControlFacility");
	}

	public void onDialogClose()
	{
		GameEvents.onGUIMissionControlDespawn.Remove(onDialogClose);
		dialogIsUp = false;
		InputLockManager.RemoveControlLock("missionControlFacility");
	}

	public override void OnOnDestroy()
	{
		if (dialogIsUp)
		{
			GameEvents.onGUIMissionControlDespawn.Remove(onDialogClose);
		}
	}
}

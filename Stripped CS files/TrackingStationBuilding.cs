using ns9;
using UnityEngine;

public class TrackingStationBuilding : SpaceCenterBuilding
{
	public override bool IsOpen()
	{
		return HighLogic.CurrentGame.Mode != Game.Modes.SCENARIO_NON_RESUMABLE;
	}

	public override void OnClicked()
	{
		if (HighLogic.CurrentGame.Mode != Game.Modes.SCENARIO_NON_RESUMABLE)
		{
			if (HighLogic.CurrentGame.Parameters.SpaceCenter.CanGoInTrackingStation)
			{
				GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
				HighLogic.LoadScene(GameScenes.TRACKSTATION);
			}
			else
			{
				InputLockManager.SetControlLock(ControlTypes.KSC_ALL, "TSFacility");
				PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("FacilityLocked", Localizer.Format("#autoLOC_7003201"), Localizer.Format("#autoLOC_6002126"), HighLogic.UISkin, new DialogGUIButton(Localizer.Format("#autoLOC_253299"), OnPopupWarningDismiss)), persistAcrossScenes: false, HighLogic.UISkin).OnDismiss = OnPopupWarningDismiss;
			}
		}
	}

	public void OnPopupWarningDismiss()
	{
		InputLockManager.RemoveControlLock("TSFacility");
	}
}

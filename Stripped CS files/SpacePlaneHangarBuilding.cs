using Expansions.Missions.Runtime;
using ns9;
using UnityEngine;

public class SpacePlaneHangarBuilding : SpaceCenterBuilding
{
	public override bool IsOpen()
	{
		return HighLogic.CurrentGame.Mode != Game.Modes.SCENARIO_NON_RESUMABLE;
	}

	public override void OnClicked()
	{
		if (HighLogic.CurrentGame.Mode == Game.Modes.SCENARIO_NON_RESUMABLE)
		{
			return;
		}
		bool flag = false;
		if (!HighLogic.CurrentGame.Parameters.SpaceCenter.CanGoInSPH && HighLogic.CurrentGame.Mode == Game.Modes.MISSION && MissionSystem.Instance != null)
		{
			for (int i = 0; i < MissionSystem.missions.Count; i++)
			{
				if (MissionSystem.missions[i].situation.VesselsArePending)
				{
					flag = true;
				}
			}
		}
		if (HighLogic.CurrentGame.Parameters.SpaceCenter.CanGoInSPH || flag)
		{
			GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
			EditorFacility editorFacility = EditorFacility.None;
			if (ShipConstruction.ShipConfig != null)
			{
				editorFacility = ShipConstruction.ShipType;
			}
			MonoBehaviour.print("Going to Hangar! CachedShipType: " + editorFacility);
			EditorDriver.StartupBehaviour = ((editorFacility == EditorFacility.const_2) ? EditorDriver.StartupBehaviours.LOAD_FROM_CACHE : EditorDriver.StartupBehaviours.START_CLEAN);
			EditorDriver.StartEditor(EditorFacility.const_2);
		}
		else
		{
			InputLockManager.SetControlLock(ControlTypes.KSC_ALL, "SPHFacility");
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("FacilityLocked", Localizer.Format("#autoLOC_7003201"), Localizer.Format("#autoLOC_6001661"), HighLogic.UISkin, new DialogGUIButton(Localizer.Format("#autoLOC_253299"), OnPopupWarningDismiss)), persistAcrossScenes: false, HighLogic.UISkin).OnDismiss = OnPopupWarningDismiss;
		}
	}

	public void OnPopupWarningDismiss()
	{
		InputLockManager.RemoveControlLock("SPHFacility");
	}
}

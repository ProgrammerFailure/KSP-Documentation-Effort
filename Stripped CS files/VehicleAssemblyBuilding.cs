using Expansions.Missions.Runtime;
using ns9;
using UnityEngine;

public class VehicleAssemblyBuilding : SpaceCenterBuilding
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
		if (!HighLogic.CurrentGame.Parameters.SpaceCenter.CanGoInVAB && HighLogic.CurrentGame.Mode == Game.Modes.MISSION && MissionSystem.Instance != null)
		{
			for (int i = 0; i < MissionSystem.missions.Count; i++)
			{
				if (MissionSystem.missions[i].situation.VesselsArePending)
				{
					flag = true;
				}
			}
		}
		if (HighLogic.CurrentGame.Parameters.SpaceCenter.CanGoInVAB || flag)
		{
			EditorFacility editorFacility = EditorFacility.None;
			if (ShipConstruction.ShipConfig != null)
			{
				editorFacility = ShipConstruction.ShipType;
			}
			EditorDriver.StartupBehaviour = ((editorFacility == EditorFacility.const_1) ? EditorDriver.StartupBehaviours.LOAD_FROM_CACHE : EditorDriver.StartupBehaviours.START_CLEAN);
			GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
			EditorDriver.StartEditor(EditorFacility.const_1);
		}
		else
		{
			InputLockManager.SetControlLock(ControlTypes.KSC_ALL, "VABFacility");
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("FacilityLocked", Localizer.Format("#autoLOC_7003201"), Localizer.Format("#autoLOC_418766"), HighLogic.UISkin, new DialogGUIButton(Localizer.Format("#autoLOC_253299"), OnPopupWarningDismiss)), persistAcrossScenes: false, HighLogic.UISkin).OnDismiss = OnPopupWarningDismiss;
		}
	}

	public void OnPopupWarningDismiss()
	{
		InputLockManager.RemoveControlLock("VABFacility");
	}
}

using ns9;
using UnityEngine;

public class RnDBuilding : SpaceCenterBuilding
{
	public bool dialogIsUp;

	public override bool IsOpen()
	{
		return ResearchAndDevelopment.Instance != null;
	}

	public override void OnClicked()
	{
		InputLockManager.SetControlLock(ControlTypes.KSC_ALL, "RnDFacility");
		if (HighLogic.CurrentGame.Parameters.SpaceCenter.CanGoToRnD)
		{
			if (ResearchAndDevelopment.Instance != null)
			{
				GameEvents.onGUIRnDComplexSpawn.Fire();
				GameEvents.onGUIRnDComplexDespawn.Add(onDialogClose);
				dialogIsUp = true;
				HighLightBuilding(mouseOverIcon: false);
			}
			else
			{
				PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("FacilityClosed", Localizer.Format("#autoLOC_7003200"), Localizer.Format("#autoLOC_6001646"), HighLogic.UISkin, new DialogGUIButton(Localizer.Format("#autoLOC_253299"), OnPopupWarningDismiss)), persistAcrossScenes: false, HighLogic.UISkin).OnDismiss = OnPopupWarningDismiss;
			}
		}
		else
		{
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("FacilityLocked", Localizer.Format("#autoLOC_7003201"), Localizer.Format("#autoLOC_6001646"), HighLogic.UISkin, new DialogGUIButton(Localizer.Format("#autoLOC_253299"), OnPopupWarningDismiss)), persistAcrossScenes: false, HighLogic.UISkin).OnDismiss = OnPopupWarningDismiss;
		}
	}

	public void OnPopupWarningDismiss()
	{
		InputLockManager.RemoveControlLock("RnDFacility");
	}

	public void onDialogClose()
	{
		GameEvents.onGUIRnDComplexDespawn.Remove(onDialogClose);
		dialogIsUp = false;
		InputLockManager.RemoveControlLock("RnDFacility");
	}

	public override void OnOnDestroy()
	{
		if (dialogIsUp)
		{
			GameEvents.onGUIRnDComplexDespawn.Remove(onDialogClose);
		}
	}
}

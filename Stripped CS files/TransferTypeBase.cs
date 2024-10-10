using System;
using System.Collections.Generic;
using ns2;
using ns9;
using UnityEngine;

[Serializable]
public abstract class TransferTypeBase
{
	public enum DrawReason
	{
		ManeuverChanged,
		VesselChanged,
		TransferTypeChanged,
		AppShown
	}

	public TransferDataBase currentSelectedTransfer;

	public TransferDataTopDataBase currentSelectTopData;

	public TransferTypeBase()
	{
	}

	public abstract string DisplayName();

	public abstract bool ChangePosition(ManeuverNode node, out string errorString);

	public abstract void ChangeTime();

	public abstract void DrawTypeControls(ManeuverToolUIFrame appFrame, DrawReason reason);

	public abstract void UpdateTransferData();

	public abstract bool CreateManeuver(out ManeuverNode newNode, out string alarmTitle, out string alarmDesc);

	public abstract void OnUpdate();

	public abstract void HideApp();

	public virtual bool PriorManeuversCheck(TransferDataBase transferData, Callback onOk, Callback onCancel)
	{
		if (!(transferData.vessel == null) && !(transferData.vessel.patchedConicSolver == null) && transferData.vessel.patchedConicSolver.maneuverNodes.Count != 0)
		{
			int num = ((transferData.startUT == 0.0) ? transferData.positionNodeIdx : (transferData.positionNodeIdx + 1));
			while (true)
			{
				if (num < transferData.vessel.patchedConicSolver.maneuverNodes.Count)
				{
					if (!(transferData.vessel.patchedConicSolver.maneuverNodes[num].double_0 >= transferData.startBurnTime))
					{
						break;
					}
					num++;
					continue;
				}
				return false;
			}
			DialogGUIButton dialogGUIButton = null;
			List<DialogGUIBase> list = new List<DialogGUIBase>();
			dialogGUIButton = new DialogGUIButton(Localizer.Format("#autoLOC_900536"), onOk, 100f, -1f, true);
			list.Add(dialogGUIButton);
			dialogGUIButton = new DialogGUIButton(Localizer.Format("#autoLOC_900535"), onCancel, 100f, -1f, true);
			list.Add(dialogGUIButton);
			DialogGUIHorizontalLayout dialogGUIHorizontalLayout = new DialogGUIHorizontalLayout(350f, 20f, list.ToArray());
			MultiOptionDialog dialog = new MultiOptionDialog("PriorManeuvers", Localizer.Format("#autoLOC_6002677"), Localizer.Format("#autoLOC_6002678"), HighLogic.UISkin, 350f, dialogGUIHorizontalLayout);
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), dialog, persistAcrossScenes: false, HighLogic.UISkin, isModal: false);
			return true;
		}
		return false;
	}

	public virtual bool FollowingManeuversCheck(TransferDataBase transferData, Callback onOk, Callback onLeave, Callback onCancel)
	{
		if (!(transferData.vessel == null) && !(transferData.vessel.patchedConicSolver == null) && transferData.vessel.patchedConicSolver.maneuverNodes.Count != 0)
		{
			int num = 0;
			while (true)
			{
				if (num < transferData.vessel.patchedConicSolver.maneuverNodes.Count)
				{
					if (!(transferData.vessel.patchedConicSolver.maneuverNodes[num].double_0 <= transferData.startBurnTime))
					{
						break;
					}
					num++;
					continue;
				}
				return false;
			}
			DialogGUIButton dialogGUIButton = null;
			List<DialogGUIBase> list = new List<DialogGUIBase>();
			dialogGUIButton = new DialogGUIButton(Localizer.Format("#autoLOC_900536"), onOk, 100f, -1f, true);
			list.Add(dialogGUIButton);
			dialogGUIButton = new DialogGUIButton(Localizer.Format("#autoLOC_6002676"), onLeave, 100f, -1f, true);
			list.Add(dialogGUIButton);
			dialogGUIButton = new DialogGUIButton(Localizer.Format("#autoLOC_900535"), onCancel, 100f, -1f, true);
			list.Add(dialogGUIButton);
			DialogGUIHorizontalLayout dialogGUIHorizontalLayout = new DialogGUIHorizontalLayout(350f, 20f, list.ToArray());
			MultiOptionDialog dialog = new MultiOptionDialog("SubsequentManeuvers", Localizer.Format("#autoLOC_6002679"), Localizer.Format("#autoLOC_6002680"), HighLogic.UISkin, 350f, dialogGUIHorizontalLayout);
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), dialog, persistAcrossScenes: false, HighLogic.UISkin, isModal: false);
			return true;
		}
		return false;
	}
}

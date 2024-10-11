using System;
using System.Runtime.CompilerServices;
using KSP.UI;

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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TransferTypeBase()
	{
		throw null;
	}

	public abstract string DisplayName();

	public abstract bool ChangePosition(ManeuverNode node, out string errorString);

	public abstract void ChangeTime();

	public abstract void DrawTypeControls(ManeuverToolUIFrame appFrame, DrawReason reason);

	public abstract void UpdateTransferData();

	public abstract bool CreateManeuver(out ManeuverNode newNode, out string alarmTitle, out string alarmDesc);

	public abstract void OnUpdate();

	public abstract void HideApp();

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool PriorManeuversCheck(TransferDataBase transferData, Callback onOk, Callback onCancel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool FollowingManeuversCheck(TransferDataBase transferData, Callback onOk, Callback onLeave, Callback onCancel)
	{
		throw null;
	}
}

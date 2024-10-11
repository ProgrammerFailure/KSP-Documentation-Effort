using System.Runtime.CompilerServices;
using KSP.UI.Util;

namespace KSP.UI.Screens.SpaceCenter.MissionSummaryDialog;

public class PartWidget : MissionSummaryWidget
{
	public AvailablePart partInfo;

	public float partValue;

	public float resourcesValue;

	public float totalValue;

	public int count;

	public ImgText partWidgetQtyContent;

	public ImgText partWidgetPartValueContent;

	public ImgText partWidgetTotalValueContent;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartWidget()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static PartWidget Create(AvailablePart partInfo, float dryCost, float fuelCost, MissionRecoveryDialog missionRecoveryDialog)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static PartWidget Create(AvailablePart partInfo, float dryCost, float fuelCost, int stackQuantity, MissionRecoveryDialog missionRecoveryDialog)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateFields()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddDuplicate(float fuelValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Equals(object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int GetHashCode()
	{
		throw null;
	}
}

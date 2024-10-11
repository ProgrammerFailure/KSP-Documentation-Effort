using System.Runtime.CompilerServices;
using KSP.UI.Util;

namespace KSP.UI.Screens.SpaceCenter.MissionSummaryDialog;

public class ResourceWidget : MissionSummaryWidget
{
	public PartResourceDefinition rscDef;

	public float unitValue;

	public float amount;

	public float totalValue;

	public ImgText rscWidgetQtyContent;

	public ImgText rscWidgetUnitValueContent;

	public ImgText rscWidgetTotalValueContent;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ResourceWidget()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ResourceWidget Create(PartResourceDefinition rscDef, float amount, float unitCost, MissionRecoveryDialog missionRecoveryDialog)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateFields()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddAmount(float amount)
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

using System.Runtime.CompilerServices;
using KSP.UI;
using PreFlightTests;

namespace Expansions.Missions;

public class VesselRestriction : DynamicModule
{
	protected UIListItem UIEntry;

	private UIStateImage UIEntryState;

	private static double epsilon;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselRestriction()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselRestriction(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static VesselRestriction()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SuscribeToEvents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void ClearEvents()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddAppUIReference(ref UIListItem listItem, ref UIStateImage listItemState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearAppUIREference()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UpdateAppEntry()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual string GetStateMessage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool IsComplete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool SameComparatorWrapper(VesselRestriction otherRestriction)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool SameComparator(VesselRestriction otherRestriction)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual IPreFlightTest GetPreflightCheck()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static bool TestFloat(float currentValue, float targetValue, TestComparisonLessGreaterEqual comparisonOperator)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool TestInt(int currentValue, int targetValue, TestComparisonLessGreaterEqual comparisonOperator)
	{
		throw null;
	}
}

using ns9;

namespace PreFlightTests;

public class ComOffset : DesignConcernBase
{
	public static string cacheAutoLOC_251096;

	public static string cacheAutoLOC_251101;

	public ComOffset(ShipConstruct ship)
	{
	}

	public override bool TestCondition()
	{
		return true;
	}

	public override string GetConcernTitle()
	{
		return cacheAutoLOC_251096;
	}

	public override string GetConcernDescription()
	{
		return cacheAutoLOC_251101;
	}

	public override EditorFacilities GetEditorFacilities()
	{
		return EditorFacilities.flag_2;
	}

	public override DesignConcernSeverity GetSeverity()
	{
		return DesignConcernSeverity.CRITICAL;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_251096 = Localizer.Format("#autoLOC_251096");
		cacheAutoLOC_251101 = Localizer.Format("#autoLOC_251101");
	}
}

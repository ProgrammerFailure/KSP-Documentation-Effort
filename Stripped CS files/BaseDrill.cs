using ns9;

public class BaseDrill : BaseConverter
{
	[KSPField]
	public float ImpactRange = 5f;

	[KSPField]
	public string ImpactTransform = "";

	[KSPField]
	public float Efficiency = 1f;

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLoc_6003021");
	}
}

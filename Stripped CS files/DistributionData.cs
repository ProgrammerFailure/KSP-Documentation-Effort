public class DistributionData
{
	public float PresenceChance { get; set; }

	public float MinAbundance { get; set; }

	public float MaxAbundance { get; set; }

	public float Variance { get; set; }

	public float Dispersal { get; set; }

	public float MinAltitude { get; set; }

	public float MaxAltitude { get; set; }

	public float MinRange { get; set; }

	public float MaxRange { get; set; }

	public bool HasVariableAltitude()
	{
		if (!((double)MinAltitude > 1E-09))
		{
			return (double)MaxAltitude > 1E-09;
		}
		return true;
	}
}

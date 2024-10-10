public struct AbundanceRequest
{
	public double Latitude { get; set; }

	public double Longitude { get; set; }

	public int BodyId { get; set; }

	public string ResourceName { get; set; }

	public HarvestTypes ResourceType { get; set; }

	public double Altitude { get; set; }

	public bool CheckForLock { get; set; }

	public string BiomeName { get; set; }

	public bool ExcludeVariance { get; set; }
}

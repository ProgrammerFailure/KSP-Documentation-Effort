using System.Collections.Generic;

public class DepletionData
{
	public int PlanetId { get; set; }

	public string ResourceName { get; set; }

	public List<DepletionNode> DepletionNodes { get; set; }

	public DepletionData()
	{
		DepletionNodes = new List<DepletionNode>();
	}
}

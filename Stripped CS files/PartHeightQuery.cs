using System.Collections.Generic;

public class PartHeightQuery
{
	public float lowestPoint;

	public Dictionary<Part, float> lowestOnParts;

	public PartHeightQuery(float lowest)
	{
		lowestPoint = lowest;
		lowestOnParts = new Dictionary<Part, float>();
	}
}

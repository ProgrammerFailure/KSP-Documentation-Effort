using System.Collections.Generic;

namespace Expansions.Missions.Flow;

public class MEPathGroup
{
	public MENode joinNode;

	public int minHops;

	public List<MEPath> Paths { get; set; }

	public MEPath First
	{
		get
		{
			if (Paths != null && Paths.Count > 0)
			{
				return Paths[0];
			}
			return null;
		}
	}

	public MEPathGroup(MEPath first)
	{
		Paths = new List<MEPath>();
		Paths.Add(first);
		minHops = 0;
	}
}

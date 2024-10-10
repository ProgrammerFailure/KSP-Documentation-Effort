namespace Expansions.Missions.Flow;

public class MEFlowConvergence
{
	public int Hops { get; set; }

	public MENode Node { get; set; }

	public MEFlowConvergence(MENode node, int hops)
	{
		Node = node;
		Hops = hops;
	}
}

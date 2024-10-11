namespace Expansions.Missions.Flow;

public interface IMEFlowBlock
{
	bool HasVisibleChildren { get; }

	bool HasReachableObjectives { get; }

	bool HasObjectives { get; }

	int CountObjectives { get; }

	void UpdateMissionFlowUI(MEFlowParser parser);
}

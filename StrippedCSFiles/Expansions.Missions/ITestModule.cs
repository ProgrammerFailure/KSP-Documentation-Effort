using Expansions.Missions.Editor;

namespace Expansions.Missions;

public interface ITestModule : IConfigNode, IMENodeDisplay
{
	bool Test();

	bool ShouldCreateCheckpoint();

	void RunValidationWrapper(MissionEditorValidator validator);

	void Initialize(TestGroup testGroup, string title = "");

	TestModule InitializeTest();

	TestModule ClearTest();

	string GetAppObjectiveInfo();
}

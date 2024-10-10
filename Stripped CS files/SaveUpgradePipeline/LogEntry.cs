namespace SaveUpgradePipeline;

public class LogEntry
{
	public TestResult testResult;

	public bool upgraded;

	public LogEntry(TestResult testResult, bool upgraded)
	{
		this.testResult = testResult;
		this.upgraded = upgraded;
	}

	public override string ToString()
	{
		return testResult switch
		{
			TestResult.Failed => string.Concat("<color=red>", testResult, "</color> \t\t| ", upgraded ? "Upgraded" : "Not Upgraded"), 
			TestResult.TooEarly => string.Concat("<color=yellow>", testResult, "</color> \t\t| ", upgraded ? "Upgraded" : "Not Upgraded"), 
			TestResult.Pass => string.Concat("<color=green>", testResult, "</color> \t\t| ", upgraded ? "<color=green>Upgraded</color>" : "Not Upgraded"), 
			_ => string.Concat(testResult, " \t| ", upgraded ? "<color=green>Upgraded</color>" : "<color=yellow>Not Upgraded</color>"), 
		};
	}
}

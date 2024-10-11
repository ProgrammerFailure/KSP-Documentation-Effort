using System.Runtime.CompilerServices;

namespace SaveUpgradePipeline;

public class LogEntry
{
	public TestResult testResult;

	public bool upgraded;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LogEntry(TestResult testResult, bool upgraded)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string ToString()
	{
		throw null;
	}
}

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace KSP.Testing;

public class TestResults
{
	public int failed;

	public int success;

	public List<TestState> states;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TestResults()
	{
		throw null;
	}
}

using System.Runtime.CompilerServices;

namespace KSP.Testing;

public class TestState
{
	public TestInfo Info;

	public bool Succeeded;

	public string Reason;

	public string Details;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TestState()
	{
		throw null;
	}
}

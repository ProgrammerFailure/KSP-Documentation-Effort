using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace KSP.Testing;

public abstract class UnitTest
{
	public List<TestState> Results;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UnitTest()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void TestStartUp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void TestTearDown()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void assertEquals(string testname, object value, object shouldbe)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal IEnumerable<TestState> PerformTest()
	{
		throw null;
	}
}

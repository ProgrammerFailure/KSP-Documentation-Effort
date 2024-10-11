using System;
using System.Runtime.CompilerServices;

namespace Smooth.Slinq.Context;

public class BacktrackException : InvalidOperationException
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public BacktrackException()
	{
		throw null;
	}
}

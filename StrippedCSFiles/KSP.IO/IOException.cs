using System;
using System.Runtime.CompilerServices;

namespace KSP.IO;

public class IOException : Exception
{
	protected string message;

	protected string source;

	protected string stack;

	public override string Message
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public override string Source
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public override string StackTrace
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public IOException(string message, string source, string stack)
	{
		throw null;
	}
}

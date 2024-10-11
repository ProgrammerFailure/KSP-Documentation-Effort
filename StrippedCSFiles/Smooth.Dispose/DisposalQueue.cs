using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Smooth.Dispose;

public static class DisposalQueue
{
	private static readonly object queueLock;

	private static Queue<IDisposable> enqueue;

	private static Queue<IDisposable> dispose;

	[MethodImpl(MethodImplOptions.NoInlining)]
	static DisposalQueue()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Enqueue(IDisposable item)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Pulse()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Dispose()
	{
		throw null;
	}
}

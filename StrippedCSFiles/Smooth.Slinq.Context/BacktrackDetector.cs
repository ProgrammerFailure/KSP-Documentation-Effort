using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Smooth.Slinq.Context;

public struct BacktrackDetector
{
	private class ReferenceContext
	{
		public int borrowId;

		public int touchId;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ReferenceContext()
		{
			throw null;
		}
	}

	private static readonly BacktrackDetector noDetection;

	private static readonly Stack<ReferenceContext> contextPool;

	public static readonly bool enabled;

	private readonly ReferenceContext context;

	private int borrowId;

	private int touchId;

	[MethodImpl(MethodImplOptions.NoInlining)]
	private BacktrackDetector(ReferenceContext context)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static BacktrackDetector()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static BacktrackDetector Borrow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Conditional("DETECT_BACKTRACK")]
	public void DetectBacktrack()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Conditional("DETECT_BACKTRACK")]
	public void Release()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Conditional("DETECT_BACKTRACK")]
	public void TryReleaseShared()
	{
		throw null;
	}
}

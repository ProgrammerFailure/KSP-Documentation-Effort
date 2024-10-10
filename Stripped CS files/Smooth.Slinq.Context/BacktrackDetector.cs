using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Smooth.Slinq.Context;

public struct BacktrackDetector
{
	public class ReferenceContext
	{
		public int borrowId;

		public int touchId;
	}

	public static readonly BacktrackDetector noDetection;

	public static readonly Stack<ReferenceContext> contextPool;

	public static readonly bool enabled;

	public readonly ReferenceContext context;

	public int borrowId;

	public int touchId;

	public BacktrackDetector(ReferenceContext context)
	{
		this.context = context;
		borrowId = context.borrowId;
		touchId = context.touchId;
	}

	static BacktrackDetector()
	{
		contextPool = null;
		enabled = false;
		if (enabled && Application.isPlaying)
		{
			UnityEngine.Debug.Log("Smooth.Slinq is running with backtrack detection enabled which will significantly reduce performance and should only be used for debugging purposes.");
		}
	}

	public static BacktrackDetector Borrow()
	{
		if (enabled)
		{
			lock (contextPool)
			{
				return new BacktrackDetector((contextPool.Count > 0) ? contextPool.Pop() : new ReferenceContext());
			}
		}
		return noDetection;
	}

	[Conditional("DETECT_BACKTRACK")]
	public void DetectBacktrack()
	{
		lock (context)
		{
			if (context.borrowId != borrowId || context.touchId != touchId)
			{
				throw new BacktrackException();
			}
			context.touchId = ++touchId;
		}
	}

	[Conditional("DETECT_BACKTRACK")]
	public void Release()
	{
		lock (context)
		{
			if (context.borrowId != borrowId || context.touchId != touchId)
			{
				throw new BacktrackException();
			}
			context.borrowId++;
		}
		lock (contextPool)
		{
			contextPool.Push(context);
		}
	}

	[Conditional("DETECT_BACKTRACK")]
	public void TryReleaseShared()
	{
		lock (context)
		{
			if (context.borrowId != borrowId)
			{
				return;
			}
			context.borrowId++;
		}
		lock (contextPool)
		{
			contextPool.Push(context);
		}
	}
}

using System.Collections.Generic;
using System.Runtime.CompilerServices;

public static class ProfileTimer
{
	private class TimerInstance
	{
		public float start
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			private set
			{
				throw null;
			}
		}

		public string name
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			private set
			{
				throw null;
			}
		}

		public float duration
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public TimerInstance(string name)
		{
			throw null;
		}
	}

	private static string timeFormat;

	private static List<TimerInstance> stack;

	private static int stackCount;

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ProfileTimer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void OutputTimer(TimerInstance timer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Push(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Pop()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Pop(string name)
	{
		throw null;
	}
}

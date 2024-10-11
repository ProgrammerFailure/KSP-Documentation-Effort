using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class EventValueComparison<T> : BaseGameEvent
{
	private class EvtDelegate
	{
		public OnEvent evt;

		public object originator;

		public string originatorType;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public EvtDelegate(OnEvent evt)
		{
			throw null;
		}
	}

	public delegate bool OnCompare(T a, T b);

	public delegate T OnEvent();

	private List<EvtDelegate> events;

	private List<EvtDelegate> eventsClone;

	private int numEventsFiring;

	private T defaultValue;

	private OnCompare comparer;

	public new string eventName
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

	public T value
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EventValueComparison(string eventName, T defaultValue, OnCompare comparer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Add(OnEvent evt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Remove(OnEvent evt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Update()
	{
		throw null;
	}
}

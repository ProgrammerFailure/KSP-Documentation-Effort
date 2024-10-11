using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class EventIntMin : BaseGameEvent
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

	public delegate int OnEvent();

	private List<EvtDelegate> events;

	private List<EvtDelegate> eventsClone;

	private int numEventsFiring;

	public int defaultValue;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EventIntMin(string eventName, int defaultValue = 0)
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
	public int Fire()
	{
		throw null;
	}
}

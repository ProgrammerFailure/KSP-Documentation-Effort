using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class EventDoubleMin : BaseGameEvent
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

	public delegate double OnEvent();

	private List<EvtDelegate> events;

	private List<EvtDelegate> eventsClone;

	private int numEventsFiring;

	public double defaultValue;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EventDoubleMin(string eventName, double defaultValue = 0.0)
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
	public double Fire()
	{
		throw null;
	}
}

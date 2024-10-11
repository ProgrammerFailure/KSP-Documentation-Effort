using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class EventVoid : BaseGameEvent
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

	public delegate void OnEvent();

	private List<EvtDelegate> events;

	private List<EvtDelegate> eventsClone;

	private int numEventsFiring;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EventVoid(string eventName)
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
	public void Fire()
	{
		throw null;
	}
}

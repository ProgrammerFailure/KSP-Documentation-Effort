using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class EventFloatMin : BaseGameEvent
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

	public delegate float OnEvent();

	private List<EvtDelegate> events;

	private List<EvtDelegate> eventsClone;

	private int numEventsFiring;

	public float defaultValue;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EventFloatMin(string eventName, float defaultValue = 0f)
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
	public float Fire()
	{
		throw null;
	}
}

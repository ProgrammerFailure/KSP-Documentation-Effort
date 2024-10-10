using System.Collections.Generic;
using UnityEngine;

public class EventValueOperation<T> : BaseGameEvent
{
	public class EvtDelegate
	{
		public OnEvent evt;

		public object originator;

		public string originatorType;

		public EvtDelegate(OnEvent evt)
		{
			this.evt = evt;
			originator = evt.Target;
			originatorType = evt.Target.GetType().Name;
		}
	}

	public delegate T OnOperation(T a, T b);

	public delegate T OnEvent();

	public List<EvtDelegate> events;

	public List<EvtDelegate> eventsClone = new List<EvtDelegate>(16);

	public int numEventsFiring;

	public T defaultValue;

	public OnOperation operation;

	public string gameEventName { get; set; }

	public T value { get; set; }

	public EventValueOperation(string eventName, T defaultValue, OnOperation operation)
		: base(eventName)
	{
		events = new List<EvtDelegate>();
		this.defaultValue = defaultValue;
		value = defaultValue;
		this.operation = operation;
	}

	public void Add(OnEvent evt)
	{
		EvtDelegate evtDelegate = new EvtDelegate(evt);
		if (debugEvent || GameEventsBase.debugEvents)
		{
			Debug.Log("EventManager: Adding event '" + gameEventName + "' for object of type '" + evtDelegate.originatorType + "'");
		}
		events.Add(evtDelegate);
	}

	public void Remove(OnEvent evt)
	{
		int count = events.Count;
		EvtDelegate evtDelegate;
		do
		{
			if (count-- > 0)
			{
				evtDelegate = events[count];
				continue;
			}
			if (debugEvent || GameEventsBase.debugEvents)
			{
				Debug.Log("EventManager: Removing event '" + gameEventName + "' - Cannot find listed event");
			}
			return;
		}
		while (!(evtDelegate.evt == evt));
		if (debugEvent || GameEventsBase.debugEvents)
		{
			Debug.Log("EventManager: Removing event '" + gameEventName + "' from object of type '" + evtDelegate.originatorType + "'");
		}
		events.Remove(evtDelegate);
	}

	public void Update()
	{
		if (debugEvent || GameEventsBase.debugEvents)
		{
			Debug.Log("EventManager: Firing event '" + gameEventName + "'");
		}
		value = defaultValue;
		numEventsFiring++;
		eventsClone.Clear();
		eventsClone.AddRange(events);
		int count = eventsClone.Count;
		while (count-- > 0)
		{
			if (eventsClone[count].originator != null)
			{
				T a = eventsClone[count].evt();
				value = operation(a, value);
				continue;
			}
			Debug.Log("EventManager: Removing event '" + gameEventName + "'for object of type '" + events[count].originatorType + "' as object is null.");
			events.Remove(eventsClone[count]);
			eventsClone.RemoveAt(count);
		}
		numEventsFiring--;
		if (numEventsFiring <= 0)
		{
			eventsClone.Clear();
			numEventsFiring = 0;
		}
	}
}

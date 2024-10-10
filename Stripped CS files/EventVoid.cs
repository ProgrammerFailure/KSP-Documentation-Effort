using System;
using System.Collections.Generic;
using UnityEngine;

public class EventVoid : BaseGameEvent
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

	public delegate void OnEvent();

	public List<EvtDelegate> events;

	public List<EvtDelegate> eventsClone = new List<EvtDelegate>(16);

	public int numEventsFiring;

	public EventVoid(string eventName)
		: base(eventName)
	{
		events = new List<EvtDelegate>();
	}

	public void Add(OnEvent evt)
	{
		EvtDelegate evtDelegate = new EvtDelegate(evt);
		if (debugEvent || GameEventsBase.debugEvents)
		{
			Debug.Log("EventManager: Adding event '" + eventName + "' for object of type '" + evtDelegate.originatorType + "'");
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
				Debug.Log("EventManager: Removing event '" + eventName + "' - Cannot find listed event");
			}
			return;
		}
		while (!(evtDelegate.evt == evt));
		if (debugEvent || GameEventsBase.debugEvents)
		{
			Debug.Log("EventManager: Removing event '" + eventName + "' from object of type '" + evtDelegate.originatorType + "'");
		}
		events.Remove(evtDelegate);
	}

	public void Fire()
	{
		if (debugEvent || GameEventsBase.debugEvents)
		{
			Debug.Log("EventManager: Firing event '" + eventName + "'");
		}
		numEventsFiring++;
		eventsClone.Clear();
		eventsClone.AddRange(events);
		int count = eventsClone.Count;
		while (count-- > 0)
		{
			if (eventsClone[count].originator == null)
			{
				Debug.Log("EventManager: Removing event '" + eventName + "'for object of type '" + events[count].originatorType + "' as object is null.");
				events.Remove(eventsClone[count]);
				eventsClone.RemoveAt(count);
			}
			else
			{
				try
				{
					eventsClone[count].evt();
				}
				catch (Exception ex)
				{
					Debug.LogError($"Exception handling event {eventName} in class {eventsClone[count].originatorType}:" + ex);
					Debug.LogException(ex);
				}
			}
		}
		numEventsFiring--;
		if (numEventsFiring <= 0)
		{
			eventsClone.Clear();
			numEventsFiring = 0;
		}
	}
}

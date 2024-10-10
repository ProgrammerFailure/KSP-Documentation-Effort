using System;
using System.Collections.Generic;
using UnityEngine;

public class EventData<T> : BaseGameEvent
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

	public delegate void OnEvent(T data);

	public List<EvtDelegate> events;

	public List<EvtDelegate> eventsClone = new List<EvtDelegate>(16);

	public int numEventsFiring;

	public EventData(string eventName)
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

	public void Fire(T data)
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
			if (eventsClone[count].originator != null)
			{
				try
				{
					eventsClone[count].evt(data);
				}
				catch (Exception ex)
				{
					Debug.LogError($"Exception handling event {eventName} in class {eventsClone[count].originatorType}:" + ex);
					Debug.LogException(ex);
				}
			}
			else
			{
				Debug.Log("EventManager: Removing event '" + eventName + "'for object of type '" + events[count].originatorType + "' as object is null.");
				events.Remove(eventsClone[count]);
				eventsClone.RemoveAt(count);
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
public class EventData<T, U> : BaseGameEvent
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

	public delegate void OnEvent(T data0, U data1);

	public List<EvtDelegate> events;

	public List<EvtDelegate> eventsClone = new List<EvtDelegate>(16);

	public int numEventsFiring;

	public EventData(string eventName)
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

	public void Fire(T data0, U data1)
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
			if (eventsClone[count].originator != null)
			{
				try
				{
					eventsClone[count].evt(data0, data1);
				}
				catch (Exception ex)
				{
					Debug.LogError($"Exception handling event {eventName} in class {eventsClone[count].originatorType}:" + ex);
					Debug.LogException(ex);
				}
			}
			else
			{
				Debug.Log("EventManager: Removing event '" + eventName + "'for object of type '" + events[count].originatorType + "' as object is null.");
				events.Remove(eventsClone[count]);
				eventsClone.RemoveAt(count);
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
public class EventData<T, U, V> : BaseGameEvent
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

	public delegate void OnEvent(T data0, U data1, V data2);

	public List<EvtDelegate> events;

	public List<EvtDelegate> eventsClone = new List<EvtDelegate>(16);

	public int numEventsFiring;

	public EventData(string eventName)
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

	public void Fire(T data0, U data1, V data2)
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
			if (eventsClone[count].originator != null)
			{
				try
				{
					eventsClone[count].evt(data0, data1, data2);
				}
				catch (Exception ex)
				{
					Debug.LogError($"Exception handling event {eventName} in class {eventsClone[count].originatorType}:" + ex);
					Debug.LogException(ex);
				}
			}
			else
			{
				Debug.Log("EventManager: Removing event '" + eventName + "'for object of type '" + events[count].originatorType + "' as object is null.");
				events.Remove(eventsClone[count]);
				eventsClone.RemoveAt(count);
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
public class EventData<T, U, V, W> : BaseGameEvent
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

	public delegate void OnEvent(T data0, U data1, V data2, W data3);

	public List<EvtDelegate> events;

	public List<EvtDelegate> eventsClone = new List<EvtDelegate>(16);

	public int numEventsFiring;

	public EventData(string eventName)
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

	public void Fire(T data0, U data1, V data2, W data3)
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
			if (eventsClone[count].originator != null)
			{
				try
				{
					eventsClone[count].evt(data0, data1, data2, data3);
				}
				catch (Exception ex)
				{
					Debug.LogError($"Exception handling event {eventName} in class {eventsClone[count].originatorType}:" + ex);
					Debug.LogException(ex);
				}
			}
			else
			{
				Debug.Log("EventManager: Removing event '" + eventName + "'for object of type '" + events[count].originatorType + "' as object is null.");
				events.Remove(eventsClone[count]);
				eventsClone.RemoveAt(count);
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

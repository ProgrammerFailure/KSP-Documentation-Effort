using System.Collections.Generic;
using UnityEngine;

public class BaseGameEvent
{
	public static Dictionary<string, BaseGameEvent> eventsByName = new Dictionary<string, BaseGameEvent>();

	public string eventName;

	public bool debugEvent;

	public string EventName => eventName;

	public BaseGameEvent(string eventName)
	{
		this.eventName = eventName;
		eventsByName[eventName] = this;
	}

	public static T FindEvent<T>(string eventName) where T : BaseGameEvent
	{
		BaseGameEvent value = null;
		eventsByName.TryGetValue(eventName, out value);
		if (value == null)
		{
			return null;
		}
		if (!(value is T result))
		{
			Debug.LogError(StringBuilderCache.Format("Type mismatch when searching for event with name {0}.  Expected type {1}, but found {2}", eventName, typeof(T), value.GetType()));
			return null;
		}
		return result;
	}
}

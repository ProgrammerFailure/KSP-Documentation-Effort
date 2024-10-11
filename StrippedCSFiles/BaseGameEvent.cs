using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class BaseGameEvent
{
	private static Dictionary<string, BaseGameEvent> eventsByName;

	protected string eventName;

	public bool debugEvent;

	public string EventName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public BaseGameEvent(string eventName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static BaseGameEvent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static T FindEvent<T>(string eventName) where T : BaseGameEvent
	{
		throw null;
	}
}

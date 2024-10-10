using System;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class KSPModelEvent : Attribute
{
	public string startEvent;

	public string finishEvent;

	public bool allowMultiple;

	public KSPModelEvent(string startEvent, string finishEvent, bool allowMultiple)
	{
		this.startEvent = startEvent;
		this.finishEvent = finishEvent;
		this.allowMultiple = allowMultiple;
	}
}

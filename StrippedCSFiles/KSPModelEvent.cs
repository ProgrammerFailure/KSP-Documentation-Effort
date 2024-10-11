using System;
using System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class KSPModelEvent : Attribute
{
	public string startEvent;

	public string finishEvent;

	public bool allowMultiple;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPModelEvent(string startEvent, string finishEvent, bool allowMultiple)
	{
		throw null;
	}
}

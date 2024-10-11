using System;
using System.Runtime.CompilerServices;

public class EventValueWrapper
{
	private Action[] updateActions;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EventValueWrapper()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Update()
	{
		throw null;
	}
}

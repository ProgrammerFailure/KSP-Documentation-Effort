using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ColorChangedEvent : UnityEvent<Color>
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public ColorChangedEvent()
	{
		throw null;
	}
}

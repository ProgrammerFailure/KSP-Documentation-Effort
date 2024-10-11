using System.Runtime.CompilerServices;
using UnityEngine.Events;

public class HSVChangedEvent : UnityEvent<float, float, float>
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public HSVChangedEvent()
	{
		throw null;
	}
}

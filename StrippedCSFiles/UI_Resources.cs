using System;
using System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class UI_Resources : UI_Control
{
	public UIPartActionResourceDrain resourcesToDrain_PAW;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UI_Resources()
	{
		throw null;
	}
}

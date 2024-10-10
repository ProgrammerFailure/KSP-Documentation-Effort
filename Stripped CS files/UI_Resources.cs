using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class UI_Resources : UI_Control
{
	public UIPartActionResourceDrain resourcesToDrain_PAW;
}

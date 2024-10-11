using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class UI_VariantSelector : UI_Control
{
	public List<PartVariant> variants;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UI_VariantSelector()
	{
		throw null;
	}
}

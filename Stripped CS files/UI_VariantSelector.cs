using System;
using System.Collections.Generic;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class UI_VariantSelector : UI_Control
{
	public List<PartVariant> variants;
}

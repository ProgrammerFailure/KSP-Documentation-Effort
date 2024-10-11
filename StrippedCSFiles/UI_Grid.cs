using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class UI_Grid : UI_Control
{
	public List<string> inventoryItems;

	public int columnCount;

	public bool updateSlotItems;

	public ModuleInventoryPart inventoryPart;

	public UIPartActionInventory pawInventory;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UI_Grid()
	{
		throw null;
	}
}

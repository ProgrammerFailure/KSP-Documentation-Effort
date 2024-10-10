using System;
using System.Collections.Generic;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class UI_Grid : UI_Control
{
	public List<string> inventoryItems;

	public int columnCount = 3;

	public bool updateSlotItems;

	public ModuleInventoryPart inventoryPart;

	public UIPartActionInventory pawInventory;
}

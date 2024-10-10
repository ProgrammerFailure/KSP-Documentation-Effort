using System.Collections.Generic;

public interface IShipConstructIDChanges
{
	void UpdatePersistentIDs(Dictionary<uint, uint> changedIDs);
}

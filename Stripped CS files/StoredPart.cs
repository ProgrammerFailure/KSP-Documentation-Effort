using System;

[Serializable]
public class StoredPart
{
	public int slotIndex = -1;

	public string partName;

	public ProtoPartSnapshot snapshot;

	public int quantity;

	public int stackCapacity = 1;

	public string variantName = "";

	public bool IsEmpty
	{
		get
		{
			if (snapshot != null)
			{
				return quantity < 1;
			}
			return false;
		}
	}

	public bool IsFull => quantity < stackCapacity;

	public bool CanStack
	{
		get
		{
			if (snapshot != null)
			{
				return stackCapacity > 1;
			}
			return false;
		}
	}

	public StoredPart(string partName, int slotIndex)
	{
		this.partName = partName;
		this.slotIndex = slotIndex;
	}

	public StoredPart(ConfigNode node)
	{
		Load(node);
	}

	public void Load(ConfigNode node)
	{
		node.TryGetValue("slotIndex", ref slotIndex);
		node.TryGetValue("partName", ref partName);
		node.TryGetValue("quantity", ref quantity);
		node.TryGetValue("stackCapacity", ref stackCapacity);
		node.TryGetValue("variantName", ref variantName);
		if (node.HasNode("PART"))
		{
			snapshot = new ProtoPartSnapshot(node.GetNode("PART"), null, null);
		}
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("slotIndex", slotIndex);
		node.AddValue("partName", partName);
		node.AddValue("quantity", quantity);
		node.AddValue("stackCapacity", stackCapacity);
		node.AddValue("variantName", variantName);
		if (snapshot != null)
		{
			snapshot.Save(node.AddNode("PART"));
		}
	}

	public StoredPart Copy()
	{
		StoredPart storedPart = new StoredPart(partName, slotIndex);
		storedPart.variantName = variantName;
		storedPart.stackCapacity = stackCapacity;
		storedPart.quantity = quantity;
		if (snapshot != null)
		{
			ConfigNode node = new ConfigNode();
			snapshot.Save(node);
			storedPart.snapshot = new ProtoPartSnapshot(node, snapshot.pVesselRef, null);
		}
		else
		{
			snapshot = null;
		}
		return storedPart;
	}
}

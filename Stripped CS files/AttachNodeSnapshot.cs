public class AttachNodeSnapshot
{
	public string id;

	public int partIdx;

	public string srfAttachMeshName;

	public AttachNodeSnapshot(AttachNode node, ProtoVessel pVesselRef)
	{
		id = node.id;
		srfAttachMeshName = node.srfAttachMeshName;
		partIdx = -1;
		if (!(node.attachedPart != null))
		{
			return;
		}
		int num = 0;
		Part part;
		while (true)
		{
			if (num < pVesselRef.vesselRef.parts.Count)
			{
				part = pVesselRef.vesselRef.parts[num];
				if (part == node.attachedPart)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		partIdx = pVesselRef.vesselRef.parts.IndexOf(part);
	}

	public AttachNodeSnapshot(string attachNodeString)
	{
		string[] array = attachNodeString.Split(',');
		id = array[0].Trim();
		partIdx = int.Parse(array[1].Trim());
		if (array.Length > 2)
		{
			srfAttachMeshName = array[2];
		}
		else
		{
			srfAttachMeshName = "";
		}
	}

	public string Save()
	{
		string text = id + ", " + partIdx;
		if (!string.IsNullOrEmpty(srfAttachMeshName))
		{
			text = text + "," + srfAttachMeshName;
		}
		return text;
	}

	public Part findAttachedPart(Vessel vessel)
	{
		if (partIdx == -1)
		{
			return null;
		}
		return vessel.parts[partIdx];
	}
}

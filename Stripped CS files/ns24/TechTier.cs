using System;

namespace ns24;

[Serializable]
public class TechTier : PartCategory
{
	public float minScienceCost;

	public float maxScienceCost;

	public override bool ExclusionCriteria(AvailablePart aP)
	{
		ProtoTechNode protoTechNode = AssetBase.RnDTechTree.FindTech(aP.TechRequired);
		float num = float.MaxValue;
		if (protoTechNode != null)
		{
			num = protoTechNode.scienceCost;
		}
		if (num >= minScienceCost && num < maxScienceCost)
		{
			return true;
		}
		return false;
	}
}

public class CometDefinition
{
	public string typeName;

	public double vfxStartDistance;

	public double comaWidthRatio;

	public double tailWidthRatio;

	public double tailMaxLengthRatio;

	public double ionTailWidthRatio;

	public float radius;

	public int seed;

	public CometOrbitType cometType;

	public int numGeysers;

	public int numNearDustEmitters;

	public ConfigNode CreateVesselNode(bool optimizedCollider, float fragmentDynamicPressureModifier, bool hasName)
	{
		ConfigNode configNode = new ConfigNode("CometVessel");
		configNode.AddValue("typeName", typeName);
		configNode.AddValue("vfxStartDistance", vfxStartDistance);
		configNode.AddValue("comaWidthRatio", comaWidthRatio);
		configNode.AddValue("tailWidthRatio", tailWidthRatio);
		configNode.AddValue("ionTailWidthRatio", ionTailWidthRatio);
		configNode.AddValue("tailMaxLengthRatio", tailMaxLengthRatio);
		configNode.AddValue("seed", seed);
		configNode.AddValue("radius", radius);
		configNode.AddValue("numGeysers", numGeysers);
		configNode.AddValue("numNearDustEmitters", numNearDustEmitters);
		configNode.AddValue("optimizedCollider", optimizedCollider);
		configNode.AddValue("fragmentDynamicPressureModifier", fragmentDynamicPressureModifier);
		if (hasName)
		{
			configNode.AddValue("hasName", value: true);
		}
		return configNode;
	}
}

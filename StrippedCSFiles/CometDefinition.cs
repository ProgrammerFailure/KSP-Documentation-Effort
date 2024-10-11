using System.Runtime.CompilerServices;

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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CometDefinition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode CreateVesselNode(bool optimizedCollider, float fragmentDynamicPressureModifier, bool hasName)
	{
		throw null;
	}
}

using System.Runtime.CompilerServices;

public class PlanetarySample
{
	public string sampleName;

	public float sampleMass;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PlanetarySample()
	{
		throw null;
	}
}

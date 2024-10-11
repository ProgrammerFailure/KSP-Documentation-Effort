using System;
using System.Runtime.CompilerServices;

[Serializable]
public class CelestialBodyScienceParams
{
	public float LandedDataValue;

	public float SplashedDataValue;

	public float FlyingLowDataValue;

	public float FlyingHighDataValue;

	public float InSpaceLowDataValue;

	public float InSpaceHighDataValue;

	public float RecoveryValue;

	public float flyingAltitudeThreshold;

	public float spaceAltitudeThreshold;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CelestialBodyScienceParams()
	{
		throw null;
	}
}

using System;

[Serializable]
public class CelestialBodyScienceParams
{
	public float LandedDataValue = 1f;

	public float SplashedDataValue = 1f;

	public float FlyingLowDataValue = 1f;

	public float FlyingHighDataValue = 1f;

	public float InSpaceLowDataValue = 1f;

	public float InSpaceHighDataValue = 1f;

	public float RecoveryValue = 1f;

	public float flyingAltitudeThreshold = 18000f;

	public float spaceAltitudeThreshold = 250000f;
}

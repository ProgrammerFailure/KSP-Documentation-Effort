using System;

[Serializable]
public class DeltaVPropellantInfo
{
	public Propellant propellant;

	public double amountAvailable;

	public double amountBurnt;

	public double maxBurnTime;

	public double setThrottleBurnTime;

	public double currentBurnTime;

	public double amountPerSecondMaxThrottle;

	public double amountPerSecondSetThrottle;

	public double amountPerSecondCurrentThrottle;

	public double pendingDemand;

	public float timeLeftSetThrottle;

	public float timeLeftCurrentThrottle;

	public DeltaVPropellantInfo(Propellant propellant)
	{
		this.propellant = propellant;
		amountAvailable = 0.0;
		amountBurnt = 0.0;
		maxBurnTime = 0.0;
		amountPerSecondMaxThrottle = 0.0;
		amountPerSecondCurrentThrottle = 0.0;
		amountPerSecondSetThrottle = 0.0;
		pendingDemand = 0.0;
		timeLeftSetThrottle = 0f;
		timeLeftCurrentThrottle = 0f;
	}

	public DeltaVPropellantInfo Copy()
	{
		return new DeltaVPropellantInfo(propellant)
		{
			amountAvailable = amountAvailable,
			amountBurnt = amountBurnt,
			maxBurnTime = maxBurnTime,
			amountPerSecondMaxThrottle = amountPerSecondMaxThrottle,
			amountPerSecondSetThrottle = amountPerSecondSetThrottle,
			amountPerSecondCurrentThrottle = amountPerSecondCurrentThrottle,
			pendingDemand = pendingDemand,
			timeLeftSetThrottle = timeLeftSetThrottle,
			timeLeftCurrentThrottle = timeLeftCurrentThrottle
		};
	}

	public float MassBurnt()
	{
		if (DeltaVGlobals.PropellantsToIgnore.Contains(propellant.id))
		{
			return 0f;
		}
		return propellant.getPartResourceDefinition().density * (float)amountBurnt;
	}
}

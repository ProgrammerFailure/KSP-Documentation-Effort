public class OrbitSnapshot
{
	public double semiMajorAxis;

	public double eccentricity;

	public double inclination;

	public double argOfPeriapsis;

	public double double_0;

	public double meanAnomalyAtEpoch;

	public double epoch;

	public int ReferenceBodyIndex;

	public OrbitSnapshot(Orbit orbitRef)
	{
		semiMajorAxis = orbitRef.semiMajorAxis;
		eccentricity = orbitRef.eccentricity;
		inclination = orbitRef.inclination;
		argOfPeriapsis = orbitRef.argumentOfPeriapsis;
		double_0 = orbitRef.double_0;
		meanAnomalyAtEpoch = orbitRef.meanAnomalyAtEpoch;
		epoch = orbitRef.epoch;
		ReferenceBodyIndex = FlightGlobals.Bodies.IndexOf(orbitRef.referenceBody);
	}

	public OrbitSnapshot(CelestialBody bodyRef)
	{
		eccentricity = 0.0;
		semiMajorAxis = bodyRef.Radius + bodyRef.Radius * 0.25;
		inclination = 0.0001;
		double_0 = 0.0;
		argOfPeriapsis = 0.0;
		meanAnomalyAtEpoch = 0.0;
		epoch = 0.0;
		ReferenceBodyIndex = bodyRef.flightGlobalsIndex;
	}

	public OrbitSnapshot(ConfigNode node)
	{
		foreach (ConfigNode.Value value in node.values)
		{
			switch (value.name)
			{
			case "MNA":
				meanAnomalyAtEpoch = double.Parse(value.value);
				break;
			case "ECC":
				eccentricity = double.Parse(value.value);
				break;
			case "REF":
				ReferenceBodyIndex = int.Parse(value.value);
				break;
			case "SMA":
				semiMajorAxis = double.Parse(value.value);
				break;
			case "LPE":
				argOfPeriapsis = double.Parse(value.value);
				break;
			case "LAN":
				double_0 = double.Parse(value.value);
				break;
			case "EPH":
				epoch = double.Parse(value.value);
				break;
			case "INC":
				inclination = double.Parse(value.value);
				break;
			}
		}
	}

	public virtual void Save(ConfigNode node)
	{
		node.AddValue("SMA", semiMajorAxis);
		node.AddValue("ECC", eccentricity);
		node.AddValue("INC", inclination);
		node.AddValue("LPE", argOfPeriapsis);
		node.AddValue("LAN", double_0);
		node.AddValue("MNA", meanAnomalyAtEpoch);
		node.AddValue("EPH", epoch);
		node.AddValue("REF", ReferenceBodyIndex);
	}

	public Orbit Load()
	{
		return new Orbit(inclination, eccentricity, semiMajorAxis, double_0, argOfPeriapsis, meanAnomalyAtEpoch, epoch, FlightGlobals.Bodies[ReferenceBodyIndex]);
	}
}

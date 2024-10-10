public class EVAConstructionUtil
{
	public static double GetConstructionGee(Vessel vessel)
	{
		if (vessel == null)
		{
			return PhysicsGlobals.GravitationalAcceleration;
		}
		if (vessel.LandedOrSplashed)
		{
			return vessel.EVALadderVessel.gravityForPos.magnitude;
		}
		return vessel.EVALadderVessel.geeForce * PhysicsGlobals.GravitationalAcceleration;
	}
}

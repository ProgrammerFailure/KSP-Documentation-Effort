public interface ICommAntenna
{
	bool CommCombinable { get; }

	double CommCombinableExponent { get; }

	double CommPower { get; }

	AntennaType CommType { get; }

	DoubleCurve CommRangeCurve { get; }

	DoubleCurve CommScienceCurve { get; }

	bool CanComm();

	bool CanCommUnloaded(ProtoPartModuleSnapshot mSnap);

	bool CanScienceTo(bool combined, double bPower, double sqrDistance);

	double CommPowerUnloaded(ProtoPartModuleSnapshot mSnap);
}

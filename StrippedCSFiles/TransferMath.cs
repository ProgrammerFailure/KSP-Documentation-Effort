using System;
using System.Runtime.CompilerServices;

[Serializable]
public class TransferMath
{
	public delegate double BisectFunc(double x);

	public static double safetyEnvelope;

	private static readonly PatchedConics.SolverParameters solverParameters;

	private static double epsilon;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TransferMath()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static TransferMath()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double AlignmentTime(TransferDataSimple transferDataSimple, Orbit sourceOrbit, Orbit targetOrbit, double startUT, double offsetDegrees = 0.0)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double TransferDV(TransferDataSimple transferDataSimple, double startUT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static bool SolveLambert(TransferDataSimple transferDataSimple, double startUT, Orbit startOrbit, Orbit targetOrbit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double FinalizeLambert(TransferDataSimple transferDataSimple, double startUT, Orbit startOrbit, Orbit targetOrbit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool CalculateCorrection(TransferDataSimple transferDataSimple, double startUT, SafeAbortBackgroundWorker bw)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void NoBurnRequired(TransferDataSimple transferDataSimple)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Vector3d CalculateCorrectionBurn(TransferDataSimple transferDataSimple, Orbit currentOrbit, Orbit target, double burnUT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Vector3d DVTimeInterCBXfer(Orbit o, double UT, Orbit target, bool syncPhaseAngle, out double burnUT, out double transferTime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d DeltaVToInterceptAtTime(Orbit o, double initialUT, Orbit target, double DT, out Vector3d secondDV)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void CalcEjectionValues(TransferDataSimple transferDataSimple, out double ejectionDVNormal, out double ejectionDVPrograde, out double ejectionHeading, out double ejectionAngle, out bool ejectionAngleisRetrograde)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool SameSOITransfer(TransferDataSimple transferDataSimple)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double CalcSameSOITransferDV(TransferDataSimple transferDataSimple, Orbit startOrbit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static double CalcSameSOITransferDV(TransferDataSimple transferDataSimple, Orbit startOrbit, double overrideTime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool FindIntercept(TransferDataSimple transferDataSimple, Orbit startOrbit, CelestialBody targetBody, Vector3d dV, double UT, bool sameSOIXfer, double initialClosestApproach, SafeAbortBackgroundWorker bw, out Vector3d adjustedDV)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool SearchDVRange(double startDv, double endDv, double step, Vector3d dV, Orbit startOrbit, CelestialBody targetBody, double UT, ref double closestApproach, out Vector3d adjustedDV, out Vector3d newDV, SafeAbortBackgroundWorker bw)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Vector3d CalcdV(TransferDataSimple transferDataSimple, Orbit startObit, bool findBestUT, double startPeriapsis, double newApoapsis)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Vector3d CalcdVReturn(TransferDataSimple transferDataSimple, Orbit startOrbit, double burnTime, double newPeriapsis)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CalculateStartEndXferTimes(CelestialBody sourceBody, CelestialBody targetBody, double offsetDegrees, out double startTime, out double endTime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CalculateStartEndXferTimes(CelestialBody sourceBody, CelestialBody targetBody, double afterUT, double offsetDegrees, out double startTime, out double endTime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CalculateStartEndXferTimes(TransferDataSimple transferDataSimple, CelestialBody sourceBody, CelestialBody targetBody, double offsetDegrees, out string startTime, out string endTime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double CalcCircularizeDV(TransferDataSimple transferDataSimple, double newApoapsis)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static double EjectionAngleCalc(Vector3d vsoi, double theta, Vector3d prograde)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double SafeOrbitRadius(CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static bool InterceptBody(SafeAbortBackgroundWorker bw, Orbit startOrbit, CelestialBody target, Vector3d dV, double UT, int maxIter, out Orbit intercept, out double closestApproach)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static double TimeToANDN(Orbit currentOrbit, Orbit targetOrbit, double startTime, bool ascending, bool closest, out double otherNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Bisect(BisectFunc f, double minimum, double maximum, double acceptableRange = 0.01, int maxIter = 15)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double Root(BisectFunc f, double a, double b, double tol, int maxIterations = 50, int sign = 0)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static double CalcTransferTime(Orbit origOrb, Orbit destOrb, double transTime, double arrivalPhaseAngle = double.NegativeInfinity)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double HohmannTimeOfFlight(Orbit origin, Orbit destination)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static double OrbitalPeriod(CelestialBody body, double apoapsis, double periapsis)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Vector3d DVTimeHohmann(Orbit o, Orbit target, double UT, bool findBestUT, out double burnUT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Vector3d DVApPhaseHohmann(Orbit o, Orbit target, double UT, out double aPPhaseAng)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool ChangingPeriapsis(Orbit start, Orbit target, double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Vector3d DVToChangePeriapsis(Orbit o, double UT, double newPe)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Vector3d DVToChangeApoapsis(Orbit o, double UT, double newAp)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Orbit TransferOrbit(Orbit o, double UT, Vector3d dV)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void LambertSolver(double mu, Vector3d r1, Vector3d v1, Vector3d r2, Vector3d v2, double tof, int nrev, out Vector3d vi, out Vector3d vf)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void LambertRHG(double GM, double R1, double R2, double TH, double TDELT, out int N, out double VR11, out double VT11, out double VR12, out double VT12, out double VR21, out double VT21, out double VR22, out double VT22)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void LamGXL(double M, double Q, double QSQFM1, double TIN, ref int N, ref double X, ref double XPL, ref int IGTL)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static double D8RT(double x)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void LamGTL(double M, double Q, double QSQFM1, double X, double N, out double T, out double DT, out double D2T, out double D3T, ref int IGTL)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SuperiorLambert(double mu, Vector3d r1, Vector3d v1, Vector3d r2, Vector3d v2, double tof, out Vector3d vi, out Vector3d vf)
	{
		throw null;
	}
}

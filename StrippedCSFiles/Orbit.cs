using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class Orbit
{
	public enum ObjectType
	{
		VESSEL,
		SPACE_DEBRIS,
		CELESTIAL_BODIES,
		UNKNOWN_MISC,
		KERBAL
	}

	public delegate int FindClosestPointsDelegate(Orbit p, Orbit s, ref double CD, ref double CCD, ref double FFp, ref double FFs, ref double SFp, ref double SFs, double epsilon, int maxIterations, ref int iterationCount);

	public struct State
	{
		public Vector3d pos;

		public Vector3d vel;

		public Vector3d acc;

		public Vector3d jrk;

		public double w;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void DumpData(string name, double UT)
		{
			throw null;
		}
	}

	public struct CASolutionState
	{
		public Orbit p;

		public Orbit s;

		public State pstate;

		public State sstate;

		public State rstate;

		public double rdv;

		public double drdv;

		public double ddrdv;

		public double MaxDT;

		public double maxdt;

		public bool targetAhead
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public CASolutionState(Orbit p, Orbit s, double MaxDT)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Update(double t, ref int iter, bool dump = false)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public double Halley_dt()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public double Clamp_dt(double dt)
		{
			throw null;
		}
	}

	public delegate double SolveClosestApproachDelegate(Orbit p, Orbit s, ref double UT, double dT, double threshold, double MinUT, double MaxUT, double epsilon, int maxIterations, ref int iterationCount);

	public delegate bool SolveSOI_Delegate(Orbit p, Orbit s, ref double UT, double dT, double Rsoi, double MinUT, double MaxUT, double epsilon, int maxIterations, ref int iterationCount);

	public delegate bool SolveSOI_BSPDelegate(Orbit p, Orbit s, ref double UT, double dT, double Rsoi, double MinUT, double MaxUT, double epsilon, int maxIterations, ref int iterationCount);

	public enum EncounterSolutionLevel
	{
		NONE,
		ESCAPE,
		ORBIT_INTERSECT,
		SOI_INTERSECT_2,
		SOI_INTERSECT_1
	}

	public enum PatchTransitionType
	{
		INITIAL,
		FINAL,
		ENCOUNTER,
		ESCAPE,
		MANEUVER,
		IMPACT
	}

	public CelestialBody referenceBody;

	public double inclination;

	public double eccentricity;

	public double semiMajorAxis;

	public double LAN;

	public double argumentOfPeriapsis;

	public double epoch;

	public const double Rad2Deg = 180.0 / Math.PI;

	public const double Deg2Rad = Math.PI / 180.0;

	public Planetarium.CelestialFrame OrbitFrame;

	public Vector3d pos;

	public Vector3d vel;

	public double orbitalEnergy;

	public double meanAnomaly;

	public double trueAnomaly;

	public double eccentricAnomaly;

	public double radius;

	public double altitude;

	public double orbitalSpeed;

	public double orbitPercent;

	public double ObT;

	public double ObTAtEpoch;

	public double timeToPe;

	public double timeToAp;

	[Obsolete("Use VesselType or CelestialBodyType instead")]
	public ObjectType objectType;

	public Vector3d h;

	public Vector3d eccVec;

	public Vector3d an;

	public double meanAnomalyAtEpoch;

	public double meanMotion;

	public double period;

	public Vector3 OrbitFrameX;

	public Vector3 OrbitFrameY;

	public Vector3 OrbitFrameZ;

	public Vector3 debugPos;

	public Vector3 debugVel;

	public Vector3 debugH;

	public Vector3 debugAN;

	public Vector3 debugEccVec;

	public double mag;

	private double drawResolution;

	public static FindClosestPointsDelegate FindClosestPoints;

	public static SolveClosestApproachDelegate SolveClosestApproach;

	public static SolveSOI_Delegate SolveSOI;

	public static SolveSOI_BSPDelegate SolveSOI_BSP;

	public int numClosePoints;

	public double FEVp;

	public double FEVs;

	public double SEVp;

	public double SEVs;

	public double UTappr;

	public double UTsoi;

	public double ClAppr;

	public double CrAppr;

	public double ClEctr1;

	public double ClEctr2;

	public double timeToTransition1;

	public double timeToTransition2;

	public double nearestTT;

	public double nextTT;

	public Vector3d secondaryPosAtTransition1;

	public Vector3d secondaryPosAtTransition2;

	public double closestTgtApprUT;

	public double StartUT;

	public double EndUT;

	public bool activePatch;

	public Orbit closestEncounterPatch;

	public CelestialBody closestEncounterBody;

	public EncounterSolutionLevel closestEncounterLevel;

	public PatchTransitionType patchStartTransition;

	public PatchTransitionType patchEndTransition;

	public Orbit nextPatch;

	public Orbit previousPatch;

	public double fromE;

	public double toE;

	public double sampleInterval;

	public double E;

	public double V;

	public double fromV;

	public double toV;

	public bool debug_returnFullEllipseTrajectory;

	public double semiMinorAxis
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double semiLatusRectum
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double PeR
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double ApR
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double PeA
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double ApA
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Orbit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Orbit(double inc, double e, double sma, double lan, double argPe, double mEp, double t, CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Orbit(Orbit orbit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static Orbit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetMeanMotion(double sma)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetOrbit(double inc, double e, double sma, double lan, double argPe, double mEp, double t, CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Init()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double SafeAcos(double c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateFromOrbitAtUT(Orbit orbit, double UT, CelestialBody toBody)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateFromStateVectors(Vector3d pos, Vector3d vel, CelestialBody refBody, double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateFromFixedVectors(Vector3d pos, Vector3d vel, CelestialBody refBody, double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateFromUT(double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetDTforTrueAnomalyAtUT(double tA, double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetDTforTrueAnomaly(double tA, double wrapAfterSeconds)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetUTforTrueAnomaly(double tA, double wrapAfterSeconds)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d getPositionAtUT(double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d getTruePositionAtUT(double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d getRelativePositionAtUT(double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d getOrbitalVelocityAtUT(double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double getObtAtUT(double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double getObTAtMeanAnomaly(double M)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double getObtAtTrueAnomaly(double tA)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetOrbitNormal()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetEccVector()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetANVector()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetVel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetRelativeVel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetRotFrameVel(CelestialBody refBody)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetRotFrameVelAtPos(CelestialBody refBody, Vector3d refPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetFrameVel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetFrameVelAtUT(double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d GetWorldSpaceVel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetTimeToPeriapsis()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetRadiusAtUT(double time)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetRadiusAtPhaseAngle(double phaseAngle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double PhaseAngle(double time, double longitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double PhaseAngle(double time)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetRelativeInclination(Orbit otherOrbit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double TrueAnomalyFromVector(Vector3d vec)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetEccentricAnomaly(double tA)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetMeanAnomaly(double E)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double RadiusAtTrueAnomaly(double tA)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal double TrueAnomalyAtRadiusSimple(double R)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double TrueAnomalyAtRadius(double R)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double trueAnomalyAtRadiusExtreme(double R)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double TrueAnomalyAtUT(double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double TrueAnomalyAtT(double T)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double EccentricAnomalyAtUT(double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double EccentricAnomalyAtObT(double T)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double solveEccentricAnomaly(double M, double ecc, double maxError = 1E-07, int maxIterations = 8)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double solveEccentricAnomalyStd(double M, double ecc, double maxError = 1E-07)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double solveEccentricAnomalyExtremeEcc(double M, double ecc, int iterations = 8)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double solveEccentricAnomalyHyp(double M, double ecc, double maxError = 1E-07)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetTrueAnomaly(double E)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetTrueAnomalyOfZupVector(Vector3d vector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d getPositionAtT(double T)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d getRelativePositionAtT(double T)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d getPositionFromMeanAnomaly(double M)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d getRelativePositionFromMeanAnomaly(double M)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double TimeOfTrueAnomaly(double tA, double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double getUTAtMeanAnomaly(double M, double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double getMeanAnomalyAtUT(double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d getPositionFromEccAnomaly(double E)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d getRelativePositionFromEccAnomaly(double E)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d getPositionFromEccAnomalyWithSemiMinorAxis(double E, double semiMinorAxis)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d getRelativePositionFromEccAnomalyWithSemiMinorAxis(double E, double semiMinorAxis)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d getPositionFromTrueAnomaly(double tA)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d getRelativePositionFromTrueAnomaly(double tA)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d getPositionFromTrueAnomaly(double tA, bool worldToLocal)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double getOrbitalSpeedAt(double time)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double getOrbitalSpeedAtRelativePos(Vector3d relPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double getOrbitalSpeedAtPos(Vector3d pos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double getOrbitalSpeedAtDistance(double d)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d getOrbitalVelocityAtObT(double ObT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d getOrbitalVelocityAtTrueAnomaly(double tA)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d getOrbitalVelocityAtTrueAnomaly(double tA, bool worldToLocal)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetOrbitalStateVectorsAtUT(double UT, out Vector3d pos, out Vector3d vel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetOrbitalStateVectorsAtObT(double ObT, double UT, out Vector3d pos, out Vector3d vel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetOrbitalStateVectorsAtTrueAnomaly(double tA, double UT, out Vector3d pos, out Vector3d vel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetOrbitalStateVectorsAtTrueAnomaly(double tA, double UT, bool worldToLocal, out Vector3d pos, out Vector3d vel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetOrbitalStateVectorsAtUT(double UT, out State state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetOrbitalStateVectorsAtObT(double ObT, out State state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetOrbitalStateVectorsAtTrueAnomaly(double tA, out State state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetOrbitalCurvatureAtTrueAnomaly(double ta)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d Prograde(double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d Radial(double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d Normal(double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d Up(double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3d Horizontal(double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetNextPeriapsisTime(double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetNextApoapsisTime(double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetNextTimeOfRadius(double UT, double radius)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DrawOrbit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DrawOrbit(Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Orbit OrbitFromStateVectors(Vector3d pos, Vector3d vel, CelestialBody body, double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool PeApIntersects(Orbit primary, Orbit secondary, double threshold)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int _FindClosestPoints(Orbit p, Orbit s, ref double CD, ref double CCD, ref double FFp, ref double FFs, ref double SFp, ref double SFs, double epsilon, int maxIterations, ref int iterationCount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void process_intercept(Targeting.Sample cept, out double Vp, out double Vs, out double D)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void FindClosestPoints_old(Orbit p, Orbit s, ref double CD, ref double CCD, ref double FFp, ref double FFs, ref double SFp, ref double SFs, double epsilon, int maxIterations, ref int iterationCount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static double SolveClosestBSP(ref double Fp, ref double Fs, double Ir, double dF, Orbit p, Orbit s, double epsilon, int maxIterations, ref int iterationCount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double RelativeStateAtUT(Orbit p, Orbit s, double UT, out State pstate, out State sstate, out State rstate, bool dump = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double SynodicPeriod(Orbit o1, Orbit o2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double _SolveClosestApproach(Orbit p, Orbit s, ref double UT, double dT, double threshold, double MinUT, double MaxUT, double epsilon, int maxIterations, ref int iterationCount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool _SolveSOI(Orbit p, Orbit s, ref double UT, double dT, double Rsoi, double MinUT, double MaxUT, double epsilon, int maxIterations, ref int iterationCount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool _SolveSOI_BSP(Orbit p, Orbit s, ref double UT, double dT, double Rsoi, double MinUT, double MaxUT, double epsilon, int maxIterations, ref int iterationCount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double NextCloseApproachTime(Orbit p, Orbit s, double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static double Separation(Orbit p, Orbit s, double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double AscendingNodeTrueAnomaly(Orbit p, Orbit s)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double DescendingNodeTrueAnomaly(Orbit p, Orbit s)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double RelativeInclination(Orbit p, Orbit s)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Orbit CreateRandomOrbitAround(CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Orbit CreateRandomOrbitAround(CelestialBody body, double minAltitude, double maxAltitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Orbit CreateRandomOrbitNearby(Orbit baseOrbit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Orbit CreateRandomOrbitFlyBy(CelestialBody tgtBody, double daysToClosestApproach)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Orbit CreateRandomOrbitFlyBy(Orbit targetOrbit, double timeToPeriapsis, double periapsis, double deltaVatPeriapsis)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d Swizzle(Vector3d vec)
	{
		throw null;
	}
}

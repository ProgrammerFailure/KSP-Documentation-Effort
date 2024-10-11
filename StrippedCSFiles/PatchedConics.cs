using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PatchedConics
{
	public class SolverParameters
	{
		public int maxGeometrySolverIterations;

		public int maxTimeSolverIterations;

		public int GeoSolverIterations;

		public int TimeSolverIterations1;

		public int TimeSolverIterations2;

		public bool FollowManeuvers;

		public bool debug_disableEscapeCheck;

		public double outerReaches;

		public double epsilon;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public SolverParameters()
		{
			throw null;
		}
	}

	public delegate double GetClosestApproachDelegate(Orbit p, Orbit s, double startEpoch, double dT, SolverParameters pars);

	public delegate bool EncountersBodyDelegate(Orbit p, Orbit s, Orbit nextPatch, OrbitDriver sec, double startEpoch, SolverParameters pars);

	public delegate bool CheckEncounterDelegate(Orbit p, Orbit nextPatch, double startEpoch, OrbitDriver sec, CelestialBody targetBody, SolverParameters pars, bool logErrors = true);

	public delegate bool CalculatePatchDelegate(Orbit p, Orbit nextPatch, double startEpoch, SolverParameters pars, CelestialBody targetBody);

	public struct PatchCastHit : IScreenCaster
	{
		public Vector3 orbitOrigin;

		public Vector3 hitPoint;

		public Vector3 orbitPoint;

		public Vector3 orbitScreenPoint;

		public double mouseTA;

		public double radiusAtTA;

		public double UTatTA;

		public PatchRendering pr;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Vector3 GetUpdatedOrbitPoint()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Vector3 GetScreenSpacePoint()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Vector3 GetUpdatedOrigin()
		{
			throw null;
		}
	}

	public delegate bool ScreenCastDelegate(Vector3 screenPos, List<PatchRendering> patchRenders, out PatchCastHit hitInfo, float orbitPixelWidth = 10f, double maxUT = -1.0, bool clampToPatches = false);

	public delegate bool ScreenCastWorkerDelegate(Vector3 screenPos, PatchRendering pr, out PatchCastHit hitInfo, float orbitPixelWidth = 10f, bool clampToPatch = false);

	public static GetClosestApproachDelegate GetClosestApproach;

	public static EncountersBodyDelegate EncountersBody;

	public static CheckEncounterDelegate CheckEncounter;

	public static CalculatePatchDelegate CalculatePatch;

	public static ScreenCastDelegate ScreenCast;

	public static ScreenCastWorkerDelegate ScreenCastWorker;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PatchedConics()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static PatchedConics()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double _GetClosestApproach(Orbit p, Orbit s, double startEpoch, double dT, SolverParameters pars)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static bool _EncountersBody(Orbit p, Orbit s, Orbit nextPatch, OrbitDriver sec, double startEpoch, SolverParameters pars)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool _CheckEncounter(Orbit p, Orbit nextPatch, double startEpoch, OrbitDriver sec, CelestialBody targetBody, SolverParameters pars, bool logErrors = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool _CalculatePatch(Orbit p, Orbit nextPatch, double startEpoch, SolverParameters pars, CelestialBody targetBody)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool _ScreenCast(Vector3 screenPos, List<PatchRendering> patchRenders, out PatchCastHit hitInfo, float orbitPixelWidth = 10f, double maxUT = -1.0, bool clampToPatches = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool _ScreenCastWorker(Vector3 screenPos, PatchRendering pr, out PatchCastHit hitInfo, float orbitPixelWidth = 10f, bool clampToPatch = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static double SolveRelativeTA_BSP(PatchRendering pr, Vector3 screenPos, ref double UT, double dT, double MinUT, double MaxUT, double epsilon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static double SolveDynamicTA_BSP(PatchRendering pr, Vector3 screenPos, ref double UT, double dT, double MinUT, double MaxUT, double epsilon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static double SolveLocalTA_BSP(PatchRendering pr, Vector3 screenPos, ref double UT, double TA, double dTA, double MinTA, double MaxTA, double epsilon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double AngleWrap(double a)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool TAIsWithinPatchBounds(double tA, Orbit patch)
	{
		throw null;
	}
}

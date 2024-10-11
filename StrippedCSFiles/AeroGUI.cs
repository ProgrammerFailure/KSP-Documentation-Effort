using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[KSPAddon(KSPAddon.Startup.Flight, false)]
public class AeroGUI : MonoBehaviour
{
	private const double DEG2RAD = Math.PI / 180.0;

	private const double RAD2DEG = 180.0 / Math.PI;

	public static Rect windowPos;

	public static Rect windowPosThermal;

	public bool winterOwlModeOff;

	private const double bodyEmissiveScalarS0Front = 0.782048841;

	private const double bodyEmissiveScalarS0Back = 0.093081228;

	private const double bodyEmissiveScalarS1 = 0.87513007;

	private const double bodyEmissiveScalarS0Top = 0.398806364;

	private const double bodyEmissiveScalarS1Top = 0.797612728;

	private double solarFlux;

	private double backgroundRadTemp;

	private double bodyAlbedoFlux;

	private double bodyEmissiveFlux;

	private double bodySunFlux;

	private double effectiveFaceTemp;

	private double bodyTemperature;

	private double sunDot;

	private double atmosphereTemperatureOffset;

	private double altTempMult;

	private double latitude;

	private double latTempMod;

	private double axialTempMod;

	private double solarAMMult;

	private double finalAtmoMod;

	private double sunFinalMult;

	private double diurnalRange;

	public double terminalV;

	public double alpha;

	public double sideslip;

	public double soundSpeed;

	public double mach;

	public double eas;

	public double thrust;

	public double climbrate;

	public double srfHeight;

	public double lift;

	public double drag;

	public double lidForce;

	public double dragUpForce;

	public double pLift;

	public double pDrag;

	public double liftUp;

	public double grav;

	public double ldRatio;

	public double Q;

	public double pressure;

	public double density;

	public double ambientTemp;

	public double shockTemp;

	public double CdS;

	public double ClS;

	public double ballisticCoeff;

	public double pitch;

	public double heading;

	public double roll;

	public double pRate;

	public double yRate;

	public double rRate;

	public double pRateA;

	public double yRateA;

	public double rRateA;

	public double dTime;

	public double convectiveTotal;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AeroGUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static AeroGUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnGUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GetThermalStats(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GetAeroStats(Vector3d nVel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DrawWindow(int windowID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DrawWindowThermal(int windowID)
	{
		throw null;
	}
}

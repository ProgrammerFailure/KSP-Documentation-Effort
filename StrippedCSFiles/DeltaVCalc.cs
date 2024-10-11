using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class DeltaVCalc
{
	public double dVinVac;

	public double dVatASL;

	public double dVActual;

	public double time;

	public List<DeltaVEngineInfo> activeEngines;

	public double ispVAC;

	public double ispASL;

	public double ispActual;

	public float TWRVac;

	public float TWRASL;

	public float TWRActual;

	public float startMass;

	public float endMass;

	public double thrustVac;

	public double thrustASL;

	public double thrustActual;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DeltaVCalc()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DeltaVCalc(double dVinVac, double dVatASL, double dVActual, double time, List<DeltaVEngineInfo> activeEngines, double ispVAC, double ispASL, double ispActual, float TWRVac, float TWRASL, float TWRActual, float startMass, float endMass, double thrustVac, double thrustASL, double thrustActual)
	{
		throw null;
	}
}

using System.Collections.Generic;

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

	public DeltaVCalc()
	{
		dVinVac = 0.0;
		dVatASL = 0.0;
		dVActual = 0.0;
		time = 0.0;
		activeEngines = new List<DeltaVEngineInfo>();
		ispVAC = 0.0;
		ispASL = 0.0;
		ispActual = 0.0;
		TWRVac = 0f;
		TWRASL = 0f;
		TWRActual = 0f;
		startMass = 0f;
		endMass = 0f;
		thrustVac = 0.0;
		thrustASL = 0.0;
		thrustActual = 0.0;
	}

	public DeltaVCalc(double dVinVac, double dVatASL, double dVActual, double time, List<DeltaVEngineInfo> activeEngines, double ispVAC, double ispASL, double ispActual, float TWRVac, float TWRASL, float TWRActual, float startMass, float endMass, double thrustVac, double thrustASL, double thrustActual)
	{
		this.dVinVac = dVinVac;
		this.dVatASL = dVatASL;
		this.dVActual = dVActual;
		this.time = time;
		this.activeEngines = new List<DeltaVEngineInfo>(activeEngines);
		this.ispVAC = ispVAC;
		this.ispASL = ispASL;
		this.ispActual = ispActual;
		this.TWRVac = TWRVac;
		this.TWRASL = TWRASL;
		this.TWRActual = TWRActual;
		this.startMass = startMass;
		this.endMass = endMass;
		this.thrustVac = thrustVac;
		this.thrustASL = thrustASL;
		this.thrustActual = thrustActual;
	}
}

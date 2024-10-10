using System;
using ns9;
using UnityEngine;

[KSPAddon(KSPAddon.Startup.Flight, false)]
public class AeroGUI : MonoBehaviour
{
	public const double DEG2RAD = Math.PI / 180.0;

	public const double RAD2DEG = 180.0 / Math.PI;

	public static Rect windowPos = new Rect(100f, 100f, 0f, 0f);

	public static Rect windowPosThermal = new Rect(200f, 200f, 0f, 0f);

	public bool winterOwlModeOff = true;

	public const double bodyEmissiveScalarS0Front = 0.782048841;

	public const double bodyEmissiveScalarS0Back = 0.093081228;

	public const double bodyEmissiveScalarS1 = 0.87513007;

	public const double bodyEmissiveScalarS0Top = 0.398806364;

	public const double bodyEmissiveScalarS1Top = 0.797612728;

	public double solarFlux;

	public double backgroundRadTemp;

	public double bodyAlbedoFlux;

	public double bodyEmissiveFlux;

	public double bodySunFlux;

	public double effectiveFaceTemp;

	public double bodyTemperature;

	public double sunDot;

	public double atmosphereTemperatureOffset;

	public double altTempMult;

	public double latitude;

	public double latTempMod;

	public double axialTempMod;

	public double solarAMMult;

	public double finalAtmoMod;

	public double sunFinalMult;

	public double diurnalRange;

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

	public double double_0;

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

	public void Start()
	{
		base.enabled = true;
		double num = 0.0;
		rRateA = 0.0;
		double num2 = num;
		num = 0.0;
		yRateA = num2;
		double num3 = num;
		num = 0.0;
		pRateA = num3;
		double num4 = num;
		num = 0.0;
		rRate = num4;
		double num5 = num;
		num = 0.0;
		yRate = num5;
		double num6 = num;
		num = 0.0;
		pRate = num6;
		double num7 = num;
		num = 0.0;
		roll = num7;
		double num8 = num;
		num = 0.0;
		heading = num8;
		double num9 = num;
		num = 0.0;
		pitch = num9;
		double clS = num;
		num = 0.0;
		ClS = clS;
		double cdS = num;
		num = 0.0;
		CdS = cdS;
		double num10 = num;
		num = 0.0;
		ballisticCoeff = num10;
		double num11 = num;
		num = 0.0;
		shockTemp = num11;
		double num12 = num;
		num = 0.0;
		ambientTemp = num12;
		double num13 = num;
		num = 0.0;
		density = num13;
		double num14 = num;
		num = 0.0;
		pressure = num14;
		double num15 = num;
		num = 0.0;
		double_0 = num15;
		double num16 = num;
		num = 0.0;
		ldRatio = num16;
		double num17 = num;
		num = 0.0;
		grav = num17;
		double num18 = num;
		num = 0.0;
		liftUp = num18;
		double num19 = num;
		num = 0.0;
		pDrag = num19;
		double num20 = num;
		num = 0.0;
		pLift = num20;
		double num21 = num;
		num = 0.0;
		dragUpForce = num21;
		double num22 = num;
		num = 0.0;
		lidForce = num22;
		double num23 = num;
		num = 0.0;
		drag = num23;
		double num24 = num;
		num = 0.0;
		lift = num24;
		double num25 = num;
		num = 0.0;
		eas = num25;
		double num26 = num;
		num = 0.0;
		srfHeight = num26;
		double num27 = num;
		num = 0.0;
		climbrate = num27;
		double num28 = num;
		num = 0.0;
		soundSpeed = num28;
		double num29 = num;
		num = 0.0;
		thrust = num29;
		double num30 = num;
		num = 0.0;
		mach = num30;
		double num31 = num;
		num = 0.0;
		alpha = num31;
		terminalV = num;
		num = 0.0;
		diurnalRange = 0.0;
		double num32 = num;
		num = 0.0;
		sunFinalMult = num32;
		double num33 = num;
		num = 0.0;
		finalAtmoMod = num33;
		double num34 = num;
		num = 0.0;
		solarAMMult = num34;
		double num35 = num;
		num = 0.0;
		axialTempMod = num35;
		double num36 = num;
		num = 0.0;
		latTempMod = num36;
		double num37 = num;
		num = 0.0;
		latitude = num37;
		double num38 = num;
		num = 0.0;
		altTempMult = num38;
		double num39 = num;
		num = 0.0;
		atmosphereTemperatureOffset = num39;
		double num40 = num;
		num = 0.0;
		sunDot = num40;
		double num41 = num;
		num = 0.0;
		bodyTemperature = num41;
		double num42 = num;
		num = 0.0;
		effectiveFaceTemp = num42;
		double num43 = num;
		num = 0.0;
		bodySunFlux = num43;
		double num44 = num;
		num = 0.0;
		bodyEmissiveFlux = num44;
		double num45 = num;
		num = 0.0;
		bodyAlbedoFlux = num45;
		solarFlux = num;
		ConfigNode configNode = null;
		ConfigNode[] configNodes = GameDatabase.Instance.GetConfigNodes("AEROGUI");
		int num46 = 0;
		if (0 < configNodes.Length)
		{
			configNode = configNodes[num46];
		}
		if (configNode != null && configNode.HasValue("WinterOwlMode") && bool.TryParse(configNode.GetValue("WinterOwlMode"), out var result))
		{
			winterOwlModeOff = !result;
		}
	}

	public void OnGUI()
	{
		if (PhysicsGlobals.AeroGUIDisplay)
		{
			windowPos = GUILayout.Window("AeroGUI".GetHashCode(), windowPos, DrawWindow, "AeroGUI");
		}
		if (PhysicsGlobals.ThermoGUIDisplay)
		{
			windowPosThermal = GUILayout.Window("AeroGUIThermal".GetHashCode(), windowPosThermal, DrawWindowThermal, "Thermal Info");
		}
	}

	public void Update()
	{
		if (!HighLogic.LoadedSceneIsFlight || (!PhysicsGlobals.AeroGUIDisplay && !PhysicsGlobals.ThermoGUIDisplay))
		{
			return;
		}
		dTime = TimeWarp.deltaTime;
		double num = heading;
		double num2 = pitch;
		double num3 = roll;
		double num4 = 0.0;
		rRateA = 0.0;
		double num5 = num4;
		num4 = 0.0;
		yRateA = num5;
		double num6 = num4;
		num4 = 0.0;
		pRateA = num6;
		double num7 = num4;
		num4 = 0.0;
		rRate = num7;
		double num8 = num4;
		num4 = 0.0;
		yRate = num8;
		double num9 = num4;
		num4 = 0.0;
		pRate = num9;
		double num10 = num4;
		num4 = 0.0;
		roll = num10;
		double num11 = num4;
		num4 = 0.0;
		heading = num11;
		double num12 = num4;
		num4 = 0.0;
		pitch = num12;
		double clS = num4;
		num4 = 0.0;
		ClS = clS;
		double cdS = num4;
		num4 = 0.0;
		CdS = cdS;
		double num13 = num4;
		num4 = 0.0;
		ballisticCoeff = num13;
		double num14 = num4;
		num4 = 0.0;
		shockTemp = num14;
		double num15 = num4;
		num4 = 0.0;
		ambientTemp = num15;
		double num16 = num4;
		num4 = 0.0;
		density = num16;
		double num17 = num4;
		num4 = 0.0;
		pressure = num17;
		double num18 = num4;
		num4 = 0.0;
		double_0 = num18;
		double num19 = num4;
		num4 = 0.0;
		ldRatio = num19;
		double num20 = num4;
		num4 = 0.0;
		grav = num20;
		double num21 = num4;
		num4 = 0.0;
		liftUp = num21;
		double num22 = num4;
		num4 = 0.0;
		pDrag = num22;
		double num23 = num4;
		num4 = 0.0;
		pLift = num23;
		double num24 = num4;
		num4 = 0.0;
		dragUpForce = num24;
		double num25 = num4;
		num4 = 0.0;
		lidForce = num25;
		double num26 = num4;
		num4 = 0.0;
		drag = num26;
		double num27 = num4;
		num4 = 0.0;
		lift = num27;
		double num28 = num4;
		num4 = 0.0;
		eas = num28;
		double num29 = num4;
		num4 = 0.0;
		srfHeight = num29;
		double num30 = num4;
		num4 = 0.0;
		climbrate = num30;
		double num31 = num4;
		num4 = 0.0;
		soundSpeed = num31;
		double num32 = num4;
		num4 = 0.0;
		thrust = num32;
		double num33 = num4;
		num4 = 0.0;
		mach = num33;
		double num34 = num4;
		num4 = 0.0;
		alpha = num34;
		terminalV = num4;
		num4 = 0.0;
		diurnalRange = 0.0;
		double num35 = num4;
		num4 = 0.0;
		sunFinalMult = num35;
		double num36 = num4;
		num4 = 0.0;
		finalAtmoMod = num36;
		double num37 = num4;
		num4 = 0.0;
		solarAMMult = num37;
		double num38 = num4;
		num4 = 0.0;
		axialTempMod = num38;
		double num39 = num4;
		num4 = 0.0;
		latTempMod = num39;
		double num40 = num4;
		num4 = 0.0;
		latitude = num40;
		double num41 = num4;
		num4 = 0.0;
		altTempMult = num41;
		double num42 = num4;
		num4 = 0.0;
		atmosphereTemperatureOffset = num42;
		double num43 = num4;
		num4 = 0.0;
		sunDot = num43;
		double num44 = num4;
		num4 = 0.0;
		bodyTemperature = num44;
		double num45 = num4;
		num4 = 0.0;
		effectiveFaceTemp = num45;
		double num46 = num4;
		num4 = 0.0;
		bodySunFlux = num46;
		double num47 = num4;
		num4 = 0.0;
		bodyEmissiveFlux = num47;
		double num48 = num4;
		num4 = 0.0;
		bodyAlbedoFlux = num48;
		solarFlux = num4;
		backgroundRadTemp = PhysicsGlobals.SpaceTemperature;
		if ((object)FlightGlobals.ActiveVessel == null)
		{
			return;
		}
		Vessel activeVessel = FlightGlobals.ActiveVessel;
		ambientTemp = activeVessel.atmosphericTemperature;
		shockTemp = activeVessel.externalTemperature;
		if (PhysicsGlobals.AeroGUIDisplay)
		{
			srfHeight = activeVessel.heightFromTerrain;
			grav = activeVessel.totalMass * FlightGlobals.getGeeForceAtPosition(activeVessel.CoM).magnitude;
			Vector3d srf_vel_direction = activeVessel.srf_vel_direction;
			if (activeVessel.srfSpeed >= 0.01)
			{
				alpha = Vector3d.Dot(activeVessel.transform.forward, Vector3d.Exclude(activeVessel.transform.right, srf_vel_direction).normalized);
				alpha = Math.Asin(alpha) * (180.0 / Math.PI);
				if (double.IsNaN(alpha))
				{
					alpha = 0.0;
				}
				sideslip = Vector3d.Dot(activeVessel.transform.up, Vector3d.Exclude(activeVessel.transform.forward, srf_vel_direction).normalized);
				sideslip = Math.Acos(sideslip) * (180.0 / Math.PI);
				if (double.IsNaN(sideslip))
				{
					sideslip = 0.0;
				}
				if (sideslip < 0.0)
				{
					sideslip = 180.0 + sideslip;
				}
			}
			climbrate = Vector3d.Dot(activeVessel.srf_velocity, activeVessel.upAxis);
			Quaternion quaternion = Quaternion.LookRotation(activeVessel.north, activeVessel.upAxis);
			Quaternion quaternion2 = Quaternion.Inverse(Quaternion.Euler(90f, 0f, 0f) * Quaternion.Inverse(activeVessel.GetTransform().rotation) * quaternion);
			Vector3 vector = Vector3.zero;
			if (activeVessel.GetComponent<Rigidbody>() != null)
			{
				vector = Quaternion.Inverse(activeVessel.GetTransform().rotation) * activeVessel.GetComponent<Rigidbody>().angularVelocity;
			}
			heading = quaternion2.eulerAngles.y;
			pitch = ((quaternion2.eulerAngles.x > 180f) ? (360.0 - (double)quaternion2.eulerAngles.x) : ((double)(0f - quaternion2.eulerAngles.x)));
			roll = ((quaternion2.eulerAngles.z > 180f) ? ((double)quaternion2.eulerAngles.z - 360.0) : ((double)quaternion2.eulerAngles.z));
			yRate = (double)(0f - vector.z) * (180.0 / Math.PI);
			pRate = (double)(0f - vector.x) * (180.0 / Math.PI);
			rRate = (double)(0f - vector.y) * (180.0 / Math.PI);
			double num49 = 1.0 / dTime;
			yRateA = (heading - num) * num49;
			pRateA = (pitch - num2) * num49;
			rRateA = (roll - num3) * num49;
			if (FlightGlobals.ActiveVessel.atmDensity > 0.0)
			{
				GetAeroStats(srf_vel_direction);
			}
		}
		if (PhysicsGlobals.ThermoGUIDisplay)
		{
			GetThermalStats(activeVessel);
		}
	}

	public void GetThermalStats(Vessel vessel)
	{
		FlightIntegrator component = vessel.GetComponent<FlightIntegrator>();
		if (component == null)
		{
			return;
		}
		shockTemp = vessel.externalTemperature;
		backgroundRadTemp = component.backgroundRadiationTemp;
		double t = component.CalculateDensityThermalLerp();
		Vector3 lhs = (Planetarium.fetch.Sun.scaledBody.transform.position - ScaledSpace.LocalToScaledSpace(vessel.transform.position)).normalized;
		solarFlux = vessel.solarFlux;
		if (!(Planetarium.fetch.Sun != vessel.mainBody))
		{
			return;
		}
		double num = ((Planetarium.fetch.Sun.scaledBody.transform.position - vessel.mainBody.scaledBody.transform.position) * ScaledSpace.ScaleFactor).sqrMagnitude;
		bodySunFlux = PhysicsGlobals.SolarLuminosity / (Math.PI * 4.0 * num);
		sunDot = Vector3.Dot(lhs, vessel.upAxis);
		Vector3 up = vessel.mainBody.bodyTransform.up;
		float num2 = Vector3.Dot(lhs, up);
		double num3 = Vector3.Dot(up, vessel.upAxis);
		double num4 = Math.Acos(num3);
		if (double.IsNaN(num4))
		{
			num4 = ((num3 < 0.0) ? Math.PI : 0.0);
		}
		double num5 = Math.Acos(num2);
		if (double.IsNaN(num5))
		{
			num5 = (((double)num2 < 0.0) ? Math.PI : 0.0);
		}
		double num6 = (1.0 + Math.Cos(num5 - num4)) * 0.5;
		double num7 = (1.0 + Math.Cos(num5 + num4)) * 0.5;
		if (num3 < 0.0)
		{
			num2 = 0f - num2;
		}
		double num8 = num4;
		double num9 = num5;
		if (num4 > Math.PI / 2.0)
		{
			num8 = Math.PI - num8;
			num9 = Math.PI - num9;
		}
		double num10 = (1.0 + (double)Vector3.Dot(lhs, Quaternion.AngleAxis(-45f * Mathf.Sign((float)vessel.mainBody.rotationPeriod), up) * vessel.upAxis)) * 0.5;
		double num11 = (num10 - num7) / (num6 - num7);
		if (double.IsNaN(num11))
		{
			num11 = ((!(num10 > 0.5)) ? 0.0 : 1.0);
		}
		latitude = Math.PI / 2.0 - num8;
		latitude *= 180.0 / Math.PI;
		if (vessel.mainBody.atmosphere)
		{
			float time = (float)latitude;
			CelestialBody bodyReferencing = CelestialBody.GetBodyReferencing(vessel.mainBody, FlightIntegrator.sunBody);
			diurnalRange = vessel.mainBody.latitudeTemperatureSunMultCurve.Evaluate(time);
			latTempMod = vessel.mainBody.latitudeTemperatureBiasCurve.Evaluate(time);
			float time2 = ((float)bodyReferencing.orbit.trueAnomaly * 57.29578f + 360f) % 360f;
			axialTempMod = vessel.mainBody.axialTemperatureSunBiasCurve.Evaluate(time2) * vessel.mainBody.axialTemperatureSunMultCurve.Evaluate(time);
			atmosphereTemperatureOffset = latTempMod + diurnalRange * num11 + axialTempMod + (double)vessel.mainBody.eccentricityTemperatureBiasCurve.Evaluate((float)((bodyReferencing.orbit.radius - bodyReferencing.orbit.PeR) / (bodyReferencing.orbit.ApR - bodyReferencing.orbit.PeR)));
			altTempMult = vessel.mainBody.atmosphereTemperatureSunMultCurve.Evaluate((float)vessel.altitude);
			if (vessel.atmDensity > 0.0)
			{
				finalAtmoMod = atmosphereTemperatureOffset * altTempMult;
				double num12 = vessel.mainBody.radiusAtmoFactor * sunDot;
				if (num12 < 0.0)
				{
					solarAMMult = Math.Sqrt(2.0 * vessel.mainBody.radiusAtmoFactor + 1.0);
				}
				else
				{
					solarAMMult = Math.Sqrt(num12 * num12 + 2.0 * vessel.mainBody.radiusAtmoFactor + 1.0) - num12;
				}
				sunFinalMult = vessel.mainBody.GetSolarPowerFactor(vessel.atmDensity * solarAMMult);
				double num13 = (vessel.mach - PhysicsGlobals.NewtonianMachTempLerpStartMach) / (PhysicsGlobals.NewtonianMachTempLerpEndMach - PhysicsGlobals.NewtonianMachTempLerpStartMach);
				if (num13 > 0.0)
				{
					num13 = Math.Pow(num13, PhysicsGlobals.NewtonianMachTempLerpExponent);
					num13 = Math.Min(1.0, num13);
					double b = Math.Pow(0.5 * vessel.convectiveMachFlux / (PhysicsGlobals.StefanBoltzmanConstant * PhysicsGlobals.RadiationFactor), 0.25);
					shockTemp = Math.Max(shockTemp, UtilMath.LerpUnclamped(shockTemp, b, num13));
				}
			}
		}
		double num14 = 0.0;
		double num15 = 0.0;
		double num16 = 0.0;
		if (vessel.mainBody.atmosphere)
		{
			double temperature = vessel.mainBody.GetTemperature(0.0);
			bodyTemperature = temperature + atmosphereTemperatureOffset;
			num15 = temperature + (double)(vessel.mainBody.latitudeTemperatureBiasCurve.Evaluate(90f) + vessel.mainBody.axialTemperatureSunMultCurve.Evaluate(0f - (float)vessel.mainBody.orbit.inclination));
			num16 = temperature + (double)(vessel.mainBody.latitudeTemperatureBiasCurve.Evaluate(0f) + vessel.mainBody.latitudeTemperatureSunMultCurve.Evaluate(0f) + vessel.mainBody.axialTemperatureSunMultCurve.Evaluate((float)vessel.mainBody.orbit.inclination));
			num14 = 1.0 - Math.Sqrt(num16) * 0.0016;
			num14 = UtilMath.Clamp01(num14);
		}
		else
		{
			double spaceTemperature = PhysicsGlobals.SpaceTemperature;
			spaceTemperature *= spaceTemperature;
			spaceTemperature *= spaceTemperature;
			double num17 = 1.0 / (PhysicsGlobals.StefanBoltzmanConstant * vessel.mainBody.emissivity);
			double num18 = bodySunFlux * (1.0 - vessel.mainBody.albedo) * num17;
			double num19 = Math.Pow(0.25 * num18 + spaceTemperature, 0.25);
			double num20 = Math.Pow(num18 + spaceTemperature, 0.25) - num19;
			num16 = num19 + Math.Sqrt(num20) * 2.0;
			num15 = num19 - Math.Pow(num20, 1.1) * 1.22;
			double t2 = 2.0 / Math.Sqrt(Math.Sqrt(vessel.mainBody.solarDayLength));
			num16 = UtilMath.Lerp(num16, num19, t2);
			num15 = UtilMath.Lerp(num15, num19, t2);
			double d = Math.Max(0.0, num6 * 2.0 - 1.0);
			d = Math.Sqrt(d);
			num14 = 1.0 - Math.Sqrt(num16) * 0.0016;
			num14 = UtilMath.Clamp01(num14);
			double num21 = (num16 - num15) * d;
			double num22 = num15 + num21;
			double num23 = num15 + num21 * num14;
			bodyTemperature = Math.Max(PhysicsGlobals.SpaceTemperature, num23 + (num22 - num23) * num11 + vessel.mainBody.coreTemperatureOffset);
		}
		effectiveFaceTemp = UtilMath.LerpUnclamped(UtilMath.LerpUnclamped(UtilMath.LerpUnclamped(num15, num16, UtilMath.LerpUnclamped(0.782048841, 0.87513007, num14)), UtilMath.LerpUnclamped(num15, num16, UtilMath.LerpUnclamped(0.093081228, 0.87513007, num14)), num11), UtilMath.LerpUnclamped(num15, num16, UtilMath.LerpUnclamped(0.398806364, 0.797612728, num14)), num6);
		double num24 = UtilMath.Lerp(bodyTemperature, effectiveFaceTemp, 0.2 + vessel.altitude / vessel.mainBody.Radius * 0.5);
		num24 *= num24;
		num24 *= num24;
		double num25 = Math.PI * 4.0 * vessel.mainBody.Radius * vessel.mainBody.Radius / (Math.PI * 4.0 * (vessel.mainBody.Radius + vessel.altitude) * (vessel.mainBody.Radius + vessel.altitude));
		bodyEmissiveFlux = PhysicsGlobals.StefanBoltzmanConstant * vessel.mainBody.emissivity * num24 * num25;
		bodyAlbedoFlux = bodySunFlux * 0.5 * (sunDot + 1.0) * vessel.mainBody.albedo * num25;
		bodyEmissiveFlux = UtilMath.Lerp(0.0, bodyEmissiveFlux, t);
		bodyAlbedoFlux = UtilMath.Lerp(0.0, bodyAlbedoFlux, t);
		int count = vessel.Parts.Count;
		while (count-- > 0)
		{
			convectiveTotal += vessel.Parts[count].thermalConvectionFlux * dTime;
		}
	}

	public void GetAeroStats(Vector3d nVel)
	{
		Vessel activeVessel = FlightGlobals.ActiveVessel;
		Vector3d zero = Vector3d.zero;
		Vector3d zero2 = Vector3d.zero;
		double sqrMagnitude = activeVessel.srf_velocity.sqrMagnitude;
		double_0 = 0.5 * activeVessel.atmDensity * sqrMagnitude;
		eas = Math.Sqrt(sqrMagnitude * activeVessel.atmDensity / 1.225);
		density = activeVessel.atmDensity;
		pressure = activeVessel.staticPressurekPa * 1000.0;
		mach = activeVessel.rootPart.machNumber;
		soundSpeed = activeVessel.speedOfSound;
		double num = 0.0;
		for (int i = 0; i < activeVessel.Parts.Count; i++)
		{
			Part part = activeVessel.Parts[i];
			num += (double)(part.mass + part.GetResourceMass());
			zero2 += -part.dragVectorDir * part.dragScalar;
			if (!part.hasLiftModule)
			{
				Vector3 vector = part.transform.rotation * (part.bodyLiftScalar * part.DragCubes.LiftForce);
				vector = Vector3.ProjectOnPlane(vector, -part.dragVectorDir);
				zero += vector;
			}
			for (int j = 0; j < part.Modules.Count; j++)
			{
				PartModule partModule = part.Modules[j];
				if (partModule is ModuleLiftingSurface)
				{
					ModuleLiftingSurface moduleLiftingSurface = (ModuleLiftingSurface)partModule;
					zero += moduleLiftingSurface.liftForce;
					zero2 += moduleLiftingSurface.dragForce;
				}
				if (partModule is ModuleEngines)
				{
					thrust += ((ModuleEngines)partModule).finalThrust;
				}
			}
		}
		pLift = zero.magnitude;
		pDrag = zero2.magnitude;
		Vector3d lhs = zero + zero2;
		Vector3d normalized = Vector3d.Exclude(nVel, zero).normalized;
		lift = Vector3d.Dot(lhs, normalized);
		liftUp = Vector3d.Dot(lhs, activeVessel.upAxis);
		drag = Vector3d.Dot(lhs, -nVel);
		lidForce = Vector3d.Dot(zero, -nVel);
		dragUpForce = Vector3d.Dot(zero2, activeVessel.upAxis);
		ldRatio = lift / drag;
		terminalV = Math.Sqrt(grav / drag) * activeVessel.speed;
		if (double.IsNaN(terminalV))
		{
			terminalV = 0.0;
		}
		double num2 = 1000.0 / double_0;
		ClS = lift * num2;
		CdS = drag * num2;
		ballisticCoeff = 1000.0 * num / CdS;
	}

	public void DrawWindow(int windowID)
	{
		GUIStyle gUIStyle = new GUIStyle(GUI.skin.button);
		gUIStyle.padding = new RectOffset(5, 5, 3, 0);
		gUIStyle.margin = new RectOffset(1, 1, 1, 1);
		gUIStyle.stretchWidth = false;
		gUIStyle.stretchHeight = false;
		GUIStyle gUIStyle2 = new GUIStyle(GUI.skin.label);
		gUIStyle2.wordWrap = false;
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("X", gUIStyle))
		{
			PhysicsGlobals.AeroGUIDisplay = false;
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label(Localizer.Format("#autoLOC_357338", pitch.ToString("N1"), pRate.ToString("N2"), pRateA.ToString("N2")), gUIStyle2);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label(Localizer.Format("#autoLOC_357342", heading.ToString("N1"), yRate.ToString("N2"), yRateA.ToString("N2")), gUIStyle2);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label(Localizer.Format("#autoLOC_357346", roll.ToString("N1"), rRate.ToString("N2"), rRateA.ToString("N2")), gUIStyle2);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label(Localizer.Format("#autoLOC_357350", mach.ToString("N3")), gUIStyle2);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label(Localizer.Format("#autoLOC_357354", eas.ToString("N2")), gUIStyle2);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label(Localizer.Format("#autoLOC_357358", terminalV.ToString("N1")), gUIStyle2);
		GUILayout.EndHorizontal();
		if (winterOwlModeOff)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(" ------ ", gUIStyle2);
			GUILayout.EndHorizontal();
			string text = ((!(double_0 > 0.0) || double_0 >= 0.1) ? double_0.ToString("N1") : double_0.ToString("E6"));
			GUILayout.BeginHorizontal();
			GUILayout.Label(Localizer.Format("#autoLOC_357369", text), gUIStyle2);
			GUILayout.EndHorizontal();
			text = ((!(pressure > 0.0) || pressure >= 0.1) ? pressure.ToString("N1") : pressure.ToString("E6"));
			GUILayout.BeginHorizontal();
			GUILayout.Label(Localizer.Format("#autoLOC_357374", text), gUIStyle2);
			GUILayout.EndHorizontal();
			text = ((!(density > 0.0) || density >= 1E-05) ? density.ToString("N6") : density.ToString("E6"));
			GUILayout.BeginHorizontal();
			GUILayout.Label(Localizer.Format("#autoLOC_357379", text), gUIStyle2);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label(Localizer.Format("#autoLOC_357389", ambientTemp.ToString("N2")), gUIStyle2);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label(Localizer.Format("#autoLOC_357393", shockTemp.ToString("N2")), gUIStyle2);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label(Localizer.Format("#autoLOC_357397", soundSpeed.ToString("N2")), gUIStyle2);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label(" ------ ", gUIStyle2);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label(Localizer.Format("#autoLOC_357404", alpha.ToString("N3")), gUIStyle2);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label(Localizer.Format("#autoLOC_357408", sideslip.ToString("N3")), gUIStyle2);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label(Localizer.Format("#autoLOC_357412", climbrate.ToString("N3")), gUIStyle2);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label(Localizer.Format("#autoLOC_357416", srfHeight.ToString("N0")), gUIStyle2);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label(Localizer.Format("#autoLOC_357420", lift.ToString("N3")), gUIStyle2);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label(Localizer.Format("#autoLOC_357424", drag.ToString("N3")), gUIStyle2);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label(Localizer.Format("#autoLOC_357428", ldRatio.ToString("N3")), gUIStyle2);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label(Localizer.Format("#autoLOC_357432", CdS.ToString("N3")), gUIStyle2);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label(Localizer.Format("#autoLOC_357436", ClS.ToString("N3")), gUIStyle2);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label(Localizer.Format("#autoLOC_357440", ballisticCoeff.ToString("N3")), gUIStyle2);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label(Localizer.Format("#autoLOC_357444", thrust.ToString("N3")), gUIStyle2);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label(Localizer.Format("#autoLOC_357448", grav.ToString("N3")), gUIStyle2);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label(Localizer.Format("#autoLOC_357452", liftUp.ToString("N3")), gUIStyle2);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label(Localizer.Format("#autoLOC_357456", lidForce.ToString("N3")), gUIStyle2);
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
		GUI.DragWindow();
	}

	public void DrawWindowThermal(int windowID)
	{
		GUIStyle gUIStyle = new GUIStyle(GUI.skin.button);
		gUIStyle.padding = new RectOffset(5, 5, 3, 0);
		gUIStyle.margin = new RectOffset(1, 1, 1, 1);
		gUIStyle.stretchWidth = false;
		gUIStyle.stretchHeight = false;
		GUIStyle gUIStyle2 = new GUIStyle(GUI.skin.label);
		gUIStyle2.wordWrap = false;
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("X", gUIStyle))
		{
			PhysicsGlobals.ThermoGUIDisplay = false;
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label(Localizer.Format("#autoLOC_357487", (solarFlux * 0.001).ToString("N3")), gUIStyle2);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label(Localizer.Format("#autoLOC_357491", solarAMMult.ToString("N5")), gUIStyle2);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label(Localizer.Format("#autoLOC_357495", sunFinalMult.ToString("N5")), gUIStyle2);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label(Localizer.Format("#autoLOC_357499", sunDot.ToString("N4")), gUIStyle2);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label(Localizer.Format("#autoLOC_357503", latitude.ToString("N3")), gUIStyle2);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label(Localizer.Format("#autoLOC_357507", latTempMod.ToString("N2")), gUIStyle2);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label(Localizer.Format("#autoLOC_357511", axialTempMod.ToString("N2")), gUIStyle2);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label(Localizer.Format("#autoLOC_357515", diurnalRange.ToString("N2")), gUIStyle2);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label(Localizer.Format("#autoLOC_357519", altTempMult.ToString("N5")), gUIStyle2);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label(Localizer.Format("#autoLOC_357523", finalAtmoMod.ToString("N2")), gUIStyle2);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label(" ------ ", gUIStyle2);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label(Localizer.Format("#autoLOC_357531", ambientTemp.ToString("N2")), gUIStyle2);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label(Localizer.Format("#autoLOC_357535", shockTemp.ToString("N2")), gUIStyle2);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label(Localizer.Format("#autoLOC_357539", backgroundRadTemp.ToString("N2")), gUIStyle2);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label(Localizer.Format("#autoLOC_357543", bodyTemperature.ToString("N2")), gUIStyle2);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label(Localizer.Format("#autoLOC_357547", effectiveFaceTemp.ToString("N2")), gUIStyle2);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label(Localizer.Format("#autoLOC_357551", (bodyEmissiveFlux * 0.001).ToString("N5")), gUIStyle2);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label(Localizer.Format("#autoLOC_357555", (bodyAlbedoFlux * 0.001).ToString("N5")), gUIStyle2);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label(Localizer.Format("#autoLOC_357559", convectiveTotal.ToString("N3")), gUIStyle2);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button(Localizer.Format("#autoLOC_6001910"), gUIStyle))
		{
			convectiveTotal = 0.0;
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUI.DragWindow();
	}
}

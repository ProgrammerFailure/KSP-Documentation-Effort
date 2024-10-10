using System;
using ns11;
using UnityEngine;
using UnityEngine.UI;

namespace ns2;

public class UIWarpToNextMorning : MonoBehaviour
{
	public Button button;

	public static double timeOfDawn = 0.3;

	public void Start()
	{
		button.onClick.AddListener(WarpToMorning);
	}

	public void Update()
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			if (FlightGlobals.ActiveVessel == null)
			{
				button.interactable = false;
			}
			else if (FlightGlobals.ActiveVessel != null)
			{
				if (FlightGlobals.ActiveVessel.situation != Vessel.Situations.FLYING && FlightGlobals.ActiveVessel.situation != Vessel.Situations.ESCAPING && FlightGlobals.ActiveVessel.situation != Vessel.Situations.SUB_ORBITAL && FlightGlobals.ActiveVessel.situation != Vessel.Situations.ORBITING && (FlightGlobals.ActiveVessel.situation != Vessel.Situations.LANDED || TimeWarp.WarpMode != TimeWarp.Modes.const_1))
				{
					button.interactable = true;
				}
				else
				{
					button.interactable = false;
				}
			}
			else
			{
				button.interactable = true;
			}
		}
		else if (HighLogic.LoadedScene == GameScenes.TRACKSTATION)
		{
			if (SpaceTracking.Instance != null && (SpaceTracking.Instance.SelectedVessel == null || (SpaceTracking.Instance.SelectedVessel != null && (SpaceTracking.Instance.SelectedVessel.situation == Vessel.Situations.FLYING || SpaceTracking.Instance.SelectedVessel.situation == Vessel.Situations.ESCAPING || SpaceTracking.Instance.SelectedVessel.situation == Vessel.Situations.SUB_ORBITAL || SpaceTracking.Instance.SelectedVessel.situation == Vessel.Situations.ORBITING))))
			{
				button.interactable = false;
			}
			else
			{
				button.interactable = true;
			}
		}
		else
		{
			button.interactable = true;
		}
	}

	public void WarpToMorning()
	{
		if (!FlightDriver.Pause)
		{
			if (Sun.Instance == null)
			{
				Debug.LogError("Cannot time warp to next morning, because there is no sun!");
			}
			else if (!TimeWarp.fetch.CancelAutoWarp(0))
			{
				double timeToDaylight = 0.0;
				Setup(ref timeToDaylight);
				TimeWarp.fetch.WarpTo(Planetarium.GetUniversalTime() + timeToDaylight, 8.0, 1.0);
			}
		}
	}

	[Obsolete("Use the definition that already gives you the universal time to sunrise")]
	public void Setup(out double localTime, out double rotPeriod)
	{
		CelestialBody celestialBody;
		double latitude;
		double longitude;
		if (SpaceCenter.Instance != null && SpaceCenter.Instance.cb != null)
		{
			celestialBody = SpaceCenter.Instance.cb;
			latitude = SpaceCenter.Instance.Latitude;
			longitude = SpaceCenter.Instance.Longitude;
		}
		else
		{
			celestialBody = FlightGlobals.GetHomeBody();
			latitude = -0.0917535863160035;
			longitude = 285.3703068811043;
		}
		rotPeriod = celestialBody.rotationPeriod;
		if (celestialBody.orbit != null)
		{
			rotPeriod = celestialBody.orbit.period * rotPeriod / (celestialBody.orbit.period - rotPeriod);
		}
		localTime = Sun.Instance.GetLocalTimeAtPosition(latitude, longitude, celestialBody);
	}

	public void Setup(ref double timeToDaylight)
	{
		CelestialBody body = FlightGlobals.GetHomeBody();
		double latitude = 0.0;
		double longitude = 0.0;
		double rotPeriod = 0.0;
		double localTime = 0.0;
		if (HighLogic.LoadedScene == GameScenes.SPACECENTER)
		{
			if (SpaceCenter.Instance != null && SpaceCenter.Instance.cb != null)
			{
				body = SpaceCenter.Instance.cb;
				latitude = SpaceCenter.Instance.Latitude;
				longitude = SpaceCenter.Instance.Longitude;
			}
			else
			{
				body = FlightGlobals.GetHomeBody();
				latitude = -0.0917535863160035;
				longitude = 285.3703068811043;
			}
			rotPeriod = body.rotationPeriod;
			if (body.orbit != null)
			{
				rotPeriod = body.orbit.period * rotPeriod / (body.orbit.period - rotPeriod);
			}
			localTime = Sun.Instance.GetLocalTimeAtPosition(latitude, longitude, body);
			timeToDaylight = rotPeriod * UtilMath.WrapAround(timeOfDawn - localTime, 0.0, 1.0) + 60.0;
		}
		else if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel != null)
		{
			timeToDaylight = CalculateTimeToDaylightFromVessel(FlightGlobals.ActiveVessel, ref body, ref latitude, ref longitude, ref rotPeriod, ref localTime);
		}
		else if (HighLogic.LoadedScene == GameScenes.TRACKSTATION && SpaceTracking.Instance.SelectedVessel != null)
		{
			timeToDaylight = CalculateTimeToDaylightFromVessel(SpaceTracking.Instance.SelectedVessel, ref body, ref latitude, ref longitude, ref rotPeriod, ref localTime);
		}
	}

	public double CalculateTimeToDaylightFromVessel(Vessel v, ref CelestialBody body, ref double latitude, ref double longitude, ref double rotPeriod, ref double localTime)
	{
		double num = 0.0;
		switch (v.situation)
		{
		default:
			body = v.orbit.referenceBody;
			latitude = v.latitude;
			longitude = v.longitude;
			rotPeriod = body.rotationPeriod;
			if (body.orbit != null && body.orbit.period - rotPeriod > 0.01)
			{
				rotPeriod = body.orbit.period * rotPeriod / (body.orbit.period - rotPeriod);
			}
			localTime = Sun.Instance.GetLocalTimeAtPosition(latitude, longitude, body);
			return rotPeriod * UtilMath.WrapAround(timeOfDawn - localTime, 0.0, 1.0) + 60.0;
		case Vessel.Situations.FLYING:
		case Vessel.Situations.SUB_ORBITAL:
		case Vessel.Situations.ORBITING:
		case Vessel.Situations.ESCAPING:
			return 0.0;
		}
	}
}

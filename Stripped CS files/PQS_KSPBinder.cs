using UnityEngine;

public class PQS_KSPBinder : MonoBehaviour
{
	[SerializeField]
	public PQS_GameBindings pqsBindings;

	public void Awake()
	{
		pqsBindings = GClass4.GameBindings;
		pqsBindings.GetSettingsReady += pqsBindings_GetSettingsReady;
		pqsBindings.GetPlanetForceShaderModel20 += pqsBindings_GetPlanetForceShaderModel20;
		pqsBindings.GetUsePlanetScatter += pqsBindings_GetUsePlanetScatter;
		pqsBindings.GetPlanetScatterFactor += pqsBindings_GetPlanetScatterFactor;
		pqsBindings.GetLoadedSceneIsGame += pqsBindings_GetLoadedSceneIsGame;
		pqsBindings.GetPresetListCompatible += pqsBindings_GetPresetListCompatible;
		pqsBindings.OnPQSCityLoaded += OnPQSCityLoaded;
		pqsBindings.OnPQSCityUnloaded += OnPQSCityUnloaded;
		pqsBindings.OnGetPQSCityLoadRange += GetPQSCityLoadRange;
		pqsBindings.OnGetPOIRange += OnGetPOIRange;
	}

	public bool pqsBindings_GetPresetListCompatible(string versionstring)
	{
		return KSPUtil.CheckVersion(versionstring, PQSCache.lastCompatibleMajor, PQSCache.lastCompatibleMinor, PQSCache.lastCompatibleRev) == VersionCompareResult.COMPATIBLE;
	}

	public bool pqsBindings_GetLoadedSceneIsGame()
	{
		return HighLogic.LoadedSceneIsGame;
	}

	public float pqsBindings_GetPlanetScatterFactor()
	{
		return GameSettings.PLANET_SCATTER_FACTOR;
	}

	public bool pqsBindings_GetUsePlanetScatter()
	{
		return GameSettings.PLANET_SCATTER;
	}

	public bool pqsBindings_GetPlanetForceShaderModel20()
	{
		return GameSettings.UNSUPPORTED_LEGACY_SHADER_TERRAIN;
	}

	public bool pqsBindings_GetSettingsReady()
	{
		return GameSettings.Ready;
	}

	public void OnPQSCityLoaded(PQSSurfaceObject city)
	{
		CelestialBody cBForPQS = GetCBForPQS(city.sphere);
		if (cBForPQS != null)
		{
			GameEvents.OnPQSCityLoaded.Fire(cBForPQS, city.gameObject.name);
		}
	}

	public void OnPQSCityUnloaded(PQSSurfaceObject city)
	{
		CelestialBody cBForPQS = GetCBForPQS(city.sphere);
		if (cBForPQS != null)
		{
			GameEvents.OnPQSCityUnloaded.Fire(cBForPQS, city.gameObject.name);
		}
	}

	public double GetPQSCityLoadRange(PQSSurfaceObject city)
	{
		CelestialBody cBForPQS = GetCBForPQS(city.sphere);
		VesselRanges vesselRanges = ((PhysicsGlobals.Instance != null) ? new VesselRanges(PhysicsGlobals.Instance.VesselRangesDefault) : new VesselRanges());
		return (cBForPQS == null || !cBForPQS.atmosphere) ? vesselRanges.subOrbital.load : vesselRanges.flying.load;
	}

	public double OnGetPOIRange(PQSSurfaceObject surfaceObject)
	{
		CelestialBody cBForPQS = GetCBForPQS(surfaceObject.sphere);
		VesselRanges vesselRanges = ((PhysicsGlobals.Instance != null) ? new VesselRanges(PhysicsGlobals.Instance.VesselRangesDefault) : new VesselRanges());
		return (cBForPQS == null || !cBForPQS.atmosphere) ? vesselRanges.subOrbital.load : vesselRanges.flying.load;
	}

	public CelestialBody GetCBForPQS(GClass4 pqs)
	{
		return pqs.gameObject.GetComponentUpwards<CelestialBody>();
	}

	public void OnDestroy()
	{
		if (pqsBindings != null)
		{
			pqsBindings.GetSettingsReady -= pqsBindings_GetSettingsReady;
			pqsBindings.GetPlanetForceShaderModel20 -= pqsBindings_GetPlanetForceShaderModel20;
			pqsBindings.GetUsePlanetScatter -= pqsBindings_GetUsePlanetScatter;
			pqsBindings.GetPlanetScatterFactor -= pqsBindings_GetPlanetScatterFactor;
			pqsBindings.GetLoadedSceneIsGame -= pqsBindings_GetLoadedSceneIsGame;
			pqsBindings.GetPresetListCompatible -= pqsBindings_GetPresetListCompatible;
			pqsBindings.OnPQSCityLoaded -= OnPQSCityLoaded;
			pqsBindings.OnPQSCityUnloaded -= OnPQSCityUnloaded;
			pqsBindings.OnGetPQSCityLoadRange -= GetPQSCityLoadRange;
		}
	}
}

using System;

public class PQS_GameBindings
{
	public bool SettingsReady => this.GetSettingsReady();

	public bool PLANET_FORCE_SHADER_MODEL_2_0 => this.GetPlanetForceShaderModel20();

	public bool PLANET_SCATTER => this.GetUsePlanetScatter();

	public float PLANET_SCATTER_FACTOR => this.GetPlanetScatterFactor();

	public bool LoadedSceneIsGame => this.GetLoadedSceneIsGame();

	public event Func<bool> GetSettingsReady = () => true;

	public event Func<bool> GetPlanetForceShaderModel20 = () => false;

	public event Func<bool> GetUsePlanetScatter = () => true;

	public event Func<float> GetPlanetScatterFactor = () => 1f;

	public event Func<bool> GetLoadedSceneIsGame = () => false;

	public event Func<string, bool> GetPresetListCompatible = (string v) => KSPUtil.CheckVersion(v, PQSCache.lastCompatibleMajor, PQSCache.lastCompatibleMinor, PQSCache.lastCompatibleRev) == VersionCompareResult.COMPATIBLE;

	public event Callback<PQSSurfaceObject> OnPQSCityLoaded = delegate
	{
	};

	public event Callback<PQSSurfaceObject> OnPQSCityUnloaded = delegate
	{
	};

	public event Func<PQSSurfaceObject, double> OnGetPQSCityLoadRange = (PQSSurfaceObject c) => double.MaxValue;

	public event Func<PQSSurfaceObject, double> OnGetPOIRange = (PQSSurfaceObject c) => double.MaxValue;

	public bool TerrainPresetListIsCompatible(string versionString)
	{
		return this.GetPresetListCompatible(versionString);
	}

	public void DispatchOnPQSCityLoaded(PQSSurfaceObject pCity)
	{
		this.OnPQSCityLoaded(pCity);
	}

	public void DispatchOnPQSCityUnloaded(PQSSurfaceObject pCity)
	{
		this.OnPQSCityUnloaded(pCity);
	}

	public double GetPQSCityLoadRange(PQSSurfaceObject city)
	{
		return this.OnGetPQSCityLoadRange(city);
	}

	public double GetPOIRange(PQSSurfaceObject surfaceObject)
	{
		return this.OnGetPOIRange(surfaceObject);
	}
}

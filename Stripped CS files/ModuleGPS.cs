using System;
using ns9;
using UnityEngine;

public class ModuleGPS : PartModule, IAnimatedModule
{
	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001476")]
	public string body = "???";

	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_438890")]
	public string bioName = "???";

	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001477")]
	public string lat = "???";

	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001478")]
	public string lon = "???";

	public bool isConnectedToAsteroid;

	public bool isConnectedToComet;

	public float secondsSinceLastDisplayUpdate;

	public float secondsUpdateRate = 0.1f;

	public static string cacheAutoLOC_6005065;

	public static string cacheAutoLOC_258910;

	public static string cacheAutoLOC_258911;

	public static string cacheAutoLOC_258912;

	public static string cacheAutoLOC_258913;

	public virtual bool CheckForAsteroidConnect()
	{
		int count = base.vessel.Parts.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				if (base.vessel.Parts[num].HasModuleImplementing<ModuleAsteroid>())
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		body = cacheAutoLOC_258910;
		bioName = cacheAutoLOC_258911;
		lat = cacheAutoLOC_258912;
		lon = cacheAutoLOC_258913;
		return true;
	}

	public virtual bool CheckForCometConnect()
	{
		int count = base.vessel.Parts.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				if (base.vessel.Parts[num].HasModuleImplementing<ModuleComet>())
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		body = cacheAutoLOC_6005065;
		bioName = cacheAutoLOC_258911;
		lat = cacheAutoLOC_258912;
		lon = cacheAutoLOC_258913;
		return true;
	}

	public virtual void Update()
	{
		secondsSinceLastDisplayUpdate += Time.deltaTime;
		if (secondsSinceLastDisplayUpdate < secondsUpdateRate)
		{
			return;
		}
		secondsSinceLastDisplayUpdate = 0f;
		if (!HighLogic.LoadedSceneIsFlight)
		{
			return;
		}
		if (base.vessel != null && base.vessel.parts.Count != 0)
		{
			isConnectedToAsteroid = CheckForAsteroidConnect();
		}
		if (isConnectedToAsteroid)
		{
			return;
		}
		if (base.vessel != null && base.vessel.parts.Count != 0)
		{
			isConnectedToComet = CheckForCometConnect();
		}
		if (isConnectedToComet || !(UIPartActionController.Instance != null) || !UIPartActionController.Instance.ItemListContains(base.part, includeSymmetryCounterparts: false))
		{
			return;
		}
		try
		{
			CelestialBody currentMainBody = FlightGlobals.currentMainBody;
			double num = ResourceUtilities.clampLat(base.vessel.latitude);
			double num2 = ResourceUtilities.clampLon(base.vessel.longitude);
			double num3 = ResourceUtilities.Deg2Rad(num);
			double num4 = ResourceUtilities.Deg2Rad(num2);
			CBAttributeMapSO.MapAttribute biome = ResourceUtilities.GetBiome(num3, num4, FlightGlobals.currentMainBody);
			body = Localizer.Format("#autoLOC_7001301", currentMainBody.displayName);
			lat = Localizer.Format("#autoLOC_6001052", Math.Abs(num).ToString("0.000"), Convert.ToInt32(num >= 0.0), num3.ToString("0.000"));
			lon = Localizer.Format("#autoLOC_6001053", Math.Abs(num2).ToString("0.000"), Convert.ToInt32(num2 >= 0.0), num4.ToString("0.000"));
			if (biome != null)
			{
				bioName = Localizer.Format("#autoLOC_7001301", biome.displayname);
			}
			else
			{
				bioName = base.vessel.SituationString;
			}
		}
		catch (Exception)
		{
			MonoBehaviour.print("[RESOURCES] Error updating ModuleGPS");
		}
	}

	public virtual void EnableModule()
	{
		isEnabled = true;
	}

	public virtual void DisableModule()
	{
		isEnabled = false;
	}

	public virtual bool ModuleIsActive()
	{
		return isEnabled;
	}

	public virtual bool IsSituationValid()
	{
		if (!isConnectedToAsteroid && !isConnectedToComet)
		{
			return true;
		}
		return false;
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLOC_8004214");
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_6005065 = Localizer.Format("#autoLOC_6005065");
		cacheAutoLOC_258910 = Localizer.Format("#autoLOC_258910");
		cacheAutoLOC_258911 = Localizer.Format("#autoLOC_258911");
		cacheAutoLOC_258912 = Localizer.Format("#autoLOC_258912");
		cacheAutoLOC_258913 = Localizer.Format("#autoLOC_258913");
	}
}

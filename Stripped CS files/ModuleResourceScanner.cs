using System;
using ns9;

public class ModuleResourceScanner : PartModule, IAnimatedModule
{
	[KSPField]
	public float MaxAbundanceAltitude = 500000000f;

	[KSPField]
	public bool RequiresUnlock = true;

	[KSPField]
	public string ResourceName = "";

	[KSPField]
	public int ScannerType;

	[KSPField(isPersistant = false, guiActive = true, guiName = "#autoLOC_6001889")]
	public string abundanceDisplay;

	public double abundanceValue;

	public string ResourceDisplayName = "";

	public BaseField aDispFld;

	public int partCacheCount = -1;

	public bool wasValid;

	public static string cacheAutoLOC_260116;

	public static string cacheAutoLOC_260121;

	public static string cacheAutoLOC_260125;

	public static string cacheAutoLOC_260145;

	public double abundance => abundanceValue;

	public void EnableModule()
	{
		isEnabled = true;
	}

	public void DisableModule()
	{
		isEnabled = false;
	}

	public bool ModuleIsActive()
	{
		return isEnabled;
	}

	public string GetDisplayFormat()
	{
		return ResourceDisplayName + "[" + GetScanningMode(abbrevation: true) + "]";
	}

	public string GetScanningMode(bool abbrevation)
	{
		string result = "";
		switch (ScannerType)
		{
		case 0:
			result = Localizer.Format("#autoLOC_6001082", Convert.ToInt32(abbrevation));
			break;
		case 1:
			result = Localizer.Format("#autoLOC_260065");
			break;
		case 2:
			result = Localizer.Format("#autoLOC_6001083", Convert.ToInt32(abbrevation));
			break;
		case 3:
			result = Localizer.Format("#autoLOC_6001084", Convert.ToInt32(abbrevation));
			break;
		}
		return result;
	}

	public override void OnAwake()
	{
		aDispFld = base.Fields["abundanceDisplay"];
	}

	public override void OnStart(StartState state)
	{
		aDispFld.guiName = GetDisplayFormat();
		PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(ResourceName);
		if (definition != null)
		{
			ResourceDisplayName = definition.displayName;
		}
		else
		{
			ResourceDisplayName = ResourceName;
		}
	}

	public void Update()
	{
		if (HighLogic.LoadedSceneIsFlight && base.vessel != null)
		{
			if (UIPartActionController.Instance != null && UIPartActionController.Instance.ItemListContains(base.part, includeSymmetryCounterparts: false))
			{
				CheckAbundanceDisplay();
			}
			aDispFld.guiActive = IsSituationValid();
			if (base.vessel != null && FlightGlobals.currentMainBody != null)
			{
				AbundanceRequest abundanceRequest = default(AbundanceRequest);
				abundanceRequest.Altitude = base.vessel.altitude;
				abundanceRequest.BodyId = FlightGlobals.currentMainBody.flightGlobalsIndex;
				abundanceRequest.CheckForLock = RequiresUnlock;
				abundanceRequest.Latitude = base.vessel.latitude;
				abundanceRequest.Longitude = base.vessel.longitude;
				abundanceRequest.ResourceType = (HarvestTypes)ScannerType;
				abundanceRequest.ResourceName = ResourceName;
				AbundanceRequest request = abundanceRequest;
				abundanceValue = ResourceMap.Instance.GetAbundance(request);
			}
			else
			{
				abundanceValue = 0.0;
			}
		}
	}

	public void CheckAbundanceDisplay()
	{
		if (ResourceUtilities.GetAltitude(base.vessel) > (double)MaxAbundanceAltitude && !base.vessel.Landed && ScannerType == 0)
		{
			abundanceDisplay = cacheAutoLOC_260116;
		}
		else if (!base.vessel.Splashed && ScannerType == 1)
		{
			abundanceDisplay = cacheAutoLOC_260121;
		}
		else if (!FlightGlobals.currentMainBody.atmosphere && ScannerType == 2)
		{
			abundanceDisplay = cacheAutoLOC_260125;
		}
		else
		{
			DisplayAbundance();
		}
	}

	public void DisplayAbundance()
	{
		double lat = ResourceUtilities.Deg2Rad(ResourceUtilities.clampLat(base.vessel.latitude));
		double lon = ResourceUtilities.Deg2Rad(ResourceUtilities.clampLon(base.vessel.longitude));
		CBAttributeMapSO.MapAttribute biome = ResourceUtilities.GetBiome(lat, lon, base.vessel.mainBody);
		string defaultSituation = ResourceMap.GetDefaultSituation((HarvestTypes)ScannerType);
		if (biome != null)
		{
			defaultSituation = biome.name;
		}
		int num;
		object obj;
		if (ScannerType == 0)
		{
			num = (ResourceMap.Instance.IsBiomeUnlocked(base.vessel.mainBody.flightGlobalsIndex, defaultSituation) ? 1 : 0);
			if (num == 0)
			{
				obj = cacheAutoLOC_260145;
				goto IL_0086;
			}
		}
		else
		{
			num = 1;
		}
		obj = "0%";
		goto IL_0086;
		IL_0086:
		string text = (string)obj;
		if (abundanceValue > 0.01)
		{
			text = (abundanceValue * 100.0).ToString("0.00") + "%";
		}
		else if (abundanceValue > 1E-09)
		{
			text = (abundanceValue * 100.0).ToString("0.0000") + "%";
		}
		if (num != 0)
		{
			abundanceDisplay = text;
			return;
		}
		abundanceDisplay = Localizer.Format("#autoLOC_6002267", text);
	}

	public void ToggleEvent(string name, bool status)
	{
		base.Events[name].active = status;
		base.Events[name].guiActive = status;
	}

	public bool IsSituationValid()
	{
		int count = base.vessel.parts.Count;
		if (count == partCacheCount)
		{
			return wasValid;
		}
		partCacheCount = count;
		int index = count;
		while (index-- > 0)
		{
			Part part = base.vessel.parts[index];
			int count2 = part.Modules.Count;
			while (count2-- > 0)
			{
				if (!(part.Modules[count2] is ModuleAsteroid))
				{
					if (part.Modules[count2] is ModuleComet)
					{
						wasValid = false;
						return false;
					}
					continue;
				}
				wasValid = false;
				return false;
			}
		}
		wasValid = true;
		return true;
	}

	public override string GetInfo()
	{
		PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(ResourceName);
		if (definition != null)
		{
			ResourceDisplayName = definition.displayName;
		}
		else
		{
			ResourceDisplayName = ResourceName;
		}
		return string.Concat(string.Concat("" + Localizer.Format("#autoLOC_260196", GetScanningMode(abbrevation: false)), Localizer.Format("#autoLOC_260197", Localizer.Format(ResourceDisplayName))), Localizer.Format("#autoLOC_260198", MaxAbundanceAltitude));
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLoc_6003056");
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_260116 = Localizer.Format("#autoLOC_260116");
		cacheAutoLOC_260121 = Localizer.Format("#autoLOC_260121");
		cacheAutoLOC_260125 = Localizer.Format("#autoLOC_260125");
		cacheAutoLOC_260145 = Localizer.Format("#autoLOC_260145");
	}
}

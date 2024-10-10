using ns17;
using ns9;
using UnityEngine;

namespace KerbNet;

public class KerbNetModeResource : KerbNetMode
{
	public MapDisplayTypes displayMode;

	public PartResourceDefinition partResource;

	public Color seedColor;

	public Color modeButtonColor;

	public Color modeTextColor;

	public Color medianColor;

	public Color startColor;

	public Color endColor;

	public float lowerBounds;

	public float upperBounds;

	public float boundsRange;

	public double vesselAltitude;

	public CelestialBody body;

	public PlanetaryResource planetResource;

	public bool scannedBody;

	public static string cacheAutoLOC_438998;

	public static string cacheAutoLOC_439000;

	public static string cacheAutoLOC_439019;

	public static string cacheAutoLOC_258912;

	public KerbNetModeResource()
	{
	}

	public KerbNetModeResource(string resource)
	{
		name = resource;
		partResource = ((PartResourceLibrary.Instance != null) ? PartResourceLibrary.Instance.GetDefinition(name) : null);
		displayName = partResource.displayName;
		seedColor = ((partResource != null) ? partResource.color : Color.white);
		KerbNetMode.hsv.FromColor(seedColor);
		KerbNetMode.hsv.s = 0.375f;
		KerbNetMode.hsv.v = 1f;
		modeButtonColor = KerbNetMode.hsv.ToColor();
		KerbNetMode.hsv.s = 0.375f;
		KerbNetMode.hsv.v = 0.5f;
		modeTextColor = KerbNetMode.hsv.ToColor();
	}

	public override void OnInit()
	{
		buttonSprite = Resources.Load<Sprite>("Scanners/resource");
		doCoordinatePass = true;
		doTerrainContourPass = true;
		doAnomaliesPass = false;
		localCoordinateInfoLabel = displayName;
		customButtonCaption = cacheAutoLOC_438998;
		customButtonCallback = OnColorClick;
		customButtonTooltip = cacheAutoLOC_439000;
	}

	public override void OnActivated()
	{
		CheckScannedBody();
		GameEvents.OnOrbitalSurveyCompleted.Add(OrbitalSurveyCompleted);
		GameEvents.onVesselSOIChanged.Add(VesselSOIChange);
	}

	public override void OnDeactivated()
	{
		GameEvents.OnOrbitalSurveyCompleted.Remove(OrbitalSurveyCompleted);
		GameEvents.onVesselSOIChanged.Remove(VesselSOIChange);
	}

	public override string GetErrorState()
	{
		if (!scannedBody)
		{
			return cacheAutoLOC_439019;
		}
		return null;
	}

	public void OrbitalSurveyCompleted(Vessel v, CelestialBody cb)
	{
		if (KerbNetDialog.isDisplaying && !(v != KerbNetDialog.Instance.DisplayVessel))
		{
			CheckScannedBody();
		}
	}

	public void VesselSOIChange(GameEvents.HostedFromToAction<Vessel, CelestialBody> fromTo)
	{
		if (KerbNetDialog.isDisplaying && !(fromTo.host != KerbNetDialog.Instance.DisplayVessel))
		{
			CheckScannedBody();
		}
	}

	public void CheckScannedBody()
	{
		if (KerbNetDialog.isDisplaying && !(KerbNetDialog.Instance.DisplayVessel == null) && !(KerbNetDialog.Instance.DisplayVessel.mainBody == null))
		{
			CelestialBody mainBody = KerbNetDialog.Instance.DisplayVessel.mainBody;
			scannedBody = ResourceMap.Instance.IsPlanetScanned(mainBody.flightGlobalsIndex);
			planetResource = ((ResourceMap.Instance != null) ? ResourceMap.Instance.GetResourceByName(name, mainBody, HarvestTypes.Planetary) : null);
			KerbNetDialog.Instance.RefreshErrorState();
		}
	}

	public override bool AutoGenerateMode()
	{
		return false;
	}

	public override Color GetModeColorTint()
	{
		return modeButtonColor;
	}

	public override string GetModeCaption()
	{
		string text = ((partResource == null) ? ((displayName.Length <= 2) ? displayName : displayName.Remove(2)) : partResource.GetShortName());
		return "<color=" + XKCDColors.ColorTranslator.ToHex(modeTextColor) + ">" + text + "</color>";
	}

	public void OnColorClick()
	{
		int num = (int)displayMode;
		num++;
		if (num == 4)
		{
			num = 0;
		}
		displayMode = (MapDisplayTypes)num;
		KerbNetDialog.Instance.FullRefresh(refreshMap: true);
	}

	public override void OnPrecache(Vessel vessel)
	{
		body = ((!(vessel != null) || !(vessel.mainBody != null)) ? FlightGlobals.GetHomeBody() : vessel.mainBody);
		planetResource = ((ResourceMap.Instance != null) ? ResourceMap.Instance.GetResourceByName(name, body, HarvestTypes.Planetary) : null);
		vesselAltitude = ((vessel != null) ? vessel.altitude : 0.0);
		medianColor = seedColor;
		if (displayMode == MapDisplayTypes.Inverse)
		{
			if (medianColor == Color.white)
			{
				medianColor = Color.magenta;
			}
			else
			{
				medianColor = new Color(1f - medianColor.r, 1f - medianColor.g, 1f - medianColor.b);
			}
		}
		startColor = new Color(medianColor.r / 3f, medianColor.g / 3f, medianColor.b / 3f, 1f);
		endColor = new Color(medianColor.r * 1.5f, medianColor.g * 1.5f, medianColor.b * 1.5f, 1f);
		lowerBounds = OverlayGenerator.Instance.GetMinAbundance(partResource, vessel.mainBody.flightGlobalsIndex, HarvestTypes.Planetary);
		upperBounds = OverlayGenerator.Instance.GetMaxAbundance(partResource, vessel.mainBody.flightGlobalsIndex, HarvestTypes.Planetary);
		boundsRange = upperBounds - lowerBounds;
	}

	public float GetAbundance(double latitude = 0.0, double longitude = 0.0)
	{
		if (ResourceMap.Instance == null)
		{
			return 0f;
		}
		AbundanceRequest abundanceRequest = default(AbundanceRequest);
		abundanceRequest.Altitude = vesselAltitude;
		abundanceRequest.BodyId = body.flightGlobalsIndex;
		abundanceRequest.CheckForLock = true;
		abundanceRequest.Latitude = latitude;
		abundanceRequest.Longitude = longitude;
		abundanceRequest.ResourceType = HarvestTypes.Planetary;
		abundanceRequest.ResourceName = name;
		AbundanceRequest request = abundanceRequest;
		return ResourceMap.Instance.GetAbundance(request);
	}

	public override Color GetCoordinateColor(Vessel vessel, double currentLatitude, double currentLongitude)
	{
		float abundance = GetAbundance(currentLatitude, currentLongitude);
		float num = abundance - lowerBounds;
		if (boundsRange > 0f)
		{
			num /= boundsRange;
		}
		if ((double)abundance > 1E-09)
		{
			return displayMode switch
			{
				MapDisplayTypes.HeatMapBlue => new Color(2f * num, 0f, 2f * (1f - num), 1f), 
				MapDisplayTypes.HeatMapGreen => new Color(2f * num, 2f * (1f - num), 0f, 1f), 
				_ => Color.Lerp(startColor, endColor, num), 
			};
		}
		return XKCDColors.DarkGrey;
	}

	public override string LocalCoordinateInfo(Vessel vessel, double centerLatitude, double centerLongitude, double waypointLatitude, double waypointLongitude, bool waypointInSpace)
	{
		if (waypointInSpace)
		{
			return cacheAutoLOC_258912;
		}
		float num = GetAbundance(waypointLatitude, waypointLongitude) * 100f;
		float num2 = ((planetResource != null) ? (planetResource.fraction * 100f) : 0f);
		return Localizer.Format("#autoLOC_439164", num.ToString("N2"), num2.ToString("N2"));
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_438998 = Localizer.Format("#autoLOC_438998");
		cacheAutoLOC_439000 = Localizer.Format("#autoLOC_439000");
		cacheAutoLOC_439019 = Localizer.Format("#autoLOC_439019");
		cacheAutoLOC_258912 = Localizer.Format("#autoLOC_258912").ToUpper();
	}
}

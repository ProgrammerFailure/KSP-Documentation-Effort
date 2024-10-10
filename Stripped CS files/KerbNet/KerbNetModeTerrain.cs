using FinePrint.Utilities;
using ns9;
using UnityEngine;

namespace KerbNet;

public class KerbNetModeTerrain : KerbNetMode
{
	public static string cacheAutoLOC_438839;

	public static string cacheAutoLOC_438841;

	public static string cacheAutoLOC_7001411;

	public static string cacheAutoLOC_258912;

	public override void OnInit()
	{
		name = "Terrain";
		displayName = cacheAutoLOC_438839;
		buttonSprite = Resources.Load<Sprite>("Scanners/terrain");
		localCoordinateInfoLabel = cacheAutoLOC_438841;
		doTerrainContourPass = true;
		doAnomaliesPass = true;
		terrainContourThreshold = 0.8f;
	}

	public override void GetTerrainContourColors(Vessel vessel, out Color lowColor, out Color highColor)
	{
		if (!(vessel == null) && !(vessel.mainBody == null) && !(vessel.mainBody.orbitDriver == null) && !(vessel.mainBody.orbitDriver.Renderer == null))
		{
			KerbNetMode.hsv.FromColor(vessel.mainBody.orbitDriver.Renderer.orbitColor);
			KerbNetMode.hsv.s = 0.25f;
			KerbNetMode.hsv.v = 0.1f;
			lowColor = KerbNetMode.hsv.ToColor();
			KerbNetMode.hsv.v = 1f;
			highColor = KerbNetMode.hsv.ToColor();
		}
		else
		{
			lowColor = Color.black;
			highColor = Color.white;
		}
	}

	public override string LocalCoordinateInfo(Vessel vessel, double centerLatitude, double centerLongitude, double waypointLatitude, double waypointLongitude, bool waypointInSpace)
	{
		if (waypointInSpace)
		{
			return cacheAutoLOC_258912;
		}
		return KSPUtil.PrintSI(CelestialUtilities.TerrainAltitude(vessel.mainBody, waypointLatitude, waypointLongitude, underwater: true), cacheAutoLOC_7001411);
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_438839 = Localizer.Format("#autoLOC_438839");
		cacheAutoLOC_438841 = Localizer.Format("#autoLOC_438841");
		cacheAutoLOC_7001411 = Localizer.Format("#autoLOC_7001411");
		cacheAutoLOC_258912 = Localizer.Format("#autoLOC_258912").ToUpper();
	}
}

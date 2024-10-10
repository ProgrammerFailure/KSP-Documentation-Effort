using ns36;
using ns9;
using UnityEngine;

public class FlightOverlays : MonoBehaviour
{
	public ScreenMessage statusMessage;

	public static string cacheAutoLOC_135685;

	public static string cacheAutoLOC_135690;

	public static string cacheAutoLOC_135693;

	public static string cacheAutoLOC_135696;

	public static string cacheAutoLOC_135699;

	public static string cacheAutoLOC_6003108;

	public static string cacheAutoLOC_6003106;

	public static string cacheAutoLOC_6001071;

	public static string cacheAutoLOC_6001072;

	public static string cacheAutoLOC_6003107;

	public void Start()
	{
		statusMessage = new ScreenMessage("", 4f, ScreenMessageStyle.LOWER_CENTER);
		PhysicsGlobals.AeroForceDisplay = false;
		PhysicsGlobals.ThermalColorsDebug = false;
	}

	public void Update()
	{
		if (!FlightGlobals.ready || !HighLogic.LoadedSceneIsFlight || CameraManager.Instance.currentCameraMode != 0)
		{
			return;
		}
		if (GameSettings.TOGGLE_TEMP_GAUGES.GetKeyDown() && !GameSettings.MODIFIER_KEY.GetKey())
		{
			if (TemperatureGaugeSystem.Instance != null)
			{
				GameSettings.TEMPERATURE_GAUGES_MODE++;
				if (GameSettings.TEMPERATURE_GAUGES_MODE > 3)
				{
					GameSettings.TEMPERATURE_GAUGES_MODE = 0;
				}
				GameSettings.SaveSettings();
				string text = cacheAutoLOC_135685;
				ScreenMessages.PostScreenMessage(GameSettings.TEMPERATURE_GAUGES_MODE switch
				{
					1 => text + cacheAutoLOC_135693, 
					2 => text + cacheAutoLOC_135696, 
					3 => text + cacheAutoLOC_135699, 
					_ => text + cacheAutoLOC_135690, 
				}, statusMessage);
			}
			else
			{
				ScreenMessages.PostScreenMessage(cacheAutoLOC_6003108, statusMessage);
			}
		}
		if (GameSettings.TOGGLE_TEMP_OVERLAY.GetKeyDown() && !GameSettings.MODIFIER_KEY.GetKey())
		{
			for (int i = 0; i < FlightGlobals.ActiveVessel.parts.Count; i++)
			{
				FlightGlobals.ActiveVessel.parts[i].ResetMPB();
			}
			PhysicsGlobals.ThermalColorsDebug = !PhysicsGlobals.ThermalColorsDebug;
			ScreenMessages.PostScreenMessage(cacheAutoLOC_6003106 + (PhysicsGlobals.ThermalColorsDebug ? cacheAutoLOC_6001072 : cacheAutoLOC_6001071), statusMessage);
		}
		if (GameSettings.TOGGLE_FLIGHT_FORCES.GetKeyDown() && !GameSettings.MODIFIER_KEY.GetKey())
		{
			PhysicsGlobals.AeroForceDisplay = !PhysicsGlobals.AeroForceDisplay;
			ScreenMessages.PostScreenMessage(cacheAutoLOC_6003107 + (PhysicsGlobals.AeroForceDisplay ? cacheAutoLOC_6001072 : cacheAutoLOC_6001071), statusMessage);
		}
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_135685 = Localizer.Format("#autoLOC_135685");
		cacheAutoLOC_135690 = Localizer.Format("#autoLOC_135690");
		cacheAutoLOC_135693 = Localizer.Format("#autoLOC_135693");
		cacheAutoLOC_135696 = Localizer.Format("#autoLOC_135696");
		cacheAutoLOC_135699 = Localizer.Format("#autoLOC_135699");
		cacheAutoLOC_6003108 = "<color=" + XKCDColors.HexFormat.KSPNotSoGoodOrange + ">" + Localizer.Format("#autoLOC_6003108") + "</color>";
		cacheAutoLOC_6003106 = Localizer.Format("#autoLOC_6003106") + ": ";
		cacheAutoLOC_6003107 = Localizer.Format("#autoLOC_6003107") + ": ";
		cacheAutoLOC_6001071 = Localizer.Format("#autoLOC_6001071");
		cacheAutoLOC_6001072 = Localizer.Format("#autoLOC_6001072");
	}
}

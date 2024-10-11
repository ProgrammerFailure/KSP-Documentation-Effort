using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class KbApp_VesselInfo : KbAppVessel
{
	public delegate bool boolDelegate_KbApp_VesselInfo(KbApp_VesselInfo kbapp, MapObject tgt);

	public GenericCascadingList cascadingListPrefab;

	public GenericCascadingList cascadingList;

	public KbItem_vesselInfo vesselInfoPrefab;

	public KbItem_flagImage flagImagePrefab;

	public TextMeshProUGUI update_type;

	public TextMeshProUGUI update_partCount;

	public TextMeshProUGUI update_mass;

	public UIStateRawImage update_typeIcon;

	public TextMeshProUGUI update_soi;

	public TextMeshProUGUI update_situation;

	public TextMeshProUGUI update_missiontime;

	public TextMeshProUGUI update_latitude;

	public TextMeshProUGUI update_longitude;

	public TextMeshProUGUI update_velocity;

	public TextMeshProUGUI update_altitude;

	public double vesselBrakeTime;

	public TextMeshProUGUI update_maxAccel;

	public TextMeshProUGUI update_burnTimeToZero;

	public static boolDelegate_KbApp_VesselInfo CallbackActivate;

	public static boolDelegate_KbApp_VesselInfo CallbackAfterActivate;

	public TextMeshProUGUI update_commNet_FirstHopDestination;

	public TextMeshProUGUI update_commNet_FirstHopStrength;

	public TextMeshProUGUI update_commNet_FirstHopDistance;

	public TextMeshProUGUI update_commNet_SignalStrength;

	public Button button_commNet_Mode;

	public float lastUpdate;

	private static string cacheAutoLOC_7003285;

	private static string cacheAutoLOC_7001411;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KbApp_VesselInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void ActivateApp(MapObject target)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CreateVesselInfoList(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KbItem_vesselInfo CreateVesselInfoBox()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool UpdateNowQuestionmark(float updateInterval, ref float lastUpdate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateVesselInfoList(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateCommNet(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}

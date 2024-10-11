using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class KbApp_UnownedInfo : KbAppUnowned
{
	public GenericCascadingList cascadingListPrefab;

	private GenericCascadingList cascadingList;

	public KbItem_unownedInfo unownedInfoPrefab;

	public GameObject textPrefab;

	private CometVessel cometVessel;

	private UIStateRawImage update_objectTypeIcon;

	private TextMeshProUGUI update_trackingStatus;

	private TextMeshProUGUI update_lastSeen;

	private TextMeshProUGUI update_lastSeenValue;

	private TextMeshProUGUI update_objSize;

	private Slider update_signalStrength;

	private TextMeshProUGUI update_objectSituation;

	private TextMeshProUGUI update_objectType;

	private TextMeshProUGUI update_obText;

	private float lastUpdate;

	private static string cacheAutoLOC_463116;

	private static string cacheAutoLOC_5050038;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KbApp_UnownedInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void ActivateApp(MapObject target)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateUnownedTrackingInfoList(IDiscoverable o)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateTrackingInfoList(IDiscoverable o)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool UpdateNowQuestionmark(float updateInterval, ref float lastUpdate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}

using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class ManeuverNodeEditorTabOrbitBasic : ManeuverNodeEditorTab
{
	[SerializeField]
	private TextMeshProUGUI apoapsisAltitude;

	[SerializeField]
	private TextMeshProUGUI apoapsisTime;

	[SerializeField]
	private TextMeshProUGUI periapsisAltitude;

	[SerializeField]
	private TextMeshProUGUI periapsisTime;

	[SerializeField]
	private TextMeshProUGUI orbitPeriod;

	private bool spaceDiscoveryUnlocked;

	private Orbit orbitToDisplay;

	private static string cacheAutoLOC_462439;

	private static string cacheAutoLOC_7001411;

	private static string cacheAutoLOC_6002332;

	private static string cacheAutoLOC_215362;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ManeuverNodeEditorTabOrbitBasic()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void SetInitialValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void UpdateUIElements()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool IsTimeAndPeriodUnlocked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CacheLocalStrings()
	{
		throw null;
	}
}

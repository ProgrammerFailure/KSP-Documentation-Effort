using System.Runtime.CompilerServices;

namespace SentinelMission;

public class SentinelModule : PartModule
{
	[KSPField(isPersistant = true)]
	public bool isTracking;

	[KSPField(guiFormat = "F3", guiActive = true, guiName = "#autoLOC_6002288", guiUnits = "")]
	public string status;

	private static string cacheAutoLOC_6002290;

	private static string cacheAutoLOC_6002296;

	private string FocusName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SentinelModule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(unfocusedRange = 3f, active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6002281")]
	public void StartTracking()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(unfocusedRange = 3f, active = false, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6002286")]
	public void StopTracking()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool TelescopeCanActivate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ShowMessage(string s)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}

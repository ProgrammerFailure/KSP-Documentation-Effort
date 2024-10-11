using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Adjusters;
using UnityEngine;

public class ModuleSAS : PartModule
{
	[KSPField]
	public int SASServiceLevel;

	public int targetSASServiceLevel;

	[KSPField]
	public int CommandModuleIndex;

	[KSPField]
	public bool RequireCrew;

	[KSPField]
	public bool standalone;

	[UI_Toggle(controlEnabled = true, disabledText = "#autoLOC_6001073", scene = UI_Scene.All, enabledText = "#autoLOC_6001074", affectSymCounterparts = UI_Scene.Editor)]
	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001371")]
	public bool standaloneToggle;

	[SerializeField]
	private bool standaloneOperational;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001372")]
	public string standaloneStateText;

	private ModuleCommand cmdModule;

	private List<AdjusterSASBase> adjusterCache;

	private static string cacheAutoLOC_257237;

	private static string cacheAutoLOC_6001373;

	private static string cacheAutoLOC_6001374;

	private static string cacheAutoLOC_218888;

	private static string cacheAutoLOC_6003057;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleSAS()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int GetValue()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnModuleAdjusterAdded(AdjusterPartModuleBase adjuster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnModuleAdjusterRemoved(AdjusterPartModuleBase adjuster)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected int ApplySASServiceLevelAdjustments(int serviceLevel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI.Dialogs;
using UnityEngine;

public class ModuleKerbNetAccess : PartModule, IAccessKerbNet, IModuleInfo
{
	[Serializable]
	public class Modes
	{
		public string Mode;

		public string displayMode;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Modes(string mode, string displaymode)
		{
			throw null;
		}
	}

	[KSPField]
	public float MinimumFoV;

	[KSPField]
	public float MaximumFoV;

	[KSPField]
	public uint EnhancedSituationMask;

	[KSPField]
	public float EnhancedMinimumFoV;

	[KSPField]
	public float EnhancedMaximumFoV;

	[KSPField]
	public float AnomalyDetection;

	[KSPField]
	public bool RequiresAnimation;

	private const string eventName = "OpenKerbNet";

	public List<Modes> modes;

	public List<string> effects;

	public bool isCheckingEffects;

	public bool partHasEffects;

	public bool partIsAnimating;

	public bool isEnhanced;

	private static string cacheAutoLOC_230499;

	private static string cacheAutoLOC_230500;

	private static string cacheAutoLOC_230501;

	private static string cacheAutoLOC_230505;

	private static string cacheAutoLOC_230517;

	private static string cacheAutoLOC_230521;

	private static string cacheAutoLOC_230533;

	private static string cacheAutoLOC_230585;

	private static string cacheAutoLOC_230588;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleKerbNetAccess()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadDisplayModes(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadRequiredEffects(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_230490")]
	public void OpenKerbNetAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(unfocusedRange = 3f, active = true, guiActiveUnfocused = false, externalToEVAOnly = false, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_230490")]
	public void OpenKerbNet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetModuleTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Callback<Rect> GetDrawModulePanelCallback()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetPrimaryField()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vessel GetKerbNetVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Part GetKerbNetPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<string> GetKerbNetDisplayModes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetKerbNetMinimumFoV()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetKerbNetMaximumFoV()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetKerbNetAnomalyChance()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetKerbNetErrorState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SituationIsEnhanced(Vessel.Situations situation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<Vessel.Situations> GetEnhancedSituations()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselSituationChanged(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> vs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasThisDialogOpen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CheckVesselSituationFoV(Vessel.Situations situation, bool firstRun)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InvokeCrewEffectCheck(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckRequiredEffects()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool PartHasRequiredEffects()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnAnimationGroupStateChanged(ModuleAnimationGroup group, bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}

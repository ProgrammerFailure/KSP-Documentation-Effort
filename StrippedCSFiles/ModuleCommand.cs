using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CommNet;
using UnityEngine;

public class ModuleCommand : PartModule, ICommNetControlSource, IResourceConsumer
{
	public enum ModuleControlState
	{
		NotEnoughCrew,
		NotEnoughResources,
		PartialManned,
		NoControlPoint,
		TouristCrew,
		PartialProbe,
		Nominal
	}

	protected ModuleControlState moduleState;

	protected VesselControlState localVesselControlState;

	protected bool commCapable;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001355")]
	public string commNetSignal;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001356")]
	public string commNetFirstHopDistance;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001357")]
	public string controlSrcStatusText;

	[KSPField]
	public bool hasHibernation;

	[UI_Toggle(disabledText = "#autoLOC_6001073", scene = UI_Scene.All, enabledText = "#autoLOC_6001074", affectSymCounterparts = UI_Scene.Editor)]
	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001358")]
	public bool hibernation;

	[KSPField]
	public double hibernationMultiplier;

	[UI_Toggle(disabledText = "#autoLOC_7001001", scene = UI_Scene.All, enabledText = "#autoLOC_7001000", affectSymCounterparts = UI_Scene.Editor)]
	[KSPField(advancedTweakable = true, isPersistant = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001359")]
	public bool hibernateOnWarp;

	[KSPField]
	public bool requiresPilot;

	[KSPField]
	public bool remoteControl;

	[KSPField]
	public bool requiresTelemetry;

	[KSPField]
	public int minimumCrew;

	protected ModuleResource lackingResource;

	protected int crewCount;

	protected int totalCrewCount;

	protected int pilots;

	protected bool networkInitialised;

	[KSPField]
	public string defaultControlPointDisplayName;

	private string controlPointDisplayName;

	[KSPField(isPersistant = true)]
	private string activeControlPointName;

	private ControlPoint activeControlPoint;

	private bool showControlPointVisual;

	public float cpArrowLength;

	public ArrowPointer cpforwardArrow;

	public ArrowPointer cpUpArrow;

	public ArrowPointer cpRightArrow;

	[SerializeField]
	private Color cpForwardColor;

	[SerializeField]
	private Color cpUpColor;

	[SerializeField]
	private Color cpRightColor;

	[SerializeField]
	private List<ControlPoint> controlPointsList;

	public DictionaryValueList<string, ControlPoint> controlPoints;

	private List<PartResourceDefinition> consumedResources;

	private static string cacheAutoLOC_7001411;

	private static string cacheAutoLOC_217448;

	private static string cacheAutoLOC_217464;

	private static string cacheAutoLOC_217408;

	private static string cacheAutoLOC_217417;

	private static string cacheAutoLOC_217429;

	private static string cacheAutoLOC_217437;

	private static string cacheAutoLOC_217509;

	private static string cacheAutoLOC_217513;

	private static string cacheAutoLOC_217517;

	private static string cacheAutoLOC_6003031;

	protected CommNetVessel Connection
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ModuleControlState ModuleState
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public VesselControlState VesselControlState
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public CommPath VesselControlPath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double VesselSignalStrength
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public SignalStrength VesselSignal
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool SignalRequired
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsHibernating
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string ActiveControlPointName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleCommand()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(advancedTweakable = true, guiActive = false, guiActiveEditor = false, guiName = "#autoLOC_8003232")]
	public void ToggleControlPointVisual()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UpdateControlPointVisuals()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void DestroyCPArrows()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiActiveEditor = true, guiName = "Change Control Point")]
	public void ChangeControlPoint()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetControlPoint(string newControlPointName = "_default")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateControlPointEvent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<PartResourceDefinition> GetConsumedResources()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnNetworkInitialised()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UpdateNetwork()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateControlState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselControlState GetControlSourceState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsCommCapable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual VesselControlState UpdateControlSourceState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001360")]
	public void MakeReferenceToggle(KSPActionParam act)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001360")]
	public virtual void MakeReference()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActiveUncommand = true, guiActive = true, guiName = "#autoLOC_900678")]
	public virtual void RenameVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001361")]
	public virtual void HibernateToggle(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ControlPoint GetControlPoint(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ControlPointsExist()
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	[SpecialName]
	string ICommNetControlSource.get_name()
	{
		throw null;
	}
}

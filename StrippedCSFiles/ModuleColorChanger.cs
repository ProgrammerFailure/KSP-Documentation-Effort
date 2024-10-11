using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ModuleColorChanger : PartModule, IScalarModule
{
	[KSPField]
	public string moduleID;

	[KSPField]
	public string shaderProperty;

	public int shaderPropertyInt;

	[KSPField]
	public FloatCurve redCurve;

	[KSPField]
	public FloatCurve greenCurve;

	[KSPField]
	public FloatCurve blueCurve;

	[KSPField]
	public FloatCurve alphaCurve;

	[KSPField]
	public float animRate;

	[KSPField(isPersistant = true)]
	public bool animState;

	[KSPField]
	public bool useRate;

	[KSPField]
	public bool toggleInEditor;

	[KSPField]
	public bool toggleInFlight;

	[KSPField]
	public bool toggleUnfocused;

	[KSPField]
	public bool toggleAction;

	[KSPField]
	public float unfocusedRange;

	[KSPField]
	public string toggleName;

	[KSPField]
	public string eventOnName;

	[KSPField]
	public string eventOffName;

	[KSPField]
	public KSPActionGroup defaultActionGroup;

	[KSPField]
	public bool useMaterialsList;

	[KSPField]
	public string materialsNames;

	[KSPField]
	public bool alphaOnly;

	[KSPField]
	public float redColor;

	[KSPField]
	public float blueColor;

	[KSPField]
	public float greenColor;

	public List<string> includedRenderers;

	public List<string> excludedRenderers;

	protected float currentRateState;

	protected Color color;

	protected List<Renderer> renderers;

	protected bool isValid;

	private EventData<float, float> OnMove;

	private EventData<float> OnStopped;

	private static string cacheAutoLOC_6003030;

	public float CurrentRateState
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string ScalarModuleID
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float GetScalar
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool CanMove
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public EventData<float, float> OnMoving
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public EventData<float> OnStop
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleColorChanger()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(unfocusedRange = 5f, guiActiveUnfocused = true, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001829")]
	public virtual void ToggleEvent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("Toggle Color", KSPActionGroup.REPLACEWITHDEFAULT)]
	public virtual void ToggleAction(KSPActionParam param)
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
	private void EditRenderers()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ProcessMaterialsList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetState(bool val)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetState(float val)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetState(Color val)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void UpdateColor(MaterialPropertyBlock mpb)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetEventName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetScalar(float t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetUIRead(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetUIWrite(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsMoving()
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

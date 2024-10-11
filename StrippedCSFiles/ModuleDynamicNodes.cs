using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ModuleDynamicNodes : PartModule
{
	public List<DynamicNodeSet> SetList;

	public const float NODE_RADIUS_ENABLED = 0.4f;

	public const float NODE_RADIUS_DISABLED = 0.0001f;

	[UI_Cycle(controlEnabled = true, scene = UI_Scene.Editor, affectSymCounterparts = UI_Scene.Editor)]
	[KSPField(isPersistant = true, guiActiveEditor = true, guiName = "#autoLOC_8005422")]
	public int setIndex;

	[KSPField]
	public bool autostrut;

	[KSPField]
	public string MenuName;

	[KSPField(isPersistant = true)]
	public int NodeSetIdx;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleDynamicNodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ChangeNodeSet(int newIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleNodeSet(int idx, bool showNodes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStartFinished(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnShipModified(ShipConstruct ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupAutoStrut()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitializeNodeSets()
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
	public void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}
}

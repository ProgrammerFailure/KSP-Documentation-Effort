using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FlagDecalBackground : FlagDecal, IThumbnailSetup, IPartMassModifier, IPartCostModifier
{
	[KSPField(isPersistant = true)]
	[SerializeField]
	protected string currentflagUrl;

	[UI_Toggle(affectSymCounterparts = UI_Scene.Editor)]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6006059")]
	public bool displayingPortrait;

	[KSPField(guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6006059")]
	[UI_Label]
	public string displayingPortraitLabel;

	[UI_ChooseOption(affectSymCounterparts = UI_Scene.Editor)]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6006057")]
	public int flagSize;

	[KSPField(guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6006057")]
	[UI_Label]
	public string flagSizeLabel;

	[KSPField(guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6006057")]
	[UI_Label]
	public string sizeLocked;

	[KSPField(isPersistant = true)]
	public int placementID;

	[SerializeField]
	public List<FlagMesh> flagMeshes;

	private UI_ChooseOption uI_ChooseOption;

	private UI_Toggle uI_FlagOrientationToggle;

	private FlagNameManager flagNameInfo;

	private int flagSizeOffset;

	private bool variantEventSubscribed;

	private float mass;

	private float cost;

	private bool updateFlagTexturebySym;

	private bool hasSurfaceAttachedParts;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FlagDecalBackground()
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
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void updateFlag(string flagURL)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnInventoryModeDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadFlagsTransformAndRenderer(GameObject icon = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DisplayingPortrait_OnValueModified(object arg1)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DisplayingPortrait_OnValueSymmetryModified(BaseField field, object arg1)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6006058")]
	public void SetFlag()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetFlagBrowserTexture(string flagUrl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UnsubscribeToFlagSelect()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToolboxSetFlagTexture(GameObject icon, string flagURL = "")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AssumeSnapshotPosition(GameObject icon, ProtoPartSnapshot partSnapshot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string ThumbSuffix(ProtoPartSnapshot partSnapshot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPartDetached()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateUIChooseOptions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IsFlagOrientationCounterPartAvailable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EnableCurrentFlagMesh(BaseField field, object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnVariantApplied(Part appliedPart, PartVariant partVariant)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateFlagRenderers()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CheckSymmetry()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool HasSurfaceAttachedParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorPartEvent(ConstructionEventType evt, Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int FlagMeshIndex(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetModuleMass(float defaultMass, ModifierStagingSituation sit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModifierChangeWhen GetModuleMassChangeWhen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetModuleCost(float defaultCost, ModifierStagingSituation sit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModifierChangeWhen GetModuleCostChangeWhen()
	{
		throw null;
	}
}

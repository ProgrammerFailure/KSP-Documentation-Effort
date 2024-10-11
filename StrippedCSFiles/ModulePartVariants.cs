using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ModulePartVariants : PartModule, IModuleInfo, IMultipleDragCube, IPartMassModifier, IPartCostModifier
{
	[SerializeField]
	public List<PartVariant> variantList;

	[KSPField]
	public bool useMultipleDragCubes;

	[KSPField]
	public bool useProceduralDragCubes;

	private List<Material> partMaterials;

	[UI_VariantSelector(controlEnabled = true, scene = UI_Scene.Editor, affectSymCounterparts = UI_Scene.Editor)]
	[KSPField]
	private int variantIndex;

	[KSPField(isPersistant = true)]
	public bool useVariantMass;

	internal ModuleJettison moduleJettison;

	public PartVariant SelectedVariant
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsMultipleCubesActive
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModulePartVariants()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVariantChanged(BaseField field, object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVariantSymmetrycallyChanged(BaseField field, object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ApplyVariant(Part part, Transform meshRoot, PartVariant variant, Material[] materials, bool skipShader)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ApplyVariant(Part part, Transform meshRoot, PartVariant variant, Material[] materials, bool skipShader, int variantIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void UpdateNode(AttachNode partNode, AttachNode variantNode, Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void UpdatePartPosition(AttachNode currentNode, AttachNode newNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetVariantIndex(string variantName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetCurrentVariantIndex()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int GetVariantThemeIndex(string variantThemeName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetVariant(string variantName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<string> GetVariantNames()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasVariant(string variantName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetVariantTheme(string variantThemeName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<string> GetVariantThemeNames()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasVariantTheme(string variantThemeName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshVariant()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetCubes(bool state)
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
	public override string GetModuleDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetModuleTitle()
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
	public static string GetTitle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string[] GetDragCubeNames()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AssumeDragCubePosition(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool UsesProceduralDragCubes()
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

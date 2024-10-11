using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ModulePhysicMaterial : PartModule
{
	[KSPField(isPersistant = true)]
	public PhysicMaterialCombine bounceCombine;

	[KSPField(isPersistant = true)]
	public float bounciness;

	[KSPField(isPersistant = true)]
	public float dynamicFriction;

	[KSPField(isPersistant = true)]
	public float staticFriction;

	[KSPField(isPersistant = true)]
	public PhysicMaterialCombine frictionCombine;

	[KSPField(isPersistant = true)]
	public string activePhysicMaterialName;

	private string activePhysicMaterialDisplayName;

	private Collider[] partColliders;

	private Renderer[] partRenderers;

	private PhysicMaterial physicMaterial;

	[SerializeField]
	public List<string> physicMaterialNames;

	private int cpIndex;

	[SerializeField]
	private List<PhysicMaterialColor> moduleMaterialColorList;

	private Dictionary<string, PhysicMaterialColor> physicMaterialColors;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModulePhysicMaterial()
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
	[KSPEvent(guiActiveEditor = true, guiName = "#autoLOC_6011081")]
	public void ChangePhysicMaterial()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVariantApplied(Part appliedPart, PartVariant partVariant)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ApplyListMaterial(string materialName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPhysicMaterial(string materialName, bool useDefault)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPhysicMaterial(PhysicMaterialDefinition materialDefinition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdatePhysicMaterialEvent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetPhysicMaterialColors()
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
}

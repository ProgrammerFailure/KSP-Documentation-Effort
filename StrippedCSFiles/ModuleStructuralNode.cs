using System.Runtime.CompilerServices;
using UnityEngine;

public class ModuleStructuralNode : PartModule, IActivateOnDecouple
{
	public Transform structTransform;

	[KSPField]
	public string attachNodeNames;

	[KSPField]
	public float nodeRadius;

	[KSPField]
	public string rootObject;

	[KSPField]
	public bool spawnManually;

	[KSPField(isPersistant = true)]
	public bool spawnState;

	[KSPField]
	public bool reverseVisibility;

	[KSPField(isPersistant = true)]
	public bool visibilityState;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleStructuralNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CheckDisplayEditor(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SpawnStructure()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DespawnStructure()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetNodeState(bool showNodes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckDisplay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DecoupleAction(string nodeName, bool weDecouple)
	{
		throw null;
	}
}

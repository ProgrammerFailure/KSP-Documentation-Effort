using System.Runtime.CompilerServices;
using UnityEngine;

namespace CompoundParts;

public class CModuleStrut : CompoundPartModule, IModuleInfo
{
	[KSPField]
	public float linearStrength;

	[KSPField]
	public float angularStrength;

	public PartJoint strutJoint;

	public Part jointRoot;

	public Part jointTarget;

	private AttachNode targetNode;

	private static string cacheAutoLOC_217004;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CModuleStrut()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnTargetSet(Part target)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnTargetLost()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStartFinished(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetModuleTitle()
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

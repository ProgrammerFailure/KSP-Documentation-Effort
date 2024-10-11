using System.Runtime.CompilerServices;
using UnityEngine;

public class ModuleMirrorPlacement : PartModule
{
	[KSPField]
	public bool applyMirrorRotationXAxis;

	[KSPField]
	public float mirrorRotationXAxis;

	[KSPField]
	public bool applyMirrorRotationYAxis;

	[KSPField]
	public float mirrorRotationYAxis;

	[KSPField]
	public bool applyMirrorRotationZAxis;

	[KSPField]
	public float mirrorRotationZAxis;

	[KSPField]
	public bool ignoreMirrorIfSurfaceXOffset;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleMirrorPlacement()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool OnWillBeMirrored(ref Quaternion rotation, AttachNode selPartNode, Part partParent)
	{
		throw null;
	}
}

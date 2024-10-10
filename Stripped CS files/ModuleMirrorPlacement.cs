using UnityEngine;

public class ModuleMirrorPlacement : PartModule
{
	[KSPField]
	public bool applyMirrorRotationXAxis = true;

	[KSPField]
	public float mirrorRotationXAxis = 180f;

	[KSPField]
	public bool applyMirrorRotationYAxis;

	[KSPField]
	public float mirrorRotationYAxis = 180f;

	[KSPField]
	public bool applyMirrorRotationZAxis = true;

	[KSPField]
	public float mirrorRotationZAxis = 180f;

	[KSPField]
	public bool ignoreMirrorIfSurfaceXOffset = true;

	public override bool OnWillBeMirrored(ref Quaternion rotation, AttachNode selPartNode, Part partParent)
	{
		if (!ignoreMirrorIfSurfaceXOffset && selPartNode != null && selPartNode.nodeType == AttachNode.NodeType.Surface && selPartNode.orientation.x != 0f)
		{
			return false;
		}
		if (applyMirrorRotationXAxis)
		{
			rotation *= Quaternion.AngleAxis(mirrorRotationXAxis, Vector3.right);
		}
		if (applyMirrorRotationYAxis)
		{
			rotation *= Quaternion.AngleAxis(mirrorRotationYAxis, Vector3.up);
		}
		if (applyMirrorRotationZAxis)
		{
			rotation *= Quaternion.AngleAxis(mirrorRotationZAxis, Vector3.forward);
		}
		return true;
	}
}

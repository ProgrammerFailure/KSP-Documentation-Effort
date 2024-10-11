using System.Runtime.CompilerServices;
using UnityEngine;

namespace CompoundParts;

public class CModuleLinkedMesh : CompoundPartModule, ICMTweakTarget
{
	[KSPField]
	public string lineObjName;

	[KSPField]
	public string mainAnchorName;

	[KSPField]
	public string targetAnchorName;

	[KSPField]
	public string anchorCapName;

	[KSPField]
	public string targetCapName;

	[KSPField]
	public string targetColliderName;

	public Transform line;

	public Transform mainAnchor;

	public Transform targetAnchor;

	public Transform startCap;

	public Transform endCap;

	public Transform targetCollider;

	private bool activeInPreview;

	private bool tweakingTarget;

	private bool targetIsRobotic;

	public bool setupFinished;

	public float lineMinimumLength;

	public bool TweakingTarget
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CModuleLinkedMesh()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Transform GetReferenceTransform()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetSymmetryValues(Vector3 newPosition, Quaternion newRotation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Collider[] GetSelectedColliders()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SelectTweakTarget(Vector3 mousePosition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
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
	public override void OnPreviewAttachment(Vector3 rDir, Vector3 rPos, Quaternion rRot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnPreviewEnd()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnTargetUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void TrackAnchor(bool setTgtAnchor, Vector3 rDir, Vector3 rPos, Quaternion rRot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetTargetPointer()
	{
		throw null;
	}
}

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class AttachNode
{
	public enum NodeType
	{
		Stack,
		Surface,
		Dock
	}

	public string id;

	public Vector3 originalPosition;

	public Vector3 originalOrientation;

	public Vector3 originalSecondaryAxis;

	public Vector3 position;

	public Vector3 orientation;

	public Vector3 secondaryAxis;

	public int size;

	public float radius;

	public float breakingForce;

	public float breakingTorque;

	public Vector3 offset;

	public Part owner;

	public Part attachedPart;

	public string srfAttachMeshName;

	public GameObject icon;

	public AttachNodeMethod attachMethod;

	public NodeType nodeType;

	public bool requestGate;

	public bool ResourceXFeed;

	public bool AllowOneWayXFeed;

	public bool rigid;

	public float contactArea;

	public float overrideDragArea;

	[SerializeField]
	private float nodeDistEpsilon;

	public Transform nodeTransform;

	public uint attachedPartId;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AttachNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AttachNode(string id, Transform transform, int size, AttachNodeMethod attachMethod, bool crossfeed, bool rigid)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FindAttachedPart(List<Part> parts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AttachNode FindOpposingNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DestroyNodeIcon()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static AttachNode Clone(AttachNode referenceNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ReverseSrfNodeDirection(AttachNode fromNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void ChangeSrfNodePosition()
	{
		throw null;
	}
}

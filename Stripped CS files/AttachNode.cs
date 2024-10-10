using System;
using System.Collections.Generic;
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

	public string id = "";

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

	public float overrideDragArea = -1f;

	[SerializeField]
	public float nodeDistEpsilon = 0.001f;

	public Transform nodeTransform;

	public uint attachedPartId;

	public AttachNode()
	{
		position = Vector3.zero;
		orientation = Vector3.up;
		secondaryAxis = Vector3.zero;
		radius = 0.4f;
		offset = Vector3.zero;
		owner = null;
		attachedPart = null;
		attachMethod = AttachNodeMethod.FIXED_JOINT;
		srfAttachMeshName = "";
		ResourceXFeed = true;
		AllowOneWayXFeed = true;
		rigid = false;
	}

	public AttachNode(string id, Transform transform, int size, AttachNodeMethod attachMethod, bool crossfeed, bool rigid)
	{
		this.id = id;
		nodeTransform = transform;
		position = transform.position;
		orientation = transform.forward;
		secondaryAxis = Vector3.zero;
		originalPosition = transform.position;
		originalOrientation = transform.forward;
		originalSecondaryAxis = Vector3.zero;
		radius = 0.4f;
		offset = Vector3.zero;
		owner = null;
		attachedPart = null;
		this.attachMethod = attachMethod;
		this.size = size;
		AllowOneWayXFeed = true;
		ResourceXFeed = crossfeed;
		this.rigid = rigid;
	}

	public void FindAttachedPart(List<Part> parts)
	{
		attachedPart = parts.GetPartByCraftID(attachedPartId);
	}

	public AttachNode FindOpposingNode()
	{
		if (attachedPart != null)
		{
			return attachedPart.FindAttachNodeByPart(owner);
		}
		return null;
	}

	public void DestroyNodeIcon()
	{
		UnityEngine.Object.Destroy(icon.GetComponent<Renderer>().material);
		UnityEngine.Object.Destroy(icon);
	}

	public static AttachNode Clone(AttachNode referenceNode)
	{
		return (AttachNode)referenceNode.MemberwiseClone();
	}

	public void ReverseSrfNodeDirection(AttachNode fromNode)
	{
		if (!(owner == null) && !(fromNode.owner == null))
		{
			fromNode.attachedPart = null;
			attachedPart = fromNode.owner;
			Vector3 vector = fromNode.owner.transform.TransformPoint(fromNode.position);
			position = owner.transform.InverseTransformPoint(vector);
		}
	}

	public void ChangeSrfNodePosition()
	{
		if (attachedPart != null || owner == null)
		{
			return;
		}
		int count = owner.attachNodes.Count;
		int count2 = owner.children.Count;
		bool[] array = new bool[count];
		int num = -1;
		for (int i = 0; i < count; i++)
		{
			AttachNode attachNode = owner.attachNodes[i];
			array[i] = attachNode.attachedPart == null;
			if ((position - attachNode.position).sqrMagnitude < nodeDistEpsilon)
			{
				num = i;
			}
			if (!array[i])
			{
				continue;
			}
			for (int j = 0; j < count2; j++)
			{
				Part part = owner.children[j];
				AttachNode srfAttachNode = part.srfAttachNode;
				if (srfAttachNode.attachedPart == owner && !((owner.transform.TransformPoint(attachNode.position) - part.transform.TransformPoint(srfAttachNode.position)).sqrMagnitude >= nodeDistEpsilon))
				{
					array[i] = false;
					break;
				}
			}
		}
		if (num > 0 && array[num])
		{
			return;
		}
		int num2 = 0;
		while (true)
		{
			if (num2 < count)
			{
				if (array[num2])
				{
					break;
				}
				num2++;
				continue;
			}
			Debug.LogWarningFormat("[AttachNode]: Unable to invert surface node on {0}. Set to center of part.", owner.partInfo.title);
			position = Vector3.zero;
			orientation = originalOrientation;
			secondaryAxis = originalSecondaryAxis;
			return;
		}
		position = owner.attachNodes[num2].position;
		orientation = owner.attachNodes[num2].orientation;
		secondaryAxis = originalSecondaryAxis;
	}
}

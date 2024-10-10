using System;
using UnityEngine;

public class ThermalLink
{
	public Part remotePart;

	public double contactArea;

	public double contactAreaSqrt;

	public double temperatureDelta;

	public Vector3 partToNode;

	public Vector3 remoteToNode;

	public bool useOrientation;

	public ThermalLink(Part part, Part remotePart)
	{
		this.remotePart = remotePart;
		AttachNode attachNode = remotePart.FindAttachNodeByPart(part);
		if (attachNode != null)
		{
			if (attachNode.contactArea > 0f)
			{
				contactArea = attachNode.contactArea;
				AttachNode attachNode2 = part.FindAttachNodeByPart(remotePart);
				if (attachNode2 != null)
				{
					partToNode = (attachNode.position - part.partTransform.position).normalized;
					remoteToNode = (attachNode2.position - remotePart.partTransform.position).normalized;
					useOrientation = true;
				}
			}
		}
		else
		{
			attachNode = part.FindAttachNodeByPart(remotePart);
			if (attachNode != null && attachNode.contactArea > 0f)
			{
				contactArea = attachNode.contactArea;
				AttachNode attachNode3 = remotePart.FindAttachNodeByPart(part);
				if (attachNode3 != null)
				{
					partToNode = (attachNode3.position - part.partTransform.position).normalized;
					remoteToNode = (attachNode.position - remotePart.partTransform.position).normalized;
					useOrientation = true;
				}
			}
		}
		contactArea = Math.Max(contactArea, 0.01);
		contactAreaSqrt = Math.Sqrt(contactArea);
		if (double.IsNaN(contactAreaSqrt))
		{
			contactAreaSqrt = 1.0;
		}
	}
}

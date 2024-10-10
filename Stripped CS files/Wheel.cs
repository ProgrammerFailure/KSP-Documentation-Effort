using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Wheel : IConfigNode
{
	public string wheelName;

	public string wheelColliderName;

	public string suspensionTransformName;

	public Transform suspensionNeutralPoint;

	public string suspensionNeutralPointName;

	public string damagedObjectName;

	public Transform damagedObject;

	public List<Transform> wheelTransforms;

	public Transform suspensionTransform;

	public Transform colliderTransform;

	public WheelCollider whCollider;

	public float rotateX;

	public float rotateY;

	public float rotateZ;

	public Vector3 wheelAxis;

	public RaycastHit hit;

	public bool isValid;

	public bool damageAble;

	public bool debug;

	public int layerMask;

	public WheelHit wheelHit;

	public float rescaleFactor = 1.25f;

	public Wheel()
	{
		rotateX = 0f;
		rotateY = 0f;
		rotateZ = 0f;
		wheelTransforms = new List<Transform>();
		layerMask = LayerUtil.DefaultEquivalent | 0x8000 | 0x10000 | 0x80000;
	}

	public void Update()
	{
		if (!isValid)
		{
			return;
		}
		Vector3 position = suspensionNeutralPoint.position;
		if (Physics.Raycast(position, suspensionNeutralPoint.up * -1f, out hit, (whCollider.suspensionDistance + whCollider.radius) * rescaleFactor, layerMask))
		{
			suspensionTransform.position = hit.point + suspensionNeutralPoint.up * whCollider.radius * rescaleFactor;
			if (debug)
			{
				Debug.DrawRay(position, suspensionNeutralPoint.up * -1f, Color.green, (whCollider.suspensionDistance + whCollider.radius) * rescaleFactor);
			}
		}
		else
		{
			suspensionTransform.position = position - suspensionNeutralPoint.up * (whCollider.suspensionDistance * rescaleFactor);
			if (debug)
			{
				Debug.DrawRay(position, suspensionNeutralPoint.up * -1f, Color.red, (whCollider.suspensionDistance + whCollider.radius) * rescaleFactor);
			}
		}
		Vector3 eulers = wheelAxis * (whCollider.rpm / 60f * 360f) * TimeWarp.deltaTime;
		int i = 0;
		for (int count = wheelTransforms.Count; i < count; i++)
		{
			wheelTransforms[i].Rotate(eulers);
		}
	}

	public void Load(ConfigNode node)
	{
		wheelName = node.GetValue("wheelName");
		wheelColliderName = node.GetValue("wheelColliderName");
		suspensionTransformName = node.GetValue("suspensionTransformName");
		suspensionNeutralPointName = node.GetValue("suspensionNeutralPointName");
		damagedObjectName = node.GetValue("damagedObjectName");
		if (node.HasValue("rotateX"))
		{
			rotateX = float.Parse(node.GetValue("rotateX"));
		}
		if (node.HasValue("rotateY"))
		{
			rotateY = float.Parse(node.GetValue("rotateY"));
		}
		if (node.HasValue("rotateZ"))
		{
			rotateZ = float.Parse(node.GetValue("rotateZ"));
		}
		wheelAxis = new Vector3(rotateX, rotateY, rotateZ).normalized;
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("wheelName", wheelName);
		node.AddValue("wheelColliderName", wheelColliderName);
		node.AddValue("suspensionTransformName", suspensionTransformName);
		node.AddValue("suspensionNeutralPointName", suspensionNeutralPointName);
		node.AddValue("damagedObjectName", damagedObjectName);
		node.AddValue("rotateX", rotateX);
		node.AddValue("rotateY", rotateY);
		node.AddValue("rotateZ", rotateZ);
	}

	public void damageWheel()
	{
		if (damagedObject != null)
		{
			damagedObject.gameObject.SetActive(value: true);
			int i = 0;
			for (int count = wheelTransforms.Count; i < count; i++)
			{
				wheelTransforms[i].gameObject.SetActive(value: false);
			}
			if (colliderTransform != null)
			{
				colliderTransform.gameObject.SetActive(value: false);
			}
		}
	}

	public void repairWheel()
	{
		if (damagedObject != null)
		{
			damagedObject.gameObject.SetActive(value: false);
			int i = 0;
			for (int count = wheelTransforms.Count; i < count; i++)
			{
				wheelTransforms[i].gameObject.SetActive(value: true);
			}
			if (colliderTransform != null)
			{
				colliderTransform.gameObject.SetActive(value: true);
			}
		}
	}
}

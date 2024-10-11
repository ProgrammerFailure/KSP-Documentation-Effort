using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

	private int layerMask;

	private WheelHit wheelHit;

	public float rescaleFactor;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Wheel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void damageWheel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void repairWheel()
	{
		throw null;
	}
}

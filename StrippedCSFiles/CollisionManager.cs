using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
	private class VesselColliderList
	{
		public Guid vesselId;

		public List<PartColliderList> colliderList;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public VesselColliderList(Guid vId)
		{
			throw null;
		}
	}

	private class PartColliderList
	{
		public uint partPersistentId;

		public bool sameVesselCollision;

		public List<Collider> colliders;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public PartColliderList(uint persistentId, bool sameVslCollision)
		{
			throw null;
		}
	}

	private bool requireUpdate;

	private static List<VesselColliderList> vesselsList;

	public static CollisionManager Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CollisionManager()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static CollisionManager()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCollisionIgnoreUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateCollisionIgnores()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<VesselColliderList> GetAllVesselColliders()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdatePartCollisionIgnores()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void UpdateAllColliders()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void IgnoreCollidersOnVessel(Vessel vessel, params Collider[] ignoreColliders)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetCollidersOnVessel(Vessel vessel, bool ignore, params Collider[] ignoreColliders)
	{
		throw null;
	}
}

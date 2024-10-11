using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RunwayCollisionHandler : MonoBehaviour
{
	[Serializable]
	public class RunwaySection
	{
		public DestructibleBuilding dBuilding;

		public Collider sectionCollider;

		public float sStart;

		public float sEnd;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public RunwaySection()
		{
			throw null;
		}
	}

	public Vector3 runwayAxis;

	public RunwaySection[] runwaySections;

	public Collider mainCollider;

	private int loaded;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RunwayCollisionHandler()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneLoadRequested(GameScenes scn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSectionLoaded(DestructibleBuilding db)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAllSectionsLoaded()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCollisionEnter(Collision c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onStructureCollapsing(DestructibleBuilding dB)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onStructureRepaired(DestructibleBuilding dB)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Disable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ReEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}
}

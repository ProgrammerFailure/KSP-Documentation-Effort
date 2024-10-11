using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class ProceduralSpaceObject : MonoBehaviour
{
	[Serializable]
	public class ModValue
	{
		public string name;

		public float minValue;

		public float maxValue;

		public float radiusFactor;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ModValue()
		{
			throw null;
		}
	}

	[Serializable]
	public class ModWrapper
	{
		public string name;

		public ModValue[] values;

		public PQSMod mod
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			set
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ModWrapper()
		{
			throw null;
		}
	}

	[SerializeField]
	protected int seed;

	[SerializeField]
	public float radius;

	[SerializeField]
	protected SphereBaseSO visualSphere;

	[SerializeField]
	protected Material primaryMaterial;

	[SerializeField]
	protected Material secondaryMaterial;

	[SerializeField]
	protected string visualLayer;

	[SerializeField]
	protected string visualTag;

	[SerializeField]
	protected SphereBaseSO colliderSphere;

	[SerializeField]
	protected PhysicMaterial colliderMaterial;

	[SerializeField]
	protected string colliderLayer;

	[SerializeField]
	protected string colliderTag;

	[SerializeField]
	protected SphereBaseSO convexSphere;

	[SerializeField]
	protected PhysicMaterial convexMaterial;

	[SerializeField]
	protected string convexLayer;

	[SerializeField]
	protected string convexTag;

	[SerializeField]
	protected bool debugGenTime;

	[SerializeField]
	protected List<ModWrapper> mods;

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected ProceduralSpaceObject()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Mesh CreateMeshVisual(SphereBaseSO sphere, float radius, List<PQSMod> modArray, out float volume, out float highestPoint)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Mesh CreateMeshCollider(SphereBaseSO sphere, float radius, List<PQSMod> modArray)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetSphereVolume(float r)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Mod_OnSetup(List<PQSMod> mods)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Mod_Build(List<PQSMod> mods, PQS.VertexBuildData vert)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected float RangefinderGeneric(Transform trf)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Update Wrappers")]
	private void UpdateWrappers()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool HasWrapperOfTypeName(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("New Seed")]
	private void NewSeed()
	{
		throw null;
	}
}

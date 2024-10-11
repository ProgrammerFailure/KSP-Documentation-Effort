using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class LandClassROC
{
	public ROCDefinition rocType;

	public string rocName;

	public int gameSeed;

	public int quadSeed;

	public int maxCache;

	public float cacheMaxSubdivMultiplier;

	public float rocFrequency;

	public bool castShadows;

	public bool recieveShadows;

	public int maxROCs;

	public int maxROCsMultiplier;

	private GameObject rocParent;

	private GameObject rocObject;

	[HideInInspector]
	private Stack<PQSMod_ROCScatterQuad> cacheUnassigned;

	[HideInInspector]
	private List<PQSMod_ROCScatterQuad> cacheAssigned;

	private int cacheUnassignedCount;

	private int cacheAssignedCount;

	private PQS sphere;

	private Vector3 rocPOS;

	private Vector3 quadNormal;

	private int rndCount;

	private PQSMod_ROCScatterQuad qc;

	private Vector3 rocUp;

	private Quaternion rocRot;

	private float rocAngle;

	private int rocLoop;

	private int rocN;

	private bool cacheCreated;

	private bool cacheRecreated;

	private PQSROCControl rocControl;

	public CelestialBody celestialBody;

	public int minLevel
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
	public LandClassROC(ROCDefinition rocDefinition, PQSROCControl rocControl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(PQS sphere)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateCache()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SphereActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SphereInactive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddScatterMeshController(PQ quad, int rocIDCounter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildCache(int countToAdd)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void DestroyQuad(PQSMod_ROCScatterQuad q)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void CreateScatterMesh(PQSMod_ROCScatterQuad q)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearCache()
	{
		throw null;
	}
}

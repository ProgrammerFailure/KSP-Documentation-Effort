using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PQSROCControl : PQSMod
{
	[Serializable]
	public class RocPositionInfo
	{
		public string rocType;

		public Vector3 position;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public RocPositionInfo()
		{
			throw null;
		}
	}

	public CelestialBody celestialBody;

	public string currentCBName;

	public List<LandClassROC> rocs;

	public float quadArea;

	public DictionaryValueList<int, List<RocPositionInfo>> rocPositionsUsed;

	private int itr;

	private bool rocsActive;

	private int rocMinSubdev;

	private int rocCount;

	private bool allowROCScatter;

	private DictionaryValueList<int, List<ROCDefinition>> quadAppliedROCTypes;

	private List<ROCDefinition> quadROCTypesCache;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSROCControl()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnQuadPreBuild(PQ quad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnVertexBuild(PQS.VertexBuildData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnQuadBuilt(PQ quad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSphereStarted()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSphereReset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnQuadDestroy(PQ quad)
	{
		throw null;
	}
}

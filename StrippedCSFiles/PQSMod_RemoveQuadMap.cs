using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Misc/Remove Quads (Map)")]
public class PQSMod_RemoveQuadMap : PQSMod
{
	public MapSO map;

	public float mapDeformity;

	public float minHeight;

	public float maxHeight;

	private bool quadVisible;

	private double mapHeight;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_RemoveQuadMap()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
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
	public override void OnVertexBuild(PQS.VertexBuildData vbData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnQuadBuilt(PQ quad)
	{
		throw null;
	}
}

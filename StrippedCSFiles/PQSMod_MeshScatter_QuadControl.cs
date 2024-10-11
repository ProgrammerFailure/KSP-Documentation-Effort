using System.Runtime.CompilerServices;
using UnityEngine;

public class PQSMod_MeshScatter_QuadControl : MonoBehaviour
{
	public MeshFilter mf;

	public bool isVisible;

	public bool isBuilt;

	public int seed;

	public PQ quad;

	public int count;

	public PQSMod_MeshScatter scatter;

	private static double maxTargetSpeedForBuild;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_MeshScatter_QuadControl()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static PQSMod_MeshScatter_QuadControl()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(PQ quad, int seed, PQSMod_MeshScatter scatter, int count)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnQuadVisible(PQ quad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnQuadInvisible(PQ quad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnQuadDestroy(PQ quad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnQuadUpdate(PQ quad)
	{
		throw null;
	}
}

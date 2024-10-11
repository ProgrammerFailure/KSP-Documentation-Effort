using System.Runtime.CompilerServices;
using UnityEngine;

namespace ProceduralFairings;

public class MeshArc
{
	public MeshPoint[] inner;

	public MeshPoint[] outer;

	public FairingXSection xSection;

	public float[] hOffsetOuter;

	public float[] hOffsetInner;

	public Vector3 pivot;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MeshArc(FairingXSection xSection, MeshPoint[] inner, MeshPoint[] outer)
	{
		throw null;
	}
}

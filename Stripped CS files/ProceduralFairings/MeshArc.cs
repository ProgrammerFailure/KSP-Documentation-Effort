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

	public MeshArc(FairingXSection xSection, MeshPoint[] inner, MeshPoint[] outer)
	{
		this.xSection = xSection;
		this.inner = inner;
		this.outer = outer;
	}
}

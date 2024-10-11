using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Material/Quad Projective UV")]
public class PQSMod_MaterialQuadRelative : PQSMod
{
	private static int shaderPropertyUpMatrix;

	private static int shaderPropertyLocalMatrix;

	private static int shaderPropertySubDiv;

	private Matrix4x4 matUp;

	private Matrix4x4 matW2L;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PQSMod_MaterialQuadRelative()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnQuadBuilt(PQ quad)
	{
		throw null;
	}
}

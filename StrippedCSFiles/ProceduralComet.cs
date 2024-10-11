using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ProceduralComet : ProceduralSpaceObject
{
	private PComet pcGenerated;

	[SerializeField]
	private SphereBaseSO optimizedCollider;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProceduralComet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PComet Generate(int seed, float radius, Transform parent, Func<Transform, float> rangefinder, Callback onComplete, bool optimizeCollider, bool isSecondary = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private PComet CreatePComet(float radius, List<PQSMod> modArray, Func<Transform, float> rangefinder, Callback onComplete, bool isSecondary, bool optimizeCollider, out Color secondaryColor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Rebuild")]
	private void Rebuild()
	{
		throw null;
	}
}

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ProceduralAsteroid : ProceduralSpaceObject
{
	private PAsteroid paGenerated;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProceduralAsteroid()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PAsteroid Generate(int seed, float radius, Transform parent, Func<Transform, float> rangefinder, Callback onComplete, bool isSecondary = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private PAsteroid CreatePAsteroid(float radius, List<PQSMod> modArray, Func<Transform, float> rangefinder, Callback onComplete, bool isSecondary, out Color secondaryColor)
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

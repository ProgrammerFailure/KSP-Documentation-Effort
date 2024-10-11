using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FXObject
{
	public GameObject effectObj;

	public List<ParticleSystem> systems;

	public AudioClip effectSound;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FXObject()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FXObject(GameObject obj)
	{
		throw null;
	}
}

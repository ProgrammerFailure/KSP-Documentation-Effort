using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class DetonatorComponent : MonoBehaviour
{
	public bool on;

	public bool detonatorControlled;

	[HideInInspector]
	public float startSize;

	public float size;

	public float explodeDelayMin;

	public float explodeDelayMax;

	[HideInInspector]
	public float startDuration;

	public float duration;

	[HideInInspector]
	public float timeScale;

	[HideInInspector]
	public float startDetail;

	public float detail;

	[HideInInspector]
	public Color startColor;

	public Color color;

	[HideInInspector]
	public Vector3 startLocalPosition;

	public Vector3 localPosition;

	[HideInInspector]
	public Vector3 startForce;

	public Vector3 force;

	[HideInInspector]
	public Vector3 startVelocity;

	public Vector3 velocity;

	public float detailThreshold;

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected DetonatorComponent()
	{
		throw null;
	}

	public abstract void Explode();

	public abstract void Init();

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetStartValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Detonator MyDetonator()
	{
		throw null;
	}
}

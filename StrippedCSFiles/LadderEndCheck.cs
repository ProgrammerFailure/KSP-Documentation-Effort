using System.Runtime.CompilerServices;
using UnityEngine;

public class LadderEndCheck : MonoBehaviour
{
	private bool reached;

	[SerializeField]
	private KerbalEVA kerbalEVA;

	public bool Reached
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LadderEndCheck()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnTriggerEnter(Collider o)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnTriggerStay(Collider o)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnTriggerExit(Collider o)
	{
		throw null;
	}
}

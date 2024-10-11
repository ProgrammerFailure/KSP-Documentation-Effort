using System.Runtime.CompilerServices;
using UnityEngine;

public class InternalAltimeterThreeHands : InternalModule
{
	[KSPField]
	public string hand100Name;

	[KSPField]
	public string hand1000Name;

	[KSPField]
	public string hand10000Name;

	[KSPField]
	public Vector3 handAxis;

	[KSPField]
	public float smoothing;

	public Transform hand100;

	public Quaternion hand100Initial;

	public Transform hand1000;

	public Quaternion hand1000Initial;

	public Transform hand10000;

	public Quaternion hand10000Initial;

	private float altitude;

	private float hundreds;

	private float thousands;

	private float tenThousands;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InternalAltimeterThreeHands()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUpdate()
	{
		throw null;
	}
}

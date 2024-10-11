using System.Runtime.CompilerServices;
using UnityEngine;

public class LinearGauge : MonoBehaviour
{
	public Vector3 minPos;

	public Vector3 maxPos;

	public float maxValue;

	public float minValue;

	public float responsiveness;

	public float logarithmic;

	public float exponent;

	public GaugeLEDRange[] ledRanges;

	public Transform pointer;

	public LED led;

	private float rawValue;

	private float currValue;

	private float tgtValue;

	private Vector3 tgtPos;

	private int currentRange;

	public float Value
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LinearGauge()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void setValue(double val)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void setValue(float val)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void startSound(bool start)
	{
		throw null;
	}
}

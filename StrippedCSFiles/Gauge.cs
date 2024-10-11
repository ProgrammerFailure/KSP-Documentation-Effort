using System.Runtime.CompilerServices;
using UnityEngine;

public class Gauge : MonoBehaviour
{
	public float minRot;

	public float maxRot;

	public float maxValue;

	public float minValue;

	public float responsiveness;

	public float logarithmic;

	public Vector3 rotationAxis;

	public GaugeLEDRange[] ledRanges;

	public Transform pointer;

	public LED led;

	public float rawValue;

	private float currValue;

	private float tgtValue;

	private float tgtRot;

	private int currentRange;

	private float currentTime;

	public float Value
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Gauge()
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

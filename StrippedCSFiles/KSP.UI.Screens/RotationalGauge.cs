using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Screens;

public class RotationalGauge : MonoBehaviour
{
	[SerializeField]
	protected float currentAngle;

	public RectTransform pointer;

	public float minValue;

	public float maxValue;

	public float minRot;

	public float maxRot;

	public Vector3 rotationAxis;

	public float responsiveness;

	public float logarithmic;

	[SerializeField]
	protected float currentValue;

	[SerializeField]
	protected float targetValue;

	public float CurrentAngle
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float CurrentValue
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float Value
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
	public RotationalGauge()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetValue(double val)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetValue(float val)
	{
		throw null;
	}
}

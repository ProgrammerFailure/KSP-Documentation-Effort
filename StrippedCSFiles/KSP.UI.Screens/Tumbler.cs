using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Screens;

public class Tumbler : MonoBehaviour
{
	public enum TumbleDirection
	{
		Up,
		Down
	}

	[Serializable]
	public class TumblerObject
	{
		public Transform transform;

		public double tgtRot;

		public double currRot;

		public double N;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public TumblerObject(Transform transform)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void TumbleTo(double n, TumbleDirection tumble)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Update(float deltaTime, float sharpness)
		{
			throw null;
		}
	}

	public Transform[] tumblerTransforms;

	public uGUITumblerObject unitTumbler;

	public MeshRenderer[] tumblerRenderers;

	private TumblerObject[] tumblers;

	public float sharpness;

	private double currValue;

	private int lastUnit;

	private float lastUpdateTime;

	private float maxValue;

	private bool negative;

	private Color negativeColor;

	private Color positiveColor;

	public double Value
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Tumbler()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetValue(double val)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetColor(Color newColor)
	{
		throw null;
	}
}

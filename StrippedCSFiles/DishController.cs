using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DishController : MonoBehaviour
{
	[Serializable]
	public class Dish
	{
		public Transform elevationTransform;

		public Transform rotationTransform;

		[HideInInspector]
		public Quaternion elevationInit;

		[HideInInspector]
		public Quaternion rotationInit;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Dish()
		{
			throw null;
		}
	}

	[Serializable]
	private class TimeStep
	{
		public int time;

		public float setupTime;

		public float elevationStart;

		public float elevationEnd;

		public float rotationStart;

		public float rotationEnd;

		public Quaternion previousRotQ;

		public Quaternion startRotQ;

		public Quaternion endRotQ;

		public Quaternion previousElevQ;

		public Quaternion startElevQ;

		public Quaternion endElevQ;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public TimeStep()
		{
			throw null;
		}
	}

	public Dish[] dishes;

	public float minElevation;

	public float maxElevation;

	public float maxSpeed;

	public float targetChangetime;

	public float fakeTimeWarp;

	private int currentTimeStep;

	private TimeStep tsPrev;

	private TimeStep tsCur;

	private Quaternion currentRot;

	private Quaternion currentElev;

	private float delta;

	private float timeStepF;

	private float tsInterval;

	private int timeStep;

	private float time;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DishController()
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
	private void SetTimeStep(int timeStep)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private TimeStep CreateTimeStep(int time)
	{
		throw null;
	}
}

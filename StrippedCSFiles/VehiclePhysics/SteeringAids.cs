using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VehiclePhysics;

public class SteeringAids
{
	public enum HelpMode
	{
		Disabled,
		AssistedSteerAngle
	}

	public enum LimitMode
	{
		Disabled,
		Street,
		Sport,
		CustomSlip
	}

	public enum Priority
	{
		PreferDrifting,
		PreferGoStraight
	}

	[Serializable]
	public class Settings
	{
		public Priority priority;

		[Header("Steering Help")]
		public HelpMode helpMode;

		[Range(0f, 1f)]
		public float helpRatio;

		[Range(0f, 0.9f)]
		public float helpGravity;

		[Header("Steering Limit")]
		public LimitMode limitMode;

		[Range(0f, 1f)]
		public float limitRatio;

		[Range(0f, 1f)]
		public float limitProportionality;

		public float limitCustomSlip;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Settings()
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SteeringAids()
	{
		throw null;
	}
}

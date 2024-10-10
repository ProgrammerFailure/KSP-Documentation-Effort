using System;
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
		public Priority priority = Priority.PreferGoStraight;

		[Header("Steering Help")]
		public HelpMode helpMode;

		[Range(0f, 1f)]
		public float helpRatio = 1f;

		[Range(0f, 0.9f)]
		public float helpGravity = 0.4f;

		[Header("Steering Limit")]
		public LimitMode limitMode;

		[Range(0f, 1f)]
		public float limitRatio = 1f;

		[Range(0f, 1f)]
		public float limitProportionality = 0.9f;

		public float limitCustomSlip = 2f;
	}
}

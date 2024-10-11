using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VehiclePhysics;

public class SpeedControl
{
	[Serializable]
	public class Settings : ISerializationCallbackReceiver
	{
		public bool cruiseControl;

		public float cruiseSpeed;

		public float minSpeedForCC;

		[Space(5f)]
		public bool speedLimiter;

		public float speedLimit;

		[Range(0f, 3f)]
		[Space(5f)]
		public float throttleSlope;

		public float minSpeed;

		[NonSerialized]
		private const float minControlledSpeed = 1.388889f;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Settings()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void OnAfterDeserialize()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void OnBeforeSerialize()
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SpeedControl()
	{
		throw null;
	}
}

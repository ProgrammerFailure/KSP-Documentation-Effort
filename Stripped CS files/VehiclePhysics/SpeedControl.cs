using System;
using UnityEngine;

namespace VehiclePhysics;

public class SpeedControl
{
	[Serializable]
	public class Settings : ISerializationCallbackReceiver
	{
		public bool cruiseControl;

		public float cruiseSpeed = 27.777779f;

		public float minSpeedForCC = 4.166667f;

		[Space(5f)]
		public bool speedLimiter;

		public float speedLimit = 13.888889f;

		[Range(0f, 3f)]
		[Space(5f)]
		public float throttleSlope = 1f;

		public float minSpeed = 4.166667f;

		[NonSerialized]
		public const float minControlledSpeed = 1.388889f;

		public void OnAfterDeserialize()
		{
			if (minSpeedForCC < 1.388889f)
			{
				minSpeedForCC = 1.388889f;
			}
			if (cruiseSpeed < minSpeedForCC)
			{
				cruiseSpeed = minSpeedForCC;
			}
			if (speedLimit < 1.388889f)
			{
				speedLimit = 1.388889f;
			}
		}

		public void OnBeforeSerialize()
		{
		}
	}
}

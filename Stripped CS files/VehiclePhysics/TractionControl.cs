using System;
using UnityEngine;

namespace VehiclePhysics;

public class TractionControl
{
	public enum Mode
	{
		Street,
		Sport,
		CustomSlip
	}

	[Serializable]
	public class Settings
	{
		public bool enabled;

		public Mode mode;

		[Range(0f, 1f)]
		public float ratio = 1f;

		public float customSlip = 2f;
	}
}

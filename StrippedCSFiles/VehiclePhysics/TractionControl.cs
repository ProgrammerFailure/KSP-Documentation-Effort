using System;
using System.Runtime.CompilerServices;
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
		public float ratio;

		public float customSlip;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Settings()
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TractionControl()
	{
		throw null;
	}
}

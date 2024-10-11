using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace VehiclePhysics;

[CreateAssetMenu(fileName = "New Engine Data", menuName = "Vehicle Physics/Engine Data", order = 509)]
public class EngineDataAsset : ScriptableObject
{
	[Serializable]
	public class CurvePoint
	{
		public float rpm;

		public float torque;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public CurvePoint()
		{
			throw null;
		}
	}

	[Serializable]
	public class Specifications
	{
		public float maxTorque;

		public float maxTorqueRpm;

		public float maxPower;

		public float maxPowerRpm;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Specifications()
		{
			throw null;
		}
	}

	public string notes;

	[FormerlySerializedAs("specifications")]
	public Specifications engineSpecs;

	[FormerlySerializedAs("curveData")]
	public List<CurvePoint> engineCurve;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EngineDataAsset()
	{
		throw null;
	}
}

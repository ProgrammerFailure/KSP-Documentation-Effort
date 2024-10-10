using System;
using System.Collections.Generic;
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
	}

	[Serializable]
	public class Specifications
	{
		public float maxTorque;

		public float maxTorqueRpm;

		public float maxPower;

		public float maxPowerRpm;
	}

	public string notes;

	[FormerlySerializedAs("specifications")]
	public Specifications engineSpecs;

	[FormerlySerializedAs("curveData")]
	public List<CurvePoint> engineCurve;
}

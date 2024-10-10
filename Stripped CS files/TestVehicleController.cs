using System;
using UnityEngine;

public class TestVehicleController : MonoBehaviour
{
	[Serializable]
	public class Wheel
	{
		public KSPWheelController kwc;

		public float sprungMass;

		public float spring;

		public float damper;

		public float boost;

		public float loadScalarX;

		public float loadScalarY;

		public float Wx;

		public float Wy;
	}

	[SerializeField]
	public Wheel[] wheels;

	[SerializeField]
	public ConfigurableJoint[] joints;

	[SerializeField]
	public Rigidbody payload;

	public Transform Tref;

	[SerializeField]
	[Range(0.001f, 0.999999f)]
	public float massDistribution;

	[SerializeField]
	public float totalMass = 5f;

	[SerializeField]
	public float distributedMassSum;

	[SerializeField]
	public float TWLSum;

	[SerializeField]
	public float suspensionSpringRatio = 20f;

	[SerializeField]
	public float suspensionDampingRatio = 0.5f;

	[SerializeField]
	[Range(0.001f, 2f)]
	public float suspensionBoostRatio = 1f;

	public float WxSum;

	public float WySum;

	public float dMassError;

	public Vector3 CoM;

	[SerializeField]
	public float WheelMass;

	[SerializeField]
	public float PayloadMass;

	[SerializeField]
	public bool ApplyForcesOnParent;

	[SerializeField]
	public Vector3 GravityDirection = Vector3.down;

	[SerializeField]
	public float GravityMagnitude = 9.80665f;

	public KSPWheelGravity gravity;

	public float XYZspring = 10000000f;

	public float XYZmaxForce = float.MaxValue;

	public float angularSpring = 10000000f;

	public float angularMaxForce = float.MaxValue;

	public void Start()
	{
		Tref = base.transform;
		gravity.Value = GravityDirection * GravityMagnitude;
		updateMassDistribution(massDistribution);
	}

	public void FixedUpdate()
	{
		updateMassDistribution(massDistribution);
		Wheel[] array = wheels;
		foreach (Wheel wheel in array)
		{
			SuspensionSpringUpdate(wheel);
		}
		ConfigurableJoint[] array2 = joints;
		foreach (ConfigurableJoint j in array2)
		{
			ConfigurableJointUpdate(j);
		}
	}

	public void updateMassDistribution(float dist)
	{
		WheelMass = Mathf.Lerp(totalMass / (float)wheels.Length, 0f, dist);
		PayloadMass = Mathf.Min(totalMass - WheelMass * (float)wheels.Length, totalMass - 0.01f * (float)wheels.Length);
		Wheel[] array = wheels;
		foreach (Wheel wheel in array)
		{
			wheel.kwc.rb.mass = Mathf.Max(WheelMass, 0.01f);
			if (ApplyForcesOnParent && wheel.kwc.RbTgt == null)
			{
				wheel.kwc.SetRigidbodyTarget(payload);
			}
			if (!ApplyForcesOnParent && wheel.kwc.RbTgt != null)
			{
				wheel.kwc.SetRigidbodyTarget(null);
			}
		}
		payload.mass = PayloadMass;
		CoM = Mathfx.GetWeightedAvgVector3(wheels.Length, (int i) => wheels[i].kwc.rb.worldCenterOfMass, (int i) => wheels[i].kwc.rb.mass);
		DebugDrawUtil.DrawCrosshairs(CoM, 0.2f, Color.yellow, 0f);
		Debug.DrawRay(CoM, gravity.Value, Color.blue, 0f);
	}

	public void SuspensionLoadUpdate(Wheel[] wheels, float totalMass, Vector3 CoM, float Rmin)
	{
		totalMass = Mathf.Max(totalMass, Rmin);
		WxSum = 0f;
		WySum = 0f;
		Vector3 normalized = Vector3.Cross(Tref.right, gravity.Value).normalized;
		Vector3 normalized2 = Vector3.Cross(Tref.forward, gravity.Value).normalized;
		Wheel[] array = wheels;
		foreach (Wheel wheel in array)
		{
			wheel.Wy = DistanceFromCoM(wheel, CoM, normalized);
			WySum += wheel.Wy;
			wheel.Wx = DistanceFromCoM(wheel, CoM, normalized2);
			WxSum += wheel.Wx;
		}
		distributedMassSum = 0f;
		array = wheels;
		foreach (Wheel wheel2 in array)
		{
			wheel2.loadScalarX = 1f - wheel2.Wx / WxSum;
			wheel2.loadScalarY = 1f - wheel2.Wy / WySum;
			wheel2.sprungMass = totalMass * wheel2.loadScalarY * wheel2.loadScalarX;
			distributedMassSum += wheel2.sprungMass;
		}
		dMassError = totalMass / distributedMassSum;
		TWLSum = 0f;
		array = wheels;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].sprungMass *= dMassError;
		}
	}

	public float DistanceFromCoM(Wheel wheel, Vector3 CoM, Vector3 alongAxis)
	{
		DebugDrawUtil.DrawCrosshairs(wheel.kwc.wheelCollider.cachedTransform.position, 0.3f, Color.cyan, 0f);
		return Mathf.Abs(Vector3.Dot(wheel.kwc.wheelCollider.cachedTransform.position - CoM, alongAxis));
	}

	public void SuspensionSpringUpdate(Wheel wheel)
	{
		wheel.spring = Mathf.Clamp(suspensionSpringRatio * totalMass * BoostCurve(wheel.boost, suspensionBoostRatio), 1f, 1E+10f);
		wheel.damper = Mathf.Clamp(wheel.spring * suspensionDampingRatio, 0.01f, 1000f);
		wheel.kwc.wheelCollider.springRate = wheel.spring;
		wheel.kwc.wheelCollider.damperRate = wheel.damper;
	}

	public float BoostCurve(float b, float f)
	{
		return Mathf.Clamp(1f / Mathf.Abs(1f - 2f / Mathf.Pow(Mathf.Clamp(b, 0f, 2f), Mathf.Clamp(f, 0.01f, 1f))), 0.01f, 1E+10f);
	}

	public void ConfigurableJointUpdate(ConfigurableJoint j)
	{
		JointDrive xDrive = j.xDrive;
		xDrive.positionSpring = XYZspring;
		xDrive.maximumForce = XYZmaxForce;
		j.xDrive = xDrive;
		xDrive = j.yDrive;
		xDrive.positionSpring = XYZspring;
		xDrive.maximumForce = XYZmaxForce;
		j.yDrive = xDrive;
		xDrive = j.zDrive;
		xDrive.positionSpring = XYZspring;
		xDrive.maximumForce = XYZmaxForce;
		j.zDrive = xDrive;
		xDrive = j.angularXDrive;
		xDrive.positionSpring = angularSpring;
		xDrive.maximumForce = angularMaxForce;
		j.angularXDrive = xDrive;
		xDrive = j.angularYZDrive;
		xDrive.positionSpring = angularSpring;
		xDrive.maximumForce = angularMaxForce;
		j.angularYZDrive = xDrive;
	}
}

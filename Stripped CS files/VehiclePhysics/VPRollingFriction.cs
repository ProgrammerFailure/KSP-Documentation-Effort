using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Vehicle Dynamics/Rolling Friction", 41)]
public class VPRollingFriction : VehicleBehaviour
{
	public enum Model
	{
		Constant,
		Linear,
		Quadratic
	}

	public Model model;

	public float coefficient = 0.05f;

	public bool showDebugLabel;

	public float[] frictionFactors = new float[0];

	public override void FixedUpdateVehicle()
	{
		if (frictionFactors.Length != 0 && frictionFactors.Length != base.vehicle.wheelCount / 2)
		{
			DebugLogWarning("frictionMultiplers count doesn't match the vehicle's axle count. Component disabled.");
			base.enabled = false;
		}
		else if (frictionFactors.Length != 0)
		{
			int i = 0;
			for (int wheelCount = base.vehicle.wheelCount; i < wheelCount; i++)
			{
				ApplyRollingFriction(base.vehicle.wheelState[i], frictionFactors[i / 2]);
			}
		}
		else
		{
			int j = 0;
			for (int wheelCount2 = base.vehicle.wheelCount; j < wheelCount2; j++)
			{
				ApplyRollingFriction(base.vehicle.wheelState[j], 1f);
			}
		}
	}

	public void ApplyRollingFriction(VehicleBase.WheelState ws, float factor)
	{
		if (ws.grounded)
		{
			float num = 0f;
			switch (model)
			{
			case Model.Quadratic:
			{
				float num2 = ws.angularVelocity * ws.wheelCol.radius * 0.036f;
				num = 0.005f + coefficient * (0.01f + 0.0095f * num2 * num2);
				break;
			}
			default:
			{
				float num2 = Mathf.Abs(ws.angularVelocity * ws.wheelCol.radius);
				num = coefficient * Mathf.InverseLerp(0.01f, 1f, num2);
				break;
			}
			case Model.Linear:
			{
				float num2 = Mathf.Abs(ws.angularVelocity * ws.wheelCol.radius);
				num = coefficient * num2;
				break;
			}
			}
			float num3 = num * ws.downforce * factor;
			base.vehicle.cachedRigidbody.AddForceAtPosition((0f - num3) * ws.wheelVelocity.normalized, ws.hit.point);
		}
	}
}

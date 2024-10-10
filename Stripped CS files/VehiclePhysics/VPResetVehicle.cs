using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Utility/Reset Vehicle", 1)]
public class VPResetVehicle : VehicleBehaviour
{
	public KeyCode resetVehicleKey = KeyCode.Return;

	public float heightIncrement = 1.6f;

	public bool m_doResetVehicle;

	public override void OnEnableVehicle()
	{
		m_doResetVehicle = false;
	}

	public override void UpdateVehicle()
	{
		if (Input.GetKeyDown(resetVehicleKey))
		{
			m_doResetVehicle = true;
		}
	}

	public override void FixedUpdateVehicle()
	{
		if (m_doResetVehicle)
		{
			ResetVehicle(base.vehicle, heightIncrement);
			m_doResetVehicle = false;
		}
	}

	public void DoReset()
	{
		if (base.isActiveAndEnabled)
		{
			ResetVehicle(base.vehicle, heightIncrement);
		}
	}

	public static void ResetVehicle(VehicleBase vehicle, float height)
	{
		if (!(vehicle == null))
		{
			Vector3 localEulerAngles = vehicle.transform.localEulerAngles;
			Rigidbody cachedRigidbody = vehicle.cachedRigidbody;
			cachedRigidbody.MoveRotation(Quaternion.Euler(0f, localEulerAngles.y, 0f));
			cachedRigidbody.MovePosition(cachedRigidbody.position + Vector3.up * height);
			cachedRigidbody.velocity = Vector3.zero;
			cachedRigidbody.angularVelocity = Vector3.zero;
		}
	}
}

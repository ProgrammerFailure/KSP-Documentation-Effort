using UnityEngine;

namespace VehiclePhysics;

public abstract class VehicleBehaviour : MonoBehaviour
{
	public VehicleBase vehicle { get; set; }

	public VehicleBehaviour()
	{
	}

	public virtual void OnEnableVehicle()
	{
	}

	public virtual void OnDisableVehicle()
	{
	}

	public virtual void UpdateVehicle()
	{
	}

	public virtual void FixedUpdateVehicle()
	{
	}

	public virtual int GetUpdateOrder()
	{
		return 0;
	}

	public virtual void UpdateVehicleSuspension()
	{
	}

	public virtual void OnEnableComponent()
	{
	}

	public virtual void OnDisableComponent()
	{
	}

	public virtual void OnReposition()
	{
	}

	public virtual void OnEnterPause()
	{
	}

	public virtual void OnLeavePause()
	{
	}

	public void DebugLogWarning(string message)
	{
		Debug.LogWarning(ToString() + ": " + message + "\n", this);
	}

	public void DebugLogError(string message)
	{
		Debug.LogError(ToString() + ": " + message + "\n", this);
	}

	public void OnEnable()
	{
		vehicle = GetVehicle();
		if (vehicle == null)
		{
			DebugLogWarning("This component requires a VehicleBase-derived component. Component disabled.");
			base.enabled = false;
			return;
		}
		OnEnableComponent();
		if (vehicle != null)
		{
			vehicle.RegisterVehicleBehaviour(this);
		}
	}

	public virtual VehicleBase GetVehicle()
	{
		return GetComponentInParent<VehicleBase>();
	}

	public void OnDisable()
	{
		if (vehicle != null)
		{
			vehicle.UnregisterVehicleBehaviour(this);
			OnDisableComponent();
			vehicle = null;
		}
	}
}

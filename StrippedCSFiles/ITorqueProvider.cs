using UnityEngine;

public interface ITorqueProvider
{
	void GetPotentialTorque(out Vector3 pos, out Vector3 neg);
}

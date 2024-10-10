using UnityEngine;

public class LocalPhysXGravity : MonoBehaviour
{
	public Transform trf;

	public void Start()
	{
		trf = base.transform;
	}

	public void FixedUpdate()
	{
		Physics.gravity = FlightGlobals.getGeeForceAtPosition(trf.position, FlightGlobals.currentMainBody);
	}
}

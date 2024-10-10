using UnityEngine;

public class InternalLeverThrottle : InternalModule
{
	[KSPField]
	public string leverName = "throttleLever";

	[KSPField]
	public float angleMin = -1f;

	[KSPField]
	public float angleMax = -90f;

	[KSPField]
	public Vector3 axis = Vector3.right;

	[KSPField]
	public float speed = 1.5f;

	public Collider leverObject;

	public Quaternion leverInitial;

	public override void OnAwake()
	{
		if (leverObject == null)
		{
			leverObject = internalProp.FindModelTransform(leverName).GetComponent<Collider>();
		}
		if (leverObject != null)
		{
			InternalButton.Create(leverObject.gameObject).OnDrag(Lever_OnDrag);
			leverInitial = leverObject.transform.localRotation;
		}
	}

	public void Lever_OnDrag()
	{
		FlightInputHandler.state.mainThrottle += (Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")) * speed * Time.deltaTime;
		FlightInputHandler.state.mainThrottle = Mathf.Clamp01(FlightInputHandler.state.mainThrottle);
	}

	public override void OnUpdate()
	{
		leverObject.transform.localRotation = leverInitial * Quaternion.AngleAxis(Mathf.Lerp(angleMin, angleMax, FlightInputHandler.state.mainThrottle), axis);
	}
}

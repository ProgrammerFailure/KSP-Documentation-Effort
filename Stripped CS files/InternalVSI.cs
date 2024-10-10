using UnityEngine;

public class InternalVSI : InternalModule
{
	[KSPField]
	public string indicatorName = "indicator";

	[KSPField]
	public InternalDialIncrement increments = new InternalDialIncrement();

	[KSPField]
	public Vector3 axis = new Vector3(0f, 0f, 1f);

	[KSPField]
	public float smooth = 10f;

	public Transform hand;

	public Quaternion handInitial;

	public float vSpeed;

	public float current;

	public override void OnAwake()
	{
		if (hand == null)
		{
			hand = internalProp.FindModelTransform(indicatorName);
			handInitial = hand.transform.localRotation;
		}
	}

	public override void OnUpdate()
	{
		vSpeed = Mathf.Lerp(vSpeed, (float)FlightGlobals.ship_verticalSpeed, smooth * Time.deltaTime);
		current = increments.CalculateAngle(vSpeed);
		hand.transform.localRotation = handInitial * Quaternion.AngleAxis(current, axis);
	}
}

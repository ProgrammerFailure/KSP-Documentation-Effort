using UnityEngine;

public class InternalCompass : InternalModule
{
	[KSPField]
	public string indicatorName = "indicator";

	[KSPField]
	public Vector3 axis = new Vector3(0f, 0f, 1f);

	[KSPField]
	public float smooth = 10f;

	public Transform hand;

	public Quaternion handInitial;

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
		current = Mathf.LerpAngle(current, FlightGlobals.ship_heading, smooth * Time.deltaTime);
		hand.transform.localRotation = handInitial * Quaternion.AngleAxis(current, axis);
	}
}

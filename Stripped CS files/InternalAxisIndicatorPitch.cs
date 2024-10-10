using UnityEngine;

public class InternalAxisIndicatorPitch : InternalModule
{
	[KSPField]
	public string indicatorName = "indicator";

	[KSPField]
	public Vector3 min = new Vector3(-10f, 0f, 0f);

	[KSPField]
	public Vector3 max = new Vector3(10f, 0f, 0f);

	[KSPField]
	public float smooth = 10f;

	public Transform hand;

	public Vector3 mid;

	public float current;

	public override void OnAwake()
	{
		if (hand == null)
		{
			hand = internalProp.FindModelTransform(indicatorName);
		}
		mid = (max + min) / 2f;
	}

	public override void OnUpdate()
	{
		current = Mathf.Lerp(current, FlightInputHandler.state.pitch, smooth * Time.deltaTime);
		if (current < 0f)
		{
			hand.transform.localPosition = Vector3.Lerp(mid, min, 0f - current);
		}
		else
		{
			hand.transform.localPosition = Vector3.Lerp(mid, max, current);
		}
	}
}

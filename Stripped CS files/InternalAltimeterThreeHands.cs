using UnityEngine;

public class InternalAltimeterThreeHands : InternalModule
{
	[KSPField]
	public string hand100Name = "hand100";

	[KSPField]
	public string hand1000Name = "hand1000";

	[KSPField]
	public string hand10000Name = "hand10000";

	[KSPField]
	public Vector3 handAxis = Vector3.forward;

	[KSPField]
	public float smoothing = 10f;

	public Transform hand100;

	public Quaternion hand100Initial;

	public Transform hand1000;

	public Quaternion hand1000Initial;

	public Transform hand10000;

	public Quaternion hand10000Initial;

	public float altitude;

	public float hundreds;

	public float thousands;

	public float tenThousands;

	public override void OnAwake()
	{
		if (hand100 == null)
		{
			hand100 = internalProp.FindModelTransform(hand100Name);
			hand100Initial = hand100.transform.localRotation;
		}
		if (hand1000 == null)
		{
			hand1000 = internalProp.FindModelTransform(hand1000Name);
			hand1000Initial = hand100.transform.localRotation;
		}
		if (hand10000 == null)
		{
			hand10000 = internalProp.FindModelTransform(hand10000Name);
			hand10000Initial = hand10000.transform.localRotation;
		}
	}

	public override void OnUpdate()
	{
		altitude = Mathf.Lerp(altitude, (float)base.vessel.altitude, smoothing * Time.deltaTime);
		if (altitude == 0f)
		{
			hundreds = 0f;
			thousands = 0f;
			tenThousands = 0f;
		}
		else
		{
			hundreds = altitude / 1000f;
			thousands = altitude / 10000f;
			tenThousands = altitude / 100000f;
		}
		hand100.transform.localRotation = hand100Initial * Quaternion.AngleAxis(hundreds * 360f, handAxis);
		hand1000.transform.localRotation = hand1000Initial * Quaternion.AngleAxis(thousands * 360f, handAxis);
		hand10000.transform.localRotation = hand10000Initial * Quaternion.AngleAxis(tenThousands * 360f, handAxis);
	}
}

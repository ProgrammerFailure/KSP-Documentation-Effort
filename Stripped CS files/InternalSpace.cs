using UnityEngine;

public class InternalSpace : MonoBehaviour
{
	public static InternalSpace Instance;

	public void Awake()
	{
		Instance = this;
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public static Vector3 InternalToWorld(Vector3 internalSpacePosition)
	{
		return InternalToWorld(Quaternion.identity) * (internalSpacePosition - Instance.transform.position) + FlightGlobals.ActiveVessel.transform.position;
	}

	public static Quaternion InternalToWorld(Quaternion internalSpaceRotation)
	{
		return FlightGlobals.ActiveVessel.transform.rotation * internalSpaceRotation;
	}

	public static Vector3 WorldToInternal(Vector3 worldSpacePosition)
	{
		return WorldToInternal(Quaternion.identity) * (worldSpacePosition - FlightGlobals.ActiveVessel.transform.position) + Instance.transform.position;
	}

	public static Quaternion WorldToInternal(Quaternion worldSpaceRotation)
	{
		return Quaternion.Inverse(InternalToWorld(Quaternion.identity)) * worldSpaceRotation;
	}
}

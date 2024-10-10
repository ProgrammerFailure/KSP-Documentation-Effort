using UnityEngine;

public class IVASun : MonoBehaviour
{
	public Transform sunT;

	public Light ivaLight;

	public Light lclLight;

	public float intensityScalar = 0.9f;

	public void Start()
	{
		ivaLight = GetComponent<Light>();
		if (sunT == null)
		{
			sunT = Sun.Instance.transform;
		}
		if (sunT != null)
		{
			lclLight = sunT.GetComponent<Light>();
		}
	}

	public void LateUpdate()
	{
		if (!(FlightGlobals.ActiveVessel == null))
		{
			base.transform.rotation = InternalSpace.WorldToInternal(sunT.rotation);
			if ((bool)ivaLight && (bool)lclLight)
			{
				ivaLight.intensity = lclLight.intensity * intensityScalar;
			}
		}
	}
}

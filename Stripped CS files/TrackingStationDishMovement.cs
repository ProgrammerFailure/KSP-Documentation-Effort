using UnityEngine;

public class TrackingStationDishMovement : MonoBehaviour
{
	public bool useUniversalTime;

	public float rotSpd = 1f;

	public Transform dishAzimuthPivot;

	public Transform dishElevationPivot;

	public void Update()
	{
		if ((bool)dishAzimuthPivot && (bool)dishElevationPivot)
		{
			if (useUniversalTime && Planetarium.fetch != null)
			{
				dishAzimuthPivot.localRotation = Quaternion.AngleAxis((float)(Planetarium.GetUniversalTime() * (double)rotSpd), Vector3.up);
			}
			else
			{
				dishAzimuthPivot.localRotation = Quaternion.AngleAxis(Time.time * rotSpd, Vector3.up);
			}
		}
	}
}

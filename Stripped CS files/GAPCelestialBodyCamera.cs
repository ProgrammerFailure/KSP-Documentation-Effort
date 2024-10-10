using Expansions.Missions.Editor;
using UnityEngine;

public class GAPCelestialBodyCamera : MonoBehaviour
{
	public Camera cam;

	public Camera camMapFX;

	public Transform pqsTarget;

	public Material GAPCelestialBodyMaterial;

	public AnimationCurve curveZoomLocal;

	public AnimationCurve curveZoomScaled;

	public AnimationCurve curveDragLocal;

	public AnimationCurve curveDragScaled;

	public double rotationSpeed;

	public double smooth;

	public double distanceMin;

	public double distanceMax;

	public bool customBoundries;

	public double x = 90.0;

	public double y = 15.0;

	public double distanceCurrent;

	public double distancePercentage;

	public QuaternionD smoothedQ;

	public Vector3d smoothPos;

	public Transform focusTarget;

	public Vector3 focusPoint;

	public Vector3 newFocusPoint;

	public CelestialBody celestialBody;

	public CelestialBody sunCelestialBody;

	public bool isActive;

	public bool lockDragging;

	public bool pqsActive;

	public double minMultiplierLocal = 1.0;

	public double maxMultiplierLocal = 2.5;

	public double minMultiplierScaled = 1.75;

	public double maxMultiplierScaled = 2.5;

	public double heightOffset;

	public float zoomValue;

	public AnimationCurve curveDrag;

	public AnimationCurve curveZoom;

	public bool LockDragging
	{
		get
		{
			return lockDragging;
		}
		set
		{
			lockDragging = value;
		}
	}

	public double ZoomValue => zoomValue;

	public double DistanceToSurface => (double)zoomValue * distanceMax;

	public Transform FocusTarget
	{
		get
		{
			return focusTarget;
		}
		set
		{
			focusTarget = value;
			focusPoint = focusTarget.transform.position;
			if (focusTarget != celestialBody)
			{
				double num = Vector3d.Distance(focusTarget.transform.position, celestialBody.transform.position);
				smoothPos = new Vector3d(0.0, 0.0, 0.0 - distanceCurrent - heightOffset + num);
				base.transform.position = smoothedQ * smoothPos + focusPoint;
			}
			newFocusPoint = focusTarget.transform.position;
		}
	}

	public void Enable(CelestialBody newCelestialBody, bool usePQS)
	{
		smooth = 25.0;
		rotationSpeed = 120.0;
		if (camMapFX == null)
		{
			camMapFX = MissionEditorMapView.CreateMapFXCamera(1, 0.1f, 1f);
			camMapFX.transform.SetParent(cam.transform, worldPositionStays: false);
		}
		cam.fieldOfView = 60f;
		camMapFX.fieldOfView = 60f;
		if (focusTarget == celestialBody || focusTarget == null)
		{
			focusTarget = newCelestialBody.transform;
		}
		celestialBody = newCelestialBody;
		pqsActive = newCelestialBody.hasSolidSurface && usePQS;
		heightOffset = celestialBody.Radius * 0.0010000000474974513;
		if (pqsActive)
		{
			curveDrag = curveDragLocal;
			curveZoom = curveZoomLocal;
			if (!customBoundries)
			{
				distanceMin = celestialBody.Radius * minMultiplierLocal;
				distanceMax = celestialBody.Radius * maxMultiplierLocal;
			}
		}
		else
		{
			curveDrag = curveDragScaled;
			curveZoom = curveZoomScaled;
			if (!customBoundries)
			{
				distanceMin = celestialBody.Radius * minMultiplierScaled;
				distanceMax = celestialBody.Radius * maxMultiplierScaled;
				cam.nearClipPlane = (float)(newCelestialBody.Radius * 0.5);
				cam.farClipPlane = (float)(newCelestialBody.Radius * 3.5);
			}
		}
		pqsTarget.gameObject.SetActive(pqsActive);
		distanceCurrent = celestialBody.Radius + distanceMax;
		smoothedQ = Quaternion.Euler((float)y, (float)x, 0f);
		smoothPos = new Vector3d(0.0, 0.0, 0.0 - distanceCurrent);
		sunCelestialBody = FlightGlobals.GetBodyByName("Sun");
		Sun.Instance.SunlightEnabled(state: true);
		Sun.Instance.sunFlare.enabled = sunCelestialBody != newCelestialBody;
		Sun.Instance.enabled = false;
		lockDragging = false;
		isActive = true;
	}

	public void Disable()
	{
		isActive = false;
	}

	public void UpdateMouse(float xAxis, float yAxis)
	{
		if (!lockDragging && Input.GetMouseButton(1))
		{
			float num = (float)(Vector3d.Distance(focusPoint, celestialBody.transform.position) / celestialBody.Radius);
			double num2 = curveDrag.Evaluate((float)distancePercentage + num);
			double num3 = rotationSpeed * num2;
			x += num3 * (double)xAxis * (double)Time.deltaTime;
			y -= num3 * (double)yAxis * (double)Time.deltaTime;
			if (y > 90.0)
			{
				y = 90.0;
			}
			else if (y < -90.0)
			{
				y = -90.0;
			}
		}
	}

	public void UpdateTransform(double surfaceHeight)
	{
		if (isActive)
		{
			focusPoint = Vector3.Lerp(focusPoint, newFocusPoint, (float)(smooth * (double)Time.deltaTime * 0.5));
			double num = Vector3d.Distance(focusPoint, celestialBody.transform.position);
			smoothedQ = Quaternion.Lerp(smoothedQ, Quaternion.Euler((float)y, (float)x, 0f), (float)(smooth * (double)Time.deltaTime));
			smoothPos = Vector3d.Lerp(smoothPos, new Vector3d(0.0, 0.0, 0.0 - distanceCurrent - (surfaceHeight + heightOffset) + num), (float)(smooth * (double)Time.deltaTime));
			base.transform.rotation = smoothedQ;
			base.transform.position = smoothedQ * smoothPos + focusPoint;
			distancePercentage = (distanceCurrent - distanceMin) / distanceMax;
			if (pqsActive)
			{
				cam.nearClipPlane = Mathf.Clamp((float)((distancePercentage - 0.025) * distanceMin), 0f, (float)distanceMax) + 10000f * (float)distancePercentage + 200f;
				cam.farClipPlane = (float)((distancePercentage + 0.5) * distanceMax);
				camMapFX.nearClipPlane = cam.nearClipPlane;
				camMapFX.farClipPlane = cam.farClipPlane + (float)celestialBody.Radius + (float)celestialBody.Radius * 0.75f;
			}
		}
	}

	public Camera GetLocalSpaceCamera()
	{
		return cam;
	}

	public void SetBoundries(double minMultiplier, double maxMultiplier)
	{
		distanceMin = celestialBody.Radius * minMultiplier;
		distanceMax = celestialBody.Radius * maxMultiplier;
		cam.farClipPlane = (float)distanceMax;
		customBoundries = true;
	}

	public void OverridePosition(double latitude, double longitude)
	{
		x = 0.0 - longitude - 90.0;
		y = latitude;
		smoothedQ = Quaternion.Euler((float)y, (float)x, 0f);
		base.transform.rotation = Quaternion.Euler((float)y, (float)x, 0f);
	}

	public void SetPosition(double latitude, double longitude)
	{
		x = 0.0 - longitude - 90.0;
		y = latitude;
	}

	public void SetLightCycle(double double_0)
	{
		if (celestialBody != sunCelestialBody)
		{
			Sun.Instance.transform.localRotation = GetLightRot(double_0, celestialBody, celestialBody.referenceBody);
		}
	}

	public Quaternion GetLightRot(double double_0, CelestialBody userCB, CelestialBody referenceCB)
	{
		Vector3d positionAtUT = referenceCB.getPositionAtUT(double_0);
		Vector3d positionAtUT2 = userCB.getPositionAtUT(double_0);
		Vector3d positionAtUT3 = sunCelestialBody.getPositionAtUT(double_0);
		Vector3d positionAtUT4 = referenceCB.getPositionAtUT(0.0);
		Vector3d vector3d = userCB.getPositionAtUT(0.0) - positionAtUT4;
		Vector3d vector3d2 = positionAtUT2 - positionAtUT4 + positionAtUT;
		Vector3d vector3d3 = vector3d2 - positionAtUT3;
		Quaternion quaternion = Quaternion.Inverse(Quaternion.LookRotation(vector3d));
		Quaternion quaternion2 = Quaternion.LookRotation(vector3d3) * Quaternion.Euler(new Vector3d(0.0, userCB.directRotAngle, 0.0));
		if (userCB.tidallyLocked)
		{
			Quaternion quaternion3 = Quaternion.LookRotation(vector3d2 - positionAtUT);
			return quaternion2 * Quaternion.Inverse(quaternion * quaternion3);
		}
		double num = double_0 / celestialBody.rotationPeriod * 360.0;
		return quaternion2 * Quaternion.Euler(new Vector3d(0.0, num, 0.0));
	}

	public void RefreshTarget()
	{
		if (focusTarget != celestialBody)
		{
			newFocusPoint = focusTarget.transform.position;
		}
	}

	public void OverrideDistance(double percentage)
	{
		zoomValue = curveZoom.Evaluate(1f - (float)percentage);
		distanceCurrent = distanceMin + distanceMax * (double)zoomValue;
	}

	public void Destroy()
	{
		Sun.Instance.enabled = true;
		Sun.Instance.sunFlare.enabled = false;
		Sun.Instance.SunlightEnabled(state: false);
		Object.Destroy(base.gameObject);
	}
}

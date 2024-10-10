using System.Collections;
using UnityEngine;

public class SurfaceObject : MonoBehaviour
{
	public double latitude;

	public double longitude;

	public double altitude;

	public bool GrabCoordsAtStart;

	public bool PopToSceneRootAtStart;

	public CelestialBody cb;

	public KFSMUpdateMode updateMode;

	public Vector3d srfNVector;

	public Transform trf;

	public Transform originalParent;

	public int initDelay;

	public bool started;

	public bool popped;

	public Transform[] children;

	public Vector3[] pristineChildPositions;

	public Quaternion[] pristineChildRotations;

	public Vector3[] pristineChildLocalScales;

	public bool IsPopped => popped;

	public IEnumerator Start()
	{
		for (int i = 0; i < initDelay; i++)
		{
			yield return null;
		}
		Vector3d worldPos = trf.position;
		if (cb == null)
		{
			cb = base.gameObject.GetComponentUpwards<CelestialBody>();
		}
		if (GrabCoordsAtStart)
		{
			cb.GetLatLonAlt(worldPos, out latitude, out longitude, out altitude);
		}
		if (PopToSceneRootAtStart)
		{
			PopToSceneRoot();
		}
		started = true;
	}

	public void onUpdate()
	{
		srfNVector = cb.GetSurfaceNVector(latitude, longitude);
		trf.position = cb.position + srfNVector * (cb.Radius + altitude);
	}

	public void PopToSceneRoot()
	{
		if (popped)
		{
			return;
		}
		children = trf.GetComponentsInChildren<Transform>(includeInactive: true);
		pristineChildPositions = new Vector3[children.Length];
		pristineChildRotations = new Quaternion[children.Length];
		pristineChildLocalScales = new Vector3[children.Length];
		int num = children.Length;
		while (num-- > 0)
		{
			Transform transform = children[num];
			if (!(transform == trf))
			{
				pristineChildPositions[num] = transform.localPosition;
				pristineChildRotations[num] = transform.localRotation;
				pristineChildLocalScales[num] = transform.localScale;
			}
		}
		originalParent = trf.parent;
		trf.parent = null;
		int num2 = children.Length;
		while (num2-- > 0)
		{
			Transform transform = children[num2];
			if (!(transform == trf))
			{
				transform.localPosition = pristineChildPositions[num2];
				transform.localRotation = pristineChildRotations[num2];
				transform.localScale = pristineChildLocalScales[num2];
			}
		}
		popped = true;
	}

	public void ReturnToParent()
	{
		if (originalParent != null)
		{
			if (!popped)
			{
				return;
			}
			trf.parent = originalParent;
			popped = false;
			int num = children.Length;
			while (num-- > 0)
			{
				Transform transform = children[num];
				if (!(transform == trf))
				{
					transform.localPosition = pristineChildPositions[num];
					transform.localRotation = pristineChildRotations[num];
					transform.localScale = pristineChildLocalScales[num];
				}
			}
		}
		else
		{
			Debug.LogError("[SurfaceObject]: Cannot return to original parent, it no longer exists.", base.gameObject);
		}
	}

	public void FixedUpdate()
	{
		if (updateMode == KFSMUpdateMode.FIXEDUPDATE && started)
		{
			onUpdate();
		}
	}

	public void Update()
	{
		if (updateMode == KFSMUpdateMode.UPDATE && started)
		{
			onUpdate();
		}
	}

	public void LateUpdate()
	{
		if (updateMode == KFSMUpdateMode.LATEUPDATE && started)
		{
			onUpdate();
		}
	}

	public static SurfaceObject Create(GameObject host, CelestialBody body, int initDelay, KFSMUpdateMode updateMode)
	{
		SurfaceObject surfaceObject = host.AddComponent<SurfaceObject>();
		surfaceObject.cb = body;
		surfaceObject.initDelay = initDelay;
		surfaceObject.GrabCoordsAtStart = true;
		surfaceObject.PopToSceneRootAtStart = true;
		surfaceObject.updateMode = updateMode;
		surfaceObject.trf = surfaceObject.transform;
		return surfaceObject;
	}

	public static SurfaceObject Create(GameObject host, CelestialBody body, double latitude, double longitude, double altitude, int initDelay, KFSMUpdateMode updateMode)
	{
		SurfaceObject surfaceObject = host.AddComponent<SurfaceObject>();
		surfaceObject.cb = body;
		surfaceObject.initDelay = initDelay;
		surfaceObject.GrabCoordsAtStart = false;
		surfaceObject.PopToSceneRootAtStart = true;
		surfaceObject.updateMode = updateMode;
		surfaceObject.latitude = latitude;
		surfaceObject.longitude = longitude;
		surfaceObject.altitude = altitude;
		surfaceObject.trf = surfaceObject.transform;
		return surfaceObject;
	}

	public static SurfaceObject Create(GameObject host, CelestialBody body, double latitude, double longitude, double altitude, bool grabCoordsAtStart, bool popToSceneRootAtStart, int initDelay, KFSMUpdateMode updateMode)
	{
		SurfaceObject surfaceObject = host.AddComponent<SurfaceObject>();
		surfaceObject.cb = body;
		surfaceObject.initDelay = initDelay;
		surfaceObject.GrabCoordsAtStart = grabCoordsAtStart;
		surfaceObject.PopToSceneRootAtStart = popToSceneRootAtStart;
		surfaceObject.updateMode = updateMode;
		surfaceObject.latitude = latitude;
		surfaceObject.longitude = longitude;
		surfaceObject.altitude = altitude;
		surfaceObject.trf = surfaceObject.transform;
		return surfaceObject;
	}

	public void Terminate()
	{
		ReturnToParent();
		Object.Destroy(this);
	}
}

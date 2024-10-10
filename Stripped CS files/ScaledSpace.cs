using System.Collections.Generic;
using UnityEngine;

public class ScaledSpace : MonoBehaviour
{
	public float scaleFactor = 6000f;

	public Transform originTarget;

	public List<MapObject> scaledSpaceObjects = new List<MapObject>();

	public static Vector3d totalOffset;

	public static ScaledSpace Instance { get; set; }

	public static float ScaleFactor
	{
		get
		{
			if (!Instance)
			{
				return 1f;
			}
			return Instance.scaleFactor;
		}
	}

	public static float InverseScaleFactor
	{
		get
		{
			if (!Instance)
			{
				return 1f;
			}
			return 1f / Instance.scaleFactor;
		}
	}

	public static Transform SceneTransform
	{
		get
		{
			if (!Instance)
			{
				return null;
			}
			return Instance.transform;
		}
	}

	public void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Object.Destroy(this);
			return;
		}
		Instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void Start()
	{
		_ = (bool)originTarget;
	}

	public void LateUpdate()
	{
		if (!originTarget)
		{
			return;
		}
		Vector3d vector3d = originTarget.position;
		totalOffset += vector3d;
		int count = scaledSpaceObjects.Count;
		while (count-- > 0)
		{
			if (scaledSpaceObjects[count] == null)
			{
				scaledSpaceObjects.RemoveAt(count);
				continue;
			}
			Transform obj = scaledSpaceObjects[count].transform;
			obj.position -= vector3d;
		}
	}

	public static void AddScaledSpaceObject(MapObject t)
	{
		if (!(Instance == null) && Instance.scaledSpaceObjects != null)
		{
			if (Instance.scaledSpaceObjects.Contains(t))
			{
				Debug.LogWarning("Warning, MapObject " + t.name + " already exists in scaled space", t);
			}
			else
			{
				Instance.scaledSpaceObjects.Add(t);
			}
		}
	}

	public static void RemoveScaledSpaceObject(MapObject t)
	{
		if (!(Instance == null) && Instance.scaledSpaceObjects != null)
		{
			Instance.scaledSpaceObjects.Remove(t);
		}
	}

	public static Vector3d LocalToScaledSpace(Vector3d localSpacePoint)
	{
		return localSpacePoint * InverseScaleFactor - totalOffset;
	}

	public static void LocalToScaledSpace(ref Vector3d localSpacePoint)
	{
		localSpacePoint = localSpacePoint * InverseScaleFactor - totalOffset;
	}

	public static void LocalToScaledSpace(List<Vector3> points)
	{
		int count = points.Count;
		while (count-- > 0)
		{
			points[count] = points[count] * InverseScaleFactor - totalOffset;
		}
	}

	public static void LocalToScaledSpace(Vector3d[] localSpacePoint, List<Vector3> scaledSpacePoint)
	{
		int num = localSpacePoint.Length;
		double num2 = InverseScaleFactor;
		for (int i = 0; i < num; i++)
		{
			scaledSpacePoint[i] = localSpacePoint[i] * num2 - totalOffset;
		}
	}

	public static Vector3d ScaledToLocalSpace(Vector3d scaledSpacePoint)
	{
		return (scaledSpacePoint + totalOffset) * ScaleFactor;
	}

	public static void ToggleAll(bool toggleValue)
	{
		for (int i = 0; i < Instance.scaledSpaceObjects.Count; i++)
		{
			Instance.scaledSpaceObjects[i].gameObject.SetActive(toggleValue);
		}
	}

	public static void Toggle(CelestialBody celestialBody, bool toggleValue)
	{
		GameObject gameObject = null;
		for (int i = 0; i < Instance.scaledSpaceObjects.Count; i++)
		{
			if (Instance.scaledSpaceObjects[i].celestialBody == celestialBody)
			{
				gameObject = Instance.scaledSpaceObjects[i].gameObject;
				break;
			}
		}
		if (gameObject != null)
		{
			gameObject.SetActive(toggleValue);
		}
	}
}

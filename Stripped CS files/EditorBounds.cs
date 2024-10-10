using UnityEngine;

public class EditorBounds : MonoBehaviour
{
	public static EditorBounds Instance;

	public Bounds constructionBounds;

	public float cameraStartDistance = 30f;

	public float cameraMaxDistance = 35f;

	public float cameraMinDistance = 3f;

	public Bounds cameraOffsetBounds;

	public Vector3 rootPartSpawnPoint;

	public Transform sceneryCenter;

	[SerializeField]
	public Color constructionBoundsColor = Color.white;

	[SerializeField]
	public Color cameraOffsetBoundsColor = Color.white;

	[SerializeField]
	public Color spawnPointColor = Color.white;

	public void Awake()
	{
		if ((bool)Instance)
		{
			Object.Destroy(this);
		}
		else
		{
			Instance = this;
		}
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.color = constructionBoundsColor;
		Gizmos.DrawCube(constructionBounds.center, constructionBounds.size);
		GizmoDrawUtil.DrawCrosshairs(sceneryCenter.position, 2f, Color.cyan);
		GizmoDrawUtil.DrawCrosshairs(rootPartSpawnPoint, 1f, spawnPointColor);
		GizmoDrawUtil.DrawReach(cameraOffsetBounds, cameraMaxDistance, cameraOffsetBoundsColor);
	}

	public static Vector3 ClampToCameraBounds(Vector3 pos, Vector3 camFwd, ref float clampHeight)
	{
		if (Instance != null)
		{
			pos.x = Mathf.Clamp(pos.x, 0f - Instance.cameraOffsetBounds.extents.x, Instance.cameraOffsetBounds.extents.x);
			pos.z = Mathf.Clamp(pos.z, 0f - Instance.cameraOffsetBounds.extents.z, Instance.cameraOffsetBounds.extents.z);
			float num = Instance.cameraMaxDistance * Mathf.Clamp01(1f - Vector3.Dot(camFwd, Vector3.up));
			float num2 = Instance.cameraMaxDistance * Mathf.Clamp01(1f - Vector3.Dot(camFwd, Vector3.down));
			if (pos.y < Instance.cameraOffsetBounds.center.y - Instance.cameraOffsetBounds.extents.y - num)
			{
				clampHeight -= pos.y - (Instance.cameraOffsetBounds.center.y - Instance.cameraOffsetBounds.extents.y - num);
			}
			if (pos.y > Instance.cameraOffsetBounds.center.y + Instance.cameraOffsetBounds.extents.y + num2)
			{
				clampHeight -= pos.y - (Instance.cameraOffsetBounds.center.y + Instance.cameraOffsetBounds.extents.y + num2);
			}
			pos.y = Mathf.Clamp(pos.y, Instance.cameraOffsetBounds.center.y - Instance.cameraOffsetBounds.extents.y - num, Instance.cameraOffsetBounds.center.y + Instance.cameraOffsetBounds.extents.y + num2);
		}
		return pos;
	}

	public static float ClampCameraDistance(float dist)
	{
		if (Instance != null)
		{
			dist = Mathf.Clamp(dist, Instance.cameraMinDistance, Instance.cameraMaxDistance);
		}
		return dist;
	}

	public static void CenterSceneryOrigin(Transform sceneryRoot)
	{
		if (Instance != null)
		{
			Instance.centerSceneryOrigin(sceneryRoot);
		}
	}

	public void centerSceneryOrigin(Transform sceneryRoot)
	{
		sceneryRoot.position = sceneryCenter.position;
	}

	public void SetExtents(Vector3 extents)
	{
		cameraOffsetBounds.extents = extents;
	}
}

using System.Runtime.CompilerServices;
using UnityEngine;

public class EditorBounds : MonoBehaviour
{
	public static EditorBounds Instance;

	public Bounds constructionBounds;

	public float cameraStartDistance;

	public float cameraMaxDistance;

	public float cameraMinDistance;

	public Bounds cameraOffsetBounds;

	public Vector3 rootPartSpawnPoint;

	public Transform sceneryCenter;

	[SerializeField]
	private Color constructionBoundsColor;

	[SerializeField]
	private Color cameraOffsetBoundsColor;

	[SerializeField]
	private Color spawnPointColor;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EditorBounds()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDrawGizmosSelected()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 ClampToCameraBounds(Vector3 pos, Vector3 camFwd, ref float clampHeight)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float ClampCameraDistance(float dist)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CenterSceneryOrigin(Transform sceneryRoot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void centerSceneryOrigin(Transform sceneryRoot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetExtents(Vector3 extents)
	{
		throw null;
	}
}

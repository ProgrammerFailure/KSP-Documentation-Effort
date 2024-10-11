using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteInEditMode]
public class CameraOffCenter : MonoBehaviour
{
	public float x;

	public float y;

	private Camera _camera;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CameraOffCenter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Matrix4x4 PerspectiveOffCenter(float x, float y, float near, float far, float fov, float aspect)
	{
		throw null;
	}
}

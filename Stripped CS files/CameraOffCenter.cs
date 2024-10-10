using System;
using UnityEngine;

[ExecuteInEditMode]
public class CameraOffCenter : MonoBehaviour
{
	public float x;

	public float y;

	public Camera _camera;

	public void LateUpdate()
	{
		float aspect = (float)Screen.width / (float)Screen.height;
		Matrix4x4 projectionMatrix = PerspectiveOffCenter(x, y, this.GetComponentCached(ref _camera).nearClipPlane, this.GetComponentCached(ref _camera).farClipPlane, this.GetComponentCached(ref _camera).fieldOfView, aspect);
		this.GetComponentCached(ref _camera).projectionMatrix = projectionMatrix;
	}

	public static Matrix4x4 PerspectiveOffCenter(float x, float y, float near, float far, float fov, float aspect)
	{
		float num = near * Mathf.Tan(fov * 0.5f * ((float)Math.PI / 180f));
		float num2 = 0f - num;
		float num3 = 0f - num;
		float num4 = num - num3;
		float num5 = num - num2;
		float num6 = 2f * near / num4;
		num6 /= aspect;
		float value = 2f * near / num5;
		float value2 = (0f - (far + near)) / (far - near);
		float value3 = (0f - 2f * far * near) / (far - near);
		float value4 = -1f;
		Matrix4x4 result = default(Matrix4x4);
		result[0, 0] = num6;
		result[0, 1] = 0f;
		result[0, 2] = 0f - x;
		result[0, 3] = 0f;
		result[1, 0] = 0f;
		result[1, 1] = value;
		result[1, 2] = 0f - y;
		result[1, 3] = 0f;
		result[2, 0] = 0f;
		result[2, 1] = 0f;
		result[2, 2] = value2;
		result[2, 3] = value3;
		result[3, 0] = 0f;
		result[3, 1] = 0f;
		result[3, 2] = value4;
		result[3, 3] = 0f;
		return result;
	}
}

using System;
using UnityEngine;

namespace EdyCommonTools;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class CameraShift : MonoBehaviour
{
	public Vector2 offset = Vector2.zero;

	public Camera m_camera;

	public void OnEnable()
	{
		m_camera = GetComponent<Camera>();
	}

	public void OnDisable()
	{
		m_camera.ResetProjectionMatrix();
	}

	public void LateUpdate()
	{
		SetPerspectiveOffset(m_camera, offset);
	}

	public static void SetPerspectiveOffset(Camera cam, Vector2 perspectiveOffset)
	{
		float num = Mathf.Tan(cam.fieldOfView * 0.5f * ((float)Math.PI / 180f));
		float num2 = num * cam.aspect;
		float nearClipPlane = cam.nearClipPlane;
		float num3 = nearClipPlane * num;
		float num4 = nearClipPlane * num2;
		float num5 = perspectiveOffset.x * num4;
		float num6 = perspectiveOffset.y * num3;
		float left = 0f - num4 - num5;
		float right = num4 - num5;
		float top = num3 - num6;
		float bottom = 0f - num3 - num6;
		cam.projectionMatrix = PerspectiveOffCenter(left, right, bottom, top, cam.nearClipPlane, cam.farClipPlane);
	}

	public static Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
	{
		float value = 2f * near / (right - left);
		float value2 = 2f * near / (top - bottom);
		float value3 = (right + left) / (right - left);
		float value4 = (top + bottom) / (top - bottom);
		float value5 = (0f - (far + near)) / (far - near);
		float value6 = (0f - 2f * far * near) / (far - near);
		float value7 = -1f;
		Matrix4x4 result = default(Matrix4x4);
		result[0, 0] = value;
		result[0, 1] = 0f;
		result[0, 2] = value3;
		result[0, 3] = 0f;
		result[1, 0] = 0f;
		result[1, 1] = value2;
		result[1, 2] = value4;
		result[1, 3] = 0f;
		result[2, 0] = 0f;
		result[2, 1] = 0f;
		result[2, 2] = value5;
		result[2, 3] = value6;
		result[3, 0] = 0f;
		result[3, 1] = 0f;
		result[3, 2] = value7;
		result[3, 3] = 0f;
		return result;
	}
}

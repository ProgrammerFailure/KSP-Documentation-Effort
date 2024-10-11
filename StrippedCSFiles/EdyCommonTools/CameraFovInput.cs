using System.Runtime.CompilerServices;
using UnityEngine;

namespace EdyCommonTools;

[RequireComponent(typeof(CameraFovController))]
public class CameraFovInput : MonoBehaviour
{
	public enum InputSource
	{
		StandardInput,
		Messages
	}

	public enum MoveParameter
	{
		Angle,
		Size,
		SizeProportional
	}

	public InputSource source;

	public string axisName;

	public MoveParameter parameter;

	public float angleSensitivity;

	public float sizeSensitivity;

	public float angleDefault;

	public float sizeDefault;

	private CameraFovController m_fov;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CameraFovInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Move(float delta)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetDefaults()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ProcessStandardInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnScroll(float delta)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Scroll(float delta)
	{
		throw null;
	}
}

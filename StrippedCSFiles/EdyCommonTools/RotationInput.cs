using System.Runtime.CompilerServices;
using UnityEngine;

namespace EdyCommonTools;

[RequireComponent(typeof(RotationController))]
public class RotationInput : MonoBehaviour
{
	public enum InputSource
	{
		StandardInput,
		Messages
	}

	public InputSource source;

	public string horizontalAxis;

	public string verticalAxis;

	public int mouseButtonForDrag;

	public bool mouseButtonExclusive;

	public float horizontalSensitivity;

	public float verticalSensitivity;

	public float horizontalDefault;

	public float verticalDefault;

	private RotationController m_rot;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RotationInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Move(Vector2 delta)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetDefaults()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetDefaultsImmediate()
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
	public void OnDrag(Vector2 delta)
	{
		throw null;
	}
}

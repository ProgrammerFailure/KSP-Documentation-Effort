using System.Runtime.CompilerServices;
using UnityEngine;

namespace EdyCommonTools;

[RequireComponent(typeof(PositionController))]
public class PositionInput : MonoBehaviour
{
	public enum InputSource
	{
		StandardInput,
		Messages
	}

	public enum OutputPlane
	{
		XZ,
		XY,
		ZY
	}

	public InputSource source;

	public OutputPlane outputPlane;

	public bool swapCoordinates;

	public string inputAxisX;

	public string inputAxisY;

	public int mouseButtonForDrag;

	public bool mouseButtonExclusive;

	public Vector2 inputSensitivity;

	public Vector2 defaultPosition;

	private PositionController m_pos;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PositionInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 MapToPlane(Vector2 v, Vector3 defaultPos)
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
	public void OnMove(Vector2 delta)
	{
		throw null;
	}
}

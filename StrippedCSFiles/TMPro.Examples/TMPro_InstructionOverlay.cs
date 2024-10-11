using System.Runtime.CompilerServices;
using UnityEngine;

namespace TMPro.Examples;

public class TMPro_InstructionOverlay : MonoBehaviour
{
	public enum FpsCounterAnchorPositions
	{
		TopLeft,
		BottomLeft,
		TopRight,
		BottomRight
	}

	public FpsCounterAnchorPositions AnchorPosition;

	private const string instructions = "Camera Control - <#ffff00>Shift + RMB\n</color>Zoom - <#ffff00>Mouse wheel.";

	private TextMeshPro m_TextMeshPro;

	private TextContainer m_textContainer;

	private Transform m_frameCounter_transform;

	private Camera m_camera;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TMPro_InstructionOverlay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Set_FrameCounter_Position(FpsCounterAnchorPositions anchor_position)
	{
		throw null;
	}
}

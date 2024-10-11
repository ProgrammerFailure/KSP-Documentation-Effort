using System.Runtime.CompilerServices;
using UnityEngine;

namespace EdyCommonTools;

public class ApplicationCursor : MonoBehaviour
{
	[Header("On Enable")]
	public bool showCursor;

	public bool lockCursor;

	public bool dontChangeInEditor;

	[Header("On Update")]
	public bool autoHide;

	public float autoHideTimeout;

	public float speedThreshold;

	public GameObject[] skipIfActive;

	[Header("On Disable")]
	public bool restoreHiddenCursor;

	private Vector3 m_lastMousePosition;

	private float m_lastMovementTime;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ApplicationCursor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDisable()
	{
		throw null;
	}
}

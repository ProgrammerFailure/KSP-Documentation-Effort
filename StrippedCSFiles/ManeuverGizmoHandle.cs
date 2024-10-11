using System.Runtime.CompilerServices;
using UnityEngine;

public class ManeuverGizmoHandle : MonoBehaviour, IMouseEvents
{
	public delegate void HandleUpdate(float value);

	public enum HandleColors
	{
		NORMAL,
		HIGHLIGHT,
		PRESS
	}

	public Transform axis;

	public Transform flag;

	private ManeuverGizmoBase gizmo;

	private Material axisMat;

	private Material flagMat;

	private Color flagNormal;

	private Color flagHighlight;

	private Color flagPress;

	private Color axisNormal;

	private Color axisHighlight;

	private Color axisPress;

	private bool hover;

	private bool drag;

	private Vector3 initialAxisScale;

	private Vector3 initialFlagScale;

	private Vector3 initialPos;

	private Vector2 mousePosAtGrabTime;

	private Vector2 mouseScreenAxis;

	private Vector2 screenAxis;

	private float maxPullScale;

	private float maxValueLimit;

	private float minValueLimit;

	private float maxDragPixels;

	public float currentDrag;

	public float currentValue;

	private float scrollWheelMinSens;

	private float scrollWheelSensBoost;

	private float scrollWheelBoostDecay;

	private float currentScrollWheelBoost;

	public HandleUpdate OnHandleUpdate;

	public bool Hover
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool Drag
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ManeuverGizmoHandle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnMouseEnter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnMouseDown()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnMouseDrag()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnMouseUp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnMouseExit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MonoBehaviour GetInstance()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetHandleColor(HandleColors c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateHandle()
	{
		throw null;
	}
}

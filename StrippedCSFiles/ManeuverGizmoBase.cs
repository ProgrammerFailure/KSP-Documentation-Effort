using System.Runtime.CompilerServices;
using UnityEngine;

public class ManeuverGizmoBase : MonoBehaviour, IMouseEvents
{
	public Transform grabArea;

	public Transform buttonRoot;

	public Transform handlesRoot;

	public ManeuverGizmoHandle handlePrograde;

	public ManeuverGizmoHandle handleRetrograde;

	public ManeuverGizmoHandle handleNormal;

	public ManeuverGizmoHandle handleAntiNormal;

	public ManeuverGizmoHandle handleRadialIn;

	public ManeuverGizmoHandle handleRadialOut;

	public Camera camera;

	public Transform[] cameraFacingBillboards;

	protected Material grabMat;

	protected Color normal;

	protected Color highlight;

	protected Color press;

	protected Color willDelete;

	protected bool hover;

	public Callback OnGizmoDraggedOff;

	public Callback OnMinimize;

	public Callback OnDelete;

	public double sensitivity;

	public double multiplier;

	public Vector3d DeltaV;

	public double UT;

	protected bool buttonMode;

	public float screenSize;

	private float lerpedScreenSize;

	protected float rootsScale;

	protected float rootsLerpedScale;

	public static bool HasMouseFocus;

	protected bool mouseOverGizmo;

	public bool MouseOverGizmo
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ManeuverGizmoBase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetMouseOverGizmo(bool h)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ChangeToButtonMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Terminate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnProgradeUpdate(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnRetrogradeUpdate(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnNormalUpdate(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnAntinormalUpdate(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnRadialInUpdate(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnRadialOutUpdate(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MonoBehaviour GetInstance()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnMouseDown()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnMouseDrag()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnMouseEnter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnMouseExit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnMouseUp()
	{
		throw null;
	}
}

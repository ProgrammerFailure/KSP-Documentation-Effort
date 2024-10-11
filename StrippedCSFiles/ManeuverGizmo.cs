using System.Runtime.CompilerServices;
using KSP.UI;
using UnityEngine;
using UnityEngine.UI;

public class ManeuverGizmo : ManeuverGizmoBase
{
	public delegate void HandlesUpdatedCallback(Vector3d dV, double UT);

	public Canvas canvas;

	public Button deleteBtn;

	public Button plusOrbitBtn;

	public Button minusOrbitbtn;

	private PointerEnterExitHandler plusBtnHover;

	private PointerEnterExitHandler minusBtnHover;

	public int orbitsAdded;

	public HandlesUpdatedCallback OnGizmoUpdated;

	public Orbit patchBefore;

	public Orbit patchAhead;

	public PatchedConicRenderer renderer;

	private Vector3 mouseOffset;

	private PatchedConics.PatchCastHit scHit;

	private bool goodHover;

	private bool startDrag;

	private bool justTweaked;

	private double prevUT;

	private double prevprevUT;

	private bool wasLocked;

	private Vector3 lastMousePosition;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ManeuverGizmo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void SetMouseOverGizmo(bool h)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(ManeuverNode node, PatchedConicRenderer rnd)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPatches(Orbit patch, Orbit nextPatch, bool reset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnMouseDown()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnMouseDrag()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool DraggingForwardInTime()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnMouseUp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void NextOrbit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PreviousOrbit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnProgradeUpdate(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnRetrogradeUpdate(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnNormalUpdate(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnAntinormalUpdate(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnRadialInUpdate(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnRadialOutUpdate(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPlusOrbitPress()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMinusOrbitPress()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDeletePress()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onGizmoUpdate(Vector3d dV, double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool PreviousOrbitPossible()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool NextOrbitPossible()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateOrbitButtons()
	{
		throw null;
	}
}

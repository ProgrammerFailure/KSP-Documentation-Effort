using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class GAPCelestialBody_InteractiveSurfaceGizmo : GAPCelestialBody_SurfaceGizmo
{
	public delegate void OnPointTranslate(double latitude, double longitude);

	public List<GAPCelestialBodyGizmoHandle> gizmoHandles;

	public Sprite spriteFocusTarget;

	public Sprite spriteFocusCelestialBody;

	public OnPointTranslate OnGAPGizmoTranslate;

	protected GAPCelestialBodyGizmoHandle selectedHandle;

	private double finalDragLatitude;

	private double finalDragLongitude;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GAPCelestialBody_InteractiveSurfaceGizmo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Initialize(GAPCelestialBody newGapRef)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override double SetGizmoPosition(double newLatitude, double newLongitude, double newAltitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoadPlanet(CelestialBody newCelestialBody)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Transform GetFocusTarget()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnGizmoHandleOver(double hoverLatitude, double hoverLongitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnGAPOver(double hoverLatitude, double hoverLongitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnGAPClick(double newLatitude, double newLongitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnGAPDrag(double dragStartLatitude, double dragStartLongitude, double dragLatitude, double dragLongitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnGAPDragEnd()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnGizmoHandleDrag(double dragStartLatitude, double dragStartLongitude, double dragLatitude, double dragLongitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnGizmoHandleDragEnd()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnGizmoHandleSelected(GAPCelestialBodyGizmoHandle selectedHandle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual string GetFooterText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool TryEndHover(Collider handleCollider)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnButtonPressed_FocusTarget()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TrySelectHandle(Collider handleCollider)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsDragReady()
	{
		throw null;
	}
}

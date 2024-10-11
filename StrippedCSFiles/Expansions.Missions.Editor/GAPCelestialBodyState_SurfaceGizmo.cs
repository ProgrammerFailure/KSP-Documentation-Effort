using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Expansions.Missions.Editor;

public class GAPCelestialBodyState_SurfaceGizmo : GAPCelestialBodyState_Base
{
	private SurfaceLocation hoveredSurfacePoint;

	private GAPCelestialBody_InteractiveSurfaceGizmo gizmoEntity;

	public GAPCelestialBody_SurfaceGizmo GizmoEntity
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GAPCelestialBodyState_SurfaceGizmo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Init(GAPCelestialBody gapRef)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void End()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void LoadPlanet(CelestialBody newCelestialBody)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void UnloadPlanet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnClickUp(RaycastHit? hit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnMouseOver(Vector2 cameraPoint)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnDrag(PointerEventData.InputButton arg0, Vector2 dragVector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnDragEnd(RaycastHit? hit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GAPCelestialBody_InteractiveSurfaceGizmo InstantiateGizmo(string gizmoName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private SurfaceLocation GetSurfaceLocation(Vector3d point)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ButtonAction_Recenter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateFooterText(SurfaceLocation? hoveredLocation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateFooterText()
	{
		throw null;
	}
}

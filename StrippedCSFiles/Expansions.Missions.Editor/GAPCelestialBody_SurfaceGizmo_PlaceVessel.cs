using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class GAPCelestialBody_SurfaceGizmo_PlaceVessel : GAPCelestialBody_InteractiveSurfaceGizmo
{
	public enum InteractMode
	{
		TRANSLATION,
		ROTATION
	}

	public delegate void OnPointRotate(Quaternion newRotation);

	public List<GameObject> gizmoIcons;

	public GameObject gizmoTranslate;

	public GameObject gizmoRotate;

	public Transform pivotRotation;

	public MeshRenderer meshCenterGizmo;

	public List<GAPCelestialBodyGizmoHandle> handlesTranslation;

	public List<GAPCelestialBodyGizmoHandle> handlesRotation;

	public OnPointRotate OnGAPGizmoRotate;

	private InteractMode interactMode;

	private VesselGroundLocation.GizmoIcon? currentGizmoIcon;

	private Quaternion rotation;

	private Quaternion dragStartRotation;

	private Quaternion finalDragRotation;

	private float dragStartAngle;

	public Quaternion Rotation
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GAPCelestialBody_SurfaceGizmo_PlaceVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Initialize(GAPCelestialBody newGapRef)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddTransformButtons()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SelectTranslate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SelectRotate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnGizmoHandleSelected(GAPCelestialBodyGizmoHandle selectedHandle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnGizmoHandleDrag(double dragStartLatitude, double dragStartLongitude, double dragLatitude, double dragLongitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnGizmoHandleDragEnd()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetGizmoRotation(Quaternion newRotation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetGizmoIcon(VesselGroundLocation.GizmoIcon gizmoIcon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetMode(InteractMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetFooterText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClearMode(InteractMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetGizmoCenterAngle()
	{
		throw null;
	}
}

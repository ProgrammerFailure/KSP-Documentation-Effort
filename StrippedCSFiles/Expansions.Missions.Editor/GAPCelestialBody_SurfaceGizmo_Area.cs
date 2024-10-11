using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class GAPCelestialBody_SurfaceGizmo_Area : GAPCelestialBody_InteractiveSurfaceGizmo
{
	public delegate void OnRadiusChange(float newRadius);

	public OnRadiusChange OnGAPGizmoRadiusChange;

	public Projector projectorArea;

	public Transform containerProjectors;

	public Color colorArea_Idle;

	public Color colorArea_Drag;

	public Color colorEdge_Idle;

	public Color colorEdge_Highlight;

	private bool areaSelected;

	private float radius;

	private float maxRadius;

	private float radiusIgnorePercetage;

	private float radiusSelectionPercentage;

	public float Radius
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public float MaxRadius
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	private Color EdgeColor
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	private Color AreaColor
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GAPCelestialBody_SurfaceGizmo_Area()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Initialize(GAPCelestialBody newGapRef)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnGAPClick(double newLatitude, double newLongitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnGAPOver(double hoverLatitude, double hoverLongitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnGAPDrag(double dragStartLatitude, double dragStartLongitude, double dragLatitude, double dragLongitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnGAPDragEnd()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnGizmoHandleSelected(GAPCelestialBodyGizmoHandle selectedHandle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoadPlanet(CelestialBody newCelestialBody)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetFooterText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckAreaSelected(double greatCircleDistance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetAreaRadius(float newRadius)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetAreaColor(AreaState areaState)
	{
		throw null;
	}
}

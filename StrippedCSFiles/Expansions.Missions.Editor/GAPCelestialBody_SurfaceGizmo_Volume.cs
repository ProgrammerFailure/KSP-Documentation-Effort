using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class GAPCelestialBody_SurfaceGizmo_Volume : GAPCelestialBody_InteractiveSurfaceGizmo
{
	public delegate void OnGAPValueChange(float newRadius);

	public delegate void OnGAPShapeChange(SurfaceVolume.VolumeShape newShape);

	public delegate void OnGAPShapeBoundsChange(float minValue, float maxValue);

	public OnGAPValueChange OnGAPGizmoRadiusChange;

	public OnGAPValueChange OnGAPGizmoHeightSphere;

	public OnGAPShapeChange OnGAPGizmoShapeChange;

	public OnGAPShapeBoundsChange OnGAPGizmoConeBoundsChange;

	public Projector projectorRadius;

	public Transform containerProjector;

	public Color colorRadius_Idle;

	public Color colorRadius_Drag;

	public Color colorRadiusEdge_Idle;

	public Color colorRadiusEdge_Highlight;

	public GAPUtil_DynamicCylinder dynamicCylinder;

	public Material materialCylinder;

	public GameObject dynamicSphere;

	public Transform dynamicSpherePivot;

	public AnimationCurve curveCylinderBounds;

	public AnimationCurve curveSphereHeight;

	public LineRenderer heightLine;

	private AreaState currentAreaState;

	private SurfaceVolume.VolumeShape currentShape;

	private float radius;

	private float maxRadius;

	private float maxLineWidth;

	private float heightSphere;

	private float heightConeMin;

	private float heightConeMax;

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

	public SurfaceVolume.VolumeShape Shape
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

	public float HeightSphere
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

	public float HeightConeMin
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

	public float HeightConeMax
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

	private Color RadiusEdgeColor
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	private Color RadiusFillColor
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GAPCelestialBody_SurfaceGizmo_Volume()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetConeBounds(float minValue, float maxValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Initialize(GAPCelestialBody newGapRef)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSimpleSliderValue(float newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDoubleSliderValue(float minValue, float maxValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnButtonPressed_Shape()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateLineRenderer(float newRadius)
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
	public void UpdateFooterText_Sphere()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateFooterText_Cone()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetAreaColor(AreaState areaState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetShape(SurfaceVolume.VolumeShape newShape)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void HideShape(SurfaceVolume.VolumeShape shapeToHide)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateAreaState(double greatCircleDist)
	{
		throw null;
	}
}

using System;
using FinePrint.Utilities;
using ns9;
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

	public AreaState currentAreaState;

	public SurfaceVolume.VolumeShape currentShape;

	public float radius;

	public float maxRadius;

	public float maxLineWidth;

	public float heightSphere;

	public float heightConeMin;

	public float heightConeMax;

	public float radiusIgnorePercetage;

	public float radiusSelectionPercentage;

	public float Radius
	{
		get
		{
			return radius;
		}
		set
		{
			if (value > maxRadius)
			{
				value = maxRadius;
			}
			projectorRadius.orthographicSize = value;
			radius = value;
			dynamicSphere.transform.localScale = Vector3.one * radius * 2f;
			UpdateLineRenderer(value);
		}
	}

	public float MaxRadius
	{
		get
		{
			return maxRadius;
		}
		set
		{
			maxRadius = value;
		}
	}

	public SurfaceVolume.VolumeShape Shape
	{
		get
		{
			return currentShape;
		}
		set
		{
			if (value != currentShape)
			{
				HideShape(currentShape);
				currentShape = value;
				SetShape(value);
			}
		}
	}

	public float HeightSphere
	{
		get
		{
			return heightSphere;
		}
		set
		{
			heightSphere = value;
			gapRef.Selector.sliderSimple.value = value / (float)gapRef.CelestialBody.Radius;
			dynamicSphere.transform.localPosition = new Vector3d(0.0, 0.0, value);
			if (currentShape == SurfaceVolume.VolumeShape.Sphere)
			{
				heightLine.enabled = value - radius > 0f;
			}
		}
	}

	public float HeightConeMin
	{
		get
		{
			return heightConeMin;
		}
		set
		{
			heightConeMin = value;
			gapRef.Selector.doubleSlider.MinValue = value / (float)gapRef.CelestialBody.Radius;
			dynamicCylinder.UpdateValues(radius, gapRef.Selector.doubleSlider.MinValue, gapRef.Selector.doubleSlider.MaxValue, celestialBody.Radius);
		}
	}

	public float HeightConeMax
	{
		get
		{
			return heightConeMax;
		}
		set
		{
			heightConeMax = value;
			gapRef.Selector.doubleSlider.MaxValue = value / (float)gapRef.CelestialBody.Radius;
			dynamicCylinder.UpdateValues(radius, gapRef.Selector.doubleSlider.MinValue, gapRef.Selector.doubleSlider.MaxValue, celestialBody.Radius);
		}
	}

	public Color RadiusEdgeColor
	{
		set
		{
			projectorRadius.material.SetColor("_Color", value);
		}
	}

	public Color RadiusFillColor
	{
		set
		{
			projectorRadius.material.SetColor("_SecColor", value);
		}
	}

	public void SetConeBounds(float minValue, float maxValue)
	{
		heightConeMin = minValue;
		heightConeMax = maxValue;
		float num = minValue / (float)gapRef.CelestialBody.Radius;
		float num2 = maxValue / (float)gapRef.CelestialBody.Radius;
		gapRef.Selector.doubleSlider.MinValue = num;
		gapRef.Selector.doubleSlider.MaxValue = num2;
		dynamicCylinder.UpdateValues(radius, num, num2, celestialBody.Radius);
	}

	public override void Initialize(GAPCelestialBody newGapRef)
	{
		base.Initialize(newGapRef);
		maxRadius = float.MaxValue;
		radiusIgnorePercetage = 0.9f;
		radiusSelectionPercentage = 1.1f;
		currentAreaState = AreaState.IDLE;
		RadiusFillColor = colorRadius_Idle;
		RadiusEdgeColor = colorRadiusEdge_Idle;
		gapRef.AddToolbarButton("buttonShape", "changeShapeIcon", "#autoLOC_8006091").onClick.AddListener(OnButtonPressed_Shape);
		dynamicCylinder.CreateMesh(materialCylinder);
		SetShape(currentShape);
		DoubleSlider doubleSlider = gapRef.Selector.doubleSlider;
		doubleSlider.onValueChanged = (DoubleSlider.OnValueChanged)Delegate.Combine(doubleSlider.onValueChanged, new DoubleSlider.OnValueChanged(OnDoubleSliderValue));
		gapRef.Selector.doubleSlider.AllowClipping = true;
		gapRef.Selector.sliderSimple.onValueChanged.AddListener(OnSimpleSliderValue);
		GAPUtil_CelestialBody selector = gapRef.Selector;
		selector.OnSliderHovered_Simple = (GAPUtil_CelestialBody.OnSliderHovered)Delegate.Combine(selector.OnSliderHovered_Simple, new GAPUtil_CelestialBody.OnSliderHovered(UpdateFooterText_Sphere));
		GAPUtil_CelestialBody selector2 = gapRef.Selector;
		selector2.OnSliderHovered_Double = (GAPUtil_CelestialBody.OnSliderHovered)Delegate.Combine(selector2.OnSliderHovered_Double, new GAPUtil_CelestialBody.OnSliderHovered(UpdateFooterText_Cone));
	}

	public void OnSimpleSliderValue(float newValue)
	{
		if (OnGAPGizmoHeightSphere != null)
		{
			OnGAPGizmoHeightSphere(newValue * (float)gapRef.CelestialBody.Radius);
		}
	}

	public void OnDoubleSliderValue(float minValue, float maxValue)
	{
		if (OnGAPGizmoConeBoundsChange != null)
		{
			OnGAPGizmoConeBoundsChange(minValue * (float)gapRef.CelestialBody.Radius, maxValue * (float)gapRef.CelestialBody.Radius);
		}
	}

	public void OnButtonPressed_Shape()
	{
		if (OnGAPGizmoShapeChange != null)
		{
			SurfaceVolume.VolumeShape newShape = ((currentShape == SurfaceVolume.VolumeShape.Cone) ? SurfaceVolume.VolumeShape.Sphere : SurfaceVolume.VolumeShape.Cone);
			OnGAPGizmoShapeChange(newShape);
		}
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		heightLine.SetPosition(0, base.transform.position);
		Vector3 position = ((currentShape == SurfaceVolume.VolumeShape.Cone) ? dynamicCylinder.GetPivotPosition() : dynamicSpherePivot.position);
		heightLine.SetPosition(1, position);
	}

	public void UpdateLineRenderer(float newRadius)
	{
		bool flag = true;
		if (currentShape == SurfaceVolume.VolumeShape.Sphere)
		{
			flag = heightSphere - newRadius > 0f;
		}
		heightLine.enabled = flag;
		if (flag)
		{
			Vector3 b = ((currentShape == SurfaceVolume.VolumeShape.Cone) ? dynamicCylinder.GetPivotPosition() : dynamicSpherePivot.position);
			float num = Vector3.Distance(base.transform.position, b);
			float num2 = Mathf.Clamp(newRadius * 0.25f, 1f, maxLineWidth);
			heightLine.startWidth = num2;
			heightLine.endWidth = num2;
			heightLine.material.mainTextureScale = new Vector2(num / num2, 1f);
		}
	}

	public override void OnGAPClick(double newLatitude, double newLongitude)
	{
		if (currentAreaState == AreaState.IDLE)
		{
			base.OnGAPClick(newLatitude, newLongitude);
		}
	}

	public override void OnGAPOver(double hoverLatitude, double hoverLongitude)
	{
		base.OnGAPOver(hoverLatitude, hoverLongitude);
		if (selectedHandle == null)
		{
			double greatCircleDist = CelestialUtilities.GreatCircleDistance(gapRef.CelestialBody, hoverLatitude, hoverLongitude, latitude, longitude);
			UpdateAreaState(greatCircleDist);
		}
	}

	public override void OnGAPDrag(double dragStartLatitude, double dragStartLongitude, double dragLatitude, double dragLongitude)
	{
		if (currentAreaState == AreaState.IDLE)
		{
			base.OnGAPDrag(dragStartLatitude, dragStartLongitude, dragLatitude, dragLongitude);
			return;
		}
		double num = CelestialUtilities.GreatCircleDistance(gapRef.CelestialBody, dragLatitude, dragLongitude, latitude, longitude);
		Radius = (float)num;
	}

	public override void OnGAPDragEnd()
	{
		base.OnGAPDragEnd();
		if (OnGAPGizmoRadiusChange != null)
		{
			OnGAPGizmoRadiusChange(radius);
		}
	}

	public override void OnGizmoHandleSelected(GAPCelestialBodyGizmoHandle selectedHandle)
	{
		base.OnGizmoHandleSelected(selectedHandle);
	}

	public override void OnLoadPlanet(CelestialBody newCelestialBody)
	{
		base.OnLoadPlanet(newCelestialBody);
		float num = (float)newCelestialBody.Radius;
		containerProjector.localPosition = new Vector3d(0.0, 0.0, num / base.transform.localScale.x);
		projectorRadius.nearClipPlane = num / 2f;
		projectorRadius.farClipPlane = num * 2f;
		projectorRadius.orthographicSize = num / 8f;
		maxLineWidth = (float)gapRef.CelestialBody.Radius * 0.02f;
	}

	public override bool IsActive()
	{
		if (!(selectedHandle != null))
		{
			return currentAreaState != AreaState.IDLE;
		}
		return true;
	}

	public override string GetFooterText()
	{
		return Localizer.Format("#autoLOC_8006107", longitude.ToString("f2"), latitude.ToString("f2"), radius.ToString("f2"));
	}

	public void UpdateFooterText_Sphere()
	{
		gapRef.SetFooterText(Localizer.Format("#autoLOC_8007302", heightSphere));
	}

	public void UpdateFooterText_Cone()
	{
		gapRef.SetFooterText(Localizer.Format("#autoLOC_8007303", heightConeMin, heightConeMax));
	}

	public void SetAreaColor(AreaState areaState)
	{
		switch (areaState)
		{
		case AreaState.IDLE:
			RadiusFillColor = colorRadius_Idle;
			RadiusEdgeColor = colorRadiusEdge_Idle;
			break;
		case AreaState.HIGHLIGHT_EDGE:
			RadiusFillColor = colorRadius_Idle;
			RadiusEdgeColor = colorRadiusEdge_Highlight;
			break;
		case AreaState.DRAGGED:
			RadiusFillColor = colorRadius_Drag;
			RadiusEdgeColor = colorRadiusEdge_Idle;
			break;
		}
	}

	public void SetShape(SurfaceVolume.VolumeShape newShape)
	{
		switch (newShape)
		{
		case SurfaceVolume.VolumeShape.Sphere:
			dynamicSphere.SetActive(value: true);
			gapRef.Selector.sliderSimple.gameObject.SetActive(value: true);
			heightLine.enabled = heightSphere - radius > 0f;
			break;
		case SurfaceVolume.VolumeShape.Cone:
			dynamicCylinder.gameObject.SetActive(value: true);
			gapRef.Selector.doubleSlider.gameObject.SetActive(value: true);
			heightLine.enabled = true;
			break;
		}
		currentShape = newShape;
	}

	public void HideShape(SurfaceVolume.VolumeShape shapeToHide)
	{
		switch (shapeToHide)
		{
		case SurfaceVolume.VolumeShape.Sphere:
			dynamicSphere.SetActive(value: false);
			gapRef.Selector.sliderSimple.gameObject.SetActive(value: false);
			break;
		case SurfaceVolume.VolumeShape.Cone:
			dynamicCylinder.gameObject.SetActive(value: false);
			gapRef.Selector.doubleSlider.gameObject.SetActive(value: false);
			break;
		}
	}

	public void UpdateAreaState(double greatCircleDist)
	{
		AreaState areaState = currentAreaState;
		if (greatCircleDist < (double)(radius * radiusSelectionPercentage))
		{
			if (greatCircleDist > (double)(radius * radiusIgnorePercetage))
			{
				areaState = AreaState.HIGHLIGHT_EDGE;
			}
			else
			{
				TrySelectHandle(gizmoHandles[2].GetComponent<Collider>());
				areaState = AreaState.DRAGGED;
			}
		}
		else
		{
			areaState = AreaState.IDLE;
		}
		if (areaState != currentAreaState)
		{
			SetAreaColor(areaState);
			currentAreaState = areaState;
		}
	}
}

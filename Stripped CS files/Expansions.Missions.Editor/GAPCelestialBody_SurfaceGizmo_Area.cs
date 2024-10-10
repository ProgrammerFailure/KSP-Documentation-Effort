using FinePrint.Utilities;
using ns9;
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

	public bool areaSelected;

	public float radius;

	public float maxRadius;

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
			projectorArea.orthographicSize = value;
			radius = value;
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

	public Color EdgeColor
	{
		set
		{
			projectorArea.material.SetColor("_Color", value);
		}
	}

	public Color AreaColor
	{
		set
		{
			projectorArea.material.SetColor("_SecColor", value);
		}
	}

	public override void Initialize(GAPCelestialBody newGapRef)
	{
		base.Initialize(newGapRef);
		maxRadius = float.MaxValue;
		radiusIgnorePercetage = 0.9f;
		radiusSelectionPercentage = 1.1f;
		SetAreaColor(AreaState.IDLE);
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
	}

	public override void OnGAPClick(double newLatitude, double newLongitude)
	{
		if (!areaSelected)
		{
			base.OnGAPClick(newLatitude, newLongitude);
		}
	}

	public override void OnGAPOver(double hoverLatitude, double hoverLongitude)
	{
		base.OnGAPOver(hoverLatitude, hoverLongitude);
		if (selectedHandle == null)
		{
			double greatCircleDistance = CelestialUtilities.GreatCircleDistance(gapRef.CelestialBody, hoverLatitude, hoverLongitude, latitude, longitude);
			CheckAreaSelected(greatCircleDistance);
		}
	}

	public override void OnGAPDrag(double dragStartLatitude, double dragStartLongitude, double dragLatitude, double dragLongitude)
	{
		if (!areaSelected)
		{
			base.OnGAPDrag(dragStartLatitude, dragStartLongitude, dragLatitude, dragLongitude);
			return;
		}
		double num = CelestialUtilities.GreatCircleDistance(gapRef.CelestialBody, dragLatitude, dragLongitude, latitude, longitude);
		SetAreaRadius((float)num);
	}

	public override void OnGAPDragEnd()
	{
		base.OnGAPDragEnd();
		if (areaSelected && OnGAPGizmoRadiusChange != null)
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
		containerProjectors.localPosition = new Vector3d(0.0, 0.0, num / base.transform.localScale.x);
		projectorArea.nearClipPlane = num / 1.5f;
		projectorArea.farClipPlane = num * 2f;
	}

	public override bool IsActive()
	{
		if (!(selectedHandle != null))
		{
			return areaSelected;
		}
		return true;
	}

	public override string GetFooterText()
	{
		return Localizer.Format("#autoLOC_8006109", longitude.ToString("f2"), latitude.ToString("f2"), radius.ToString("f2"));
	}

	public void CheckAreaSelected(double greatCircleDistance)
	{
		areaSelected = false;
		if (greatCircleDistance < (double)(radius * radiusSelectionPercentage))
		{
			areaSelected = true;
			if (greatCircleDistance > (double)(radius * radiusIgnorePercetage))
			{
				SetAreaColor(AreaState.HIGHLIGHT_EDGE);
				return;
			}
			TrySelectHandle(gizmoHandles[2].GetComponent<Collider>());
			SetAreaColor(AreaState.DRAGGED);
		}
		else
		{
			SetAreaColor(AreaState.IDLE);
		}
	}

	public void SetAreaRadius(float newRadius)
	{
		Radius = newRadius;
	}

	public void SetAreaColor(AreaState areaState)
	{
		if (areaSelected)
		{
			switch (areaState)
			{
			case AreaState.IDLE:
				AreaColor = colorArea_Idle;
				EdgeColor = colorEdge_Idle;
				break;
			case AreaState.HIGHLIGHT_EDGE:
				AreaColor = colorArea_Idle;
				EdgeColor = colorEdge_Highlight;
				break;
			case AreaState.DRAGGED:
				AreaColor = colorArea_Drag;
				EdgeColor = colorEdge_Idle;
				break;
			}
		}
		else
		{
			AreaColor = colorArea_Idle;
			EdgeColor = colorEdge_Idle;
		}
	}
}

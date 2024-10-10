using System;
using System.Collections.Generic;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class GAPCelestialBody_InteractiveSurfaceGizmo : GAPCelestialBody_SurfaceGizmo
{
	public delegate void OnPointTranslate(double latitude, double longitude);

	public List<GAPCelestialBodyGizmoHandle> gizmoHandles;

	public Sprite spriteFocusTarget;

	public Sprite spriteFocusCelestialBody;

	public OnPointTranslate OnGAPGizmoTranslate;

	public GAPCelestialBodyGizmoHandle selectedHandle;

	public double finalDragLatitude;

	public double finalDragLongitude;

	public override void Initialize(GAPCelestialBody newGapRef)
	{
		base.Initialize(newGapRef);
		selectedHandle = null;
		gapRef.AddToolbarButton("buttonTarget", "targetIcon", "#autoLOC_8006090").onClick.AddListener(OnButtonPressed_FocusTarget);
		gapRef.AddToolbarButton("buttonRecenter", "recenterIcon", "#autoLOC_8006092");
		gapRef.Selector.containerFooter.SetActive(value: true);
	}

	public override bool IsActive()
	{
		return selectedHandle != null;
	}

	public override double SetGizmoPosition(double newLatitude, double newLongitude, double newAltitude)
	{
		double result = base.SetGizmoPosition(newLatitude, newLongitude, newAltitude);
		if (OnGAPGizmoTranslate != null)
		{
			OnGAPGizmoTranslate(newLatitude, newLongitude);
		}
		return result;
	}

	public override void OnLoadPlanet(CelestialBody newCelestialBody)
	{
		base.OnLoadPlanet(newCelestialBody);
		gapRef.CelestialCamera.FocusTarget = gapRef.CelestialBody.transform;
	}

	public override void OnUpdate()
	{
		double num = gapRef.CelestialCamera.ZoomValue * 2.0 + 0.0010000000474974513;
		gizmoContainer.localScale = Vector3d.one * (num * celestialBody.Radius);
	}

	public virtual Transform GetFocusTarget()
	{
		return base.transform;
	}

	public virtual void OnGizmoHandleOver(double hoverLatitude, double hoverLongitude)
	{
	}

	public virtual void OnGAPOver(double hoverLatitude, double hoverLongitude)
	{
	}

	public virtual void OnGAPClick(double newLatitude, double newLongitude)
	{
		if (OnGAPGizmoTranslate != null)
		{
			OnGAPGizmoTranslate(newLatitude, newLongitude);
		}
		gapRef.CelestialCamera.RefreshTarget();
	}

	public virtual void OnGAPDrag(double dragStartLatitude, double dragStartLongitude, double dragLatitude, double dragLongitude)
	{
	}

	public virtual void OnGAPDragEnd()
	{
	}

	public virtual void OnGizmoHandleDrag(double dragStartLatitude, double dragStartLongitude, double dragLatitude, double dragLongitude)
	{
		double num = (dragStartLongitude - dragLongitude) * (double)selectedHandle.moveDirection.x;
		double num2 = (dragStartLatitude - dragLatitude) * (double)selectedHandle.moveDirection.y;
		finalDragLongitude = longitude - num;
		finalDragLatitude = latitude - num2;
		Vector3 vector = celestialBody.GetRelSurfacePosition(finalDragLatitude, finalDragLongitude, 0.0);
		double num3 = celestialBody.TerrainAltitude(latitude, longitude, allowNegative: true);
		if (num3 < 0.0)
		{
			SetInSurfaceFromPointAtSeaLevel(vector, Math.Abs(num3));
		}
		else
		{
			SetInSurfaceFromPoint(vector);
		}
	}

	public virtual void OnGizmoHandleDragEnd()
	{
		if (OnGAPGizmoTranslate != null)
		{
			OnGAPGizmoTranslate(finalDragLatitude, finalDragLongitude);
		}
		gapRef.CelestialCamera.RefreshTarget();
	}

	public virtual void OnGizmoHandleSelected(GAPCelestialBodyGizmoHandle selectedHandle)
	{
	}

	public virtual string GetFooterText()
	{
		return "<b><color=#4DC44D>Gizmo: Lon: </color></b>" + longitude.ToString("f1") + " <color=#4DC44D>Lat: </color>" + latitude.ToString("f1");
	}

	public bool TryEndHover(Collider handleCollider)
	{
		if (selectedHandle != null && handleCollider.gameObject != selectedHandle.gameObject)
		{
			selectedHandle.OnHoverEnd();
			selectedHandle = null;
			return true;
		}
		return false;
	}

	public void OnButtonPressed_FocusTarget()
	{
		if (!(gapRef.CelestialCamera.FocusTarget == null) && !(gapRef.CelestialCamera.FocusTarget == gapRef.CelestialBody.transform))
		{
			gapRef.CelestialCamera.FocusTarget = gapRef.CelestialBody.transform;
		}
		else
		{
			gapRef.CelestialCamera.FocusTarget = GetFocusTarget();
		}
		gapRef.CelestialCamera.SetPosition(latitude, longitude);
	}

	public bool TrySelectHandle(Collider handleCollider)
	{
		bool result = false;
		TryEndHover(handleCollider);
		for (int i = 0; i < gizmoHandles.Count; i++)
		{
			if (handleCollider.gameObject == gizmoHandles[i].gameObject)
			{
				result = true;
				if (selectedHandle == null)
				{
					selectedHandle = gizmoHandles[i];
					selectedHandle.OnHoverStart();
					OnGizmoHandleSelected(selectedHandle);
				}
				break;
			}
		}
		return result;
	}

	public bool IsDragReady()
	{
		bool result = false;
		if (selectedHandle != null)
		{
			result = true;
		}
		return result;
	}
}

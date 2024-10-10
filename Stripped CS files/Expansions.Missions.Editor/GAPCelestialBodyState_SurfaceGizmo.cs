using System;
using ns2;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Expansions.Missions.Editor;

public class GAPCelestialBodyState_SurfaceGizmo : GAPCelestialBodyState_Base
{
	public SurfaceLocation hoveredSurfacePoint;

	public GAPCelestialBody_InteractiveSurfaceGizmo gizmoEntity;

	public GAPCelestialBody_SurfaceGizmo GizmoEntity => gizmoEntity;

	public override void Init(GAPCelestialBody gapRef)
	{
		base.Init(gapRef);
		gapRef.TogglePQS(usePQS: true);
		gapRef.AddToolbarButton("buttonRecenter", "recenterIcon", "#autoLOC_8006092").onClick.AddListener(ButtonAction_Recenter);
	}

	public override void Update()
	{
		gizmoEntity.OnUpdate();
	}

	public override void End()
	{
		gizmoEntity.OnDestroy();
		UnityEngine.Object.Destroy(gizmoEntity.gameObject);
	}

	public override void LoadPlanet(CelestialBody newCelestialBody)
	{
		base.LoadPlanet(newCelestialBody);
		gizmoEntity.OnLoadPlanet(newCelestialBody);
		gapRef.CelestialCamera.OverridePosition(gizmoEntity.Latitude, gizmoEntity.Longitude);
	}

	public override void UnloadPlanet()
	{
		base.UnloadPlanet();
	}

	public override void OnClickUp(RaycastHit? hit)
	{
		if (hit.HasValue && gapRef.CelestialBody.BiomeMap != null)
		{
			SurfaceLocation surfaceLocation = GetSurfaceLocation(hit.Value.point);
			gizmoEntity.OnGAPClick(surfaceLocation.latitude, surfaceLocation.longitude);
		}
	}

	public override void OnMouseOver(Vector2 cameraPoint)
	{
		if (gapRef.CelestialBody.BiomeMap == null)
		{
			return;
		}
		RaycastHit[] array = gapRef.DoLocalSpaceRayAll(cameraPoint);
		if (array != null && array.Length != 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				RaycastHit raycastHit = array[i];
				if (raycastHit.collider != null)
				{
					bool num = gizmoEntity.TrySelectHandle(raycastHit.collider);
					hoveredSurfacePoint = GetSurfaceLocation(raycastHit.point);
					if (num)
					{
						gizmoEntity.OnGizmoHandleOver(hoveredSurfacePoint.latitude, hoveredSurfacePoint.longitude);
						break;
					}
					gizmoEntity.OnGAPOver(hoveredSurfacePoint.latitude, hoveredSurfacePoint.longitude);
				}
			}
		}
		UpdateFooterText(hoveredSurfacePoint);
	}

	public override void OnDrag(PointerEventData.InputButton arg0, Vector2 dragVector)
	{
		if (Input.GetMouseButton(1))
		{
			return;
		}
		Vector2 point = Vector2.zero;
		gapRef.GetMousePointOnCamera(Input.mousePosition, UIMasterController.Instance.mainCanvas.worldCamera, ref point);
		if (gapRef.DoLocalSpaceRay(point, out var rayHit) && rayHit.collider != null)
		{
			SurfaceLocation surfaceLocation = GetSurfaceLocation(rayHit.point);
			if (gizmoEntity.IsDragReady())
			{
				gizmoEntity.OnGizmoHandleDrag(hoveredSurfacePoint.latitude, hoveredSurfacePoint.longitude, surfaceLocation.latitude, surfaceLocation.longitude);
			}
			else
			{
				gizmoEntity.OnGAPDrag(hoveredSurfacePoint.latitude, hoveredSurfacePoint.longitude, surfaceLocation.latitude, surfaceLocation.longitude);
			}
			UpdateFooterText(surfaceLocation);
		}
		else
		{
			UpdateFooterText(null);
		}
	}

	public override void OnDragEnd(RaycastHit? hit)
	{
		if (!Input.GetMouseButton(1))
		{
			if (gizmoEntity.IsDragReady())
			{
				gizmoEntity.OnGizmoHandleDragEnd();
			}
			gizmoEntity.OnGAPDragEnd();
		}
	}

	public GAPCelestialBody_InteractiveSurfaceGizmo InstantiateGizmo(string gizmoName)
	{
		GameObject original = MissionsUtils.MEPrefab("Prefabs/" + gizmoName + ".prefab");
		gizmoEntity = UnityEngine.Object.Instantiate(original).GetComponent<GAPCelestialBody_InteractiveSurfaceGizmo>();
		gizmoEntity.Initialize(gapRef);
		return gizmoEntity;
	}

	public SurfaceLocation GetSurfaceLocation(Vector3d point)
	{
		Vector3d vector3d = (QuaternionD)Quaternion.LookRotation(point) * Vector3d.forward;
		double surfaceHeight = gapRef.CelestialBody.pqsController.GetSurfaceHeight(vector3d);
		Vector3d normalized = (vector3d * surfaceHeight).normalized;
		SurfaceLocation result = default(SurfaceLocation);
		result.altitude = surfaceHeight - gapRef.CelestialBody.Radius;
		result.latitude = Math.Asin(0.0 - normalized.y) * -57.295780181884766;
		result.longitude = Math.Atan2(normalized.z, normalized.x) * 57.295780181884766;
		return result;
	}

	public void ButtonAction_Recenter()
	{
		gapRef.CelestialCamera.SetPosition(gizmoEntity.Latitude, gizmoEntity.Longitude);
	}

	public void UpdateFooterText(SurfaceLocation? hoveredLocation)
	{
		if (gapRef.Selector.HoveredSlider == GAPUtil_CelestialBody.GAPUtilSlider.None)
		{
			gapRef.SetFooterText(gizmoEntity.GetFooterText());
		}
	}

	public void UpdateFooterText()
	{
		UpdateFooterText(null);
	}
}

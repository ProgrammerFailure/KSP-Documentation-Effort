using System;
using System.Collections.Generic;
using ns2;
using ns9;
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

	public InteractMode interactMode;

	public VesselGroundLocation.GizmoIcon? currentGizmoIcon;

	public Quaternion rotation;

	public Quaternion dragStartRotation;

	public Quaternion finalDragRotation;

	public float dragStartAngle;

	public Quaternion Rotation => rotation;

	public override void Initialize(GAPCelestialBody newGapRef)
	{
		base.Initialize(newGapRef);
		SetMode(InteractMode.TRANSLATION);
	}

	public void AddTransformButtons()
	{
		gapRef.AddToolbarButton("buttonRotate", "rotateIcon", "#autoLOC_8006089").onClick.AddListener(SelectRotate);
		gapRef.AddToolbarButton("buttonTranslate", "translateIcon", "#autoLOC_8006088").onClick.AddListener(SelectTranslate);
	}

	public void SelectTranslate()
	{
		SetMode(InteractMode.TRANSLATION);
	}

	public void SelectRotate()
	{
		SetMode(InteractMode.ROTATION);
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
	}

	public override void OnGizmoHandleSelected(GAPCelestialBodyGizmoHandle selectedHandle)
	{
		switch (interactMode)
		{
		case InteractMode.ROTATION:
			dragStartRotation = pivotRotation.transform.localRotation;
			dragStartAngle = GetGizmoCenterAngle();
			break;
		case InteractMode.TRANSLATION:
			base.OnGizmoHandleSelected(selectedHandle);
			break;
		}
	}

	public override void OnGizmoHandleDrag(double dragStartLatitude, double dragStartLongitude, double dragLatitude, double dragLongitude)
	{
		switch (interactMode)
		{
		case InteractMode.ROTATION:
		{
			Vector3 lhs = pivotRotation.transform.TransformDirection(selectedHandle.moveDirection);
			Vector3 rhs = base.transform.position - gapRef.CelestialCamera.transform.position;
			float num = Mathf.Sign(Vector3.Dot(lhs, rhs));
			float gizmoCenterAngle = GetGizmoCenterAngle();
			Quaternion quaternion = Quaternion.AngleAxis(Mathf.DeltaAngle(dragStartAngle, gizmoCenterAngle), selectedHandle.moveDirection * num);
			finalDragRotation = dragStartRotation * quaternion;
			pivotRotation.transform.localRotation = finalDragRotation;
			break;
		}
		case InteractMode.TRANSLATION:
			base.OnGizmoHandleDrag(dragStartLatitude, dragStartLongitude, dragLatitude, dragLongitude);
			break;
		}
	}

	public override void OnGizmoHandleDragEnd()
	{
		switch (interactMode)
		{
		case InteractMode.ROTATION:
			if (OnGAPGizmoRotate != null)
			{
				OnGAPGizmoRotate(finalDragRotation);
			}
			break;
		case InteractMode.TRANSLATION:
			base.OnGizmoHandleDragEnd();
			break;
		}
	}

	public void SetGizmoRotation(Quaternion newRotation)
	{
		pivotRotation.transform.localRotation = newRotation;
		rotation = newRotation;
	}

	public void SetGizmoIcon(VesselGroundLocation.GizmoIcon gizmoIcon)
	{
		if (currentGizmoIcon.HasValue)
		{
			gizmoIcons[(int)currentGizmoIcon.Value].SetActive(value: false);
		}
		if ((int)gizmoIcon < gizmoIcons.Count)
		{
			gizmoIcons[(int)gizmoIcon].SetActive(value: true);
		}
		currentGizmoIcon = gizmoIcon;
	}

	public void SetMode(InteractMode mode)
	{
		ClearMode(interactMode);
		switch (mode)
		{
		case InteractMode.ROTATION:
			gizmoRotate.SetActive(value: true);
			gizmoHandles = handlesRotation;
			break;
		case InteractMode.TRANSLATION:
			gizmoTranslate.SetActive(value: true);
			gizmoHandles = handlesTranslation;
			break;
		}
		interactMode = mode;
	}

	public override string GetFooterText()
	{
		return Localizer.Format("#autoLOC_8006105", currentGizmoIcon.Description(), longitude.ToString("f2"), latitude.ToString("f2"));
	}

	public void ClearMode(InteractMode mode)
	{
		switch (mode)
		{
		case InteractMode.ROTATION:
			gizmoRotate.SetActive(value: false);
			break;
		case InteractMode.TRANSLATION:
			gizmoTranslate.SetActive(value: false);
			break;
		}
	}

	public float GetGizmoCenterAngle()
	{
		Vector2 point = Vector2.zero;
		gapRef.GetMousePointOnCamera(Input.mousePosition, UIMasterController.Instance.mainCanvas.worldCamera, ref point);
		Vector2 vector = gapRef.DisplayCamera.WorldToScreenPoint(base.transform.position);
		float y = vector.y - point.y;
		float x = vector.x - point.x;
		return Mathf.Atan2(y, x) * 180f / (float)Math.PI;
	}
}

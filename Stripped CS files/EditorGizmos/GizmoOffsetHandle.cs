using UnityEngine;

namespace EditorGizmos;

public class GizmoOffsetHandle : GizmoHandle
{
	[SerializeField]
	public GizmoOffset host;

	[SerializeField]
	public Transform stretchSection;

	public Vector3 handlePos0;

	public Vector3 handleAxis0;

	public Camera refCamera;

	public Transform trf;

	public Vector2 mousePosAtGrabTime;

	public Vector2 mouseScreenAxis;

	public Vector2 screenAxis;

	public float pixel2WorldFactor;

	public float dOffset;

	public Callback<GizmoOffsetHandle, Vector3> onDragStart;

	public Callback<GizmoOffsetHandle, Vector3, float> onDrag;

	public Callback<GizmoOffsetHandle, Vector3, float> onDragEnded;

	public void Setup(GizmoOffset host, Callback<GizmoOffsetHandle, Vector3> onHandleDragStart, Callback<GizmoOffsetHandle, Vector3, float> onHandleDrag, Callback<GizmoOffsetHandle, Vector3, float> onHandleDragEnd, Camera referenceCamera)
	{
		BaseSetup();
		this.host = host;
		refCamera = referenceCamera;
		trf = base.transform;
		stretchSection.GetComponent<Renderer>().material = primaryRenderer.material;
		onDragStart = onHandleDragStart;
		onDrag = onHandleDrag;
		onDragEnded = onHandleDragEnd;
	}

	public override bool CanHover()
	{
		return !host.IsDragging;
	}

	public override void On_MouseEnter()
	{
	}

	public override void On_MouseDown()
	{
		handlePos0 = trf.position;
		handleAxis0 = handlePos0 - host.transform.position;
		mousePosAtGrabTime = Input.mousePosition;
		screenAxis = refCamera.WorldToScreenPoint(trf.position) - refCamera.WorldToScreenPoint(host.transform.position);
		pixel2WorldFactor = handleAxis0.magnitude / screenAxis.magnitude;
		onDragStart(this, handleAxis0.normalized);
	}

	public override void On_MouseDrag()
	{
		screenAxis = refCamera.WorldToScreenPoint(trf.position) - refCamera.WorldToScreenPoint(host.transform.position);
		mouseScreenAxis = (Vector2)Input.mousePosition - mousePosAtGrabTime;
		dOffset = Vector2.Dot(mouseScreenAxis, screenAxis.normalized) * pixel2WorldFactor;
		if (host.useGrid)
		{
			dOffset -= dOffset % host.SnapInterval;
		}
		onDrag(this, handleAxis0.normalized, dOffset);
	}

	public override void On_MouseUp()
	{
		onDragEnded(this, handleAxis0.normalized, dOffset);
	}

	public override void On_MouseExit()
	{
	}

	public bool IsFacingDirection(Vector3 direction)
	{
		return Vector3.Dot(trf.position - host.transform.position, direction) > 0f;
	}
}

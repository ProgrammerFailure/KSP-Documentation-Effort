using UniLinq;
using UnityEngine;

namespace EditorGizmos;

public class GizmoOffset : MonoBehaviour
{
	[SerializeField]
	public GizmoOffsetHandle[] handles;

	[SerializeField]
	public LookAtObject centerPivotController;

	[SerializeField]
	public ScreenSpaceObjectScaling ssScaling;

	public float gridSnapInterval = 0.2f;

	public float gridSnapIntervalFine = 0.1f;

	public bool useGrid;

	public Camera refCamera;

	public Transform host;

	public Quaternion rotOffset;

	public Callback<Vector3> onGizmoMove = delegate
	{
	};

	public Callback<Vector3> onGizmoMoved = delegate
	{
	};

	public bool isDragging;

	public Vector3 trfPos0;

	public Vector3 offset0;

	public Vector3 offset;

	public static Space coordSpace;

	public float GetScreenSize => ssScaling.GetScreenSize();

	public bool GetMouseOverGizmo => handles.Any((GizmoOffsetHandle h) => h.Hover || h.Drag);

	public float SnapInterval
	{
		get
		{
			if (!GameSettings.Editor_fineTweak.GetKey())
			{
				return gridSnapInterval;
			}
			return gridSnapIntervalFine;
		}
	}

	public bool IsDragging => isDragging;

	public Space CoordSpace => coordSpace;

	public static GizmoOffset Attach(Transform host, Quaternion rotOffset, Callback<Vector3> onMove, Callback<Vector3> onMoved, Camera refCamera = null)
	{
		GizmoOffset component = Object.Instantiate(AssetBase.GetPrefab("OffsetGizmo")).GetComponent<GizmoOffset>();
		component.transform.position = host.position;
		switch (coordSpace)
		{
		case Space.Self:
			component.transform.rotation = host.rotation * Quaternion.Inverse(rotOffset);
			break;
		case Space.World:
			component.transform.rotation = Quaternion.identity;
			break;
		}
		component.host = host;
		component.rotOffset = rotOffset;
		component.onGizmoMove = onMove;
		component.onGizmoMoved = onMoved;
		component.refCamera = refCamera;
		component.useGrid = GameSettings.VAB_USE_ANGLE_SNAP;
		GameEvents.onEditorSnapModeChange.Add(component.onEditorSnapChanged);
		return component;
	}

	public void Detach()
	{
		GameEvents.onEditorSnapModeChange.Remove(onEditorSnapChanged);
		Object.Destroy(base.gameObject);
	}

	public void Start()
	{
		refCamera = refCamera ?? Camera.main;
		centerPivotController.target = refCamera.transform;
		ssScaling.refCamera = refCamera;
		int i = 0;
		for (int num = handles.Length; i < num; i++)
		{
			handles[i].Setup(this, OnHandleMoveStart, OnHandleMove, OnHandleMoveEnd, refCamera);
		}
		offset0 = Vector3.zero;
	}

	public void Update()
	{
		if (!HighLogic.LoadedSceneIsFlight || !(host != null))
		{
			return;
		}
		base.transform.position = host.position;
		switch (coordSpace)
		{
		case Space.Self:
			base.transform.rotation = host.rotation;
			break;
		case Space.World:
			if (EVAConstructionModeController.Instance != null && EVAConstructionModeController.Instance.IsOpen && EVAConstructionModeController.Instance.evaEditor != null && EVAConstructionModeController.Instance.evaEditor.SelectedPart != null && EVAConstructionModeController.Instance.evaEditor.SelectedPart.vessel != null && EVAConstructionModeController.Instance.evaEditor.SelectedPart.vessel.rootPart != null)
			{
				base.transform.rotation = EVAConstructionModeController.Instance.evaEditor.SelectedPart.vessel.rootPart.transform.rotation;
			}
			break;
		}
	}

	public void LateUpdate()
	{
		int num = handles.Length;
		while (num-- > 0)
		{
			if (handles[num].IsFacingDirection(refCamera.transform.forward) && handles[num].gameObject.activeSelf)
			{
				handles[num].gameObject.SetActive(value: false);
			}
			else if (!handles[num].IsFacingDirection(refCamera.transform.forward) && !handles[num].gameObject.activeSelf)
			{
				handles[num].gameObject.SetActive(value: true);
			}
		}
	}

	public void OnHandleMoveStart(GizmoOffsetHandle h, Vector3 axis)
	{
		if (useGrid && coordSpace == Space.World)
		{
			SnapToGrid();
		}
		offset0 = Vector3.zero;
		trfPos0 = base.transform.position;
		isDragging = true;
		int i = 0;
		for (int num = handles.Length; i < num; i++)
		{
			GizmoHandle gizmoHandle = handles[i];
			if (!(h == gizmoHandle))
			{
				gizmoHandle.SetLock(lockSt: true);
			}
		}
	}

	public void OnHandleMove(GizmoOffsetHandle h, Vector3 axis, float distance)
	{
		offset = offset0 + axis * distance;
		base.transform.position = trfPos0 + offset;
		onGizmoMove(offset);
	}

	public void OnHandleMoveEnd(GizmoOffsetHandle h, Vector3 axis, float deltaAngle)
	{
		isDragging = false;
		int i = 0;
		for (int num = handles.Length; i < num; i++)
		{
			GizmoHandle gizmoHandle = handles[i];
			if (!(h == gizmoHandle))
			{
				gizmoHandle.SetLock(lockSt: false);
			}
		}
		onGizmoMoved(offset);
	}

	public void onEditorSnapChanged(bool useSnap)
	{
		useGrid = useSnap;
	}

	public void SetCoordSystem(Space space)
	{
		if (space != coordSpace)
		{
			switch (space)
			{
			case Space.Self:
				base.transform.rotation = host.rotation * Quaternion.Inverse(rotOffset);
				break;
			case Space.World:
				base.transform.rotation = Quaternion.identity;
				break;
			}
			offset0 = Vector3.zero;
			trfPos0 = base.transform.position;
			coordSpace = space;
		}
	}

	public void SnapToGrid()
	{
		Space space = coordSpace;
		if (space != 0 && space == Space.Self)
		{
			Vector3 localPosition = host.localPosition;
			Vector3 localPosition2 = new Vector3(Mathf.Round(localPosition.x / SnapInterval) * SnapInterval, Mathf.Round(localPosition.y / SnapInterval) * SnapInterval, Mathf.Round(localPosition.z / SnapInterval) * SnapInterval);
			host.localPosition = localPosition2;
			base.transform.position = host.position;
		}
		else
		{
			Vector3 localPosition = ((host.parent != null) ? (host.position - host.parent.position) : host.position);
			Vector3 localPosition2 = new Vector3(Mathf.Round(localPosition.x / SnapInterval) * SnapInterval, Mathf.Round(localPosition.y / SnapInterval) * SnapInterval, Mathf.Round(localPosition.z / SnapInterval) * SnapInterval);
			host.position = ((host.parent != null) ? (localPosition2 + host.parent.position) : localPosition2);
			base.transform.position = host.position;
		}
	}
}

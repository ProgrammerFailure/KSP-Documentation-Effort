using UniLinq;
using UnityEngine;

namespace EditorGizmos;

public class GizmoRotate : MonoBehaviour
{
	[SerializeField]
	public GizmoRotateHandle[] handles;

	public static Space coordSpace = Space.Self;

	public float gridSnapInterval = 15f;

	public float gridSnapIntervalFine = 5f;

	public bool useAngleSnap;

	public Camera refCamera;

	public Vector3 pivot;

	public Quaternion rot0;

	public Quaternion hostRot0;

	public Quaternion rotOffset;

	public Transform host;

	public Callback<Quaternion> onGizmoRotate = delegate
	{
	};

	public Callback<Quaternion> onGizmoRotated = delegate
	{
	};

	public bool isDragging;

	[SerializeField]
	public ScreenSpaceObjectScaling ssScaling;

	public Space CoordSpace => coordSpace;

	public float SnapDegrees
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

	public Quaternion HostRot0 => hostRot0;

	public bool IsDragging => isDragging;

	public float GetScreenSize => ssScaling.GetScreenSize();

	public bool GetMouseOverGizmo => handles.Any((GizmoRotateHandle h) => h.Hover || h.Drag);

	public static GizmoRotate Attach(Transform host, Vector3 pivotPoint, Quaternion rotOffset, Callback<Quaternion> onRotate, Callback<Quaternion> onRotated, Camera refCamera = null)
	{
		GizmoRotate component = Object.Instantiate(AssetBase.GetPrefab("RotateGizmo")).GetComponent<GizmoRotate>();
		component.transform.position = pivotPoint;
		component.host = host;
		component.onGizmoRotate = onRotate;
		component.onGizmoRotated = onRotated;
		component.refCamera = refCamera;
		component.useAngleSnap = GameSettings.VAB_USE_ANGLE_SNAP;
		component.hostRot0 = host.transform.rotation;
		component.rotOffset = rotOffset;
		switch (coordSpace)
		{
		case Space.Self:
			component.transform.rotation = host.rotation * Quaternion.Inverse(rotOffset);
			break;
		case Space.World:
			component.transform.rotation = Quaternion.identity;
			break;
		}
		GameEvents.onEditorSnapModeChange.Add(component.onAngleSnapChanged);
		return component;
	}

	public void Detach()
	{
		GameEvents.onEditorSnapModeChange.Remove(onAngleSnapChanged);
		Object.Destroy(base.gameObject);
	}

	public void Start()
	{
		refCamera = refCamera ?? Camera.main;
		ssScaling.refCamera = refCamera;
		int i = 0;
		for (int num = handles.Length; i < num; i++)
		{
			handles[i].Setup(this, OnHandleRotateStart, OnHandleRotate, OnHandleRotateEnd, refCamera);
		}
		rot0 = base.transform.rotation;
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

	public void OnHandleRotateStart(GizmoRotateHandle h, Vector3 axis)
	{
		if (useAngleSnap && coordSpace == Space.World)
		{
			SnapToGrid();
		}
		rot0 = base.transform.rotation;
		isDragging = true;
		if (host != null)
		{
			hostRot0 = host.transform.rotation;
		}
		int i = 0;
		for (int num = handles.Length; i < num; i++)
		{
			GizmoRotateHandle gizmoRotateHandle = handles[i];
			if (!(h == gizmoRotateHandle))
			{
				gizmoRotateHandle.SetLock(lockSt: true);
			}
		}
	}

	public void OnHandleRotate(GizmoRotateHandle h, Vector3 axis, float dAngle)
	{
		if (!float.IsNaN(dAngle))
		{
			if (coordSpace == Space.Self)
			{
				base.transform.rotation = Quaternion.AngleAxis(dAngle, axis) * rot0;
			}
			onGizmoRotate(Quaternion.AngleAxis(dAngle, axis));
		}
	}

	public void OnHandleRotateEnd(GizmoRotateHandle h, Vector3 axis, float deltaAngle)
	{
		isDragging = false;
		int i = 0;
		for (int num = handles.Length; i < num; i++)
		{
			GizmoRotateHandle gizmoRotateHandle = handles[i];
			if (!(h == gizmoRotateHandle))
			{
				gizmoRotateHandle.SetLock(lockSt: false);
			}
		}
		onGizmoRotated(Quaternion.AngleAxis(deltaAngle, axis));
	}

	public void onAngleSnapChanged(bool useSnap)
	{
		useAngleSnap = useSnap;
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
			rot0 = base.transform.rotation;
			coordSpace = space;
		}
	}

	public void SnapToGrid()
	{
		Vector3 eulerAngles = host.transform.rotation.eulerAngles;
		Vector3 euler = new Vector3(Mathf.Round(eulerAngles.x / SnapDegrees) * SnapDegrees, Mathf.Round(eulerAngles.y / SnapDegrees) * SnapDegrees, Mathf.Round(eulerAngles.z / SnapDegrees) * SnapDegrees);
		host.transform.rotation = Quaternion.Euler(euler);
	}
}

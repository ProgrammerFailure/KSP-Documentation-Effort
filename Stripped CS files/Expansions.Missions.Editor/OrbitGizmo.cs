using System;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class OrbitGizmo : ManeuverGizmoBase
{
	public delegate void HandlesUpdatedCallback(Vector3d dV, double double_0);

	public GameObject pointHandles;

	public GameObject globalHandles;

	public ManeuverGizmoHandle handleIncClockwise;

	public ManeuverGizmoHandle handleIncCounter;

	public ManeuverGizmoHandle handleEccIn;

	public ManeuverGizmoHandle handleEccOut;

	public ManeuverGizmoHandle handleSMA;

	public ManeuverGizmoHandle handleAntiSMA;

	public SpriteButton resetBtn;

	public SpriteButton pointModeBtn;

	public SpriteButton globalModeBtn;

	public float incMultiplier = 0.3f;

	public float smaMultiplier = 500f;

	public float eccMultiplier = 0.005f;

	public Callback OnOrbitReset = delegate
	{
	};

	public HandlesUpdatedCallback OnPointGizmoUpdated = delegate
	{
	};

	public HandlesUpdatedCallback OnGlobalGizmoUpdated = delegate
	{
	};

	public OrbitGizmoMode mode = OrbitGizmoMode.Point;

	public GAPCelestialBodyState_Orbit gapOrbit;

	public OrbitRendererBase.OrbitCastHit hit;

	public bool isDragging;

	public OrbitGizmoMode GizmoMode
	{
		get
		{
			if (!buttonMode)
			{
				return mode;
			}
			return OrbitGizmoMode.None;
		}
	}

	public override void Start()
	{
		base.Start();
		handleIncClockwise.OnHandleUpdate = OnIncClockwiseUpdate;
		handleIncCounter.OnHandleUpdate = OnIncCounterUpdate;
		handleEccIn.OnHandleUpdate = OnEccInUpdate;
		handleEccOut.OnHandleUpdate = OnEccOutUpdate;
		handleSMA.OnHandleUpdate = OnSMAUpdate;
		handleAntiSMA.OnHandleUpdate = OnAntiSMAUpdate;
		resetBtn.onClick.AddListener(OnResetButtonPress);
		pointModeBtn.onClick.AddListener(OnPointModeButtonPress);
		globalModeBtn.onClick.AddListener(OnGlobalModeButtonPress);
	}

	public OrbitGizmo Create(Vector3 position, Camera cam, Transform parent)
	{
		OrbitGizmo orbitGizmo = UnityEngine.Object.Instantiate(this, parent);
		orbitGizmo.transform.position = position;
		orbitGizmo.transform.rotation = Quaternion.identity;
		orbitGizmo.camera = cam;
		orbitGizmo.buttonMode = true;
		return orbitGizmo;
	}

	public void ChangeToPointMode()
	{
		OnPointModeButtonPress();
	}

	public void ChangeToGlobalMode()
	{
		OnGlobalModeButtonPress();
	}

	public override void OnProgradeUpdate(float value)
	{
		OnPointGizmoUpdated(Vector3d.forward * Math.Pow(Math.Abs(value), sensitivity) * multiplier * Math.Sign(value), double_0);
	}

	public override void OnRetrogradeUpdate(float value)
	{
		OnPointGizmoUpdated(-Vector3d.forward * Math.Pow(Math.Abs(value), sensitivity) * multiplier * Math.Sign(value), double_0);
	}

	public override void OnNormalUpdate(float value)
	{
		OnPointGizmoUpdated(Vector3d.up * Math.Pow(Math.Abs(value), sensitivity) * multiplier * Math.Sign(value), double_0);
	}

	public override void OnAntinormalUpdate(float value)
	{
		OnPointGizmoUpdated(-Vector3d.up * Math.Pow(Math.Abs(value), sensitivity) * multiplier * Math.Sign(value), double_0);
	}

	public override void OnRadialInUpdate(float value)
	{
		OnPointGizmoUpdated(-Vector3d.right * Math.Pow(Math.Abs(value), sensitivity) * multiplier * Math.Sign(value), double_0);
	}

	public override void OnRadialOutUpdate(float value)
	{
		OnPointGizmoUpdated(Vector3d.right * Math.Pow(Math.Abs(value), sensitivity) * multiplier * Math.Sign(value), double_0);
	}

	public void OnIncClockwiseUpdate(float value)
	{
		OnGlobalGizmoUpdated(-Vector3d.up * Math.Pow(Math.Abs(value), sensitivity) * incMultiplier * Math.Sign(value), double_0);
	}

	public void OnIncCounterUpdate(float value)
	{
		OnGlobalGizmoUpdated(Vector3d.up * Math.Pow(Math.Abs(value), sensitivity) * incMultiplier * Math.Sign(value), double_0);
	}

	public void OnSMAUpdate(float value)
	{
		OnGlobalGizmoUpdated(-Vector3d.forward * Math.Pow(Math.Abs(value), sensitivity) * smaMultiplier * Math.Sign(value), double_0);
	}

	public void OnAntiSMAUpdate(float value)
	{
		OnGlobalGizmoUpdated(Vector3d.forward * Math.Pow(Math.Abs(value), sensitivity) * smaMultiplier * Math.Sign(value), double_0);
	}

	public void OnEccInUpdate(float value)
	{
		OnGlobalGizmoUpdated(-Vector3d.right * Math.Pow(Math.Abs(value), sensitivity) * eccMultiplier * Math.Sign(value), double_0);
	}

	public void OnEccOutUpdate(float value)
	{
		OnGlobalGizmoUpdated(Vector3d.right * Math.Pow(Math.Abs(value), sensitivity) * eccMultiplier * Math.Sign(value), double_0);
	}

	public void OnResetButtonPress()
	{
		Debug.Log("Reset button click");
		OnOrbitReset();
	}

	public void OnPointModeButtonPress()
	{
		DeltaV = Vector3d.zero;
		pointHandles.SetActive(value: true);
		globalHandles.SetActive(value: false);
		buttonMode = false;
		mode = OrbitGizmoMode.Point;
	}

	public void OnGlobalModeButtonPress()
	{
		DeltaV = Vector3d.zero;
		pointHandles.SetActive(value: false);
		globalHandles.SetActive(value: true);
		buttonMode = false;
		mode = OrbitGizmoMode.Global;
	}

	public override void OnMouseDrag()
	{
		base.OnMouseDrag();
		isDragging = true;
		if (gapOrbit.GizmoDragCastHit(out hit))
		{
			double_0 = hit.UTatTA;
			if (mode == OrbitGizmoMode.Point)
			{
				OnPointGizmoUpdated(Vector3d.zero, double_0);
			}
			else
			{
				OnGlobalGizmoUpdated(Vector3d.zero, double_0);
			}
		}
	}

	public override void OnMouseUp()
	{
		base.OnMouseUp();
		isDragging = false;
		if (Input.GetMouseButtonUp(1) && !buttonMode)
		{
			ChangeToButtonMode();
		}
	}
}

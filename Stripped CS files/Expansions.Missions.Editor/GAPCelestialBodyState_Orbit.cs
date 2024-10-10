using System;
using ns23;
using ns9;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Expansions.Missions.Editor;

public class GAPCelestialBodyState_Orbit : GAPCelestialBodyState_Base
{
	public Callback OnOrbitReset = delegate
	{
	};

	public OrbitGizmo.HandlesUpdatedCallback OnPointGizmoUpdate = delegate
	{
	};

	public OrbitGizmo.HandlesUpdatedCallback OnGlobalGizmoUpdate = delegate
	{
	};

	public GAPOrbitRenderer orbitRenderer;

	public MapNode hoverNode;

	public MapNode userNode;

	public OrbitRendererBase.OrbitCastHit orbitHit;

	public bool hoverOrbit;

	public RaycastHit hit;

	public OrbitGizmo orbitGizmo;

	public Vector3 cameraPoint;

	public override void Init(GAPCelestialBody gapRef)
	{
		base.Init(gapRef);
		gapRef.StartingZoomValue = 0.8f;
		gapRef.TogglePQS(usePQS: false);
		gapRef.CelestialCamera.OverridePosition(45.0, 0.0);
		orbitRenderer = GAPOrbitRenderer.Create(gapRef);
		hoverNode = CreateMapNode("Node", orbitRenderer.orbitColor, 32, MapObject.ObjectType.Generic, OnUpdatePositionHover, OnUpdateVisibleHover, null);
		gapRef.Selector.containerFooter.SetActive(value: true);
	}

	public override void LateUpdate()
	{
		base.LateUpdate();
		hoverNode.NodeUpdate();
		if (userNode != null)
		{
			userNode.NodeUpdate();
		}
		if (!gapRef.CelestialCamera.LockDragging || (orbitGizmo != null && orbitGizmo.GizmoMode != OrbitGizmoMode.Point) || (orbitGizmo != null && orbitGizmo.isDragging))
		{
			UpdateGizmoPosition();
		}
	}

	public override void End()
	{
		if (orbitRenderer != null)
		{
			orbitRenderer.Destroy();
		}
		if (hoverNode != null)
		{
			UnityEngine.Object.Destroy(hoverNode.gameObject);
		}
		if (userNode != null)
		{
			UnityEngine.Object.Destroy(userNode.gameObject);
		}
		if (orbitGizmo != null)
		{
			orbitGizmo.Terminate();
		}
	}

	public override void LoadPlanet(CelestialBody body)
	{
		base.LoadPlanet(body);
		double maxMultiplier = MissionOrbit.GetEditorMaxOrbitRadius(body) / body.Radius * 2.5;
		gapRef.CelestialCamera.SetBoundries(3.5, maxMultiplier);
		gapRef.CelestialCamera.OverrideDistance(gapRef.Selector.sliderZoom.value);
		gapRef.Selector.sliderZoom.value = 1f;
		UpdateOrbit();
	}

	public override void UnloadPlanet()
	{
		base.UnloadPlanet();
		if (orbitGizmo != null)
		{
			orbitGizmo.Terminate();
		}
	}

	public override void OnClick(RaycastHit? hit)
	{
		if (hoverOrbit && Input.GetMouseButtonUp(0))
		{
			if (orbitGizmo == null)
			{
				orbitGizmo = MissionEditorMapView.OrbitGizmoPrefab.Create(Vector3.zero, ScaledCamera.Instance.cam, null);
				orbitGizmo.OnOrbitReset = OnOrbitReset;
				orbitGizmo.OnPointGizmoUpdated = OnPointGizmoUpdate;
				orbitGizmo.OnGlobalGizmoUpdated = OnGlobalGizmoUpdate;
				orbitGizmo.gapOrbit = this;
				orbitGizmo.ChangeToPointMode();
			}
			orbitGizmo.double_0 = orbitHit.UTatTA;
			UpdateGizmoPosition();
		}
	}

	public override void OnMouseOver(Vector2 cameraPoint)
	{
		hoverOrbit = false;
		this.cameraPoint = cameraPoint;
		if (gapRef.Raycast(cameraPoint, out hit, 1 << LayerMask.NameToLayer("MapFX")))
		{
			MouseRayEventsHandler component = hit.collider.GetComponent<MouseRayEventsHandler>();
			if (component != null)
			{
				if (Input.GetMouseButtonDown(0))
				{
					gapRef.CelestialCamera.LockDragging = true;
				}
				component.OnRayHit(hit);
			}
			else
			{
				hoverOrbit = orbitRenderer.OrbitCast(cameraPoint, ScaledCamera.Instance.cam, out orbitHit, 20f);
			}
		}
		else
		{
			hoverOrbit = orbitRenderer.OrbitCast(cameraPoint, ScaledCamera.Instance.cam, out orbitHit, 20f);
		}
		if (Input.GetMouseButtonUp(0))
		{
			gapRef.CelestialCamera.LockDragging = false;
		}
	}

	public override void OnDrag(PointerEventData.InputButton arg0, Vector2 arg1)
	{
		hoverOrbit = false;
	}

	public void UpdateOrbit()
	{
		if (orbitRenderer.OrbitLine != null)
		{
			orbitRenderer.DrawOrbit(isDirty: true);
		}
	}

	public void UpdateGizmoPosition()
	{
		if (!(orbitGizmo == null))
		{
			orbitGizmo.transform.position = ScaledSpace.LocalToScaledSpace(gapRef.BodyOrbit.getPositionAtUT(orbitGizmo.double_0));
			Vector3d xzy = gapRef.BodyOrbit.getRelativePositionAtUT(orbitGizmo.double_0).xzy;
			Vector3d xzy2 = gapRef.BodyOrbit.getOrbitalVelocityAtUT(orbitGizmo.double_0).xzy;
			orbitGizmo.transform.rotation = Quaternion.LookRotation(xzy2, Vector3d.Cross(-xzy, xzy2));
		}
	}

	public bool GizmoDragCastHit(out OrbitRendererBase.OrbitCastHit hitInfo)
	{
		return orbitRenderer.OrbitCast(cameraPoint, ScaledCamera.Instance.cam, out hitInfo, 60f);
	}

	public void ToggleMeanAnomalyIcon(bool toggleValue)
	{
		if (toggleValue)
		{
			if (userNode == null)
			{
				userNode = CreateMapNode("User", orbitRenderer.orbitColor, 32, MapObject.ObjectType.CelestialBody, OnUpdatePositionUser, OnUpdateVisibleUser, OnUpdateCaptionUser);
			}
		}
		else if (userNode != null)
		{
			UnityEngine.Object.Destroy(userNode.gameObject);
		}
	}

	public MapNode CreateMapNode(string nodeName, Color nodeColor, int size, MapObject.ObjectType objectType, Func<MapNode, Vector3d> onUpdatePositionFunction, Callback<MapNode, MapNode.IconData> onUpdateVisibleCallback, Callback<MapNode, MapNode.CaptionData> onUpdateCaptionCallback)
	{
		MapObject mObj = MapObject.Create(nodeName, nodeName, gapRef.BodyOrbit, objectType);
		bool hoverable = onUpdateCaptionCallback != null;
		MapNode mapNode = MapNode.Create(mObj, nodeColor, size, hoverable, pinnable: false, blocksInput: false, gapRef.transform);
		if (onUpdateVisibleCallback != null)
		{
			mapNode.OnUpdateVisible += onUpdateVisibleCallback;
		}
		if (onUpdatePositionFunction != null)
		{
			mapNode.OnUpdatePosition += onUpdatePositionFunction;
		}
		if (onUpdateCaptionCallback != null)
		{
			mapNode.OnUpdateCaption += onUpdateCaptionCallback;
		}
		mapNode.OnUpdateType += OnUpdateType;
		mapNode.Event_0 += OnUpdatePositionToUI;
		return mapNode;
	}

	public void OnUpdateVisibleUser(MapNode mn, MapNode.IconData iData)
	{
		iData.color = orbitRenderer.orbitColor * 0.5f + Color.white * 0.5f;
		iData.pixelSize = 36;
		iData.visible = true;
	}

	public void OnUpdateCaptionUser(MapNode mn, MapNode.CaptionData iData)
	{
		iData.Header = Localizer.Format("#autoLOC_8006119");
		iData.captionLine1 = (gapRef.BodyOrbit.epoch / gapRef.BodyOrbit.period).ToString("P1");
		iData.captionLine2 = "MNA: " + gapRef.BodyOrbit.meanAnomaly.ToString("F2");
	}

	public Vector3d OnUpdatePositionUser(MapNode mn)
	{
		return ScaledSpace.LocalToScaledSpace(gapRef.BodyOrbit.getPositionAtUT(0.0));
	}

	public void OnUpdateVisibleHover(MapNode mn, MapNode.IconData iData)
	{
		iData.color = orbitRenderer.orbitColor;
		iData.pixelSize = 32;
		iData.visible = hoverOrbit;
	}

	public Vector3d OnUpdatePositionHover(MapNode mn)
	{
		if (hoverOrbit)
		{
			return orbitHit.orbitPoint;
		}
		return Vector3d.zero;
	}

	public void OnUpdateType(MapNode mn, MapNode.TypeData tData)
	{
	}

	public Vector3 OnUpdatePositionToUI(MapNode n, Vector3d scaledSpacePos)
	{
		return MapViewCanvasUtil.ScaledToUISpacePos(scaledSpacePos, gapRef.displayImage.rectTransform, ScaledCamera.Instance.cam, ref n.VisualIconData.visible, MapNode.zSpaceEasing, MapNode.zSpaceMidpoint, MapNode.zSpaceUIStart, MapNode.zSpaceLength);
	}

	public static double CalculateBodyEditorAngle(CelestialBody body)
	{
		return body.rotationAngle - body.directRotAngle;
	}
}

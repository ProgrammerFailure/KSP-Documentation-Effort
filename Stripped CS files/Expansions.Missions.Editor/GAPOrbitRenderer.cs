using FinePrint.Utilities;
using ns23;
using UnityEngine;
using Vectrosity;

namespace Expansions.Missions.Editor;

public class GAPOrbitRenderer : OrbitRendererBase
{
	public GAPCelestialBody gapRef;

	public bool isInteractive;

	public string objectName;

	public MapNode.TypeData typeData;

	public bool objectIconVisible;

	public GAPCelestialBody GAPRef
	{
		get
		{
			return gapRef;
		}
		set
		{
			gapRef = value;
		}
	}

	public string ObjectName
	{
		get
		{
			return objectName;
		}
		set
		{
			objectName = value;
		}
	}

	public MapNode.TypeData TypeData
	{
		get
		{
			return typeData;
		}
		set
		{
			typeData = value;
		}
	}

	public override void Start()
	{
		layerMask = LayerMask.NameToLayer("Scaled Scenery");
		nodesParent = gapRef.transform;
		base.Start();
		if (!(gapRef != null))
		{
			return;
		}
		if (isInteractive)
		{
			ascNode.InputBlocking = false;
			descNode.InputBlocking = false;
			apNode.InputBlocking = false;
			peNode.InputBlocking = false;
			if (objectNode != null)
			{
				objectNode.Event_0 += OnUpdatePositionToUI;
			}
			ascNode.Event_0 += OnUpdatePositionToUI;
			descNode.Event_0 += OnUpdatePositionToUI;
			apNode.Event_0 += OnUpdatePositionToUI;
			peNode.Event_0 += OnUpdatePositionToUI;
			gapRef.Orbits.UpdateOrbit();
		}
		else
		{
			DrawOrbit(isDirty: true);
			orbitLine.active = objectIconVisible;
		}
		gapRef.Selector.transform.SetAsLastSibling();
	}

	public override void AttachNodeUIs(Transform parent)
	{
		if (isInteractive)
		{
			base.AttachNodeUIs(parent);
		}
		else
		{
			objectNode = AttachObjectNode(32);
		}
	}

	public override void LateUpdate()
	{
		if (!(driver != null))
		{
			return;
		}
		if (base.OrbitLine.rectTransform.parent == null)
		{
			if (isInteractive)
			{
				gapRef.Orbits.UpdateOrbit();
			}
			else
			{
				objectNode.NodeUpdate();
			}
		}
		base.LateUpdate();
	}

	public override void DrawSpline()
	{
		if (autoTextureOffset)
		{
			if (base.orbit.eccentricity < 1.0)
			{
				double num = 0.01745329238474369 - UtilMath.TwoPI;
				eccOffset = (base.orbit.eccentricAnomaly - num) % UtilMath.TwoPI / UtilMath.TwoPI;
				twkOffset = (float)eccOffset * MapView.GetEccOffset((float)eccOffset, (float)base.orbit.eccentricity, 4f);
				textureOffset = 1f - twkOffset;
			}
			else
			{
				textureOffset = 0f;
			}
		}
		orbitLine.ContinuousTextureOffset = textureOffset;
		orbitLine.Draw3D();
	}

	public static GAPOrbitRenderer Create(GameObject objRef, Camera orbitCam, Orbit orbit, bool isInteractive)
	{
		VectorLine.SetCamera3D(orbitCam);
		Color color = (isInteractive ? SystemUtilities.RandomColor(orbit.referenceBody.GetInstanceID(), 1f, 1f, 1f) : new Color(0.5f, 0.5f, 0.5f, 0.5f));
		GAPOrbitRenderer gAPOrbitRenderer = objRef.AddComponent<GAPOrbitRenderer>();
		gAPOrbitRenderer.isInteractive = isInteractive;
		gAPOrbitRenderer.drawNodes = true;
		gAPOrbitRenderer.isFocused = true;
		gAPOrbitRenderer.drawIcons = DrawIcons.OBJ_PE_AP;
		gAPOrbitRenderer.drawMode = DrawMode.REDRAW_AND_RECALCULATE;
		gAPOrbitRenderer.SetColor(color);
		gAPOrbitRenderer.autoTextureOffset = false;
		OrbitDriver orbitDriver = objRef.AddComponent<OrbitDriver>();
		orbitDriver.orbit = orbit;
		orbitDriver.orbitColor = gAPOrbitRenderer.GetOrbitColour();
		orbitDriver.Renderer = gAPOrbitRenderer;
		orbitDriver.enabled = false;
		gAPOrbitRenderer.driver = orbitDriver;
		return gAPOrbitRenderer;
	}

	public static GAPOrbitRenderer Create(GAPCelestialBody gapRef)
	{
		GAPOrbitRenderer gAPOrbitRenderer = Create(gapRef.gameObject, ScaledCamera.Instance.cam, gapRef.BodyOrbit, isInteractive: true);
		gAPOrbitRenderer.gapRef = gapRef;
		return gAPOrbitRenderer;
	}

	public void DrawOrbit(bool isDirty)
	{
		if (isDirty)
		{
			base.orbit.Init();
			if (MapView.fetch != null && MapView.MapIsEnabled)
			{
				MapView.fetch.UpdateMap();
			}
			if (isInteractive)
			{
				SetColor(SystemUtilities.RandomColor(base.orbit.referenceBody.GetInstanceID(), 1f, 1f, 1f));
			}
			DrawOrbit(DrawMode.REDRAW_AND_RECALCULATE);
		}
		else
		{
			DrawOrbit(DrawMode.REDRAW_ONLY);
		}
	}

	public Vector3 OnUpdatePositionToUI(MapNode n, Vector3d scaledSpacePos)
	{
		return MapViewCanvasUtil.ScaledToUISpacePos(scaledSpacePos, gapRef.displayImage.rectTransform, ScaledCamera.Instance.cam, ref n.VisualIconData.visible, MapNode.zSpaceEasing, MapNode.zSpaceMidpoint, MapNode.zSpaceUIStart, MapNode.zSpaceLength);
	}

	public MapNode AttachObjectNode(int size)
	{
		MapNode mapNode = MapNode.Create(MapObject.Create(objectName, objectName, base.orbit, typeData.oType), Color.white, size, hoverable: true, pinnable: false, blocksInput: false, gapRef.transform);
		mapNode.OnUpdateVisible += OnUpdateVisibleHover;
		mapNode.OnUpdatePosition += OnUpdatePosition;
		mapNode.OnUpdateCaption += OnUpdateCaption;
		mapNode.Event_0 += OnUpdatePositionToUI;
		mapNode.SetType(typeData);
		return mapNode;
	}

	public Vector3d OnUpdatePosition(MapNode mn)
	{
		return ScaledSpace.LocalToScaledSpace(mn.mapObject.orbit.getPositionAtUT(0.0));
	}

	public void OnUpdateVisibleHover(MapNode mn, MapNode.IconData iData)
	{
		iData.pixelSize = 32;
		iData.visible = objectIconVisible;
	}

	public void OnUpdateCaption(MapNode mn, MapNode.CaptionData iData)
	{
		iData.Header = mn.mapObject.DisplayName;
	}

	public void Display(bool value)
	{
		if (orbitLine != null)
		{
			orbitLine.active = value;
		}
		objectIconVisible = value;
	}

	public void Destroy()
	{
		Object.Destroy(driver);
		Object.Destroy(this);
		if (objectNode != null)
		{
			Object.Destroy(objectNode.gameObject);
		}
		if (isInteractive)
		{
			Object.Destroy(ascNode.gameObject);
			Object.Destroy(descNode.gameObject);
			Object.Destroy(apNode.gameObject);
			Object.Destroy(peNode.gameObject);
		}
	}
}

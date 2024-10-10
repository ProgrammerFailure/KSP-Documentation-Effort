using ns23;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class GAPSurfaceIcon
{
	public MapNode mapNode;

	public string name;

	public double latitude;

	public double longitude;

	public double altitude;

	public MapNode.TypeData typeData;

	public GAPCelestialBody gapRef;

	public bool visible;

	public GAPSurfaceIcon(string name, ref GAPCelestialBody gapRef, Transform parent, MapNode.TypeData typeData, double latitude, double longitude, double altitude)
	{
		this.name = name;
		this.typeData = typeData;
		this.longitude = longitude - gapRef.CelestialBody.directRotAngle;
		this.latitude = latitude;
		this.altitude = altitude;
		this.gapRef = gapRef;
		visible = true;
		mapNode = CreateMapNode(this.name, 32, parent);
	}

	public void OnUpdate()
	{
		mapNode.NodeUpdate();
	}

	public MapNode CreateMapNode(string name, int size, Transform parent)
	{
		MapNode obj = MapNode.Create(name, Color.white, size, hoverable: true, pinnable: false, blocksInput: false);
		obj.transform.SetParent(parent);
		obj.OnUpdateVisible += OnUpdateVisibleHover;
		obj.OnUpdatePosition += OnUpdatePosition;
		obj.OnUpdateCaption += OnUpdateCaption;
		obj.Event_0 += OnUpdatePositionToUI;
		obj.SetType(typeData);
		return obj;
	}

	public Vector3d OnUpdatePosition(MapNode mn)
	{
		return gapRef.CelestialBody.GetWorldSurfacePosition(latitude, longitude, altitude);
	}

	public void OnUpdateVisibleHover(MapNode mn, MapNode.IconData iData)
	{
		iData.pixelSize = 32;
		iData.visible = visible;
	}

	public void OnUpdateCaption(MapNode mn, MapNode.CaptionData iData)
	{
		iData.Header = mn.name;
	}

	public Vector3 OnUpdatePositionToUI(MapNode n, Vector3d worldPos)
	{
		Vector3d vector3d = gapRef.CelestialCamera.cam.WorldToScreenPoint(worldPos);
		RectTransform rectTransform = gapRef.displayImage.rectTransform;
		vector3d.x /= gapRef.DisplayTexture.width;
		vector3d.y /= gapRef.DisplayTexture.height;
		vector3d.z = 0.0;
		vector3d.x = vector3d.x * (double)rectTransform.sizeDelta.x - (double)rectTransform.sizeDelta.x * 0.5;
		vector3d.y = vector3d.y * (double)rectTransform.sizeDelta.y - (double)rectTransform.sizeDelta.y * 0.5;
		n.VisualIconData.visible = true;
		return vector3d;
	}

	public void Display(bool value)
	{
		visible = value;
		mapNode.NodeUpdate();
	}

	public void Destroy()
	{
		mapNode.Terminate();
	}
}

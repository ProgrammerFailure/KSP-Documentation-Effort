using UnityEngine;

namespace Expansions.Missions.Editor;

public class MissionEditorMapView : MapView
{
	public OrbitGizmo orbitGizmoPrefab;

	public new static MissionEditorMapView fetch { get; set; }

	public static OrbitGizmo OrbitGizmoPrefab => fetch.orbitGizmoPrefab;

	public override void Awake()
	{
		base.Awake();
		fetch = this;
		draw3Dlines = true;
		started = true;
		updateMap = true;
	}

	public static Camera CreateMapFXCamera(int depthOffset, float nearPlane, float farPlane)
	{
		return fetch.createMapFXCamera(depthOffset, nearPlane, farPlane);
	}
}

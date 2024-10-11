using System;
using System.Runtime.CompilerServices;
using KSP.UI.Screens.Mapview;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Expansions.Missions.Editor;

public class GAPCelestialBodyState_Orbit : GAPCelestialBodyState_Base
{
	public Callback OnOrbitReset;

	public OrbitGizmo.HandlesUpdatedCallback OnPointGizmoUpdate;

	public OrbitGizmo.HandlesUpdatedCallback OnGlobalGizmoUpdate;

	private GAPOrbitRenderer orbitRenderer;

	private MapNode hoverNode;

	private MapNode userNode;

	private OrbitRendererBase.OrbitCastHit orbitHit;

	private bool hoverOrbit;

	private RaycastHit hit;

	private OrbitGizmo orbitGizmo;

	private Vector3 cameraPoint;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GAPCelestialBodyState_Orbit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Init(GAPCelestialBody gapRef)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void End()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void LoadPlanet(CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void UnloadPlanet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnClick(RaycastHit? hit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnMouseOver(Vector2 cameraPoint)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnDrag(PointerEventData.InputButton arg0, Vector2 arg1)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateOrbit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void UpdateGizmoPosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool GizmoDragCastHit(out OrbitRendererBase.OrbitCastHit hitInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleMeanAnomalyIcon(bool toggleValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected MapNode CreateMapNode(string nodeName, Color nodeColor, int size, MapObject.ObjectType objectType, Func<MapNode, Vector3d> onUpdatePositionFunction, Callback<MapNode, MapNode.IconData> onUpdateVisibleCallback, Callback<MapNode, MapNode.CaptionData> onUpdateCaptionCallback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateVisibleUser(MapNode mn, MapNode.IconData iData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateCaptionUser(MapNode mn, MapNode.CaptionData iData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3d OnUpdatePositionUser(MapNode mn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateVisibleHover(MapNode mn, MapNode.IconData iData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3d OnUpdatePositionHover(MapNode mn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateType(MapNode mn, MapNode.TypeData tData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3 OnUpdatePositionToUI(MapNode n, Vector3d scaledSpacePos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double CalculateBodyEditorAngle(CelestialBody body)
	{
		throw null;
	}
}

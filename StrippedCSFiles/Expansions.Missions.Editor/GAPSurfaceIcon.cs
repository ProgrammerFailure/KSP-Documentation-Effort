using System.Runtime.CompilerServices;
using KSP.UI.Screens.Mapview;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class GAPSurfaceIcon
{
	private MapNode mapNode;

	private string name;

	private double latitude;

	private double longitude;

	private double altitude;

	private MapNode.TypeData typeData;

	private GAPCelestialBody gapRef;

	private bool visible;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GAPSurfaceIcon(string name, ref GAPCelestialBody gapRef, Transform parent, MapNode.TypeData typeData, double latitude, double longitude, double altitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected MapNode CreateMapNode(string name, int size, Transform parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3d OnUpdatePosition(MapNode mn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateVisibleHover(MapNode mn, MapNode.IconData iData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateCaption(MapNode mn, MapNode.CaptionData iData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Vector3 OnUpdatePositionToUI(MapNode n, Vector3d worldPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Display(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Destroy()
	{
		throw null;
	}
}

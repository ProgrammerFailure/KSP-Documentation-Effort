using System.Runtime.CompilerServices;
using FinePrint;
using KSP.UI.Screens.Mapview;
using UnityEngine;

public class SiteNode : MonoBehaviour
{
	public ISiteNode siteObject;

	public Waypoint wayPoint;

	public MapNode.SiteType siteType;

	private bool showNode;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SiteNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static SiteNode Spawn(ISiteNode siteNodeObject)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void SetupWaypoint(SiteNode siteNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private MapNode SetupMapNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnMapEntered()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnMapExited()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnSceneLoaded(GameScenes scn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateType(MapNode mn, MapNode.TypeData tData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateNodeIcon(MapNode n, MapNode.IconData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3d OnUpdateNodePosition(MapNode n)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateNodeCaption(MapNode mn, MapNode.CaptionData data)
	{
		throw null;
	}
}

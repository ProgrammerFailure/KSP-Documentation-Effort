using System.Runtime.CompilerServices;
using KSP.UI.Screens.Mapview;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class GAPOrbitRenderer : OrbitRendererBase
{
	private GAPCelestialBody gapRef;

	private bool isInteractive;

	private string objectName;

	private MapNode.TypeData typeData;

	private bool objectIconVisible;

	public GAPCelestialBody GAPRef
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public string ObjectName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public MapNode.TypeData TypeData
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GAPOrbitRenderer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void AttachNodeUIs(Transform parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void DrawSpline()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static GAPOrbitRenderer Create(GameObject objRef, Camera orbitCam, Orbit orbit, bool isInteractive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static GAPOrbitRenderer Create(GAPCelestialBody gapRef)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DrawOrbit(bool isDirty)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Vector3 OnUpdatePositionToUI(MapNode n, Vector3d scaledSpacePos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected MapNode AttachObjectNode(int size)
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

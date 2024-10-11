using System.Runtime.CompilerServices;
using Expansions.Missions;
using KSP.UI.Screens.Mapview;
using UnityEngine;

public class MapObject : MonoBehaviour
{
	public enum ObjectType
	{
		Null,
		Generic,
		CelestialBody,
		Vessel,
		ManeuverNode,
		Periapsis,
		Apoapsis,
		AscendingNode,
		DescendingNode,
		ApproachIntersect,
		CelestialBodyAtUT,
		PatchTransition,
		MENode,
		Site
	}

	public Transform trf;

	public Transform tgtRef;

	public Vessel vessel;

	public CelestialBody celestialBody;

	public ManeuverNode maneuverNode;

	public MENode missionsNode;

	public LaunchSite launchSite;

	public ObjectType type;

	public Orbit orbit;

	public IDiscoverable Discoverable;

	public MapNode uiNode;

	public string DisplayName;

	internal Vessel vesselRef;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MapObject()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MapObject Create(string name, string displayName, Orbit orbit, ManeuverNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MapObject Create(string name, string displayName, Transform tgtRef, Orbit orbit, ObjectType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MapObject Create(string name, string displayName, Orbit orbit, ObjectType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnLateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual string OnGetName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CelestialBody GetReferenceBody()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Terminate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnWillDestroy()
	{
		throw null;
	}
}

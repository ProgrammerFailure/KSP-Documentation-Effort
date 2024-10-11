using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI.Screens.Mapview;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Expansions.Missions.Editor;

public class GAPCelestialBodyState_Base
{
	public enum AdditionalEntity
	{
		Kerbal,
		Asteroid,
		Vessel,
		Flag,
		LaunchSite,
		Objective
	}

	protected GAPCelestialBody gapRef;

	protected AdditionalEntity[] entityTypes;

	internal DictionaryValueList<AdditionalEntity, List<GAPSurfaceIcon>> additionalIcons;

	internal DictionaryValueList<AdditionalEntity, List<GAPOrbitRenderer>> additionalOrbits;

	internal static Dictionary<AdditionalEntity, bool> displayEntity;

	internal List<GAPCelestialBody_SurfaceGizmo_Icon> additionalSurfaceGizmos;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GAPCelestialBodyState_Base()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Init(GAPCelestialBody gapRef)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void End()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void LoadPlanet(CelestialBody newCelestialBody)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UnloadPlanet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnClick(RaycastHit? hit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnClickUp(RaycastHit? hit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnMouseOver(Vector2 cameraPoint)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnDrag(PointerEventData.InputButton arg0, Vector2 arg1)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnDragEnd(RaycastHit? hit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadAdditionalInfo(CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GAPSurfaceIcon CreateSurfaceIcon(AdditionalEntity entityType, string name, MapNode.TypeData typeData, double latitude, double longitude, double altitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GAPOrbitRenderer CreateSimpleOrbit(AdditionalEntity entityType, string name, Orbit orbit, MapNode.TypeData typeData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CreateSurfaceAreaGizmo(double latitude, double longitude, double radius)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClearAdditionalInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateAdditionalInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnFilterButton(AdditionalEntity entityType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFilterButton_Kerbals()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFilterButton_Asteroids()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFilterButton_Comets()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFilterButton_Vessels()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFilterButton_Flags()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFilterButton_LaunchSites()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFilterButton_Objectives()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetFilter(AdditionalEntity entityType, bool displayIcons)
	{
		throw null;
	}
}

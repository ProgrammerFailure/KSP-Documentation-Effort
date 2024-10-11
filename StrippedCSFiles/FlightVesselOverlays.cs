using System.Runtime.CompilerServices;
using UnityEngine;

public class FlightVesselOverlays : MonoBehaviour
{
	public delegate bool ValidVesselOverride(Vessel vessel, bool checkEngine, bool checkLift);

	public EditorMarker_CoM CoMmarker;

	public EditorMarker_CoT CoTmarker;

	public EditorMarker_CoL CoLmarker;

	public DictionaryValueList<Vessel, EditorMarker_CoM> ComMarkers;

	public DictionaryValueList<Vessel, EditorMarker_CoT> CotMarkers;

	public DictionaryValueList<Vessel, EditorMarker_CoL> ColMarkers;

	public bool ComMarkersActive;

	public bool ColMarkersActive;

	public bool CotMarkersActive;

	public Camera overlayCamera;

	public ValidVesselOverride validVesselOverride;

	public static FlightVesselOverlays fetch;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FlightVesselOverlays()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselLoaded(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselUnloaded(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetCamera(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void TerminateComMarkers()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void TerminateColMarkers()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void TerminateCotMarkers()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddComMarker(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddColMarker(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddCotMarker(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ValidVesselType(Vessel vessel, bool checkEngine, bool checkLift)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleCoM()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleCoT()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleCoL()
	{
		throw null;
	}
}

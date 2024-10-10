using UnityEngine;
using UnityEngine.UI;

public class ConstructionVesselOverlays : MonoBehaviour
{
	public Button toggleCoMbtn;

	public Button toggleCoTbtn;

	public Button toggleCoLbtn;

	public static ConstructionVesselOverlays fetch;

	public void Awake()
	{
		if ((bool)fetch)
		{
			Object.Destroy(this);
		}
		else
		{
			fetch = this;
		}
	}

	public void Start()
	{
		if ((bool)FlightVesselOverlays.fetch)
		{
			toggleCoMbtn.onClick.AddListener(ToggleCoM);
			toggleCoTbtn.onClick.AddListener(ToggleCoT);
			toggleCoLbtn.onClick.AddListener(ToggleCoL);
		}
	}

	public void OnDisable()
	{
		if ((bool)FlightVesselOverlays.fetch)
		{
			FlightVesselOverlays.fetch.TerminateColMarkers();
			FlightVesselOverlays.fetch.TerminateCotMarkers();
			FlightVesselOverlays.fetch.TerminateComMarkers();
			FlightVesselOverlays.fetch.SetCamera(state: false);
		}
	}

	public void ToggleCoM()
	{
		if ((bool)FlightVesselOverlays.fetch)
		{
			FlightVesselOverlays.fetch.ToggleCoM();
		}
	}

	public void ToggleCoT()
	{
		if ((bool)FlightVesselOverlays.fetch)
		{
			FlightVesselOverlays.fetch.ToggleCoT();
		}
	}

	public void ToggleCoL()
	{
		if ((bool)FlightVesselOverlays.fetch)
		{
			FlightVesselOverlays.fetch.ToggleCoL();
		}
	}
}

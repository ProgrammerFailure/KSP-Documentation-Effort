using System.Collections.Generic;
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

	public void Awake()
	{
		if ((bool)fetch)
		{
			Object.Destroy(this);
			return;
		}
		fetch = this;
		if ((bool)FlightCamera.fetch && FlightCamera.fetch.cameras.Length != 0)
		{
			GameObject gameObject = FlightCamera.fetch.cameras[0].gameObject;
			GameObject gameObject2 = new GameObject("OverlayCam");
			gameObject2.transform.SetParent(gameObject.transform);
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.transform.localRotation = Quaternion.identity;
			overlayCamera = gameObject2.AddComponent<Camera>();
			overlayCamera.clearFlags = CameraClearFlags.Depth;
			overlayCamera.depth = 4f;
			overlayCamera.cullingMask = 1 << LayerMask.NameToLayer("PartsList_Icons");
			overlayCamera.gameObject.SetActive(value: false);
		}
	}

	public void Start()
	{
		ComMarkers = new DictionaryValueList<Vessel, EditorMarker_CoM>();
		CotMarkers = new DictionaryValueList<Vessel, EditorMarker_CoT>();
		ColMarkers = new DictionaryValueList<Vessel, EditorMarker_CoL>();
		GameEvents.onVesselLoaded.Add(OnVesselLoaded);
		GameEvents.onVesselUnloaded.Add(OnVesselUnloaded);
	}

	public void OnDestroy()
	{
		GameEvents.onVesselLoaded.Remove(OnVesselLoaded);
		GameEvents.onVesselUnloaded.Remove(OnVesselUnloaded);
	}

	public void Update()
	{
		if (!CotMarkersActive && !ComMarkersActive && !ColMarkersActive)
		{
			SetCamera(state: false);
		}
		else
		{
			SetCamera(state: true);
		}
	}

	public void OnDisable()
	{
		TerminateColMarkers();
		TerminateCotMarkers();
		TerminateComMarkers();
		SetCamera(state: false);
	}

	public void OnVesselLoaded(Vessel vessel)
	{
		if (CotMarkersActive || ComMarkersActive || ColMarkersActive)
		{
			if (ColMarkersActive && !ColMarkers.ContainsKey(vessel))
			{
				AddColMarker(vessel);
			}
			if (ComMarkersActive && !ComMarkers.ContainsKey(vessel))
			{
				AddComMarker(vessel);
			}
			if (CotMarkersActive && !CotMarkers.ContainsKey(vessel))
			{
				AddCotMarker(vessel);
			}
		}
	}

	public void OnVesselUnloaded(Vessel vessel)
	{
		if (ColMarkers.ContainsKey(vessel))
		{
			ColMarkers.Remove(vessel);
		}
		if (ComMarkers.ContainsKey(vessel))
		{
			ComMarkers.Remove(vessel);
		}
		if (CotMarkers.ContainsKey(vessel))
		{
			CotMarkers.Remove(vessel);
		}
	}

	public void SetCamera(bool state)
	{
		if (overlayCamera != null && overlayCamera.gameObject.activeInHierarchy != state)
		{
			overlayCamera.gameObject.SetActive(state);
		}
	}

	public void TerminateComMarkers()
	{
		int count = ComMarkers.ValuesList.Count;
		while (count-- > 0)
		{
			if (ComMarkers.ValuesList[count] != null)
			{
				ComMarkers.ValuesList[count].gameObject.DestroyGameObject();
			}
		}
		ComMarkers.Clear();
		ComMarkersActive = false;
	}

	public void TerminateColMarkers()
	{
		int count = ColMarkers.ValuesList.Count;
		while (count-- > 0)
		{
			if (ColMarkers.ValuesList[count] != null)
			{
				ColMarkers.ValuesList[count].gameObject.DestroyGameObject();
			}
		}
		ColMarkers.Clear();
		ColMarkersActive = false;
	}

	public void TerminateCotMarkers()
	{
		int count = CotMarkers.ValuesList.Count;
		while (count-- > 0)
		{
			if (CotMarkers.ValuesList[count] != null)
			{
				CotMarkers.ValuesList[count].gameObject.DestroyGameObject();
			}
		}
		CotMarkers.Clear();
		CotMarkersActive = false;
	}

	public void AddComMarker(Vessel vessel)
	{
		if (ValidVesselType(vessel, checkEngine: false, checkLift: false) && !(CoMmarker == null) && !ComMarkers.Contains(vessel))
		{
			EditorMarker_CoM editorMarker_CoM = Object.Instantiate(CoMmarker);
			editorMarker_CoM.rootPart = vessel.rootPart;
			editorMarker_CoM.Update();
			editorMarker_CoM.gameObject.SetActive(value: true);
			ComMarkers.Add(vessel, editorMarker_CoM);
		}
	}

	public void AddColMarker(Vessel vessel)
	{
		if (ValidVesselType(vessel, checkEngine: false, checkLift: true) && !(CoLmarker == null) && !ColMarkers.Contains(vessel))
		{
			EditorMarker_CoL editorMarker_CoL = Object.Instantiate(CoLmarker);
			editorMarker_CoL.rootPart = vessel.rootPart;
			editorMarker_CoL.Update();
			editorMarker_CoL.gameObject.SetActive(value: true);
			ColMarkers.Add(vessel, editorMarker_CoL);
		}
	}

	public void AddCotMarker(Vessel vessel)
	{
		if (ValidVesselType(vessel, checkEngine: true, checkLift: false) && !(CoTmarker == null) && !CotMarkers.Contains(vessel))
		{
			EditorMarker_CoT editorMarker_CoT = Object.Instantiate(CoTmarker);
			editorMarker_CoT.rootPart = vessel.rootPart;
			editorMarker_CoT.Update();
			editorMarker_CoT.gameObject.SetActive(value: true);
			CotMarkers.Add(vessel, editorMarker_CoT);
		}
	}

	public bool ValidVesselType(Vessel vessel, bool checkEngine, bool checkLift)
	{
		if (validVesselOverride != null)
		{
			return validVesselOverride(vessel, checkEngine, checkLift);
		}
		if (vessel.vesselType >= VesselType.Probe && vessel.vesselType <= VesselType.Base && vessel.vesselType != VesselType.const_11)
		{
			if (checkEngine && vessel.FindPartModuleImplementing<ModuleEngines>() == null)
			{
				return false;
			}
			if (checkLift && vessel.FindPartModuleImplementing<ModuleLiftingSurface>() == null)
			{
				return false;
			}
			return true;
		}
		return false;
	}

	public void ToggleCoM()
	{
		if (ComMarkersActive)
		{
			TerminateComMarkers();
		}
		else if (HighLogic.LoadedSceneIsFlight && !(FlightGlobals.ActiveVessel == null) && FlightGlobals.ActiveVessel.parts.Count != 0 && (bool)CoMmarker)
		{
			List<Vessel> vesselsLoaded = FlightGlobals.VesselsLoaded;
			for (int i = 0; i < vesselsLoaded.Count; i++)
			{
				AddComMarker(vesselsLoaded[i]);
			}
			ComMarkersActive = true;
		}
	}

	public void ToggleCoT()
	{
		if (CotMarkersActive)
		{
			TerminateCotMarkers();
		}
		else if (HighLogic.LoadedSceneIsFlight && !(FlightGlobals.ActiveVessel == null) && FlightGlobals.ActiveVessel.parts.Count != 0 && (bool)CoTmarker)
		{
			List<Vessel> vesselsLoaded = FlightGlobals.VesselsLoaded;
			for (int i = 0; i < vesselsLoaded.Count; i++)
			{
				AddCotMarker(vesselsLoaded[i]);
			}
			CotMarkersActive = true;
		}
	}

	public void ToggleCoL()
	{
		if (ColMarkersActive)
		{
			TerminateColMarkers();
		}
		else if (HighLogic.LoadedSceneIsFlight && !(FlightGlobals.ActiveVessel == null) && FlightGlobals.ActiveVessel.parts.Count != 0 && (bool)CoLmarker)
		{
			List<Vessel> vesselsLoaded = FlightGlobals.VesselsLoaded;
			for (int i = 0; i < vesselsLoaded.Count; i++)
			{
				AddColMarker(vesselsLoaded[i]);
			}
			ColMarkersActive = true;
		}
	}
}

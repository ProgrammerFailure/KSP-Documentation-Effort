using System.Collections.Generic;
using ns11;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ns32;

public class ScreenVesselMassInfo : MonoBehaviour
{
	[SerializeField]
	public TextMeshProUGUI activeVesselName_text;

	[SerializeField]
	public TextMeshProUGUI vesselMass_text;

	[SerializeField]
	public Button updateStats;

	[SerializeField]
	public RectTransform contentParent;

	[SerializeField]
	public GameObject partItemPrefab;

	[SerializeField]
	public List<ScreenVesselMassPartInfo> screenObjects;

	public bool resetScreenObjects;

	[SerializeField]
	public Vessel vessel;

	[SerializeField]
	public ShipConstruct ship;

	public bool noVessel = true;

	public string cached_autoLOC_901099;

	public void Awake()
	{
		updateStats.onClick.AddListener(UpdateList);
		screenObjects = new List<ScreenVesselMassPartInfo>();
		GameEvents.onVesselChange.Add(ChangeVessel);
		GameEvents.onEditorStarted.Add(UpdateList);
		GameEvents.onEditorPodPicked.Add(onEditorPodPicked);
		GameEvents.onEditorLoad.Add(onEditorLoad);
		GameEvents.onEditorRestoreState.Add(onEditorRestoreState);
		GameEvents.onVesselWasModified.Add(ChangeVessel);
		GameEvents.onEditorShipModified.Add(ChangeShip);
		GameEvents.onEditorShipCrewModified.Add(ChangeCrew);
		SceneManager.sceneLoaded += OnSceneLoaded;
		cached_autoLOC_901099 = Localizer.Format("#autoLOC_901099");
		UpdateList();
	}

	public void Start()
	{
	}

	public void OnDestroy()
	{
		updateStats.onClick.RemoveListener(UpdateList);
		ClearScreenObjects();
		GameEvents.onVesselChange.Remove(ChangeVessel);
		GameEvents.onEditorStarted.Remove(FindVessel);
		GameEvents.onEditorPodPicked.Remove(onEditorPodPicked);
		GameEvents.onEditorLoad.Remove(onEditorLoad);
		GameEvents.onEditorRestoreState.Remove(onEditorRestoreState);
		GameEvents.onVesselWasModified.Remove(ChangeVessel);
		GameEvents.onEditorShipModified.Remove(ChangeShip);
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	public void ClearScreenObjects()
	{
		for (int i = 0; i < screenObjects.Count; i++)
		{
			Object.Destroy(screenObjects[i].gameObject);
		}
		screenObjects.Clear();
		for (int j = 0; j < contentParent.childCount; j++)
		{
			contentParent.GetChild(j).gameObject.DestroyGameObject();
		}
		resetScreenObjects = false;
	}

	public void FindVessel()
	{
		ship = null;
		vessel = null;
		vesselMass_text.text = "";
		if (HighLogic.LoadedSceneIsEditor)
		{
			if ((bool)EditorLogic.fetch && EditorLogic.fetch.ship != null)
			{
				ship = EditorLogic.fetch.ship;
				activeVesselName_text.text = ship.shipName;
				ClearScreenObjects();
			}
		}
		else if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel != null)
		{
			vessel = FlightGlobals.ActiveVessel;
			activeVesselName_text.text = vessel.vesselName;
			ClearScreenObjects();
		}
		noVessel = ship == null && vessel == null;
		if (noVessel)
		{
			activeVesselName_text.text = cached_autoLOC_901099;
			ClearScreenObjects();
		}
	}

	public void UpdateList()
	{
		FindVessel();
		if (!noVessel)
		{
			if (resetScreenObjects)
			{
				ClearScreenObjects();
			}
			List<Part> list = null;
			if (ship != null)
			{
				list = ship.parts;
			}
			if (vessel != null)
			{
				list = vessel.parts;
			}
			for (int i = 0; i < list.Count; i++)
			{
				Part part = list[i];
				GameObject obj = Object.Instantiate(partItemPrefab);
				obj.transform.SetParent(contentParent);
				obj.transform.localScale = Vector3.one;
				obj.GetComponent<ScreenVesselMassPartInfo>().UpdateData(part, i == 0);
			}
			if (ship != null)
			{
				vesselMass_text.text = ship.GetTotalMass().ToString("0.000");
			}
			if (vessel != null)
			{
				vesselMass_text.text = vessel.GetTotalMass().ToString("0.000");
			}
		}
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (mode != LoadSceneMode.Additive)
		{
			UpdateList();
		}
	}

	public void onEditorRestoreState()
	{
		UpdateList();
	}

	public void onEditorLoad(ShipConstruct ship, CraftBrowserDialog.LoadType loadType)
	{
		UpdateList();
	}

	public void onEditorPodPicked(Part part)
	{
		UpdateList();
	}

	public void ChangeVessel(Vessel vessel)
	{
		UpdateList();
	}

	public void ChangeShip(ShipConstruct ship)
	{
		UpdateList();
	}

	public void ChangeCrew(VesselCrewManifest crew)
	{
		UpdateList();
	}
}

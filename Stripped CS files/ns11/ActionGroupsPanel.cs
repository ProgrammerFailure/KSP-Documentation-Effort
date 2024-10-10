using System.Collections.Generic;
using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

public class ActionGroupsPanel : MonoBehaviour
{
	public ToggleGroup overrideButtonGroup;

	public Toggle overrideButtonPrefab;

	public Button editActionGroupsButtonPrefab;

	public GameObject spacerPrefab;

	public Toggle[] agToggles;

	public Button editActionGroupsButton;

	public GameObject spacer;

	public List<GameObject> actionPanelObjects;

	public List<Selectable> selectables;

	public PointerEnterExitHandler pointerEnterExitHandler;

	public static bool actionGroupPanelOpen;

	public void Awake()
	{
		selectables = new List<Selectable>();
		pointerEnterExitHandler = GetComponent<PointerEnterExitHandler>();
	}

	public void OnEnable()
	{
		actionGroupPanelOpen = true;
	}

	public void OnDisable()
	{
		actionGroupPanelOpen = false;
	}

	public void Start()
	{
		ActionGroupsFlightController.Instance.GetActionGroupPanel(this);
		UpdateActionGroups();
		GameEvents.onVesselChange.Add(UpdateButtons);
		GameEvents.OnMapEntered.Add(UpdateButtons);
		GameEvents.OnMapExited.Add(UpdateButtons);
	}

	public void Update()
	{
		if (GameSettings.PAUSE.GetKeyUp() && base.gameObject.activeInHierarchy)
		{
			ActionGroupsApp.Instance.HideAppFromInput();
		}
	}

	public void OnDestroy()
	{
		GameEvents.onVesselChange.Remove(UpdateButtons);
		GameEvents.OnMapEntered.Remove(UpdateButtons);
		GameEvents.OnMapExited.Remove(UpdateButtons);
	}

	public void SetGroupName(Toggle toggle, string groupName, int groupIndex)
	{
		TMP_Text componentInChildren = toggle.transform.GetComponentInChildren<TMP_Text>();
		if (groupIndex > 0)
		{
			if (!string.IsNullOrEmpty(groupName))
			{
				componentInChildren.text = groupName;
				return;
			}
			componentInChildren.text = Localizer.Format("#autoLOC_6013001", groupIndex);
		}
		else
		{
			componentInChildren.text = Localizer.Format("#autoLOC_6013000");
		}
	}

	public void UpdateButtons(Vessel vessel)
	{
		if (vessel == null || vessel.OverrideGroupNames == null)
		{
			return;
		}
		int i;
		for (i = 0; i < agToggles.Length && i < vessel.OverrideGroupNames.Length + 1; i++)
		{
			string groupName = null;
			if (i > 0)
			{
				groupName = vessel.OverrideGroupNames[i - 1];
			}
			SetGroupName(agToggles[i], groupName, i);
		}
		for (; i < agToggles.Length; i++)
		{
			SetGroupName(agToggles[i], null, i);
		}
	}

	public void SelectOverride(int groupOverride, bool on)
	{
		Vessel activeVessel = FlightGlobals.ActiveVessel;
		if (on && activeVessel != null)
		{
			activeVessel.SetGroupOverride(groupOverride);
		}
	}

	public void UpdateButtons()
	{
		Vessel activeVessel = FlightGlobals.ActiveVessel;
		int num = 0;
		if (activeVessel != null)
		{
			num = activeVessel.GroupOverride;
		}
		if (agToggles != null)
		{
			for (int i = 0; i < agToggles.Length; i++)
			{
				agToggles[i].isOn = i == num;
			}
		}
		if (spacer != null && editActionGroupsButton != null)
		{
			if (!MapView.MapIsEnabled && !spacer.activeSelf)
			{
				spacer.SetActive(value: true);
				editActionGroupsButton.gameObject.SetActive(value: true);
			}
			else if (MapView.MapIsEnabled && spacer.activeSelf)
			{
				spacer.SetActive(value: false);
				editActionGroupsButton.gameObject.SetActive(value: false);
			}
		}
	}

	public void UpdateActionGroups()
	{
		if (actionPanelObjects != null && actionPanelObjects.Count > 0)
		{
			for (int i = 0; i < actionPanelObjects.Count; i++)
			{
				Object.Destroy(actionPanelObjects[i]);
			}
		}
		actionPanelObjects = new List<GameObject>();
		if (GameSettings.ADDITIONAL_ACTION_GROUPS)
		{
			agToggles = new Toggle[Vessel.NumOverrideGroups + 1];
			for (int j = 0; j < agToggles.Length; j++)
			{
				int groupOverride = j;
				agToggles[j] = Object.Instantiate(overrideButtonPrefab);
				agToggles[j].group = overrideButtonGroup;
				agToggles[j].transform.SetParent(overrideButtonGroup.transform, worldPositionStays: false);
				agToggles[j].onValueChanged.AddListener(delegate(bool on)
				{
					SelectOverride(groupOverride, on);
				});
				actionPanelObjects.Add(agToggles[j].gameObject);
			}
			UpdateButtons();
			UpdateButtons(FlightGlobals.ActiveVessel);
			spacer = Object.Instantiate(spacerPrefab);
			spacer.transform.SetParent(overrideButtonGroup.transform, worldPositionStays: false);
			actionPanelObjects.Add(spacer);
		}
		else
		{
			agToggles = new Toggle[1];
			agToggles[0] = Object.Instantiate(overrideButtonPrefab);
			agToggles[0].group = overrideButtonGroup;
			agToggles[0].transform.SetParent(overrideButtonGroup.transform, worldPositionStays: false);
			agToggles[0].onValueChanged.AddListener(delegate(bool on)
			{
				SelectOverride(0, on);
			});
			actionPanelObjects.Add(agToggles[0].gameObject);
			UpdateButtons();
			UpdateButtons(FlightGlobals.ActiveVessel);
			spacer = Object.Instantiate(spacerPrefab);
			spacer.transform.SetParent(overrideButtonGroup.transform, worldPositionStays: false);
			actionPanelObjects.Add(spacer);
		}
		editActionGroupsButton = Object.Instantiate(editActionGroupsButtonPrefab);
		editActionGroupsButton.transform.SetParent(overrideButtonGroup.transform, worldPositionStays: false);
		editActionGroupsButton.onClick.AddListener(ActionGroupsFlightController.Instance.SelectPanelActions);
		editActionGroupsButton.transform.GetComponentInChildren<TMP_Text>().text = Localizer.Format("#autoLOC_6011122");
		actionPanelObjects.Add(editActionGroupsButton.gameObject);
	}
}

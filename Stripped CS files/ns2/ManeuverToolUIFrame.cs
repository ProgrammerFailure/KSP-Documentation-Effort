using System;
using System.Collections.Generic;
using ns11;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns2;

public class ManeuverToolUIFrame : GenericAppFrame
{
	public delegate void OnActionDelegate(bool state);

	public delegate void OnErrorStateDelegate(bool state);

	public AppUIMemberDropdown transferTypeParent;

	public TMP_Dropdown transferTypeDropDown;

	public TMP_Dropdown positionDropDown;

	[SerializeField]
	public ToggleGroup timeReadoutGroup;

	[SerializeField]
	public ToggleGroup timeDisplayGroup;

	[SerializeField]
	public Toggle timeDisplayUT;

	[SerializeField]
	public Toggle timeDisplayOffsetTime;

	[SerializeField]
	public Toggle timeDisplayLong;

	[SerializeField]
	public Toggle timeDisplaySecs;

	[SerializeField]
	public TextMeshProUGUI transferTypeHeader;

	public AppUIInputPanel inputPanel;

	[SerializeField]
	public TextMeshProUGUI errorText;

	[SerializeField]
	public GameObject bottomSection;

	[SerializeField]
	public Button createManeuverBtn;

	[SerializeField]
	public Toggle createAlarmToggle;

	[SerializeField]
	public TextMeshProUGUI timeDisplay;

	[SerializeField]
	public TextMeshProUGUI dvDisplay;

	public ManeuverTool maneuverApp;

	[SerializeField]
	public TextMeshProUGUI helpText;

	public bool errorState;

	[SerializeField]
	public GameObject topPanel;

	[SerializeField]
	public Button btnTopPanel;

	[SerializeField]
	public UIPanelTransition topPanelTransition;

	public AppUIInputPanel topPanelData;

	[SerializeField]
	public TextMeshProUGUI topPanelHeader;

	[SerializeField]
	public TextMeshProUGUI topPanelCol1Header;

	[SerializeField]
	public TextMeshProUGUI topPanelCol2Header;

	[SerializeField]
	public TextMeshProUGUI topPanelCol3Header;

	public bool topPanelActive = true;

	public bool topPanelExtended;

	[SerializeField]
	public GameObject visualPanel;

	[SerializeField]
	public Button btnVisualPanel;

	[SerializeField]
	public UIPanelTransition visualPanelTransition;

	public bool visualPanelActive = true;

	public bool visualPanelExtended;

	[SerializeField]
	public ManeuverToolCBContainer planetContainerPrefab;

	[SerializeField]
	public List<ManeuverToolCBContainer> planetContainers;

	[SerializeField]
	public Transform sidePanelParentTransform;

	public Vector2 sourceBodyLocalPos = new Vector2(-126f, -168f);

	public Vector2 targetBodyLocalPos = new Vector2(94f, 154f);

	public Vector2 parentBodyLocalPos = new Vector2(-132f, 70f);

	public OnActionDelegate onSidePanelClicked;

	public OnErrorStateDelegate onErrorStateChanged;

	public bool ErrorState => errorState;

	public bool TopPanelTransitioning
	{
		get
		{
			if (topPanelTransition != null)
			{
				return topPanelTransition.Transitioning;
			}
			return false;
		}
	}

	public bool TopPanelActive
	{
		get
		{
			if (topPanelActive)
			{
				return btnTopPanel.interactable;
			}
			return false;
		}
	}

	public bool TopPanelExtended => topPanelExtended;

	public bool VisualPanelTransitioning
	{
		get
		{
			if (visualPanelTransition != null)
			{
				return visualPanelTransition.Transitioning;
			}
			return false;
		}
	}

	public bool VisualPanelActive
	{
		get
		{
			if (visualPanelActive)
			{
				return btnVisualPanel.interactable;
			}
			return false;
		}
	}

	public bool VisualPanelExtended => visualPanelExtended;

	public void AddOnSidePanelClicked(OnActionDelegate method)
	{
		if (onSidePanelClicked == null)
		{
			onSidePanelClicked = method;
		}
		else
		{
			onSidePanelClicked = (OnActionDelegate)Delegate.Combine(onSidePanelClicked, method);
		}
	}

	public void RemoveOnSidePanelClicked(OnActionDelegate method)
	{
		if (onSidePanelClicked == method)
		{
			onSidePanelClicked = null;
		}
		else
		{
			onSidePanelClicked = (OnActionDelegate)Delegate.Remove(onSidePanelClicked, method);
		}
	}

	public void AddOnErrorStateChanged(OnErrorStateDelegate method)
	{
		if (onErrorStateChanged == null)
		{
			onErrorStateChanged = method;
		}
		else
		{
			onErrorStateChanged = (OnErrorStateDelegate)Delegate.Combine(onErrorStateChanged, method);
		}
	}

	public void RemoveOnErrorStateChanged(OnErrorStateDelegate method)
	{
		if (onErrorStateChanged == method)
		{
			onErrorStateChanged = null;
		}
		else
		{
			onErrorStateChanged = (OnErrorStateDelegate)Delegate.Remove(onErrorStateChanged, method);
		}
	}

	public void Start()
	{
		CreatePlanets();
		if (timeReadoutGroup != null && timeDisplayUT != null && timeDisplayOffsetTime != null)
		{
			timeDisplayUT.group = timeReadoutGroup;
			timeDisplayOffsetTime.group = timeReadoutGroup;
			timeDisplayUT.isOn = true;
			timeDisplayUT.onValueChanged.AddListener(OnTimeDisplayUTToggle);
			OnTimeDisplayUTToggle(value: true);
		}
		if (timeDisplayGroup != null && timeDisplayLong != null && timeDisplaySecs != null)
		{
			timeDisplayLong.group = timeDisplayGroup;
			timeDisplaySecs.group = timeDisplayGroup;
			timeDisplayLong.isOn = true;
			timeDisplaySecs.onValueChanged.AddListener(OnTimeDisplaySecToggle);
			OnTimeDisplaySecToggle(value: false);
		}
		if (createManeuverBtn != null)
		{
			createManeuverBtn.onClick.AddListener(OnCreateManeuverBtn);
		}
		if (btnTopPanel != null)
		{
			btnTopPanel.onClick.AddListener(OnBtnTopPanelClick);
		}
		SetTopPanelState(topPanelActive);
		if (btnVisualPanel != null)
		{
			btnVisualPanel.onClick.AddListener(OnBtnSidePanelClick);
		}
		SetVisualPanelState(visualPanelActive);
	}

	public new void OnDestroy()
	{
		if (createManeuverBtn != null)
		{
			createManeuverBtn.onClick.RemoveListener(OnCreateManeuverBtn);
		}
		if (btnTopPanel != null)
		{
			btnTopPanel.onClick.RemoveListener(OnBtnTopPanelClick);
		}
		if (btnVisualPanel != null)
		{
			btnVisualPanel.onClick.RemoveListener(OnBtnSidePanelClick);
		}
		if (planetContainers != null)
		{
			for (int i = 0; i < planetContainers.Count; i++)
			{
				UnityEngine.Object.Destroy(planetContainers[i]);
			}
		}
		if (sidePanelParentTransform != null)
		{
			int childCount = sidePanelParentTransform.childCount;
			while (childCount-- > 0)
			{
				UnityEngine.Object.Destroy(sidePanelParentTransform.GetChild(childCount).gameObject);
			}
		}
	}

	public void Update()
	{
	}

	public void CreatePlanets()
	{
		planetContainers = new List<ManeuverToolCBContainer>();
		if (!(PSystemManager.Instance == null) && !(PSystemManager.Instance.systemPrefab == null))
		{
			if (PSystemManager.Instance.systemPrefab.rootBody == null)
			{
				Debug.LogError("PSystemManager: systemPrefab root body is null!");
			}
			else
			{
				AddPlanetRecursively(PSystemManager.Instance.systemPrefab.rootBody, 20000f);
			}
		}
		else
		{
			Debug.LogError("PSystemManager: systemPrefab is null!");
		}
	}

	public ManeuverToolCBContainer AddPlanetRecursively(PSystemBody body, float offset_pos)
	{
		if (body.scaledVersion == null)
		{
			Debug.LogError("PSystemBody (" + body.name + "): scaledVersion is null!");
			return null;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(body.scaledVersion);
		UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<ScaledSpaceFader>());
		UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<MaterialSetDirection>());
		UnityEngine.Object.DestroyImmediate(GameObject.Find(gameObject.name + "/Atmosphere"));
		UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<SphereCollider>());
		float container_height = 130f;
		float scale = 1f;
		ManeuverToolCBContainer maneuverToolCBContainer = UnityEngine.Object.Instantiate(planetContainerPrefab);
		maneuverToolCBContainer.Setup(sidePanelParentTransform.gameObject, body.celestialBody.bodyName, body.celestialBody.displayName, gameObject, scale, container_height, offset_pos);
		offset_pos += 200f;
		planetContainers.Add(maneuverToolCBContainer);
		int count = body.children.Count;
		for (int i = 0; i < count; i++)
		{
			AddPlanetRecursively(body.children[i], offset_pos);
			offset_pos += 200f;
		}
		maneuverToolCBContainer.Hide();
		return maneuverToolCBContainer;
	}

	public void SetTopPanelState(bool state)
	{
		topPanelActive = state;
		if (!state)
		{
			if (topPanel != null)
			{
				if (topPanelTransition != null && topPanelExtended)
				{
					topPanelTransition.Transition("In");
					topPanelExtended = false;
				}
				btnTopPanel.interactable = false;
			}
		}
		else
		{
			btnTopPanel.interactable = true;
		}
	}

	public void TransitionTopPanelImmediate()
	{
		if (topPanelTransition != null)
		{
			int index = 0;
			topPanelTransition.GetState(topPanelExtended ? "Out" : "In", out index);
			topPanelTransition.TransitionImmediate(index);
		}
	}

	public void OnBtnTopPanelClick()
	{
		if (topPanelTransition != null)
		{
			if (topPanelExtended)
			{
				topPanelTransition.Transition("In");
				topPanelExtended = false;
			}
			else
			{
				topPanelTransition.Transition("Out");
				topPanelExtended = true;
			}
		}
	}

	public void OnBtnSidePanelClick()
	{
		if (!(visualPanelTransition != null))
		{
			return;
		}
		if (visualPanelExtended)
		{
			visualPanelTransition.Transition("In");
			visualPanelExtended = false;
			if (onSidePanelClicked != null)
			{
				onSidePanelClicked(state: false);
			}
			return;
		}
		visualPanelTransition.Transition("Out", delegate
		{
			if (onSidePanelClicked != null)
			{
				onSidePanelClicked(state: true);
			}
		});
		visualPanelExtended = true;
	}

	public void SetVisualPanelState(bool state)
	{
		visualPanelActive = state;
		if (!state)
		{
			if (visualPanel != null)
			{
				if (visualPanelTransition != null && visualPanelExtended)
				{
					visualPanelTransition.Transition("In");
					visualPanelExtended = false;
				}
				btnVisualPanel.interactable = false;
			}
		}
		else
		{
			btnVisualPanel.interactable = true;
		}
	}

	public void TransitionVisualPanelImmediate()
	{
		if (visualPanelTransition != null)
		{
			int index = 0;
			visualPanelTransition.GetState(visualPanelExtended ? "Out" : "In", out index);
			visualPanelTransition.TransitionImmediate(index);
		}
	}

	public void OnTimeDisplayUTToggle(bool value)
	{
		if (maneuverApp != null)
		{
			maneuverApp.ChangeTime(value, timeDisplaySecs.isOn);
		}
	}

	public void OnTimeDisplaySecToggle(bool value)
	{
		if (maneuverApp != null)
		{
			maneuverApp.ChangeTime(timeDisplayUT.isOn, value);
		}
	}

	public void OnCreateManeuverBtn()
	{
		if (maneuverApp != null)
		{
			maneuverApp.CreateManeuver(createAlarmToggle != null && createAlarmToggle.isOn);
		}
	}

	public void UpdateTimeText(string text)
	{
		if (timeDisplay != null)
		{
			timeDisplay.text = text;
		}
	}

	public void UpdateDVText(string text)
	{
		if (dvDisplay != null)
		{
			dvDisplay.text = text;
		}
	}

	public void SetHelpString(string text)
	{
		if (helpText != null)
		{
			helpText.text = Localizer.Format(text);
		}
	}

	public void SetErrorState(bool state, string text, AppUI_Data data, bool toggleInputPanel, Callback onDataChanged)
	{
		errorText.text = text;
		if (toggleInputPanel && inputPanel != null)
		{
			if (state)
			{
				if (inputPanel.SetupComplete)
				{
					inputPanel.ReleaseData();
				}
			}
			else if (!inputPanel.SetupComplete)
			{
				inputPanel.Setup(data, onDataChanged);
			}
		}
		if (inputPanel != null)
		{
			inputPanel.SetErrorState(state);
		}
		errorText.gameObject.SetActive(state);
		if (bottomSection != null)
		{
			bottomSection.SetActive(!state);
		}
		bool num = errorState;
		errorState = state;
		if (num != errorState && onErrorStateChanged != null)
		{
			onErrorStateChanged(state);
		}
	}

	public void SetHeaderText(string text)
	{
		if (transferTypeHeader != null)
		{
			transferTypeHeader.text = text;
		}
	}

	public void SetCalculationState(TransferDataBase.ManeuverCalculationState state, int percentage)
	{
		switch (state)
		{
		case TransferDataBase.ManeuverCalculationState.waiting:
			SetCreateManeuverButton(interactable: false);
			break;
		case TransferDataBase.ManeuverCalculationState.calculating:
			SetCreateManeuverButton(interactable: false);
			UpdateDVText(Localizer.Format("#autoLOC_6002695", percentage));
			break;
		case TransferDataBase.ManeuverCalculationState.failed:
			SetCreateManeuverButton(interactable: false);
			break;
		case TransferDataBase.ManeuverCalculationState.complete:
			SetCreateManeuverButton(interactable: true);
			break;
		}
	}

	public void SetCreateManeuverButton(bool interactable)
	{
		if (createManeuverBtn != null)
		{
			createManeuverBtn.interactable = interactable;
		}
	}

	public void SetTopPanelHeader(string topHeader, string col1Header, string col2Header, string col3Header)
	{
		if (topPanelHeader != null)
		{
			topPanelHeader.text = topHeader;
		}
		if (topPanelCol1Header != null)
		{
			topPanelCol1Header.text = col1Header;
		}
		if (topPanelCol2Header != null)
		{
			topPanelCol2Header.text = col2Header;
		}
		if (topPanelCol3Header != null)
		{
			topPanelCol3Header.text = col3Header;
		}
	}

	public ManeuverToolCBContainer ActivatePlanet(string bodyName, Vector2 position, float scale)
	{
		ManeuverToolCBContainer maneuverToolCBContainer = FindPlanet(bodyName);
		if (maneuverToolCBContainer != null)
		{
			maneuverToolCBContainer.Show(position, scale);
		}
		return maneuverToolCBContainer;
	}

	public void DeactivatePlanet(string bodyName)
	{
		ManeuverToolCBContainer maneuverToolCBContainer = FindPlanet(bodyName);
		if (maneuverToolCBContainer != null)
		{
			DeactivatePlanet(maneuverToolCBContainer);
		}
	}

	public void DeactivatePlanet(ManeuverToolCBContainer planet)
	{
		if (planet != null)
		{
			planet.Hide();
		}
	}

	public ManeuverToolCBContainer FindPlanet(string bodyName)
	{
		ManeuverToolCBContainer result = null;
		for (int i = 0; i < planetContainers.Count; i++)
		{
			if (planetContainers[i].name == bodyName)
			{
				result = planetContainers[i];
				break;
			}
		}
		return result;
	}

	public GameObject AddObjectToVisualizerWindow(GameObject prefab)
	{
		return AddObjectToVisualizerWindow(prefab, Vector2.zero, Vector3.one);
	}

	public GameObject AddObjectToVisualizerWindow(GameObject prefab, Vector2 localPosition, Vector3 localScale)
	{
		if (prefab != null && sidePanelParentTransform != null)
		{
			GameObject obj = UnityEngine.Object.Instantiate(prefab, sidePanelParentTransform, worldPositionStays: false);
			obj.transform.localPosition = localPosition;
			obj.transform.localScale = localScale;
			return obj;
		}
		return null;
	}

	public bool RemoveObjectFromVisualizerWindow(string name)
	{
		Transform transform = sidePanelParentTransform.Find(name);
		if (transform != null)
		{
			UnityEngine.Object.Destroy(transform.gameObject);
			return true;
		}
		return false;
	}
}

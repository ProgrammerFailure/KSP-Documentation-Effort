using ns9;

namespace Expansions.Missions.Editor;

[MEGUI_VesselPartSelect]
public class MEGUIParameterVesselPartSelector : MEGUIParameterVessel
{
	public int vesselIndex;

	public GAPVesselDisplay vesselDisplay;

	public Part selectedPart;

	public MEGUIParameterVesselDropdownList vesselDropdownList;

	public MEGUIParameterLabel partNameLabel;

	public VesselPartIDPair FieldValue
	{
		get
		{
			return (VesselPartIDPair)field.GetValue();
		}
		set
		{
			MissionEditorHistory.PushUndoAction(this, OnHistoryValueChange);
			field.SetValue(value);
		}
	}

	public void SetFieldValues(uint partId, uint vesselID, string partName)
	{
		VesselPartIDPair vesselPartIDPair = new VesselPartIDPair();
		vesselPartIDPair.partID = partId;
		vesselPartIDPair.VesselID = vesselID;
		vesselPartIDPair.partName = partName;
		FieldValue = vesselPartIDPair;
		UpdateNodeBodyUI();
	}

	public new void Awake()
	{
		base.Awake();
		if (HighLogic.LoadedSceneIsMissionBuilder)
		{
			GameEvents.Mission.onVesselSituationChanged.Add(onVesselSituationChanged);
			GameEvents.Mission.onMissionLoaded.Add(onVesselSituationChanged);
		}
	}

	public new void OnDestroy()
	{
		base.OnDestroy();
		GameEvents.Mission.onVesselSituationChanged.Remove(onVesselSituationChanged);
		GameEvents.Mission.onMissionLoaded.Remove(onVesselSituationChanged);
	}

	public void onVesselSituationChanged()
	{
		RefreshUI();
	}

	public override void Setup(string name)
	{
		base.Setup(name);
		vesselDropdownList = subParameters["VesselID"] as MEGUIParameterVesselDropdownList;
		vesselDropdownList.SetCraftOnly();
		vesselDropdownList.dropdownList.onValueChanged.AddListener(OnVesselValueChange);
		vesselIndex = mission.GetSituationsIndexByPart(FieldValue.partID);
		partNameLabel = subParameters["partName"] as MEGUIParameterLabel;
		partNameLabel.title.text = name;
		if (vesselIndex < 0)
		{
			vesselIndex = 0;
		}
		LoadSelectedPart();
		RefreshUI();
	}

	public void LoadSelectedPart()
	{
		vesselIndex = mission.GetSituationsIndexByPart(FieldValue.partID);
		if (vesselIndex < 0)
		{
			vesselIndex = 0;
		}
		MissionCraft missionCraft = null;
		if (base.VesselList.Count > 0 && base.VesselList.Count > vesselIndex)
		{
			missionCraft = mission.GetMissionCraftByFileName(base.VesselList[vesselIndex].craftFile);
		}
		if (missionCraft == null)
		{
			return;
		}
		ShipConstruct shipConstruct = MissionEditorLogic.Instance.LoadVessel(missionCraft);
		if (shipConstruct == null)
		{
			return;
		}
		for (int i = 0; i < shipConstruct.parts.Count; i++)
		{
			if (shipConstruct.parts[i].persistentId == FieldValue.partID)
			{
				selectedPart = shipConstruct.parts[i];
			}
		}
	}

	public override void ResetDefaultValue(string value)
	{
		uint result = 0u;
		if (uint.TryParse(value, out result))
		{
			SetFieldValues(result, base.VesselList[vesselIndex].persistentId, Localizer.Format("#autoLOC_8001004"));
		}
	}

	public override void RefreshUI()
	{
		base.RefreshUI();
		vesselIndex = mission.GetSituationsIndexByVessel(FieldValue.VesselID);
		if (vesselIndex < 0)
		{
			vesselIndex = 0;
		}
		vesselDropdownList.dropdownList.value = vesselIndex;
		if (base.VesselList.Count > 0 && base.VesselList.Count > vesselIndex)
		{
			partNameLabel.labelText.text = (base.VesselList[vesselIndex].playerCreated ? Localizer.Format("#autoLOC_8001005") : Localizer.Format(FieldValue.partName));
		}
		else
		{
			partNameLabel.labelText.text = Localizer.Format("#autoLOC_8001005");
		}
	}

	public void UpdateSelectedPartInDisplay()
	{
		if (vesselDisplay != null && vesselIndex < base.VesselList.Count)
		{
			vesselDisplay.SetupVessel(mission.GetMissionCraftByFileName(base.VesselList[vesselIndex].craftFile), base.VesselList[vesselIndex], this);
			vesselDisplay.ChangeSelectedPart(FieldValue.partID);
		}
	}

	public override void DisplayGAP()
	{
		base.DisplayGAP();
		vesselDisplay = MissionEditorLogic.Instance.actionPane.GAPInitialize<GAPVesselDisplay>();
		vesselIndex = mission.GetSituationsIndexByVessel(FieldValue.VesselID);
		if (vesselIndex < 0)
		{
			vesselIndex = 0;
		}
		if (vesselIndex < base.VesselList.Count)
		{
			UpdateSelectedPartInDisplay();
		}
		else
		{
			vesselDisplay.SetupVessel(null, null, this);
		}
	}

	public void OnVesselValueChange(int value)
	{
		vesselIndex = value;
		SetFieldValues(0u, base.VesselList[vesselIndex].persistentId, Localizer.Format("#autoLOC_8001004"));
		UpdateSelectedPartInDisplay();
		RefreshUI();
	}

	public override void OnChangePartSelection(Part part)
	{
		selectedPart = part;
		if (part != null)
		{
			SetFieldValues(part.persistentId, base.VesselList[vesselIndex].persistentId, part.partInfo.title);
		}
		else
		{
			SetFieldValues(0u, base.VesselList[vesselIndex].persistentId, Localizer.Format("#autoLOC_8001004"));
		}
		UpdateNodeBodyUI();
		RefreshUI();
	}

	public Part GetSelectedPart()
	{
		return selectedPart;
	}

	public override void OnNextVessel()
	{
		if (base.VesselList.Count > 0)
		{
			vesselIndex = (vesselIndex + 1) % base.VesselList.Count;
		}
		else
		{
			vesselIndex = 0;
		}
		UpdateSelectedPartInDisplay();
	}

	public override void OnPrevVessel()
	{
		if (base.VesselList.Count > 0)
		{
			vesselIndex = ((vesselIndex - 1 < 0) ? (base.VesselList.Count - 1) : (vesselIndex - 1));
		}
		else
		{
			vesselIndex = 0;
		}
		UpdateSelectedPartInDisplay();
	}

	public void OnHistoryValueChange(ConfigNode data, HistoryType type)
	{
		VesselPartIDPair vesselPartIDPair = new VesselPartIDPair();
		vesselPartIDPair.Load(data);
		SetFieldValues(vesselPartIDPair.partID, vesselPartIDPair.VesselID, vesselPartIDPair.partName);
		LoadSelectedPart();
		UpdateSelectedPartInDisplay();
		RefreshUI();
	}

	public override ConfigNode GetState()
	{
		ConfigNode configNode = new ConfigNode();
		configNode.AddValue("pinned", isPinned);
		FieldValue.Save(configNode);
		return configNode;
	}
}

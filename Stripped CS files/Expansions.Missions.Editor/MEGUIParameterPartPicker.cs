using System.Collections.Generic;
using System.Reflection;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[MEGUI_PartPicker]
public class MEGUIParameterPartPicker : MEGUIParameter, IMEHistoryTarget
{
	public TextMeshProUGUI selectedPartsText;

	public Button openPartPickerButton;

	public MEPartSelectorBrowser partSelectorPrefab;

	public GAPPartPicker gapPartPicker;

	public MethodInfo getExcludedParts;

	public MethodInfo updatePartnerExcludedPartsFilter;

	public List<string> FieldValue
	{
		get
		{
			return field.GetValue() as List<string>;
		}
		set
		{
			field.SetValue(value);
		}
	}

	public Dictionary<string, List<string>> ExcludedParts
	{
		get
		{
			if (getExcludedParts == null)
			{
				return null;
			}
			return getExcludedParts.Invoke(field.host, null) as Dictionary<string, List<string>>;
		}
	}

	public Color SelectedPartsColor => ((MEGUI_PartPicker)field.Attribute).SelectedPartsColor;

	public string DialogTitle => ((MEGUI_PartPicker)field.Attribute).DialogTitle;

	public override void Setup(string name)
	{
		title.text = name;
		selectedPartsText.text = Localizer.Format("#autoLOC_8100315", FieldValue.Count);
		openPartPickerButton.onClick.AddListener(OnPartPickerOpen);
		getExcludedParts = field.host.GetType().GetMethod(((MEGUI_PartPicker)field.Attribute).getExcludedPartsFilter, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		if (getExcludedParts != null && (getExcludedParts.ReturnType != typeof(Dictionary<string, List<string>>) || getExcludedParts.ContainsGenericParameters))
		{
			getExcludedParts = null;
		}
	}

	public override void ResetDefaultValue(string value)
	{
		string[] array = value.Split(',');
		if (array.Length != 0)
		{
			FieldValue.Clear();
			FieldValue.AddRange(array);
		}
	}

	public override void RefreshUI()
	{
		selectedPartsText.text = Localizer.Format("#autoLOC_8100315", FieldValue.Count);
	}

	public void OnPartPickerOpen()
	{
		if (gapPartPicker != null && !MissionEditorLogic.Instance.actionPane.IsGAPLocked)
		{
			gapPartPicker.Setup(this, OnPartSelectionChange);
		}
	}

	public override void DisplayGAP()
	{
		base.DisplayGAP();
		gapPartPicker = MissionEditorLogic.Instance.actionPane.GAPInitialize<GAPPartPicker>();
		gapPartPicker.Setup(this, OnPartSelectionChange);
	}

	public void OnPartSelectionChange(AvailablePart part, bool status)
	{
		MissionEditorHistory.PushUndoAction(this, OnHistoryValueChange);
		if (status)
		{
			FieldValue.Add(part.name);
		}
		else
		{
			FieldValue.Remove(part.name);
		}
		selectedPartsText.text = Localizer.Format("#autoLOC_8100315", FieldValue.Count);
	}

	public void UpdateDisplayedParts()
	{
		if (base.IsSelected)
		{
			gapPartPicker.Setup(this, OnPartSelectionChange);
		}
	}

	public override ConfigNode GetState()
	{
		ConfigNode configNode = new ConfigNode();
		for (int i = 0; i < FieldValue.Count; i++)
		{
			configNode.AddValue("SelectedParts", FieldValue[i]);
		}
		return configNode;
	}

	public void OnHistoryValueChange(ConfigNode data, HistoryType type)
	{
		FieldValue = data.GetValuesList("SelectedParts");
		RefreshUI();
		if (base.IsSelected)
		{
			UpdateDisplayedParts();
			return;
		}
		if (updatePartnerExcludedPartsFilter == null)
		{
			updatePartnerExcludedPartsFilter = field.host.GetType().GetMethod(((MEGUI_PartPicker)field.Attribute).updatePartnerExcludedPartsFilter, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}
		if (updatePartnerExcludedPartsFilter != null)
		{
			updatePartnerExcludedPartsFilter.Invoke(field.host, null);
		}
	}
}

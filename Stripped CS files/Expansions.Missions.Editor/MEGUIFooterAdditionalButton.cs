using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class MEGUIFooterAdditionalButton : MEGUIParameter
{
	public TMP_Dropdown additionalParametrsDropdown;

	public TMP_Dropdown removeParametersDropdown;

	public BaseAPFieldList fieldList;

	public List<MEGUIDropDownItem> addList;

	public List<MEGUIDropDownItem> removeList;

	public override bool IsInteractable
	{
		get
		{
			if (additionalParametrsDropdown.interactable)
			{
				return removeParametersDropdown.interactable;
			}
			return false;
		}
		set
		{
			additionalParametrsDropdown.interactable = value;
			removeParametersDropdown.interactable = value;
		}
	}

	public MEGUIParameter Create(BaseAPFieldList fieldList, Transform parent)
	{
		MEGUIFooterAdditionalButton mEGUIFooterAdditionalButton = Object.Instantiate(this);
		mEGUIFooterAdditionalButton.transform.SetParent(parent);
		mEGUIFooterAdditionalButton.transform.localScale = Vector3.one;
		if (fieldList != null && fieldList.Count > 0)
		{
			mEGUIFooterAdditionalButton.module = fieldList[0].host as IMENodeDisplay;
		}
		mEGUIFooterAdditionalButton.fieldList = fieldList;
		mEGUIFooterAdditionalButton.Setup("Additional");
		return mEGUIFooterAdditionalButton;
	}

	public override void Setup(string name)
	{
		addList = new List<MEGUIDropDownItem>();
		addList.Add(new MEGUIDropDownItem("", "", ""));
		removeList = new List<MEGUIDropDownItem>();
		removeList.Add(new MEGUIDropDownItem("", "", ""));
		int count = fieldList.Count;
		while (count-- > 0)
		{
			if (!module.HasSAPParameter(fieldList[count].name))
			{
				addList.Add(new MEGUIDropDownItem(fieldList[count].name, fieldList[count].name, fieldList[count].guiName));
			}
			else
			{
				removeList.Add(new MEGUIDropDownItem(fieldList[count].name, fieldList[count].name, fieldList[count].guiName));
			}
		}
		additionalParametrsDropdown.ClearOptions();
		additionalParametrsDropdown.AddOptions(addList.GetDisplayStrings());
		additionalParametrsDropdown.value = 0;
		additionalParametrsDropdown.onValueChanged.AddListener(OnAdditionalValueChanged);
		removeParametersDropdown.ClearOptions();
		removeParametersDropdown.AddOptions(removeList.GetDisplayStrings());
		removeParametersDropdown.value = 0;
		removeParametersDropdown.onValueChanged.AddListener(OnRemovevalueChanged);
	}

	public void OnAdditionalValueChanged(int value)
	{
		if (value > 0)
		{
			module.AddParameterToSAP(addList[value].key);
			MissionEditorLogic.Instance.actionPane.SAPRefreshNodeParameters();
			module.UpdateNodeBodyUI();
			Setup("Additional");
		}
	}

	public void OnRemovevalueChanged(int value)
	{
		if (value > 0)
		{
			module.RemoveParameterFromSAP(removeList[value].key);
			MissionEditorLogic.Instance.actionPane.SAPRefreshNodeParameters();
			module.UpdateNodeBodyUI();
			Setup("Additional");
		}
	}
}

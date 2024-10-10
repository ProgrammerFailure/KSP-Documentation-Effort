using System;
using System.Collections.Generic;
using UnityEngine;

namespace Expansions.Missions.Editor;

[MEGUI_ParameterSwitchCompound]
public class MEGUIParameterSwitchCompound : MEGUICompoundParameter
{
	public MEGUIParameterDropdownList dropDownSelector;

	public BaseAPField selectorEnum;

	public string activeControlName;

	public Enum listenum;

	public List<string> valuesToExclude;

	public List<string> activeControlNames;

	public override void Setup(string name, object value)
	{
		activeControlName = "none";
		subParameters = new Dictionary<string, MEGUIParameter>();
		BaseAPFieldList baseAPFieldList = new BaseAPFieldList(value);
		valuesToExclude = new List<string>();
		activeControlNames = new List<string>();
		if (field != null && field.Attribute != null && !string.IsNullOrEmpty(((MEGUI_ParameterSwitchCompound)field.Attribute).excludeParamFields))
		{
			valuesToExclude = new List<string>(((MEGUI_ParameterSwitchCompound)field.Attribute).excludeParamFields.Split(','));
		}
		for (int i = 0; i < baseAPFieldList.Count; i++)
		{
			if (!(baseAPFieldList[i].Attribute.GetType() == typeof(MEGUI_ParameterSwitchCompound_KeyField)))
			{
				continue;
			}
			selectorEnum = baseAPFieldList[i];
			listenum = (Enum)selectorEnum.GetValue();
			MEGUIParameter value2 = MEGUIParametersController.Instance.GetControl(typeof(MEGUI_Dropdown)).Create(selectorEnum, base.transform);
			subParameters.Add(selectorEnum.name, value2);
			dropDownSelector = subParameters[selectorEnum.name] as MEGUIParameterDropdownList;
			if (valuesToExclude != null)
			{
				dropDownSelector.SetExcludedOptions(valuesToExclude);
			}
			dropDownSelector.dropdownList.onValueChanged.AddListener(OnControlToDisplaySet);
			dropDownSelector.SetTooltipText(base.Tooltip);
			dropDownSelector.SetTooltipActive(!string.IsNullOrEmpty(base.Tooltip));
			string[] names = Enum.GetNames(baseAPFieldList[i].FieldType);
			int j = 0;
			for (int num = names.Length; j < num; j++)
			{
				if (!valuesToExclude.Contains(names[j]))
				{
					activeControlNames.AddUnique(names[j]);
				}
			}
			break;
		}
		if (dropDownSelector != null)
		{
			for (int k = 0; k < baseAPFieldList.Count; k++)
			{
				if (!(baseAPFieldList[k].Attribute.GetType() != typeof(MEGUI_ParameterSwitchCompound_KeyField)))
				{
					continue;
				}
				MEGUIParameter control = MEGUIParametersController.Instance.GetControl(baseAPFieldList[k].Attribute.GetType());
				if (control != null)
				{
					MEGUIParameter mEGUIParameter = control.Create(baseAPFieldList[k], base.transform);
					mEGUIParameter.isSelectable = mEGUIParameter.isSelectable && subParametersSelectable;
					if (!subParametersSelectable && mEGUIParameter.selectedIndicator != null)
					{
						mEGUIParameter.selectedIndicator.gameObject.SetActive(value: false);
					}
					if (baseAPFieldList[k].name == listenum.ToString())
					{
						activeControlName = baseAPFieldList[k].name;
					}
					else
					{
						mEGUIParameter.gameObject.SetActive(value: false);
					}
					subParameters.Add(baseAPFieldList[k].name, mEGUIParameter);
				}
			}
		}
		else
		{
			Debug.LogError("Unable to instantiate the KeyField Dropdown");
		}
	}

	public void OnControlToDisplaySet(int ddlIndex)
	{
		string text = activeControlNames[ddlIndex];
		if (activeControlName != "none" && subParameters.ContainsKey(activeControlName))
		{
			subParameters[activeControlName].gameObject.SetActive(value: false);
		}
		if (text != "none" && subParameters.ContainsKey(text))
		{
			subParameters[text].Display();
		}
		activeControlName = text;
	}

	public override void Display()
	{
		base.Display();
		if (subParameters.ContainsKey(activeControlName))
		{
			subParameters[activeControlName].Display();
		}
	}
}

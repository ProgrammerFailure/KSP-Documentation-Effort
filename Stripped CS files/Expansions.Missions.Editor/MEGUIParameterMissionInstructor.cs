using ns9;
using UnityEngine;

namespace Expansions.Missions.Editor;

[MEGUI_MissionInstructor]
public class MEGUIParameterMissionInstructor : MEGUICompoundParameter
{
	public GAPPrefabDisplay gapKerbal;

	public GameObject prefabToDisplay;

	public MEGUIParameterDropdownList dropDownInstructors;

	public MEGUIParameterCheckbox checkboxVintageSuit;

	public bool duringSetup;

	public MissionInstructor FieldValue
	{
		get
		{
			return (MissionInstructor)field.GetValue();
		}
		set
		{
			field.SetValue(value);
		}
	}

	public override void Setup(string name)
	{
		base.Setup(name);
		title.text = name;
		dropDownInstructors = subParameters["instructorName"] as MEGUIParameterDropdownList;
		dropDownInstructors.dropdownList.onValueChanged.AddListener(OnParameterValueChanged);
		checkboxVintageSuit = subParameters["vintageSuit"] as MEGUIParameterCheckbox;
		checkboxVintageSuit.toggle.onValueChanged.AddListener(onVintageSuitChanged);
		duringSetup = true;
		OnParameterValueChanged(0);
		duringSetup = false;
	}

	public void OnParameterValueChanged(int value)
	{
		if (!(dropDownInstructors.SelectedKey == "Jeb") && !(dropDownInstructors.SelectedKey == "Bob") && !(dropDownInstructors.SelectedKey == "Bill") && !(dropDownInstructors.SelectedKey == "Val"))
		{
			checkboxVintageSuit.gameObject.SetActive(value: false);
		}
		else
		{
			checkboxVintageSuit.gameObject.SetActive(value: true);
		}
		FieldValue.instructorName = dropDownInstructors.SelectedValue.ToString();
		UpdateNodeBodyUI();
		if (base.HasGAP && !duringSetup)
		{
			RefreshGapPrefab();
		}
	}

	public void onVintageSuitChanged(bool value)
	{
		UpdateNodeBodyUI();
		if (base.HasGAP)
		{
			RefreshGapPrefab();
		}
	}

	public GameObject SetGAPKerbal()
	{
		GameObject gameObject = null;
		if (!string.IsNullOrEmpty(dropDownInstructors.SelectedKey))
		{
			string text = dropDownInstructors.SelectedValue.ToString();
			text = text.Replace("Instructor_", "GAPKerbal_");
			text = text.Replace("Strategy_", "GAPKerbal_");
			if (!(dropDownInstructors.SelectedKey == "Jeb") && !(dropDownInstructors.SelectedKey == "Bob") && !(dropDownInstructors.SelectedKey == "Bill"))
			{
				if (dropDownInstructors.SelectedKey == "Val")
				{
					text = "GAPKerbal_FemaleVeteran" + (checkboxVintageSuit.FieldValue ? "_Vintage" : "");
				}
			}
			else
			{
				text = "GAPKerbal_MaleVeteran" + (checkboxVintageSuit.FieldValue ? "_Vintage" : "");
			}
			gameObject = MissionsUtils.MEPrefab("Prefabs/" + text + ".prefab");
			if ((gameObject != null && dropDownInstructors.SelectedKey == "Jeb") || dropDownInstructors.SelectedKey == "Bob" || dropDownInstructors.SelectedKey == "Bill" || dropDownInstructors.SelectedKey == "Val")
			{
				KerbalInstructorBase component = gameObject.gameObject.GetComponent<KerbalInstructorBase>();
				if (component != null)
				{
					switch (dropDownInstructors.SelectedKey)
					{
					case "Val":
						component.CharacterName = Localizer.Format("#autoLOC_20827");
						break;
					case "Bob":
						component.CharacterName = Localizer.Format("#autoLOC_20819");
						break;
					case "Bill":
						component.CharacterName = Localizer.Format("#autoLOC_20811");
						break;
					case "Jeb":
						component.CharacterName = Localizer.Format("#autoLOC_20803");
						break;
					}
				}
			}
		}
		return gameObject;
	}

	public override void DisplayGAP()
	{
		base.DisplayGAP();
		gapKerbal = MissionEditorLogic.Instance.actionPane.GAPInitialize<GAPPrefabDisplay>();
		RefreshGapPrefab();
	}

	public void RefreshGapPrefab()
	{
		base.DisplayGAP();
		gapKerbal = MissionEditorLogic.Instance.actionPane.GAPInitialize<GAPPrefabDisplay>();
		prefabToDisplay = SetGAPKerbal();
		gapKerbal.Setup(prefabToDisplay, 1f);
	}
}

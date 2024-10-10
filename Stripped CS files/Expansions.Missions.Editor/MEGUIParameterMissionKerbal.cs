using Expansions.Serenity;
using UnityEngine;

namespace Expansions.Missions.Editor;

[MEGUI_MissionKerbal]
public class MEGUIParameterMissionKerbal : MEGUICompoundParameter
{
	public GAPPrefabDisplay gapKerbal;

	public GameObject prefabToDisplay;

	public MEGUIParameterDropdownList dropDownKerbals;

	public MEGUIParameterDropdownList dropDownKerbalType;

	public MissionKerbal FieldValue
	{
		get
		{
			return (MissionKerbal)field.GetValue();
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
		dropDownKerbals = subParameters["kerbal"] as MEGUIParameterDropdownList;
		dropDownKerbals.dropdownList.onValueChanged.AddListener(OnParameterValueChanged);
		dropDownKerbalType = subParameters["typeToShow"] as MEGUIParameterDropdownList;
		FieldValue.statusToShow = ((MEGUI_MissionKerbal)field.Attribute).statusToShow;
		FieldValue.showAllRosterStatus = ((MEGUI_MissionKerbal)field.Attribute).showAllRosterStatus;
		FieldValue.showStrandedOnly = ((MEGUI_MissionKerbal)field.Attribute).showStranded;
	}

	public override void Display()
	{
		base.Display();
		int itemIndex = dropDownKerbalType.GetItemIndex(FieldValue.TypeToShow);
		if (itemIndex != -1)
		{
			dropDownKerbalType.FieldValue = itemIndex;
			dropDownKerbalType.RebuildDropDown();
		}
		dropDownKerbals.RebuildDropDown();
	}

	public void OnParameterValueChanged(int value)
	{
		if (FieldValue.Kerbal == null)
		{
			FieldValue.AnyValid = true;
		}
		UpdateNodeBodyUI();
		if (base.HasGAP)
		{
			RefreshGapPrefab();
		}
	}

	public GameObject SetGAPKerbal()
	{
		GameObject result = null;
		if (FieldValue.Kerbal != null)
		{
			string text = ((!FieldValue.Kerbal.veteran) ? ((FieldValue.Kerbal.gender == ProtoCrewMember.Gender.Male) ? "GAPKerbal_Male" : "GAPKerbal_Female") : ((FieldValue.Kerbal.gender == ProtoCrewMember.Gender.Male) ? "GAPKerbal_MaleVeteran" : "GAPKerbal_FemaleVeteran"));
			if (FieldValue.Kerbal.suit == ProtoCrewMember.KerbalSuit.Vintage)
			{
				text += "_Vintage";
			}
			if (FieldValue.Kerbal.suit == ProtoCrewMember.KerbalSuit.Future)
			{
				text = ((FieldValue.Kerbal.gender == ProtoCrewMember.Gender.Male) ? "GAPKerbal_Male_Future" : "GAPKerbal_Female_Future");
				return SerenityUtils.SerenityPrefab("Prefabs/" + text + ".prefab");
			}
			if (FieldValue.Kerbal.suit == ProtoCrewMember.KerbalSuit.Slim)
			{
				text += "_SlimSuit";
			}
			result = MissionsUtils.MEPrefab("Prefabs/" + text + ".prefab");
		}
		return result;
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

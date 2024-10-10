using System.Collections;
using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Actions;

public class ActionVesselActionGroup : ActionModule
{
	[MEGUI_Dropdown(onControlSetupComplete = "actionGroupControlComplete", guiName = "#autoLOC_900525")]
	public KSPActionGroup actionGroup;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8003179");
	}

	public void actionGroupControlComplete(MEGUIParameterDropdownList parameter)
	{
		parameter.dropdownList.options.RemoveAt(parameter.dropdownList.options.Count - 1);
	}

	public override IEnumerator Fire()
	{
		if (FlightGlobals.ActiveVessel != null)
		{
			yield return new WaitForSeconds(0.1f);
			FlightGlobals.ActiveVessel.ActionGroups.ToggleGroup(actionGroup);
		}
		yield return null;
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8003180");
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("actionGroup", actionGroup);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetEnum("actionGroup", ref actionGroup, KSPActionGroup.None);
	}
}

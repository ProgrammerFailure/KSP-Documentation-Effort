using System.Collections.Generic;

namespace Expansions.Missions.Editor;

[MEGUI_LaunchSiteSelect]
public class MEGUIParameterLaunchSiteDropdownList : MEGUIParameterDropdownList
{
	public DictionaryValueList<string, string> dropdownOptions;

	public override void Setup(string name)
	{
		base.Setup(name);
		title.text = name;
		GameEvents.Mission.onBuilderNodeAdded.Add(onBuilderNodeAdded);
		GameEvents.Mission.onBuilderNodeDeleted.Add(onBuilderNodeDeleted);
	}

	public void OnDestroy()
	{
		GameEvents.Mission.onBuilderNodeAdded.Remove(onBuilderNodeAdded);
		GameEvents.Mission.onBuilderNodeDeleted.Remove(onBuilderNodeDeleted);
	}

	public void onBuilderNodeAdded(MENode node)
	{
		if (node.IsLaunchPadNode)
		{
			RebuildDropDown();
		}
	}

	public void onBuilderNodeDeleted(MENode node)
	{
		if (node.IsLaunchPadNode)
		{
			RebuildDropDown();
		}
	}

	public override void BuildDropDownItemsList()
	{
		int itemIndex = GetItemIndex(field.GetValue());
		base.Items = new DictionaryValueList<string, MEGUIDropDownItem>();
		List<MEGUIDropDownItem> list = new List<MEGUIDropDownItem>();
		List<PSystemSetup.SpaceCenterFacility> spaceCenterFacilityLaunchSites = PSystemSetup.Instance.SpaceCenterFacilityLaunchSites;
		for (int i = 0; i < spaceCenterFacilityLaunchSites.Count; i++)
		{
			PSystemSetup.SpaceCenterFacility spaceCenterFacility = spaceCenterFacilityLaunchSites[i];
			list.Add(new MEGUIDropDownItem(spaceCenterFacility.name, spaceCenterFacility.name, spaceCenterFacility.facilityDisplayName));
		}
		List<LaunchSite> launchSites = PSystemSetup.Instance.LaunchSites;
		for (int j = 0; j < launchSites.Count; j++)
		{
			LaunchSite launchSite = launchSites[j];
			list.Add(new MEGUIDropDownItem(launchSite.name, launchSite.name, launchSite.launchSiteName));
		}
		int count = list.Count;
		while (count-- > 0)
		{
			if (optionsToExclude != null && optionsToExclude.Contains(list[count].key))
			{
				list.RemoveAt(count);
			}
		}
		for (int k = 0; k < list.Count; k++)
		{
			base.Items.Add(list[k].key, list[k]);
		}
		dropdownList.ClearOptions();
		dropdownList.AddOptions(list.GetDisplayStrings());
		int itemIndex2 = GetItemIndex(field.GetValue());
		if (itemIndex != itemIndex2 || itemIndex2 < 0)
		{
			selectedKey = base.Items.KeyAt((itemIndex2 >= 0) ? itemIndex2 : 0);
			field.SetValue(base.Items[selectedKey].value);
		}
	}
}

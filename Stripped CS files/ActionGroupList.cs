using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ActionGroupList
{
	public List<bool>[] groupStates;

	public List<bool> groups;

	public List<double> cooldownTimes;

	public Vessel v;

	public bool this[KSPActionGroup group]
	{
		get
		{
			return groups[BaseAction.GetGroupIndex(group)];
		}
		set
		{
			groups[BaseAction.GetGroupIndex(group)] = value;
		}
	}

	public ActionGroupList(Vessel v)
	{
		this.v = v;
		int actionGroupsLength = BaseAction.ActionGroupsLength;
		groupStates = new List<bool>[Vessel.NumOverrideGroups];
		for (int i = 0; i < groupStates.Length; i++)
		{
			groupStates[i] = new List<bool>(actionGroupsLength);
			for (int j = 0; j < actionGroupsLength; j++)
			{
				groupStates[i].Add(item: false);
			}
		}
		cooldownTimes = new List<double>();
		groups = new List<bool>(actionGroupsLength);
		for (int k = 0; k < actionGroupsLength; k++)
		{
			groups.Add(item: false);
			cooldownTimes.Add(0.0);
		}
	}

	public void ToggleGroup(KSPActionGroup group)
	{
		int groupIndex = BaseAction.GetGroupIndex(group);
		if (Planetarium.GetUniversalTime() < cooldownTimes[groupIndex])
		{
			Debug.Log("Cannot toggle " + group.ToString() + ". " + (cooldownTimes[groupIndex] - Planetarium.GetUniversalTime()).ToString("0.0") + " seconds until it becomes available again.", v.gameObject);
			return;
		}
		List<bool> list = groups;
		int num = 0;
		bool flag = false;
		bool flag2 = false;
		if (GameSettings.ADDITIONAL_ACTION_GROUPS)
		{
			num = v.GroupOverride;
			if (num > 0 && num <= v.OverrideDefault.Length)
			{
				flag = v.OverrideDefault[num - 1];
				flag2 = (v.OverrideActionControl[num - 1] & group) != 0;
				list = groupStates[num - 1];
			}
		}
		list[groupIndex] = !list[groupIndex];
		if (num > 0 && !(flag || flag2))
		{
			groups[groupIndex] = !groups[groupIndex];
		}
		KSPActionParam kSPActionParam = new KSPActionParam(group, (!list[groupIndex]) ? KSPActionType.Deactivate : KSPActionType.Activate);
		List<BaseAction> list2 = BaseAction.CreateActionList(v.parts, group, num, flag, include: true);
		int count = list2.Count;
		for (int i = 0; i < count; i++)
		{
			list2[i].Invoke(kSPActionParam);
		}
		cooldownTimes[groupIndex] = Planetarium.GetUniversalTime() + (double)kSPActionParam.Cooldown;
	}

	public void SetGroup(KSPActionGroup group, bool active)
	{
		int groupIndex = BaseAction.GetGroupIndex(group);
		if (groups[groupIndex] != active)
		{
			ToggleGroup(group);
		}
	}

	public void Load(ConfigNode node)
	{
		int actionGroupsLength = BaseAction.ActionGroupsLength;
		for (int i = 0; i < actionGroupsLength; i++)
		{
			string value = node.GetValue(((KSPActionGroup)(1 << i)).ToString());
			if (value != null)
			{
				string[] array = value.Split(',');
				if (array.Length == 2)
				{
					groups[i] = bool.Parse(array[0].Trim());
					cooldownTimes[i] = double.Parse(array[1].Trim());
				}
				else
				{
					groups[i] = bool.Parse(value);
				}
			}
		}
	}

	public void Save(ConfigNode node)
	{
		int actionGroupsLength = BaseAction.ActionGroupsLength;
		for (int i = 0; i < actionGroupsLength; i++)
		{
			node.AddValue(((KSPActionGroup)(1 << i)).ToStringCached(), groups[i] + ", " + cooldownTimes[i]);
		}
	}

	public double CooldownTimeRemaining(KSPActionGroup group)
	{
		int groupIndex = BaseAction.GetGroupIndex(group);
		if (Planetarium.GetUniversalTime() < cooldownTimes[groupIndex])
		{
			return cooldownTimes[groupIndex] - Planetarium.GetUniversalTime();
		}
		return 0.0;
	}

	public void CopyFrom(ActionGroupList acl)
	{
		int count = groups.Count;
		for (int i = 0; i < count; i++)
		{
			groups[i] = acl.groups[i];
			cooldownTimes[i] = acl.cooldownTimes[i];
		}
	}
}

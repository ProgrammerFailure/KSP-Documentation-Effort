using System.Collections.Generic;

public class AxisGroupsModule : VesselModule
{
	public Dictionary<int, BaseAxisFieldList> AxisGroups;

	public int groupOverride;

	public void AddAxis(KSPAxisGroup axisGroup, BaseAxisField axisField)
	{
		groupOverride = 0;
		bool overrideDefault = false;
		if (GameSettings.ADDITIONAL_ACTION_GROUPS)
		{
			groupOverride = vessel.GroupOverride;
			if (groupOverride > 0 && groupOverride <= vessel.OverrideDefault.Length)
			{
				overrideDefault = vessel.OverrideDefault[groupOverride - 1];
			}
		}
		if (axisField.IsInGroup(axisGroup, groupOverride, overrideDefault, include: true))
		{
			if (!AxisGroups.ContainsKey((int)axisGroup))
			{
				AxisGroups[(int)axisGroup] = new BaseAxisFieldList();
			}
			AxisGroups[(int)axisGroup].Add(axisField);
		}
	}

	public void BuildAxisGroups()
	{
		if (AxisGroups == null)
		{
			AxisGroups = new Dictionary<int, BaseAxisFieldList>();
		}
		else
		{
			AxisGroups.Clear();
		}
		List<PartModule> list = vessel.FindPartModulesImplementing<PartModule>();
		int count = list.Count;
		while (count-- > 0)
		{
			PartModule partModule = list[count];
			int count2 = partModule.Fields.Count;
			while (count2-- > 0)
			{
				if (partModule.Fields[count2] is BaseAxisField baseAxisField && baseAxisField.FieldInfo.FieldType == typeof(float))
				{
					for (int i = 0; i < BaseAxisField.AxisGroupsLength; i++)
					{
						KSPAxisGroup axisGroup = (KSPAxisGroup)(1 << i);
						AddAxis(axisGroup, baseAxisField);
					}
				}
			}
		}
	}

	public void onVesselWasModified(Vessel v)
	{
		if (v == vessel && vessel.loaded)
		{
			BuildAxisGroups();
		}
	}

	public void OnVesselOverrideGroupChanged(Vessel v)
	{
		if (v == vessel && vessel.loaded)
		{
			BuildAxisGroups();
		}
	}

	public override Activation GetActivation()
	{
		return Activation.FlightScene | Activation.LoadedVessels;
	}

	public override void OnAwake()
	{
		GameEvents.onVesselWasModified.Add(onVesselWasModified);
		GameEvents.OnVesselOverrideGroupChanged.Add(OnVesselOverrideGroupChanged);
	}

	public void OnDestroy()
	{
		GameEvents.onVesselWasModified.Remove(onVesselWasModified);
		GameEvents.OnVesselOverrideGroupChanged.Remove(OnVesselOverrideGroupChanged);
	}

	public override void OnLoadVessel()
	{
		BuildAxisGroups();
	}

	public override bool ShouldBeActive()
	{
		return base.ShouldBeActive();
	}

	public void SetAxisGroup(KSPAxisGroup axisGroup, float axisValue)
	{
		if (AxisGroups != null && AxisGroups.TryGetValue((int)axisGroup, out var value))
		{
			int count = value.Count;
			while (count-- > 0)
			{
				value[count].SetAxis(axisValue);
			}
		}
	}

	public void IncrementAxisGroup(KSPAxisGroup axisGroup, float axisRate)
	{
		if (AxisGroups != null && AxisGroups.TryGetValue((int)axisGroup, out var value))
		{
			int count = value.Count;
			while (count-- > 0)
			{
				value[count].IncrementAxis(axisRate);
			}
		}
	}

	public void UpdateAxisGroup(KSPAxisGroup axisGroup, float axisValue)
	{
		if (AxisGroups != null && AxisGroups.TryGetValue((int)axisGroup, out var value))
		{
			int count = value.Count;
			while (count-- > 0)
			{
				value[count].UpdateAxis(axisGroup, groupOverride, axisValue);
			}
		}
	}
}

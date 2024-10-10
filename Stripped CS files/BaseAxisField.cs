using System;
using System.Collections.Generic;
using System.Reflection;
using Expansions.Serenity;
using UnityEngine;

public class BaseAxisField : BaseField
{
	public static KSPAxisGroup MaxAxisGroup = KSPAxisGroup.Custom04;

	public float minValue;

	public float maxValue;

	public float incrementalSpeed;

	public float incrementalSpeedMultiplier;

	public bool ignoreClampWhenIncremental;

	public bool ignoreIncrementByZero;

	public bool active;

	public KSPAxisGroup defaultAxisGroup;

	public KSPAxisGroup axisGroup;

	public KSPAxisGroup axisIncremental;

	public KSPAxisGroup axisInverted;

	public KSPAxisGroup[] overrideGroups;

	public KSPAxisGroup[] overrideIncremental;

	public KSPAxisGroup[] overrideInverted;

	public static int axisGroupsLength = -1;

	public PartModule module => base.host as PartModule;

	public static int AxisGroupsLength
	{
		get
		{
			if (axisGroupsLength == -1)
			{
				axisGroupsLength = Enum.GetNames(typeof(KSPAxisGroup)).Length - 2;
			}
			return axisGroupsLength;
		}
	}

	public event Callback OnAxisSpeedChanged;

	public BaseAxisField(KSPAxisField fieldAttrib, FieldInfo fieldInfo, object host)
		: base(fieldAttrib, fieldInfo, host)
	{
		minValue = fieldAttrib.minValue;
		maxValue = fieldAttrib.maxValue;
		incrementalSpeed = fieldAttrib.incrementalSpeed;
		incrementalSpeedMultiplier = fieldAttrib.incrementalSpeedMultiplier;
		defaultAxisGroup = fieldAttrib.axisGroup;
		axisGroup = fieldAttrib.axisGroup;
		active = true;
		if (fieldAttrib.axisMode == KSPAxisMode.Incremental)
		{
			axisIncremental = (KSPAxisGroup)(((int)MaxAxisGroup << 1) - 1);
		}
		else
		{
			axisIncremental = KSPAxisGroup.None;
		}
		axisIncremental &= ~KSPAxisGroup.MainThrottle;
		axisInverted = KSPAxisGroup.None;
		overrideGroups = new KSPAxisGroup[Vessel.NumOverrideGroups];
		overrideIncremental = new KSPAxisGroup[Vessel.NumOverrideGroups];
		for (int i = 0; i < overrideIncremental.Length; i++)
		{
			overrideIncremental[i] = axisIncremental;
		}
		overrideInverted = new KSPAxisGroup[Vessel.NumOverrideGroups];
		ignoreClampWhenIncremental = fieldAttrib.ignoreClampWhenIncremental;
		ignoreIncrementByZero = fieldAttrib.ignoreIncrementByZero;
	}

	public KSPAxisGroup GetAxisGroup(int groupOverride)
	{
		if (groupOverride > 0 && groupOverride <= overrideGroups.Length)
		{
			return overrideGroups[groupOverride - 1];
		}
		return axisGroup;
	}

	public override void CopyField(BaseField field)
	{
		base.CopyField(field);
		BaseAxisField baseAxisField = field as BaseAxisField;
		axisGroup = baseAxisField.axisGroup;
		axisIncremental = baseAxisField.axisIncremental;
		axisInverted = baseAxisField.axisInverted;
		int num = overrideIncremental.Length;
		while (num-- > 0)
		{
			overrideIncremental[num] = baseAxisField.overrideInverted[num];
		}
		int num2 = overrideInverted.Length;
		while (num2-- > 0)
		{
			overrideInverted[num2] = baseAxisField.overrideInverted[num2];
		}
	}

	public bool IsInGroup(KSPAxisGroup group, int overrideGroup, bool overrideDefault, bool include)
	{
		bool flag = false;
		if (overrideGroup > 0 && overrideGroup <= overrideGroups.Length)
		{
			flag = (overrideGroups[overrideGroup - 1] & group) != 0;
		}
		if (overrideGroup == 0 || (!overrideDefault && include))
		{
			flag |= (axisGroup & group) != 0;
		}
		return flag;
	}

	public bool ContainsNonDefaultAxes()
	{
		if (axisGroup != defaultAxisGroup)
		{
			return true;
		}
		int num = 0;
		while (true)
		{
			if (num < overrideGroups.Length)
			{
				if (overrideGroups[num] != 0)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public KSPAxisGroup ParseGroup(string groupName, KSPAxisGroup defaultGroup)
	{
		KSPAxisGroup result = defaultGroup;
		if (!string.IsNullOrEmpty(groupName))
		{
			try
			{
				result = (KSPAxisGroup)Enum.Parse(typeof(KSPAxisGroup), groupName);
			}
			catch (ArgumentException)
			{
			}
		}
		return result;
	}

	public void OnLoad(ConfigNode node)
	{
		string value = node.GetValue("axisGroup");
		axisGroup = ParseGroup(value, defaultAxisGroup);
		axisIncremental = ParseGroup(node.GetValue("axisIncremental"), KSPAxisGroup.None);
		node.TryGetValue("axisSpeedMultiplier", ref incrementalSpeedMultiplier);
		axisInverted = ParseGroup(node.GetValue("axisInverted"), KSPAxisGroup.None);
		for (int i = 0; i < overrideGroups.Length; i++)
		{
			value = "overrideGroup" + i;
			overrideGroups[i] = ParseGroup(node.GetValue(value), KSPAxisGroup.None);
			value = "overrideIncremental" + i;
			overrideIncremental[i] = ParseGroup(node.GetValue(value), KSPAxisGroup.None);
			value = "overrideInverted" + i;
			overrideInverted[i] = ParseGroup(node.GetValue(value), KSPAxisGroup.None);
		}
	}

	public void OnSave(ConfigNode node)
	{
		node.AddValue("axisGroup", axisGroup.ToStringCached());
		node.AddValue("axisIncremental", axisIncremental.ToStringCached());
		node.AddValue("axisSpeedMultiplier", incrementalSpeedMultiplier);
		node.AddValue("axisInverted", axisInverted.ToStringCached());
		for (int i = 0; i < overrideGroups.Length; i++)
		{
			if (overrideGroups[i] != 0)
			{
				string value = overrideGroups[i].ToStringCached();
				node.AddValue("overrideGroup" + i, value);
			}
			if (overrideIncremental[i] != 0)
			{
				string value2 = overrideIncremental[i].ToStringCached();
				node.AddValue("overrideIncremental" + i, value2);
			}
			if (overrideInverted[i] != 0)
			{
				string value3 = overrideInverted[i].ToStringCached();
				node.AddValue("overrideInverted" + i, value3);
			}
		}
	}

	public void SetAxis(float axisValue)
	{
		float num = (axisValue + 1f) / 2f;
		float num2 = minValue + (maxValue - minValue) * num;
		SetValue(num2, base.host);
	}

	public void IncrementAxis(float axisRate)
	{
		if (!ignoreIncrementByZero || axisRate != 0f)
		{
			float num = (float)GetValue(base.host);
			if (incrementalSpeedMultiplier == 0f)
			{
				incrementalSpeedMultiplier = GameSettings.AXIS_INCREMENTAL_SPEED_MULTIPLIER_DEFAULT;
			}
			incrementalSpeed = (maxValue - minValue) * incrementalSpeedMultiplier;
			num += axisRate * incrementalSpeed * TimeWarp.fixedDeltaTime;
			if (!ignoreClampWhenIncremental)
			{
				num = Mathf.Min(num, maxValue);
				num = Mathf.Max(num, minValue);
			}
			SetValue(num, base.host);
		}
	}

	public static BaseAxisFieldList CreateAxisList(List<Part> parts, KSPAxisGroup group, int overrideGroup, bool overrideDefault, bool include)
	{
		BaseAxisFieldList baseAxisFieldList = new BaseAxisFieldList();
		for (int i = 0; i < parts.Count; i++)
		{
			baseAxisFieldList.AddRange(CreateAxisList(parts[i], group, overrideGroup, overrideDefault, include));
		}
		return baseAxisFieldList;
	}

	public static void AddAxis(BaseAxisFieldList axisFieldList, KSPAxisGroup group, int overrideGroup, bool overrideDefault, bool include, BaseAxisField axisField)
	{
		bool flag = axisField.IsInGroup(group, overrideGroup, overrideDefault, include);
		if (include == flag)
		{
			axisFieldList.Add(axisField);
		}
	}

	public static BaseAxisFieldList CreateAxisList(Part part, KSPAxisGroup group, int overrideGroup, bool overrideDefault, bool include)
	{
		BaseAxisFieldList baseAxisFieldList = new BaseAxisFieldList();
		for (int i = 0; i < part.Modules.Count; i++)
		{
			PartModule partModule = part.Modules[i];
			if (!partModule.isEnabled)
			{
				continue;
			}
			for (int j = 0; j < partModule.Fields.Count; j++)
			{
				if (partModule.Fields[j] is BaseAxisField { active: not false } baseAxisField)
				{
					AddAxis(baseAxisFieldList, group, overrideGroup, overrideDefault, include, baseAxisField);
				}
			}
		}
		return baseAxisFieldList;
	}

	public static int GetAxisGroupsLength(float facilityLevel, bool isVAB = true)
	{
		return AxisGroupsLength;
	}

	public static string[] GetAxisGroups(int maxlevel)
	{
		string[] array = new string[GetAxisGroupsLength(1000f)];
		string[] names = Enum.GetNames(typeof(KSPAxisGroup));
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = names[i + 1];
		}
		return array;
	}

	public static List<bool> CreateGroupList(List<Part> parts)
	{
		return CreateGroupList(parts, 0);
	}

	public static List<bool> CreateGroupList(List<Part> parts, int groupOverride)
	{
		int num = AxisGroupsLength;
		List<bool> list = new List<bool>(num);
		for (int i = 0; i < num; i++)
		{
			KSPAxisGroup kSPAxisGroup = (KSPAxisGroup)(1 << i);
			bool flag = false;
			for (int j = 0; j < parts.Count; j++)
			{
				Part part = parts[j];
				for (int k = 0; k < part.Modules.Count; k++)
				{
					PartModule partModule = part.Modules[k];
					for (int l = 0; l < partModule.Fields.Count; l++)
					{
						if (!(partModule.Fields[l] is BaseAxisField baseAxisField) || (baseAxisField.GetAxisGroup(groupOverride) & kSPAxisGroup) == 0)
						{
							if (flag)
							{
								break;
							}
							continue;
						}
						flag = true;
						break;
					}
					if (flag)
					{
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			list.Add(flag);
		}
		return list;
	}

	public static bool ContainsNonDefaultAxes(Part part)
	{
		int count = part.Modules.Count;
		while (count-- > 0)
		{
			PartModule partModule = part.Modules[count];
			int count2 = partModule.Fields.Count;
			while (count2-- > 0)
			{
				if (partModule.Fields[count2] is BaseAxisField baseAxisField && baseAxisField.ContainsNonDefaultAxes())
				{
					return true;
				}
			}
		}
		return false;
	}

	public void UpdateAxis(KSPAxisGroup group, int groupOverride, float axisValue)
	{
		KSPAxisGroup kSPAxisGroup = axisInverted;
		KSPAxisGroup kSPAxisGroup2 = axisIncremental;
		if (groupOverride > 0 && groupOverride <= overrideGroups.Length)
		{
			kSPAxisGroup = overrideInverted[groupOverride - 1];
			kSPAxisGroup2 = overrideIncremental[groupOverride - 1];
		}
		if ((group & kSPAxisGroup) != 0)
		{
			axisValue = 0f - axisValue;
		}
		if ((group & kSPAxisGroup2) != 0)
		{
			IncrementAxis(axisValue);
		}
		else
		{
			SetAxis(axisValue);
		}
	}

	public void SetIncrementalSpeedMultiplier(float value)
	{
		incrementalSpeedMultiplier = value;
		if (this.OnAxisSpeedChanged != null)
		{
			this.OnAxisSpeedChanged();
		}
	}

	public static BaseAxisFieldList CreateAxisList(List<Part> parts, ModuleRoboticController controller, bool include)
	{
		BaseAxisFieldList baseAxisFieldList = new BaseAxisFieldList();
		for (int i = 0; i < parts.Count; i++)
		{
			if (controller.PartPersistentId != parts[i].persistentId)
			{
				baseAxisFieldList.AddRange(CreateAxisList(parts[i], controller, include));
			}
		}
		return baseAxisFieldList;
	}

	public static BaseAxisFieldList CreateAxisList(Part part, ModuleRoboticController controller, bool include)
	{
		BaseAxisFieldList baseAxisFieldList = new BaseAxisFieldList();
		for (int i = 0; i < part.Modules.Count; i++)
		{
			PartModule partModule = part.Modules[i];
			if (!partModule.isEnabled)
			{
				continue;
			}
			for (int j = 0; j < partModule.Fields.Count; j++)
			{
				if (partModule.Fields[j] is BaseAxisField { active: not false } baseAxisField)
				{
					AddAxis(baseAxisFieldList, controller, part, include, baseAxisField);
				}
			}
		}
		return baseAxisFieldList;
	}

	public static void AddAxis(BaseAxisFieldList axisFieldList, ModuleRoboticController controller, Part part, bool include, BaseAxisField axisField)
	{
		ControlledAxis action = null;
		bool symmetryPartner = false;
		bool flag = controller.TryGetPartAxisField(part, axisField, out action, out symmetryPartner);
		if (include == flag && !symmetryPartner)
		{
			axisFieldList.Add(axisField);
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Actions;

public class ActionPartResourceAmount : ActionVessel
{
	[MEGUI_Dropdown(onControlSetupComplete = "OnResourceNameControlSetup", order = 10, SetDropDownItems = "SetResourceNames", guiName = "#autoLOC_8000014", Tooltip = "#autoLOC_8000018")]
	public string resourceName;

	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, order = 20, resetValue = "0", guiName = "#autoLOC_8000019", Tooltip = "#autoLOC_8000020")]
	public float adjustAmount;

	public static double epsilon = 1E-15;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000016");
		useActiveVessel = true;
	}

	public override IEnumerator Fire()
	{
		if (base.vessel != null)
		{
			PartResourceDefinition def = PartResourceLibrary.Instance.GetDefinition(resourceName);
			if (def == null)
			{
				Debug.LogError("[ActionPartResourceAmount] failed to find resource definition for " + resourceName);
				yield return null;
			}
			if (base.vessel.loaded)
			{
				double num = base.vessel.RequestResource(base.vessel.rootPart, def.id, 0f - adjustAmount, usePriority: true);
				if (Math.Abs(num - (double)adjustAmount) > epsilon)
				{
					Debug.Log("[ActionPartResourceAmount] failed to adjust resource by full amount. Amt Requested: " + adjustAmount + " Amt Added/Received: " + num);
				}
				if (num > 0.0)
				{
					GameEvents.Mission.onNodeChangedVesselResources.Fire(node, base.vessel, base.vessel.rootPart, null);
				}
			}
			else
			{
				double num = adjustAmount;
				bool flag = adjustAmount < 0f;
				for (int i = 0; i < base.vessel.protoVessel.protoPartSnapshots.Count; i++)
				{
					double num2 = adjustUnloadedPart(base.vessel.protoVessel.protoPartSnapshots[i], num, flag);
					num = ((!flag) ? (num - num2) : (num + num2));
					if (num2 > 0.0)
					{
						GameEvents.Mission.onNodeChangedVesselResources.Fire(node, base.vessel, null, base.vessel.protoVessel.protoPartSnapshots[i]);
					}
					if (Math.Abs(num) <= epsilon)
					{
						break;
					}
				}
				if (Math.Abs(num - (double)adjustAmount) > epsilon)
				{
					Debug.Log("[ActionPartResourceAmount] failed to adjust resource by full amount. Amt Requested: " + adjustAmount + " Amt Added/Received: " + num);
				}
			}
		}
		yield return null;
	}

	public double adjustUnloadedPart(ProtoPartSnapshot InProtoPart, double adjustAmount, bool pulling)
	{
		double num = 0.0;
		adjustAmount = Math.Abs(adjustAmount);
		int num2 = 0;
		ProtoPartResourceSnapshot protoPartResourceSnapshot;
		while (true)
		{
			if (num2 < InProtoPart.resources.Count)
			{
				protoPartResourceSnapshot = InProtoPart.resources[num2];
				if (protoPartResourceSnapshot.resourceName == resourceName)
				{
					double num3 = 0.0;
					if (pulling)
					{
						num3 = protoPartResourceSnapshot.amount;
						if (num3 > 0.0)
						{
							if (!(num3 < adjustAmount))
							{
								protoPartResourceSnapshot.amount -= adjustAmount;
								return num + adjustAmount;
							}
							protoPartResourceSnapshot.amount = 0.0;
							num += num3;
							adjustAmount += num3;
						}
					}
					else
					{
						num3 = protoPartResourceSnapshot.maxAmount - protoPartResourceSnapshot.amount;
						if (num3 > 0.0)
						{
							if (!(num3 < adjustAmount))
							{
								break;
							}
							protoPartResourceSnapshot.amount = protoPartResourceSnapshot.maxAmount;
							num += num3;
							adjustAmount -= num3;
						}
					}
				}
				num2++;
				continue;
			}
			return num;
		}
		protoPartResourceSnapshot.amount += adjustAmount;
		return num + adjustAmount;
	}

	public List<MEGUIDropDownItem> SetResourceNames()
	{
		List<MEGUIDropDownItem> list = new List<MEGUIDropDownItem>();
		IEnumerator<PartResourceDefinition> enumerator = PartResourceLibrary.Instance.resourceDefinitions.GetEnumerator();
		while (enumerator.MoveNext())
		{
			PartResourceDefinition current = enumerator.Current;
			list.Add(new MEGUIDropDownItem(current.name, current.name, current.displayName));
		}
		return list;
	}

	public void OnResourceNameControlSetup(MEGUIParameterDropdownList parameter)
	{
		UpdateNodeBodyUI();
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "resourceName")
		{
			string text = Localizer.Format("#autoLOC_6003083");
			if (!string.IsNullOrEmpty(resourceName))
			{
				PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(resourceName);
				if (definition != null)
				{
					text = definition.displayName;
				}
			}
			return Localizer.Format("#autoLOC_8004190", field.guiName, text);
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004000");
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("resourceName", resourceName);
		node.AddValue("adjustAmount", adjustAmount);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("resourceName", ref resourceName);
		node.TryGetValue("adjustAmount", ref adjustAmount);
	}
}

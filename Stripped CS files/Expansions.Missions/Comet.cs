using System;
using System.Collections.Generic;
using Expansions.Missions.Editor;
using UnityEngine;

namespace Expansions.Missions;

[Serializable]
public class Comet : IConfigNode
{
	[MEGUI_InputField(tabStop = true, order = 1, guiName = "#autoLOC_8100112")]
	public string name;

	public uint seed;

	public float radius;

	[MEGUI_Dropdown(onDropDownValueChange = "OnCometTypeChanged", order = 2, SetDropDownItems = "SetCometTypes", addDefaultOption = false, gapDisplay = true, guiName = "#autoLOC_8100113")]
	public string cometType;

	public string cometDisplayType;

	[MEGUI_Dropdown(order = 3, gapDisplay = true, guiName = "#autoLOC_8100114")]
	public UntrackedObjectClass cometClass;

	public uint persistentId;

	public Comet()
	{
		name = "Comet";
		radius = 0f;
		seed = 0u;
		if (CometManager.Instance != null && CometManager.Instance.cometDefinitions.Count > 0 && CometManager.Instance.cometDefinitions[0] != null)
		{
			cometType = CometManager.Instance.cometDefinitions[0].name;
			cometDisplayType = CometManager.Instance.cometDefinitions[0].displayName;
		}
		else
		{
			cometDisplayType = (cometType = "");
		}
		cometClass = UntrackedObjectClass.const_2;
		persistentId = 0u;
	}

	public void Randomize()
	{
		name = DiscoverableObjectsUtil.GenerateCometName();
		persistentId = FlightGlobals.GetUniquepersistentId();
		seed = GetRandomCometSeed();
	}

	public static uint GetRandomCometSeed()
	{
		return (uint)UnityEngine.Random.Range(0, int.MaxValue);
	}

	public GameObject SetGAPComet()
	{
		return MissionsUtils.MEPrefab("Prefabs/GAP_ProceduralComet.prefab");
	}

	public void OnGAPPrefabInstantiated(GameObject prefabInstance)
	{
		prefabInstance.GetComponent<GAPProceduralComet>().Setup(seed, cometClass);
	}

	public void OnCometTypeChanged(MEGUIParameterDropdownList sender, int newIndex)
	{
		if (!(CometManager.Instance != null))
		{
			return;
		}
		List<CometOrbitType> cometDefinitions = CometManager.Instance.cometDefinitions;
		cometDisplayType = cometType;
		int num = 0;
		while (true)
		{
			if (num < cometDefinitions.Count)
			{
				if (cometType == cometDefinitions[num].name)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		cometDisplayType = cometDefinitions[num].displayName;
	}

	public List<MEGUIDropDownItem> SetCometTypes()
	{
		List<MEGUIDropDownItem> list = new List<MEGUIDropDownItem>();
		if (CometManager.Instance != null)
		{
			for (int i = 0; i < CometManager.Instance.cometDefinitions.Count; i++)
			{
				list.Add(new MEGUIDropDownItem(CometManager.Instance.cometDefinitions[i].name, CometManager.Instance.cometDefinitions[i].name, CometManager.Instance.cometDefinitions[i].displayName));
			}
		}
		return list;
	}

	public void Load(ConfigNode node)
	{
		for (int i = 0; i < node.values.Count; i++)
		{
			ConfigNode.Value value = node.values[i];
			switch (value.name)
			{
			case "persistentId":
				uint.TryParse(value.value, out persistentId);
				break;
			case "cometClass":
				cometClass = (UntrackedObjectClass)Enum.Parse(typeof(UntrackedObjectClass), value.value);
				break;
			case "cometType":
				cometType = value.value;
				OnCometTypeChanged(null, 0);
				break;
			case "cometradius":
				float.TryParse(value.value, out radius);
				break;
			case "cometSeed":
				uint.TryParse(value.value, out seed);
				break;
			case "cometName":
				name = value.value;
				break;
			}
		}
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("cometName", name);
		node.AddValue("cometSeed", seed);
		node.AddValue("cometradius", radius);
		node.AddValue("cometType", cometType);
		node.AddValue("cometClass", cometClass);
		node.AddValue("persistentId", persistentId);
	}
}

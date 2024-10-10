using System;
using UnityEngine;

[Serializable]
public struct ResourceRatio
{
	public string ResourceName;

	public double Ratio;

	public bool DumpExcess;

	public ResourceFlowMode FlowMode;

	public ResourceRatio(string resourceName, double ratio, bool dumpExcess)
	{
		ResourceName = resourceName;
		Ratio = ratio;
		DumpExcess = dumpExcess;
		FlowMode = ResourceFlowMode.NULL;
	}

	public ResourceRatio(string resourceName, double ratio, bool dumpExcess, ResourceFlowMode flowMode)
	{
		ResourceName = resourceName;
		Ratio = ratio;
		DumpExcess = dumpExcess;
		FlowMode = flowMode;
	}

	public void Load(ConfigNode node)
	{
		ResourceName = node.GetValue("ResourceName");
		if (node.HasValue("Ratio"))
		{
			Ratio = float.Parse(node.GetValue("Ratio"));
		}
		if (node.HasValue("DumpExcess"))
		{
			DumpExcess = bool.Parse(node.GetValue("DumpExcess"));
		}
		if (node.HasValue("FlowMode"))
		{
			try
			{
				FlowMode = (ResourceFlowMode)Enum.Parse(typeof(ResourceFlowMode), node.GetValue("FlowMode"));
			}
			catch (Exception ex)
			{
				Debug.LogError("[ResourceRatio]: Error parsing flow mode, given mode: " + node.GetValue("FlowMode") + ", exception " + ex);
				FlowMode = ResourceFlowMode.NULL;
			}
		}
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("ResourceName", ResourceName);
		node.AddValue("Ratio", Ratio);
		node.AddValue("DumpExcess", DumpExcess);
		node.AddValue("FlowMode", FlowMode);
	}
}

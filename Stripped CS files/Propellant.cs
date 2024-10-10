using System;
using UnityEngine;

[Serializable]
public class Propellant
{
	public string name;

	public string displayName;

	public int id;

	public float ratio;

	public double minResToLeave;

	public bool ignoreForIsp;

	public bool ignoreForThrustCurve;

	public double currentRequirement;

	public double currentAmount;

	public bool isDeprived;

	public bool drawStackGauge;

	public PartResourceDefinition _cachedResourceDef;

	public int displayNameLimit = 14;

	[SerializeField]
	public ResourceFlowMode flowMode = ResourceFlowMode.NULL;

	[SerializeField]
	public ResourceFlowMode fMode = ResourceFlowMode.NULL;

	public double actualTotalAvailable;

	public double totalResourceCapacity;

	public bool hasAlternatePropellant;

	public PartResourceDefinition resourceDef
	{
		get
		{
			if (_cachedResourceDef == null)
			{
				_cachedResourceDef = PartResourceLibrary.Instance.GetDefinition(id);
			}
			return _cachedResourceDef;
		}
	}

	public double totalResourceAvailable
	{
		get
		{
			double num = actualTotalAvailable - minResToLeave;
			if (num > 0.0)
			{
				return num;
			}
			return 0.0;
		}
		set
		{
			actualTotalAvailable = value;
		}
	}

	public void UpdateConnectedResources(Part p)
	{
		p.GetConnectedResourceTotals(id, GetFlowMode(), out actualTotalAvailable, out totalResourceCapacity);
	}

	public void Load(ConfigNode node)
	{
		name = node.GetValue("name");
		id = name.GetHashCode();
		PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(id);
		if (definition != null)
		{
			if (definition.displayName.Length > displayNameLimit && definition.displayName != "Electric Charge")
			{
				displayName = definition.abbreviation;
			}
			else
			{
				displayName = definition.displayName;
			}
		}
		if (string.IsNullOrEmpty(displayName))
		{
			displayName = name;
		}
		node.TryGetValue("ratio", ref ratio);
		node.TryGetValue("minResToLeave", ref minResToLeave);
		node.TryGetValue("ignoreForIsp", ref ignoreForIsp);
		node.TryGetValue("ignoreForThrustCurve", ref ignoreForThrustCurve);
		node.TryGetValue("DrawGauge", ref drawStackGauge);
		if (node.HasValue("resourceFlowMode"))
		{
			flowMode = (ResourceFlowMode)Enum.Parse(typeof(ResourceFlowMode), node.GetValue("resourceFlowMode"));
			fMode = flowMode;
		}
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("name", name);
		node.AddValue("ratio", ratio);
	}

	public PartResourceDefinition getPartResourceDefinition()
	{
		if (PartResourceLibrary.Instance != null)
		{
			return PartResourceLibrary.Instance.GetDefinition(id);
		}
		return null;
	}

	public ResourceFlowMode GetFlowMode()
	{
		if (flowMode == ResourceFlowMode.NULL)
		{
			flowMode = PartResourceLibrary.GetDefaultFlowMode(id);
		}
		return flowMode;
	}

	public string GetFlowModeDescription()
	{
		string text = "";
		if (resourceDef != null)
		{
			text += resourceDef.GetFlowModeDescription();
		}
		return text;
	}
}

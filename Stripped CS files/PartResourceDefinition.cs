using System;
using ns9;
using UnityEngine;

[Serializable]
public class PartResourceDefinition
{
	[SerializeField]
	public string _name = "";

	[SerializeField]
	public string _displayName = "";

	[SerializeField]
	public int _id = -1;

	[SerializeField]
	public float _density = 1f;

	[SerializeField]
	public float _volume = 5f;

	[SerializeField]
	public string _abbreviation = "";

	[SerializeField]
	public float _unitCost;

	[SerializeField]
	public float _specificHeatCapacity;

	[SerializeField]
	public ConfigNode _config;

	[SerializeField]
	public Color _color = Color.white;

	[SerializeField]
	public ResourceFlowMode _resourceFlowMode;

	[SerializeField]
	public ResourceTransferMode _resourceTransferMode;

	[SerializeField]
	public bool _isTweakable;

	public bool _isVisible = true;

	public PartResourceDrainDefinition partResourceDrainDefinition;

	public string name => _name;

	public string displayName => _displayName;

	public int id => _id;

	public float density => _density;

	public float volume => _volume;

	public string abbreviation => _abbreviation;

	public float unitCost => _unitCost;

	public float specificHeatCapacity => _specificHeatCapacity;

	public ConfigNode Config => _config;

	public Color color => _color;

	public ResourceFlowMode resourceFlowMode => _resourceFlowMode;

	public ResourceTransferMode resourceTransferMode => _resourceTransferMode;

	public bool isTweakable => _isTweakable;

	public bool isVisible => _isVisible;

	public PartResourceDefinition()
	{
		partResourceDrainDefinition = new PartResourceDrainDefinition();
	}

	public PartResourceDefinition(string name)
	{
		_name = name;
		_id = name.GetHashCode();
		partResourceDrainDefinition = new PartResourceDrainDefinition();
	}

	public PartResourceDefinition(string name, Color color)
	{
		_name = name;
		_id = name.GetHashCode();
		_color = color;
		partResourceDrainDefinition = new PartResourceDrainDefinition();
	}

	public void Load(ConfigNode node)
	{
		_name = node.GetValue("name");
		_id = name.GetHashCode();
		if (node.HasValue("displayName"))
		{
			_displayName = node.GetValue("displayName");
		}
		else
		{
			_displayName = KSPUtil.PrintModuleName(_name);
		}
		if (node.HasValue("abbreviation"))
		{
			_abbreviation = node.GetValue("abbreviation");
		}
		else
		{
			_abbreviation = GetShortName();
		}
		if (node.HasValue("density"))
		{
			_density = float.Parse(node.GetValue("density"));
		}
		if (node.HasValue("volume"))
		{
			_volume = float.Parse(node.GetValue("volume"));
		}
		if (node.HasValue("unitCost"))
		{
			_unitCost = float.Parse(node.GetValue("unitCost"));
		}
		if (node.HasValue("hsp"))
		{
			_specificHeatCapacity = float.Parse(node.GetValue("hsp"));
		}
		if (node.HasValue("isTweakable"))
		{
			_isTweakable = bool.Parse(node.GetValue("isTweakable"));
		}
		if (node.HasValue("isVisible"))
		{
			_isVisible = bool.Parse(node.GetValue("isVisible"));
		}
		if (node.HasValue("flowMode"))
		{
			_resourceFlowMode = (ResourceFlowMode)Enum.Parse(typeof(ResourceFlowMode), node.GetValue("flowMode"));
		}
		if (node.HasValue("transfer"))
		{
			_resourceTransferMode = (ResourceTransferMode)Enum.Parse(typeof(ResourceTransferMode), node.GetValue("transfer"));
		}
		if (node.HasValue("color"))
		{
			_color = ConfigNode.ParseColor(node.GetValue("color"));
		}
		if (node.HasNode("RESOURCE_DRAIN_DEFINITION"))
		{
			partResourceDrainDefinition.Load(node.GetNode("RESOURCE_DRAIN_DEFINITION"));
		}
		_config = new ConfigNode();
		node.CopyTo(_config);
	}

	public string GetShortName(int length = 2)
	{
		if (_abbreviation.Length > 0)
		{
			return _abbreviation.Substring(0, length);
		}
		return displayName.Substring(0, length);
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("name", name);
		node.AddValue("density", density);
		node.AddValue("volume", volume);
		node.AddValue("unitCost", unitCost);
		node.AddValue("hsp", specificHeatCapacity);
		node.AddValue("flowMode", resourceFlowMode);
		node.AddValue("transfer", resourceTransferMode);
		if (partResourceDrainDefinition != null)
		{
			ConfigNode node2 = new ConfigNode("RESOURCE_DRAIN_DEFINITION");
			partResourceDrainDefinition.Save(node2);
			node.AddNode(node2);
		}
		if (_color != Color.white)
		{
			node.AddValue("color", ConfigNode.WriteColor(_color));
		}
	}

	public string GetFlowModeDescription()
	{
		string text = "";
		switch (_resourceFlowMode)
		{
		case ResourceFlowMode.NO_FLOW:
			text += Localizer.Format("#autoLOC_245149");
			break;
		case ResourceFlowMode.ALL_VESSEL:
		case ResourceFlowMode.ALL_VESSEL_BALANCE:
			text += Localizer.Format("#autoLOC_245153", XKCDColors.HexFormat.KSPUnnamedCyan);
			break;
		case ResourceFlowMode.STAGE_PRIORITY_FLOW:
		case ResourceFlowMode.STAGE_PRIORITY_FLOW_BALANCE:
			text += Localizer.Format("#autoLOC_245157", XKCDColors.HexFormat.KSPBadassGreen);
			break;
		case ResourceFlowMode.STACK_PRIORITY_SEARCH:
		case ResourceFlowMode.STAGE_STACK_FLOW:
		case ResourceFlowMode.STAGE_STACK_FLOW_BALANCE:
			text += Localizer.Format("#autoLOC_245162", XKCDColors.HexFormat.YellowishOrange);
			break;
		}
		return text;
	}
}

using System;
using UnityEngine;

[Serializable]
public class PhysicMaterialDefinition
{
	[SerializeField]
	public string _name = "";

	[SerializeField]
	public string _displayName = "";

	[SerializeField]
	public int _id = -1;

	[SerializeField]
	public float _dynamicFriction;

	[SerializeField]
	public float _staticFriction;

	[SerializeField]
	public float _bounciness;

	[SerializeField]
	public PhysicMaterialCombine _frictionCombine;

	[SerializeField]
	public PhysicMaterialCombine _bounceCombine;

	[SerializeField]
	public ConfigNode _config;

	public PhysicMaterial material;

	public string name => _name;

	public string displayName => _displayName;

	public int id => _id;

	public float dynamicFriction => _dynamicFriction;

	public float staticFriction => _staticFriction;

	public float bounciness => _bounciness;

	public PhysicMaterialCombine frictionCombine => _frictionCombine;

	public PhysicMaterialCombine bounceCombine => _bounceCombine;

	public ConfigNode Config => _config;

	public PhysicMaterial Material => material;

	public PhysicMaterialDefinition()
	{
	}

	public PhysicMaterialDefinition(string name)
	{
		_name = name;
		_id = name.GetHashCode();
	}

	public PhysicMaterialDefinition(string name, string displayname, float dynamicFriction, float staticFriction, float bounciness, PhysicMaterialCombine frictionCombine, PhysicMaterialCombine bounceCombine)
	{
		_name = name;
		_id = name.GetHashCode();
		_displayName = displayname;
		_dynamicFriction = dynamicFriction;
		_staticFriction = staticFriction;
		_bounciness = bounciness;
		_frictionCombine = frictionCombine;
		_bounceCombine = bounceCombine;
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
		if (node.HasValue("dynamicFriction"))
		{
			_dynamicFriction = float.Parse(node.GetValue("dynamicFriction"));
		}
		if (node.HasValue("staticFriction"))
		{
			_staticFriction = float.Parse(node.GetValue("staticFriction"));
		}
		if (node.HasValue("bounciness"))
		{
			_bounciness = float.Parse(node.GetValue("bounciness"));
		}
		if (node.HasValue("frictionCombine"))
		{
			_frictionCombine = (PhysicMaterialCombine)Enum.Parse(typeof(PhysicMaterialCombine), node.GetValue("frictionCombine"));
		}
		if (node.HasValue("bounceCombine"))
		{
			_bounceCombine = (PhysicMaterialCombine)Enum.Parse(typeof(PhysicMaterialCombine), node.GetValue("bounceCombine"));
		}
		_config = new ConfigNode();
		node.CopyTo(_config);
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("name", name);
		node.AddValue("displayName", displayName);
		node.AddValue("dynamicFriction", dynamicFriction);
		node.AddValue("staticFriction", staticFriction);
		node.AddValue("bounciness", bounciness);
		node.AddValue("frictionCombine", frictionCombine);
		node.AddValue("bounceCombine", bounceCombine);
	}
}

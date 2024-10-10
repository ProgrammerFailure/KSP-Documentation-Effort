using System;
using UnityEngine;

[Serializable]
public class PartResourceDrainDefinition
{
	[SerializeField]
	public bool _isDrainable = true;

	[SerializeField]
	public bool _showDrainFX = true;

	[SerializeField]
	public int _drainFXPriority = 5;

	[SerializeField]
	public float _drainForceISP = 50f;

	[SerializeField]
	public string _drainFXDefinition = "gasDraining";

	public bool isDrainable => _isDrainable;

	public bool showDrainFX => _showDrainFX;

	public int drainFXPriority => _drainFXPriority;

	public float drainForceISP => _drainForceISP;

	public string drainFXDefinition => _drainFXDefinition;

	public PartResourceDrainDefinition()
	{
	}

	public PartResourceDrainDefinition(bool drainable, bool showFX, int priority, float drainISP, string fxName)
	{
		_isDrainable = drainable;
		_showDrainFX = showFX;
		_drainFXPriority = priority;
		_drainForceISP = drainISP;
		_drainFXDefinition = fxName;
	}

	public void Load(ConfigNode node)
	{
		if (node.HasValue("isDrainable"))
		{
			_isDrainable = bool.Parse(node.GetValue("isDrainable"));
		}
		if (node.HasValue("showDrainFX"))
		{
			_showDrainFX = bool.Parse(node.GetValue("showDrainFX"));
		}
		if (node.HasValue("drainFXPriority"))
		{
			_drainFXPriority = int.Parse(node.GetValue("drainFXPriority"));
		}
		if (node.HasValue("drainForceISP"))
		{
			_drainForceISP = float.Parse(node.GetValue("drainForceISP"));
		}
		if (node.HasValue("drainFXDefinition"))
		{
			_drainFXDefinition = node.GetValue("drainFXDefinition");
		}
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("isDrainable", isDrainable);
		node.AddValue("showDrainFX", showDrainFX);
		node.AddValue("drainFXPriority", drainFXPriority);
		node.AddValue("drainForceISP", drainForceISP);
		node.AddValue("drainFXDefinition", drainFXDefinition);
	}
}

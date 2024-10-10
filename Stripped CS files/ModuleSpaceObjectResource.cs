using System;
using UnityEngine;

public class ModuleSpaceObjectResource : PartModule
{
	[KSPField(isPersistant = true)]
	public float abundance;

	[KSPField(isPersistant = true)]
	public float displayAbundance;

	[KSPField]
	public int highRange;

	[KSPField]
	public int lowRange;

	[KSPField]
	public int presenceChance;

	[KSPField]
	public string resourceName = "";

	[KSPField]
	public string FlowMode = "";

	public ResourceFlowMode _flowMode = ResourceFlowMode.NULL;

	public override void OnLoad(ConfigNode node)
	{
		base.OnLoad(node);
		PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(resourceName);
		_flowMode = ResourceFlowMode.NULL;
		if (definition != null)
		{
			_flowMode = definition.resourceFlowMode;
		}
		if (!string.IsNullOrEmpty(FlowMode))
		{
			try
			{
				_flowMode = (ResourceFlowMode)Enum.Parse(typeof(ResourceFlowMode), FlowMode);
			}
			catch (Exception ex)
			{
				Debug.LogError("[ModuleSpaceObjectResource]: Error parsing flow mode, given mode: " + FlowMode + ", exception " + ex);
			}
		}
	}
}

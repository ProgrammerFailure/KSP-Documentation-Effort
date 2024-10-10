using System.Collections.Generic;
using ns9;

namespace CommNet;

public class ModuleRelayEnabler : PartModule, IResourceConsumer, IRelayEnabler
{
	public bool isRelaying = true;

	[KSPField(guiName = "#autoLOC_6001812")]
	public string status = string.Empty;

	[KSPField]
	public bool showStatus;

	public List<PartResourceDefinition> consumedResources;

	public List<PartResourceDefinition> GetConsumedResources()
	{
		return consumedResources;
	}

	public override void OnAwake()
	{
		if (consumedResources == null)
		{
			consumedResources = new List<PartResourceDefinition>();
		}
		else
		{
			consumedResources.Clear();
		}
		int i = 0;
		for (int count = resHandler.inputResources.Count; i < count; i++)
		{
			consumedResources.Add(PartResourceLibrary.Instance.GetDefinition(resHandler.inputResources[i].name));
		}
	}

	public override void OnSave(ConfigNode node)
	{
		base.OnSave(node);
		node.AddValue("canRelay", isRelaying);
	}

	public override void OnStart(StartState state)
	{
		base.Fields["status"].guiActive = showStatus;
	}

	public virtual void FixedUpdate()
	{
		if (!HighLogic.LoadedSceneIsFlight)
		{
			return;
		}
		if (resHandler.UpdateModuleResourceInputs(ref status, 1.0, 0.9, returnOnFirstLack: true))
		{
			isRelaying = true;
			if (status != "Nominal")
			{
				status = "Nominal";
			}
		}
		else
		{
			isRelaying = false;
		}
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_118968", resHandler.PrintModuleResources());
	}

	public bool CanRelay()
	{
		return isRelaying;
	}

	public bool CanRelayUnloaded(ProtoPartModuleSnapshot mSnap)
	{
		if (mSnap == null)
		{
			return true;
		}
		int count = mSnap.moduleValues.values.Count;
		do
		{
			if (count-- <= 0)
			{
				return true;
			}
		}
		while (!(mSnap.moduleValues.values[count].name == "canRelay"));
		return mSnap.moduleValues.values[count].value != "False";
	}
}

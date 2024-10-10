using System;
using ns9;

[Serializable]
public class ModuleResource : IConfigNode
{
	public string name;

	public string title;

	public int id;

	public double amount;

	public double rate;

	public double currentRequest;

	public double currentAmount;

	public bool varyTime = true;

	public ResourceFlowMode flowMode = ResourceFlowMode.NULL;

	public PartResourceDefinition _cachedResourceDef;

	public bool useSI;

	public double displayUnitMult = 1.0;

	public string unitName = "";

	public bool available;

	public bool isDeprived;

	public bool shutOffStartUpUsePercent;

	public bool shutOffHandler;

	public bool startUpHandler;

	public float shutOffAmount;

	public float shutOffPercent;

	public float startUpAmount;

	public float startUpPercent;

	public ModuleResourceAutoShiftState autoStateShifter;

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

	public virtual void OnAwake()
	{
		if (autoStateShifter == null)
		{
			autoStateShifter = new ModuleResourceAutoShiftState();
		}
	}

	public void Load(ConfigNode node)
	{
		name = node.GetValue("name");
		title = KSPUtil.PrintLocalizedModuleName(name);
		id = name.GetHashCode();
		if (node.HasValue("resourceFlowMode"))
		{
			flowMode = (ResourceFlowMode)Enum.Parse(typeof(ResourceFlowMode), node.GetValue("resourceFlowMode"));
		}
		else if (resourceDef != null)
		{
			flowMode = resourceDef.resourceFlowMode;
		}
		if (node.HasValue("rate"))
		{
			rate = double.Parse(node.GetValue("rate"));
		}
		if (node.HasValue("amount"))
		{
			amount = double.Parse(node.GetValue("amount"));
		}
		if (node.HasValue("varyTime"))
		{
			varyTime = bool.Parse(node.GetValue("varyTime"));
		}
		if (node.HasValue("useSI"))
		{
			useSI = bool.Parse(node.GetValue("useSI"));
		}
		if (node.HasValue("displayUnitMult"))
		{
			displayUnitMult = double.Parse(node.GetValue("displayUnitMult"));
		}
		if (node.HasValue("unitName"))
		{
			unitName = node.GetValue("unitName");
		}
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("name", name);
		node.AddValue("rate", rate);
		node.AddValue("amount", amount);
		if (flowMode != _cachedResourceDef.resourceFlowMode)
		{
			node.AddValue("resourceFlowMode", flowMode);
		}
	}

	public string PrintRate(double mult = 1.0)
	{
		return PrintRate(showFlowMode: false, mult);
	}

	public string PrintRate(bool showFlowMode, double mult = 1.0)
	{
		string text = "";
		double num = rate * mult;
		PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(name.GetHashCode());
		string text2 = "";
		text2 = ((definition == null) ? title : definition.displayName);
		text = (varyTime ? ((Math.Abs(num) > 0.1) ? (text + Localizer.Format("#autoLOC_244197", text2, num.ToString("0.0"))) : ((!(Math.Abs(num) > 1.0 / 600.0)) ? (text + Localizer.Format("#autoLOC_6002411", text2, (num * 3600.0).ToString("0.0"))) : (text + Localizer.Format("#autoLOC_244201", text2, (num * 60.0).ToString("0.0"))))) : ((!useSI) ? (text + Localizer.Format("#autoLOC_6002412", text2, (num * displayUnitMult).ToString("0.000"), unitName)) : (text + "- <b>" + text2 + ": </b>" + KSPUtil.PrintSI(num * displayUnitMult, unitName, 3, longPrefix: true) + "\n")));
		if (showFlowMode && definition != null)
		{
			text += definition.GetFlowModeDescription();
		}
		return text;
	}

	public bool IsDeprived(float requestThreshold = 0.9f)
	{
		return currentAmount < currentRequest * (double)requestThreshold;
	}
}

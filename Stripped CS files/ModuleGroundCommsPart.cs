using System.Collections.Generic;
using System.Globalization;
using CommNet;
using ns9;

public class ModuleGroundCommsPart : ModuleGroundSciencePart, ICommAntenna
{
	[KSPField]
	public bool antennaCombinable;

	[KSPField]
	public double antennaCombinableExponent = 0.75;

	[KSPField]
	public AntennaType antennaType = AntennaType.DIRECT;

	[KSPField]
	public DoubleCurve rangeCurve;

	[KSPField]
	public DoubleCurve scienceCurve;

	[KSPField]
	public double antennaPower = 500000.0;

	public ModuleGroundExpControl controllerModule;

	public Part loadedPart;

	public List<string> tempAppliedUpgrades = new List<string>();

	public bool CommCombinable => antennaCombinable;

	public double CommCombinableExponent => antennaCombinableExponent;

	public AntennaType CommType => antennaType;

	public DoubleCurve CommRangeCurve => rangeCurve;

	public DoubleCurve CommScienceCurve => scienceCurve;

	public virtual double CommPower => antennaPower;

	public bool CanScienceTo(bool combined, double bPower, double sqrDistance)
	{
		return false;
	}

	public bool CanComm()
	{
		if (base.ScienceClusterData != null)
		{
			if (controllerModule == null)
			{
				if (loadedPart == null)
				{
					FlightGlobals.FindLoadedPart(base.ScienceClusterData.ControlModulePartId, out loadedPart);
				}
				if (loadedPart != null)
				{
					controllerModule = loadedPart.FindModuleImplementing<ModuleGroundExpControl>();
				}
			}
			if (base.ScienceClusterData.ControlModulePartId != base.part.persistentId && loadedPart != null && controllerModule != null)
			{
				if (moduleIsEnabled && deployedOnGround && base.ScienceClusterData.IsPowered && base.Enabled)
				{
					return controllerModule.Enabled;
				}
				return false;
			}
			if (moduleIsEnabled && deployedOnGround && base.ScienceClusterData.IsPowered)
			{
				return base.Enabled;
			}
			return false;
		}
		return false;
	}

	public bool CanCommUnloaded(ProtoPartModuleSnapshot mSnap)
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
		while (!(mSnap.moduleValues.values[count].name == "canComm"));
		return mSnap.moduleValues.values[count].value != "False";
	}

	public virtual double CommPowerUnloaded(ProtoPartModuleSnapshot mSnap)
	{
		double value = antennaPower;
		ConfigNode node;
		if (mSnap != null && (node = mSnap.moduleValues.GetNode("UPGRADESAPPLIED")) != null)
		{
			LoadUpgradesApplied(tempAppliedUpgrades, node);
			ConfigNode configNode = new ConfigNode();
			ApplyUpgradeNode(tempAppliedUpgrades, configNode, doLoad: false);
			configNode.TryGetValue("antennaPower", ref value);
		}
		return value;
	}

	public override string GetInfo()
	{
		string text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(antennaType.displayDescription());
		string info = base.GetInfo();
		info = info + "<color=" + XKCDColors.HexFormat.Cyan + ">" + Localizer.Format("#autoLOC_8002327") + "</color>\n";
		string unitName = Localizer.Format("#autoLOC_7001411");
		info += Localizer.Format("#autoLOC_7001005", text);
		string text2 = KSPUtil.PrintSI(CommPower, string.Empty) + (antennaCombinable ? (" " + Localizer.Format("#autoLOC_236248")) : string.Empty);
		info += Localizer.Format("#autoLOC_7001006", text2);
		info = info + Localizer.Format("#autoLOC_236834") + " " + KSPUtil.PrintSI(CommNetScenario.RangeModel.GetMaximumRange(antennaPower, GameVariables.Instance.GetDSNRange(0f)), unitName);
		info = info + Localizer.Format("#autoLOC_236835") + " " + KSPUtil.PrintSI(CommNetScenario.RangeModel.GetMaximumRange(antennaPower, GameVariables.Instance.GetDSNRange(0.5f)), unitName);
		info = info + Localizer.Format("#autoLOC_236836") + " " + KSPUtil.PrintSI(CommNetScenario.RangeModel.GetMaximumRange(antennaPower, GameVariables.Instance.GetDSNRange(1f)), unitName);
		info += "\n";
		if (!moduleIsEnabled)
		{
			info += Localizer.Format("#autoLOC_236849");
		}
		return info;
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLOC_6012035");
	}
}

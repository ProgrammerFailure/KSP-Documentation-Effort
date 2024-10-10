using ns9;
using UnityEngine;

namespace CompoundParts;

public class CModuleFuelLine : CompoundPartModule, IModuleInfo
{
	public static string cacheAutoLOC_216750;

	public static string cacheAutoLOC_216757;

	public override void OnTargetSet(Part target)
	{
		target.fuelLookupTargets.Add(base.part);
		GameEvents.onPartFuelLookupStateChange.Fire(new GameEvents.HostedFromToAction<bool, Part>(host: false, target, base.part));
		base.part.stackIcon.SetIconColor(XKCDColors.BrightTeal);
	}

	public override void OnTargetLost()
	{
		CloseFuelLine();
	}

	public void CloseFuelLine()
	{
		if (base.target != null)
		{
			base.target.fuelLookupTargets.Remove(base.part);
			GameEvents.onPartFuelLookupStateChange.Fire(new GameEvents.HostedFromToAction<bool, Part>(host: false, base.target, base.part));
		}
		base.part.stackIcon.SetIconColor(XKCDColors.SlateGrey);
	}

	public override void OnStartFinished(StartState state)
	{
		base.OnStartFinished(state);
		if (HighLogic.LoadedSceneIsFlight && EVAConstructionModeController.Instance.IsOpen)
		{
			base.compoundPart.attachState = CompoundPart.AttachState.Attaching;
		}
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_216749", base.compoundPart.maxLength.ToString("0.0")) + cacheAutoLOC_216750;
	}

	public string GetModuleTitle()
	{
		return "Fuel Line";
	}

	public Callback<Rect> GetDrawModulePanelCallback()
	{
		return null;
	}

	public string GetPrimaryField()
	{
		return "";
	}

	public override string GetModuleDisplayName()
	{
		return cacheAutoLOC_216757;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_216750 = Localizer.Format("#autoLOC_216750");
		cacheAutoLOC_216757 = Localizer.Format("#autoLOC_216757");
	}
}

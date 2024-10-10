using ns37;
using ns9;
using UnityEngine;

public class ExperimentTransfer : PartItemTransfer
{
	public IScienceDataContainer container;

	public static ExperimentTransfer Create(Part srcPart, IScienceDataContainer cont, Callback<DismissAction, Part> onDialogDismiss)
	{
		if (ExperimentsResultDialog.Instance != null && ExperimentsResultDialog.Instance.pages.Count > 0)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_133305"), 3f, ScreenMessageStyle.UPPER_CENTER);
			return null;
		}
		ExperimentTransfer experimentTransfer = new GameObject("Experiment Transfer Host").AddComponent<ExperimentTransfer>();
		experimentTransfer.container = cont;
		experimentTransfer.Setup(srcPart, "#autoLOC_6002371", "#autoLOC_6002370", string.Empty, onDialogDismiss);
		return experimentTransfer;
	}

	public override bool IsSemiValidPart(Part p)
	{
		return false;
	}

	public override bool IsValidPart(Part p)
	{
		ModuleScienceContainer moduleScienceContainer = p.FindModuleImplementing<ModuleScienceContainer>();
		if (moduleScienceContainer != null)
		{
			return moduleScienceContainer.canBeTransferredToInVessel;
		}
		return false;
	}

	public override void OnPartSelect(Part p)
	{
		Dismiss(DismissAction.ItemMoved, p);
	}

	public override void OnSrcPartSelect(Part p)
	{
		OnPartSelect(p);
	}
}

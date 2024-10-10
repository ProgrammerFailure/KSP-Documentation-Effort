using ns9;

public class ModuleFuelJettison : PartModule
{
	[KSPField]
	public string ResourceName;

	[KSPEvent(unfocusedRange = 5f, active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001475")]
	public void FuelJettison()
	{
		if (ResourceName == string.Empty)
		{
			int count = base.part.Resources.Count;
			for (int i = 0; i < count; i++)
			{
				Dump(base.part.Resources[i]);
			}
		}
		else
		{
			PartResource partResource = base.part.Resources[ResourceName];
			if (partResource != null)
			{
				Dump(partResource);
			}
		}
	}

	public void Dump(PartResource res)
	{
		ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6001051", res.amount.ToString("0.00"), res.info.displayName), 5f, ScreenMessageStyle.UPPER_CENTER);
		base.part.TransferResource(res, double.MinValue, base.part);
	}

	[KSPAction("#autoLOC_6001475", activeEditor = false)]
	public void FuelJettisonAction(KSPActionParam param)
	{
		FuelJettison();
	}

	public override void OnStart(StartState state)
	{
		if (ResourceName != string.Empty)
		{
			base.Actions["FuelJettisonAction"].guiName = Localizer.Format("#autoLOC_6001888", ResourceName);
			base.Events["FuelJettison"].guiName = Localizer.Format("#autoLOC_6001888", ResourceName);
		}
	}
}

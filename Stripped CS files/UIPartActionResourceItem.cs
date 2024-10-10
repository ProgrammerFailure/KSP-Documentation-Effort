public class UIPartActionResourceItem : UIPartActionItem
{
	public PartResource resource;

	public PartResource Resource => resource;

	public virtual void Setup(UIPartActionWindow window, Part part, UI_Scene scene, UI_Control control, PartResource resource)
	{
		SetupItem(window, part, null, scene, control);
		this.resource = resource;
	}

	public void SetSymCounterpartsAmount(double amount)
	{
		if (part != null)
		{
			int count = part.symmetryCounterparts.Count;
			for (int i = 0; i < count; i++)
			{
				part.symmetryCounterparts[i].Resources[resource.info.name].amount = amount;
			}
		}
	}

	public void SetSymCounterpartsFlowState(bool state)
	{
		if (part != null)
		{
			int count = part.symmetryCounterparts.Count;
			for (int i = 0; i < count; i++)
			{
				part.symmetryCounterparts[i].Resources[resource.info.name].flowState = state;
			}
		}
	}
}

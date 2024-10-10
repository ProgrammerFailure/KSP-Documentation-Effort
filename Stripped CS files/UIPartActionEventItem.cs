public class UIPartActionEventItem : UIPartActionItem
{
	public BaseEvent evt;

	public bool isExternal;

	public bool ExternalToEVAOnly;

	public float itemRange;

	public BaseEvent Evt => evt;

	public virtual void Setup(UIPartActionWindow window, Part part, PartModule partModule, UI_Scene scene, UI_Control control, BaseEvent partEvent)
	{
		SetupItem(window, part, partModule, scene, control);
		evt = partEvent;
		isExternal = partEvent.guiActiveUnfocused;
		itemRange = partEvent.unfocusedRange;
		ExternalToEVAOnly = partEvent.externalToEVAOnly;
	}

	public override bool IsItemValid()
	{
		if (scene == UI_Scene.Flight)
		{
			if (part == null || part.State == PartStates.DEAD || evt == null || !evt.active || evt.EventIsDisabledByVariant || (!GameSettings.ADVANCED_TWEAKABLES && evt.advancedTweakable) || (isModule && (partModule == null || !partModule.isEnabled)) || ((FlightGlobals.ActiveVessel == part.vessel) ? (!evt.guiActive) : (!evt.guiActiveUnfocused)) || !CheckInRange())
			{
				return false;
			}
		}
		else if (part == null || part.State == PartStates.DEAD || evt == null || !evt.active || !evt.guiActiveEditor || evt.EventIsDisabledByVariant || (!GameSettings.ADVANCED_TWEAKABLES && evt.advancedTweakable) || (isModule && (partModule == null || !partModule.isEnabled)))
		{
			return false;
		}
		return true;
	}

	public bool CheckInRange()
	{
		if (!isExternal)
		{
			return FlightGlobals.ActiveVessel == part.vessel;
		}
		if (FlightGlobals.ActiveVessel != null)
		{
			if (!(FlightGlobals.ActiveVessel == part.vessel))
			{
				if (!ExternalToEVAOnly || FlightGlobals.ActiveVessel.isEVA)
				{
					return (FlightGlobals.ActiveVessel.transform.position - part.partTransform.position).sqrMagnitude < itemRange * itemRange;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	public void FireSymCounterparts()
	{
		if (isModule)
		{
			if (!(base.part != null) || !(base.partModule != null))
			{
				return;
			}
			int num = base.part.Modules.IndexOf(base.partModule);
			int count = base.part.symmetryCounterparts.Count;
			for (int i = 0; i < count; i++)
			{
				Part part = base.part.symmetryCounterparts[i];
				PartModule partModule;
				if (num < part.Modules.Count && part.Modules[num] != null && base.partModule.GetType() == part.Modules[num].GetType())
				{
					partModule = part.Modules[num];
					partModule.Events.Send(evt.id);
					continue;
				}
				partModule = part.Modules[base.partModule.ClassName];
				if (partModule != null)
				{
					partModule.Events.Send(evt.id);
				}
			}
		}
		else if (base.part != null)
		{
			int count2 = base.part.symmetryCounterparts.Count;
			for (int j = 0; j < count2; j++)
			{
				base.part.symmetryCounterparts[j].Events.Send(evt.id);
			}
		}
	}
}

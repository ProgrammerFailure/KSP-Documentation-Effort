public class UIPartActionFieldItem : UIPartActionItem
{
	public BaseField field;

	public bool isActiveUnfocused;

	public float itemRange;

	public bool removedIfPinned;

	public BaseField Field => field;

	public object Host
	{
		get
		{
			if (isModule)
			{
				return partModule;
			}
			return part;
		}
	}

	public virtual void Setup(UIPartActionWindow window, Part part, PartModule partModule, UI_Scene scene, UI_Control control, BaseField field)
	{
		SetupItem(window, part, partModule, scene, control);
		this.field = field;
		itemRange = field.guiUnfocusedRange;
		isActiveUnfocused = field.guiActiveUnfocused;
	}

	public override bool IsItemValid()
	{
		if (isModule)
		{
			if (part == null || part.State == PartStates.DEAD || partModule == null || !partModule.isEnabled || field == null || (!GameSettings.ADVANCED_TWEAKABLES && field.advancedTweakable) || (scene == UI_Scene.Flight && (!field.guiActive || (field.guiActive && !CheckInRange()))) || (scene == UI_Scene.Editor && !field.guiActiveEditor))
			{
				return false;
			}
		}
		else if (part == null || part.State == PartStates.DEAD || field == null || (!GameSettings.ADVANCED_TWEAKABLES && field.advancedTweakable) || (scene == UI_Scene.Flight && (!field.guiActive || (field.guiActive && !CheckInRange()))) || (scene == UI_Scene.Editor && !field.guiActiveEditor))
		{
			return false;
		}
		return true;
	}

	public BaseField GetField(string name)
	{
		if (isModule)
		{
			return partModule.Fields[name];
		}
		return part.Fields[name];
	}

	public bool SetSymCounterpartValue(object value)
	{
		bool flag = false;
		if (isModule)
		{
			if (base.part != null && base.partModule != null)
			{
				int num = base.part.Modules.IndexOf(base.partModule);
				int count = base.part.symmetryCounterparts.Count;
				for (int i = 0; i < count; i++)
				{
					Part part = base.part.symmetryCounterparts[i];
					PartModule partModule;
					if (num < part.Modules.Count && part.Modules[num] != null && base.partModule.GetType() == part.Modules[num].GetType())
					{
						partModule = part.Modules[num];
						flag = flag || !object.Equals(partModule.Fields.GetValue(field.name), value);
						partModule.Fields.SetValue(field.name, value);
						FireSymmetryEvents(partModule.Fields[field.name], value);
						continue;
					}
					partModule = part.Modules[base.partModule.ClassName];
					if (partModule != null)
					{
						flag = flag || !object.Equals(partModule.Fields.GetValue(field.name), value);
						partModule.Fields.SetValue(field.name, value);
						FireSymmetryEvents(partModule.Fields[field.name], value);
					}
				}
			}
		}
		else if (base.part != null)
		{
			int count2 = base.part.symmetryCounterparts.Count;
			for (int j = 0; j < count2; j++)
			{
				Part part2 = base.part.symmetryCounterparts[j];
				flag = flag || !object.Equals(part2.Fields.GetValue(field.name), value);
				part2.Fields.SetValue(field.name, value);
			}
		}
		return flag;
	}

	public void FireSymmetryEvents(BaseField baseField, object value)
	{
		if (baseField != null)
		{
			if (baseField.uiControlEditor != null && baseField.uiControlEditor.onSymmetryFieldChanged != null)
			{
				baseField.uiControlEditor.onSymmetryFieldChanged(field, value);
			}
			if (baseField.uiControlFlight != null && baseField.uiControlFlight.onSymmetryFieldChanged != null)
			{
				baseField.uiControlFlight.onSymmetryFieldChanged(field, value);
			}
		}
	}

	public void SetFieldValue(object newValue)
	{
		object value = field.GetValue(field.host);
		bool flag = !object.Equals(value, newValue);
		if ((control.affectSymCounterparts & scene) != 0)
		{
			flag = SetSymCounterpartValue(newValue) || flag;
		}
		if (flag)
		{
			field.SetValue(newValue, field.host);
			if (control.onFieldChanged != null)
			{
				control.onFieldChanged(field, value);
			}
			if (!control.suppressEditorShipModified && HighLogic.LoadedSceneIsEditor)
			{
				GameEvents.onEditorShipModified.Fire(EditorLogic.fetch.ship);
			}
		}
	}

	public bool CheckInRange()
	{
		if (!isActiveUnfocused)
		{
			return FlightGlobals.ActiveVessel == part.vessel;
		}
		if (FlightGlobals.ActiveVessel != null)
		{
			if (!(FlightGlobals.ActiveVessel == part.vessel))
			{
				return (FlightGlobals.ActiveVessel.transform.position - part.partTransform.position).sqrMagnitude < itemRange * itemRange;
			}
			return true;
		}
		return false;
	}
}

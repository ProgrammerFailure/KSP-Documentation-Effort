using System;

namespace ns1;

public abstract class MapContextMenuOption : DialogGUIButton
{
	public MapContextMenuOption(string caption)
		: base(caption, delegate
		{
		}, dismissOnSelect: true)
	{
		onOptionSelected = OnSelect;
		OptionInteractableCondition = (Func<bool>)Delegate.Combine(OptionInteractableCondition, new Func<bool>(CheckEnabled));
		OptionEnabledCondition = (Func<bool>)Delegate.Combine(OptionEnabledCondition, new Func<bool>(CheckAvailable));
	}

	public abstract void OnSelect();

	public bool CheckEnabled()
	{
		string fbText = string.Copy(OptionText);
		bool result = OnCheckEnabled(out fbText);
		if (fbText != OptionText)
		{
			SetOptionText(fbText);
		}
		return result;
	}

	public virtual bool OnCheckEnabled(out string fbText)
	{
		fbText = OptionText;
		return true;
	}

	public virtual bool CheckAvailable()
	{
		return true;
	}
}

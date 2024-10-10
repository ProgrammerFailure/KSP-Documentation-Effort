using UnityEngine;

[RequireComponent(typeof(UIPartActionFieldItem))]
public class UIMarquee_PAW : UIMarquee
{
	public UIPartActionFieldItem fieldItem;

	public override void Start()
	{
		fieldItem = GetComponent<UIPartActionFieldItem>();
		if (fieldItem != null && fieldItem.Field != null)
		{
			fieldItem.Field.OnValueModified += Field_OnValueModified;
		}
		GameEvents.onPartActionUIShown.Add(OnPartActionUIShown);
		GameEvents.onPartActionUIDismiss.Add(OnPartActionUIDismiss);
		base.Start();
	}

	public override void OnDestroy()
	{
		if (fieldItem != null)
		{
			fieldItem.Field.OnValueModified -= Field_OnValueModified;
		}
		GameEvents.onPartActionUIShown.Remove(OnPartActionUIShown);
		GameEvents.onPartActionUIDismiss.Remove(OnPartActionUIDismiss);
		base.OnDestroy();
	}

	public void OnPartActionUIShown(UIPartActionWindow window, Part inpPart)
	{
		if (inpPart.persistentId == fieldItem.Part.persistentId)
		{
			Configure();
			AutoStartMarquee();
		}
	}

	public void OnPartActionUIDismiss(Part inpPart)
	{
		if (inpPart.persistentId == fieldItem.Part.persistentId)
		{
			StopMarquee();
		}
	}
}

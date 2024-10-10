using ns2;
using UnityEngine;

[RequireComponent(typeof(UISelectableGridLayoutGroupItem))]
public class UIMarquee_ActionField : UIMarquee
{
	public UISelectableGridLayoutGroupItem fieldItem;

	public override void Start()
	{
		fieldItem = GetComponent<UISelectableGridLayoutGroupItem>();
		GameEvents.onGUIActionGroupClosed.Add(OnPartActionUIDismiss);
		base.Start();
	}

	public override void OnDestroy()
	{
		GameEvents.onGUIActionGroupClosed.Remove(OnPartActionUIDismiss);
		base.OnDestroy();
	}

	public void OnPartActionUIDismiss()
	{
		StopMarquee();
	}
}
